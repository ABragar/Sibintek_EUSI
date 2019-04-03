using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.PBX.Entities;
using Base.Social.Entities.Components;
using Data.EF;


namespace Data
{
    public class SocialConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<Voiting>(x => x.Save(s => s.SaveOneObject(o => o.User)))
                .Entity<Сomments>(x => x.Save(s => s.SaveOneObject(u => u.User)));
        }
    }
}
