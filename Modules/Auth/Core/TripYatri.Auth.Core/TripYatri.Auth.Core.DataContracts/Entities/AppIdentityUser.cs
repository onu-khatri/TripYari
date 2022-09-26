using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripYari.Auth.Core.DataContracts.Entities
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public AppIdentityUser()
        {
            this.ForgotLinks = new List<AspForgotLinks>();
            PreviousPasswords = new List<AspPreviousPasswords>();
            LoginHistoryData = new List<AspLoginHistory>();
        }
        

        [MaxLength(250)]
        public string FirstName { get; set; }

        [MaxLength(250)]
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? CreatedBy { get; set;}
        public DateTime? LastLogin { get; set; }

        [MaxLength(250)]
        public string SecurityQuestion { get; set; }

        [MaxLength(250)]
        public string SecurityAnswer { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsDeactive { get; set; }
        public string ModifyBy { get; set; }        
        public Nullable<System.DateTime> ModifyDate { get; set; }


       //public string TableName { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserManager manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //   var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    return userIdentity;
        //   // return null;
        //}

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserManager manager, string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
        //    // Add custom user claims here
        //    return userIdentity;
        //}

        public virtual ICollection<AspForgotLinks> ForgotLinks { get; set; }
        public virtual ICollection<AspLoginHistory> LoginHistoryData { get; set; }
        public virtual ICollection<UserEmailHistory> UserEmailHistoryData { get; set; }
        public virtual ICollection<AspPreviousPasswords> PreviousPasswords { get; set; }
    }


   


    

   
}


//appIdentityUser_Id