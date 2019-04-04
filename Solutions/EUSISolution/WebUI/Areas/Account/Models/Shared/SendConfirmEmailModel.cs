using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebUI.Areas.Account.Models.Shared
{

    [Bind(Exclude = nameof(Message))]
    public class SendConfirmEmailModel
    {
        public string Message { get; set; }

        [Required]
        public string Login { get; set; }

        public string CaptchaName { get; } = "confirm_captcha";
    }
}