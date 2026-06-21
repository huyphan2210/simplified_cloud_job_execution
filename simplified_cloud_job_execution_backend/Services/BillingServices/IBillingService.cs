using simplified_cloud_job_execution_backend.Domain.Jobs;

namespace simplified_cloud_job_execution_backend.Services.BillingServices;

public interface IBillingService
{
  decimal CalculateCost(ComputeType computeType, double durationSeconds);
}