using Base.DAL;
using Base.DAL.EF;
using Base.Multimedia.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class MultimediaConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<MultimediaObject>()
                .Entity<SourceFile>();
        }
    }
}