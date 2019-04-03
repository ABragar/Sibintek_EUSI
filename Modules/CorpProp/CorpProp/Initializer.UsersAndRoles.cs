using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Base.UI.Service;
using Base.Word.Entities;
using CorpProp.Entities;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Request;
using CorpProp.Entities.Request.ResponseCells;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using Base.Extensions;
using Base.Links.Entities;

namespace CorpProp
{
    public class UsersAndRolesInitializer
    {
        public static void Seed(IUnitOfWork uow, ILoginProvider loginProvider, IPresetRegistorService presetRegistorService)
        {
            var presets = new Dictionary<string, ICollection<PresetRegistor>>();
            CreateMenu(uow, presetRegistorService, presets);
            CreateRoles(uow, presetRegistorService, presets);
            CreateUserCategorys(uow, loginProvider, presets);
        }

        private static IList<Type> GetAssemblyTypes(System.Reflection.Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (System.Reflection.ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null).ToList();
            }
        }
        #region Получение набора разрешений
        /// <summary>
        /// Предоставляет набор основных базовых разрешений
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetBasePermissions()
        {
            //Базовые разрешения
            List<Permission> DefaultPermission = new List<Permission>()
                {
                    new Permission(typeof(LinkItem), TypePermission.Create |TypePermission.Delete | TypePermission.Navigate | TypePermission.Read | TypePermission.Write),
                    //Базовый класс пользователя
                    new Permission(typeof(User), TypePermission.Read),
                    //Группы пользователей
                    new Permission(typeof(UserCategory), TypePermission.Read),
                    //Базовый класс профиля пользователя
                    new Permission(typeof(BaseProfile)),
                    new Permission(typeof(SibUser)),
                    //Класс профиля пользователя
                    new Permission(typeof(SimpleProfile)),               
                    //Телефоны профиля
                    new Permission(typeof(ProfilePhone)),
                    //Настройка Меню
                    new Permission(typeof(MenuPreset)),
                    new Permission(typeof(MenuElement)),
                    new Permission(typeof(PresetRegistor), TypePermission.Read),
                    //Виджеты
                    new Permission(typeof(DashboardVm)),
                    //Настройки Виджетов
                    new Permission(typeof(DashboardPreset)),
                    //Настройки табличного отображения
                    new Permission(typeof(GridPreset)),
                    new Permission(typeof(Base.UI.RegisterMnemonics.Entities.ListViewFilterEx), TypePermission.Read | TypePermission.Create | TypePermission.Write | TypePermission.Delete | TypePermission.Navigate),
                    //Фильтры
                    new Permission(typeof(GridExtendedFilterPreset)),
                    new Permission(typeof(ColumnExtendedFilterPreset)),
                    //Настройки коллонок табличного отображения
                    new Permission(typeof(ColumnPreset)),
                    //Настройки панели кнопок
                    new Permission(typeof(QuickAccessBarPreset)),
                    new Permission(typeof(QuickAccessBarButton)),
                    new Permission(typeof(Notification),
                        TypePermission.Read | TypePermission.Create | TypePermission.Navigate |
                        TypePermission.Write),
                    //Конечный автомат
                    new Permission(typeof(Workflow), TypePermission.Read),
                    new Permission(typeof(ActionComment),
                        TypePermission.Create | TypePermission.Read | TypePermission.Write),
                    //Поддержка
                    new Permission(typeof(HelpItem), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(HelpItemTag), TypePermission.Read),
                    new Permission(typeof(HelpItemTag), TypePermission.Read),
                    //Контроль доступа
                    new Permission(typeof(ObjectAccessItem)),
                    //ПХД
                    new Permission(typeof(FileStorageItem), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileStorageCategory), TypePermission.Read),
                    new Permission(typeof(ViewSettingsByMnemonic),TypePermission.Read),
                    //Базовый класс для задач
                    new Permission(typeof(BaseTaskCategory), TypePermission.Read),
                    new Permission(typeof(System.Threading.Tasks.Task),
                        TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
                        TypePermission.Write),
                    //Базовый класс обращений по поддержке
                    new Permission(typeof(BaseSupport),
                        TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
                        TypePermission.Write),
                    new Permission(typeof(SupportRequest),
                        TypePermission.Read | TypePermission.Navigate | TypePermission.Create |
                        TypePermission.Write),
                    new Permission(typeof(SupportFile)),
                    //Класс контента приложения
                    new Permission(typeof(ContentCategory), TypePermission.Read),
                    new Permission(typeof(ContentItem), TypePermission.Read),
                    new Permission(typeof(Tag), TypePermission.Read),
                    //Фильтры
                    new Permission(typeof(MnemonicFilter), TypePermission.Read),
                    new Permission(typeof(GlobalMnemonicFilter), TypePermission.Read),
                    new Permission(typeof(UsersMnemonicFilter), TypePermission.Read | TypePermission.Navigate | TypePermission.Create | TypePermission.Write),
                    //Комментарии
                    new Permission(typeof(Сomments), TypePermission.Read | TypePermission.Navigate | TypePermission.Create | TypePermission.Write),
                    //Шаблоны для печати
                    new Permission(typeof(PrintingSettings), TypePermission.Read),
                };
            return DefaultPermission;
        }

        /// <summary>
        /// Предоставляет набор разрешений для импорта
        /// </summary>
        /// <returns>Permission</returns>
        /// <param name="tp">Назначение типов разрешений</param>
        public static IEnumerable<Permission> GetImportPermission(TypePermission tp)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(w => !w.IsAbstract && !w.IsInterface && w.FullName.Contains("Entities.Import"))
                .DefaultIfEmpty()
                .ToList();

            foreach (var item in types)
            {
                yield return new Permission(item, tp);
            }
        }

        /// <summary>
        /// Предоставляет набор типов НСИ
        /// </summary>
        /// <returns>Type</returns>
        private static List<Type> GetNSITypes()
        {
            return (from assemblyType in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                    where assemblyType.IsSubclassOf(typeof(DictObject)) || assemblyType.Equals(typeof(DictObject)) || assemblyType.Equals(typeof(HDictObject)) || assemblyType.IsSubclassOf(typeof(HDictObject))
                    select assemblyType).ToList();
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения НСИ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetNSIReadAndOpenPerms()
        {
            var perms = new List<Permission>() { };

            foreach (var item in GetNSITypes())
            {
                if(item != typeof(ScheduleStateYear) && item != typeof(NonCoreAssetInventory))
                perms.Add(new Permission(item, TypePermission.Read | TypePermission.Navigate));
            }
            perms.Add(new Permission(typeof(CorpProp.Entities.NSI.NSI), TypePermission.Read | TypePermission.Navigate));
            perms.Add(new Permission(typeof(DictObject), TypePermission.Read));
            perms.Add(new Permission(typeof(HDictObject), TypePermission.Read));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения ПХД
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetDocumentReadePermissions()
        {
            return
                   new List<Permission>() {                            
                    new Permission(typeof(FileCard),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardOne),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardMany),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardOneAndFileCardMany),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CardFolder),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileDB),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileDataHelper),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardPermission),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(AccessModifier),TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования ПХД
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetDocumentReadeAndWritePermissions()
        {
            return
                   new List<Permission>() {
                    new Permission(typeof(FileCard),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FileCardOne),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FileCardMany),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FileCardOneAndFileCardMany),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CardFolder),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FileDB),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FileDataHelper),TypePermission.Read | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FileCardPermission),TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(AccessModifier),TypePermission.Read | TypePermission.Create | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Правопредшественников
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetPredecessorReadeAndWritePermissions(IUnitOfWork uow)
        {
            return
                   new List<Permission>(GetSocietyOnlyAndWritePermissions(uow)) {
                    new Permission(typeof(Predecessor), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения ОГ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetSocietyOnlyReadePermissions(IUnitOfWork uow)
        {
            var society = new Permission(typeof(Society), TypePermission.Read | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(society, TypePermission.Read, @"(IDEUP == @userIDEUP ) || @isFromCauk || ( @userTerritory.Contains(IDEUP) )  || (@userBelows.Contains(IDEUP))"));

            return
                   new List<Permission>() {
                    society
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и сохранения ОГ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetSocietyOnlyAndWritePermissions(IUnitOfWork uow)
        {
            var society = new Permission(typeof(Society), TypePermission.Read | TypePermission.Navigate | TypePermission.Write);
        //разерешения на экземпляры
        uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(society, TypePermission.Read, @"(IDEUP == @userIDEUP ) || @isFromCauk || ( @userTerritory.Contains(IDEUP) )  || (@userBelows.Contains(IDEUP))"));

            return
                   new List<Permission>() {
                    society,
                   };
}

/// <summary>
/// Предоставляет набор разрешений для чтения ОГ
/// </summary>
/// <returns>Permission</returns>
public static List<Permission> GetSocietyReadePermissions(IUnitOfWork uow)
        {
            return
                   new List<Permission>(GetSocietyOnlyReadePermissions(uow)) {
                    new Permission(typeof(Predecessor), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SocietyAgents), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SocietySubsidiaries), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования ОГ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetSocietyReadeAndWritePermissions(IUnitOfWork uow)
        {
            var society = new Permission(typeof(Society), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(society, TypePermission.Read, @"(IDEUP == @userIDEUP ) || @isFromCauk || ( @userTerritory.Contains(IDEUP) )  || (@userBelows.Contains(IDEUP))"));
            return
                   new List<Permission>() {
                       society,
                        new Permission(typeof(Predecessor), TypePermission.Read | TypePermission.Navigate),
                        new Permission(typeof(SocietyAgents), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                        new Permission(typeof(SocietySubsidiaries), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Деловых партнеров
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetSubjectReadePermissions()
        {
            return
                   new List<Permission>() {
                    new Permission(typeof(BankingDetail), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Subject), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Деловых партнеров
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetSubjectReadeAndWritePermissions()
        {
            return
                   new List<Permission>() {
                    new Permission(typeof(BankingDetail), TypePermission.Read | TypePermission.Write | TypePermission.Create  | TypePermission.Navigate),
                    new Permission(typeof(Subject), TypePermission.Read | TypePermission.Write | TypePermission.Create  | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Проектов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetProjectReadePermissions()
        {
            return
                   new List<Permission>() {
                    new Permission(typeof(SibProject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibProjectTemplate), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndSibProject), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Проектов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetProjectReadeAndWritePermissions()
        {
            return
                   new List<Permission>() {
                    new Permission(typeof(SibProject), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(SibProjectTemplate), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(FileCardAndSibProject), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Задач проектов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetTaskReadePermissions()
        {
            return
                   new List<Permission>() {
                    new Permission(typeof(SibTask), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskGanttDependency), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReport), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskTemplate), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(BaseTask), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(BaseTaskDependency), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndAppraisal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndDeal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndEstate), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndFileCard), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndRight), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndSibUser), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndAppraisal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndDeal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndEstate), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndFileCard), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndRight), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Задач проектов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetTaskReadeAndWritePermissions()
        {
            return
                   new List<Permission>() {
                    new Permission(typeof(SibTask), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskGanttDependency), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReport), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskTemplate), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(BaseTask), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(BaseTaskDependency), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndAppraisal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndDeal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndEstate), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndFileCard), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndRight), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndSibUser), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndAppraisal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndDeal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndEstate), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndFileCard), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndRight), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Запросов в ОГ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetRequestReadePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(FileCardAndRequest), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndRequestContent), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RequestAndSociety), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Request), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RequestAndSibUserManyToManyAssociation), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RequestColumn), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RequestColumnItems), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RequestContent), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RequestRow), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RequestTemplate), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RequestDynamicType),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(IRequest),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(IRequestColumn),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(IRequestRow),TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения Запросов в ОГ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetRequestReadeAndWritePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(FileCardAndRequest), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndRequestContent), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(RequestAndSociety), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(Request), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(RequestAndSibUserManyToManyAssociation), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(RequestColumn), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(RequestColumnItems), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(RequestContent), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(RequestRow), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(RequestTemplate), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(RequestDynamicType), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(IRequest), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(IRequestColumn), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(IRequestRow), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
        };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Ответов на запросы в ОГ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetResponseReadePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(Response), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellBoolean), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellDateTime), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellDecimal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellDict), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellDouble), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellFloat), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellInt), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellString), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseRow), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndResponse), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseAndSibUser), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ResponseGridEditor), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(IResponse),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(IResponseColumn), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Ответов на запросы в ОГ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetResponseReadeAndWritePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(Response), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellBoolean), TypePermission.Read | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellDateTime), TypePermission.Read | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellDecimal), TypePermission.Read | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellDict), TypePermission.Read | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellDouble), TypePermission.Read | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellFloat), TypePermission.Read | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellInt), TypePermission.Read | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(ResponseCellString), TypePermission.Read | TypePermission.Write | TypePermission.Navigate),
                    new Permission(typeof(ResponseRow), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndResponse), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(ResponseAndSibUser), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(ResponseGridEditor), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(IResponse),TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(IResponseColumn), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Сделок
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetDealReadePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(DealCurrencyConversion) , TypePermission.Read| TypePermission.Navigate),
                    new Permission(typeof(DealParticipant), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Doc), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(DocTask), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibDeal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(EstateDeal), TypePermission.Read | TypePermission.Navigate),
            };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения Сделок
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetDealReadeAndWritePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(DealCurrencyConversion), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(DealParticipant),  TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(Doc),  TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(DocTask),  TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(SibDeal),  TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(EstateDeal),  TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения ННА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetNonCoreAssetReadePermissions( IUnitOfWork uow)
        {
            var permNCA = new Permission(typeof(NonCoreAsset), TypePermission.Read | TypePermission.Navigate);
            var permNCAL = new Permission(typeof(NonCoreAssetList), TypePermission.Read | TypePermission.Navigate);
            var permNCAI = new Permission(typeof(NonCoreAssetInventory), TypePermission.Read | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(permNCA, TypePermission.Read, @"@isFromCauk || ( AssetOwner.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(AssetOwner.IDEUP) )"));
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(permNCAL, TypePermission.Read, @"@isFromCauk || ( Society.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(Society.IDEUP) )"));
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(permNCAI, TypePermission.Read, @"@isFromCauk"));

            return
                   new List<Permission>()
                   {
                    permNCA,
                    permNCAL,
                    permNCAI,
                    new Permission(typeof(NonCoreAssetAndList), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetAndListTbl), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetListItemAndNonCoreAssetSaleAccept), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetSale), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetSaleAccept), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetSaleOffer), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetTbl), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения ННА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetNonCoreAssetReadeAndWritePermissions(IUnitOfWork uow)
        {
            var permNCA = new Permission(typeof(NonCoreAsset), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
            var permNCAL = new Permission(typeof(NonCoreAssetList), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
            var permNCAI = new Permission(typeof(NonCoreAssetInventory), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(permNCA, TypePermission.Read, @"@isFromCauk || ( AssetOwner.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(AssetOwner.IDEUP) )"));
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(permNCAL, TypePermission.Read, @"@isFromCauk || ( Society.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(Society.IDEUP) )"));
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(permNCAI, TypePermission.Read, @"@isFromCauk"));

            return
                   new List<Permission>()
                   {
                    permNCA,
                    permNCAL,
                    permNCAI,
                    new Permission(typeof(NonCoreAssetAndList),TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetAndListTbl),TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetListItemAndNonCoreAssetSaleAccept),TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetSale),TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetSaleAccept),TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetSaleOffer),TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(NonCoreAssetTbl),TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Оценка
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAppraisalReadePermissions(IUnitOfWork uow)
        {
            var Apprs = new Permission(typeof(Appraisal), TypePermission.Read | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(Apprs, TypePermission.Read, @"@isFromCauk || @isFromService || (Customer != null && (Customer.IDEUP == @userIDEUP || @userTerritory.Contains(Customer.IDEUP) ))"));
            return
                   new List<Permission>()
                   {
                    Apprs,
                    new Permission(typeof(Appraiser), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(AppraisalOrgData), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(AppraiserDataFinYear), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(EstateAppraisal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(IndicateEstateAppraisalView), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(EstateAndEstateAppraisal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndAppraisal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndAppraisal), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndAppraisal), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения Оценка
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAppraisalReadeAndWritePermissions(IUnitOfWork uow)
        {
            var Apprs = new Permission(typeof(Appraisal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(Apprs, TypePermission.Read, @"@isFromCauk || @isFromService || (Customer != null && (Customer.IDEUP == @userIDEUP || @userTerritory.Contains(Customer.IDEUP) ))"));
            return
                   new List<Permission>()
                   {
                    Apprs,
                    new Permission(typeof(Appraiser), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(AppraisalOrgData), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(AppraiserDataFinYear), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(EstateAppraisal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(IndicateEstateAppraisalView), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(EstateAndEstateAppraisal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndAppraisal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskAndAppraisal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(SibTaskReportAndAppraisal), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Акционер/участник
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetShareholderReadePermissions(IUnitOfWork uow)
        {
            var shareholder = new Permission(typeof(Shareholder), TypePermission.Read | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(shareholder, TypePermission.Read, @"(SocietyRecipient.IDEUP == @userIDEUP ) || @isFromCauk"));

            return
                   new List<Permission>() {
                    shareholder,
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Акционер/участник
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetShareholderReadeAndWritePermissions(IUnitOfWork uow)
        {
            var shareholder = new Permission(typeof(Shareholder), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(shareholder, TypePermission.Read, @"(SocietyRecipient.IDEUP == @userIDEUP ) || @isFromCauk"));

            return
                   new List<Permission>() {
                    shareholder,
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения НМА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetIntangibleAssetReadePermissions()
        {
            return
                   new List<Permission>(GetEstateBaseReadePermissions())
                   {
                    new Permission(typeof(IntangibleAsset) , TypePermission.Read| TypePermission.Navigate),
                    new Permission(typeof(IntangibleAssetAndSibCountry), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(IntangibleAssetRight), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения НМА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetIntangibleAssetReadeAndWritePermissions()
        {
            return
                   new List<Permission>(GetEstateBaseReadeAndWritePermissions())
                   {
                    new Permission(typeof(IntangibleAsset), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(IntangibleAssetAndSibCountry), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(IntangibleAssetRight), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения объектов бухгалтерского учета
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAccountingObjectReadePermissions(IUnitOfWork uow)
        {
            var AccObj = new Permission(typeof(AccountingObject), TypePermission.Read | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
                .Create(new ObjectPermission(AccObj, TypePermission.Read, @"@isFromCauk || @isFromService || ( Owner.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(Owner.IDEUP) ) || ( MainOwner.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(MainOwner.IDEUP) )  || (@userBelows.Contains(Owner.IDEUP)) || ((@userAgents.Contains(Owner.IDEUP)) && (WhoUse.IDEUP == @userIDEUP))"));
            return
                   new List<Permission>()
                   {
                    AccObj,
                    new Permission(typeof(AccountingObjectTbl), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndAccountingObject), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения объектов бухгалтерского учета
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAccountingObjectReadeAndWritePermissions(IUnitOfWork uow)
        {
            var AccObj = new Permission(typeof(AccountingObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
                .Create(new ObjectPermission(AccObj, TypePermission.Read, @"@isFromCauk || @isFromService || ( Owner.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(Owner.IDEUP) ) || ( MainOwner.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(MainOwner.IDEUP) )  || (@userBelows.Contains(Owner.IDEUP)) || ((@userAgents.Contains(Owner.IDEUP)) && (WhoUse.IDEUP == @userIDEUP))"));
            return
                   new List<Permission>()
                   {
                    AccObj,
                    new Permission(typeof(AccountingObjectTbl), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndAccountingObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Имущественных комплексов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetPropertyComplexReadePermissions()
        {
            var perms = new List<Permission>(GetEstateBaseReadePermissions()) { };
            perms.Add(new Permission(typeof(PropertyComplex), TypePermission.Read | TypePermission.Navigate));
            perms.Add(new Permission(typeof(PropertyComplexIO), TypePermission.Read | TypePermission.Navigate));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения Имущественных комплексов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetPropertyComplexReadeAndWritePermissions()
        {
            var perms = new List<Permission>(GetEstateBaseReadeAndWritePermissions()) { };
            perms.Add(new Permission(typeof(PropertyComplex), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate));
            perms.Add(new Permission(typeof(PropertyComplexIO), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения ГГР
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetScheduleStateReadePermissions(IUnitOfWork uow)
        {
            var SSR = new Permission(typeof(ScheduleStateRegistration), TypePermission.Read | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(SSR, TypePermission.Read, @"(Society != null && Society.IDEUP == @userIDEUP ) || @isFromCauk || @isFromService "));

            var SST = new Permission(typeof(ScheduleStateTerminate), TypePermission.Read | TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(SST, TypePermission.Read, @"(Society != null && Society.IDEUP == @userIDEUP ) || @isFromCauk || @isFromService "));

            return
                   new List<Permission>()
                   {
                    SSR,
                    new Permission(typeof(ScheduleStateRegistrationRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ScheduleStateRegistrationStatus), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ScheduleStateRegistrationType), TypePermission.Read | TypePermission.Navigate),
                    SST,
                    new Permission(typeof(ScheduleStateTerminateRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ScheduleStateYear), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndScheduleStateRegistrationRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndScheduleStateTerminateRecord), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndScheduleStateYear), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения изменения ГГР
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetScheduleStateReadeAndWritePermissions(IUnitOfWork uow)
        {
            var SSR = new Permission(typeof(ScheduleStateRegistration), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(SSR, TypePermission.Read, @"(Society != null && Society.IDEUP == @userIDEUP ) || @isFromCauk || @isFromService "));

            var SST = new Permission(typeof(ScheduleStateTerminate), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate);
            //разерешения на экземпляры
            uow.GetRepository<ObjectPermission>()
            .Create(new ObjectPermission(SST, TypePermission.Read, @"(Society != null && Society.IDEUP == @userIDEUP ) || @isFromCauk || @isFromService "));

            return
                   new List<Permission>()
                   {
                    SSR,
                    new Permission(typeof(ScheduleStateRegistrationRecord), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    SST,
                    new Permission(typeof(ScheduleStateTerminateRecord), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(ScheduleStateYear), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(FileCardAndScheduleStateRegistrationRecord), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(FileCardAndScheduleStateTerminateRecord), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(FileCardAndScheduleStateYear), TypePermission.Read | TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        #region Получение набора разрешений на типы Объектов имущества
        /// <summary>
        /// Предоставляет набор разрешений для Объектов имущества
        /// </summary>
        /// <param name="tp">Назначение типов разрешений</param>
        /// <returns>Permission</returns>
        public static IEnumerable<Permission> GetEstatePermission(TypePermission tp)
        {
            var types = System.Reflection.Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(w => !w.IsAbstract && !w.IsInterface && w.FullName.Contains("CorpProp.Entities.Estate"))
                .DefaultIfEmpty()
                .ToList();

            foreach (var item in types)
            {
                yield return new Permission(item, tp);
            }
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и записи Объектов имущества
        /// </summary>
        /// <returns>Permission</returns>
        public static IEnumerable<Permission> GetEstatePermission_RW()
        {
            var types = System.Reflection.Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(w => !w.IsAbstract && !w.IsInterface && w.FullName.Contains("CorpProp.Entities.Estate"))
                .DefaultIfEmpty()
                .ToList();

            foreach (var item in types)
            {
                yield return new Permission(item, TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
            }
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Объектов имущества
        /// </summary>
        /// <returns>Permission</returns>
        public static IEnumerable<Permission> GetEstatePermission_RO()
        {
            var types = System.Reflection.Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(w => !w.IsAbstract && !w.IsInterface && w.FullName.Contains("CorpProp.Entities.Estate"))
                .DefaultIfEmpty()
                .ToList();

            foreach (var item in types)
            {
                yield return new Permission(item, TypePermission.Read | TypePermission.Navigate);
            }
        }
        #endregion

        /// <summary>
        /// Предоставляет набор разрешений для чтения базовых типов объектов имущества
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetEstateBaseReadePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(Estate) , TypePermission.Read| TypePermission.Navigate),
                    new Permission(typeof(EstateCalculatedField), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения базовых типов объектов имущества
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetEstateBaseReadeAndWritePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(Estate), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(EstateCalculatedField), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения базовых типов объектов имущества
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetRealEstateBaseReadePermissions()
        {
            return
                   new List<Permission>(GetEstateBaseReadePermissions())
                   {
                    new Permission(typeof(InventoryObject), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RealEstate), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения базовых типов объектов имущества
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetRealEstateBaseReadeAndWritePermissions()
        {
            return
                   new List<Permission>(GetEstateBaseReadeAndWritePermissions())
                   {
                    new Permission(typeof(InventoryObject), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(RealEstate), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Кадастровых объектов имущества
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetCadastralReadePermissions()
        {
            return
                   new List<Permission>(GetRealEstateBaseReadePermissions())
                   {
                    new Permission(typeof(Cadastral), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CadastralPart), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CadastralValue), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Land), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(BuildingStructure), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CarParkingSpace), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Room), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RealEstateComplex), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(UnfinishedConstruction), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и изменения Кадастровых объектов имущества
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetCadastralReadeAndWritePermissions()
        {
            return
                   new List<Permission>(GetRealEstateBaseReadeAndWritePermissions())
                   {
                    new Permission(typeof(Cadastral), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(CadastralPart), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(CadastralValue), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(Land), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(BuildingStructure), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(CarParkingSpace), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(Room), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(RealEstateComplex), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(UnfinishedConstruction), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Прав
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetRightReadePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(DuplicateRightView) , TypePermission.Read| TypePermission.Navigate),
                    new Permission(typeof(IntangibleAssetRight), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Right), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(RightCostView), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Прав
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetRightReadeAndWritePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(DuplicateRightView), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(IntangibleAssetRight), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(Right), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                    new Permission(typeof(RightCostView), TypePermission.Read| TypePermission.Write| TypePermission.Create| TypePermission.Delete| TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Выписок
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetExtractReadePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(Extract), TypePermission.Read| TypePermission.Navigate),
                    new Permission(typeof(ExtractFormat), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ExtractItem), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(ExtractType), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(CadastralAndExtract), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndExtract), TypePermission.Read | TypePermission.Navigate),
                    new Permission(typeof(Entities.Mapping.RosReestrTypeEstate), TypePermission.Read | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Выписок
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetExtractReadeAndWritePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(Extract) , TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(ExtractFormat), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(ExtractItem), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(ExtractType), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(CadastralAndExtract), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(FileCardAndExtract), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                    new Permission(typeof(Entities.Mapping.RosReestrTypeEstate), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Расширенной отчетности
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAnalyzeReadePermissions()
        {
            return
                   new List<Permission>()
                   {
                    
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования Расширенной отчетности
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAnalyzeReadeAndWritePermissions()
        {
            return
                   new List<Permission>()
                   {

                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения Отчетов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetPrintedFormReadePermissions()
        {
            return
                   new List<Permission>()
                   {
                    new Permission(typeof(PrintedFormList), TypePermission.Read | TypePermission.Navigate),
                   };
        }
            
        /// <summary>
        /// Предоставляет набор разрешений для чтения
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetPrintedFormReadeAndWritePermissions()
        {
            return
                   new List<Permission>()
                   {
                       new Permission(typeof(PrintedFormRegistry), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate),
                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> Get3___ReadePermissions()
        {
            return
                   new List<Permission>()
                   {

                   };
        }

        /// <summary>
        /// Предоставляет набор разрешений для чтения
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> Get4___ReadePermissions()
        {
            return
                   new List<Permission>()
                   {

                   };
        }



        public static IEnumerable<Permission> GetBusinessProcesses_RW()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(ww => !ww.IsDynamic)
                .SelectMany(s => s.GetTypes())
                .Where(w => !w.IsAbstract && !w.IsInterface && w.IsSubclassOf(typeof(BaseObject))
                && w.FullName.Contains("Base.BusinessProcesses"))
                .ToList();

            foreach (var item in types)
            {
                yield return new Permission(item, TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
            }

        }
        #endregion

        #region Получение разрешений для Роли
        /// <summary>
        /// Предоставляет набор разрешений для чтения и редактирования НСИ
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetNSIReadAndWritePerms()
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            
            foreach (var item in GetNSITypes())
            {
                perms.Add(new Permission(item, TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate));
            }
            perms.Add(new Permission(typeof(CorpProp.Entities.NSI.NSI), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate));
            perms.Add(new Permission(typeof(DictObject), TypePermission.Read | TypePermission.Write | TypePermission.Create));
            perms.Add(new Permission(typeof(HDictObject), TypePermission.Read | TypePermission.Write | TypePermission.Create));
            //-->
            //
            perms.AddRange(GetImportPermission(TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate));

            //perms.Add(new Permission(typeof(ImportErrorLog), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate));
            //perms.Add(new Permission(typeof(ImportHistory), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate));
            //perms.Add(new Permission(typeof(ImportObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate));
            //perms.Add(new Permission(typeof(ImportTemplate), TypePermission.Read));
            //perms.Add(new Permission(typeof(ImportTemplateItem), TypePermission.Read));
            //perms.Add(new Permission(typeof(TypeImportObject), TypePermission.Read));
            //
            //<--

            perms.AddRange(GetDocumentReadeAndWritePermissions());
            return perms;
        }







        /// <summary>
        /// Предоставляет набор разрешений для роли Базовый пользователь
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetDefaultPermissions(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetDocumentReadePermissions());
            perms.AddRange(GetSocietyReadePermissions(uow));
            perms.AddRange(GetSubjectReadePermissions());
            perms.AddRange(GetProjectReadePermissions());
            perms.AddRange(GetTaskReadeAndWritePermissions());
            perms.AddRange(GetRequestReadePermissions());
            perms.AddRange(GetResponseReadeAndWritePermissions());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Просмотр данных о бизнес-деятельности
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetBusinessActivityPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetSocietyReadePermissions(uow));
            perms.AddRange(GetSubjectReadePermissions());
            perms.AddRange(GetDocumentReadePermissions());
            perms.AddRange(GetProjectReadePermissions());
            perms.AddRange(GetTaskReadePermissions());
            perms.AddRange(GetDealReadePermissions());
            perms.AddRange(GetNonCoreAssetReadePermissions(uow));
            perms.AddRange(GetAppraisalReadePermissions(uow));
            perms.AddRange(GetShareholderReadePermissions(uow));
            perms.AddRange(GetIntangibleAssetReadePermissions());
            perms.AddRange(GetAccountingObjectReadePermissions(uow));
            perms.AddRange(GetPropertyComplexReadePermissions());
            perms.AddRange(GetScheduleStateReadePermissions(uow));
            return perms;
        }


        /// <summary>
        /// Предоставляет набор разрешений для роли Просмотр данных об имуществе
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetEstateReadPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetAccountingObjectReadePermissions(uow));
            perms.AddRange(GetCadastralReadePermissions());
            perms.AddRange(GetPropertyComplexReadePermissions());
            perms.AddRange(GetDealReadePermissions());
            perms.AddRange(GetDealReadePermissions());
            perms.AddRange(GetShareholderReadePermissions(uow));
            perms.AddRange(GetExtractReadePermissions());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Изменение данных об имуществе
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetEstateWritePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetAccountingObjectReadeAndWritePermissions(uow));
            perms.AddRange(GetCadastralReadeAndWritePermissions());
            perms.AddRange(GetPropertyComplexReadeAndWritePermissions());
            perms.AddRange(GetRightReadeAndWritePermissions());
            perms.AddRange(GetDealReadeAndWritePermissions());
            perms.AddRange(GetDocumentReadeAndWritePermissions());
            perms.AddRange(GetExtractReadePermissions());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Просмотр данных о НМА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetIntangibleReadPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetIntangibleAssetReadePermissions());
            perms.AddRange(GetRightReadePermissions());
            perms.AddRange(GetAppraisalReadePermissions(uow));
            perms.AddRange(GetDealReadePermissions());
            perms.AddRange(GetShareholderReadePermissions(uow));
            perms.AddRange(GetProjectReadePermissions());
            perms.AddRange(GetTaskReadePermissions());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Изменение данных о НМА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetIntangibleWritePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetIntangibleAssetReadeAndWritePermissions());
            perms.AddRange(GetProjectReadePermissions());
            perms.AddRange(GetTaskReadePermissions());
            perms.AddRange(GetRightReadeAndWritePermissions());
            perms.AddRange(GetAppraisalReadePermissions(uow));
            perms.AddRange(GetDealReadeAndWritePermissions());
            perms.AddRange(GetShareholderReadePermissions(uow));
            perms.AddRange(GetDocumentReadeAndWritePermissions());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Просмотр данных об оценках
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAppraisalReadPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetAppraisalReadePermissions(uow));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Изменение данных об оценках
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetAppraisalWritePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetAppraisalReadeAndWritePermissions(uow));
            perms.AddRange(GetDocumentReadeAndWritePermissions());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Просмотр данных о ННА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetNonCoreAssetReadPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetCadastralReadePermissions());
            perms.AddRange(GetExtractReadePermissions());
            perms.AddRange(GetAppraisalReadePermissions(uow));
            perms.AddRange(GetNonCoreAssetReadePermissions(uow));

            //разрешения на поля для типов роли Просмотр данных о ННА
            var objtype = typeof(BuildingStructure).GetTypeName();
            var perm = perms.FirstOrDefault(f => f.FullName == objtype);
            if (perm != null)
                perm.PropertyPermissions = new List<PropertyPermission>()
                {
                      new PropertyPermission(){ PropertyName = "IsNonCoreAsset", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsTaxCadastral", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Area", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BuildingArea", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Depth", AllowRead = true, AllowWrite = false }
                };
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Изменение данных о ННА
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetNonCoreAssetWritePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetCadastralReadePermissions());
            perms.AddRange(GetExtractReadePermissions());
            perms.AddRange(GetAppraisalReadePermissions(uow));
            perms.AddRange(GetNonCoreAssetReadeAndWritePermissions(uow));
            perms.AddRange(GetDocumentReadeAndWritePermissions());


            //разрешения на поля для типов роли Изменение данных о ННА
            var buildtype = typeof(BuildingStructure).GetTypeName();
            var buildPerm = perms.FirstOrDefault(f => f.FullName == buildtype);
            if (buildPerm != null)
                buildPerm.PropertyPermissions = new List<PropertyPermission>()
                {
                      new PropertyPermission(){ PropertyName = "IsNonCoreAsset", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsTaxCadastral", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Area", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "BuildingArea", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Depth", AllowRead = true, AllowWrite = false }
                };
            
            var landtype = typeof(Land).GetTypeName();
            var landPerm = perms.FirstOrDefault(f => f.FullName == landtype);
            if (landPerm != null)
                landPerm.PropertyPermissions = new List<PropertyPermission>()
                {
                      new PropertyPermission(){ PropertyName = "IsNonCoreAsset", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Wood", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "IsTaxCadastral", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "Area", AllowRead = true, AllowWrite = false }
                    , new PropertyPermission(){ PropertyName = "UseArea", AllowRead = true, AllowWrite = false }
                };
           

            return perms;
        }


        /// <summary>
        /// Предоставляет набор разрешений для роли Изменение данных о правопредшественниках
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetPredecessorWritePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetPredecessorReadeAndWritePermissions(uow));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Просмотр графиков государственной регистрации
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetScheduleStateReadPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetRightReadePermissions());
            perms.AddRange(GetExtractReadePermissions());
            perms.AddRange(GetDealReadePermissions());
            perms.AddRange(GetScheduleStateReadePermissions(uow));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Изменение графиков гос. регистрации
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetScheduleStateWritePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetRightReadePermissions());
            perms.AddRange(GetExtractReadePermissions());
            perms.AddRange(GetDealReadePermissions());
            perms.AddRange(GetScheduleStateReadeAndWritePermissions(uow));
            perms.AddRange(GetDocumentReadeAndWritePermissions());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Создание проектов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetProjectWritePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetProjectReadeAndWritePermissions());
            perms.AddRange(GetTaskReadePermissions());
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Создание запросов
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetRequestWritePerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetResponseReadePermissions());
            perms.AddRange(GetRequestReadeAndWritePermissions());

            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Оператор импорта данных из БУС
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetImportAccountingPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetAccountingObjectReadeAndWritePermissions(uow));
            //perms.AddRange(GetCadastralReadeAndWritePermissions());
            perms.AddRange(GetEstatePermission(TypePermission.Read | TypePermission.Write | TypePermission.Create));
            perms.AddRange(GetPropertyComplexReadeAndWritePermissions());
            perms.AddRange(GetIntangibleAssetReadeAndWritePermissions());
            perms.AddRange(GetRightReadeAndWritePermissions());
            perms.AddRange(GetImportPermission(TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate));
            perms.AddRange(GetDocumentReadeAndWritePermissions());

            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Оператор импорта данных прочих ИС
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetImportOtherPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetNSIReadAndOpenPerms());
            perms.AddRange(GetSocietyReadeAndWritePermissions(uow));
            perms.AddRange(GetSubjectReadeAndWritePermissions());
            perms.AddRange(GetDealReadeAndWritePermissions());
            perms.AddRange(GetShareholderReadeAndWritePermissions(uow));
            perms.AddRange(GetDocumentReadeAndWritePermissions());
            perms.AddRange(GetImportPermission(TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Оператор импорта данных Росреестра
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetImportRosreestrPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
            perms.AddRange(GetCadastralReadeAndWritePermissions());
            perms.AddRange(GetExtractReadeAndWritePermissions());
            perms.AddRange(GetRightReadeAndWritePermissions());
            perms.AddRange(GetDocumentReadeAndWritePermissions());
            perms.AddRange(GetImportPermission(TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Navigate));

            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Просмотр стандартной отчетности
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetReportPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) {
                new Permission(typeof(InventoryObject), TypePermission.Read | TypePermission.Navigate),
            };
            perms.AddRange(GetAppraisalReadePermissions(uow));
            perms.AddRange(GetScheduleStateReadPerms(uow));
            perms.AddRange(GetEstateBaseReadePermissions());
            perms.AddRange(GetNonCoreAssetReadePermissions(uow));
            perms.AddRange(GetAppraisalReadePermissions(uow));



            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Формирование отчетности
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetReportingFormationPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
                perms.AddRange(GetAccountingObjectReadePermissions(uow));
                perms.AddRange(GetSocietyReadePermissions(uow));
                perms.AddRange(GetRightReadePermissions());
                perms.AddRange(GetScheduleStateReadePermissions(uow));
                perms.AddRange(GetCadastralReadePermissions());
                perms.AddRange(GetPropertyComplexReadePermissions());
                perms.AddRange(GetIntangibleAssetReadePermissions());
                perms.AddRange(GetNonCoreAssetReadePermissions(uow));
                perms.AddRange(GetAppraisalReadePermissions(uow));
                perms.AddRange(GetPrintedFormReadeAndWritePermissions());
                perms.AddRange(GetShareholderReadePermissions(uow));
            return perms;
        }

        /// <summary>
        /// Предоставляет набор разрешений для роли Просмотр отчетов по бизнес-аналитике
        /// </summary>
        /// <returns>Permission</returns>
        public static List<Permission> GetReportReadPerms(IUnitOfWork uow)
        {
            var perms = new List<Permission>(GetBasePermissions()) { };
                perms.AddRange(GetPrintedFormReadePermissions());
            perms.AddRange(GetAccountingObjectReadePermissions(uow));
            perms.AddRange(GetSocietyReadePermissions(uow));
            perms.AddRange(GetRightReadePermissions());
            perms.AddRange(GetScheduleStateReadePermissions(uow));
            perms.AddRange(GetCadastralReadePermissions());
            perms.AddRange(GetPropertyComplexReadePermissions());
            perms.AddRange(GetIntangibleAssetReadePermissions());
            perms.AddRange(GetNonCoreAssetReadePermissions(uow));
            perms.AddRange(GetAppraisalReadePermissions(uow));
            perms.AddRange(GetShareholderReadePermissions(uow));
            return perms;
        }

        #endregion



        private static void CreateRoles(IUnitOfWork uow
            , IPresetRegistorService presetRegistorService
            , IDictionary<string, ICollection<PresetRegistor>> presets)
        {

            #region Перечень ролей
            //Администратор приложения: Admin
            //Системный Администратор: SystemAdmin
            //Администратор баз данных: DBAdmin
            //Администратор информационной безопасности: SecurityAdmin
            //Разработчик: Developer
            //Управление НСИ: NSIManager
            //Базовый пользователь: Default
            //Просмотр данных о бизнес-деятельности: BusinessActivity
            //Просмотр данных об имуществе: EstateRead
            //Изменение данных об имуществе: EstateWrite
            //Просмотр данных о НМА: IntangibleRead
            //Изменение данных о НМА: IntangibleWrite
            //Просмотр данных об оценках: AppraisalRead
            //Изменение данных об оценках: AppraisalWrite
            //Просмотр данных о ННА: NonCoreAssetRead
            //Изменение данных о ННА: NonCoreAssetWrite
            //Изменение данных о правопредшественниках: PredecessorWrite
            //Просмотр графиков государственной регистрации: ScheduleStateRead
            //Изменение графиков гос. регистрации: ScheduleStateWrite
            //Создание проектов: ProjectWrite
            //Создание запросов: RequestWrite
            //Оператор импорта данных из БУС: ImportAccounting
            //Оператор импорта данных прочих ИС: ImportOther
            //Оператор импорта данных Росреестра: ImportRosreestr
            //Просмотр стандартной отчетности: Report
            //Формирование отчетности: ReportingFormation
            //Просмотр отчетов по бизнес - аналитике: ReportRead
            #endregion

            //мега-роль
            //CreateMegaRole(uow, "Роль");

            //Базовая роль с базовыми правами на тип
            //CreateRole(uow, GetBasePermissions(), "Base", "Base", presetRegistorService, presets);
            //Супер Админ
            CreateRoleAdmin(uow, SystemRole.Admin, new List<Permission>() { }, "Неопределено", "Unknown", presetRegistorService, presets);


            //Администратор приложения: Admin
            //CreateRoleAdmin(uow, SystemRole.Admin, new List<Permission>() { }, "Администратор приложения", "Admin", presetRegistorService, presets);
            //Системный Администратор: SystemAdmin
            CreateRoleAdmin(uow, SystemRole.Admin, new List<Permission>() { }, "Системный Администратор", "SystemAdmin", presetRegistorService, presets);
            //Администратор баз данных: DBAdmin
            CreateRoleAdmin(uow, SystemRole.Admin, new List<Permission>() { }, "Администратор баз данных", "DBAdmin", presetRegistorService, presets);
            //Администратор информационной безопасности: SecurityAdmin
            CreateRoleAdmin(uow, SystemRole.Admin, new List<Permission>() { }, "Администратор информационной безопасности", "SecurityAdmin", presetRegistorService, presets);
            //Разработчик: Developer
            CreateRoleAdmin(uow, SystemRole.Admin, new List<Permission>() { }, "Разработчик", "Developer", presetRegistorService, presets);

            

            //Управление НСИ: NSIManager
            CreateRole(uow, GetNSIReadAndWritePerms(), "Управление НСИ", "NSIManager", presetRegistorService, presets);

            //Базовый пользователь: Default
            CreateRole(uow, GetDefaultPermissions(uow), "Базовый пользователь", "Default", presetRegistorService, presets);

            //Просмотр данных о бизнес-деятельности: BusinessActivity
            CreateRole(uow, GetBusinessActivityPerms(uow), "Просмотр данных о бизнес-деятельности", "BusinessActivity", presetRegistorService, presets);

            //Просмотр данных об имуществе: EstateRead
            CreateRole(uow, GetEstateReadPerms(uow), "Просмотр данных об имуществе", "EstateRead", presetRegistorService, presets);

            //Изменение данных об имуществе: EstateWrite
            CreateRole(uow, GetEstateWritePerms(uow), "Изменение данных об имуществе", "EstateWrite", presetRegistorService, presets);

            //View data on intangible assets
            //Просмотр данных о НМА: IntangibleRead
            CreateRole(uow, GetIntangibleReadPerms(uow), "Просмотр данных о НМА", "IntangibleRead", presetRegistorService, presets);

            //Changing the HMA data
            //Изменение данных о НМА: IntangibleWrite
            CreateRole(uow, GetIntangibleWritePerms(uow), "Изменение данных о НМА", "IntangibleWrite", presetRegistorService, presets);

            //View Rating Data
            //Просмотр данных об оценках: AppraisalRead
            CreateRole(uow, GetAppraisalReadPerms(uow), "Просмотр данных об оценках", "AppraisalRead", presetRegistorService, presets);

            //Changing Valuation Data
            //Изменение данных об оценках: AppraisalWrite
            CreateRole(uow, GetAppraisalWritePerms(uow), "Изменение данных об оценках", "AppraisalWrite", presetRegistorService, presets);

            //Viewing data on NNA is a non - core inefficient asset
            //Просмотр данных о ННА: NonCoreAssetRead
            CreateRole(uow, GetNonCoreAssetReadPerms(uow), "Просмотр данных о ННА", "NonCoreAssetRead", presetRegistorService, presets);

            //Changing the NTA
            //Изменение данных о ННА: NonCoreAssetWrite
            CreateRole(uow, GetNonCoreAssetWritePerms(uow), "Изменение данных о ННА", "NonCoreAssetWrite", presetRegistorService, presets);

            //Changing the information about the legal predecessors
            //Изменение данных о правопредшественниках: PredecessorWrite
            CreateRole(uow, GetPredecessorWritePerms(uow), "Изменение данных о правопредшественниках", "PredecessorWrite", presetRegistorService, presets);

            //Viewing schedules of state registration
            //Просмотр графиков государственной регистрации: ScheduleStateRead
            CreateRole(uow, GetScheduleStateReadPerms(uow), "Просмотр графиков государственной регистрации", "ScheduleStateRead", presetRegistorService, presets);

            //Changing schedules of state registration
            //Изменение графиков гос. регистрации: ScheduleStateWrite
            CreateRole(uow, GetScheduleStateWritePerms(uow), "Изменение графиков гос. регистрации", "ScheduleStateWrite", presetRegistorService, presets);

            //Creating Projects
            //Создание проектов: ProjectWrite
            CreateRole(uow, GetProjectWritePerms(uow), "Создание проектов", "ProjectWrite", presetRegistorService, presets);

            //Creating queries
            //Создание запросов: RequestWrite
            CreateRole(uow, GetRequestWritePerms(uow), "Создание запросов", "RequestWrite", presetRegistorService, presets);

            //Оператор импорта данных из БУС (бухгалтерская учетная система): ImportAccounting
            CreateRole(uow, GetImportAccountingPerms(uow), "Оператор импорта данных из БУС", "ImportAccounting", presetRegistorService, presets);

            //Оператор импорта данных прочих ИС (информационная система): ImportOther
            CreateRole(uow, GetImportOtherPerms(uow), "Оператор импорта данных прочих ИС", "ImportOther", presetRegistorService, presets);

            //Перенесено в Common.Data.UsersAndRolesInitializer
            //Оператор импорта данных Росреестра: ImportRosreestr
            //CreateRole(uow, GetImportRosreestrPerms(uow), "Оператор импорта данных Росреестра", "ImportRosreestr", presetRegistorService, presets);

            //Просмотр стандартной отчетности: Report           
            CreateRole(uow, GetReportPerms(uow), "Просмотр стандартной отчетности", "Report", presetRegistorService, presets);

            //Формирование отчетности: ReportingFormation
            CreateRole(uow, GetReportingFormationPerms(uow), "Формирование отчетности", "ReportingFormation", presetRegistorService, presets);

            //Просмотр отчетов по бизнес-аналитике: ReportRead
            CreateRole(uow, GetReportReadPerms(uow), "Просмотр отчетов по бизнес-аналитике", "ReportRead", presetRegistorService, presets);

        }


        private static void CreateMenu(
            IUnitOfWork uow
            , IPresetRegistorService presetRegistorService
            , IDictionary<string, ICollection<PresetRegistor>> presets
           )
        {
            var adminMenu = new CorpProp.DefaultData.MenuDefaultData(uow,presetRegistorService).CreateAdminMenu("Admin");
            var adminPreset = new PresetRegistor()
            {
                Title = "Меню Администратора приложения",
                Type = typeof(MenuPreset).GetTypeName(),
                For = "Menu",
                Preset = adminMenu
            };
            presets.Add("admins", new List<PresetRegistor>() { presetRegistorService.Create(uow, adminPreset) });

            var userMenu = new CorpProp.DefaultData.MenuDefaultData(uow,presetRegistorService).CreateUserMenu();
            var userPreset = new PresetRegistor()
            {
                Title = "Меню Пользователя",
                Type = typeof(MenuPreset).GetTypeName(),
                For = "Menu",
                Preset = userMenu
            };

            presets.Add("users", new List<PresetRegistor>() { presetRegistorService.Create(uow, userPreset) });
        }

        /// <summary>
        /// Создание группы для пользователей Системы и самих пользователей
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="loginProvider"></param>
        /// <param name="presets"></param>
        private static void CreateUserCategorys(
            IUnitOfWork uow
            , ILoginProvider loginProvider
            , IDictionary<string, ICollection<PresetRegistor>> presets)
        {
            var userRepository = uow.GetRepository<User>();

            var defaultRole = uow.GetRepository<Role>()
                   .Filter(f => !f.Hidden && f.Code == "Default")
                   .FirstOrDefault();

            var baseCategory = new UserCategory()
            {
                SysName = "users",
                Name = "Пользователи",
                ProfileMnemonic = nameof(SibUser),
                IsAccessible = true,
                Roles = new List<Role>() { defaultRole }
            };


            //TODO: 
            baseCategory.Presets =
                      presets["users"].Select(x => new UserCategoryPreset() { Object = x }).ToList();

            uow.GetRepository<UserCategory>().Create(baseCategory);

            var adminCategory = uow.GetRepository<UserCategory>()
                        .Filter(f => !f.Hidden && f.SysName == "admins")
                        .FirstOrDefault();

            if (adminCategory != null)
            {
                adminCategory.ProfileMnemonic = nameof(SibUser);
                adminCategory.Presets =
                    presets["admins"].Select(x => new UserCategoryPreset() { Object = x }).ToList();
            }
            var admin = uow.GetRepository<User>()
                        .Filter(f => !f.Hidden && f.SysName == "admin")
                        .FirstOrDefault();

            if (admin != null)
            {
                var profile = new SibUser() { LastName = "Пользователь системы", IsEmpty = false, User = admin };
                var pr = uow.GetRepository<SibUser>().Create(profile);
                admin.Profile = pr;

            }
            uow.SaveChanges();

            //тестовые пользователи
            for (int i = 1; i < 101; i++)
            {

                var user = new User
                {
                    SysName = "User" + i.ToString(),
                    CategoryID = adminCategory.ID,
                };
                var profile = new SibUser() { LastName = "Пользователь " + i, IsEmpty = false, User = user };
                user.Profile = uow.GetRepository<SibUser>().Create(profile);

                user = uow.GetRepository<User>().Create(user);

                uow.SaveChanges();

                if (!loginProvider.Exist(uow, "User" + i))
                    loginProvider.AttachSystemPassword(uow, user.ID, "User" + i, "123456Ss");
                uow.SaveChanges();
            }

        }

        #region Создание мега-роли
        /// <summary>
        /// Создает мега-роль с разрешениями на все типы сборки.
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="roleName">Имя роли</param>
        private static void CreateMegaRole(IUnitOfWork uow, string roleName)
        {
            var megaRole = uow.GetRepository<Base.Security.Role>()
                        .Filter(f => !f.Hidden && f.Name == roleName)
                        .FirstOrDefault();

            if (megaRole == null)
            {

                //на экземпляры ОБУ
                var AccObj = new Permission(typeof(AccountingObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>()
                    .Create(new ObjectPermission(AccObj, TypePermission.Read,
                    @"@isFromCauk || @isFromService || ( Owner.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(Owner.IDEUP) ) || ( MainOwner.IDEUP == @userIDEUP ) || ( @userTerritory.Contains(MainOwner.IDEUP) ) || (@userBelows.Contains(Owner.IDEUP)) || ((@userAgents.Contains(Owner.IDEUP)) && (WhoUse.IDEUP == @userIDEUP))"));

                /*
                  //разрешения на имущество
                List<Permission> MegaEstatePermissions = new List<Permission>();

                string strEstatePermissionCriteria = @"@isFromCauk || @isFromService || @isFromService || (Calculate.Owner.IDEUP == @userIDEUP) || (@userTerritory.Contains(Calculate.Owner.IDEUP)) || (@userBelows.Contains(Calculate.Owner.IDEUP)) || ((@userAgents.Contains(Calculate.Owner.IDEUP)) && (Calculate.WhoUse.IDEUP == @userIDEUP))";
                string strInventoryObjectPermissionCriteria = @"@isFromCauk || (Calculate.Owner.IDEUP == @userIDEUP) || (@userTerritory.Contains(Calculate.Owner.IDEUP)) || (@userBelows.Contains(Calculate.Owner.IDEUP)) || ((@userAgents.Contains(Calculate.Owner.IDEUP)) && (Calculate.WhoUse.IDEUP == @userIDEUP)) || (IsPropertyComplex == True)";
                
                //на экземпляры Estate
                var EstateObj = new Permission(typeof(Estate), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(EstateObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(EstateObj);

                //на экземпляры InventoryObject
                var InventoryObjectObj = new Permission(typeof(InventoryObject), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(InventoryObjectObj, TypePermission.Read, strInventoryObjectPermissionCriteria));
                MegaEstatePermissions.Add(InventoryObjectObj);

                //на экземпляры MovableEstate
                var MovableEstateObj = new Permission(typeof(MovableEstate), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(MovableEstateObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(MovableEstateObj);

                //на экземпляры Vehicle
                var VehicleObj = new Permission(typeof(Vehicle), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(VehicleObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(VehicleObj);

                //на экземпляры RealEstate
                var RealEstateObj = new Permission(typeof(RealEstate), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(RealEstateObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(RealEstateObj);

                //на экземпляры Cadastral
                var CadastralObj = new Permission(typeof(Cadastral), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(CadastralObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(CadastralObj);

                //на экземпляры NonCadastral
                var NonCadastralObj = new Permission(typeof(NonCadastral), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(NonCadastralObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(NonCadastralObj);

                //на экземпляры BuildingStructure
                var BuildingStructureObj = new Permission(typeof(BuildingStructure), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(BuildingStructureObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(BuildingStructureObj);

                //на экземпляры CarParkingSpace
                var CarParkingSpaceObj = new Permission(typeof(CarParkingSpace), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(CarParkingSpaceObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(CarParkingSpaceObj);

                //на экземпляры Room
                var RoomObj = new Permission(typeof(Room), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(RoomObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(RoomObj);

                //на экземпляры Land
                var LandObj = new Permission(typeof(Land), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(LandObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(LandObj);

                //на экземпляры RealEstateComplex
                var RealEstateComplexObj = new Permission(typeof(RealEstateComplex), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(RealEstateComplexObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(RealEstateComplexObj);

                //на экземпляры UnfinishedConstruction
                var UnfinishedConstructionObj = new Permission(typeof(UnfinishedConstruction), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(UnfinishedConstructionObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(UnfinishedConstructionObj);

                //на экземпляры Aircraft
                var AircraftObj = new Permission(typeof(Aircraft), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(AircraftObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(AircraftObj);

                //на экземпляры Ship
                var ShipObj = new Permission(typeof(Ship), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(ShipObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(ShipObj);

                //на экземпляры SpaceShip
                var SpaceShipObj = new Permission(typeof(SpaceShip), TypePermission.Read | TypePermission.Write | TypePermission.Create | TypePermission.Delete | TypePermission.Navigate);
                uow.GetRepository<ObjectPermission>().Create(new ObjectPermission(SpaceShipObj, TypePermission.Read, strEstatePermissionCriteria));
                MegaEstatePermissions.Add(SpaceShipObj);
                */

                //разрешения
                List<Permission> MegaPermissions = new List<Permission>();

                var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic);

                List<Type> types = new List<Type>();
                foreach (var item in assemblys)
                {
                    var ass = GetAssemblyTypes(item)
                        .Where(w => !w.IsAbstract && !w.IsInterface && w.IsSubclassOf(typeof(Base.BaseObject)));
                    if (ass.Any())
                        types.AddRange(ass);

                }

                foreach (var tt in types)
                {
                    if (!tt.Equals(typeof(AccountingObject)))
                        MegaPermissions
                        .Add(new Permission(tt,
                            TypePermission.Read |
                            TypePermission.Write |
                            TypePermission.Create |
                            TypePermission.Delete |
                            TypePermission.Navigate));
                    else
                    {
                        MegaPermissions.Add(AccObj);
                        //MegaPermissions.AddRange(MegaEstatePermissions);
                    }
                }

                //роль
                megaRole = new Base.Security.Role()
                {
                    Name = roleName,
                    Code = "MegaRole",
                    SystemRole = SystemRole.Base,
                    Permissions = new List<Permission>(MegaPermissions) { },
                };

                uow.GetRepository<Base.Security.Role>().Create(megaRole);
            }

        }
        #endregion


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

                var userMenu = new CorpProp.DefaultData.MenuDefaultData(uow, presetRegistorService).CreateAdminMenu(roleCode);
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


    }
}
    
