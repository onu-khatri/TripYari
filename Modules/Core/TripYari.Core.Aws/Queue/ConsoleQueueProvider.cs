using TripYari.Core.Loggers;
using TripYari.Core.RuntimeContext;

namespace TripYari.Core.Aws.Queue
{
    public class ConsoleQueueProvider : IQueueProvider
    {
        private readonly ILogger _logger;
        private readonly RuntimeContextProvider _runtimeContext;

        public ConsoleQueueProvider(ILogger logger, RuntimeContextProvider runtimeContext)
        {
            _logger = logger;
            _runtimeContext = runtimeContext;
        }

        public Task EnqueueMessagesAsync<T>(string sqsQueueUrl, IEnumerable<AsyncQueueMessage<T>> messages)
        {
            _logger.LogInfo($"Enqueuing to {sqsQueueUrl} {messages.Count()} messages");
            foreach (var message in messages)
                _logger.LogInfo($"Enqueuing to {sqsQueueUrl} message of type {message.GetType().Name}",
                    message);
            return Task.CompletedTask;
        }

        public Task EnqueueMessageAsync<T>(string sqsQueueUrl, T payload)
        {
            _logger.LogInfo($"Enqueuing to {sqsQueueUrl} message of type {payload.GetType().Name}",
                new AsyncQueueMessage<T>(_runtimeContext.GetNextRequestId(), payload));
            return Task.CompletedTask;
        }
    }
}