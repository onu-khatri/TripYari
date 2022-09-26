using MediatR;
using Microsoft.Extensions.Options;
using TripYari.Auth.Core.UseCases.Enum;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Travel.Core.Helpers.Models;
using Travel.Core.Helpers;
using TripYari.Auth.Core.DataContracts.Repositories;
using TripYari.Auth.Core.DataContracts.Entities;

namespace TripYari.Auth.Core.UseCases.Token
{
    public class GetTokenInteractor : IRequestHandler<GetTokenRequest, GetTokenResponse>
    {
        private readonly AppSettings _appSettings;
        private readonly IAppUserManager<AppIdentityUser> _userManager;
        private readonly ITokenFactory _tokenFactory;

        public GetTokenInteractor(IAppUserManager<AppIdentityUser> userManager, IOptions<AppSettings> appSettings, ITokenFactory tokenFactory)
        {
            _userManager = userManager;
            _tokenFactory = tokenFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<GetTokenResponse> Handle(GetTokenRequest request, CancellationToken cancellationToken)
        {
            var tokenData = new GetTokenResponse();

            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                tokenData.FailureMessage = "email do not exists";
                return tokenData;
            }

            var userValidation = await _userManager.CheckPasswordAsync(user, request.Password);
            var roleType = UserType.None;

            if (userValidation)
            {

                if (await _userManager.IsLockedOutAsync(user))
                {
                    tokenData.FailureMessage = string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", _appSettings.DefaultAccountLockoutTimeSpan);

                    return tokenData;
                }
                else if (await _userManager.GetLockoutEnabledAsync(user) && !userValidation)
                {
                    // if user is subject to lockouts and the credentials are invalid
                    // record the failure and check if user is lockedout and display message, otherwise,
                    // display the number of attempts remaining before lockout

                    // Record the failure which also may cause the user to be locked out
                    await _userManager.AccessFailedAsync(user);


                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        tokenData.FailureMessage = string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", _appSettings.DefaultAccountLockoutTimeSpan);
                        
                    }
                    else
                    {
                        int accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);


                         int attemptsLeft = _appSettings.MaxFailedAccessAttemptsBeforeLockout - accessFailedCount;
                        
                        tokenData.FailureMessage = string.Format("Invalid credentials. You have {0} more attempt(s) before your account gets locked out.", attemptsLeft);

                    }

                    return tokenData;
                }
                else
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                
                var roleData = await _userManager.GetRolesAsync(user);
                if (roleData != null && roleData.Count() > 0)
                    roleType = EnumExtensions.ParseEnum<UserType>(roleData.FirstOrDefault());

                if (roleType != UserType.None)
                {
                    if (roleType != request.UserRole && roleType != UserType.SuperAdmin)
                    {
                        tokenData.FailureMessage = "You are not a valid user.";
                        return tokenData;
                    }
                }
                else
                {
                    tokenData.FailureMessage = "Role not found for user.";
                    return tokenData;
                }                   
            }
            else
            {
                tokenData.FailureMessage = "Invalid login attempt.";
                return tokenData;
            }


            var jwtTokenData = _tokenFactory.CreateJwtTokens(user, _appSettings, roleType.ToString());
            tokenData.AccessToken = jwtTokenData.AccessToken;
            tokenData.UserName = user.UserName;
            tokenData.RefreshToken = jwtTokenData.RefreshToken;
            tokenData.RefreshTokenSerial = jwtTokenData.RefreshTokenSerial;
            tokenData.Status = true;
            return tokenData;
        }
    }
}
