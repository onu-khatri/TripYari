using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;
using TripYatri.Core.Base.Providers.Json;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.Base.Providers.Sns
{
    public class ConsoleSnsProvider : ISnsProvider
    {
        private readonly IJsonProvider _jsonProvider;
        private readonly ILogger _logger;
        private readonly RuntimeContext _runtimeContext;

        public ConsoleSnsProvider(
            IJsonProvider jsonProvider,
            ILogger logger,
            RuntimeContext runtimeContext)
        {
            _jsonProvider = jsonProvider;
            _logger = logger;
            _runtimeContext = runtimeContext;
        }

        public Task SendNotificationAsync(string topicArn, object message, string subject = null)
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

            _logger.LogInfo("Publishing SNS Message", publishRequest);

            return Task.CompletedTask;
        }
    }
}