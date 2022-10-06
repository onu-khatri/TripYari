using System.Net;
using System.Text.RegularExpressions;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using TripYari.Core.Metrics;
using TripYari.Core.Loggers;
using TripYari.Core.RuntimeContext;

namespace TripYari.Core.Aws.Queue
{
    public class SqsQueueProvider : IQueueProvider, IDisposable
    {
        private readonly IMetricsProvider _metricsProvider;
        private readonly ILogger _logger;
        private readonly IRuntimeContextProvider _runtimeContext;
        private readonly AmazonSQSClient _sqsClient;

        private static readonly Regex SqsBatchEntryIdValidCharacters =
            new Regex("[^a-z0-9-_]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public SqsQueueProvider(
            IMetricsProvider metricsProvider,
            ILogger logger,
            IRuntimeContextProvider runtimeContext)
        {
            _metricsProvider = metricsProvider;
            _logger = logger;
            _runtimeContext = runtimeContext;
            _sqsClient = new AmazonSQSClient();
        }

        public async Task EnqueueMessagesAsync<T>(string sqsQueueUrl, IEnumerable<AsyncQueueMessage<T>> messages)
        {
            using var _ = _metricsProvider.BeginTiming(this, task: sqsQueueUrl);
            if (sqsQueueUrl.StartsWith("arn"))
            {
                var sqsQueueUrlResponse = await _sqsClient.GetQueueUrlAsync(sqsQueueUrl);
                if (sqsQueueUrlResponse.HttpStatusCode >= HttpStatusCode.BadRequest)
                    throw new Exception(
                        $"Failed to resolve SQS Queue URL from ARN {sqsQueueUrl}. StatusCode {sqsQueueUrlResponse.HttpStatusCode}");
                sqsQueueUrl = sqsQueueUrlResponse.QueueUrl;
            }

            foreach (var messagesBatch in messages
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / 10)
                .Select(x => x.Select(v => v.Value)))
            {
                var jsonMessages = messagesBatch
                    .Select(message => JsonSerializer.Serialize(message));

                var sendRequestEntries = jsonMessages
                    .Select(jsonMessage =>
                        new SendMessageBatchRequestEntry(
                            SqsBatchEntryIdValidCharacters.Replace(
                                _runtimeContext.GetNextRequestId(),
                                "_"),
                            jsonMessage))
                    .ToList();
                var sendMessageBatchRequest = new SendMessageBatchRequest(sqsQueueUrl, sendRequestEntries);
                var sendMessageBatchResponse = await _sqsClient.SendMessageBatchAsync(sendMessageBatchRequest);
                if (sendMessageBatchResponse.HttpStatusCode >= HttpStatusCode.BadRequest
                    || sendMessageBatchResponse.Failed.Any())
                {
                    _logger.LogError($"Failed to enqueue messages", sendMessageBatchResponse.Failed);
                    _logger.LogWarning("Successfully enqueued messages: ", sendMessageBatchResponse.Successful);

                    // retrying each failed message individually
                    var individualFailures = new List<Exception>();
                    _logger.LogWarning("Retrying failed messages individually...");
                    foreach (var failedMessage in sendMessageBatchResponse.Failed)
                    {
                        var originalBatchRequestEntry =
                            sendRequestEntries.FirstOrDefault(x => x.Id == failedMessage.Id);
                        if (originalBatchRequestEntry == null)
                        {
                            _logger.LogWarning($"Could not find original message {failedMessage.Id}");
                            continue;
                        }

                        _logger.LogWarning(
                            $"Retrying message {originalBatchRequestEntry.Id}...",
                            originalBatchRequestEntry.MessageBody);
                        var sendMessageRequest =
                            new SendMessageRequest(sqsQueueUrl, originalBatchRequestEntry.MessageBody);
                        var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);
                        if (sendMessageResponse.HttpStatusCode >= HttpStatusCode.BadRequest)
                        {
                            _logger.LogError(
                                $"Failed to enqueue the message {failedMessage.Id} to SQS",
                                sendMessageResponse);
                            individualFailures.Add(new Exception(
                                $"Failed to enqueue the message {failedMessage.Id} to SQS: {sendMessageResponse.HttpStatusCode}"));
                        }
                    }

                    if (individualFailures.Any())
                        throw new AggregateException("Failed to enqueue messages to SQS", individualFailures);
                }
            }
        }

        public async Task EnqueueMessageAsync<T>(string sqsQueueUrl, T payload)
        {
            using var _ = _metricsProvider.BeginTiming(this, task: sqsQueueUrl);
            if (sqsQueueUrl.StartsWith("arn"))
            {
                var sqsQueueUrlResponse = await _sqsClient.GetQueueUrlAsync(sqsQueueUrl);
                if (sqsQueueUrlResponse.HttpStatusCode >= HttpStatusCode.BadRequest)
                    throw new Exception(
                        $"Failed to resolve SQS Queue URL from ARN {sqsQueueUrl}. StatusCode {sqsQueueUrlResponse.HttpStatusCode}");
                sqsQueueUrl = sqsQueueUrlResponse.QueueUrl;
            }

            var payloadJson =
                JsonSerializer.Serialize(new AsyncQueueMessage<T>(_runtimeContext.GetNextRequestId(), payload));
            var sendMessageRequest = new SendMessageRequest(sqsQueueUrl, payloadJson);
            var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);
            if (sendMessageResponse.HttpStatusCode >= HttpStatusCode.BadRequest)
            {
                _logger.LogError(
                    $"Failed to enqueue the message to SQS:  {sendMessageResponse.HttpStatusCode}",
                    sendMessageResponse);
                throw new Exception($"Failed to enqueue the message to SQS:  {sendMessageResponse.HttpStatusCode}");
            }
        }

        public void Dispose()
        {
            _sqsClient?.Dispose();
        }
    }
}