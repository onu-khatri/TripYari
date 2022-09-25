using TripYatri.Core.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.API.Filters
{
    public class OAuthHandler : AuthorizationHandler<OAuthRequirement>
    {
        private readonly RuntimeContext _runtimeContext;
        private readonly RuntimeEnvironment _runtimeEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthValidator _authValidator;
        private readonly ILogger _logger;

        public OAuthHandler(RuntimeContext runtimeContext, RuntimeEnvironment runtimeEnvironment, IHttpContextAccessor httpContextAccessor, AuthValidator authValidator, ILogger logger)
        {
            _runtimeContext = runtimeContext;
            _runtimeEnvironment = runtimeEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _authValidator = authValidator;
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OAuthRequirement requirement)
        {
            try
            {
                // Get auth token
                var authHeader = GetRequestHeader(AuthConstants.AuthorizationHeader);
                BearerToken token = null;
                if (string.IsNullOrWhiteSpace(authHeader))
                {
                    throw new UnauthorizedAccessException("Failed to authenticate request with no Authorization header");
                }

                token = AuthTokenParser.ParseToken(authHeader);
                _runtimeContext.OAuthOwnerId = token.OwnerId;
                _runtimeContext.OAuthClientId = token.ClientId;
                _runtimeContext.AccountDid = token.AccountDid;
                _runtimeContext.UserDid = token.UserDid;

                // Skip proxy validation in Development
                if (!_runtimeEnvironment.IsDevelopment())
                {
                    // Get proxy secret token
                    var xProxySecretToken = GetRequestHeader(AuthConstants.XProxyHeader);
                    if (string.IsNullOrWhiteSpace(xProxySecretToken))
                    {
                        _logger.LogWarning($"No {AuthConstants.XProxyHeader} header value");
                    }

                    _authValidator.ValidateJwtHeader(xProxySecretToken);
                }
                else
                {
                    _logger.LogWarning($"Skipping Proxy Secret Token validation for environment {_runtimeEnvironment}");
                }

                // Set principal since this is an authenticated call
                _httpContextAccessor.HttpContext.User = ConstructClaimsPrincipal(token.ClientId, _runtimeContext.GetNextRequestId());

                context.Succeed(requirement);
                _logger.LogDebug($"Authenticated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                context.Fail();
            }

            return Task.CompletedTask;
        }

        private string GetRequestHeader(string headerName)
        {
            foreach (var hname in new[] { headerName, headerName.ToLower() })
            {
                var headerValues = _httpContextAccessor.HttpContext.Request.Headers[hname];
                if (headerValues.Count > 0)
                    return headerValues.FirstOrDefault();
            }

            return null;
        }

        private static ClaimsPrincipal ConstructClaimsPrincipal(string clientId, string requestId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, clientId),
                new Claim(ClaimTypes.UserData, requestId)
            };

            var id = new ClaimsIdentity(claims, "OAuth");
            return new ClaimsPrincipal(new[] { id });
        }
    }
}
