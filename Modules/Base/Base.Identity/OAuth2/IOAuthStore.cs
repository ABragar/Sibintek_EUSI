using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Identity.Entities;

namespace Base.Identity.OAuth2
{
    public interface IOAuthStore
    {
        Task<OAuthClient> FindClientAsync(string client_id);

        Task<IReadOnlyCollection<string>> GetClientScopesAsync(string client_id);
    }
}