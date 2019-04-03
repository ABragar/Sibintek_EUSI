using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Account.Models.Shared
{
    public class AttachPasswordModel: UserActionModel
    {

        [Display(Name = Constants.PasswordName)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }


        [Display(Name = Constants.ConfirmPasswordName)]
        [DataType(DataType.Password)]
        [Required]
        [Compare(nameof(Password), ErrorMessage = Constants.ConfirmPasswordError)]
        public string ConfirmPassword { get; set; }
    }
}