using Base.Conference.Entities;
using Base.DAL;
using Base.DAL.EF;
using Common.Data.EF;

namespace Common.Data
{
    public class ConferenceConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Conference>()
                .Entity<ConferenceMember>()
                .Entity<ConferenceMessage>()
                .Entity<PublicMessage>()
                .Entity<PrivateMessage>()
                .Entity<ConferenceSetting>();
        }
    }
}