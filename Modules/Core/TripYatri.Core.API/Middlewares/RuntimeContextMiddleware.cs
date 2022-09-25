using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using TripYatri.Core.Base.Providers.Logger;
using Microsoft.Extensions.Primitives;
using TripYatri.Core.Base;

namespace TripYatri.Core.API.Middlewares
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RuntimeContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RuntimeContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RuntimeContext runtimeContext)
        {
            #region RequestId

            var requestId = context.Request.Headers["request-id"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(requestId)) requestId = Guid.NewGuid().ToString();

            runtimeContext.SetRequestId(requestId);

            if (!context.Response.Headers.ContainsKey("request-id"))
                context.Response.Headers["request-id"] = new StringValues(requestId);

            #endregion

            #region LogLevel

            if (context.Request.Query.TryGetValue("loglevel", out var logLevelValues)
                && Enum.TryParse(logLevelValues.FirstOrDefault(), true, out LogLevel logLevel))
            {
                runtimeContext.LogLevel = logLevel;
            }

            #endregion

            #region Debug

            runtimeContext.IsDebug =
                runtimeContext.CurrentEnvironment.IsDevelopment()
                || context.Request.Query.ContainsKey("debug")
                && (
                    context.Request.Query["debug"].Contains("yes")
                    || context.Request.Query["debug"].Contains("true")
                );

            if (runtimeContext.IsDebug)
                runtimeContext.LogLevel = LogLevel.Debug;

            #endregion

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}