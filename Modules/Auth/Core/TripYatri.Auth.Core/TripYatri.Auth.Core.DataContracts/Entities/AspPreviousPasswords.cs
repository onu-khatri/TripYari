using System;

namespace TripYari.Auth.Core.DataContracts.Entities
{
    public class AspPreviousPasswords : EntityBase<int>
    {
        public AspPreviousPasswords()
        {
            CreateDate = DateTimeOffset.Now;
        }
        
        public string PasswordHash { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Guid UserId { get; set; }

       // public virtual ICollection<AppIdentityUser> appIdentityUser { get; } = new List<AppIdentityUser>();
        public virtual AppIdentityUser appIdentityUser { get; set; }

    }
}
