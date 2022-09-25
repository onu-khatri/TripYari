using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json.Converters;

namespace TripYatri.Core.Base.Providers.Logger
{
    public abstract class BaseLogger : ILogger
    {
        private readonly RuntimeContext _runtimeContext;

        #region Members

        private JsonSerializerSettings SerializerSettings { get; set; }

        #endregion

        #region Constructor

        public BaseLogger(RuntimeContext runtimeContext)
        {
            _runtimeContext = runtimeContext;
            SerializerSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {new StringEnumConverter()},
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
        }

        #endregion

        #region Methods

        public abstract void Log(LogLevel logLevel, string message, Exception exception = null,
            [CallerMemberName] string caller = "");

        public void Log(LogLevel logLevel, Exception exception, [CallerMemberName] string caller = "")
        {
            if (logLevel < _runtimeContext.LogLevel) return;

            Log(logLevel, exception.Message, exception, caller);
        }

        public void Log(
            LogLevel logLevel,
            string message,
            object jsonObject,
            Exception exception = null,
            string caller = "")
        {
            if (logLevel < _runtimeContext.LogLevel) return;

            var json = SerializeObjectToJson(jsonObject);

            Log(logLevel, $"{message}{Environment.NewLine}{json}", exception: exception, caller: caller);
        }

        private string SerializeObjectToJson(object jsonObject)
        {
            var json = jsonObject as string ??
                       JsonConvert.SerializeObject(jsonObject,
                           _runtimeContext.CurrentEnvironment.IsProduction() ? Formatting.None : Formatting.Indented,
                           SerializerSettings);

            if (json.Length <= 1024) return json;
            var jsonBuilder = new StringBuilder();
            for (var i = 0; i < json.Length; i += 1024)
                jsonBuilder
                    .Append(i)
                    .Append(">> ")
                    .AppendLine(json.Substring(i, Math.Min(1024, json.Length - i)));
            return jsonBuilder.ToString();
        }

        public void LogDebug(string message, Exception exception = null, [CallerMemberName] string caller = "")
        {
            Log(LogLevel.Debug, message, exception, caller);
        }

        public void LogDebug(Exception exception, [CallerMemberName] string caller = "")
        {
            LogDebug(exception.Message, exception, caller);
        }

        public void LogDebug(string message, object jsonObject, Exception exception = null, string caller = "")
        {
            var json = SerializeObjectToJson(jsonObject);
            LogDebug($"{message}{Environment.NewLine}{json}", exception: exception, caller: caller);
        }

        public void LogAudit(string message, Exception exception = null, [CallerMemberName] string caller = "")
        {
            Log(LogLevel.Audit, message, exception, caller);
        }

        public void LogAudit(Exception exception, [CallerMemberName] string caller = "")
        {
            LogAudit(exception.Message, exception, caller);
        }

        public void LogAudit(string message, object jsonObject, Exception exception = null, string caller = "")
        {
            var json = SerializeObjectToJson(jsonObject);
            LogAudit($"{message}{Environment.NewLine}{json}", exception: exception, caller: caller);
        }

        public void LogError(string message, Exception exception = null, [CallerMemberName] string caller = "")
        {
            Log(LogLevel.Error, message, exception, caller);
        }

        public void LogError(Exception exception, [CallerMemberName] string caller = "")
        {
            LogError(exception.Message, exception, caller);
        }

        public void LogError(string message, object jsonObject, Exception exception = null, string caller = "")
        {
            var json = SerializeObjectToJson(jsonObject);
            LogError($"{message}{Environment.NewLine}{json}", exception: exception, caller: caller);
        }

        public void LogWarning(string message, Exception exception = null, [CallerMemberName] string caller = "")
        {
            Log(LogLevel.Warning, message, exception, caller);
        }

        public void LogWarning(Exception exception, [CallerMemberName] string caller = "")
        {
            LogWarning(exception.Message, exception, caller);
        }

        public void LogWarning(string message, object jsonObject, Exception exception = null, string caller = "")
        {
            var json = SerializeObjectToJson(jsonObject);
            LogWarning($"{message}{Environment.NewLine}{json}", exception: exception, caller: caller);
        }

        public void LogInfo(string message, Exception exception = null, [CallerMemberName] string caller = "")
        {
            Log(LogLevel.Info, message, exception, caller);
        }

        public void LogInfo(Exception exception, [CallerMemberName] string caller = "")
        {
            LogInfo(exception.Message, exception, caller);
        }

        public void LogInfo(string message, object jsonObject, Exception exception = null, string caller = "")
        {
            var json = SerializeObjectToJson(jsonObject);
            LogInfo($"{message}{Environment.NewLine}{json}", exception: exception, caller: caller);
        }

        #endregion
    }
}