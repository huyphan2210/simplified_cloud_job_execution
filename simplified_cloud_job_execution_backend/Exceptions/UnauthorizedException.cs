namespace simplified_cloud_job_execution_backend.Exceptions;

public sealed class UnauthorizedException(string message) : AppException(message, StatusCodes.Status401Unauthorized)
{ }