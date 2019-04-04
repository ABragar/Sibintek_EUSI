using System.Collections.Generic;
using WebUI.Areas.Account.Models.Shared;

namespace WebUI.Areas.Account.Models.Manage
{
    public class ManageViewModel: BaseAccountViewModel
    {
        public bool CanConfirmEmail { get; set; }

        public bool CanDetachPassword { get; set; }

        public bool CanDetachProvider { get; set; }

        public IReadOnlyCollection<ProviderInfo> CanAttachProviders { get; set; }

    }
}