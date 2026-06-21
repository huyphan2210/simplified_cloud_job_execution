using simplified_cloud_job_execution_backend.Domain.Jobs;

namespace simplified_cloud_job_execution_backend.Dtos.Jobs;

public class BillingSummaryResponse
{
  public Guid JobId { get; set; }
  public string JobName { get; set; } = string.Empty;
  public string ProjectId { get; set; } = string.Empty;
  public ComputeType ComputeType { get; set; }
  public JobStatus Status { get; set; }
  public double? ExecutionDurationSeconds { get; set; }
  public decimal? CreditCost { get; set; }
  public string? InputFileReference { get; set; }
  public string? OutputFileReference { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
