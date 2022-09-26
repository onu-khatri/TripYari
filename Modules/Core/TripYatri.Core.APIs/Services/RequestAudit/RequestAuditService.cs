using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TripYatri.Core.Base.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.API.Providers.RequestAudit
{
    public class RequestAuditProvider : IRequestAuditProvider
    {
        private readonly ILogger _logger;
        private readonly RequestAuditBufferedProvider _requestAuditBufferedProvider;
        private readonly RuntimeContext _runtimeContext;

        public RequestAuditProvider(
            ILogger logger,
            RequestAuditBufferedProvider requestAuditBufferedProvider,
            RuntimeContext runtimeContext)
        {
            _logger = logger;
            _requestAuditBufferedProvider = requestAuditBufferedProvider;
            _runtimeContext = runtimeContext;
        }

        public async Task Audit(HttpContext httpContext, DateTimeOffset startTime, TimeSpan elapsedTime)
        {
            var controllerName = "<Unknown>";
            var actionName = "<Unknown>";

            try
            {
                // Capture controller & action names
                var endpoint = httpContext
                    .Features.Get<IEndpointFeature>()?.Endpoint; // .GetEndpoint()
                if (endpoint != null)
                {
                    var controllerActionDescriptor = endpoint.Metadata
                        .GetMetadata<ControllerActionDescriptor>();

                    controllerName = controllerActionDescriptor?.ControllerName;
                    if (string.IsNullOrWhiteSpace(controllerName))
                        controllerName = "<Unknown>";
                    actionName = controllerActionDescriptor?.ActionName;
                    if (string.IsNullOrWhiteSpace(actionName))
                        actionName = "<Unknown>";
                }
                else
                {
                    _logger.LogWarning("Could not find MVC Endpoint. No controller/action could be determined.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to find controller/action", ex);
            }

            // find client ip
            var clientIp = httpContext.Request.Headers["X-FORWARDED-FOR"].ToString();
            if (string.IsNullOrWhiteSpace(clientIp))
                clientIp = httpContext.Connection.RemoteIpAddress.ToString();

            // get the request body/payload
            // Leave the body open so the next middleware can read it.
            var requestBody = "";
            httpContext.Request.Body.Position = 0;
            using (var reader = new StreamReader(
                httpContext.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024 * 30,
                leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                // Do some processing with body…

                // Reset the request body stream position so the next middleware can read it
                httpContext.Request.Body.Position = 0;
            }

            var requestAudit = new Repositories.RequestAudit.RequestAudit(
                $"{_runtimeContext.Team ?? "<Unknown>"}-{_runtimeContext.Application ?? "<Unknown>"}",
                _runtimeContext.BuildVersion ?? "UnknownVersion",
                _runtimeContext.GetBaseRequestId(),
                startTime,
                (int) elapsedTime.TotalMilliseconds,
                clientIp,
                _runtimeContext.OAuthOwnerId ?? "UnknownOid",
                _runtimeContext.OAuthClientId ?? "UnknownCid",
                _runtimeContext.AccountDid,
                _runtimeContext.UserDid,
                controllerName,
                actionName,
                httpContext.Request.Method,
                httpContext.Request.Path.ToUriComponent(),
                httpContext.Request.QueryString.ToUriComponent(),
                requestBody,
                httpContext.Response.StatusCode);

            _requestAuditBufferedProvider.Audit(requestAudit);
        }
    }
}