using System.Configuration;
using System.Security.Claims;
using System.Web.Configuration;
using RestService.Helpers;
using Telerik.Reporting.Services;

namespace RestService.Identity
{
    public interface IAuthStrategyFactory
    {
        IAuthUserInfoStrategy GetAuthUserInfoStrategy(ClaimsPrincipal principal);
    }

    public class AuthStrategyFactory : IAuthStrategyFactory
    {
        public IAuthUserInfoStrategy GetAuthUserInfoStrategy(ClaimsPrincipal principal)
        {
            AuthenticationSection section = ConfigurationManagerWrapper.GetAuthenticationSection();
            if (section != null && section.Mode == AuthenticationMode.Windows)
            {
                return new WinAuthIdentityStrategy();
            }
            else
            {
                return new OAuthIdentityStrategy();
            }
        }
    }
}