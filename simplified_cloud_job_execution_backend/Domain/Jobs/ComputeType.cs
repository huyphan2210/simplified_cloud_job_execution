using System.Text.Json.Serialization;

namespace simplified_cloud_job_execution_backend.Domain.Jobs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComputeType
{
  CpuSmall = 1,
  CpuLarge = 2,
  Gpu = 3
}