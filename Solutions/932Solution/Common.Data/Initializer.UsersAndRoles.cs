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
using System.Collections.Generic;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace Common.Data
{
    public static class UsersAndRolesInitializer
    {
        
        public static void Seed(IUnitOfWork unitOfWork, ILoginProvider loginProvider,
            IDictionary<string, ICollection<PresetRegistor>> presets)
        {
            //Базовые разрешения
            List <Permission> DefaultPermission = new List<Permission>()
            {
                //Базовый класс пользователя
                new Permission(typeof(User), TypePermission.Read),
                //Группы пользователей
                new Permission(typeof(UserCategory), TypePermission.Read),
                //Базовый класс профиля пользователя
                new Permission(typeof(BaseProfile)),
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
                //Базовый класс для задач
                new Permission(typeof(BaseTaskCategory), TypePermission.Read),
                new Permission(typeof(Task),
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
            unitOfWork.GetRepository<Role>()
                .Create(new Role()
                        {
                            Name = "Base",
                            Code = "Base",
                            SystemRole = SystemRole.Base,
                            Permissions = new List<Permission>(DefaultPermission) { },
                        });

            var userRepository = unitOfWork.GetRepository<User>();

            User admin = null;

            if (!userRepository.All().Any())
            {
                var adminCategory = new UserCategory()
                {
                    SysName = "admins",
                    Name = "Администраторы",                  
                    Roles = new List<Role>()
                    {
                        new Role()
                        {
                            Name = "Admin",
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


       
    }
}