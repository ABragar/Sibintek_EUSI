namespace WebUI.Areas.Account.Models.Account
{
    public class LoginActionsViewModel
    {
        public bool RegistrationAllowed { get; set; }

        public bool ResetPasswordAllowed { get; set; }

        public bool AnyAllowed => RegistrationAllowed || ResetPasswordAllowed;
    }
}