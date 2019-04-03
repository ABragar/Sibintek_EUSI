using System.Collections.Generic;
using Base;
using Base.DAL;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;

namespace Common.Data
{
    internal static class PresetsInitializer
    {
        public static void Seed(IUnitOfWork unitOfWork, IPresetRegistorService presetRegistorService,
            IDictionary<string, ICollection<PresetRegistor>> presets)
        {
          
            CreateStandartDashboard(unitOfWork, presetRegistorService, presets);
            //GreateStandartQuickAccessBar(unitOfWork, presetRegistorService);
        }

        //private static void CreateAdminMenu(IUnitOfWork unitOfWork, IPresetRegistorService presetRegistorService,
        //    IDictionary<string, ICollection<PresetRegistor>> presets)
        //{
        //    var menuPreset = new MenuPreset()
        //    {
        //        For = "Menu",
        //    };

        //    menuPreset.MenuElements.Add(new MenuElement("Безопасность", null, "glyphicon glyphicon-lock")
        //    {
        //        Children = new List<MenuElement>()
        //        {
        //            new MenuElement("Пользователи", "AccessUser", "halfling halfling-user"),
        //            new MenuElement("Роли", "Role", "glyphicon glyphicon-keys")
        //        }
        //    });

        //    menuPreset.MenuElements.Add(new MenuElement("Сервис", null, "halfling halfling-cog")
        //    {
        //        Children = new List<MenuElement>()
        //        {
        //            new MenuElement("Бизнес-процессы", "BPWorkflow", "glyphicon glyphicon-retweet-2"),
        //            new MenuElement("Настройки", "SettingItem", "halfling halfling-tasks"),
        //            new MenuElement("Перечисления", "UiEnum", "glyphicon glyphicon-tags"),
        //            new MenuElement("Пресеты", "PresetRegistor", "glyphicon glyphicon-credit-card")
        //        }
        //    });

        //    menuPreset.MenuElements.Add(new MenuElement("Карта", null, "glyphicon glyphicon-map")
        //    {
        //        Children = new List<MenuElement>()
        //        {
        //            new MenuElement("Настройки", "MapLayerConfig", "mdi mdi-google-maps"),
        //            new MenuElement("Тест карта", null, "mdi mdi-map-marker")
        //            {
        //                URL = "/Map/View?mnemonics=TestBaseMapObject,TestSimpleMapObject"
        //            }
        //        }
        //    });

        //    var menuPresetRegistor = new PresetRegistor()
        //    {
        //        Title = "Меню Администратора",
        //        Type = typeof(MenuPreset).GetTypeName(),
        //        For = "Menu",
        //        Preset = menuPreset
        //    };

        //    var preset = presetRegistorService.Create(unitOfWork, menuPresetRegistor);

        //    if (!presets.ContainsKey("admins"))
        //        presets.Add("admins", new List<PresetRegistor>());

        //    presets["admins"].Add(preset);
        //}

        //public static void CreateUserMenu(IUnitOfWork unitOfWork, IPresetRegistorService presetRegistorService,
        //    IDictionary<string, ICollection<PresetRegistor>> presets)
        //{
        //    var menuPreset = new MenuPreset()
        //    {
        //        For = "Menu",
        //    };

        //    menuPreset.MenuElements.Add(new MenuElement("Безопасность", null, "glyphicon glyphicon-lock")
        //    {
        //        Children = new List<MenuElement>()
        //        {
        //            new MenuElement("Пользователи", "AccessUser", "halfling halfling-user"),
        //            new MenuElement("Роли", "Role", "glyphicon glyphicon-keys")
        //        }
        //    });

        //    menuPreset.MenuElements.Add(new MenuElement("Сервис", null, "halfling halfling-cog")
        //    {
        //        Children = new List<MenuElement>()
        //        {
        //            new MenuElement("Бизнес-процессы", "BPWorkflow", "glyphicon glyphicon-retweet-2"),
        //            new MenuElement("Настройки", "SettingItem", "halfling halfling-tasks"),
        //            new MenuElement("Перечисления", "UiEnum", "glyphicon glyphicon-tags"),
        //            new MenuElement("Пресеты", "PresetRegistor", "glyphicon glyphicon-credit-card")
        //        }
        //    });

        //    var menuPresetRegistor = new PresetRegistor()
        //    {
        //        Title = "Меню Пользователя",
        //        Type = typeof(MenuPreset).GetTypeName(),
        //        For = "Menu",
        //        Preset = menuPreset
        //    };

        //    var preset = presetRegistorService.Create(unitOfWork, menuPresetRegistor);

        //    if (!presets.ContainsKey("users"))
        //        presets.Add("users", new List<PresetRegistor>());

        //    presets["users"].Add(preset);
        //}

     

      

        private static void GreateStandartQuickAccessBar(IUnitOfWork unitOfWork,
            IPresetRegistorService presetRegistorService)
        {
            var quickAccessBarPreset = new QuickAccessBarPreset {For = "QuickAccessBar"};
            quickAccessBarPreset.BarButtons.Add(new QuickAccessBarButton
            {
                Icon = {Value = "glyphicon glyphicon-group"},
                ButtonType = QABarButtonType.Link,
                Mnemonic = "User",
                Title = "Пользователи"
            });

            quickAccessBarPreset.BarButtons.Add(new QuickAccessBarButton
            {
                Icon = {Value = "glyphicon glyphicon-rabbit"},
                ButtonType = QABarButtonType.Window,
                Mnemonic = "Task",
                Title = "Создать задачу"
            });

            quickAccessBarPreset.BarButtons.Add(new QuickAccessBarButton
            {
                Icon = {Value = "glyphicon glyphicon-file"},
                ButtonType = QABarButtonType.Window,
                Mnemonic = "FileStorageItem",
                Title = "Добавить файл"
            });
            var quickAccessBarPresetRegistor = new PresetRegistor()
            {
                Title = "Стандартная панель Быстрого Доступа",
                Type = typeof(QuickAccessBarPreset).GetTypeName(),
                For = "QuickAccessBar",
                Preset = quickAccessBarPreset
            };

            presetRegistorService.Create(unitOfWork, quickAccessBarPresetRegistor);
        }

        private static void CreateStandartDashboard(IUnitOfWork unitOfWork, IPresetRegistorService presetRegistorService,
            IDictionary<string, ICollection<PresetRegistor>> presets)
        {
            var globalDashboardPreset = new PresetRegistor()
            {
                Title = "Стандартный рабочий стол",
                Type = typeof(DashboardPreset).GetTypeName(),
                For = "Global",
                Preset = new DashboardPreset()
                {
                    For = "Global"
                }
            };

            var preset = presetRegistorService.Create(unitOfWork, globalDashboardPreset);

            if (!presets.ContainsKey("admins"))
                presets.Add("admins", new List<PresetRegistor>());

            presets["admins"].Add(preset);

            if (!presets.ContainsKey("users"))
                presets.Add("users", new List<PresetRegistor>());

            presets["users"].Add(preset);
        }
    }
}