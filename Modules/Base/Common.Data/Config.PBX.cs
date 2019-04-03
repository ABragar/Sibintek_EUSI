using Base.DAL;
using Base.DAL.EF;
using Base.PBX.Entities;
using Common.Data.EF;

namespace Common.Data
{
    public class PBXConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<PBXServer>()
                .Entity<SIPAccount>();
        }
    }
}