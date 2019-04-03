using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Microsoft.AspNet.Identity;

namespace Base.Identity.Entities
{
    public class AccountEntry : BaseObject, IUser<int>
    {
        public bool IsSystem { get; set; }
        public bool IsUser { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        int IUser<int>.Id => ID;

        [UniqueIndex("UserName")]
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }
        public virtual ICollection<ExternalLogin> ExternalLogins { get; set; } = new List<ExternalLogin>();


    }
}