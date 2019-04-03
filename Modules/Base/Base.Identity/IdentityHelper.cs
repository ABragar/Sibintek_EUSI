using System.Linq;
using System.Security.Claims;
using Base.Security;

namespace Base.Identity
{
    public static class IdentityHelper
    {
        public static ClaimsIdentity CreateIdentity(int user_id, string login, string type)
        {

            var claims = login == null ?
                new[] {
                    new Claim(ClaimTypes.NameIdentifier, user_id.ToString(), ClaimValueTypes.Integer),
                }
                : new[] {
                    new Claim(ClaimTypes.Name,login),
                    new Claim(ClaimTypes.NameIdentifier, user_id.ToString(), ClaimValueTypes.Integer),
                };

            return new ClaimsIdentity(claims, type);
        }

        public static ClaimsIdentity CreateIdentity(ISecurityUser user, string type)
        {
            var identity = CreateIdentity(user.ID, user.Login, type);

            if (user.IsAdmin)
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));

            if (user.ProfileInfo.FullName != null)
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.ProfileInfo.FullName));

            if (user.CategoryInfo != null)
            {


                var category = HCategory.IdToString(user.CategoryInfo.ID);

                if (user.CategoryInfo.SysAllParents != null)
                    category += HCategory.Seperator + user.CategoryInfo.SysAllParents;

                identity.AddClaim(new Claim("category", category));
            }



            return identity;
        }
    }
}