using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;
using simplified_cloud_job_execution_backend.Infrastructure.AWS.Sqs;

namespace simplified_cloud_job_execution_backend.Infrastructure.Execution;

public class JobExecutionWorker(
    IAmazonSQS sqs,
    IOptions<SqsOptions> options,
    IServiceScopeFactory scopeFactory,
    ILogger<JobExecutionWorker> logger) : BackgroundService
{
  private readonly IAmazonSQS _sqs = sqs;
  private readonly SqsOptions _options = options.Value;
  private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
  private readonly ILogger<JobExecutionWorker> _logger = logger;

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      var response = await _sqs.ReceiveMessageAsync(
          new ReceiveMessageRequest
          {
            QueueUrl = _options.QueueUrl,
            MaxNumberOfMessages = 10,
            WaitTimeSeconds = 20
          },
        stoppingToken
      );

      if (response.Messages is null || response.Messages.Count == 0)
      {
        continue;
      }

      foreach (var message in response.Messages)
      {
        try
        {
          var payload = JsonSerializer.Deserialize<JobQueueMessage>(message.Body);
          if (payload == null)
          {
            continue;
          }

          using var scope = _scopeFactory.CreateScope();
          var executor = scope.ServiceProvider.GetRequiredService<IJobExecutor>();
          await executor.ExecuteAsync(payload.JobId, stoppingToken);
          await _sqs.DeleteMessageAsync(_options.QueueUrl, message.ReceiptHandle, stoppingToken);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Error processing SQS message");
        }
      }
    }
  }
}