using System.Threading.Tasks;

namespace TripYatri.Core.Base.Providers.Sns
{
    public interface ISnsProvider
    {
        Task SendNotificationAsync(string topicArn, object message, string subject = null);
    }
}