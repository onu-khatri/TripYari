using System.Net;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using TripYari.Core.Loggers;
using TripYari.Core.RuntimeContext;
using System.Text.Json;

namespace TripYari.Core.Aws.Sns
{
    public class SnsProvider : ISnsProvider
    {
        private static AmazonSimpleNotificationServiceClient _snsClient;

        private readonly RuntimeContextProvider _runtimeContext;
        private readonly ILogger _logger;
        private IAmazonSimpleNotificationService SnsClient => _snsClient ??= new AmazonSimpleNotificationServiceClient();

        public SnsProvider(
            ILogger logger,
            RuntimeContextProvider runtimeContext)
        {
            _runtimeContext = runtimeContext;
            _logger = logger;
        }

        public async Task SendNotificationAsync(string topicArn, object message, string subject = null)
        {
            string messageString;
            if (message is string s)
                messageString = s;
            else
                messageString = JsonSerializer.Serialize(message);

            var publishRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = messageString,
                Subject = string.IsNullOrWhiteSpace(subject) ? _runtimeContext.GetNextRequestId() : subject
            };
            var publishResult = await SnsClient.PublishAsync(publishRequest);

            if (publishResult.MessageId != "" && publishResult.HttpStatusCode < (HttpStatusCode) 300)
                _logger.LogInfo($"SNS Notification published. MessageId {publishResult.MessageId}", message);
            else
            {
                _logger.LogError($"Notification exception. Status Code {publishResult.HttpStatusCode}",
                    publishResult.ResponseMetadata);
                throw new Exception($"Failed to send SNS Notification. Status Code {publishResult.HttpStatusCode}");
            }
        }
    }
}