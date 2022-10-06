using System.Security.Cryptography;
using System.Text;

namespace TripYari.Core.Base.Providers.Hash
{
    public class HashProvider : IHashProvider
    {
        public byte[] Sha512(IEnumerable<string> args)
        {
            var toHash = string.Join("", args.Select(s => (s ?? "").ToLower().Trim()));
            var hashSvc = SHA512.Create();
            var hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(toHash));
            var hashString = BitConverter.ToString(hash).Replace("-", "").TrimStart('0').ToLower();
            return Encoding.ASCII.GetBytes(hashString);
        }
    }
}