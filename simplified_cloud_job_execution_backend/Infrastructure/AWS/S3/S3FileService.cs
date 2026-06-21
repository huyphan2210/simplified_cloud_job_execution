using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using simplified_cloud_job_execution_backend.Exceptions;

namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.S3;

public class S3FileService(IAmazonS3 s3Client, IOptions<S3Options> options, ILogger<S3FileService> logger) : IS3FileService
{
  private readonly IAmazonS3 _s3Client = s3Client;
  private readonly S3Options _options = options.Value;
  private readonly ILogger<S3FileService> _logger = logger;

  public async Task<string> UploadFileAsync(IFormFile file, Guid jobId, CancellationToken cancellationToken = default)
  {
    if (file == null || file.Length == 0)
      throw new ValidationException("No file provided");

    if (file.Length > 104857600) // 100MB limit
      throw new ValidationException("File size exceeds 100MB limit");

    var fileKey = GenerateFileKey(jobId, file.FileName, isOutput: false);

    try
    {
      using var stream = file.OpenReadStream();
      var request = new PutObjectRequest
      {
        BucketName = _options.BucketName,
        Key = fileKey,
        InputStream = stream,
        ContentType = file.ContentType ?? "application/octet-stream"
      };

      await _s3Client.PutObjectAsync(request, cancellationToken);
      _logger.LogInformation("File uploaded successfully: {FileKey}", fileKey);
      return fileKey;
    }
    catch (AmazonS3Exception ex)
    {
      _logger.LogError(ex, "S3 error uploading file {FileName}", file.FileName);
      throw new S3Exception($"Failed to upload file: {ex.Message}");
    }
  }

  public async Task<string> UploadOutputAsync(string content, Guid jobId, string fileName, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(content))
      throw new ValidationException("Output content is required");

    var fileKey = GenerateFileKey(jobId, fileName, isOutput: true);

    try
    {
      var request = new PutObjectRequest
      {
        BucketName = _options.BucketName,
        Key = fileKey,
        ContentBody = content,
        ContentType = "application/json"
      };

      await _s3Client.PutObjectAsync(request, cancellationToken);
      _logger.LogInformation("Output uploaded successfully: {FileKey}", fileKey);
      return fileKey;
    }
    catch (AmazonS3Exception ex)
    {
      _logger.LogError(ex, "S3 error uploading output file {FileName}", fileName);
      throw new S3Exception($"Failed to upload output file: {ex.Message}");
    }
  }

  public async Task<string> GetPresignedUrlAsync(string fileKey, int expirationHours = 1, CancellationToken cancellationToken = default)
  {
    try
    {
      var request = new GetPreSignedUrlRequest
      {
        BucketName = _options.BucketName,
        Key = fileKey,
        Expires = DateTime.UtcNow.AddHours(expirationHours)
      };

      var url = _s3Client.GetPreSignedURL(request);
      return url;
    }
    catch (AmazonS3Exception ex)
    {
      _logger.LogError(ex, "S3 error generating presigned URL for {FileKey}", fileKey);
      throw new S3Exception($"Failed to generate presigned URL: {ex.Message}");
    }
  }

  public async Task DeleteFileAsync(string fileKey, CancellationToken cancellationToken = default)
  {
    try
    {
      var request = new DeleteObjectRequest
      {
        BucketName = _options.BucketName,
        Key = fileKey
      };

      await _s3Client.DeleteObjectAsync(request, cancellationToken);
      _logger.LogInformation("File deleted successfully: {FileKey}", fileKey);
    }
    catch (AmazonS3Exception ex)
    {
      _logger.LogError(ex, "S3 error deleting file {FileKey}", fileKey);
      throw new S3Exception($"Failed to delete file: {ex.Message}");
    }
  }

  private static string GenerateFileKey(Guid jobId, string fileName, bool isOutput)
  {
    var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
    var sanitizedFileName = SanitizeFileName(fileName);
    var folder = isOutput ? "outputs" : "inputs";
    return $"jobs/{jobId}/{folder}/{timestamp}_{sanitizedFileName}";
  }

  private static string SanitizeFileName(string fileName)
  {
    // Remove path separators and invalid characters
    var invalidChars = Path.GetInvalidFileNameChars();
    return new string(fileName
      .Where(c => !invalidChars.Contains(c) && c != '/' && c != '\\')
      .ToArray());
  }
}
