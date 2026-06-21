namespace simplified_cloud_job_execution_backend.Exceptions;

public sealed class S3Exception(string message) : AppException(message, StatusCodes.Status500InternalServerError)
{ }
