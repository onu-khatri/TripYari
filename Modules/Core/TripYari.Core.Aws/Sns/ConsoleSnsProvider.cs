using Amazon.SimpleNotificationService.Model;
using TripYari.Core.Loggers;
using TripYari.Core.RuntimeContext;
using System.Text.Json;

namespace TripYari.Core.Aws.Sns
{
    public class ConsoleSnsProvider : ISnsProvider
    {
        private readonly ILogger _logger;
        private readonly IRuntimeContextProvider _runtimeContext;

        public ConsoleSnsProvider(
            ILogger logger,
            IRuntimeContextProvider runtimeContext)
        {
            _logger = logger;
            _runtimeContext = runtimeContext;
        }

        public Task SendNotificationAsync(string topicArn, object message, string subject = null)
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

            _logger.LogInfo("Publishing SNS Message", publishRequest);

            return Task.CompletedTask;
        }
    }
}