using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using TripYatri.Core.API.Providers;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.API.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// See https://stackoverflow.com/a/61885809/908607.
    /// In order for GetEndpoint() to return the actual endpoint instead of null, the following conditions have to be met.
    /// * Enable endpoint routing (AddControllers() instead of AddMvc())
    /// * Call your middleware between UseRouting() and UseEndpoints().
    /// </remarks>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RequestAuditMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestAuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context, 
            RuntimeEnvironment runtimeEnvironment,
            IRequestAuditProvider requestAuditProvider,
            ILogger logger)
        {
            context.Request.EnableBuffering();
            
            var start = DateTime.Now;

            // Call the next delegate/middleware in the pipeline
            await _next(context);

            var elapsed = DateTime.Now - start;

            if (runtimeEnvironment.IsDevelopment() || context.Request.Path.Value != "/healthcheck")
            {
                try
                {
                    await requestAuditProvider.Audit(context, start, elapsed);
                }
                catch (Exception ex)
                {
                    logger.LogError("Failed to audit request", ex);
                }
            }
        }
    }
}