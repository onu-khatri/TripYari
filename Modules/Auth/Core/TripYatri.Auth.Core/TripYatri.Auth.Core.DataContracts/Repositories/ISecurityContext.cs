using System;

namespace TripYari.Auth.Core.DataContracts.Repositories
{
    public interface ISecurityContext
    {
        bool HasClaims();
        bool IsAdmin();
        Guid GetCurrentUserId();
        string GetCurrentUserName();
        string GetCurrentEmail();
        string GetIndentityProvider();
    }
}
