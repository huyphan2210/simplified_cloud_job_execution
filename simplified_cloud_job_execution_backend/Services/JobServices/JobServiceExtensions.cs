using simplified_cloud_job_execution_backend.Dtos.Jobs;
using simplified_cloud_job_execution_backend.Domain.Jobs;

namespace simplified_cloud_job_execution_backend.Services.JobServices;

public static class JobServiceExtensions
{
  public static BillingSummaryResponse ToBillingSummaryResponse(this Job job)
  {
    return new BillingSummaryResponse
    {
      JobId = job.Id,
      JobName = job.JobName,
      ProjectId = job.ProjectId,
      ComputeType = job.ComputeType,
      Status = job.Status,
      ExecutionDurationSeconds = job.ExecutionDurationSeconds,
      CreditCost = job.CreditCost,
      InputFileReference = job.InputFileReference,
      OutputFileReference = job.OutputFileReference,
      CreatedAt = job.CreatedAt,
      UpdatedAt = job.UpdatedAt
    };
  }
}
