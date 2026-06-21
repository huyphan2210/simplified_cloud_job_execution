namespace simplified_cloud_job_execution_backend.Domain.Jobs;

public class Job
{
  public Guid Id { get; set; }
  public required string JobName { get; set; }
  public required string ProjectId { get; set; }
  public ComputeType ComputeType { get; set; }
  public JobStatus Status { get; set; }
  public required string InputFileName { get; set; }
  public string? InputFileReference { get; set; }
  public string? OutputFileReference { get; set; }
  public double? ExecutionDurationSeconds { get; set; }
  public decimal? CreditCost { get; set; }
  public bool BillingProcessed { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public bool IsDeleted { get; set; }
}