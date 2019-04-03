using System.ComponentModel.DataAnnotations;


namespace WebUI.Areas.Account.Models.Account
{
    public class PasswordLoginModel
    {
        
        [Required]
        [Display(Name = Constants.Login)]
        public string Login { get; set; }


        [Display(Name = Constants.PasswordName)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        
        public bool? RememberMe { get; set; }

        public bool? ValidateCaptcha { get; set; }


        public string CaptchaName { get; } = "password_login_catcha";
    }
}