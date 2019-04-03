using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Account.Models.Shared
{
    public class DetachExternalLoginModel : UserActionModel
    {


        [Required]
        public string LoginProvider { get; set; }

        [Required]
        public string ProviderKey { get; set; }

    }
}