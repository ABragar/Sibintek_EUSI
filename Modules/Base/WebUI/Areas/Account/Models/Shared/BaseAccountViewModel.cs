using System.Collections.Generic;

namespace WebUI.Areas.Account.Models.Shared
{
    public abstract class BaseAccountViewModel
    {
        public int UserId { get; set; }

        public string Login { get; set; }

        public bool HasPassword { get; set; }
        public IReadOnlyCollection<UserProviderInfo> UserProviders { get; set; }
    }
}