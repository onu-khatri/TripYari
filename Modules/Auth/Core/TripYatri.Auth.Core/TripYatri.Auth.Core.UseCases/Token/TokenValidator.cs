using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TripYari.Auth.Core.DataContracts.Entities;
using TripYari.Auth.Core.DataContracts.Repositories;

namespace TripYari.Auth.Core.UseCases.Token
{
    public interface ITokenValidator
    {
        Task ValidateAsync(TokenValidatedContext context);

    }

    public class TokenValidator : ITokenValidator
    {
        private readonly IAppUserManager<AppIdentityUser> _userManager;

        public TokenValidator(IAppUserManager<AppIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(TokenValidatedContext context)
        {
            var userPrincipal = context.Principal;

            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("This is not our issued token. It has no claims.");
                return;
            }

            var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {
                context.Fail("This is not our issued token. It has no serial.");
                return;
            }

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value; 
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail("This is not our issued token. It has no user-id.");
                return;
            }

            if(!Guid.TryParse(userId, out Guid validId))
                context.Fail("This token is expired. Please login again.");

            var user = await _userManager.FindByIdAsync(validId.ToString());
            if (user == null)// || user.SerialNumber != serialNumberClaim.Value || !user.IsActive)
            {
                // user has changed his/her password/roles/stat/IsActive
                context.Fail("This token is expired. Please login again.");
            }

            var accessToken = context.SecurityToken as JwtSecurityToken;
            if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.RawData))
            {
                context.Fail("This token is not in our database.");
                return;
            }
        }
    }
}
