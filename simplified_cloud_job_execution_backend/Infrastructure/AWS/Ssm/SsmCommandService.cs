using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Options;

namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.Ssm;

/// <summary>
/// Implements SSM command execution with polling.
/// </summary>
public class SsmCommandService(
    IAmazonSimpleSystemsManagement ssmClient,
    IOptions<SsmOptions> ssmOptions,
    ILogger<SsmCommandService> logger) : ISsmCommandService
{
  private readonly IAmazonSimpleSystemsManagement _ssmClient = ssmClient;
  private readonly SsmOptions _ssmOptions = ssmOptions.Value;
  private readonly ILogger<SsmCommandService> _logger = logger;

  public async Task<string?> ExecuteCommandAsync(string instanceId, string commandText, CancellationToken cancellationToken)
  {
    try
    {
      _logger.LogInformation("Sending SSM command to instance {InstanceId}", instanceId);

      var commandRequest = new SendCommandRequest
      {
        InstanceIds = [instanceId],
        DocumentName = _ssmOptions.DocumentName,
        Parameters = new Dictionary<string, List<string>>
        {
          ["commands"] = [commandText]
        }
      };

      var commandResponse = await _ssmClient.SendCommandAsync(commandRequest, cancellationToken);
      var commandId = commandResponse.Command.CommandId;

      _logger.LogInformation("Command sent with ID {CommandId}; polling for completion...", commandId);

      GetCommandInvocationResponse? invocation = null;
      while (!cancellationToken.IsCancellationRequested)
      {
        try
        {
          invocation = await _ssmClient.GetCommandInvocationAsync(new GetCommandInvocationRequest
          {
            CommandId = commandId,
            InstanceId = instanceId
          }, cancellationToken);
        }
        catch (InvocationDoesNotExistException)
        {
          _logger.LogDebug("SSM invocation not yet available for command {CommandId} on instance {InstanceId}, retrying...", commandId, instanceId);
          await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
          continue;
        }

        if (invocation.Status == CommandInvocationStatus.Success 
            || invocation.Status == CommandInvocationStatus.Failed 
            || invocation.Status == CommandInvocationStatus.TimedOut 
            || invocation.Status == CommandInvocationStatus.Cancelled)
        {
          break;
        }

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
      }

      if (invocation == null)
      {
        _logger.LogError("Command invocation result is null for command {CommandId}", commandId);
        return null;
      }

      _logger.LogInformation("Command {CommandId} completed with status {Status}", commandId, invocation.Status);

      if (invocation.Status != CommandInvocationStatus.Success)
      {
        _logger.LogError("Command {CommandId} failed with status {Status}. Error: {Error}", 
          commandId, invocation.Status, invocation.StandardErrorContent);
        return null;
      }

      return invocation.StandardOutputContent ?? string.Empty;
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "SSM command execution failed for instance {InstanceId}", instanceId);
      return null;
    }
  }
}
