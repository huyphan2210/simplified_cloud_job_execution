namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.S3;

public interface IS3FileService
{
  /// <summary>
  /// Uploads a file to S3 and returns the file reference/key
  /// </summary>
  Task<string> UploadFileAsync(IFormFile file, Guid jobId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Uploads generated output content to S3 and returns the file reference/key
  /// </summary>
  Task<string> UploadOutputAsync(string content, Guid jobId, string fileName, CancellationToken cancellationToken = default);

  /// <summary>
  /// Generates a presigned URL for downloading a file from S3
  /// </summary>
  Task<string> GetPresignedUrlAsync(string fileKey, int expirationHours = 1, CancellationToken cancellationToken = default);

  /// <summary>
  /// Deletes a file from S3
  /// </summary>
  Task DeleteFileAsync(string fileKey, CancellationToken cancellationToken = default);
}
