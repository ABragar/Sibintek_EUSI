using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.DAL;
using Base.Extensions;
using Base.Identity.Entities;

namespace Base.Identity.OAuth2
{
    public class OAuthStore : IOAuthStore
    {

        private readonly IUnitOfWorkFactory _unit_of_work_factory;

        public OAuthStore(IUnitOfWorkFactory unit_of_work_factory)
        {
            _unit_of_work_factory = unit_of_work_factory;
        }

        public async Task<OAuthClient> FindClientAsync(string client_id)
        {

            using (var uow = _unit_of_work_factory.Create())
            {
                return await 
                    uow.GetRepository<OAuthClient>()
                        .All()
                        .Where(x => x.Enabled && client_id == x.ClientId)
                        .SingleOrDefaultAsync();

            }
            
        }

        public async Task<IReadOnlyCollection<string>> GetClientScopesAsync(string client_id)
        {
            using (var uow = _unit_of_work_factory.Create())
            {
                return await
                    uow.GetRepository<OAuthScope>()
                        .All()
                        .Where(x => x.Enabled && x.Clients.Any(c=>c.ClientId == client_id))
                        .Select(x=>x.Name).ToListAsync();

            }

        }
    }
}