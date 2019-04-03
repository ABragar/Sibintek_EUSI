using Base.DAL;
using Base.DAL.EF;
using Base.Identity;
using Base.Identity.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class IdentityConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<AccountEntry>()
                .Entity<ExternalLogin>()
                .Entity<AuthSettings>()
                .Entity<OAuthClient>(x => x.Save(s => s.SaveManyToMany(c => c.Scopes)))
                .Entity<OAuthScope>();
        }
    }
}