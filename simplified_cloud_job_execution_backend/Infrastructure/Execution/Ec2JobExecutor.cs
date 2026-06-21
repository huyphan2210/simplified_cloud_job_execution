using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using simplified_cloud_job_execution_backend.Domain.Jobs;
using simplified_cloud_job_execution_backend.Infrastructure.AWS.S3;
using simplified_cloud_job_execution_backend.Infrastructure.AWS.Ssm;
using simplified_cloud_job_execution_backend.Repositories.JobRepository;
using simplified_cloud_job_execution_backend.Services.BillingServices;

namespace simplified_cloud_job_execution_backend.Infrastructure.Execution;

public class Ec2JobExecutor(
    IJobRepository jobRepository,
    IBillingService billingService,
    ISsmCommandService ssmCommandService,
    IOptions<SsmOptions> ssmOptions,
    IS3FileService s3FileService,
    ILogger<Ec2JobExecutor> logger) : IJobExecutor
{
  private readonly IJobRepository _jobRepository = jobRepository;
  private readonly IBillingService _billingService = billingService;
  private readonly ISsmCommandService _ssmCommandService = ssmCommandService;
  private readonly SsmOptions _ssmOptions = ssmOptions.Value;
  private readonly IS3FileService _s3FileService = s3FileService;
  private readonly ILogger<Ec2JobExecutor> _logger = logger;

  public async Task ExecuteAsync(Guid jobId, CancellationToken cancellationToken)
  {
    var job = await _jobRepository.GetJobByIdAsync(jobId, cancellationToken);
    if (job == null || job.Status != JobStatus.Queued || job.BillingProcessed)
    {
      return;
    }

    var start = DateTime.UtcNow;
    job.Status = JobStatus.Running;
    await _jobRepository.UpdateJobAsync(job, cancellationToken);

    try
    {
      // Build the payload to be executed on EC2
      var payloadObj = new
      {
        jobId = job.Id,
        jobName = job.JobName,
        projectId = job.ProjectId,
        computeType = job.ComputeType.ToString(),
        inputFileReference = job.InputFileReference,
        timestamp = DateTime.UtcNow.ToString("O")
      };

      var json = JsonSerializer.Serialize(payloadObj);
      var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
      var commandText = $"echo {b64} | base64 -d > /tmp/job-{job.Id}-result.json\ncat /tmp/job-{job.Id}-result.json";

      // Execute command on EC2 via SSM
      var outputContent = await _ssmCommandService.ExecuteCommandAsync(_ssmOptions.InstanceId, commandText, cancellationToken);

      if (string.IsNullOrEmpty(outputContent))
      {
        _logger.LogError("No output from SSM command for job {JobId}", jobId);
        job.Status = JobStatus.Failed;
        await _jobRepository.UpdateJobAsync(job, cancellationToken);
        return;
      }

      // Upload output to S3
      var duration = (DateTime.UtcNow - start).TotalSeconds;
      var outputFileName = $"job-{job.Id}-result.json";
      var outputFileKey = await _s3FileService.UploadOutputAsync(outputContent, job.Id, outputFileName, cancellationToken);

      // Update job with results
      job.ExecutionDurationSeconds = duration;
      job.OutputFileReference = outputFileKey;
      job.CreditCost = _billingService.CalculateCost(job.ComputeType, duration);
      job.BillingProcessed = true;
      job.Status = JobStatus.Completed;

      _logger.LogInformation("Job {JobId} completed successfully. Duration: {Duration}s, Cost: {Cost} credits", 
        jobId, duration, job.CreditCost);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "EC2 execution failed for job {JobId}", jobId);
      job.Status = JobStatus.Failed;
      await _jobRepository.UpdateJobAsync(job, cancellationToken);
      return;
    }

    await _jobRepository.UpdateJobAsync(job, cancellationToken);
  }
}