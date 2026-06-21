using simplified_cloud_job_execution_backend.Domain.Jobs;

namespace simplified_cloud_job_execution_backend.Repositories.JobRepository;

public interface IJobRepository
{
  Task<Job> CreateJobAsync(Job job, CancellationToken cancellationToken = default);
  Task<Job?> GetJobByIdAsync(Guid jobId, CancellationToken cancellationToken = default);
  Task<Job> UpdateJobAsync(Job job, CancellationToken cancellationToken = default);
}