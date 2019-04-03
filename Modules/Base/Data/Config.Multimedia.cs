using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Multimedia.Entities;
using Data.EF;

namespace Data
{
    public class MultimediaConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<MultimediaObject>()
                .Entity<SourceFile>();
        }
    }
}