using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.API.Auth
{
    public class AuthValidator
    {
        private OAuthOptions OAuthOptions { get; }
        private ILogger Logger { get; }

        public AuthValidator(IOptions<OAuthOptions> oauthOptions, ILogger logger)
        {
            OAuthOptions = oauthOptions.Value;
            Logger = logger;
        }

        public void ValidateJwtHeader(string xProxySecretToken)
        {
            var isValidUrl = false;
            var publicKey = GetPublicKey(OAuthOptions.XProxyTokenPublicKey);
            var jwtDecodedToken = Jose.JWT.Decode(xProxySecretToken, publicKey);
            var jwtToken = JsonConvert.DeserializeObject<JObject>(jwtDecodedToken);

            if (jwtToken["aud"]?.Type.ToString() == "String")
            {
                isValidUrl = OAuthOptions.Audiences.Any(aud => jwtToken["aud"].ToString().ToLower().StartsWith(aud));
            }
            else if (jwtToken["aud"]?.Type.ToString() == "Array")
            {
                foreach (string item in (Array)jwtToken["aud"])
                {
                    if (OAuthOptions.Audiences.Any(aud => item.ToLower().StartsWith(aud)))
                    {
                        isValidUrl = true;
                        break;
                    }
                }
            }

            Logger.LogDebug($"IsValidJwtHeader2 JWT Token values. proxySecretToken: {xProxySecretToken}, jwtToken: ", jwtToken);

            var longExp = (long)jwtToken["exp"];

            if (jwtToken["iss"]?.ToString().ToLower() == "api.careerbuilder" && isValidUrl && new DateTime(longExp) < DateTime.Now)
            {
                return;
            }

            throw new UnauthorizedAccessException($"Failed to validate JwtHeader {AuthConstants.XProxyHeader}: {xProxySecretToken}");
        }

        private static AsymmetricAlgorithm GetPublicKey(string encodedKey)
        {
            var pkDecoded = Convert.FromBase64String(encodedKey);

            var cert = new X509Certificate2(pkDecoded);
            return cert.PublicKey.Key;
        }
    }
}
