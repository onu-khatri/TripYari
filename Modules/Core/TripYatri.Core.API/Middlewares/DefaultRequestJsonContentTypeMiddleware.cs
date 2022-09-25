using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.API.Middlewares
{
    public class DefaultRequestJsonContentTypeMiddleware
    {
        private readonly RequestDelegate _next;

        public DefaultRequestJsonContentTypeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, RuntimeEnvironment runtimeEnvironment, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(context.Request.ContentType))
            {
                context.Request.ContentType = "application/json";
                logger.LogDebug("No content-type specified on the request. Assuming application/json.");
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}