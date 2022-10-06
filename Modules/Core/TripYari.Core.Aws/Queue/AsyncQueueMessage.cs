namespace TripYari.Core.Aws.Queue
{
    public class AsyncQueueMessage<T>
    {
        public DateTimeOffset Timestamp { get; set; }
        public string RequestId { get; set; }
        public string MessageType { get; set; }
        public T Message { get; set; }

        public AsyncQueueMessage()
        {
        }

        public AsyncQueueMessage(string requestId, T request)
            : this(DateTimeOffset.UtcNow, requestId, request)
        {
        }

        public AsyncQueueMessage(DateTimeOffset timestamp, string requestId, T message)
        {
            Timestamp = timestamp;
            RequestId = requestId;
            MessageType = message?.GetType().Name ?? "<null>";
            Message = message;
        }
    }
}