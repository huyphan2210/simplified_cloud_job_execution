namespace simplified_cloud_job_execution_backend.Dtos.Exceptions;

public class ErrorResponse
{
  public string? ErrorCode { get; set; }
  public required string Message { get; set; }
}