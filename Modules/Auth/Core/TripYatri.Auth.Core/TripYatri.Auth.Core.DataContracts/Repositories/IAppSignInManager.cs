using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using TripYari.Auth.Core.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TripYari.Auth.Core.DataContracts.Repositories
{
    public interface IAppSignInManager<TUser> : IDisposable where TUser : class
    {
        Task<ClaimsPrincipal> CreateUserPrincipalAsync(TUser user);
        Task<bool> IsTwoFactorClientRememberedAsync(TUser user);
        Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null);
        Task SignOutAsync();
        Task SignInAsync(TUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor);
        Task ForgetTwoFactorClientAsync();
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        Task<AppExternalLoginInfo> AppGetExternalLoginInfoAsync(string expectedXsrf = null);
        Task<TUser> GetTwoFactorAuthenticationUserAsync();
        bool IsSignedIn(ClaimsPrincipal principal);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null);
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
        Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
        Task RefreshSignInAsync(TUser user);
        Task RememberTwoFactorClientAsync(TUser user);
        Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient);
        Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode);
        Task<SignInResult> TwoFactorSignInAsync(string provider, string code, bool isPersistent, bool rememberClient);
        Task<IdentityResult> UpdateExternalAuthenticationTokensAsync(AppExternalLoginInfo externalLogin);
    }
}