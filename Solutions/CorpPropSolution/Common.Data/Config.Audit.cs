using System.Data.Entity;
using Base.Audit.Entities;
using Base.DAL;
using Base.DAL.EF;
using Common.Data.EF;

namespace Common.Data
{
    public class AuditConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                .Entity<AuditItem>()
                .Entity<DiffItem>()
                .Entity<AuditSetting>(c => c.Save(save => save.SaveOneToMany(x => x.Entities)))
                .Entity<AuditSettingEntity>()
                .Entity<AuditAuthResult>()
                .Entity<AuditRegisterResult>();
        }
    }
}