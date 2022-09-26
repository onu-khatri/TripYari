using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TripYatri.Core.Base.Providers.ProxyContactInformation
{
    public class ShuffledDictionaryProxyContactInformationProvider : IProxyContactInformationProvider
    {
        private readonly RuntimeContext _runtimeContext;

        private static readonly char[] Dictionary = @"abcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();
        private static readonly Regex SinglePlusRegex = new Regex(@"(?<=[^\+])\+(?=[^\+])", RegexOptions.Compiled);

        private string ShuffleKey { get; }
        private string ProxyDomain => ProxyDomains.DefaultIfEmpty("proxy.jobs.net").FirstOrDefault();
        private string[] ProxyDomains { get; }

        public ShuffledDictionaryProxyContactInformationProvider(
            IOptions<ProxyContactOptions> proxyContactOptions,
            RuntimeContext runtimeContext
        )
        {
            _runtimeContext = runtimeContext;
            ShuffleKey = proxyContactOptions.Value.EncryptionKey;
            if (string.IsNullOrEmpty(ShuffleKey))
                ShuffleKey = "Ac5vH0DP/TGPfbVfDk0y2U1dGQmzhTVXjj6+n1Ioa5c=";

            ProxyDomains = proxyContactOptions.Value.EmailProxyActiveDomains;
            if (ProxyDomains == null || !ProxyDomains.Any())
                ProxyDomains = new[]
                {
                    "stageproxy.jobs.net",
                    "stageproxy1.jobs.net",
                    "stageproxy2.jobs.net",
                    "stageproxy3.jobs.net",
                    "proxy.jobs.net"
                };
        }


        public string EncryptProxyEmail(string candidateEmail, string accountDid)
        {
            if (string.IsNullOrWhiteSpace(accountDid))
                accountDid = _runtimeContext.AccountDid;

            if (string.IsNullOrEmpty(ShuffleKey))
                throw new InvalidOperationException("Proxy Encryption Key is not configured");
            if (string.IsNullOrWhiteSpace(candidateEmail))
                throw new ArgumentException("Email to encrypt is required", nameof(candidateEmail));
            if (string.IsNullOrWhiteSpace(accountDid))
                throw new ArgumentException("Account DID is required", nameof(accountDid));

            var shuffleKeyB64 = ShuffleKey;
            var shuffleKey = Convert.FromBase64String(shuffleKeyB64);

            var proxyPayload = new ProxyEmailPayload(accountDid, candidateEmail);
            var proxyPayloadStr = proxyPayload.ToString().ToLower();

            var iv = proxyPayloadStr.Substring(0, 6);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            var payloadToEncode = proxyPayloadStr.Substring(6);

            var newDictionary = ShuffleDictionary(shuffleKey, ivBytes, Dictionary);

            var encoded = iv + Tr(payloadToEncode.Replace("+", "++").Replace('@', '+'), Dictionary, newDictionary);

            return $"{encoded}@{ProxyDomain}";
        }

        public ProxyEmailPayload DecryptProxyEmail(string proxyEmail)
        {
            var shuffleKeyB64 = ShuffleKey;
            var shuffleKey = Convert.FromBase64String(shuffleKeyB64);

            var proxyEmailTokens = proxyEmail.Split('@');

            var iv = proxyEmailTokens[0].Substring(0, 6);
            var ivBytes = Encoding.UTF8.GetBytes(iv);
            var shuffledPayload = proxyEmailTokens[0].Substring(6);

            var newDictionary = ShuffleDictionary(shuffleKey, ivBytes, Dictionary);

            var decrypted = iv + SinglePlusRegex.Replace(Tr(shuffledPayload, newDictionary, Dictionary), "@")
                .Replace("++", "+");

            return ProxyEmailPayload.From(decrypted);
        }

        /// <summary>
        /// An aproximation to Perl's tr function.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentDictionary"></param>
        /// <param name="newDictionary"></param>
        /// <returns></returns>
        private static string Tr(string data, char[] currentDictionary, IReadOnlyList<char> newDictionary)
        {
            var sb = new StringBuilder();
            foreach (var dataChar in data)
            {
                var currentDictionaryIndex = Array.FindIndex(currentDictionary, c => c == dataChar);
                if (currentDictionaryIndex >= 0)
                    sb.Append(newDictionary[currentDictionaryIndex]);
                else
                    sb.Append(dataChar);
            }

            return sb.ToString();
        }

        private static char[] ShuffleDictionary(IReadOnlyList<byte> shuffleKey, IReadOnlyList<byte> ivBytes,
            IReadOnlyCollection<char> dictionary)
        {
            var shuffledDictionary = new StringBuilder();
            var availableDictionary = dictionary.ToList();
            var shuffleKeyIdx = shuffleKey[0];
            for (var dictionaryIdx = 0; dictionaryIdx < dictionary.Count; dictionaryIdx++)
            {
                shuffleKeyIdx ^= (byte)(shuffleKey[dictionaryIdx % shuffleKey.Count] ^
                                        ivBytes[dictionaryIdx % ivBytes.Count]);
                var availableDictionaryIdx = shuffleKeyIdx % availableDictionary.Count;
                shuffledDictionary.Append(availableDictionary[availableDictionaryIdx]);
                availableDictionary.RemoveAt(availableDictionaryIdx);
            }

            return shuffledDictionary.ToString().ToCharArray();
        }

        public bool EmailEndsWithProxyDomain(string email)
        {
            var lowerEmail = email.ToLower();
            return ProxyDomains.Any(proxyDomain => lowerEmail.EndsWith(proxyDomain))
                   || lowerEmail.EndsWith(ProxyDomain);
        }

        public string DecryptOrSame(string proxyEmail)
        {
            if (string.IsNullOrWhiteSpace(proxyEmail))
                return string.Empty;

            if (EmailEndsWithProxyDomain(proxyEmail))
                return DecryptProxyEmail(proxyEmail).CandidateEmail;

            return proxyEmail;
        }
    }
}