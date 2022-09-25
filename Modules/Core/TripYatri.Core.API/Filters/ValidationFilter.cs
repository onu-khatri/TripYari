using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TripYatri.Core.API.Models;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly RuntimeContext _runtimeContext;
        private readonly ILogger _logger;

        public ValidationFilter(RuntimeContext runtimeContext, ILogger logger)
        {
            _runtimeContext = runtimeContext;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(mse =>
                        mse.Value.Errors.Select(e =>
                            new Error(
                                mse.Key,
                                (int)HttpStatusCode.BadRequest,
                                e.Exception?.GetType()?.Name ?? HttpStatusCode.BadRequest.ToString(),
                                !string.IsNullOrWhiteSpace(e.ErrorMessage)
                                    ? e.ErrorMessage
                                    : e.Exception?.Message ?? "Unknown error",
                                _runtimeContext.CurrentEnvironment.IsProduction()
                                    ? ""
                                    : e.Exception?.ToString() ?? "")));

                _logger.LogError("Request validation errors", errors);

                var apiResponse = new ApiResponse();
                apiResponse.Errors.AddRange(errors);
                context.Result = new BadRequestObjectResult(apiResponse);
                return;
            }

            await next();
        }
    }
}