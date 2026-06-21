namespace simplified_cloud_job_execution_backend.Exceptions;

public abstract class AppException(string message, int statusCode) : Exception(message)
{
  public int StatusCode { get; } = statusCode;
}
