using Newtonsoft.Json;
using System;

namespace TripYatri.Core.Base.Providers.ProxyContactInformation
{
    public class ProxyEmailPayload
    {
        [JsonProperty("a")]
        public string Account { get; set; }

        [JsonProperty("c")]
        public string CandidateEmail { get; set; }

        public static ProxyEmailPayload From(string encoded)
        {
            var tokens = encoded.Split(new[] { '~' }, 2);

            if (tokens.Length == 1)
                throw new ArgumentException($"Proxy Email {encoded} doesn't contain an account separator.");
            
            return new ProxyEmailPayload(tokens[0], tokens[1]);
        }

        public ProxyEmailPayload(string account, string candidateEmail)
        {
            Account = account;
            CandidateEmail = candidateEmail;
        }

        public override string ToString()
        {
            return Account + "~" + CandidateEmail;
        }
    }
}
