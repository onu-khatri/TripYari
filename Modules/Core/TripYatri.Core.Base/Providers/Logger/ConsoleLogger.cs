using System;
using System.Runtime.CompilerServices;

namespace TripYatri.Core.Base.Providers.Logger
{
    public class ConsoleLogger : BaseLogger
    {
        #region Members

        private static readonly object Lock = new object();
        private readonly RuntimeContext _runtimeContext;

        #endregion

        #region Constructor

        public ConsoleLogger(RuntimeContext runtimeContext)
            : base(runtimeContext)
        {
            _runtimeContext = runtimeContext;
        }

        #endregion

        #region Methods

        public override void Log(LogLevel logLevel, string message, Exception exception = null, [CallerMemberName] string caller = "")
        {
            if (logLevel < _runtimeContext.LogLevel) return;
            
            var output = Console.Out;
            if (logLevel == LogLevel.Error)
                output = Console.Error;

            var prefix =
                $"[{DateTimeOffset.UtcNow:O}] [{_runtimeContext.GetNextRequestId()}] [{logLevel}] [{_runtimeContext.OAuthClientId}] [{_runtimeContext.AccountDid}] [{_runtimeContext.UserDid}] [{caller}] ";
            var logMessage = $"{message}{(exception != null ? Environment.NewLine + exception : "")}";
            logMessage = prefix + logMessage.Replace(Environment.NewLine, Environment.NewLine + prefix);
            
            lock (Lock)
            {
                output.WriteLine(logMessage);
            }
        }

        #endregion
    }
}
