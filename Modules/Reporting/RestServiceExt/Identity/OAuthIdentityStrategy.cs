using System.Security.Claims;

namespace RestService.Identity
{
    public class OAuthIdentityStrategy: IAuthUserInfoStrategy
    {
        public UserInfo GetUserInfo(ClaimsPrincipal principal)
        {
            return new UserInfo()
            {
                UserId = ParseToInt(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                IsAdmin = principal.FindFirst(x => x.Type == ClaimTypes.Role && x.Value == "admin") != null,
                CategoryIds = principal.FindFirst(x => x.Type == "category").Value
            };
        }
        private  int? ParseToInt(string value)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;

            return null;
        }
    }
}