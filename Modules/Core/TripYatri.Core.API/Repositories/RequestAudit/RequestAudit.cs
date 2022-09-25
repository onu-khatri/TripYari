using System;

namespace TripYatri.Core.API.Repositories.RequestAudit
{
    public class RequestAudit
    {
        public long Id { get; set; }
        public string Application { get; set; }
        public string BuildVersion { get; set; }
        public string RequestId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public int ElapsedTime { get; set; }
        public string ClientIp { get; set; }
        public string OwnerId { get; set; }
        public string ClientId { get; set; }
        public string AccountDid { get; set; }
        public string UserDid { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public int StatusCode { get; set; }
        public string Payload { get; set; }

        public RequestAudit(string application, string buildVersion, string requestId, DateTimeOffset startTime, 
            int elapsedTime, string clientIp, string ownerId, string clientId, string accountDid, string userDid, 
            string controller, string action, string method, string path, string query, string payload, int statusCode)
        {
            Application = TruncateToLength(application, 64);
            BuildVersion = TruncateToLength(buildVersion, 10);
            RequestId = TruncateToLength(requestId, 128);
            StartTime = startTime;
            ElapsedTime = elapsedTime;
            ClientIp = TruncateToLength(clientIp, 40);
            OwnerId = TruncateToLength(ownerId, 20);
            ClientId = TruncateToLength(clientId, 20);
            AccountDid = TruncateToLength(accountDid, 20);
            UserDid = TruncateToLength(userDid, 20);
            Controller = TruncateToLength(controller, 64);
            Action = TruncateToLength(action, 64);
            Method = TruncateToLength(method, 10);
            Path = TruncateToLength(path, 256);
            Query = TruncateToLength(query, 256);
            StatusCode = statusCode;
            Payload = TrimForTsv(TruncateToLength(payload, 4096));
        }

        private static string TruncateToLength(string value, int length)
        {
            if (value == null) return null;
            return value.Length > length ? value.Substring(0, length) : value;
        }

        private static string TrimForTsv(string value)
        {
            return value?.Replace(Environment.NewLine, string.Empty)
                .Replace("\t", "  ");
        }
    }
}   