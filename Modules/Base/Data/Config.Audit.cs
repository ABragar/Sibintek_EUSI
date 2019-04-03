using Base;
using Base.Audit.Entities;
using Base.DAL;
using Base.DAL.EF;
using Data.EF;

namespace Data
{
    public class AuditConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)
                .Entity<AuditItem>()
                .Entity<DiffItem>()
                .Entity<AuditSetting>(c => c.Save(save => save.SaveOneToMany(x => x.Entities)))
                .Entity<AuditSettingEntity>();
        }
    }
}