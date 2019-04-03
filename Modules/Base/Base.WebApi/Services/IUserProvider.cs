using System.Security.Claims;

namespace Base.WebApi.Services
{
    public interface IUserProvider
    {
        ClaimsPrincipal GetUser();
    }
}