using Base.BusinessProcesses.Entities;
using Base.Content.Entities;
using Base.DAL;
using Base.Enums;
using Base.FileStorage;
using Base.Help.Entities;
using Base.Notification.Entities;
using Base.Security;
using Base.Security.ObjectAccess;
using Base.Security.Service.Abstract;
using Base.Support.Entities;
using Base.Task.Entities;
using Base.UI;
using Base.UI.Presets;
using CorpProp.Entities.Security;
using System.Collections.Generic;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace Data
{
    public static class UsersAndRolesInitializer
    {
        public static void Seed(IUnitOfWork unitOfWork, ILoginProvider loginProvider,
            IDictionary<string, ICollection<PresetRegistor>> presets)
        {
            var userRepository = unitOfWork.GetRepository<User>();

            User admin;

            if (!userRepository.All().Any())
            {
                var adminCategory = new UserCategory()
                {
                    SysName = "admins",
                    Name = "Администраторы",
                    ProfileMnemonic = nameof(SibUser),
                    Roles = new List<Role>()
                    {
                        new Role()
                        {
                            Name = "Admin",
                            SystemRole = SystemRole.Admin
                        }
                    }
                };

                if (presets.ContainsKey("admins"))
                    adminCategory.Presets =
                        presets["admins"].Select(x => new UserCategoryPreset() {Object = x}).ToList();

                unitOfWork.GetRepository<UserCategory>().Create(adminCategory);

                unitOfWork.SaveChanges();

                var baseCategory = new UserCategory()
                {
                    SysName = "users",
                    Name = "Пользователи",
                    ProfileMnemonic = nameof(SibUser),
                    IsAccessible = true,
                    Roles = new List<Role>()
                    {
                        new Role()
                        {
                            Name = "Base",
                            SystemRole = SystemRole.Base,
                            Permissions = new List<Permission>()
                            {
                                new Permission(typeof(User), TypePermission.Read),
                                new Permission(typeof(UserCategory), TypePermission.Read),
                                new Permission(typeof(BaseProfile)),
                                new Permission(typeof(SibUser)),
                                new Permission(typeof(ProfilePhone)),
                                new Permission(typeof(PresetRegistor), TypePermission.Read),
                                new Permission(typeof(DashboardVm)),
                                new Permission(typeof(DashboardPreset)),
                                new Permission(typeof(GridPreset)),
                                new Permission(typeof(ColumnPreset)),
                                new Permission(typeof(MenuPreset)),
                                new Permission(typeof(MenuElement)),
                                new Permission(typeof(QuickAccessBarPreset)),
                                new Permission(typeof(QuickAccessBarButton)),
                                new Permission(typeof(Notification),
                                    TypePermission.Read | TypePermission.Create | TypePermission.Navigate |
                                    TypePermission.Write),
                                new Permission(typeof(Workflow), TypePermission.Read),
                                new Permission(typeof(ActionComment),
                                    TypePermission.Create | TypePermission.Read | TypePermission.Write),
                                new Permission(typeof(HelpItem), TypePermission.Read | TypePermission.Navigate),
                                new Permission(typeof(HelpItemTag), TypePermission.Read),
                                new Permission(typeof(HelpItemTag), TypePermission.Read),
                                new Permission(typeof(ObjectAccessItem)),
                                new Permission(typeof(FileStorageItem), TypePermission.Read | TypePermission.Navigate),
                                new Permission(typeof(FileStorageCategory), TypePermission.Read),
                                new Permission(typeof(TaskCategory), TypePermission.Read),
                                new Permission(typeof(Task),
                                    TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
                                    TypePermission.Write),
                                new Permission(typeof(BaseSupport),
                                    TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
                                    TypePermission.Write),
                                new Permission(typeof(SupportRequest),
                                    TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
                                    TypePermission.Write),
                                //new Permission(typeof(SupportRequestWizard), TypePermission.Create),
                                //new Permission(typeof(SupportBoType), TypePermission.Read),
                                new Permission(typeof(SupportFile)),
                                new Permission(typeof(ContentCategory), TypePermission.Read),
                                new Permission(typeof(ContentItem), TypePermission.Read),
                                new Permission(typeof(Tag), TypePermission.Read),

                                ////TODO: тест, удалить
                                //new Permission(typeof(TestProfile)),
                            }
                        }
                    }
                };

                if (presets.ContainsKey("users"))
                    baseCategory.Presets = presets["users"].Select(x => new UserCategoryPreset() {Object = x}).ToList();

                unitOfWork.GetRepository<UserCategory>().Create(baseCategory);

                var user = new User
                {
                    SysName = "admin",
                    CategoryID = adminCategory.ID,                    
                };
                var profile = new SibUser() {LastName = "Администратор" , IsEmpty = false, User = user};
                var pr =unitOfWork.GetRepository<SibUser>().Create(profile);
                user.Profile = pr;

                admin = unitOfWork.GetRepository<User>().Create(user);

                unitOfWork.SaveChanges();

                var publicMapCategory = new UserCategory()
                {
                    SysName = "public_map",
                    Name = "Публичная карта",
                    Roles = new List<Role>(),
                    ProfileMnemonic = nameof(SibUser)
                };

                unitOfWork.GetRepository<UserCategory>().Create(publicMapCategory);

                var publicMapUser = new User
                {
                    SysName = "public_map",
                    CategoryID = publicMapCategory.ID,
                    Profile = null,
                };

                unitOfWork.GetRepository<User>().Create(publicMapUser);
                unitOfWork.SaveChanges();
            }
            else
            {
                admin = unitOfWork.GetRepository<User>().Find(x => x.SysName == "admin");
            }

            if (!loginProvider.Exist(unitOfWork, "admin"))
            {
                loginProvider.AttachSystemPassword(unitOfWork, admin.ID, "admin", "!QAZ2wsx");
            }
        }
    }
}