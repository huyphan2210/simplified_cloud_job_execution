namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.Sqs;

public class SqsOptions
{
  public const string SectionName = "Sqs";
  public string QueueUrl { get; set; } = string.Empty;
}