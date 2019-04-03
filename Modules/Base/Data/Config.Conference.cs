using Base;
using Base.Conference.Entities;
using Base.DAL;
using Base.DAL.EF;
using Data.EF;

namespace Data
{
    public class ConferenceConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<Conference>()
                .Entity<ConferenceMember>()
                .Entity<ConferenceMessage>()
                .Entity<PublicMessage>()
                .Entity<PrivateMessage>()
                .Entity<ConferenceSetting>();
        }
    }
}