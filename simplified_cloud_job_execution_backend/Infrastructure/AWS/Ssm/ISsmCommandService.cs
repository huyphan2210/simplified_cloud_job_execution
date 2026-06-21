namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.Ssm;

/// <summary>
/// Abstracts SSM SendCommand and polling logic.
/// </summary>
public interface ISsmCommandService
{
  /// <summary>
  /// Executes a shell command on an EC2 instance via SSM and waits for completion.
  /// </summary>
  /// <param name="instanceId">EC2 instance ID.</param>
  /// <param name="commandText">Shell command to execute.</param>
  /// <param name="cancellationToken">Cancellation token.</param>
  /// <returns>Command output (stdout), or null if execution failed.</returns>
  Task<string?> ExecuteCommandAsync(string instanceId, string commandText, CancellationToken cancellationToken);
}
