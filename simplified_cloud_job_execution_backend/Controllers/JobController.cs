using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using simplified_cloud_job_execution_backend.Dtos.Exceptions;
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
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetJobStatusResponse))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
  public async Task<ActionResult<GetJobStatusResponse>> GetJobStatus(Guid jobId)
  {
    var response = await _jobServices.GetJobStatusAsync(jobId);
    return Ok(response);
  }

  [HttpPost]
  [Consumes("multipart/form-data")]
  [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateJobResponse))]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
  public async Task<ActionResult<CreateJobResponse>> CreateJob([FromForm] CreateJobRequest request)
  {
    var response = await _jobServices.CreateJobAsync(request);

    return CreatedAtAction(nameof(GetJobStatus), new { jobId = response.JobId }, response);
  }

  [HttpPost("{jobId}:complete")]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompleteJobResponse))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
  public async Task<ActionResult<CompleteJobResponse>> CompleteJob(Guid jobId)
  {
    var response = await _jobServices.CompleteJobAsync(jobId);
    return Ok(response);
  }

  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedJobsResponse))]
  [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
  public async Task<ActionResult<PagedJobsResponse>> GetAllJobs([FromQuery] int page = 1, [FromQuery] int chunk = 10)
  {
    var response = await _jobServices.GetJobsAsync(page, chunk);
    return Ok(response);
  }

  [HttpGet("{jobId}/billing")]
  [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BillingSummaryResponse))]
  [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
  [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
  public async Task<ActionResult<BillingSummaryResponse>> GetBillingSummary(Guid jobId)
  {
    var response = await _jobServices.GetBillingSummaryAsync(jobId);
    return Ok(response);
  }
}
