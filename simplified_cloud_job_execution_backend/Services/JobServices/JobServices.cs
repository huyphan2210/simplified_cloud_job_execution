using System.Linq;
using Microsoft.EntityFrameworkCore;
using simplified_cloud_job_execution_backend.Exceptions;
using simplified_cloud_job_execution_backend.Domain.Jobs;
using simplified_cloud_job_execution_backend.Dtos.Jobs;
using simplified_cloud_job_execution_backend.Infrastructure.AWS.S3;
using simplified_cloud_job_execution_backend.Infrastructure.AWS.Sqs;
using simplified_cloud_job_execution_backend.Infrastructure.Execution;
using simplified_cloud_job_execution_backend.Repositories.JobRepository;

namespace simplified_cloud_job_execution_backend.Services.JobServices;

public class JobServices(IJobRepository jobRepository, IJobQueue sqsJobQueue, IS3FileService s3FileService, IJobExecutor jobExecutor, ILogger<JobServices> logger) : IJobServices
{
  private readonly IJobRepository _jobRepository = jobRepository;
  private readonly IJobQueue _sqsJobQueue = sqsJobQueue;
  private readonly IS3FileService _s3FileService = s3FileService;
  private readonly IJobExecutor _jobExecutor = jobExecutor;
  private readonly ILogger<JobServices> _logger = logger;

  public async Task<GetJobStatusResponse> GetJobStatusAsync(Guid jobId, CancellationToken cancellationToken = default)
  {
    var job = await _jobRepository.GetJobByIdAsync(jobId, cancellationToken) ?? throw new NotFoundException("Job not found");
    return job.ToGetJobStatusResponse();
  }

  public async Task<PagedJobsResponse> GetJobsAsync(int page = 1, int chunk = 10, CancellationToken cancellationToken = default)
  {
    var (jobs, totalCount, currentPage, currentChunk) = await _jobRepository.GetPagedJobsAsync(page, chunk, cancellationToken);
    var totalPage = (int)Math.Ceiling(totalCount / (double)currentChunk);

    return new PagedJobsResponse
    {
      Jobs = [.. jobs.Select(job => job.ToGetJobStatusResponse())],
      CurrentPage = currentPage,
      TotalPage = totalPage,
      PageSize = currentChunk
    };
  }

  public async Task<CreateJobResponse> CreateJobAsync(CreateJobRequest request, CancellationToken cancellationToken = default)
  {
    string? inputFileReference = null;
    var jobId = Guid.NewGuid();

    // Upload input file if provided
    if (request.InputFile != null)
    {
      inputFileReference = await _s3FileService.UploadFileAsync(request.InputFile, jobId, cancellationToken);
      _logger.LogInformation("File uploaded for job {JobId}: {FileReference}", jobId, inputFileReference);
    }

    var job = await _jobRepository.CreateJobAsync(new Job
    {
      Id = jobId,
      JobName = request.JobName,
      ProjectId = request.ProjectId,
      ComputeType = request.ComputeType,
      InputFileName = request.InputFile?.FileName ?? "not-provided",
      InputFileReference = inputFileReference,
      Status = JobStatus.Queued,
      CreatedAt = DateTime.UtcNow,
    }, cancellationToken);

    await _sqsJobQueue.EnqueueAsync(job.Id);

    return new CreateJobResponse
    {
      JobId = job.Id,
      Status = JobStatus.Queued.ToString()
    };
  }

  public async Task<CompleteJobResponse> CompleteJobAsync(Guid jobId, CancellationToken cancellationToken = default)
  {
    var job = await _jobRepository.GetJobByIdAsync(jobId, cancellationToken) ?? throw new NotFoundException("Job not found");

    if (job.Status != JobStatus.Queued)
    {
      return new CompleteJobResponse
      {
        JobId = job.Id,
        Status = job.Status.ToString(),
        CreditCost = job.CreditCost
      };
    }

    await _jobExecutor.ExecuteAsync(jobId, cancellationToken);

    var completedJob = await _jobRepository.GetJobByIdAsync(jobId, cancellationToken) ?? throw new NotFoundException("Job not found after execution");

    return new CompleteJobResponse
    {
      JobId = completedJob.Id,
      Status = completedJob.Status.ToString(),
      CreditCost = completedJob.CreditCost
    };
  }

  public async Task<BillingSummaryResponse> GetBillingSummaryAsync(Guid jobId, CancellationToken cancellationToken = default)
  {
    var job = await _jobRepository.GetJobByIdAsync(jobId, cancellationToken) ?? throw new NotFoundException("Job not found");
    return job.ToBillingSummaryResponse();
  }
}
