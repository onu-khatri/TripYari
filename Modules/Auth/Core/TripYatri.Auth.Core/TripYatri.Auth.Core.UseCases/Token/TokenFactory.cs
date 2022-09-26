using Microsoft.IdentityModel.Tokens;
using TripYari.Auth.Core.UseCases.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Travel.Core.Helpers.Models;
using TripYari.Auth.Core.DataContracts.Entities;

namespace TripYari.Auth.Core.UseCases.Token
{
    public interface ITokenFactory
    {
        JwtTokensData CreateJwtTokens(AppIdentityUser user, AppSettings appSettings, string userRole);
        string GetRefreshTokenSerial(string refreshTokenValue, AppSettings appSettings);
    }

    public class TokenFactory : ITokenFactory
    {

        public  JwtTokensData CreateJwtTokens(AppIdentityUser user, AppSettings appSettings, string userRole)
        {
            var (accessToken, claims) = CreateAccessToken(user, appSettings, userRole);
            var (refreshTokenValue, refreshTokenSerial) = createRefreshToken(appSettings);
            return new JwtTokensData
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                RefreshTokenSerial = refreshTokenSerial
            };
        }

        Func<Guid> CreateCryptographicallySecureGuid = () => {
            var bytes = new byte[16];
            RandomNumberGenerator.Create().GetBytes(bytes);
            return new Guid(bytes);
        };


        private (string RefreshTokenValue, string RefreshTokenSerial) createRefreshToken(AppSettings appSettings)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var refreshTokenSerial = CreateCryptographicallySecureGuid().ToString().Replace("-", "");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, appSettings.JwtIssuer),
                 new Claim(JwtRegisteredClaimNames.Iss, appSettings.JwtIssuer, ClaimValueTypes.String, appSettings.JwtIssuer),
                 new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, appSettings.JwtIssuer),                
                 new Claim(ClaimTypes.SerialNumber, refreshTokenSerial, ClaimValueTypes.String,  appSettings.JwtIssuer)
        };

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: appSettings.JwtIssuer,
                audience: appSettings.JwtIssuer,
                claims: claims,
                notBefore: now,
                expires: now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

            return (tokenHandler.WriteToken(token), refreshTokenSerial);
        }


        private (string AccessToken, IEnumerable<Claim> Claims) CreateAccessToken(AppIdentityUser user, AppSettings appSettings, string userRole)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var claims = new List<Claim>
            {
                // Unique Id for all Jwt tokes
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String, appSettings.JwtIssuer),
                new Claim(ClaimTypes.Name, user.UserName, ClaimValueTypes.String, appSettings.JwtIssuer),
                new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.String, appSettings.JwtIssuer),
                new Claim(JwtRegisteredClaimNames.Jti, CreateCryptographicallySecureGuid().ToString(), ClaimValueTypes.String, appSettings.JwtIssuer),
                 new Claim(JwtRegisteredClaimNames.Iss, appSettings.JwtIssuer, ClaimValueTypes.String, appSettings.JwtIssuer),
                 new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, appSettings.JwtIssuer),
                 new Claim(ClaimTypes.Role, userRole, ClaimValueTypes.String, appSettings.JwtIssuer),
                 new Claim(ClaimTypes.SerialNumber, CreateCryptographicallySecureGuid().ToString().Replace("-",""), ClaimValueTypes.String,  appSettings.JwtIssuer)
        };

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: appSettings.JwtIssuer,
                audience: appSettings.JwtIssuer,
                claims: claims,
                notBefore: now,
                expires: now.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

            return (tokenHandler.WriteToken(token), claims);
        }
    

        public string GetRefreshTokenSerial(string refreshTokenValue, AppSettings appSettings)
        {
                if (string.IsNullOrWhiteSpace(refreshTokenValue))
                {
                    return null;
                }

                ClaimsPrincipal decodedRefreshTokenPrincipal = null;
                try
                {
                    decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                        refreshTokenValue,
                        new TokenValidationParameters
                        {
                            RequireExpirationTime = true,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtIssuer)),
                            ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                        ValidateLifetime = true, // validate the expiration
                        ClockSkew = TimeSpan.Zero // tolerance for the expiration date
                    },
                        out _
                    );
                }
                catch (Exception ex)
                {
                   // _logger.LogError(ex, $"Failed to validate refreshTokenValue: `{refreshTokenValue}`.");
                }

                return decodedRefreshTokenPrincipal?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
            
        }
    }
}
