using System;
using System.Security.Cryptography.X509Certificates;
using Base.Enums;

namespace Base.Security
{
    public interface ISecurityUser
    {
        int ID { get; }

        string Login { get; }
        bool IsAdmin { get; }
        UserProfileInfo ProfileInfo { get; }
        UserCategoryInfo CategoryInfo { get; }
        bool IsPermission(Type type, TypePermission typePermission);
        bool IsPermission<T>(TypePermission typePermission);
        bool IsRole(string role);
        bool IsRole(int roleID);
        bool IsSysRole(SystemRole sysrole);
        bool PropertyCanRead(Type type, string propertyName);
        bool PropertyCanWrite(Type type, string propertyName);
        string GetKey();

        //sib
        bool IsRoleCode(string roleCode);
        //end sib
    }

    public class UserProfileInfo
    {
        public int ID;
        public Guid ImageGuid;
        public string FullName;
        public string Email;
        public bool IsEmpty;
        public string Mnemonic;
    }

    public class UserCategoryInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SysAllParents { get; set; }
    }
}
