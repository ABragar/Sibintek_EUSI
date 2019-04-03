using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Account.Models.Shared
{
    public class ExternalLoginActionModel: UserActionModel
    {
        [Required]
        public string LoginProvider { get; set; }
    }
}