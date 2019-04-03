
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Account.Models.Account
{
    public class SendResetPasswordModel
    {

        [Required]
        [Display(Name = Constants.EmailName)]
        [DataType(DataType.EmailAddress)]
        public string Login { get; set; }

        public string CaptchaName { get; } = "send_reset_password_captcha";
    }
}