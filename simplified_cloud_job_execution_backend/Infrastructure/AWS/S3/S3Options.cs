namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.S3;

public class S3Options
{
  public const string SectionName = "S3";
  public required string BucketName { get; set; }
}
