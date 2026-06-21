namespace simplified_cloud_job_execution_backend.Infrastructure.Execution;

public interface IJobExecutor
{
  Task ExecuteAsync(Guid jobId, CancellationToken cancellationToken = default);
}