using System.Text.Json.Serialization;

namespace simplified_cloud_job_execution_backend.Domain.Jobs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum JobStatus
{
  Queued = 1,
  Running = 2,
  Completed = 3,
  Failed = 4
}