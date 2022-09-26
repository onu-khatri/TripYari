using System.Collections.Generic;
using System.Threading.Tasks;
using TripYatri.Core.Base.Providers.Queue;

namespace TripYatri.Core.Base.Providers.Queue
{
    public interface IQueueProvider
    {
        Task EnqueueMessagesAsync<T>(string sqsQueueUrl, IEnumerable<AsyncQueueMessage<T>> payload);
        Task EnqueueMessageAsync<T>(string sqsQueueUrl, T payload);
    }
}