using Base;
using Base.DAL;
using Base.DAL.EF;
using Base.FileStorage;
using Base.Links.Entities;
using Base.Macros.Entities.Rules;
using Base.Security;
using Base.Security.Entities.Concrete;
using Base.Security.ObjectAccess;
using Base.Settings;
using Base.SysRegistry;
using Base.UI;
using Base.UI.DetailViewSetting;
using Base.UI.RegisterMnemonics.Entities;
using Data.EF;

namespace Data
{
    public class BaseConfig
    {
        public static void Init(EntityConfigurationBuilder config)
        {
            config.Context(EFRepositoryFactory<DataContext>.Instance)

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
                
                //ObjectType Entity
                .EntityObjectType<BaseObjectType>()
                ;
        }
    }
}