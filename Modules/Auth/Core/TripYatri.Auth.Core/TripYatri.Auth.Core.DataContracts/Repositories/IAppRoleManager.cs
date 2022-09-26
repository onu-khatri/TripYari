using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TripYari.Auth.Core.DataContracts.Repositories
{
    public interface IAppRoleManager<TRole> : IDisposable where TRole : class
    {
        IQueryable<TRole> Roles { get; }
        Task<IdentityResult> AddClaimAsync(TRole role, Claim claim);
        Task<IdentityResult> CreateAsync(TRole role);
        Task<IdentityResult> DeleteAsync(TRole role);
        Task<TRole> FindByIdAsync(Guid roleId);
        Task<TRole> FindByNameAsync(string roleName);
        Task<IList<Claim>> GetClaimsAsync(TRole role);
        Task<Guid> GetRoleIdAsync(TRole role);
        Task<string> GetRoleNameAsync(TRole role);
        Task<IdentityResult> RemoveClaimAsync(TRole role, Claim claim);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> SetRoleNameAsync(TRole role, string name);
    }
}
