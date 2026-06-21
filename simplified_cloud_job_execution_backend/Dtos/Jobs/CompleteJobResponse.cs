namespace simplified_cloud_job_execution_backend.Dtos.Jobs;

public class CompleteJobResponse
{
  public Guid JobId { get; set; }
  public string Status { get; set; } = string.Empty;
  public decimal? CreditCost { get; set; }
}
