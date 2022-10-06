namespace TripYari.Core.RuntimeContext
{
    public interface IRuntimeContextProvider
    {
        string AccountDid { get; set; }
        string Application { get; set; }
        string BuildVersion { get; set; }
        RuntimeEnvironment CurrentEnvironment { get; }
        bool IsDebug { get; set; }
        string OAuthClientId { get; set; }
        string OAuthOwnerId { get; set; }
        DateTimeOffset RequestMaxEndTime { get; }
        TimeSpan RequestRemainingTime { get; }
        DateTimeOffset RequestStartTime { get; }
        string ServerName { get; set; }
        string Team { get; set; }
        string UserDid { get; set; }

        string GetNextRequestId();
        string GetRuntimeContextRequestId();
        void SetRequestId(string requestId);
    }
}