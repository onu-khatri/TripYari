namespace TripYari.Core.Aws.Queue
{
    public interface IQueueProvider
    {
        Task EnqueueMessagesAsync<T>(string sqsQueueUrl, IEnumerable<AsyncQueueMessage<T>> payload);
        Task EnqueueMessageAsync<T>(string sqsQueueUrl, T payload);
    }
}