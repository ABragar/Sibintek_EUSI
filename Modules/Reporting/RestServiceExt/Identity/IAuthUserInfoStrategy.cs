using System.Security.Claims;

namespace RestService.Identity
{
    public  interface IAuthUserInfoStrategy
    {
        UserInfo GetUserInfo(ClaimsPrincipal principal);
    }
}