namespace simplified_cloud_job_execution_backend.Exceptions;

public sealed class ValidationException(string message) : AppException(message, StatusCodes.Status400BadRequest)
{ }