namespace TripYari.Core.Aws.Sns
{
    public interface ISnsProvider
    {
        Task SendNotificationAsync(string topicArn, object message, string subject = null);
    }
}