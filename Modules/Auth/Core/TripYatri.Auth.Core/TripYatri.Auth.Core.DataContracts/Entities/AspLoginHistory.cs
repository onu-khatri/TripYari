using System;

namespace TripYari.Auth.Core.DataContracts.Entities
{
    public class AspLoginHistory : EntityBase<int>
    {
        public Guid UserId { get; set; }
        public string IPAddress { get; set; }
        public bool? LoginStatus { get; set; }
        public string MACAddress { get; set; }
        public DateTime LoginDateTime { get; set; }
        public virtual AppIdentityUser appIdentityUser { get; set; }

    }
}
