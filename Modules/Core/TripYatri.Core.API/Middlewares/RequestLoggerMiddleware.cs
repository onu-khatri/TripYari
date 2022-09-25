using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Threading.Tasks;
using TripYatri.Core.Base;

namespace TripYatri.Core.API.Middlewares
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RuntimeEnvironment runtimeEnvironment, ILogger logger)
        {
            var log = runtimeEnvironment.IsDevelopment() || context.Request.Path.Value != "/healthcheck";
            var start = DateTime.Now;
            if (log) logger.LogInfo($"Start {context.Request.Method} {context.Request.GetEncodedPathAndQuery()}");

            // Call the next delegate/middleware in the pipeline
            await _next(context);

            var elapsed = DateTime.Now - start;
            var logLevel = LogLevel.Info;
            if (context.Response.StatusCode >= 400) logLevel = LogLevel.Error;
            if (log) logger.Log(logLevel, $"End {context.Request.Method} {context.Request.GetEncodedPathAndQuery()} Status {context.Response.StatusCode} in {elapsed.TotalMilliseconds:n}ms");
        }
    }
}
