using System.Configuration;
using System.Web.Configuration;

namespace RestService.Helpers
{
    public static class ConfigurationManagerWrapper
    {

        public static AuthenticationSection GetAuthenticationSection()
        {
            return (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");
        }
    }
}