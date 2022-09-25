using System.Collections.Generic;

namespace TripYatri.Core.Base.Providers.Hash
{
    public interface IHashProvider
    {
        byte[] Sha512(IEnumerable<string> args);
    }
}