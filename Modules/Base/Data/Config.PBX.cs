using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.PBX.Entities;
using Data.EF;


namespace Data
{
    public class PBXConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<PBXServer>()
                .Entity<SIPAccount>();
        }
    }
}