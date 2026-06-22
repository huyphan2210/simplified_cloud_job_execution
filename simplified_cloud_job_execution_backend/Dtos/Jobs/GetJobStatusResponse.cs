using simplified_cloud_job_execution_backend.Domain.Jobs;
using simplified_cloud_job_execution_backend.Domain.Projects;

namespace simplified_cloud_job_execution_backend.Dtos.Jobs;

public class GetJobStatusResponse
{
  public Guid JobId { get; set; }
  public string JobName { get; set; } = string.Empty;
  public Guid ProjectId { get; set; }
  public Project? Project { get; set; }
  public ComputeType ComputeType { get; set; }
  public JobStatus Status { get; set; }
  public string InputFileName { get; set; } = string.Empty;
  public string? InputFileReference { get; set; }
  public string? OutputFileReference { get; set; }
  public double? ExecutionDurationSeconds { get; set; }
  public decimal? CreditCost { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}