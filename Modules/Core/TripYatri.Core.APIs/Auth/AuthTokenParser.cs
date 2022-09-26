using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TripYatri.Core.API.Auth
{
    public static class AuthTokenParser
    {
        public static BearerToken ParseToken(string tokenString)
        {
            try
            {
                BearerToken token;
                var tokenArray = tokenString.Split('|');
                var keyPairs = new Dictionary<string, string>();

                foreach (var pair in tokenArray)
                {
                    var parts = pair.Split('=');
                    if (parts.Length == 2)
                    {
                        keyPairs.Add(parts[0], parts[1]);
                    }
                }

                if (keyPairs.ContainsKey("cid") && keyPairs.ContainsKey("cbu") && keyPairs.ContainsKey("cba") && keyPairs.ContainsKey("oid"))
                {
                    token = new BearerToken(keyPairs["cid"], keyPairs["cba"], keyPairs["cbu"], keyPairs["oid"]);
                }
                else
                {
                    token = new BearerToken(tokenArray[0], tokenArray[1], tokenArray[2]);
                }

                return token;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Bad Request. Invalid Authorization token.", ex);
            }
        }
    }
}
