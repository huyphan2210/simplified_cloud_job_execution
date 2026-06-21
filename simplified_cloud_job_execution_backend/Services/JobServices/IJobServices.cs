using simplified_cloud_job_execution_backend.Dtos.Jobs;

namespace simplified_cloud_job_execution_backend.Services.JobServices;

public interface IJobServices
{
  Task<GetJobStatusResponse> GetJobStatusAsync(Guid jobId, CancellationToken cancellationToken = default);
  Task<CreateJobResponse> CreateJobAsync(CreateJobRequest request, CancellationToken cancellationToken = default);
  Task<CompleteJobResponse> CompleteJobAsync(Guid jobId, CancellationToken cancellationToken = default);
  Task<BillingSummaryResponse> GetBillingSummaryAsync(Guid jobId, CancellationToken cancellationToken = default);
}