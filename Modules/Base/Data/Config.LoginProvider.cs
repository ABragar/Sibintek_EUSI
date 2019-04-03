using Base.DAL;
using Base.DAL.EF;
using Base.Identity;
using Base.Identity.Entities;
using Data.EF;

namespace Data
{
    public class IdentityConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<AccountEntry>()
                .Entity<ExternalLogin>()
                .Entity<AuthSettings>()
                .Entity<OAuthClient>(x => x.Save(s => s.SaveManyToMany(c => c.Scopes)))
                .Entity<OAuthScope>();
        }
    }
}