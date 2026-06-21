using Microsoft.AspNetCore.Mvc;
using simplified_cloud_job_execution_backend.Domain.Jobs;
using simplified_cloud_job_execution_backend.Dtos.Jobs;
using simplified_cloud_job_execution_backend.Services.JobServices;

namespace simplified_cloud_job_execution_backend.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobController(IJobServices jobServices, ILogger<JobController> logger) : ControllerBase
{
  private readonly ILogger<JobController> _logger = logger;
  private readonly IJobServices _jobServices = jobServices;

  [HttpGet("{jobId}")]
  public async Task<ActionResult<GetJobStatusResponse>> GetJobStatus(Guid jobId)
  {
    var response = await _jobServices.GetJobStatusAsync(jobId);
    return Ok(response);
  }

  [HttpPost]
  [Consumes("multipart/form-data")]
  public async Task<ActionResult<CreateJobResponse>> CreateJob(
    [FromForm] string jobName,
    [FromForm] string projectId,
    [FromForm] ComputeType computeType,
    [FromForm] IFormFile? inputFile)
  {
    var request = new CreateJobRequest
    {
      JobName = jobName,
      ProjectId = projectId,
      ComputeType = computeType,
      InputFile = inputFile
    };

    var response = await _jobServices.CreateJobAsync(request);

    return CreatedAtAction(nameof(GetJobStatus), new { jobId = response.JobId }, response);
  }

  [HttpPost("{jobId}:complete")]
  public async Task<ActionResult<CompleteJobResponse>> CompleteJob(Guid jobId)
  {
    var response = await _jobServices.CompleteJobAsync(jobId);
    return Ok(response);
  }

  [HttpGet("{jobId}/billing")]
  public async Task<ActionResult<BillingSummaryResponse>> GetBillingSummary(Guid jobId)
  {
    var response = await _jobServices.GetBillingSummaryAsync(jobId);
    return Ok(response);
  }
}
