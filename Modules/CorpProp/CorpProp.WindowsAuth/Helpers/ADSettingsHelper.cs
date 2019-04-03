using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CorpProp.WindowsAuth.Helpers
{
    public static class ADSettingsHelper
    {
        private const string AD_ADMIN_GROUPS = "ADAdminsGroups";
        private const string AD_USERS_GROUP = "ADUsersGroups";

        private static List<string> _adminsGroups;
        private static List<string> _usersGroups;

        public static bool IsWindowsAuthentification()
        {
            var authenticationSection = (AuthenticationSection)WebConfigurationManager.GetSection("system.web/authentication");
            return AuthenticationMode.Windows == authenticationSection.Mode;
        }

        public static List<string> GetADAdminsGroups()
        {
            if (_adminsGroups == null)
            {
                _adminsGroups = new List<string>();
                var adminsGroups = WebConfigurationManager.AppSettings[AD_ADMIN_GROUPS];
                if (!string.IsNullOrEmpty(adminsGroups))
                {
                    _adminsGroups.AddRange(adminsGroups.Split(','));
                }
            }
            return _adminsGroups;
        }

        public static List<string> GetADUsersGroups()
        {
            if (_usersGroups == null)
            {
                _usersGroups = new List<string>();
                var usersGroups = WebConfigurationManager.AppSettings[AD_USERS_GROUP];
                if (!string.IsNullOrEmpty(usersGroups))
                {
                    _usersGroups.AddRange(usersGroups.Split(','));
                }
            }
            return _usersGroups;
        }

        public static List<string> GetAllADUsersGroups()
        {
            return GetADAdminsGroups().Concat(GetADUsersGroups()).ToList();
        }
    }
}
