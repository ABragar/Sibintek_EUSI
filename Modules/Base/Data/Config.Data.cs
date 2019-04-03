using Base;
using Base.DAL;
using Base.DAL.EF;
using Common.Data.EF;
using Data.EF;

namespace Data
{
    public class DataConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance);
            //Data
        }
    }
}