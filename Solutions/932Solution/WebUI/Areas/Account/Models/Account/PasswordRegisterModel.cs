using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Account.Models.Account
{
    public class PasswordRegisterModel
    {

        [Display(Name = Constants.EmailName)]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }


        [Display(Name = Constants.FirstNameName)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = Constants.LastNameName)]
        public string LastName { get; set; }

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