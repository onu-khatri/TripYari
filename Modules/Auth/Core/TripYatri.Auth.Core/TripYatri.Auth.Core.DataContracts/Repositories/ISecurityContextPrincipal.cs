using System.Security.Claims;

namespace TripYari.Auth.Core.DataContracts.Repositories
{
    public interface ISecurityContextPrincipal
    {
        ClaimsIdentity Claims { set; }
    }
}
