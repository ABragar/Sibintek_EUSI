using Base.Identity;
using Base.Security;
using Base.Security.Entities.Concrete;
using Base.Security.ObjectAccess.Services;
using Base.Security.Service;
using Base.Security.Service.Abstract;
using Base.Security.Service.Concrete;
using Base.Service;
using Base.Service.Log;
using SimpleInjector;
using WebUI.Concrete;

namespace WebUI.Bindings
{
    public class SecurityBindings
    {
        public static void Bind(Container container)
        {
            //Security
            container.Register<Base.Security.Initializer>();
            container.Register<IAccessUserService, AccessUserService>(Lifestyle.Singleton);
            container.Register<IAccessUserWizardService<AccessUserWizard>, AccessUserWizardService<AccessUserWizard>>();
            container.Register<ISecurityService, SecurityService>();
            container.Register<ILogService, LogService>(Lifestyle.Singleton);
            container.Register<ISecurityUserService, SecurityUserService>(Lifestyle.Singleton);
            container.Register<IUserInfoService, UserInfoService>(Lifestyle.Singleton);
            container.Register<IUserTokenService, UserTokenService>();

            //Security User
            container.Register<IUserStatusService, UserStatusService>(Lifestyle.Singleton);
            container.Register<IRoleService, RoleService>(Lifestyle.Singleton);
            container.Register<IUserService<User>, UserService>(Lifestyle.Singleton);
            container.Register<IBaseProfileService, BaseProfileService>(Lifestyle.Singleton);
            container.Register<ICrudProfileService<SimpleProfile>, CrudProfileService<SimpleProfile>>(Lifestyle.Singleton);
            container.Register<IUserCategoryService, UserCategoryService>(Lifestyle.Singleton);
            container.Register<IAccessErrorDescriber, AccessErrorDescriber>(Lifestyle.Singleton);


            //Security ObjectAccess
            container.Register<IObjectAccessItemService, ObjectAccessItemService>();
            container.Register<IUserAccessService, UserAccessService>();
            container.Register<IUserCategoryAccessService, UserCategoryAccessService>();
            container.Register<IAccessPolicyFactory, AccessPolicyFactory>();

            //Password
            container.Register<IPasswordService, PasswordService>(Lifestyle.Singleton);
        }
    }
}