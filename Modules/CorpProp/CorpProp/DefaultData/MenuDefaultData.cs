using Base;
using Base.Audit.Entities;
using Base.DAL;
using Base.Reporting.Entities;
using Base.Security;
using Base.Settings;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.History;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.NSI;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Subject;
using CorpProp.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CorpProp.DefaultData
{
    /// <summary>
    /// Предоставляет методы для создания кастомизированного меню приложения.
    /// </summary>
    public class MenuDefaultData
    {
        private readonly IPresetRegistorService _presetRegistorService;
        private IUnitOfWork uow;


        public void Seed(IUnitOfWork unitOfWork)
        {
            uow = unitOfWork;
            var userPreset = this.CreateUserMenu(unitOfWork);
            this.SetUserCategoryPreset(unitOfWork, "users", userPreset);
            var adminPreset = this.CreateAdminMenu(unitOfWork);
            this.SetUserCategoryPreset(unitOfWork, "admins", adminPreset);
            unitOfWork.SaveChanges();
        }

        public void SetUserCategoryPreset(IUnitOfWork unitOfWork, string sysname, PresetRegistor presetRegistor)
        {
            var presets = unitOfWork.GetRepository<UserCategory>().Find(_ => _.SysName == sysname).Presets;
            var similarUserCategoryPreset =
                presets.FirstOrDefault(preset =>
                                           preset.Object.For == presetRegistor.For &&
                                           preset.Object.Title == presetRegistor.Title &&
                                           preset.Object.Type == presetRegistor.Type);
            if (similarUserCategoryPreset == null)
                presets.Add(new UserCategoryPreset() { Object = presetRegistor });
            else
                similarUserCategoryPreset.Object = presetRegistor;
        }

        public MenuDefaultData(IUnitOfWork unitOfWork, IPresetRegistorService presetRegistorService)
        {
            _presetRegistorService = presetRegistorService;
            uow = unitOfWork;
        }


        private MenuElement checkMenuElement(List<string> listRole, string roleCode, MenuElement menuElement)
        {
            var tMenuElement = (listRole.Contains(roleCode))? menuElement : new MenuElement("Null",null);
            return tMenuElement;
        }
        private List<MenuElement> checkChildrenMenuElement(List<string> listRole, string roleCode, List<MenuElement> listMenuElement)
        {
            var children = new List<MenuElement>(){ };

            if (listRole.Contains(roleCode))
                children.AddRange(listMenuElement);
            return children;
        }

        /// <summary>
        /// Создает и возвращает пункты навигацонного меню пользователя.
        /// </summary>
        /// <returns></returns>
        public virtual MenuPreset CreateUserMenu()
        {
            return CreateUserMenu("");
        }
        /// <summary>
        /// Создает и возвращает пункты навигацонного меню пользователя.
        /// </summary>
        /// <returns></returns>
        public virtual MenuPreset CreateUserMenu(string roleCode)
        {
            //var tRole = uow.GetRepository<Base.Security.Role>()
            //            .Filter(f => !f.Hidden && f.Code == roleCode)
            //            .FirstOrDefault();
            var tRoleCode = (string.IsNullOrEmpty(roleCode))? "" : roleCode; // tRole?.Code;

            List<MenuElement> tChildren = new List<MenuElement>() { };

            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Unknown", "Default", "BusinessActivity", "PredecessorWrite", "ImportOther", "ReportingFormation", "ReportRead"}, tRoleCode, new MenuElement("ОГ", typeof(Society).Name)));
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Unknown", "BusinessActivity", "EstateRead", "EstateWrite", "IntangibleRead", "IntangibleWrite", "NonCoreAssetRead", "NonCoreAssetWrite", "ScheduleStateRead", "ScheduleStateWrite", "ImportAccounting", "ImportRosreestr", "Report", "ReportingFormation", "ReportRead", "ImportBusinessIntelligenceData" }, tRoleCode, new MenuElement("Учет", null)
            {
                Children = checkChildrenMenuElement(new List<string>() {"Unknown", "BusinessActivity","EstateRead","EstateWrite","IntangibleRead","IntangibleWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData" }, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","EstateRead","EstateWrite","ImportAccounting","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("Объекты БУ", typeof(AccountingObject).Name)),
                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("Материальные активы", "InventoryObjectMenuList")
                    {
                            Children = checkChildrenMenuElement (new List<string>(){"Unknown","BusinessActivity","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new List<MenuElement>()
                            {
                                checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("Недвижимое имущество", "RealEstateMenuList")
                                {
                                    Children = checkChildrenMenuElement (new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("Кадастровые объекты", "CadastralMenuList")
                                        {
                                        Children = checkChildrenMenuElement (new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new List<MenuElement>()
                                        {
                                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("Земельные участки", "LandMenuList")),
                                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("Здания/сооружения", "BuildingStructureMenuList")
                                            {
                                                Children = checkChildrenMenuElement (new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new List<MenuElement>()
                                            {
                                                checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("Помещения",  "RoomMenuList")),
                                                checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("Машиноместа", "CarParkingSpaceMenuList")),
                                            }),
                                            }),
                                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("НЗС", "UnfinishedConstructionMenuList")),
                                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ImportAccounting","ImportRosreestr","Report","ReportingFormation","ReportRead"},tRoleCode, new MenuElement("ЕНК (Единый недвижимый комплекс", "RealEstateComplexMenuList")),
                                        }),
                                        }),

                                        checkMenuElement(new List<string>(){"Unknown"},tRoleCode, new MenuElement("Речные/морские суда", "ShipMenuList")),
                                        checkMenuElement(new List<string>(){"Unknown"},tRoleCode, new MenuElement("Воздушные суда", "AircraftMenuList")),
                                        //new MenuElement("Космические объекты", null) { URL = @"#" },
                                     
                                    }),
                                }),
                                checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("Движимое имущество", "MovableEstateMenuList")
                                {
                                    Children = checkChildrenMenuElement(new List<string>(){"Unknown"}, tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("Транспортные средства", "VehicleMenuList"))
                                    })
                                }),
                                checkMenuElement(new List<string>(){"Unknown","BusinessActivity","EstateRead","EstateWrite","ImportAccounting","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Имущественные комплексы", "InventoryObjectTree"))
                            }),
                    }),
                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","IntangibleRead","IntangibleWrite","ImportAccounting","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("НМА", typeof(IntangibleAsset).Name)),
                    //new MenuElement("Кадастровые объекты", "CadastralMenuList"),
                    checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","IntangibleRead","IntangibleWrite","ScheduleStateRead","ScheduleStateWrite","ImportAccounting","ImportRosreestr","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Права АИС КС", typeof(Right).Name)),
                    //new MenuElement("Имущественные комплексы", "PropertyComplexTree"),                    
                    //new MenuElement("ИК", "PCInventory"),
                    checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new MenuElement("Данные Росреестра", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new MenuElement("Выписки о правах юридического лица", "ExtractSubj")),
                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new MenuElement("Выписки о характеристиках объекта недвижимости", "ExtractObject")),
                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new MenuElement("ОНИ", "ObjectRecord")),
                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new MenuElement("Права ЕГРН", "RightRecord")),
                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new MenuElement("Обременения/Ограничения ЕГРН", "RestrictRecord")),
                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new MenuElement("Документы ЕГРН", "DocumentRecord")),
                            checkMenuElement(new List<string>(){"Unknown","EstateRead","EstateWrite","NonCoreAssetRead","NonCoreAssetWrite","ScheduleStateRead","ScheduleStateWrite","ImportRosreestr"}, tRoleCode, new MenuElement("Субъекты", "SubjectRecord"))
                        })
                    }),
                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Данные расширенной отчетности", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Банковские счёта", "BankAccount")),
                            checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Финансовый показатель общества", "FinancialIndicatorItem")),
                            checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Исполнение бюджета", "RecordBudgetLine")),
                            checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Дополнительные сведения об ОГ", "AnalyzeSociety")),
                        })
                    })
                })
            }));

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Unknown", "Default", "BusinessActivity", "EstateRead", "EstateWrite", "IntangibleRead", "IntangibleWrite", "AppraisalRead", "AppraisalWrite", "NonCoreAssetRead", "NonCoreAssetWrite", "ScheduleStateRead", "ScheduleStateWrite", "ProjectWrite", "RequestWrite", "ImportOther", "Report"}, tRoleCode, new MenuElement("Управление", null)
            {
                Children = checkChildrenMenuElement(new List<string>() {"Unknown", "Default", "BusinessActivity", "EstateRead", "EstateWrite", "IntangibleRead", "IntangibleWrite", "AppraisalRead", "AppraisalWrite", "NonCoreAssetRead", "NonCoreAssetWrite", "ScheduleStateRead", "ScheduleStateWrite", "ProjectWrite", "RequestWrite", "ImportOther", "Report"}, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","EstateRead","EstateWrite","IntangibleRead","IntangibleWrite","ScheduleStateRead","ScheduleStateWrite","ImportOther"}, tRoleCode, new MenuElement("Сделки", typeof(CorpProp.Entities.DocumentFlow.SibDeal).Name)),
                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","IntangibleRead","IntangibleWrite","AppraisalRead","AppraisalWrite","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Оценка", typeof(Appraisal).Name)
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","BusinessActivity","IntangibleRead","IntangibleWrite","AppraisalRead","AppraisalWrite","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","IntangibleRead","IntangibleWrite","AppraisalRead","AppraisalWrite","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Сводный реестр оценок", "SummaryAppraisal")),
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","IntangibleRead","IntangibleWrite","AppraisalRead","AppraisalWrite","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Оценки по исполнителям", "SummaryAppraisalExecutor")),
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","IntangibleRead","IntangibleWrite","AppraisalRead","AppraisalWrite","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Оценочные организации ", typeof(Appraiser).Name))
                        })
                    }),
                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","ScheduleStateRead","ScheduleStateWrite","Report"}, tRoleCode, new MenuElement("Гос. регистрация", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","BusinessActivity","ScheduleStateRead","ScheduleStateWrite","Report"}, tRoleCode, new List<MenuElement>()
                        {
                                checkMenuElement(new List<string>(){"Unknown","BusinessActivity","ScheduleStateRead","ScheduleStateWrite","Report"}, tRoleCode, new MenuElement("Регистрация права", typeof(ScheduleStateRegistration).Name)),
                                checkMenuElement(new List<string>(){"Unknown","BusinessActivity","ScheduleStateRead","ScheduleStateWrite","Report"}, tRoleCode, new MenuElement("Регистрация прекращения права", typeof(ScheduleStateTerminate).Name)),
                                //new MenuElement("ГГР (регистрация)", "ScheduleStateRegistrationRecordPivot"),
                                //new MenuElement("ГГР (прекращение)", "ScheduleStateTerminateRecordPivot"),
                                checkMenuElement(new List<string>(){"Unknown","BusinessActivity","ScheduleStateRead","ScheduleStateWrite","Report"}, tRoleCode, new MenuElement("Сводный ГГР на год", typeof(ScheduleStateYear).Name))
                        })

                    }),
                    checkMenuElement(new List<string>(){"Unknown","Default","BusinessActivity","IntangibleRead","IntangibleWrite","ProjectWrite"}, tRoleCode, new MenuElement("Проекты и задачи", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","Default","BusinessActivity","IntangibleRead","IntangibleWrite","ProjectWrite"}, tRoleCode, new List<MenuElement>()
                        {
                                checkMenuElement(new List<string>(){"Unknown","Default","BusinessActivity","IntangibleRead","IntangibleWrite","ProjectWrite"}, tRoleCode, new MenuElement("Проекты", "SibProjectMenuList")),
                                checkMenuElement(new List<string>(){"Unknown","Default","BusinessActivity","IntangibleRead","IntangibleWrite","ProjectWrite"}, tRoleCode, new MenuElement("Шаблоны проектов ", "SibProjectTemplate")),
                                //new MenuElement("Задачи", typeof(SibTask).Name)
                                checkMenuElement(new List<string>(){"Unknown","Default","BusinessActivity","IntangibleRead","IntangibleWrite"}, tRoleCode, new MenuElement("Задачи", "SibTaskMenuList")),
                                checkMenuElement(new List<string>(){"Unknown","Default","BusinessActivity","IntangibleRead","IntangibleWrite","ProjectWrite"}, tRoleCode, new MenuElement("Шаблоны задач", "SibTaskTemplate"))
                        })

                    }),
                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("ННА", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Объекты ННА", typeof(NonCoreAsset).Name)),
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Перечни ННА", typeof(NonCoreAssetList).Name)),
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Реестры ННА", typeof(NonCoreAssetInventory).Name)
                            {
                                Children = checkChildrenMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new List<MenuElement>() {
                                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("К исполнению", "NonCoreAssetAndListMenu"))
                                })
                            }),
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Способы распоряжения активами", typeof(NonCoreAssetSaleOffer).Name)),
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Одобрения реализации", typeof(NonCoreAssetSaleAccept).Name)),
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","NonCoreAssetRead","NonCoreAssetWrite","Report"}, tRoleCode, new MenuElement("Перечни к реализации", typeof(NonCoreAssetSale).Name)),
                        })

                    }),
                    checkMenuElement(new List<string>(){"Unknown","Default","RequestWrite"}, tRoleCode, new MenuElement("Запросы ЦАУК", typeof(CorpProp.Entities.Request.Request).Name)),
                    checkMenuElement(new List<string>(){"Unknown","Default","RequestWrite"}, tRoleCode, new MenuElement("Запрос в ОГ", "Response")
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","Default","RequestWrite"}, tRoleCode, new List<MenuElement>()
                        {
                                checkMenuElement(new List<string>(){"Unknown","Default","RequestWrite"}, tRoleCode, new MenuElement("Входящие запросы", "IncomingResponse")),
                                checkMenuElement(new List<string>(){"Unknown","Default","RequestWrite"}, tRoleCode, new MenuElement("Исходящие ответы", "OutgoingResponse"))
                        })
                    }),
                })
            }));

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Unknown", "NSIManager"}, tRoleCode, new MenuElement("НСИ", nameof(NSI))));
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Default", "BusinessActivity", "ImportOther"}, tRoleCode, new MenuElement("Деловые партнеры", typeof(Subject).Name)));
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"BusinessActivity", "EstateRead", "IntangibleRead", "IntangibleWrite", "ImportOther", "ReportingFormation", "ReportRead"}, tRoleCode, new MenuElement("Данные об Акционерах/Участии", typeof(Shareholder).Name))); 

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Unknown", "BusinessActivity", "Report", "ReportingFormation", "ReportRead", "ImportBusinessIntelligenceData" }, tRoleCode, new MenuElement("Отчетность", null)
            {
                URL = @"#",
                Children = checkChildrenMenuElement(new List<string>() {"Unknown", "BusinessActivity", "Report", "ReportingFormation", "ReportRead", "ImportBusinessIntelligenceData" }, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>(){"Unknown","BusinessActivity","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Стандартная", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","BusinessActivity","Report","ReportingFormation","ReportRead"}, tRoleCode, new List<MenuElement>()
                            {
                            checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Общества Группы", typeof(SocietyCalculatedField).Name)),
                            checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Имущество",  null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new List<MenuElement>()
                                    {
                                        //new MenuElement("Отчет по НИ",  "RealEstateMenuReport"),
                                        checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Сводная аналитика оценки", typeof(IndicateEstateAppraisalView).Name)),
                                        checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Стоимость права", typeof(RightCostView).Name)),
                                    })
                            }),


                            checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("ГГР",  null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new List<MenuElement>()
                                        {
                                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("ГГР (регистрация)", "GGR")),
                                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("ГГР (прекращение)", "ScheduleStateTerminateRecordPivot")),
                                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("ГГР Проверка данных о праве", "SSRCheckRight"))
                                        })
                            }),
                            checkMenuElement(new List<string>(){"Unknown","BusinessActivity","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Отчет о выбытии",  "LeavingReport")),

                            checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("ННА",  null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Отчет об ИК",  "ReportPCNonCoreAsset")),
                                        checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Перечень ННА (формат ЛНД)",  null) { URL = @"#reportCode=NCAListReport"}),
                                        checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Реестр ННА (формат ЛНД)",  null) { URL = @"#reportCode=NCAAndListReport"}),
                                    })
                            }),

                            checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Оценки",  null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>(){"Unknown","Report","ReportingFormation","ReportRead"}, tRoleCode, new MenuElement("Объекты оценки", "SummaryEstateAppraisal")),
                                    })
                            }),
                        })
                    }),

                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Контрольная",  null)
                        {
                                URL = @"#",
                                Children = checkChildrenMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new List<MenuElement>()
                                    {
                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Спорные ОБУ", "DisputeOBU")),
                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Контроль задвоения прав", typeof(DuplicateRightView).Name)),
                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Основные характеристики ОБУ ОИ Росреестр", nameof(AccountingEstateRightView))),
                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Основные характеристики ОБУ ОИ Сделка",  null) { URL = @"#reportCode=AOAndAppraisalReport" }),
                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Контрольный отчёт по имуществу",  null) { URL = @"#reportCode=EstateControlReport"}),
                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Задвоение объектов в рамках одной выписки",  null) { URL = @"#reportCode=DuplicateInExtractReport" }),
                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Сравнение выписок",  null) { URL = @"#reportCode=ExtractCompareReport" }),
                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation"}, tRoleCode, new MenuElement("Характеристики ОБУ и ОИ с данными по Росреестру",  null) { URL = @"#reportCode=EstateRosreestrControlReport" }),
                                    })
                    }),

                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Бизнес-аналитика", "PrintedFormList")
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown", "ReportingFormation", "ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Паспорт Общества Группы", null){URL = @"#?reportCode=pasportBook"}),
                            checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Анализ данных по Обществам Группы",null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Сводная аналитика по Обществам Группы",  null) { URL = @"#reportCode=rSummaryAnalytics" }),
                                        checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Выборки Обществ Группы",  null)
                                        {
                                                URL = @"#",
                                                Children = checkChildrenMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new List<MenuElement>()
                                                {
                                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Список Обществ Группы с детализацией",  null) { URL = @"#reportCode=AnalyzeReport_2_2" }),
                                                    checkMenuElement(new List<string>(){"Unknown","ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Распределение Обществ Группы по БС и ББ",  null) { URL = @"#reportCode=AnalyzeReport_2_9" }),
                                                })
                                        }),
                                    })
                            }),
                        })
                    }),
                })
            }));


            
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Unknown", "Default", "BusinessActivity", "EstateWrite", "IntangibleWrite", "AppraisalWrite", "NonCoreAssetWrite", "ScheduleStateWrite", "ImportOther"}, tRoleCode, new MenuElement("Документы АИС КС", "FileCardTree")));


            //menuPreset.MenuElements.Add(new MenuElement("Росреестр", null)
            //{
            //    URL = @"#",
            //    Children = new List<MenuElement>()
            //    {
            //        new MenuElement("Выписки", typeof(Extract).Name),
            //        new MenuElement("ОНИ", typeof(ObjectRecord).Name),
            //        new MenuElement("Права ЕГРН", typeof(RightRecord).Name),
            //        new MenuElement("Обременения/Ограничения ЕГРН", typeof(RestrictRecord).Name),                   
            //        new MenuElement("Документы ЕГРН", typeof(DocumentRecord).Name),
            //        new MenuElement("Субъекты", typeof(SubjectRecord).Name)
            //    }

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Unknown", "NSIManager", "ImportAccounting", "ImportOther", "ImportRosreestr"}, tRoleCode, new MenuElement("Интеграция", null)
            {
                URL = @"#",
                Children = checkChildrenMenuElement(new List<string>() {"Unknown", "NSIManager", "ImportAccounting", "ImportOther", "ImportRosreestr"}, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>(){"Unknown","ImportAccounting"}, tRoleCode, new MenuElement("Импорт данных бухгалтерского учета", "ImportAccountingObject")),
                    checkMenuElement(new List<string>(){"Unknown","NSIManager","ImportOther"}, tRoleCode, new MenuElement("Импорт прочих данных КИС", "ImportKIS")
                        { Children = checkChildrenMenuElement(new List<string>(){"Unknown","NSIManager","ImportOther"}, tRoleCode, new List<MenuElement>()
                                {
                                    checkMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new MenuElement("Данные об ОГ", "ImportSociety")),
                                    checkMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new MenuElement("Данные об Акционерах/Участии", "ImportShareholder")),
                                    checkMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new MenuElement("Данные о Сделках", "ImportDeal")),
                                    checkMenuElement(new List<string>(){"Unknown","NSIManager"}, tRoleCode, new MenuElement("Данные НСИ", "ImportNSI")),
                                    checkMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new MenuElement("Данные о ДП", null)
                                                {
                                                    URL = @"#",
                                                    Children = checkChildrenMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new List<MenuElement>()
                                                        {
                                                        checkMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new MenuElement("Основные данные", "ImportSubject")),
                                                        checkMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new MenuElement("Банковские реквизиты", "ImportBankingDetail")),
                                                        checkMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new MenuElement("Данные об оценщиках", "ImportAppraiserDataFinYear")),
                                                        checkMenuElement(new List<string>(){"Unknown","ImportOther"}, tRoleCode, new MenuElement("Данные оценочных организаций", "ImportAppraisalOrgData")),
                                                        })
                                                }),
                                })
                        }),
                    checkMenuElement(new List<string>(){"Unknown","ImportAccounting","ImportRosreestr"}, tRoleCode, new MenuElement("Импорт данных Росреестра", "ImportRosReestr")),

                    checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("Пользовательские файлы", "ImportOtherFiles")
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown"}, tRoleCode, new List<MenuElement>()
                        {
                        checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("Импорт данных о ГГР", "ImportSSR")),
                        checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("Импорт данных о ННА", "ImportNCA")),
                        //new MenuElement("Импорт данных об объектах оценки", "ImportEstateAppraisal"),
                        checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("Импорт данных об оценках", "ImportAppraisal")),
                        })
                    }),
                    checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("Журнал передачи данных", "ImportDataTransfer")),
                    checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("История импорта", typeof(ImportHistory).Name)),
                    checkMenuElement(new List<string>(){"Unknown"}, tRoleCode, new MenuElement("Журнал ошибок", typeof(ImportErrorLog).Name)),
                })
            }));

            menuPreset.MenuElements.RemoveAll(f => f.Name.ToLower() == "null");
            if (menuPreset.MenuElements.Count > 0)
                foreach (MenuElement me in menuPreset.MenuElements)
                {
                    me.RemoveNulls();
                }

            return menuPreset;
        }

        
        /// <summary>
        /// Создает и возвращает пункты навигацонного меню администратора.
        /// </summary>
        /// <returns></returns>
        public virtual MenuPreset CreateAdminMenu()
        {
            return CreateAdminMenu("");
        }

        /// <summary>
        /// Создает и возвращает пункты навигацонного меню администратора.
        /// </summary>
        /// <returns></returns>        
        public virtual MenuPreset CreateAdminMenu(string roleCode)
        {
            var tRole = this.uow.GetRepository<Base.Security.Role>()
                        .Filter(f => !f.Hidden && f.Code == roleCode)
                        .FirstOrDefault();
            var tRoleCode = (string.IsNullOrEmpty(roleCode)) ? "" : roleCode;//tRole?.Code;
            //TODO: наполнить меню администратора, т.е. убрать из меню админа пункты навигации для пользователей.
            //Добавим меню пользователя, т.к. сейчас демонстрации проходят 
            var menuPreset = CreateUserMenu(roleCode);
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() {"Unknown", "Admin", "SystemAdmin", "SecurityAdmin"}, tRoleCode, new MenuElement("Администрирование", null)
            {
                Children = checkChildrenMenuElement(new List<string>() {"Unknown", "Admin", "SystemAdmin", "SecurityAdmin"}, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>(){"Unknown","Admin","SystemAdmin","SecurityAdmin"}, tRoleCode, new MenuElement("Безопасность", null, "glyphicon glyphicon-lock")
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","Admin","SystemAdmin","SecurityAdmin"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"Unknown","Admin","SystemAdmin"}, tRoleCode, new MenuElement("Пользователи (Форма)", "AccessUserForm", "halfling halfling-user")),
                            checkMenuElement(new List<string>(){"Unknown","Admin","SystemAdmin"}, tRoleCode, new MenuElement("Пользователи (Active Directory)", "AccessUserAD", "halfling halfling-user")),
                            checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Роли", "Role", "glyphicon glyphicon-keys")),
                            checkMenuElement(new List<string>(){"Unknown","Admin","SecurityAdmin"}, tRoleCode, new MenuElement("Аудит", typeof(SettingItem).Name)
                            {
                                Children = checkChildrenMenuElement(new List<string>(){"Unknown","Admin","SecurityAdmin"}, tRoleCode, new List<MenuElement>()
                                {
                                    checkMenuElement(new List<string>(){"Unknown","Admin","SecurityAdmin"}, tRoleCode, new MenuElement("События аудита", typeof(AuditItem).Name))
                                })
                            }),
                            checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Территориальное распределение", nameof(SibUserTerritory))),
                        })
                    }),

                    checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Сервис", null, "halfling halfling-cog")
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new List<MenuElement>()
                        {
                                checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Уведомления", "SibNotification", "glyphicon glyphicon-envelope")),
                                checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Историчность", nameof(HistoricalSettings), "glyphicon glyphicon-notes")),
                                checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Бизнес-процессы", "BPWorkflow")),
                                checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Общие Пресеты", nameof(PresetRegistor))),
                                checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Пользовательские пресеты", "UserPreset")),
                                checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Настройка адреса сервиса отчетов", nameof(Base.Reporting.ReportingSetting))),
                                checkMenuElement(new List<string>(){"Unknown","Admin"}, tRoleCode, new MenuElement("Настройка форм отчетов", nameof(PrintedFormRegistry)))
                        })
                    }),

                    })
            }));



            menuPreset.MenuElements.RemoveAll(f => f.Name.ToLower() == "null");
            if (menuPreset.MenuElements.Count > 0)
                foreach (MenuElement me in menuPreset.MenuElements)
                {
                    me.RemoveNulls();
                }

            return menuPreset;
        }


        public virtual PresetRegistor CreateAdminMenu(IUnitOfWork unitOfWork)
        {

            var menuPreset = CreateAdminMenu();

            var menuPresetRegistor = new PresetRegistor()
            {
                Title = "Меню Администратора",
                Type = typeof(MenuPreset).GetTypeName(),
                For = "Menu",
                Preset = menuPreset
            };

            CreateOrUpdatePresetRegistor(unitOfWork, menuPresetRegistor);

            return menuPresetRegistor;

        }

        public virtual PresetRegistor CreateUserMenu(IUnitOfWork unitOfWork)
        {
            var menuPreset = CreateUserMenu();

            var menuPresetRegistor = new PresetRegistor()
            {
                Title = "Меню Пользователя",
                Type = typeof(MenuPreset).GetTypeName(),
                For = "Menu",
                Preset = menuPreset
            };

            CreateOrUpdatePresetRegistor(unitOfWork, menuPresetRegistor);
            return menuPresetRegistor;
        }


        protected void CreateOrUpdatePresetRegistor(IUnitOfWork unitOfWork, PresetRegistor presetRegistor)
        {
            var existPreset = _presetRegistorService.GetAll(unitOfWork).Any(registor => registor.For == presetRegistor.For);
            if (existPreset)
            {
                //_presetRegistorService.Update(unitOfWork, presetRegistor);
            }
            else
            {
                _presetRegistorService.Create(unitOfWork, presetRegistor);
            }
        }

    }
}
