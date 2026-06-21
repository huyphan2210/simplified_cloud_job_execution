namespace simplified_cloud_job_execution_backend.Exceptions;

public sealed class NotFoundException(string message) : AppException(message, StatusCodes.Status404NotFound)
{ }