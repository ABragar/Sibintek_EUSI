using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Account.Models.Shared
{
    public class UserActionModel
    {
        [Required]
        public int UserId { get; set; }
    }
}