using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using TripYatri.Core.Base.Providers.Json;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.Base.Providers.Sns
{
    public class SnsProvider : ISnsProvider
    {
        private static AmazonSimpleNotificationServiceClient _snsClient;

        private readonly RuntimeContext _runtimeContext;
        private readonly IJsonProvider _jsonProvider;
        private readonly ILogger _logger;
        private IAmazonSimpleNotificationService SnsClient => _snsClient ??= new AmazonSimpleNotificationServiceClient();

        public SnsProvider(
            IJsonProvider jsonProvider,
            ILogger logger,
            RuntimeContext runtimeContext)
        {
            _runtimeContext = runtimeContext;
            _jsonProvider = jsonProvider;
            _jsonProvider.WithSnakeCase()
                .WithSimpleIsoDateFormat();
            _logger = logger;
        }

        public async Task SendNotificationAsync(string topicArn, object message, string subject = null)
        {
            string messageString;
            if (message is string s)
                messageString = s;
            else
                messageString = _jsonProvider.Serialize(message);

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