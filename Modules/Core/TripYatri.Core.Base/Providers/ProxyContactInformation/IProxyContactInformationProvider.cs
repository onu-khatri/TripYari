
using TripYatri.Core.Base.Providers.ProxyContactInformation;

namespace TripYatri.Core.Base.Providers.ProxyContactInformation
{
    public interface IProxyContactInformationProvider
    {
        string EncryptProxyEmail(string candidateEmail, string accountDid = null);
        ProxyEmailPayload DecryptProxyEmail( string proxyEmail);
        string DecryptOrSame(string proxyEmail);
    }
}
