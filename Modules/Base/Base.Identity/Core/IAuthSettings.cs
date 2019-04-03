namespace Base.Identity.Core
{
    public interface IAuthSettings
    {
        bool ResetPasswordByTokenAllowed { get; }

        bool ExternalLoginAllowed { get; }

        bool RegistrationAllowed { get; }

        bool NotConfirmedLoginAllowed { get; }

        bool ConfirmAllowed { get; }
    }
}