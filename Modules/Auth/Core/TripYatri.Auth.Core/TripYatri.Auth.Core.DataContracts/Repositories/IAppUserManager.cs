using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TripYari.Auth.Core.DataContracts.Repositories
{
    public interface IAppUserManager<TUser> : IDisposable where TUser : class
    {
        IdentityOptions Options { get; set; }

        IQueryable<TUser> Users { get; }
        IPasswordHasher<TUser> PasswordHasher { get; set; }

        Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(TUser user, int number);

        Task AddForgotLinkAsync(TUser user, string token);
        Task ExpirePreviousForgotLinkAsync(TUser user);
        Task UpdateForgotLinkAsync(TUser user, string token, int status);

        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByIdAsync(string userId);
        Task<TUser> FindByLoginAsync(string loginProvider, string providerKey);
        Task<TUser> FindByNameAsync(string userName);

        Task<IdentityResult> CreateAsync(TUser user);
        Task<IdentityResult> CreateAsync(TUser user, string password);
        Task<IdentityResult> AddToRoleAsync(TUser user, string role);
        Task<IdentityResult> DeleteAsync(TUser user);

        Task<IdentityResult> AddToRolesAsync(TUser user, IEnumerable<string> roles);
        Task<IdentityResult> ChangeEmailAsync(TUser user, string newEmail, string token);
        Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword);
        Task<IdentityResult> ChangePhoneNumberAsync(TUser user, string phoneNumber, string token);
        Task<bool> CheckPasswordAsync(TUser user, string password);
        Task<IdentityResult> ConfirmEmailAsync(TUser user, string token);
        Task<int> CountRecoveryCodesAsync(TUser user);

        Task<IdentityResult> AccessFailedAsync(TUser user);
        Task<IdentityResult> AddClaimAsync(TUser user, Claim claim);
        Task<IdentityResult> AddClaimsAsync(TUser user, IEnumerable<Claim> claims);
        Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login);
        Task<IdentityResult> AddPasswordAsync(TUser user, string password);

        Task<string> GenerateChangeEmailTokenAsync(TUser user, string newEmail);
        Task<string> GenerateChangePhoneNumberTokenAsync(TUser user, string phoneNumber);
        Task<string> GenerateConcurrencyStampAsync(TUser user);

        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);

        Task<string> GeneratePasswordResetTokenAsync(TUser user);
        Task<string> GenerateTwoFactorTokenAsync(TUser user, string tokenProvider);
        Task<string> GenerateUserTokenAsync(TUser user, string tokenProvider, string purpose);
        Task<int> GetAccessFailedCountAsync(TUser user);
        Task<string> GetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName);
        Task<string> GetAuthenticatorKeyAsync(TUser user);
        Task<IList<Claim>> GetClaimsAsync(TUser user);

        Task<string> GetEmailAsync(TUser user);

        Task<bool> GetLockoutEnabledAsync(TUser user);
        Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user);
        Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user);
        Task<string> GetPhoneNumberAsync(TUser user);
        Task<IList<string>> GetRolesAsync(TUser user);
        Task<string> GetSecurityStampAsync(TUser user);
        Task<bool> GetTwoFactorEnabledAsync(TUser user);
        Task<TUser> GetUserAsync(ClaimsPrincipal principal);
        Guid GetUserId(ClaimsPrincipal principal);
        Task<Guid> GetUserIdAsync(TUser user);
        string GetUserName(ClaimsPrincipal principal);
        Task<string> GetUserNameAsync(TUser user);
        Task<IList<TUser>> GetUsersForClaimAsync(Claim claim);
        Task<IList<TUser>> GetUsersInRoleAsync(string roleName);
        Task<IList<string>> GetValidTwoFactorProvidersAsync(TUser user);
        Task<bool> HasPasswordAsync(TUser user);
        Task<bool> IsEmailConfirmedAsync(TUser user);
        Task<bool> IsInRoleAsync(TUser user, string role);
        Task<bool> IsLockedOutAsync(TUser user);
        Task<bool> IsPhoneNumberConfirmedAsync(TUser user);

        Task<IdentityResult> RemoveAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName);
        Task<IdentityResult> RemoveClaimAsync(TUser user, Claim claim);
        Task<IdentityResult> RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims);
        Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role);
        Task<IdentityResult> RemoveFromRolesAsync(TUser user, IEnumerable<string> roles);
        Task<IdentityResult> RemoveLoginAsync(TUser user, string loginProvider, string providerKey);
        Task<IdentityResult> RemovePasswordAsync(TUser user);
        Task<IdentityResult> ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim);
        Task<IdentityResult> ResetAccessFailedCountAsync(TUser user);
        Task<IdentityResult> ResetAuthenticatorKeyAsync(TUser user);
        Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword);

        Task<IdentityResult> SetEmailAsync(TUser user, string email);
        Task<IdentityResult> SetLockoutEnabledAsync(TUser user, bool enabled);
        Task<IdentityResult> SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd);
        Task<IdentityResult> SetPhoneNumberAsync(TUser user, string phoneNumber);
        Task<IdentityResult> SetTwoFactorEnabledAsync(TUser user, bool enabled);
        Task<IdentityResult> SetUserNameAsync(TUser user, string userName);
        Task<IdentityResult> UpdateAsync(TUser user);
        Task UpdateNormalizedEmailAsync(TUser user);
        Task UpdateNormalizedUserNameAsync(TUser user);
        Task<IdentityResult> UpdateSecurityStampAsync(TUser user);
        Task<bool> VerifyChangePhoneNumberTokenAsync(TUser user, string token, string phoneNumber);
        Task<bool> VerifyTwoFactorTokenAsync(TUser user, string tokenProvider, string token);
        Task<bool> VerifyUserTokenAsync(TUser user, string tokenProvider, string purpose, string token);
        
        Task<IdentityResult> UpdatePasswordHash(TUser user, string newPassword, bool validatePassword);
        Task<IdentityResult> UpdateUserAsync(TUser user);

        Task<IdentityResult> ValidatePasswordAsync(TUser user, string password);
        Task<IdentityResult> ValidateUserAsync(TUser user);

        Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<TUser> store, TUser user, string password);
    }
}