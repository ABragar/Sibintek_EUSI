using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Base.Identity
{
    public class AccountInfo
    {
        public int? UserId { get; set; }
        public string Login { get; set; }

        public bool HasPassword { get; set; }

        public bool HasEmail { get; set; }
        public bool EmailConfirmed { get; set; }   

        public IList<UserLoginInfo> ExternalLogins { get; set; }

        public bool IsExist
        {
            get
            {
                return UserId.HasValue;
            }
        }
    }
}