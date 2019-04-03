using Base;
using Base.BusinessProcesses.Entities;
using Base.Content.Entities;
using Base.DAL;
using Base.Enums;
using Base.FileStorage;
using Base.Help.Entities;
using Base.Notification.Entities;
using Base.Reporting.Entities;
using Base.Security;
using Base.Security.ObjectAccess;
using Base.Security.Service.Abstract;
using Base.Social.Entities.Components;
using Base.Support.Entities;
using Base.Task.Entities;
using Base.UI;
using Base.UI.Filter;
using Base.UI.Presets;
using Base.Word.Entities;
using CorpProp.Entities.Security;
using System.Collections.Generic;
using System.Linq;
using Task = System.Threading.Tasks.Task;
using RosreestrProject = CorpProp.RosReestr;
using AnalyzeProject = CorpProp.Analyze;

namespace Common.Data
{
    public static class UsersAndRolesInitializer
    {
        
        public static void Seed(IUnitOfWork unitOfWork, ILoginProvider loginProvider
            , Base.UI.Service.IPresetRegistorService presetRegistorService
            ,IDictionary<string, ICollection<PresetRegistor>> presets)
        {
            ////Базовые разрешения
            //List <Permission> DefaultPermission = new List<Permission>()
            //{
            //    //Базовый класс пользователя
            //    new Permission(typeof(User), TypePermission.Read),
            //    //Группы пользователей
            //    new Permission(typeof(UserCategory), TypePermission.Read),
            //    //Базовый класс профиля пользователя
            //    new Permission(typeof(BaseProfile)),
            //    //Класс профиля пользователя
            //    new Permission(typeof(SimpleProfile)),               
            //    //Телефоны профиля
            //    new Permission(typeof(ProfilePhone)),
            //    //Настройка Меню
            //    new Permission(typeof(MenuPreset)),
            //    new Permission(typeof(MenuElement)),
            //    new Permission(typeof(PresetRegistor), TypePermission.Read),
            //    //Виджеты
            //    new Permission(typeof(DashboardVm)),
            //    //Настройки Виджетов
            //    new Permission(typeof(DashboardPreset)),
            //    //Настройки табличного отображения
            //    new Permission(typeof(GridPreset)),
            //    //Фильтры
            //    new Permission(typeof(GridExtendedFilterPreset)),
            //    new Permission(typeof(ColumnExtendedFilterPreset)),
            //    //Настройки коллонок табличного отображения
            //    new Permission(typeof(ColumnPreset)),
            //    //Настройки панели кнопок
            //    new Permission(typeof(QuickAccessBarPreset)),
            //    new Permission(typeof(QuickAccessBarButton)),
            //    new Permission(typeof(Notification),
            //        TypePermission.Read | TypePermission.Create | TypePermission.Navigate |
            //        TypePermission.Write),
            //    //Конечный автомат
            //    new Permission(typeof(Workflow), TypePermission.Read),
            //    new Permission(typeof(ActionComment),
            //        TypePermission.Create | TypePermission.Read | TypePermission.Write),
            //    //Поддержка
            //    new Permission(typeof(HelpItem), TypePermission.Read | TypePermission.Navigate),
            //    new Permission(typeof(HelpItemTag), TypePermission.Read),
            //    new Permission(typeof(HelpItemTag), TypePermission.Read),
            //    //Контроль доступа
            //    new Permission(typeof(ObjectAccessItem)),
            //    //ПХД
            //    new Permission(typeof(FileStorageItem), TypePermission.Read | TypePermission.Navigate),
            //    new Permission(typeof(FileStorageCategory), TypePermission.Read),
            //    //Базовый класс для задач
            //    new Permission(typeof(BaseTaskCategory), TypePermission.Read),
            //    new Permission(typeof(Task),
            //        TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
            //        TypePermission.Write),
            //    //Базовый класс обращений по поддержке
            //    new Permission(typeof(BaseSupport),
            //        TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
            //        TypePermission.Write),
            //    new Permission(typeof(SupportRequest),
            //        TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
            //        TypePermission.Write),
            //    new Permission(typeof(SupportFile)),
            //    //Класс контента приложения
            //    new Permission(typeof(ContentCategory), TypePermission.Read),
            //    new Permission(typeof(ContentItem), TypePermission.Read),
            //    new Permission(typeof(Tag), TypePermission.Read),
            //    //Фильтры
            //    new Permission(typeof(MnemonicFilter), TypePermission.Read),
            //    new Permission(typeof(GlobalMnemonicFilter), TypePermission.Read),
            //    new Permission(typeof(UsersMnemonicFilter), TypePermission.Read | TypePermission.Navigate | TypePermission.Create | TypePermission.Write),
            //    //Комментарии
            //    new Permission(typeof(Сomments), TypePermission.Read | TypePermission.Navigate | TypePermission.Create | TypePermission.Write),
            //    //Шаблоны для печати
            //    new Permission(typeof(PrintingSettings), TypePermission.Read),
            //};
            //unitOfWork.GetRepository<Role>()
            //    .Create(new Role()
            //            {
            //                Name = "Base",
            //                Code = "Base",
            //                SystemRole = SystemRole.Base,
            //                Permissions = new List<Permission>(DefaultPermission) { },
            //            });

            //Оператор импорта данных Росреестра: ImportRosreestr
            var tExtractRole = CreateRole(unitOfWork, RosreestrProject.Initializer.GetExtractReadeAndWritePermissions(unitOfWork), "Оператор импорта данных Росреестра", "ImportRosreestr");
            CreatePresetForRole(unitOfWork, tExtractRole, "Оператор импорта данных Росреестра", "ImportRosreestr", presetRegistorService, presets);

            //Ответственный за передачу данных по бизнес-аналитике: ImportBusinessIntelligenceData
            var tAnalyzeRole = CreateRole(unitOfWork, AnalyzeProject.Initializer.GetAnalyzeReadeAndWritePermissions(unitOfWork), "Ответственный за передачу данных по бизнес-аналитике", "ImportBusinessIntelligenceData");
            CreatePresetForRole(unitOfWork, tAnalyzeRole, "Ответственный за передачу данных по бизнес-аналитике", "ImportBusinessIntelligenceData", presetRegistorService, presets);

            var userRepository = unitOfWork.GetRepository<User>();
            User admin = null;

            if (!userRepository.All().Any())
            {
                var adminCategory = new UserCategory()
                {
                    SysName = "admins",
                    Name = "Администратор приложения",                  
                    Roles = new List<Role>()
                    {
                        new Role()
                        {
                            Name = "Администратор приложения",
                            Code = "Admin",
                            SystemRole = SystemRole.Admin
                        }
                    }
                };

                if (presets.ContainsKey("admins"))
                    adminCategory.Presets =
                        presets["admins"].Select(x => new UserCategoryPreset() { Object = x }).ToList();

                unitOfWork.GetRepository<UserCategory>().Create(adminCategory);

                unitOfWork.SaveChanges();
                
                var user = new User
                {
                    SysName = "admin",
                    CategoryID = adminCategory.ID,
                };
                admin = unitOfWork.GetRepository<User>().Create(user);
                

                var publicMapCategory = new UserCategory()
                {
                    SysName = "public_map",
                    Name = "Публичная карта",
                    Roles = new List<Role>()                    
                };

                unitOfWork.GetRepository<UserCategory>().Create(publicMapCategory);

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

        /// <summary>
        /// Создание роли
        /// </summary>
        /// <param name="uow">Сессия</param>
        /// <param name="perms">Список разрешений</param>
        /// <param name="roleName">Имя роли</param>
        /// <param name="roleCode">Код роли</param>
        private static Role CreateRole(IUnitOfWork uow, List<Permission> perms, string roleName, string roleCode)
        {
            var tRole = uow.GetRepository<Base.Security.Role>()
                        .Filter(f => !f.Hidden && f.Name == roleName && f.Code == roleCode)
                        .FirstOrDefault();

            if (tRole == null)
            {
                var newRole = uow.GetRepository<Role>()
                 .Create(new Role()
                 {
                     Name = roleName,
                     Code = roleCode,
                     SystemRole = SystemRole.Base,
                     Permissions = new List<Permission>(perms) { },
                 });
                return newRole;
            }
            return tRole;
        }

        /// <summary>
        /// Создание Меню для роли
        /// </summary>
        /// <param name="uow">Сессия</param>
        /// <param name="roleName">Имя роли</param>
        /// <param name="roleCode">Код роли</param>
        private static void CreatePresetForRole(IUnitOfWork uow, Role tRole, string roleName, string roleCode
            , Base.UI.Service.IPresetRegistorService presetRegistorService
            , IDictionary<string, ICollection<PresetRegistor>> presets)
        {
            if (tRole == null)
            {
                tRole = uow.GetRepository<Base.Security.Role>()
                          .Filter(f => !f.Hidden && f.Name == roleName && f.Code == roleCode)
                          .FirstOrDefault();
            }

            if (tRole != null)
            {
                var userMenu = new CorpProp.DefaultData.MenuDefaultData(uow,presetRegistorService).CreateUserMenu(roleCode);
                var RolePreset = new PresetRegistor()
                {
                    Title = "Меню " + roleName,
                    Type = typeof(MenuPreset).GetTypeName(),
                    For = "Menu",
                    Preset = userMenu
                };

                presets.Add(roleCode, new List<PresetRegistor>() { presetRegistorService.Create(uow, RolePreset) });

                var baseCategory = new UserCategory()
                {
                    SysName = roleCode,
                    Name = roleName,
                    ProfileMnemonic = nameof(SibUser),
                    IsAccessible = false,
                    Roles = new List<Role>() { tRole }
                };

                //TODO: 
                baseCategory.Presets =
                          presets[roleCode].Select(x => new UserCategoryPreset() { Object = x }).ToList();

                uow.GetRepository<UserCategory>().Create(baseCategory);
            }
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Выписок Росреестра
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetExtractRosreestrReadePermissions()
        {
            return
                   new List<Permission>(CorpProp.UsersAndRolesInitializer.GetExtractReadePermissions())
                   {
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractObject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractSubj), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.AnotherSubject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.BaseParameter), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.BuildRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.CadNumber), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.CarParkingSpaceLocationInBuildPlans), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ContourOKSOut), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.DealRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.DocumentRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractBuild), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractLand), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractNZS), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractObject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractSubj), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Governance), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.IndividualSubject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.LandRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.LegalSubject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.NameRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Notice), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.NoticeSubj), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ObjectRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.OldNumber), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Organization), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.PermittedUse), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.PublicSubject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Refusal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RefusalSubj), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RestrictedRightsPartyOut), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RestrictRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightHolder), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightRecordNumber), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RoomLocationInBuildPlans), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.SubjectRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.SubjRight), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.TPerson), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateHistory), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateLog), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateState), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Выписок Росреестра
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetExtractRosreestrReadeAndWritePermissions()
        {
            return
                   new List<Permission>(CorpProp.UsersAndRolesInitializer.GetExtractReadeAndWritePermissions())
                   {
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractSubj), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.AnotherSubject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.BaseParameter), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.BuildRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.CadNumber), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.CarParkingSpaceLocationInBuildPlans), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ContourOKSOut), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.DealRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.DocumentRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractBuild), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractLand), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractNZS), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ExtractSubj), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Governance), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.IndividualSubject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.LandRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.LegalSubject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.NameRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Notice), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.NoticeSubj), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.ObjectRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.OldNumber), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Organization), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.PermittedUse), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.PublicSubject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.Refusal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RefusalSubj), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RestrictedRightsPartyOut), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RestrictRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightHolder), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RightRecordNumber), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.RoomLocationInBuildPlans), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.SubjectRecord), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.SubjRight), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Entities.TPerson), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateHistory), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateLog), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CorpProp.RosReestr.Migration.MigrateState), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                   };
        }



    }
}