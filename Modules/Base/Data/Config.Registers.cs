using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.Entities;
using Data.EF;

namespace Data
{
    public class RegistersConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<Country>();
        }
    }
}