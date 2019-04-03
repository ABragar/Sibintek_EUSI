using Base;
using Base.DAL;
using Base.Enums;
using Base.Security;
using Base.Security.Service.Abstract;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using CorpProp.Entities.Document;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using EUSI.Entities.Accounting;
using EUSI.Entities.Estate;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Links.Entities;
using EUSI.Entities.BSC;
using EUSI.Entities.NU;
using EUSI.Entities.Report;
using Base.Audit.Entities;
using EUSI.Entities.Mapping;
using EUSI.Entities.Audit;

namespace EUSI
{
    public class UsersAndRolesInitializer
    {
        public static void Seed(IUnitOfWork uow, ILoginProvider loginProvider, IPresetRegistorService presetRegistorService)
        {
            var presets = new Dictionary<string, ICollection<PresetRegistor>>();
            ModifyRoles(uow);
            //CreateRoles(uow);
            CreateRoles(uow, presetRegistorService, presets);

        }

        /// <summary>
        /// Обновление разрешений для уже существующих ролей.
        /// </summary>
        /// <param name="uow"></param>
        public static void ModifyRoles(IUnitOfWork uow)
        {
            var megaRole = uow.GetRepository<Base.Security.Role>()
                .Filter(f => !f.Hidden && f.Code == "MegaRole")
                .FirstOrDefault();
            if (megaRole != null)
            {               
                var types = System.Reflection.Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(w => !w.IsInterface && !w.IsAbstract && w.IsSubclassOf(typeof(BaseObject)))
                    .DefaultIfEmpty()
                    .ToList();

                foreach (var tt in types)
                {
                    megaRole.Permissions
                            .Add(new Permission(tt,
                            TypePermission.Read |
                            TypePermission.Write |
                            TypePermission.Create |
                            TypePermission.Delete |
                            TypePermission.Navigate));                    
                }
            }
        }

        /// <summary>
        /// Создание ролей для модуля ЕУСИ.
        /// </summary>
        /// <param name="uow"></param>
        public static void CreateRoles(IUnitOfWork uow
            , IPresetRegistorService presetRegistorService
            , IDictionary<string, ICollection<PresetRegistor>> presets)
        {

            //Супер Админ
            CreateRoleAdmin(uow, SystemRole.Admin, new List<Permission>() { }, "Неопределено ЕУСИ", "UnknownEUSI", presetRegistorService, presets);

            // Инициатор Заявки ЕУСИ: InitEstateRegistration
            CreateRole(uow, GetInitRolePerms(uow), "Инициатор Заявки ЕУСИ", "InitEstateRegistration", presetRegistorService, presets);

            // Ответственный за проверку Заявки ЕУСИ: ResponsibleER
            CreateRole(uow, GetCheckERPerms(uow), "Ответственный за проверку Заявки ЕУСИ", "ResponsibleER", presetRegistorService, presets);

            //Ответственный за загрузку Заявки ЕУСИ: ImportEstateRegistration
            CreateRole(uow, GetImportERPerms(uow), "Ответственный за загрузку Заявки ЕУСИ", "ImportEstateRegistration", presetRegistorService, presets);

            //Ответственный за выполнение контролей: ResponsibleReportControl
            CreateRole(uow, GetReportControlPerms(uow), "Ответственный за выполнение контролей", "ResponsibleReportControl", presetRegistorService, presets);

            //Ответственный за выгрузку в шаблоны параллельного учета: TransportMovings
            CreateRole(uow, GetTransportMovingsPerms(uow), "Ответственный за выгрузку в шаблоны параллельного учета", "TransportMovings", presetRegistorService, presets);
            
            //Ответственный за передачу данных(ОС/ НМА): ResponsibleTransportOS
            CreateRole(uow, GetTransportOSPerms(uow), "Ответственный за передачу данных(ОС/ НМА)", "ResponsibleTransportOS", presetRegistorService, presets);

            //Ответственный за дообогащение Карточки ОИ: ResponsibleRichEstate
            CreateRole(uow, GetResponsibleRichEstatePerms(uow), "Ответственный за дообогащение Карточки ОИ", "ResponsibleRichEstate", presetRegistorService, presets);
            
            //Ответственный за дообогащение Карточки ОС/НМА: ResponsibleRichOS
            CreateRole(uow, GetResponsibleRichOSPerms(uow), "Ответственный за дообогащение Карточки ОС/НМА", "ResponsibleRichOS", presetRegistorService, presets);
            
            //Ответственный за загрузку данных БУС: ResponsibleBUSDataImport
            CreateRole(uow, GetImportBUSDAtaPerms(uow), "Ответственный за загрузку данных БУС", "ResponsibleBUSDataImport", presetRegistorService, presets);

            //Ответственный за формирование карточки ИК: ResponsiblePropertyComplexIO
            CreateRole(uow, GetResponsibleIKPerms(uow), "Ответственный за формирование карточки ИК", "ResponsiblePropertyComplexIO", presetRegistorService, presets);

            //Ответственный за миграцию ОС: ResponsibleMigrateOS
            CreateRole(uow, GetMigrateOSPerms(uow), "Ответственный за миграцию ОС", "ResponsibleMigrateOS", presetRegistorService, presets);

            //Ответственный за импорт данных ИР Аренда: ResponsibleImportRentalOS
            CreateRole(uow, GetImportRentalOSPerms(uow), "Ответственный за импорт данных ИР Аренда", "ResponsibleImportRentalOS", presetRegistorService, presets);
        }


        #region Создание роли
        /// <summary>
        /// Создание роли
        /// </summary>
        /// <param name="uow">Сессия</param>
        /// <param name="perms">Список разрешений</param>
        /// <param name="roleName">Имя роли</param>
        /// <param name="roleCode">Код роли</param>
        private static void CreateRole(IUnitOfWork uow, List<Permission> perms, string roleName, string roleCode
            , IPresetRegistorService presetRegistorService
            , IDictionary<string, ICollection<PresetRegistor>> presets)
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

                var userMenu = new EUSI.DefaultData.MenuDefaultData(uow, presetRegistorService).CreateUserMenu(roleCode);

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
                    Roles = new List<Role>() { newRole }
                };


                //TODO: 
                baseCategory.Presets =
                          presets[roleCode].Select(x => new UserCategoryPreset() { Object = x }).ToList();

                uow.GetRepository<UserCategory>().Create(baseCategory);
            }
        }

        /// <summary>
        /// Создание роли для администраторов
        /// </summary>
        /// <param name="uow">Сессия</param>
        /// <param name="perms">Список разрешений</param>
        /// <param name="roleName">Имя роли</param>
        /// <param name="roleCode">Код роли</param>
        private static void CreateRoleAdmin(IUnitOfWork uow, SystemRole systemRole, List<Permission> perms, string roleName, string roleCode
            , IPresetRegistorService presetRegistorService
            , IDictionary<string, ICollection<PresetRegistor>> presets)
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
                     SystemRole = systemRole,
                     Permissions = new List<Permission>(perms) { },
                 });

                var userMenu = new EUSI.DefaultData.MenuDefaultData(uow, presetRegistorService).CreateAdminMenu(roleCode);
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
                    Roles = new List<Role>() { newRole }
                };


                //TODO: 
                baseCategory.Presets =
                          presets[roleCode].Select(x => new UserCategoryPreset() { Object = x }).ToList();

                uow.GetRepository<UserCategory>().Create(baseCategory);
            }
        }
        #endregion

        /// <summary>
        /// Предоставляет набор разрешений для Инициатора Заявки ЕУСИ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetInitRolePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission_RO());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateAccountingObject_Read());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetDocumentReadePermissions());
            perms.Add(new Permission(typeof(FileCardAndEstateRegistrationObject), TypePermission.Read | TypePermission.Navigate));

            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetBusinessProcesses_RW());
            perms.AddRange(EstateRegistration_RO());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за проверку Заявки ЕУСИ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetCheckERPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetDocumentReadeAndWritePermissions());
            perms.Add(new Permission(typeof(FileCardAndEstateRegistrationObject), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write));

            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission_RO());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateAccountingObject_Read());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetBusinessProcesses_RW());
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(GetAuditReadePerms());
            perms.AddRange(EstateRegistration_RW());

            foreach (var item in perms
                .Where(f => f.GetType() == typeof(Permission) && f.FullName == typeof(CorpProp.Entities.Accounting.AccountingObject).FullName))
            {
                uow.GetRepository<PropertyPermission>().Create(
                    new PropertyPermission()
                    {
                        PropertyName = "Comment",
                        AllowRead = true,
                        AllowWrite = true,
                    });
            }
                return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за загрузку Заявки ЕУСИ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetImportERPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetDocumentReadeAndWritePermissions());
            perms.Add(new Permission(typeof(FileCardAndEstateRegistrationObject), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetAccountingObjectReadeAndWritePermissions(uow));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetBusinessProcesses_RW());
            perms.AddRange(EstateRegistration_RW());
            perms.AddRange(GetAuditReadePerms());
            perms.AddRange(new[]
                    {
                            new Permission(typeof(Subject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(SibCountry), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(SibRegion), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(Society), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(FileCard), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(FileCardOne), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(CardFolder), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(CorpProp.Entities.Import.ImportHistory),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(CorpProp.Entities.Import.ImportErrorLog),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(ViewSettingsByMnemonic),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                            new Permission(typeof(ERTypeERReceiptReason),TypePermission.Read | TypePermission.Navigate),
                    });
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за формирование карточки ИК
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetResponsibleIKPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateAccountingObject_Read());
            return perms;
        }


        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за дообогащение Карточки ОИ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetResponsibleRichEstatePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateAccountingObject_Read());
            perms.Add(new Permission(typeof(Society), TypePermission.Read | TypePermission.Navigate));
            
            return SetOGPropPermissions(perms);
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за миграцию ОС
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetMigrateOSPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission(TypePermission.Read | TypePermission.Create | TypePermission.Write));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateOSPermission(TypePermission.Read | TypePermission.Create | TypePermission.Write));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetImportPermission(TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Navigate));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за импорт данных ИР Аренда
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetImportRentalOSPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission(TypePermission.Read | TypePermission.Create | TypePermission.Write));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateOSPermission(TypePermission.Read | TypePermission.Create | TypePermission.Write));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetImportPermission(TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Navigate));
            perms.AddRange(CreateRentalOSPermission_RW());
            return perms;
        }

        public static List<Permission> SetOGPropPermissions(List<Permission> tPerms)
        {
            var ogType =  typeof(Society).GetTypeName();
            var perms = tPerms.Where(f => f.FullName == ogType);
            foreach (var perm in perms)
            {
                perm.PropertyPermissions = new List<PropertyPermission>()
                {
                      new PropertyPermission(){ PropertyName = "ActualKindActivity", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "AddressLegalString", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "AggregateFinancialResultOfThePeriod", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BaseExclusionFromPerimeter", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BaseInclusionInPerimeter", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BCA", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BeneficialShareInCapital", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BeneficialShareInVotingRights", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BusinessDirection", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BusinessSegment", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BusinessBlock", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "ChA", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Curator", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Currency", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "DateExclusionFromGroup", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "DateExclusionFromPerimeter", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "DateInclusionInGroup", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "DateInclusionInPerimeter", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Description", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "DirectParticipantCount", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "DirectParticipantList", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "DirectShare", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "DirectShareInVotingRights", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "HeadName", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "HeadPosition", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Hidden", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "ImportDate", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "ImportUpdateDate", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsExclusionFromPerimeter", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsKOUControl", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsShareControl", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsSociety", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsSocietyControlled", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsSocietyJoint", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsSocietyKey", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsSocietyResident", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsSoleExecutiveBodyControl", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsSubjectMSP", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "KPP", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "KSK", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "NetProfit", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "OGRN", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "OGRNIP", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "OKATO", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "OKPO", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "OKVED", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "OPF", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "ProductionBlock", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "ResponsableForResponse", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SalesRevenue", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SDP", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "ShareInEquity", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "ShareInVotingRights", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SizeAuthorizedCapital", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SocietyPredecessorsCount", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SoleExecutiveBodyDateFrom", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SoleExecutiveBodyDateTo", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SortOrder", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SubjectKind", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "SubjectType", AllowRead = false, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "UnitOfCompany", AllowRead = false, AllowWrite = false }
                };
            }
            return tPerms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за дообогащение Карточки ОС/НМА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetResponsibleRichOSPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission_RO());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateOSPermission(TypePermission.Read | TypePermission.Write | TypePermission.Navigate));

            return SetOGPropPermissions(perms);
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за загрузку данных БУС
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetImportBUSDAtaPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission(TypePermission.Read | TypePermission.Create | TypePermission.Write));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateOSPermission(TypePermission.Read | TypePermission.Create | TypePermission.Write));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetImportPermission(TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Navigate));
            return perms;
        }


        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за выполнение контролей
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetReportControlPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission(TypePermission.Read | TypePermission.Navigate));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateOSPermission(TypePermission.Read | TypePermission.Navigate));
            perms.AddRange(CreateReportPermission(TypePermission.Read | TypePermission.Write | TypePermission.Navigate, "ResponsibleReportControl"));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetImportPermission(TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Navigate));
            perms.AddRange(new List<Permission>() { new Permission(typeof(BCSData), TypePermission.Create | TypePermission.Read | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate) });
            perms.AddRange(new List<Permission>() { new Permission(typeof(DraftOS), TypePermission.Read | TypePermission.Navigate) });
            perms.AddRange(CreateDeclarationPerms());
            perms.AddRange(CreateSaldoPerms());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за передачу данных (ОС/НМА)
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetTransportOSPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetEstatePermission(TypePermission.Read | TypePermission.Navigate));
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateOSPermission(TypePermission.Read | TypePermission.Navigate));
            perms.AddRange(CreateAccountingObject_Read());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetImportPermission(TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Navigate));
            perms.AddRange(new List<Permission>() { new Permission(typeof(DraftOS), TypePermission.Read | TypePermission.Navigate) });
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для Ответственного за выгрузку в шаблоны параллельного учета
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetTransportMovingsPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(CorpProp.UsersAndRolesInitializer.GetBasePermissions()) { };
            perms.AddRange(DefaultEUSIPermission_RW());
            perms.AddRange(CorpProp.UsersAndRolesInitializer.GetNSIReadAndWritePerms());
            perms.AddRange(CreateOSPermission(TypePermission.Read | TypePermission.Navigate));
            perms.AddRange(CreateAccountingObject_Read());
            perms.AddRange(new List<Permission>() {
                     new Permission(typeof(EUSI.Entities.Accounting.ExportMoving), TypePermission.Create | TypePermission.Read | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate)
                   , new Permission(typeof(EUSI.Entities.Accounting.AccountingMoving), TypePermission.Read | TypePermission.Write | TypePermission.Navigate)
                   , new Permission(typeof(EUSI.Entities.Accounting.AccountingMovingMSFO), TypePermission.Read | TypePermission.Write | TypePermission.Navigate)
            });

            return perms;
        }

        

        public static IEnumerable<Permission> CreateReportPermission(TypePermission tp, string roleCode)
        {
            List<Permission> perms = new List<Permission>();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(w => !w.IsAbstract && !w.IsInterface && ( w.FullName.Contains("Entities.Report") || w.FullName.Contains("Reporting.Entities")))
                .DefaultIfEmpty()
                .ToList();

            foreach (var item in types)
            {
                var perm = new Permission(item, tp);
                if (item.Equals(typeof(ReportMonitoring)))
                {
                    perm.PropertyPermissions = new List<PropertyPermission>()
                    {
                          new PropertyPermission(){PropertyName ="ImportDateTime", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="ReportName", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="ReportCode", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="SibUser", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="Consolidation", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="StartDate", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="EndDate", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="IsValid", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="ResultText", AllowRead = true, AllowWrite = false }
                        , new PropertyPermission(){PropertyName ="Comment", AllowRead = true, AllowWrite = (roleCode == "ResponsibleReportControl") }
                    };
                }

                perms.Add(perm);
            }
            return perms;
        }

        public static IEnumerable<Permission> CreateOSPermission(TypePermission tp, string roleCode = null)
        {
            List<Permission> list = new List<Permission>();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())                
                .Where(w => !w.IsAbstract && !w.IsInterface && w.FullName.Contains("Entities.Accounting"))
                .DefaultIfEmpty()
                .ToList();

            foreach (var item in types)
            {
                var perm = new Permission(item, tp);
                if (item.Equals(typeof(CorpProp.Entities.Accounting.AccountingObject)))
                {
                    if (perm.PropertyPermissions != null)
                    {
                        perm.PropertyPermissions.Add(new PropertyPermission()
                        {
                            PropertyName = "Comment",
                            AllowRead = true,
                            AllowWrite = (roleCode == "ResponsibleER") ? true : false,
                        });
                    }
                    else
                    {
                        perm.PropertyPermissions = new List<PropertyPermission>() {new PropertyPermission()
                        {
                            PropertyName = "Comment",
                            AllowRead = true,
                            AllowWrite = (roleCode == "ResponsibleER") ? true : false,
                        }};
                    }
                }
                list.Add(perm);
            }
            return list;
        }

        public static IEnumerable<Permission> CreateRentalOSPermission_RW()
        {
            return
                new List<Permission>()
                {
                    new Permission(typeof(RentalOS), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(RentalOSMoving), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(RentalOSState), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Navigate),
                };
        }

        public static IEnumerable<Permission> CreateRentalOSPermission__Read()
        {
            return
                new List<Permission>()
                {
                    new Permission(typeof(RentalOS), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RentalOSMoving), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RentalOSState), TypePermission.Read | TypePermission.Navigate),
                };
        }


        private static List<Permission> GetAuditReadePerms()
        {
            return
                new List<Permission>()
                {
                    new Permission(typeof(AuditItem), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(DiffItem), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CustomDiffItem), TypePermission.Read | TypePermission.Navigate),
                };
        }

        private static List<Permission> CreateAccountingObject_RW()
        {
            return
                new List<Permission>()
                {
                    new Permission(typeof(CorpProp.Entities.Accounting.AccountingObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(AccountingMoving), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(AccountingMovingMSFO), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(EstateRegistration), TypePermission.Read | TypePermission.Navigate),

                    new Permission(typeof(Subject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Society), TypePermission.Read  | TypePermission.Navigate),

                    new Permission(typeof(SibCountry), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                    new Permission(typeof(SibRegion), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),

                    new Permission(typeof(FileCard), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                    new Permission(typeof(FileCardOne), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                    new Permission(typeof(CardFolder), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                };
        }

        private static List<Permission> CreateAccountingObject_Read()
        {
            return
                new List<Permission>()
                {
                    new Permission(typeof(CorpProp.Entities.Accounting.AccountingObject), TypePermission.Read| TypePermission.Navigate),
                    new Permission(typeof(AccountingMoving), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(AccountingMovingMSFO), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(EstateRegistration), TypePermission.Read | TypePermission.Navigate),

                    new Permission(typeof(Subject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Society), TypePermission.Read  | TypePermission.Navigate),

                    new Permission(typeof(SibCountry), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                    new Permission(typeof(SibRegion), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                   
                    new Permission(typeof(FileCard), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                    new Permission(typeof(FileCardOne), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                    new Permission(typeof(CardFolder), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                };
        }

        private static List<Permission> EstateRegistration_RW()
        {
            return
                new List<Permission>()
                {
                    //new Permission(typeof(AccountingObjectExt), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),

                    new Permission(typeof(EstateRegistration), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(EstateRegistrationRow), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(Subject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibCountry), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibRegion), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(Society), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(FileCard), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(FileCardOne), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(FileCardMany), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(CardFolder), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(ERTypeERReceiptReason),TypePermission.Read | TypePermission.Navigate),
                };
        }
        private static List<Permission> EstateRegistration_RO()
        {
            return
                new List<Permission>()
                {
                    new Permission(typeof(EstateRegistration), TypePermission.Read | TypePermission.Navigate)
                    , new Permission(typeof(EstateRegistrationRow), TypePermission.Read | TypePermission.Navigate)
                    , new Permission(typeof(ERTypeERReceiptReason),TypePermission.Read | TypePermission.Navigate)
                    , new Permission(typeof(FileCardAndEstateRegistrationObject), TypePermission.Read | TypePermission.Navigate)
                    , new Permission(typeof(EstateAndEstateRegistrationObject), TypePermission.Read | TypePermission.Navigate)
                    , new Permission(typeof(AccountingObjectAndEstateRegistrationObject), TypePermission.Read | TypePermission.Navigate)
                };
        }

        public static List<Permission> DefaultEUSIPermission_RW()
        {
            return new List<Permission>()
            {
                
                new Permission(typeof(AddonOKOF2014), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(Deposit), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(DepreciationMethodRSBU), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)

                , new Permission(typeof(EstateDefinitionType), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(EstateMovableNSI), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(EstateRegistrationStateNSI), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(EstateRegistrationTypeNSI), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(OKOFClassNSI), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(RentTypeRSBU), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(SibCityNSI), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(StateObjectRSBU), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(StructurePlan), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)

                , new Permission(typeof(FileCardAndEstateRegistrationObject), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(EstateAndEstateRegistrationObject), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(AccountingObjectAndEstateRegistrationObject), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(AddonOKOF2014), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                , new Permission(typeof(LinkItem), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)

            };

        }


        //public static List<Permission> DefaultBasePermission()
        //{
        //    return new List<Permission>()
        //    {

        //         new Permission(typeof(MenuPreset), TypePermission.Navigate | TypePermission.Read)
        //        , new Permission(typeof(PresetRegistor), TypePermission.Navigate | TypePermission.Read)
        //        , new Permission(typeof(ViewSettingsByMnemonic), TypePermission.Read)
        //        , new Permission(typeof(GridPreset), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate)

        //        , new Permission(typeof(Base.UI.Filter.UsersMnemonicFilter), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate)
        //        , new Permission(typeof(Base.UI.Filter.MnemonicFilter), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate)
        //        , new Permission(typeof(Base.UI.Filter.GlobalMnemonicFilter), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate)
        //        , new Permission(typeof(Base.UI.Presets.ColumnExtendedFilterPreset), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate)
        //        , new Permission(typeof(Base.UI.Presets.GridExtendedFilterPreset), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate)
        //        , new Permission(typeof(Base.UI.RegisterMnemonics.Entities.ListViewFilterEx), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate)


        //        , new Permission(typeof(Society), TypePermission.Navigate | TypePermission.Read)
        //        , new Permission(typeof(NSI), TypePermission.Navigate | TypePermission.Read)               
        //        , new Permission(typeof(SibUser), TypePermission.Navigate | TypePermission.Read)

        //        , new Permission(typeof(FileCard), TypePermission.Navigate | TypePermission.Read |TypePermission.Write | TypePermission.Create | TypePermission.Delete)
        //        , new Permission(typeof(FileCardOne), TypePermission.Navigate | TypePermission.Read |TypePermission.Write | TypePermission.Create | TypePermission.Delete)
        //        , new Permission(typeof(FileCardMany), TypePermission.Navigate | TypePermission.Read |TypePermission.Write | TypePermission.Create | TypePermission.Delete)
        //        , new Permission(typeof(CardFolder), TypePermission.Navigate | TypePermission.Read |TypePermission.Write | TypePermission.Create | TypePermission.Delete)

        //    };

        //}


        /// <summary>
        /// Разрешения для расчетов и НА.
        /// </summary>
        /// <returns></returns>
        public static List<Permission> CreateDeclarationPerms()
        {
            var perms = new List<Permission>()
            {
                  new Permission(typeof(Declaration), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                 , new Permission(typeof(DeclarationCalcEstate), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                 , new Permission(typeof(DeclarationEstate), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                 , new Permission(typeof(DeclarationLand), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                 , new Permission(typeof(DeclarationRow), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                 , new Permission(typeof(DeclarationVehicle), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                 , new Permission(typeof(AccountingCalculatedField), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                 , new Permission(typeof(CalculatingRecord), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                 , new Permission(typeof(CalculatingError), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)

            };
            return perms;
        }

        /// <summary>
        /// Разрешения для протоколов сверки сальдо.
        /// </summary>
        /// <returns></returns>
        public static List<Permission> CreateSaldoPerms()
        {
            var perms = new List<Permission>()
            {
                  new Permission(typeof(BalanceReconciliationReport), TypePermission.Create | TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write)
                
            };
            return perms;
        }
    }

}
