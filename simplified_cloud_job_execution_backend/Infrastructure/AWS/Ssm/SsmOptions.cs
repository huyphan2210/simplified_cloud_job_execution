namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.Ssm;

public class SsmOptions
{
  public const string SectionName = "Ssm";
  public required string InstanceId { get; set; }
  public string DocumentName { get; set; } = "AWS-RunShellScript";
}
