using System;

namespace TripYari.Auth.Core.DataContracts.Entities
{
    public class UserEmailHistory: EntityBase<int>
    {
        public string Email { get; set; }
        public int EmailType { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Attachments { get; set; }
        public string BodyModelJson { get; set; }
        public string TemplateUrl { get; set; }
        public Guid UserId { get; set; }
        public virtual AppIdentityUser appIdentityUser { get; set; }
    }
}
