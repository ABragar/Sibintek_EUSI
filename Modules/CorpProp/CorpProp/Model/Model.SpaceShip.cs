using Base;
using Base.UI;
using CorpProp.Entities.Estate;
using CorpProp.Services.Estate;
using System.Collections.Generic;
using System.Linq;
using CorpProp.Helpers;
using CorpProp.Entities.ManyToMany;
using Base.UI.Editors;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Common;
using CorpProp.Extentions;

namespace CorpProp.Model
{
    /// <summary>
    /// Предоставляет методы конфигурации модели космического объекта.
    /// </summary>
    public static class SpaceShipModel
    {
        private const string SpaceShip_AdditionalFeatures = nameof(SpaceShip_AdditionalFeatures);

        /// <summary>
        /// Создает конфигурацию модели космического объекта по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<AdditionalFeatures>(SpaceShip_AdditionalFeatures);

            context.CreateVmConfig<SpaceShip>()
                   .Service<ISpaceShipService>()
                   .Title("Космический объект")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.Name));

            context.CreateVmConfig<SpaceShip>("SpaceShipMenuList")
                     .Service<ISpaceShipService>()
                     .Title("Космический объект")
                     .DetailView_Default()
                     .ListView_SpaceShipMenuList()
                     .LookupProperty(x => x.Text(c => c.Name));

        }

        /// <summary>
        /// Конфигурация карточки космического объекта по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<SpaceShip> DetailView_Default(this ViewModelConfigBuilder<SpaceShip> conf)
        {
            
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                  //Доп. характеристики
                  .AddPartialEditor(SpaceShip_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID),
                        peb => peb.TabName(EstateTabs.Additionals20))
                  //общая информация
                  .Add(ed => ed.Name, ac => ac.Title("Наименование").TabName(EstateTabs.GeneralInfo).IsRequired(true).Visible(true).Order(1))
                  .Add(ed => ed.RegNumber, ac => ac.Title("Регистрационный номер").TabName(EstateTabs.GeneralInfo).Visible(true).Order(2))
                  .Add(ed => ed.Marking, ac => ac.Title("Обозначение космического объекта").TabName(EstateTabs.GeneralInfo).IsRequired(true).Visible(true).Order(3))
                  .Add(ed => ed.LaunchVehicle, ac => ac.Title("Средства выведения").TabName(EstateTabs.GeneralInfo).Visible(true).Order(4))
                  .Add(ed => ed.LaunchPlace, ac => ac.Title("Место запуска").TabName(EstateTabs.GeneralInfo).Visible(true).Order(5))
                  .Add(ed => ed.Apogee, ac => ac.Title("Апогей орбиты, км").TabName(EstateTabs.GeneralInfo).Visible(true).Order(6))
                  .Add(ed => ed.Perigee, ac => ac.Title("Перигей орбиты, км").TabName(EstateTabs.GeneralInfo).Visible(true).Order(7))
                  .Add(ed => ed.OrbitalInclination, ac => ac.Title("Наклонение орбиты, град.").TabName(EstateTabs.GeneralInfo).Visible(true).Order(8))
                  .Add(ed => ed.NodalPeriod, ac => ac.Title("Период обращения, мин.").TabName(EstateTabs.GeneralInfo).Visible(true).Order(9))
                  .Add(ed => ed.LaunchDate, ac => ac.Title("Дата запуска на орбиту").TabName(EstateTabs.GeneralInfo).Visible(true).Order(10))
                  .Add(ed => ed.DeOrbitingDate, ac => ac.Title("Дата вывода с орбиты").TabName(EstateTabs.GeneralInfo).Visible(true).Order(11))
                  .Add(ed => ed.IsNonCoreAsset, ac => ac.Title("Является ННА").TabName(EstateTabs.GeneralInfo).Visible(true).Order(12))


                  ////данные БУ
                  //.Add(ed => ed.ExternalID, ac => ac.Title("Внутренний №").TabName(EstateTabs.AccountingData3).Visible(true).Order(1))
                  //.Add(ed => ed.InventoryNumber, ac => ac.Title("Инвентарный номер").TabName(EstateTabs.AccountingData3).Visible(true).Order(2))
                  //.Add(ed => ed.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").TabName(EstateTabs.AccountingData3).Visible(true).Order(3))
                  .Add(ed => ed.Owner, ac => ac.Title("Балансодержатель").TabName(EstateTabs.AccountingData3).Visible(true).IsReadOnly(true).Order(4))
                  .Add(ed => ed.MainOwner, ac => ac.Title("Собственник").TabName(EstateTabs.AccountingData3).Visible(true).IsReadOnly(true).Order(5))
                  //.Add(ed => ed.AccountNumber, ac => ac.Title("Счет").TabName(EstateTabs.AccountingData3).Visible(true).Order(5))
                  //.Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.AccountingData3).Visible(true).Order(6))
                  //.Add(ed => ed.AccountingName, ac => ac.Title("Наименование").TabName(EstateTabs.AccountingData3).Visible(true).Order(7))
                  //.Add(ed => ed.Description, ac => ac.Title("Описание").TabName(EstateTabs.AccountingData3).Visible(true).Order(8))
                  //.Add(ed => ed.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").TabName(EstateTabs.AccountingData3).Visible(true).Order(9))
                  .Add(ed => ed.WhoUse, ac => ac.Title("Пользователь").TabName(EstateTabs.AccountingData3).Visible(true).IsReadOnly(true).Order(10))
                  //.Add(ed => ed.IsRealEstate, ac => ac.Title("Признак недвижимого имущества").TabName(EstateTabs.AccountingData3).Visible(true).Order(11))
                  //.Add(ed => ed.MOL, ac => ac.Title("МОЛ").TabName(EstateTabs.AccountingData3).Visible(true).Order(12))
                  //.Add(ed => ed.DateOfReceipt, ac => ac.Title("Дата оприходования").TabName(EstateTabs.AccountingData3).Visible(true).Order(13))
                  //.Add(ed => ed.ReceiptReason, ac => ac.Title("Причина поступления").TabName(EstateTabs.AccountingData3).Visible(true).Order(14))
                  //.Add(ed => ed.LeavingDate, ac => ac.Title("Дата списания").TabName(EstateTabs.AccountingData3).Visible(true).Order(15))
                  //.Add(ed => ed.LeavingReason, ac => ac.Title("Причина выбытия").TabName(EstateTabs.AccountingData3).Visible(true).Order(16))
                  .Add(ed => ed.DealProps, ac => ac.Title("Реквизиты договора").TabName(EstateTabs.AccountingData3).Visible(true).IsReadOnly(true).Order(20))

                  ////классификаторы
                  //.Add(ed => ed.OKTMOCode, ac => ac.Title("Код ОКТМО").TabName(EstateTabs.Classifiers4).Visible(true).Order(1))
                  //.Add(ed => ed.OKTMOName, ac => ac.Title("ОКТМО").TabName(EstateTabs.Classifiers4).Visible(true).Order(2))
                  //.Add(ed => ed.OKTMORegion, ac => ac.Title("Регион по ОКТМО").TabName(EstateTabs.Classifiers4).Visible(true).Order(3))
                  //.Add(ed => ed.OKATO, ac => ac.Title("ОКАТО").TabName(EstateTabs.Classifiers4).Visible(true).Order(4))
                  //.Add(ed => ed.OKATORegion, ac => ac.Title("Регион по ОКАТО").TabName(EstateTabs.Classifiers4).Visible(true).Order(5))

                  ////стоимость
                  //.Add(ed => ed.UpdateDate, ac => ac.Title("Дата обновления информации").TabName(EstateTabs.Cost5).Visible(true).Order(1))
                  //.Add(ed => ed.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").TabName(EstateTabs.Cost5).Visible(true).Order(2))
                  //.Add(ed => ed.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").TabName(EstateTabs.Cost5).Visible(true).Order(3))
                  //.Add(ed => ed.DepreciationCost, ac => ac.Title("Начисленная амортизация, руб.").TabName(EstateTabs.Cost5).Visible(true).Order(4))
                  //.Add(ed => ed.Useful, ac => ac.Title("СПИ").TabName(EstateTabs.Cost5).Visible(true).Order(5))
                  //.Add(ed => ed.UsefulEnd, ac => ac.Title("Оставшийся срок использования").TabName(EstateTabs.Cost5).Visible(true).Order(6))
                  //.Add(ed => ed.LeavingCost, ac => ac.Title("Стоимость выбытия, руб.").TabName(EstateTabs.Cost5).Visible(true).Order(7))
                  //.Add(ed => ed.UsefulEndDate, ac => ac.Title("Дата окончания СПИ").TabName(EstateTabs.Cost5).Visible(true).Order(8))
                  //.Add(ed => ed.MarketCost, ac => ac.Title("Рыночная стоимость, руб.").TabName(EstateTabs.Cost5).Visible(true).Order(9))
                  //.Add(ed => ed.MarketDate, ac => ac.Title("Дата рыночной оценки").TabName(EstateTabs.Cost5).Visible(true).Order(10))
                  //.Add(ed => ed.AppraisalFileCard, ac => ac.Title("Реквизиты отчета об оценке").TabName(EstateTabs.Cost5).Visible(true).Order(11))
                  ////состояние
                  //.Add(ed => ed.InConservation, ac => ac.Title("Признак консервации").TabName(EstateTabs.State5).Visible(true).Order(1))
                  //.Add(ed => ed.ConservationFrom, ac => ac.Title("Дата начала консервации").TabName(EstateTabs.State5).Visible(true).Order(2))
                  //.Add(ed => ed.ConservationTo, ac => ac.Title("Дата окончания консервации").TabName(EstateTabs.State5).Visible(true).Order(3))
                  //.Add(ed => ed.Status, ac => ac.Title("Статус").TabName(EstateTabs.State5).Visible(true).Order(4))
                  //.Add(ed => ed.StartDate, ac => ac.Title("Дата начала").TabName(EstateTabs.State5).Visible(true).Order(5))
                  //.Add(ed => ed.EndDate, ac => ac.Title("Дата окончания").TabName(EstateTabs.State5).Visible(true).Order(6))

                  //.Add(ed => ed.SubjectName, ac => ac.Title("Контрагент по договору").TabName(EstateTabs.State5).Visible(true).Order(8))

                  //.Add(ed => ed.PropertyComplex, ac => ac.Title("ИК").TabName(EstateTabs.Links5).Visible(true).Order(1))

                  ////фотографии
                  //.Add(ed => ed.Images, ac => ac.Title("Фотографии").TabName(EstateTabs.Links5).Visible(true).Order(2))

                  .AddOneToManyAssociation<AccountingObject>("SpaceShip_AccountingObjects",
                        editor => editor
                        .TabName(EstateTabs.OBU6)
                        .IsReadOnly(true)
                        .Title("Объекты БУ")
                        .IsLabelVisible(false)
                        .Order(1)
                      .Filter((uofw, q, id, oid) => q.Where(w => w.Estate != null && w.Estate.Oid == oid)))
                   )
                   .DefaultSettings((uow, r, commonEditorViewModel) =>
                   {
                       var ncaObjects = uow.GetRepository<Entities.Asset.NonCoreAsset>().Filter(f => f.EstateObjectID == r.ID).ToList();

                       if (ncaObjects.Count > 0)
                       {
                           foreach (var ncaObj in ncaObjects)
                           {
                               List<Entities.Asset.NonCoreAssetAndList> ncaRows = uow.GetRepository<Entities.Asset.NonCoreAssetAndList>().Filter(f => f.ObjLeftId == ncaObj.ID && !f.Hidden).ToList<Entities.Asset.NonCoreAssetAndList>();

                               if (ncaRows.Count > 0)
                               {
                                   foreach (var row in ncaRows)
                                   {
                                       var list = uow.GetRepository<Entities.Asset.NonCoreAssetList>().Find(row.ObjRigthId);
                                       if (list.NonCoreAssetListState != null && list.NonCoreAssetListState.Code == "103")
                                       {
                                           commonEditorViewModel.ReadOnly(ro => ro.IsNonCoreAsset);
                                           return;
                                       }
                                   }
                               }
                           }
                       }
                   })
                   )
                  //документы
                  .Config.DetailView.Editors
                  .AddManyToMany("SpaceShip_FileCards"
                      , typeof(FileCardAndEstate)
                      , typeof(IManyToManyLeftAssociation<>)
                      , ManyToManyAssociationType.Rigth
                      , y => y.TabName(EstateTabs.Links5).Visible(true).Order(3))                 
                  ;
            return conf;

        }

        /// <summary>
        /// Конфигурация реестра космических объектов по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<SpaceShip> ListView_Default(this ViewModelConfigBuilder<SpaceShip> conf)
        {
            return
                conf.ListView(x => x
                .Title("Космические объекты")
                .Columns(col => col
                    .Add(c => c.Owner, ac => ac.Title("Балансодержатель").Visible(true).Order(10))
                    .Add(c => c.WhoUse, ac => ac.Title("Пользователь").Visible(true).Order(20))    
                    .Add(c => c.Name, ac => ac.Title("Наименование БУ").Visible(true).Order(30))
                    .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер").Visible(true).Order(40))
                    .Add(c => c.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").Visible(true).Order(50))
                    
                    .Add(c => c.RegNumber, ac => ac.Title("Регистрационный номер").Visible(true).Order(60))
                    .Add(c => c.Marking, ac => ac.Title("Обозначение космического объекта").Visible(true).Order(70))
                    .Add(c => c.LaunchVehicle, ac => ac.Title("Средства выведения").Visible(true).Order(80))
                    .Add(c => c.LaunchPlace, ac => ac.Title("Место запуска").Visible(true).Order(90))
                    
                    //.Add(c => c.ClassFixedAsset, ac => ac.Title("Класс БУ").Visible(true).Order(9))
                    //.Add(c => c.Name, ac => ac.Title("Наименование").Visible(true).Order(10))

                    //.Add(c => c.DateOfReceipt, ac => ac.Title("Дата оприходования").Visible(true).Order(12))
                    //.Add(c => c.OKTMORegion, ac => ac.Title("Регион по ОКТМО").Visible(true).Order(13))
                    //.Add(c => c.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").Visible(true).Order(14))
                    //.Add(c => c.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").Visible(true).Order(15))

                    //скрытые колонки
                    .Add(c => c.Apogee, ac => ac.Title("Апогей орбиты, км").Visible(false))
                    .Add(c => c.Perigee, ac => ac.Title("Перигей орбиты, км").Visible(false))
                    .Add(c => c.OrbitalInclination, ac => ac.Title("Наклонение орбиты, град.").Visible(false))
                    .Add(c => c.NodalPeriod, ac => ac.Title("Период обращения, мин.").Visible(false))
                    .Add(c => c.LaunchDate, ac => ac.Title("Дата запуска на орбиту").Visible(false))
                    .Add(c => c.DeOrbitingDate, ac => ac.Title("Дата вывода с орбиты").Visible(false))
                    //.Add(c => c.ExternalID, ac => ac.Title("Внутренний №").Visible(false))
                    //.Add(c => c.AccountNumber, ac => ac.Title("Счет").Visible(false))
                    //.Add(c => c.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").Visible(false))
                    //.Add(c => c.ReceiptReason, ac => ac.Title("Причина поступления").Visible(false))
                    //.Add(c => c.LeavingDate, ac => ac.Title("Дата списания").Visible(false))
                    //.Add(c => c.LeavingReason, ac => ac.Title("Причина выбытия").Visible(false))
                    //.Add(c => c.OKTMOCode, ac => ac.Title("Код ОКТМО").Visible(false))
                    //.Add(c => c.OKTMOName, ac => ac.Title("ОКТМО").Visible(false))
                    //.Add(c => c.OKATO, ac => ac.Title("ОКАТО").Visible(false))
                    //.Add(c => c.OKATORegion, ac => ac.Title("Регион по ОКАТО").Visible(false))
                    //.Add(c => c.UpdateDate, ac => ac.Title("Дата обновления информации").Visible(false))
                    //.Add(c => c.DepreciationCost, ac => ac.Title("Начисленная амортизация, руб.").Visible(false))
                    //.Add(c => c.InConservation, ac => ac.Title("Признак консервации").Visible(false))
                    //.Add(c => c.ConservationFrom, ac => ac.Title("Дата начала консервации").Visible(false))
                    //.Add(c => c.ConservationTo, ac => ac.Title("Дата окончания консервации").Visible(false))
                    //.Add(c => c.Status, ac => ac.Title("Статус").Visible(false))
                    //.Add(c => c.StartDate, ac => ac.Title("Дата начала").Visible(false))
                    //.Add(c => c.EndDate, ac => ac.Title("Дата окончания").Visible(false))
                    .Add(c => c.DealProps, ac => ac.Title("Реквизиты договора").Visible(true).Order(100))
                    //.Add(c => c.SubjectName, ac => ac.Title("Контрагент по договору").Visible(false))
                    .Add(c => c.PropertyComplex, ac => ac.Title("ИК").Visible(false))
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false))

                )
                .ColumnsFrom<AdditionalFeatures>(SpaceShip_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID))
                );
        }

        /// <summary>
        /// Конфигурация реестра космиечских объектов для мнемоники реестра, отображаемого при открытии пункта навигационного меню.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<SpaceShip> ListView_SpaceShipMenuList(this ViewModelConfigBuilder<SpaceShip> conf)
        {
            return
                conf.ListView_Default()
                .ListView(l => l
                           .Toolbar(factory => factory.Add("GetEstateToolBar", "AdditionalProperty"))
                           .IsMultiSelect(true));

        }

    }
}
