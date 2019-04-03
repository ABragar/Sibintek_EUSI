using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.FileStorage;
using Base.Identity.Entities;
using Base.Security;
using Base.Security.Entities.Concrete;
using Base.Security.ObjectAccess;
using Base.Settings;
using Base.SysRegistry;
using Base.UI;
using Base.UI.Presets;
using Common.Data.EF;

namespace Common.Data
{
    public class BaseConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)

                //Security
                .Entity<User>()
                .Entity<BaseProfile>()
                .Entity<SimpleProfile>()
                .Entity<ProfilePhone>()
                .Entity<ProfileEmail>()
                .Entity<UserCategory>()
                .Entity<UserCategoryPreset>()
                .Entity<Role>()
                .Entity<Permission>()
                .Entity<ProfileAccess>()
                .Entity<UserToken>(u => u.Save(s => s.SaveOneObject(x => x.User)))

                //ObjectAccess
                .Entity<ObjectAccessItem>()
                .Entity<UserAccess>()
                .Entity<UserCategoryAccess>()

                //Settings
                .Entity<SettingItem>()
                .Entity<ImageSetting>()

                //FileStorage
                .Entity<FileData>()
                .Entity<FileStorageItem>()
                .Entity<FileStorageCategory>()
                .Entity<SystemRegistryItem>()
                .Entity<GridExtendedFilterPreset>(builder => builder.Save(saver => saver.SaveOneToMany(preset => preset.Columns)))
                .Entity<ColumnExtendedFilterPreset>();
        }
    }
}