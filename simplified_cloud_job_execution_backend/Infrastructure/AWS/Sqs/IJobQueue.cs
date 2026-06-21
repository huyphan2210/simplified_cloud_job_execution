namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.Sqs;

public interface IJobQueue
{
  Task EnqueueAsync(Guid jobId);
}