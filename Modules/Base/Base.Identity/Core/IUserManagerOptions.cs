using Base.Identity.Entities;
using Base.Security.Service.Abstract;
using Microsoft.AspNet.Identity;

namespace Base.Identity.Core
{
    public interface IUserManagerOptions
    {
        IUserTokenProvider<AccountEntry, int> UserTokenProvider { get; }
        IIdentityMessageService EmailService { get; }

        IUserInfoService UserInfoService { get; }

        ICustomPasswordStorage CustomPasswordStorage { get; }
    }
}