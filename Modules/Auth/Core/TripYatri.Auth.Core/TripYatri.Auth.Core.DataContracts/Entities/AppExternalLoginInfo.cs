using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace TripYari.Auth.Core.DataContracts.Entities
{
    public class AppExternalLoginInfo: UserLoginInfo
    {
        public AppExternalLoginInfo(string loginProvider, string providerKey, string displayName) : base(loginProvider, providerKey, displayName)
        {
        }

        public ClaimsPrincipal Principal { get; set; }
        public IEnumerable<AuthenticationToken> AuthenticationTokens { get; set; }
    }
}
