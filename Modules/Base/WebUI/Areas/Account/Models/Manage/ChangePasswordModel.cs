using System.ComponentModel.DataAnnotations;
using WebUI.Areas.Account.Models.Shared;

namespace WebUI.Areas.Account.Models.Manage
{
    public class ChangePasswordModel: UserActionModel
    {

        [Display(Name = Constants.CurrentPasswordName)]
        [DataType(DataType.Password)]
        [Required]
        public string CurrentPassword { get; set; }

        [Display(Name = Constants.NewPasswordName)]
        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }


        [Display(Name = Constants.ConfirmPasswordName)]
        [DataType(DataType.Password)]
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = Constants.ConfirmPasswordError)]
        public string ConfirmPassword { get; set; }
    }
}