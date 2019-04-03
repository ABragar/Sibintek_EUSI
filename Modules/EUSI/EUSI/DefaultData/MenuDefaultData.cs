using Base.Audit.Entities;
using Base.DAL;
using Base.Reporting.Entities;
using Base.Settings;
using Base.UI;
using Base.UI.Presets;
using Base.UI.Service;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Asset;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Estate;
using CorpProp.Entities.History;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Extentions;
using EUSI.Entities.Accounting;
using EUSI.Entities.Estate;
using EUSI.Entities.Import;
using EUSI.Entities.NU;
using EUSI.Entities.Report;
using EUSI.Model.Import;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EUSI.DefaultData
{
    /// <summary>
    /// Предоставляет методы для создания кастомизированного меню приложения.
    /// 
    /// </summary>
    public class MenuDefaultData: CorpProp.DefaultData.MenuDefaultData
    {
        private readonly IPresetRegistorService _presetRegistorService;
        private IUnitOfWork uow;

        public MenuDefaultData(IUnitOfWork unitOfWork, IPresetRegistorService presetRegistorService) : base(unitOfWork, presetRegistorService)
        {
            _presetRegistorService = presetRegistorService;
            uow = unitOfWork;
        }

        MenuElement GetMenuElementsByPath(MenuElement element, IEnumerable<MenuElement> elements, Queue<string> path)
        {
            try
            {
                if (path.Count <= 0)
                    return element;
                var menu = path.Dequeue();
                if (element?.Name == menu)
                    return element;
                    
                foreach (var childrenElement in elements)
                {
                    if (childrenElement.Name == menu)
                    {
                        MenuElement elementX = GetMenuElementsByPath(childrenElement, childrenElement.Children, path);
                        if (elementX != null)
                            return elementX;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;
        }
        private MenuElement checkMenuElement(List<string> listRole, string roleCode, MenuElement menuElement)
        {
            var tMenuElement = (listRole.Contains(roleCode)) ? menuElement : new MenuElement("Null", null);
            return tMenuElement;
        }
        private List<MenuElement> checkChildrenMenuElement(List<string> listRole, string roleCode, List<MenuElement> listMenuElement)
        {
            var children = new List<MenuElement>() { };

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
            var tRoleCode = (string.IsNullOrEmpty(roleCode)) ? "" : roleCode; // tRole?.Code;
            List<MenuElement> tChildren = new List<MenuElement>() { };

            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS", "InitEstateRegistration", "ImportEstateRegistration", "ResponsibleER" }, tRoleCode, new MenuElement("ОГ", typeof(Society).Name)));



            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleTransportOS", "InitEstateRegistration", "ImportEstateRegistration", "ResponsibleER" }, tRoleCode, new MenuElement("Заявки ЕУСИ", null)
            {
                Children = checkChildrenMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleTransportOS", "InitEstateRegistration", "ImportEstateRegistration", "ResponsibleER" }, tRoleCode, new List<MenuElement>()
                {
                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleTransportOS", "InitEstateRegistration", "ImportEstateRegistration", "ResponsibleER"},tRoleCode, new MenuElement("Заявки на регистрацию", typeof(EstateRegistration).Name)),
                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ImportEstateRegistration"},tRoleCode, new MenuElement("Журнал результатов загрузки данных", nameof(ExternalImportLog))),
                })
            }));

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "ResponsibleTransportOS", "TransportMovings", "ResponsibleReportControl", "ImportBusinessIntelligenceData" }, tRoleCode, new MenuElement("Учет", null)
            {
                Children = checkChildrenMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "ResponsibleTransportOS", "TransportMovings", "ResponsibleReportControl", "ImportBusinessIntelligenceData" }, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "ResponsibleTransportOS", "TransportMovings", "ResponsibleReportControl"   },tRoleCode, new MenuElement("Карточка ОС/НМА", typeof(AccountingObject).Name)
                    {
                        Children = checkChildrenMenuElement(new List<string>() {"UnknownEUSI", "Admin", "ResponsibleReportControl"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleReportControl"},tRoleCode, new MenuElement("Журнал расчета суммы налогов/авансовых платежей", null)
                            {
                                Children = checkChildrenMenuElement(new List<string>() {"UnknownEUSI", "Admin", "ResponsibleReportControl"}, tRoleCode, new List<MenuElement>()
                                {
                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleReportControl"},tRoleCode, new MenuElement("Расчет налога на имущество организаций", "CalculatingRecordInventory")),
                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleReportControl"},tRoleCode, new MenuElement("Расчет земельного налога ", "CalculatingRecordLand")),
                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleReportControl"},tRoleCode, new MenuElement("Расчет транспортного налога/Налога на имущество ", "CalculatingRecordTransport")),
                                })
                            })
                        })
                    }),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "ResponsibleTransportOS", "TransportMovings", "ResponsibleReportControl"   },tRoleCode, new MenuElement("ОС/НМА помеченные на удаление", "ArchivedObu")),

                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleTransportOS"},tRoleCode, new MenuElement("Журнал результатов загрузки в БУС", typeof(ExternalImportLog).Name)),

                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Регистр движений", null)
                    {
                        Children = checkChildrenMenuElement(new List<string>() {"UnknownEUSI", "Admin", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "TransportMovings"  }, tRoleCode, new List<MenuElement>()
                        {
                           checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("РСБУ", "AccMovingRSBU")),
                           checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("МСФО", "AccMovingMSFO")),
                           checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Упрощенное внедрение", nameof(AccountingMovingMSFO))
                            {
                                Children = checkChildrenMenuElement(new List<string>() {"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  }, tRoleCode, new List<MenuElement>()
                                {
                                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Дебет 01", "Debit01")),
                                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Кредит 01", "Credit01")),
                                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Амортизация 01", "Depreciation01")),
                                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Дебет 07", "Debit07")),
                                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Кредит 07", "Credit07")),
                                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Дебет 08", "Debit08")),
                                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "TransportMovings"  },tRoleCode, new MenuElement("Кредит 08", "Credit08")),
                                })
                            }),
                        })
                    }),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS" },tRoleCode, new MenuElement("Карточка ОИ", null)
                    {
                         Children = checkChildrenMenuElement(new List<string>() {"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS" }, tRoleCode, new List<MenuElement>()
                         {
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS" },tRoleCode, new MenuElement("Материальные активы", "InventoryObjectMenuList")
                            {
                                Children = checkChildrenMenuElement (new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new List<MenuElement>()
                                {
                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("Недвижимое имущество", "RealEstateMenuList")
                                    {
                                        Children = checkChildrenMenuElement (new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new List<MenuElement>()
                                        {
                                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("Кадастровые объекты", "CadastralMenuList")
                                            {
                                                Children = checkChildrenMenuElement (new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new List<MenuElement>()
                                                {
                                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("Земельные участки", "LandMenuList")),
                                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("Здания/сооружения", "BuildingStructureMenuList")
                                                    {
                                                        Children = checkChildrenMenuElement (new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new List<MenuElement>()
                                                    {
                                                        checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("Помещения",  "RoomMenuList")),
                                                        checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("Машиноместа", "CarParkingSpaceMenuList")),
                                                    }),
                                                    }),
                                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("НЗС", "UnfinishedConstructionMenuList")),
                                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("ЕНК (Единый недвижимый комплекс", "RealEstateComplexMenuList")),
                                                }),
                                            }),

                                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("Речные/морские суда", "ShipMenuList")),
                                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"},tRoleCode, new MenuElement("Воздушные суда", "AircraftMenuList")),
                                            //new MenuElement("Космические объекты", null) { URL = @"#" },
                                        }),
                                    }),
                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"}, tRoleCode, new MenuElement("Движимое имущество", "MovableEstateMenuList")
                                    {
                                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"}, tRoleCode, new List<MenuElement>()
                                        {
                                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"}, tRoleCode, new MenuElement("Транспортные средства", "VehicleMenuList"))
                                        })
                                    }),
                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"}, tRoleCode, new MenuElement("Имущественные комплексы", "InventoryObjectTree")),

                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS"}, tRoleCode, new MenuElement("ОИ помеченные на удаление", "ArchivedEstate"))
                                }),
                            }),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO" , "ResponsibleRichEstate", "ResponsibleRichOS" }, tRoleCode, new MenuElement("НМА", typeof(IntangibleAsset).Name)),
                          }),
                     }),

                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Права АИС КС", typeof(Right).Name)),
                    //new MenuElement("Имущественные комплексы", "InventoryObjectTree"), 
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Данные Росреестра", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Выписки о правах юридического лица", "ExtractSubj")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Выписки о характеристиках объекта недвижимости", "ExtractObject")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ОНИ", "ObjectRecord")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Права ЕГРН", "RightRecord")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Обременения/Ограничения ЕГРН", "RestrictRecord")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Документы ЕГРН", "DocumentRecord")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Субъекты", "SubjectRecord"))
                        })
                    }),
                    checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Данные расширенной отчетности", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Банковские счёта", "BankAccount")),
                            checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Финансовый показатель общества", "FinancialIndicatorItem")),
                            checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Исполнение бюджета", "RecordBudgetLine")),
                            checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Дополнительные сведения об ОГ", "AnalyzeSociety")),
                        })
                    }),
                   checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleReportControl"}, tRoleCode, new MenuElement("Налоговые декларации", typeof(Declaration).Name))
                })
            }));

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Управление", null)
            {
                Children = checkChildrenMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Сделки", typeof(CorpProp.Entities.DocumentFlow.SibDeal).Name)),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Оценка", typeof(Appraisal).Name)
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Сводный реестр оценок", "SummaryAppraisal")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Оценки по исполнителям", "SummaryAppraisalExecutor")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Оценочные организации ", typeof(Appraiser).Name))
                        })
                    }),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Гос. регистрация", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Регистрация права", typeof(ScheduleStateRegistration).Name)),
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Регистрация прекращения права", typeof(ScheduleStateTerminate).Name)),
                                //checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ГГР (регистрация)", "ScheduleStateRegistrationRecordPivot")),
                                //checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ГГР (прекращение)", "ScheduleStateTerminateRecordPivot")),
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Сводный ГГР на год", typeof(ScheduleStateYear).Name))
                        })

                    }),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Проекты и задачи", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Проекты", "SibProjectMenuList")),
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Шаблоны проектов ", "SibProjectTemplate")),
                                //checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Задачи", typeof(SibTask).Name)),
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Задачи", "SibTaskMenuList")),
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Шаблоны задач", "SibTaskTemplate"))
                        })

                    }),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ННА", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Объекты ННА", typeof(NonCoreAsset).Name)),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Перечни ННА", typeof(NonCoreAssetList).Name)),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Реестры ННА", typeof(NonCoreAssetInventory).Name)
                            {
                                Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>() {
                                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("К исполнению", "NonCoreAssetAndListMenu"))
                                })
                            }),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Способы распоряжения активами", typeof(NonCoreAssetSaleOffer).Name)),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Одобрения реализации", typeof(NonCoreAssetSaleAccept).Name)),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Перечни к реализации", typeof(NonCoreAssetSale).Name)),
                        })

                    }),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Запросы ЦАУК", typeof(CorpProp.Entities.Request.Request).Name)),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Запрос в ОГ", "Response")
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Входящие запросы", "IncomingResponse")),
                                checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Исходящие ответы", "OutgoingResponse"))
                        })
                    }),
                })
            }));

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS", "InitEstateRegistration", "ImportEstateRegistration", "ResponsibleER" }, tRoleCode, new MenuElement("НСИ", nameof(NSI))));
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "NonUnknownEUSI" }, tRoleCode, new MenuElement("Деловые партнеры", typeof(Subject).Name)));
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "NonUnknownEUSI" }, tRoleCode, new MenuElement("Данные об Акционерах/Участии", typeof(Shareholder).Name)));

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleTransportOS", "ResponsibleReportControl", "ImportBusinessIntelligenceData" }, tRoleCode, new MenuElement("Отчетность", null)
            {

                URL = @"#",
                Children = checkChildrenMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleTransportOS", "ResponsibleReportControl", "ImportBusinessIntelligenceData" }, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Стандартная", null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                            {
                            checkMenuElement(new List<string>(){ "UnknownEUSI", "Report","ReportingFormation","ReportRead", "ResponsibleReportControl"}, tRoleCode, new MenuElement("Общества Группы", typeof(SocietyCalculatedField).Name)),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Имущество",  null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                                    {
                                        //new MenuElement("Отчет по НИ",  "RealEstateMenuReport"),
                                        checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Сводная аналитика оценки", typeof(IndicateEstateAppraisalView).Name)),
                                        checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Стоимость права", typeof(RightCostView).Name)),
                                    })
                            }),


                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ГГР",  null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                                        {
                                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ГГР (регистрация)", "GGR")),
                                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ГГР (прекращение)", "ScheduleStateTerminateRecordPivot")),
                                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ГГР Проверка данных о праве", "SSRCheckRight"))
                                        })
                            }),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Отчет о выбытии",  "LeavingReport")),

                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("ННА",  null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Отчет об ИК",  "ReportPCNonCoreAsset")),
                                        checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Перечень ННА (формат ЛНД)",  null) { URL = @"#reportCode=NCAListReport"}),
                                        checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Реестр ННА (формат ЛНД)",  null) { URL = @"#reportCode=NCAAndListReport"}),
                                    })
                            }),

                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Оценки",  null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Объекты оценки", "SummaryEstateAppraisal")),
                                    })
                            }),
                        })
                    }),

                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleTransportOS", "ResponsibleReportControl"}, tRoleCode, new MenuElement("Контрольная",  null)
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleTransportOS", "ResponsibleReportControl"}, tRoleCode, new List<MenuElement>()
                        {
                            
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleTransportOS", "ResponsibleReportControl"}, tRoleCode, new MenuElement("Контроль полученных данных по прототипам ОС/НМА из БУС", "DraftOS")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleTransportOS", "ResponsibleReportControl"}, tRoleCode, new MenuElement("Контроль отправки уведомлений по прототипам ОС/НМА", "DraftOSPassBus")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleTransportOS", "ResponsibleReportControl"}, tRoleCode, new MenuElement("Контроль передачи данных по действующим ОС/НМА из БУС", "NoImportDataOS")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Спорные ОБУ", "DisputeOBU")),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Контроль задвоения прав", typeof(DuplicateRightView).Name)),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Основные характеристики ОБУ ОИ Росреестр", nameof(AccountingEstateRightView))),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Основные характеристики ОБУ ОИ Сделка",  null) { URL = @"#reportCode=AOAndAppraisalReport" }),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Контрольный отчёт по имуществу",  null) { URL = @"#reportCode=EstateControlReport"}),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Задвоение объектов в рамках одной выписки",  null) { URL = @"#reportCode=DuplicateInExtractReport" }),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Сравнение выписок",  null) { URL = @"#reportCode=ExtractCompareReport" }),
                            checkMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Характеристики ОБУ и ОИ с данными по Росреестру",  null) { URL = @"#reportCode=EstateRosreestrControlReport" }),
                        })
                    }),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleReportControl" }, tRoleCode, new MenuElement("Бизнес-аналитика", "PrintedFormList")
                    {
                        URL = @"#",
                        Children = checkChildrenMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation", "ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Паспорт Общества Группы", null){URL = @"#?reportCode=pasportBook"}),
                            checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Анализ данных по Обществам Группы",null)
                            {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Сводная аналитика по Обществам Группы",  null) { URL = @"#reportCode=rSummaryAnalytics" }),
                                        checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Выборки Обществ Группы",  null)
                                        {
                                                URL = @"#",
                                                Children = checkChildrenMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new List<MenuElement>()
                                                {
                                                    checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Список Обществ Группы с детализацией",  null) { URL = @"#reportCode=AnalyzeReport_2_2" }),
                                                    checkMenuElement(new List<string>(){ "UnknownEUSI", "Admin", "ReportingFormation","ReportRead", "ImportBusinessIntelligenceData"}, tRoleCode, new MenuElement("Распределение Обществ Группы по БС и ББ",  null) { URL = @"#reportCode=AnalyzeReport_2_9" }),
                                                })
                                        }),
                                    })
                            }),
                        })
                    }),
                    checkMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleReportControl"}, tRoleCode, new MenuElement("Журнал контроля", nameof(ReportMonitoring))),
                })
            }));

            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsiblePropertyComplexIO", "ResponsibleRichEstate", "ResponsibleRichOS", "ResponsibleBUSDataImport", "ResponsibleReportControl", "InitEstateRegistration", "ImportEstateRegistration", "ResponsibleER" }, tRoleCode, new MenuElement("Документы", "FileCardTree")));
                       
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "ResponsibleTransportOS", "ResponsibleReportControl", "ImportEstateRegistration", "ResponsibleMigrateOS" }, tRoleCode, new MenuElement("Интеграция", null)
            {
                URL = @"#",
                Children = checkChildrenMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleBUSDataImport", "ResponsibleTransportOS", "ResponsibleReportControl", "ImportEstateRegistration", "ResponsibleMigrateOS" }, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт ИР-Аренда", null)
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт ФСД (аренда)", "ImportRentalOS")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт ФСД (движения)", "ImportRentalOSMoving")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт ФСД (состояния)", "ImportRentalOSState")),
                        })
                    }),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ImportEstateRegistration" }, tRoleCode, new MenuElement("Импорт заявок на регистрацию", "ImportEstateRegistration")
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Заявки. Отчет", ImportErrorLogModel.ImportErrorLogEstateRegistration)),
                        })
                    }),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleBUSDataImport"  }, tRoleCode, new MenuElement("Импорт данных бухгалтерского учета", "ImportAccountingObject")),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleReportControl" }, tRoleCode, new MenuElement("Импорт данных (ЕУСИ)", null)
                    {
                    Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin", "ResponsibleReportControl"}, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleReportControl" }, tRoleCode, new MenuElement("Импорт данных BCS", "ImportBCSData")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleReportControl" }, tRoleCode, new MenuElement("Импорт данных ИС НА", "ImportDeclaration")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleReportControl" }, tRoleCode, new MenuElement("Импорт протокола сверки сальдо", "ImportSaldo")),
                        })
                    }),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт прочих данных КИС", "ImportKIS")
                        {
                            Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                            {
                                checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Данные об ОГ", "ImportSociety")),
                                checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Данные об Акционерах/Участии", "ImportShareholder")),
                                checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Данные о Сделках", "ImportDeal")),
                                checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Данные НСИ", "ImportNSI")),
                                checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Данные о ДП", null)
                                {
                                    URL = @"#",
                                    Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                                    {
                                        checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Основные данные", "ImportSubject")),
                                        checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Банковские реквизиты", "ImportBankingDetail")),
                                        checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Данные об оценщиках", "ImportAppraiserDataFinYear")),
                                        checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Данные оценочных организаций", "ImportAppraisalOrgData")),
                                    })
                                }),
                            })
                        }),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт данных Росреестра", "ImportRosReestr")),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт данных ИС НА", "ImportDeclaration")),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Пользовательские файлы", "ImportOtherFiles")
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт данных о ГГР", "ImportSSR")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт данных о ННА", "ImportNCA")),
                            //checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт данных об объектах оценки", "ImportEstateAppraisal")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Импорт данных об оценках", "ImportAppraisal")),
                        })
                    }),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleTransportOS" }, tRoleCode, new MenuElement("Журнал передачи данных", "ImportDataTransfer")),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("История импорта", typeof(ImportHistory).Name)),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Журнал ошибок", typeof(ImportErrorLog).Name)),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin", "ResponsibleTransportOS" }, tRoleCode, new MenuElement("Импорт журнала загрузки из БУС", "ImportExternalLog")),
                    checkMenuElement(new List<string>() { "UnknownEUSI", "ResponsibleMigrateOS" }, tRoleCode, new MenuElement("Миграция ОС/НМА", "ImportMigrateOS")),
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
            menuPreset.MenuElements.Add(checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Администрирование", null)
            {
                Children = checkChildrenMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                {
                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Безопасность", null, "glyphicon glyphicon-lock")
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {

                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Пользователи (Форма)", "AccessUserForm", "halfling halfling-user")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Пользователи (Active Directory)", "AccessUserAD", "halfling halfling-user")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Роли", "Role", "glyphicon glyphicon-keys")),
                            checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Аудит", typeof(SettingItem).Name)
                            {
                               Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                                 {
                                    checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("События аудита", typeof(AuditItem).Name))
                                 })
                            }),

                           checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Территориальное распределение", nameof(SibUserTerritory))),

                        })
                    }),

                     checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode,new MenuElement("Сервис", null, "halfling halfling-cog")
                    {
                        Children = checkChildrenMenuElement(new List<string>(){"UnknownEUSI", "Admin" }, tRoleCode, new List<MenuElement>()
                        {
                             checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Уведомления", "SibNotification", "glyphicon glyphicon-envelope")),
                             checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Историчность", nameof(HistoricalSettings), "glyphicon glyphicon-notes")),
                             checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Бизнес-процессы", "BPWorkflow")),
                             checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Общие Пресеты", nameof(PresetRegistor))),
                             checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Пользовательские пресеты", "UserPreset")),
                             checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Настройка адреса сервиса отчетов", nameof(Base.Reporting.ReportingSetting))),
                             checkMenuElement(new List<string>() { "UnknownEUSI", "Admin" }, tRoleCode, new MenuElement("Настройка форм отчетов", nameof(PrintedFormRegistry)))
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

        /// <summary>
        /// Меню Ответственный за формирование карточки ИК
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateResponsibleIKMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(new MenuElement("ОГ", nameof(Society)));
            menuPreset.MenuElements.Add(new MenuElement("ЕУСИ", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Учет", null)
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Карточка ОС/НМА", typeof(AccountingObject).Name),                            
                            new MenuElement("Карточка ОИ", null)
                            {
                                 Children = new List<MenuElement>()
                                 {
                                    new MenuElement("Материальные активы", "InventoryObjectMenuList")
                                    {
                                         Children = new List<MenuElement>()
                                         {
                                             new MenuElement("Недвижимое имущество", "RealEstateMenuList")
                                             {
                                                 Children = new List<MenuElement>()
                                                 {
                                                     new MenuElement("Кадастровые объекты", "CadastralMenuList")
                                                     {
                                                      Children = new List<MenuElement>()
                                                       {
                                                         new MenuElement("Земельные участки", "LandMenuList"),
                                                         new MenuElement("Здания/сооружения", "BuildingStructureMenuList")

                                                           {
                                                             Children = new List<MenuElement>()
                                                            {
                                                              new MenuElement("Помещения",  "RoomMenuList"),
                                                              new MenuElement("Машиноместа", "CarParkingSpaceMenuList")

                                                            }
                                                           },
                                                         new MenuElement("НЗС", "UnfinishedConstructionMenuList"),
                                                         new MenuElement("ЕНК (Единый недвижимый комплекс", "RealEstateComplexMenuList"),
                                                        },
                                                     },
                                                     new MenuElement("Речные/морские суда", "ShipMenuList"),
                                                     new MenuElement("Воздушные суда", "AircraftMenuList"),    
                                                 }
                                             },
                                             new MenuElement("Движимое имущество", "MovableEstateMenuList")
                                             {
                                                  Children = new List<MenuElement>()
                                                  {
                                                      new MenuElement("Транспортные средства", "VehicleMenuList")
                                                  }
                                             },
                                             new MenuElement("Имущественные комплексы", "InventoryObjectTree")
                                         }
                                    },                                    
                                 }
                            },

                        }
                    }
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("НСИ", nameof(NSI)));
            menuPreset.MenuElements.Add(new MenuElement("Документы", "FileCardTree"));
            return menuPreset;
        }

        /// <summary>
        /// Меню Ответственный за дообогащение Карточки ОИ/ОС
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateResponsibleRichEstateMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(new MenuElement("ОГ", nameof(Society)));
            menuPreset.MenuElements.Add(new MenuElement("ЕУСИ", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Учет", null)
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Карточка ОС/НМА", typeof(AccountingObject).Name),
                            new MenuElement("Регистр движений", null)
                            {
                                Children = new List<MenuElement>()
                                {
                                   new MenuElement("РСБУ", "AccMovingRSBU"),
                                   new MenuElement("МСФО", "AccMovingMSFO"),                                  
                                }
                            },
                            new MenuElement("Карточка ОИ", null)
                            {
                                 Children = new List<MenuElement>()
                                 {
                                    new MenuElement("Материальные активы", "InventoryObjectMenuList")
                                    {
                                         Children = new List<MenuElement>()
                                         {
                                             new MenuElement("Недвижимое имущество", "RealEstateMenuList")
                                             {
                                                 Children = new List<MenuElement>()
                                                 {
                                                     new MenuElement("Кадастровые объекты", "CadastralMenuList")
                                                     {
                                                      Children = new List<MenuElement>()
                                                       {
                                                         new MenuElement("Земельные участки", "LandMenuList"),
                                                         new MenuElement("Здания/сооружения", "BuildingStructureMenuList")

                                                           {
                                                             Children = new List<MenuElement>()
                                                            {
                                                              new MenuElement("Помещения",  "RoomMenuList"),
                                                              new MenuElement("Машиноместа", "CarParkingSpaceMenuList")

                                                            }
                                                           },
                                                         new MenuElement("НЗС", "UnfinishedConstructionMenuList"),
                                                         new MenuElement("ЕНК (Единый недвижимый комплекс", "RealEstateComplexMenuList"),
                                                        },
                                                     },

                                                     new MenuElement("Речные/морские суда", "ShipMenuList"),
                                                     new MenuElement("Воздушные суда", "AircraftMenuList"),  
                                                     //new MenuElement("Космические объекты", null) { URL = @"#" },
                                     
                                                 }
                                             },
                                             new MenuElement("Движимое имущество", "MovableEstateMenuList")
                                             {
                                                  Children = new List<MenuElement>()
                                                  {
                                                      new MenuElement("Транспортные средства", "VehicleMenuList")
                                                  }
                                             },
                                             new MenuElement("Имущественные комплексы", "InventoryObjectTree")
                                         }
                                    },
                                    new MenuElement("НМА", typeof(IntangibleAsset).Name),
                                 }
                            },
                            
                        }
                    }
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("НСИ", nameof(NSI)));
            menuPreset.MenuElements.Add(new MenuElement("Документы", "FileCardTree"));
            return menuPreset;
        }

        /// <summary>
        /// Меню Ответственный за загрузку данных БУС
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateImportBUSDAtaRoleMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };
            
            menuPreset.MenuElements.Add(new MenuElement("ЕУСИ", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Учет", null)
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Карточка ОС/НМА", typeof(AccountingObject).Name),
                            new MenuElement("Регистр движений", null)
                            {
                                Children = new List<MenuElement>()
                                {
                                   new MenuElement("РСБУ", "AccMovingRSBU"),
                                   new MenuElement("МСФО", "AccMovingMSFO"),
                                   new MenuElement("Упрощенное внедрение", nameof(AccountingMovingMSFO))
                                    {
                                        Children = new List<MenuElement>()
                                        {
                                           new MenuElement("Дебет 01", "Debit01"),
                                           new MenuElement("Кредит 01", "Credit01"),
                                           new MenuElement("Амортизация 01", "Depreciation01"),
                                           new MenuElement("Дебет 07", "Debit07"),
                                           new MenuElement("Кредит 07", "Credit07"),
                                           new MenuElement("Дебет 08", "Debit08"),
                                           new MenuElement("Кредит 08", "Credit08"),
                                        }
                                    },
                                }
                            },                         

                        }
                    }
                }
            });          
            menuPreset.MenuElements.Add(new MenuElement("Документы", "FileCardTree"));
            menuPreset.MenuElements.Add(new MenuElement("Интеграция", null)
            {
                URL = @"#",
                Children = new List<MenuElement>()
                {                    
                    new MenuElement("Импорт данных бухгалтерского учета", "ImportAccountingObject"),
                }
            });
            return menuPreset;
        }

        /// <summary>
        /// Меню Ответственный за передачу данных
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateTransportOSRoleMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(new MenuElement("ЕУСИ", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Заявки ЕУСИ", null)
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Заявки на регистрацию", typeof(EstateRegistration).Name),
                        }
                    },
                    new MenuElement("Учет", null)
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Карточка ОС/НМА", typeof(AccountingObject).Name),
                            new MenuElement("Журнал результатов загрузки в БУС", typeof(ExternalImportLog).Name),
                        }
                    }
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("Отчетность", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Контрольная", null)
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Контроль полученных данных по прототипам ОС/НМА из БУС", "DraftOS"),
                           
                        }
                    },                    
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("Интеграция", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Журнал передачи данных", "ImportDataTransfer"),
                    new MenuElement("Импорт журнала загрузки из БУС", "ImportExternalLog"),
                }
            });
           
            return menuPreset;
        }

        /// <summary>
        /// Меню Ответственный за выгрузку в шаблоны параллельного учета.
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateTransportMovingsRoleMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(new MenuElement("ЕУСИ", null)
            {
                Children = new List<MenuElement>()
                {                   
                    new MenuElement("Учет", null)
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Карточка ОС/НМА", typeof(AccountingObject).Name),
                            new MenuElement("Регистр движений", null)
                            {
                                Children = new List<MenuElement>()
                                {
                                   new MenuElement("РСБУ", "AccMovingRSBU"),
                                   new MenuElement("МСФО", "AccMovingMSFO"),
                                   new MenuElement("Упрощенное внедрение", nameof(AccountingMovingMSFO))
                                    {
                                        Children = new List<MenuElement>()
                                        {
                                           new MenuElement("Дебет 01", "Debit01"),
                                           new MenuElement("Кредит 01", "Credit01"),
                                           new MenuElement("Амортизация 01", "Depreciation01"),
                                           new MenuElement("Дебет 07", "Debit07"),
                                           new MenuElement("Кредит 07", "Credit07"),
                                           new MenuElement("Дебет 08", "Debit08"),
                                           new MenuElement("Кредит 08", "Credit08"),
                                        }
                                    },
                                }
                            },
                        }
                    }
                }
            });

            return menuPreset;
        }

        /// <summary>
        /// Меню Ответственный за выполнение контролей.
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateReportControlRoleMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };
            menuPreset.MenuElements.Add(new MenuElement("Налоговые декларации", nameof(Declaration)));
            menuPreset.MenuElements.Add(new MenuElement("Журнал расчета суммы налогов/авансовых платежей", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Расчет налога на имущество организаций", "CalculatingRecordInventory"),
                    new MenuElement("Расчет земельного налога ", "CalculatingRecordLand"),
                    new MenuElement("Расчет транспортного налога/Налога на имущество ", "CalculatingRecordTransport"),
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("Отчетность", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Бизнес-аналитика", nameof(PrintedFormList)),
                    new MenuElement("Контрольная", null) {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Контроль полученных данных по прототипам ОС/НМА из БУС", "DraftOS"),
                            new MenuElement("Контроль передачи данных по действующим ОС/НМА из БУС", "NoImportDataOS"),
                        }
                    },
                    new MenuElement("Журнал контроля", nameof(ReportMonitoring)),
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("Документы", "FileCardTree"));
            menuPreset.MenuElements.Add(new MenuElement("Интеграция", null)
            {
                URL = @"#",
                Children = new List<MenuElement>()
                {
                    new MenuElement("Импорт данных BCS", "ImportBCSData"),
                    new MenuElement("Импорт данных ИС НА", "ImportDeclaration"),
                    new MenuElement("Импорт протокола сверки сальдо", "ImportSaldo"),
                }
            });

            return menuPreset;
        }


        /// <summary>
        /// Меню Инициатор Заявки ЕУСИ
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateInitRoleMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(new MenuElement("ОГ", nameof(Society)));
            menuPreset.MenuElements.Add(new MenuElement("Заявки ЕУСИ", null)
            {
                Children = new List<MenuElement>()
                {
                   new MenuElement("Заявки на регистрацию", typeof(EstateRegistration).Name),
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("НСИ", nameof(NSI)));
            menuPreset.MenuElements.Add(new MenuElement("Документы", "FileCardTree"));
            return menuPreset;
        }

        /// <summary>
        /// Меню Ответственный за загрузку Заявки ЕУСИ
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateImportERRoleMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(new MenuElement("ОГ", nameof(Society)));
            menuPreset.MenuElements.Add(new MenuElement("Заявки ЕУСИ", null)
            {
                Children = new List<MenuElement>()
                {
                   new MenuElement("Заявки на регистрацию", typeof(EstateRegistration).Name),
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("НСИ", nameof(NSI)));
            menuPreset.MenuElements.Add(new MenuElement("Документы", "FileCardTree"));
            menuPreset.MenuElements.Add(new MenuElement("Интеграция", null)
            {
                URL = @"#",
                Children = new List<MenuElement>()
                {
                    new MenuElement("Импорт заявок на регистрацию", "ImportEstateRegistration"),
                    new MenuElement("Журнал результатов загрузки данных", nameof(ExternalImportLog)),
                }
            });
            return menuPreset;
        }

        /// <summary>
        /// Меню Ответственный за проверку Заявки ЕУСИ
        /// </summary>
        /// <returns></returns>
        public MenuPreset CreateCheckERRoleMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(new MenuElement("ОГ", nameof(Society)));
            menuPreset.MenuElements.Add(new MenuElement("Заявки ЕУСИ", null)
            {
                Children = new List<MenuElement>()
                {
                   new MenuElement("Заявки на регистрацию", typeof(EstateRegistration).Name),
                }
            });
            menuPreset.MenuElements.Add(new MenuElement("НСИ", nameof(NSI)));
            menuPreset.MenuElements.Add(new MenuElement("Документы", "FileCardTree"));
            return menuPreset;
        }
    }
}
