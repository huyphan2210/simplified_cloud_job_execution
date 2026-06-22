using simplified_cloud_job_execution_backend.Domain.Jobs;

namespace simplified_cloud_job_execution_backend.Dtos.Jobs;

public class CreateJobRequest
{
  public required string JobName { get; set; }
  public required Guid ProjectId { get; set; }
  public ComputeType ComputeType { get; set; }
  public IFormFile? InputFile { get; set; }
}