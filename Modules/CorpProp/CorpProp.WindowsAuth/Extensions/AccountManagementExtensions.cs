using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.WindowsAuth.Extentions
{
    public static class AccountManagementExtensions
    {
        public static String GetProperty(this Principal principal, String property)
        {
            DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            if (directoryEntry.Properties.Contains(property))
            {
                return directoryEntry.Properties[property].Value.ToString();
            }

            return String.Empty;
        }

        public static String GetCompany(this Principal principal)
        {
            return principal.GetProperty("company");
        }

        public static String GetDepartment(this Principal principal)
        {
            return principal.GetProperty("department");
        }

        public static String GetPost(this Principal principal)
        {
            return principal.GetProperty("post");
        }

        public static String GetJobTitle(this Principal principal)
        {
            return principal.GetProperty("title");
        }

        public static String GetMobilePhone(this Principal principal)
        {
            return principal.GetProperty("mobile");
        }
    }
}
