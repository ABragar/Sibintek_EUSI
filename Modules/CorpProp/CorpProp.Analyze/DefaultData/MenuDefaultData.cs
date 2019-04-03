using System.Collections.Generic;
using Base.Audit.Entities;
using Base.DAL;
using Base.Settings;
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

namespace CorpProp.Analyze.DefaultData
{
    public class MenuDefaultData : CorpProp.DefaultData.MenuDefaultData
    {
        public MenuDefaultData(IUnitOfWork unitOfWork, IPresetRegistorService presetRegistorService) : base(unitOfWork, presetRegistorService)
        {
        }
        /// <summary>
        /// Создает и возвращает пункты навигацонного меню пользователя.
        /// </summary>
        /// <returns></returns>
        public override MenuPreset CreateUserMenu()
        {
            var menuPreset = new MenuPreset()
            {
                For = "Menu",
            };

            menuPreset.MenuElements.Add(new MenuElement("ОГ", typeof(Society).Name));


            menuPreset.MenuElements.Add(new MenuElement("Учет", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Карточка ОС/НМА", typeof(AccountingObject).Name),
                    new MenuElement("Карточка ОИ", typeof(AccountingObject).Name)
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


                    new MenuElement("Права", typeof(Right).Name),
                    new MenuElement("Имущественные комплексы", "InventoryObjectTree"),
                    new MenuElement("Данные Росреестра", null)
                        {
                            URL = @"#",
                            Children = new List<MenuElement>()
                            {
                                new MenuElement("Выписки о правах юридического лица", "ExtractSubj"),
                                new MenuElement("Выписки о характеристиках объекта недвижимости", "ExtractObject"),
                                new MenuElement("ОНИ", "ObjectRecord"),
                                new MenuElement("Права", "RightRecord"),
                                new MenuElement("Обременения/Ограничения", "RestrictRecord"),
                                new MenuElement("Документы", "DocumentRecord"),
                                new MenuElement("Субъекты", "SubjectRecord")
                            }
                        }



                }
            });

            menuPreset.MenuElements.Add(new MenuElement("Управление", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Сделки", typeof(CorpProp.Entities.DocumentFlow.SibDeal).Name),
                    new MenuElement("Оценка", typeof(Appraisal).Name)
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Сводный реестр оценок", "SummaryAppraisal"),
                            new MenuElement("Оценки по исполнителям", "SummaryAppraisalExecutor"),
                            new MenuElement("Оценочные организации ", typeof(Appraiser).Name)
                        }
                    },
                    new MenuElement("Гос. регистрация", null)
                    {
                        URL = @"#",
                        Children = new List<MenuElement>()
                        {
                             new MenuElement("Регистрация права", typeof(ScheduleStateRegistration).Name),
                             new MenuElement("Регистрация прекращения права", typeof(ScheduleStateTerminate).Name),
                             //new MenuElement("ГГР (регистрация)", "ScheduleStateRegistrationRecordPivot"),
                             //new MenuElement("ГГР (прекращение)", "ScheduleStateTerminateRecordPivot"),
                          
                             new MenuElement("Сводный ГГР на год", typeof(ScheduleStateYear).Name)
                        }

                    },
                    new MenuElement("Проекты и задачи", null)
                    {
                        URL = @"#",
                        Children = new List<MenuElement>()
                        {
                             new MenuElement("Проекты", "SibProjectMenuList"),
                             new MenuElement("Шаблоны проектов ", "SibProjectTemplate"),
                             //new MenuElement("Задачи", typeof(SibTask).Name)
                             new MenuElement("Задачи", "SibTaskMenuList"),
                             new MenuElement("Шаблоны задач", "SibTaskTemplate")
                        }

                    },
                    new MenuElement("ННА", null)
                    {
                        URL = @"#",
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Объекты ННА", typeof(NonCoreAsset).Name),
                            new MenuElement("Перечни ННА", typeof(NonCoreAssetList).Name),
                            new MenuElement("Реестры ННА", typeof(NonCoreAssetInventory).Name)
                            {
                                Children =  new List<MenuElement>() {
                                    new MenuElement("К исполнению", "NonCoreAssetAndListMenu")
                            }
                            },
                            new MenuElement("Способы распоряжения активами", typeof(NonCoreAssetSaleOffer).Name),
                            new MenuElement("Одобрения реализации", typeof(NonCoreAssetSaleAccept).Name),
                            new MenuElement("Перечни к реализации", typeof(NonCoreAssetSale).Name),
                        }

                    },
                    new MenuElement("Запросы ЦАУК", typeof(CorpProp.Entities.Request.Request).Name),
                    new MenuElement("Запрос в ОГ", "Response")
                    {
                        URL = @"#",
                        Children = new List<MenuElement>()
                        {
                             new MenuElement("Входящие запросы", "IncomingResponse"),
                             new MenuElement("Исходящие ответы", "OutgoingResponse")
                        }
                    },

                }
            });

            menuPreset.MenuElements.Add(new MenuElement("НСИ", nameof(NSI)));

            menuPreset.MenuElements.Add(new MenuElement("Отчетность", null)
            {

                URL = @"#",
                Children = new List<MenuElement>()
                {
                    new MenuElement("Стандартная", null)
                    {
                    URL = @"#",
                    Children = new List<MenuElement>()
                       {
                        new MenuElement("Имущество",  null)
                        {
                             URL = @"#",
                              Children = new List<MenuElement>()
                                  {
                                   //new MenuElement("Отчет по НИ",  "RealEstateMenuReport"),
                                   new MenuElement("Сводная аналитика оценки", typeof(IndicateEstateAppraisalView).Name),

                                   new MenuElement("Стоимость права", typeof(RightCostView).Name),
                                  }
                        },


                        new MenuElement("ГГР",  null)
                        {
                             URL = @"#",
                              Children = new List<MenuElement>()
                                  {
                                     new MenuElement("ГГР (регистрация)", "GGR"),
                                     new MenuElement("ГГР (прекращение)", "ScheduleStateTerminateRecordPivot"),
                                     new MenuElement("ГГР Проверка данных о праве", "SSRCheckRight")
                                  }
                        },
                        new MenuElement("Отчет о выбытии",  "LeavingReport"),

                        new MenuElement("ННА",  null)
                        {
                             URL = @"#",
                              Children = new List<MenuElement>()
                                  {
                                   new MenuElement("Отчет об ИК",  "ReportPCNonCoreAsset"),
                                   new MenuElement("Перечень ННА (формат ЛНД)",  null) { URL = @"#reportCode=NCAListReport"},
                                   new MenuElement("Реестр ННА (формат ЛНД)",  null) { URL = @"#reportCode=NCAAndListReport"},
                                  }
                        },

                        new MenuElement("Оценки",  null)
                        {
                             URL = @"#",
                              Children = new List<MenuElement>()
                                  {
                                   new MenuElement("Объекты оценки", "SummaryEstateAppraisal"),
                                  }
                        },
                       }
                    },

                    new MenuElement("Контрольная",  null)
                        {
                             URL = @"#",
                              Children = new List<MenuElement>()
                                  {
                                   new MenuElement("Спорные ОБУ", "DisputeOBU"),
                                   new MenuElement("Контроль задвоения прав", typeof(DuplicateRightView).Name),
                                   new MenuElement("Основные характеристики ОБУ ОИ Росреестр", nameof(AccountingEstateRightView)),
                                   new MenuElement("Основные характеристики ОБУ ОИ Сделка",  null) { URL = @"#reportCode=AOAndAppraisalReport" },
                                   new MenuElement("Контрольный отчёт по имуществу",  null) { URL = @"#reportCode=EstateControlReport"},
                                   new MenuElement("Задвоение объектов в рамках одной выписки",  null) { URL = @"#reportCode=DuplicateInExtractReport" },
                                   new MenuElement("Сравнение выписок",  null) { URL = @"#reportCode=ExtractCompareReport" },
                                  }
                        },
                    new MenuElement("Бизнес-аналитика", null)
                    {
                        URL = @"#",
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Паспорт Общества Группы",null)
                            {
                                URL = @"#?reportCode=pasportBook"
                            },
                            new MenuElement("Анализ данных по Обществам Группы",null)
                            {
                                URL = @"#",
                                Children = new List<MenuElement>()
                                {
                                    new MenuElement("Сводная аналитика по Обществам Группы",  null) { URL = @"#reportCode=rSummaryAnalytics" },
                                    new MenuElement("Выборки Обществ Группы",  null)
                                    {
                                        URL = @"",
                                        Children = new List<MenuElement>()
                                        {
                                            new MenuElement("Список Обществ Группы с детализацией",  null) { URL = @"#reportCode=AnalyzeReport_2_2" },
                                            new MenuElement("Распределение Обществ Группы по БС и ББ",  null) { URL = @"#reportCode=AnalyzeReport_2_9" },
                                        }
                                    }
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
                    new MenuElement("Импорт прочих данных КИС", "ImportKIS")
                        { Children = new List<MenuElement>()
                                {
                                   new MenuElement("Данные об ОГ", "ImportSociety"),
                                   new MenuElement("Данные об Акционерах/Участии", "ImportShareholder"),
                                   new MenuElement("Данные о Сделках", "ImportDeal"),
                                   new MenuElement("Данные НСИ", "ImportNSI"),
                                   new MenuElement("Данные о ДП", null)
                                                {
                                                  URL = @"#",
                                                  Children = new List<MenuElement>()
                                                     {
                                                       new MenuElement("Основные данные", "ImportSubject"),
                                                       new MenuElement("Банковские реквизиты", "ImportBankingDetail"),
                                                       new MenuElement("Данные об оценщиках", "ImportAppraiserDataFinYear"),
                                                       new MenuElement("Данные оценочных организаций", "ImportAppraisalOrgData"),
                                                     }
                                                },
                                }
                        },
                    new MenuElement("Импорт данных Росреестра", "ImportRosReestr"),

                    new MenuElement("Пользовательские файлы", "ImportOtherFiles")
                    {
                                Children = new List<MenuElement>()
                                    {
                                    new MenuElement("Импорт данных о ГГР", "ImportSSR"),
                                    new MenuElement("Импорт данных о ННА", "ImportNCA"),
                                    //new MenuElement("Импорт данных об объектах оценки", "ImportEstateAppraisal"),
                                    new MenuElement("Импорт данных об оценках", "ImportAppraisal"),
                                    }
                    },
                    new MenuElement("Журнал передачи данных", "ImportDataTransfer"),
                    new MenuElement("История импорта", typeof(ImportHistory).Name),
                    new MenuElement("Журнал ошибок", typeof(ImportErrorLog).Name),


                }
            });

            return menuPreset;
        }

        public override MenuPreset CreateAdminMenu()
        {
            var menuPreset = this.CreateUserMenu();
            menuPreset.MenuElements.Add(new MenuElement("Администрирование", null)
            {
                Children = new List<MenuElement>()
                {
                    new MenuElement("Безопасность", null, "glyphicon glyphicon-lock")
                    {
                        Children = new List<MenuElement>()
                        {

                            new MenuElement("Пользователи", "AccessUserForm", "halfling halfling-user"),
                            new MenuElement("Роли", "Role", "glyphicon glyphicon-keys"),
                            new MenuElement("Аудит", typeof(SettingItem).Name)
                            {
                               Children = new List<MenuElement>()
                                 {
                            new MenuElement("События аудита", typeof(AuditItem).Name)
                                 }
                            },

                           new MenuElement("Территориальное распределение", nameof(SibUserTerritory)),

                        }
                    },

                    new MenuElement("Сервис", null, "halfling halfling-cog")
                    {
                        Children = new List<MenuElement>()
                        {
                            new MenuElement("Уведомления", "SibNotification", "glyphicon glyphicon-envelope"),
                            //new MenuElement("Бизнес-процессы", "BPWorkflow", "glyphicon glyphicon-retweet-2"),
                            //new MenuElement("Настройки", "SettingItem", "halfling halfling-tasks"),
                            //new MenuElement("Перечисления", "UiEnum", "glyphicon glyphicon-tags"),
                            //new MenuElement("Пресеты", "PresetRegistor", "glyphicon glyphicon-credit-card")
                             new MenuElement("Историчность", nameof(HistoricalSettings), "glyphicon glyphicon-notes"),
                        }
                    },

                    new MenuElement("Регистрация аналитических отчётов", "PrintedFormRegistry", "halfling glyphicon-notes"),
                }
            });

            return menuPreset;
        }
    }
}