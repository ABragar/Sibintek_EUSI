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
    /// Предоставляет методы конфигурации модели воздушного судна.
    /// </summary>
    public static class AircraftModel
    {
        private const string Aircraft_AdditionalFeatures = nameof(Aircraft_AdditionalFeatures);

        /// <summary>
        /// Создает конфигурацию модели воздушного судна по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context) 
        {

            context.CreateVmConfig<AdditionalFeatures>(Aircraft_AdditionalFeatures);

            context.CreateVmConfig<Aircraft>()
                     .Service<IAircraftService>()
                     .Title("Воздушное судно")
                     .DetailView_Default()                    
                     .ListView_Default()
                     .LookupProperty(x => x.Text(c => c.Name));

            context.CreateVmConfig<Aircraft>("AircraftMenuList")
                     .Service<IAircraftService>()
                     .Title("Воздушное судно")                   
                     .DetailView_Default()
                     .ListView_AircraftMenuList()
                     .LookupProperty(x => x.Text(c => c.Name));
        }
               
        /// <summary>
        /// Конфигурация карточки воздушного судна по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Aircraft> DetailView_Default(this ViewModelConfigBuilder<Aircraft> conf)
        {

            conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
               //Доп. характеристики
               .AddPartialEditor(Aircraft_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID),
                            peb => peb.TabName(EstateTabs.Additionals20))
               //Основные данные
              .Add(ed => ed.IsNonCoreAsset, ac => ac.Title("Является ННА").TabName(EstateTabs.MainData).Visible(true).Order(10))
              .Add(ed => ed.IsPropertyComplex, ac => ac.Title("ОИ как имущественный комплекс").TabName(EstateTabs.MainData).Visible(false).Order(30))
              .Add(ed => ed.Parent, ac => ac.Title("Вышестоящий объект имущества").TabName(EstateTabs.MainData).Visible(true).Order(40))
              .Add(ed => ed.AircraftKind, ac => ac.Title("Вид судна").TabName(EstateTabs.MainData).IsRequired(true).Visible(true).Order(50))
              .Add(ed => ed.TailNumber, ac => ac.Title("Бортовой (регистрационный) номер").TabName(EstateTabs.MainData).Visible(true).Order(60))
              .Add(ed => ed.AircraftType, ac => ac.Title("Тип (модель) судна").TabName(EstateTabs.MainData).IsRequired(true).Visible(true).Order(70))
              .Add(ed => ed.SerialNumber, ac => ac.Title("Серийный (идентификационный, заводской) номер").TabName(EstateTabs.MainData).Visible(true).Order(80))
              .Add(ed => ed.Appointments, ac => ac.Title("Назначение судна").TabName(EstateTabs.MainData).Visible(true).Order(90))
              .Add(ed => ed.GliderNumber, ac => ac.Title("Номер планера").TabName(EstateTabs.MainData).Visible(true).Order(100))
              .Add(ed => ed.EngineNumber, ac => ac.Title("Номера двигателей").TabName(EstateTabs.MainData).Visible(true).Order(110))
              .Add(ed => ed.PropulsionNumber, ac => ac.Title("Номера вспомогательных силовых установок").TabName(EstateTabs.MainData).Visible(true).Order(120))
              .Add(ed => ed.ProductionDate, ac => ac.Title("Дата изготовления").TabName(EstateTabs.MainData).Visible(true).Order(130))
              .Add(ed => ed.MakerName, ac => ac.Title("Наименование изготовителя").TabName(EstateTabs.MainData).Visible(true).Order(140))
              .Add(ed => ed.MaxLiftingWeight, ac => ac.Title("Максимальная взлетная масса").TabName(EstateTabs.MainData).Visible(true).Order(150))
              .Add(ed => ed.Location, ac => ac.Title("Адрес места базирования").TabName(EstateTabs.MainData).Visible(true).Order(160))

              //Данные бухгалтерского учета
              //.Add(ed => ed.ExternalID, ac => ac.Title("Внутренний системный номер в БУС").TabName(EstateTabs.AccountingData2).Visible(true).Order(10))
              .Add(ed => ed.InventoryNumber, ac => ac.Title("Инвентарный номер").TabName(EstateTabs.AccountingData2).Visible(true).Order(20))
              .Add(ed => ed.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").TabName(EstateTabs.AccountingData2).Visible(true).Order(30))
              .Add(ed => ed.Owner, ac => ac.Title("Балансодержатель").TabName(EstateTabs.AccountingData2).Visible(true).IsReadOnly(true).Order(40))
              .Add(ed => ed.MainOwner, ac => ac.Title("Собственник").TabName(EstateTabs.AccountingData2).Visible(true).IsReadOnly(true).Order(50))
              .Add(ed => ed.WhoUse, ac => ac.Title("Пользователь").TabName(EstateTabs.AccountingData2).Visible(true).IsReadOnly(true).Order(60))
              //.Add(ed => ed.AccountNumber, ac => ac.Title("Счет").TabName(EstateTabs.AccountingData2).Visible(true).Order(60))
              //.Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.AccountingData2).Visible(true).Order(70))
              .Add(ed => ed.Name, ac => ac.Title("Наименование БУ").TabName(EstateTabs.AccountingData2).Visible(true).Order(80))
              .Add(ed => ed.Description, ac => ac.Title("Описание").TabName(EstateTabs.AccountingData2).Visible(true).Order(90))
              //.Add(ed => ed.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").TabName(EstateTabs.AccountingData2).Visible(true).Order(100))
              .Add(ed => ed.DealProps, ac => ac.Title("Реквизиты договора").TabName(EstateTabs.AccountingData2).Visible(true).IsReadOnly(true).Order(110))

              .Add(ed => ed.IsRealEstate, ac => ac.Title("Признак недвижимого имущества").TabName(EstateTabs.AccountingData2).Visible(true).Order(110))
              //.Add(ed => ed.MOL, ac => ac.Title("МОЛ").TabName(EstateTabs.AccountingData2).Visible(true).Order(12))
              //.Add(ed => ed.DateOfReceipt, ac => ac.Title("Дата оприходования").TabName(EstateTabs.AccountingData2).Visible(true).Order(13))
              //.Add(ed => ed.ReceiptReason, ac => ac.Title("Причина поступления").TabName(EstateTabs.AccountingData2).Visible(true).Order(14))
              //.Add(ed => ed.LeavingDate, ac => ac.Title("Дата списания").TabName(EstateTabs.AccountingData2).Visible(true).Order(15))
              //.Add(ed => ed.LeavingReason, ac => ac.Title("Причина выбытия").TabName(EstateTabs.AccountingData2).Visible(true).Order(16))

              //.Add(ed => ed.EstateType, ac => ac.Title("Класс КС").TabName(EstateTabs.Classifiers3).Visible(true).Order(1))
              //.Add(ed => ed.OKOFCode, ac => ac.Title("Код ОКОФ").TabName(EstateTabs.Classifiers3).Visible(true).Order(2))
              //.Add(ed => ed.OKOFName, ac => ac.Title("Класс ОКОФ").TabName(EstateTabs.Classifiers3).Visible(true).Order(3))
              //.Add(ed => ed.OKOFCode2, ac => ac.Title("Код ОКОФ 2").TabName(EstateTabs.Classifiers3).Visible(true).Order(4))
              //.Add(ed => ed.OKOFName2, ac => ac.Title("Класс ОКОФ 2").TabName(EstateTabs.Classifiers3).Visible(true).Order(5))
              //.Add(ed => ed.OKTMOCode, ac => ac.Title("Код ОКТМО").TabName(EstateTabs.Classifiers3).Visible(true).Order(6))
              //.Add(ed => ed.OKTMOName, ac => ac.Title("ОКТМО").TabName(EstateTabs.Classifiers3).Visible(true).Order(7))
              //.Add(ed => ed.OKTMORegion, ac => ac.Title("Регион по ОКТМО").TabName(EstateTabs.Classifiers3).Visible(true).Order(8))
              //.Add(ed => ed.OKATO, ac => ac.Title("ОКАТО").TabName(EstateTabs.Classifiers3).Visible(true).Order(9))
              //.Add(ed => ed.OKATORegion, ac => ac.Title("Регион по ОКАТО").TabName(EstateTabs.Classifiers3).Visible(true).Order(10))

              //.Add(ed => ed.UpdateDate, ac => ac.Title("Дата обновления информации").TabName(EstateTabs.Cost4).Visible(true).Order(1))
              //.Add(ed => ed.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").TabName(EstateTabs.Cost4).Visible(true).Order(2))
              //.Add(ed => ed.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").TabName(EstateTabs.Cost4).Visible(true).Order(3))
              //.Add(ed => ed.DepreciationCost, ac => ac.Title("Начисленная амортизация, руб.").TabName(EstateTabs.Cost4).Visible(true).Order(4))
              //.Add(ed => ed.Useful, ac => ac.Title("СПИ").TabName(EstateTabs.Cost4).Visible(true).Order(5))
              //.Add(ed => ed.UsefulEnd, ac => ac.Title("Оставшийся срок использования").TabName(EstateTabs.Cost4).Visible(true).Order(6))
              //.Add(ed => ed.LeavingCost, ac => ac.Title("Стоимость выбытия, руб.").TabName(EstateTabs.Cost4).Visible(true).Order(7))
              //.Add(ed => ed.UsefulEndDate, ac => ac.Title("Дата окончания СПИ").TabName(EstateTabs.Cost4).Visible(true).Order(8))
              //.Add(ed => ed.MarketCost, ac => ac.Title("Рыночная стоимость, руб.").TabName(EstateTabs.Cost4).Visible(true).Order(9))
              //.Add(ed => ed.MarketDate, ac => ac.Title("Дата рыночной оценки").TabName(EstateTabs.Cost4).Visible(true).Order(10))
              //.Add(ed => ed.AppraisalFileCard, ac => ac.Title("Реквизиты отчета об оценке").TabName(EstateTabs.Cost4).Visible(true).Order(11))

              //.Add(ed => ed.InConservation, ac => ac.Title("Признак консервации").TabName(EstateTabs.State5).Visible(true).Order(1))
              //.Add(ed => ed.ConservationFrom, ac => ac.Title("Дата начала консервации").TabName(EstateTabs.State5).Visible(true).Order(2))
              //.Add(ed => ed.ConservationTo, ac => ac.Title("Дата окончания консервации").TabName(EstateTabs.State5).Visible(true).Order(3))
              //.Add(ed => ed.Status, ac => ac.Title("Статус").TabName(EstateTabs.State5).Visible(true).Order(4))
              //.Add(ed => ed.StartDate, ac => ac.Title("Дата начала").TabName(EstateTabs.State5).Visible(true).Order(5))
              //.Add(ed => ed.EndDate, ac => ac.Title("Дата окончания").TabName(EstateTabs.State5).Visible(true).Order(6))
              
              //.Add(ed => ed.SubjectName, ac => ac.Title("Контрагент по договору").TabName(EstateTabs.State5).Visible(true).Order(8))

              .Add(ed => ed.PropertyComplex, ac => ac.Title("ИК").TabName(EstateTabs.Links5).Visible(true).Order(1))


              ////фотографии
              //.Add(ed => ed.Images, ac => ac.Title("Фотографии").TabName(EstateTabs.Links5).Visible(true).Order(2))

              //.Add(ed => ed.Land, ac => ac.Title("Земельный участок").TabName(EstateTabs.Links5).Visible(true).Order(3))

              .AddOneToManyAssociation<AccountingObject>("Aircraft_AccountingObjects",
                        editor => editor
                        .TabName(EstateTabs.OBU6)
                        .IsReadOnly(true)
                        .Title("Объекты БУ")
                        .IsLabelVisible(false)
                        .Order(1)
                  .Filter((uofw, q, id, oid) =>
                      q.Where(w => w.Estate != null && w.Estate.Oid == oid)
                 ))

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
              .Config.DetailView.Editors.AddManyToMany("Aircraft_FileCards"
              , typeof(FileCardAndEstate)
              , typeof(IManyToManyLeftAssociation<>)
              , ManyToManyAssociationType.Rigth
              , y => y.TabName(EstateTabs.Links5)
                      .Title("Ссылки")
                      .IsLabelVisible(true)
                      .Order(3)) 
            
              ;
            return conf;
            
        }
       


        /// <summary>
        /// Конфигурация реестра воздушных судов по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Aircraft> ListView_Default(this ViewModelConfigBuilder<Aircraft> conf)
        {
            return
                conf.ListView(x => x
                .Title("Воздушные суда")
                .Columns(col => col
                    .Add(c => c.Owner, ac => ac.Title("Балансодержатель").Visible(true).Order(10))
                    .Add(c => c.WhoUse, ac => ac.Title("Пользователь").Visible(true).Order(20))
                    .Add(c => c.Name, ac => ac.Title("Наименование БУ").Visible(true).Order(30))
                    .Add(c => c.AircraftKind, ac => ac.Title("Вид судна").Visible(true).Order(40))
                    .Add(c => c.AircraftType, ac => ac.Title("Тип (модель) судна").Visible(true).Order(50))
                    .Add(c => c.TailNumber, ac => ac.Title("Бортовой (регистрационный) номер").Visible(true).Order(60))
                    .Add(c => c.Appointments, ac => ac.Title("Назначение судна").Visible(true).Order(70))
                    .Add(c => c.ProductionDate, ac => ac.Title("Дата изготовления").Visible(true).Order(80))
                    .Add(c => c.Location, ac => ac.Title("Адрес места базирования").Visible(true).Order(90))
                    .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер").Visible(true).Order(100))
                    .Add(c => c.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").Visible(true).Order(110))
                    
                    //.Add(c => c.ClassFixedAsset, ac => ac.Title("Класс БУ").Visible(true).Order(10))
                    
                    
                    //.Add(c => c.DateOfReceipt, ac => ac.Title("Дата оприходования").Visible(true).Order(13))
                    //.Add(c => c.EstateType, ac => ac.Title("Класс КС").Visible(true).Order(14))
                    //.Add(c => c.OKTMORegion, ac => ac.Title("Регион по ОКТМО").Visible(true).Order(15))
                    //.Add(c => c.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").Visible(true).Order(16))
                    //.Add(c => c.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").Visible(true).Order(17))
                    
                    //скрытые колонки
                    //.Add(c => c.ExternalID, ac => ac.Title("Внутренний системный номер в БУС").Visible(false))
                    //.Add(c => c.AccountNumber, ac => ac.Title("Счет").Visible(false))
                    //.Add(c => c.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").Visible(false))
                    //.Add(c => c.IsRealEstate, ac => ac.Title("Признак недвижимого имущества").Visible(false))
                    //.Add(c => c.ReceiptReason, ac => ac.Title("Причина поступления").Visible(false))
                    //.Add(c => c.LeavingDate, ac => ac.Title("Дата списания").Visible(false))
                    //.Add(c => c.LeavingReason, ac => ac.Title("Причина выбытия").Visible(false))
                    //.Add(c => c.OKOFCode, ac => ac.Title("Код ОКОФ").Visible(false))
                    //.Add(c => c.OKOFName, ac => ac.Title("Класс ОКОФ").Visible(false))
                    //.Add(c => c.OKOFCode2, ac => ac.Title("Код ОКОФ 2").Visible(false))
                    //.Add(c => c.OKOFName2, ac => ac.Title("Класс ОКОФ 2").Visible(false))
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
                    .Add(c => c.DealProps, ac => ac.Title("Реквизиты договора").Visible(true).Order(120))
                    //.Add(c => c.SubjectName, ac => ac.Title("Контрагент по договору").Visible(false))
                    //.Add(c => c.PropertyComplex, ac => ac.Title("ИК").Visible(false))
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false))
                )
                    .ColumnsFrom<AdditionalFeatures>(Aircraft_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID))
                );

        }

        /// <summary>
        /// Конфигурация реестра воздушных судов для мнемоники реестра, отображаемого при открытии пункта навигационного меню.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Aircraft> ListView_AircraftMenuList(this ViewModelConfigBuilder<Aircraft> conf)
        {
           return 
                conf.ListView_Default()
                .ListView( l=> l
                            .Toolbar(factory => factory.Add("GetEstateToolBar", "AdditionalProperty"))
                            .IsMultiSelect(true));   
        }       

    }
}
