using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Account.Models.Account
{
    public class ResetPasswordModel
    {

        [Required]
        public string Code { get; set; }


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