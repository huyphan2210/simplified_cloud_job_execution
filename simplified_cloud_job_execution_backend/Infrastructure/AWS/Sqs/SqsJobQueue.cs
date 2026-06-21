using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace simplified_cloud_job_execution_backend.Infrastructure.AWS.Sqs;

public class SqsJobQueue(
    IAmazonSQS sqs,
    IOptions<SqsOptions> options) : IJobQueue
{
  private readonly IAmazonSQS _sqs = sqs;
  private readonly SqsOptions _options = options.Value;

  public async Task EnqueueAsync(Guid jobId)
  {
    var payload = JsonSerializer.Serialize(new JobQueueMessage
    {
      JobId = jobId
    });

    await _sqs.SendMessageAsync(
      new SendMessageRequest
      {
        QueueUrl = _options.QueueUrl,
        MessageBody = payload
      }
    );
  }
}