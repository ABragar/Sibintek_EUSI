using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Event.Entities;
using Data.EF;

namespace Data
{
    public class EventConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<Event>()
                .Entity<Participant>()
                .Entity<EventFile>()
                .Entity<Call>()
                .Entity<SimpleEvent>()
                .Entity<Meeting>();
        }
    }
}