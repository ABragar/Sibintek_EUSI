using Base.DAL;
using Base.DAL.EF;
using Base.Map.Entities;
using Data.EF;

namespace Data
{
    public class MapConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<MapLayerConfig>();
        }
    }
}