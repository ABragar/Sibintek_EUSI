using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.Account.Models.Account
{
    public class ExternalRegisterModel
    {

        [Required]
        public string LoginProvider { get; set; }

        [Required]
        public string ProviderKey { get; set; }


        [Display(Name = Constants.EmailName)]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }


        [Display(Name = Constants.FirstNameName)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = Constants.LastNameName)]
        public string LastName { get; set; }
    }
}