using System.Web.Security;
using Base.Identity.Core;
using Base.Identity.Entities;
using Base.Identity.OAuth2;
using Base.Security.Service.Abstract;
using Base.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using SimpleInjector;

namespace WebUI.Bindings
{
    public class IdentityBindings
    {
        public static void Bind(Container container)
        {

            //container.Register<IUserInfoService>();
            container.Register<Base.Identity.Initializer>();
            container.Register<IAccountManager, AccountManager>(Lifestyle.Singleton);
            container.Register<ILoginProvider, DefaultLoginProvider>(Lifestyle.Singleton);
            container.Register<IUserManagerOptions, UserManagerOptions>(Lifestyle.Singleton);
            container.RegisterSingleton<IDataProtectionProvider>(() => new MachineKeyDataProtectionProvider());
            container.Register<IOAuthStore, OAuthStore>(Lifestyle.Singleton);
        }

        private class UserManagerOptions : IUserManagerOptions
        {
            public UserManagerOptions(IEmailService email_service, IUserInfoService user_info_service, IDataProtectionProvider provider)
            {
                EmailService = new EMailMessageService(email_service);
                UserInfoService = user_info_service;
                UserTokenProvider = new DataProtectorTokenProvider<AccountEntry, int>(provider.Create("user token provider"));
                CustomPasswordStorage = null;
            }

            public IUserTokenProvider<AccountEntry, int> UserTokenProvider { get; }
            public IIdentityMessageService EmailService { get; }
            public IUserInfoService UserInfoService { get; }
            public ICustomPasswordStorage CustomPasswordStorage { get; }
        }


        public class MachineKeyDataProtectionProvider : IDataProtectionProvider
        {
            public IDataProtector Create(params string[] purposes)
            {
                return new MachineKeyDataProtector(purposes);
            }
        }

        public class MachineKeyDataProtector : IDataProtector
        {
            private readonly string[] purposes;

            public MachineKeyDataProtector(params string[] purposes)
            {
                this.purposes = purposes;
            }

            public byte[] Protect(byte[] userData)
            {
                return MachineKey.Protect(userData, purposes);
            }

            public byte[] Unprotect(byte[] protectedData)
            {
                return MachineKey.Unprotect(protectedData, purposes);
            }
        }

    }


}