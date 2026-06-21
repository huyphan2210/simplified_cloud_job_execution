using simplified_cloud_job_execution_backend.Domain.Jobs;

namespace simplified_cloud_job_execution_backend.Services.BillingServices;

public class BillingService : IBillingService
{
  public decimal CalculateCost(ComputeType computeType, double durationSeconds)
  {
    var minutes = Math.Ceiling(durationSeconds / 60);

    var rate = computeType switch
    {
      ComputeType.CpuSmall => 1,
      ComputeType.CpuLarge => 3,
      ComputeType.Gpu => 8,
      _ => 1
    };

    return (decimal)minutes * rate;
  }
}