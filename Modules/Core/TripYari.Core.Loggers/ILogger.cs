using System.Runtime.CompilerServices;

namespace TripYari.Core.Loggers
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, string message, Exception exception = null, [CallerMemberName] string caller = "");
        void Log(LogLevel logLevel, Exception exception, [CallerMemberName] string caller = "");
        void Log(LogLevel logLevel, string message, object jsonObject, Exception exception = null, [CallerMemberName] string caller = "");

        void LogDebug(string message, Exception exception = null, [CallerMemberName] string caller = "");
        void LogDebug(Exception exception, [CallerMemberName] string caller = "");
        void LogDebug(string message, object jsonObject, Exception exception = null, [CallerMemberName] string caller = "");

        void LogAudit(string message, Exception exception = null, [CallerMemberName] string caller = "");
        void LogAudit(Exception exception, [CallerMemberName] string caller = "");
        void LogAudit(string message, object jsonObject, Exception exception = null, [CallerMemberName] string caller = "");

        void LogError(string message, Exception exception = null, [CallerMemberName] string caller = "");
        void LogError(Exception exception, [CallerMemberName] string caller = "");
        void LogError(string message, object jsonObject, Exception exception = null, [CallerMemberName] string caller = "");

        void LogWarning(string message, Exception exception = null, [CallerMemberName] string caller = "");
        void LogWarning(Exception exception, [CallerMemberName] string caller = "");
        void LogWarning(string message, object jsonObject, Exception exception = null, [CallerMemberName] string caller = "");

        void LogInfo(string message, Exception exception = null, [CallerMemberName] string caller = "");
        void LogInfo(Exception exception, [CallerMemberName] string caller = "");
        void LogInfo(string message, object jsonObject, Exception exception = null, [CallerMemberName] string caller = "");
    }
}
