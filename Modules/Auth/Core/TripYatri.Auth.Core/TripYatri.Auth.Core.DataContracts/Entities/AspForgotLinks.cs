using System;

namespace TripYari.Auth.Core.DataContracts.Entities
{
    public class AspForgotLinks : EntityBase<int>
    {

        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UsedDate { get; set; }
        /// <summary>
        /// Gets or sets the status.0 = Not used; 1 = used; 2 = Expire.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        /// 
        public int status { get; set; }
        public virtual AppIdentityUser appIdentityUser { get; set; }
    }
}
