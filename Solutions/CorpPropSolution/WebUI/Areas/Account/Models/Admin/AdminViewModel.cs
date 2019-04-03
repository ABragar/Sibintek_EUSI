using WebUI.Areas.Account.Models.Shared;

namespace WebUI.Areas.Account.Models.Admin
{
    public class AdminViewModel : BaseAccountViewModel
    {

        public bool CanDetachPassword { get; set; }

        public bool CanDatachProviders { get; set; }

        public bool CanAttachPassword { get; set; }

    }
}