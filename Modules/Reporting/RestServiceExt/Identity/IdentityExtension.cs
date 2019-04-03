using NLog;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using DAL.EF;

namespace RestService.Identity
{
    public static class IdentityExtension
    {
        private static ILogger _log = LogManager.GetCurrentClassLogger();
        public static IAuthUserInfoStrategy AuthUserInfoStrategy { get; set; }

        public static UserInfo GetUserInfo(this ClaimsPrincipal identity)
        {
            IAuthStrategyFactory authStrategyFactory = SimpleInjectorResolver.Container.GetInstance<IAuthStrategyFactory>();
            return authStrategyFactory.GetAuthUserInfoStrategy(identity).GetUserInfo(identity);
        }


    }
}