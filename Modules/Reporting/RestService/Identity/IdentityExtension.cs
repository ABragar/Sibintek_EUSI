using System.Security.Claims;

namespace RestService.Identity
{
    public static class IdentityExtension
    {
        public static UserInfo GetUserInfo(this ClaimsIdentity identity)
        {
            return new UserInfo()
            {
                UserId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value.Parse(),
                IsAdmin = identity.FindFirst(x => x.Type == ClaimTypes.Role && x.Value == "admin") != null,
                CategoryIds = identity.FindFirst(x => x.Type == "category").Value
            };
        }

        public static UserInfo GetUserInfo(this ClaimsPrincipal identity)
        {
            return new UserInfo()
            {
                UserId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value.Parse(),
                IsAdmin = identity.FindFirst(x => x.Type == ClaimTypes.Role && x.Value == "admin") != null,
                CategoryIds = identity.FindFirst(x => x.Type == "category").Value
            };
        }

        private static int? Parse(this string value)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;

            return null;
        }
    }
}