using Base.DAL;
using Base.DAL.EF;
using Base.Event.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class EventConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<Event>()
                .Entity<Participant>()
                .Entity<EventFile>()
                .Entity<Call>()
                .Entity<SimpleEvent>()
                .Entity<Meeting>();
        }
    }
}