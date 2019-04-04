using Base.DAL;
using Base.DAL.EF;
using Base.Social.Entities.Components;
using Common.Data.EF;

namespace Common.Data
{
    public class SocialConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Voiting>(x => x.Save(s => s.SaveOneObject(o => o.User)))
                .Entity<Сomments>(x => x.Save(s => s.SaveOneObject(u => u.User)));
        }
    }
}
