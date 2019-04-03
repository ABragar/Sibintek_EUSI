using Base;
using Base.UI;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
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
    /// Предоставляет методы конфигурации модели ОНИ.
    /// </summary>
    public static class RealEstateModel
    {
        private const string RealEstate_AdditionalFeatures = nameof(RealEstate_AdditionalFeatures);

        /// <summary>
        /// Создает конфигурацию модели ОНИ по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<AdditionalFeatures>(RealEstate_AdditionalFeatures);

            context.CreateVmConfig<RealEstate>()
                   .Service<IRealEstateService>()
                   .Title("Объект недвижимого имущества")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.Name));

            context.CreateVmConfig<RealEstate>("RealEstateMenuList")
                     .Service<IRealEstateService>()
                     .Title("Объект недвижимого имущества")
                     .DetailView_Default()
                     .ListView_RealEstateMenuList()
                     .LookupProperty(x => x.Text(c => c.Name));

            context.CreateVmConfig<RealEstate>("RealEstateMenuReport")
                   .Service<IRealEstateService>()
                   .Title("Отчет по НИ")
                   .DetailView_Default()
                   .ListView_RealEstateMenuList()
                   .LookupProperty(x => x.Text(c => c.Name));

        }

        /// <summary>
        /// Конфигурация карточки ОНИ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<RealEstate> DetailView_Default(this ViewModelConfigBuilder<RealEstate> conf)
        {
            
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                  //Доп. характеристики
                  .AddPartialEditor(RealEstate_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID),
                        peb => peb.TabName(EstateTabs.Additionals20))
                  //общая информация    
                  .Add(ed => ed.Description, ac => ac.Title("Описание").TabName(EstateTabs.GeneralInfo).Visible(true).Order(2))
                  .Add(ed => ed.Address, ac => ac.Title("Адрес (местоположение)").TabName(EstateTabs.GeneralInfo).Visible(true).Order(3))                 
                  //.Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.GeneralInfo).Visible(true).Order(4))                  
                  .Add(ed => ed.IsNonCoreAsset, ac => ac.Title("Является ННА").TabName(EstateTabs.GeneralInfo).Visible(true).Order(5))

                  
                   //характеристики  
                   .Add(ed => ed.FloorsCount, ac => ac.Title("Количество этажей (в том числе подземных)").TabName(EstateTabs.Characteristics3).Visible(true).Order(1))
                   .Add(ed => ed.RealEstatePurpose, ac => ac.Title("Назначение здания/сооружения").TabName(EstateTabs.Characteristics3).Visible(true).Order(3))
                   .Add(ed => ed.RealEstateKinds, ac => ac.Title("Вид(ы) разрешенного использования").TabName(EstateTabs.Characteristics3).Visible(true).Order(4))
                   .Add(ed => ed.YearCommissionings, ac => ac.Title("Год ввода в эксплуатацию").TabName(EstateTabs.Characteristics3)
                   .Description("Год ввода в эксплуатацию по завершении строительства").Visible(true).Order(5))
                   .Add(ed => ed.RealEstateKind, ac => ac.Title("Вид объекта недвижимости").TabName(EstateTabs.Characteristics3).Visible(true).Order(6))                   

                  ////данные БУ
                  //.Add(ed => ed.ExternalID, ac => ac.Title("Внутренний №").TabName(EstateTabs.AccountingDt5).Visible(true).Order(1))
                  //.Add(ed => ed.InventoryNumber, ac => ac.Title("Инвентарный номер").TabName(EstateTabs.AccountingDt5).Visible(true).Order(2))
                  //.Add(ed => ed.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").TabName(EstateTabs.AccountingDt5).Visible(true).Order(3))
                  .Add(ed => ed.Owner, ac => ac.Title("Балансодержатель").TabName(EstateTabs.AccountingDt5).Visible(true).IsReadOnly(true).Order(4))
                  .Add(ed => ed.MainOwner, ac => ac.Title("Собственник").TabName(EstateTabs.AccountingDt5).Visible(true).IsReadOnly(true).Order(5))
                  .Add(ed => ed.WhoUse, ac => ac.Title("Пользователь").TabName(EstateTabs.AccountingDt5).Visible(true).IsReadOnly(true).Order(10))
                  //.Add(ed => ed.AccountNumber, ac => ac.Title("Счет").TabName(EstateTabs.AccountingDt5).Visible(true).Order(5))
                  //.Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.AccountingDt5).Visible(true).Order(6))
                  //.Add(ed => ed.Name, ac => ac.Title("Наименование").TabName(EstateTabs.AccountingDt5).Visible(true).Order(7))
                  //.Add(ed => ed.Description, ac => ac.Title("Описание").TabName(EstateTabs.AccountingDt5).Visible(true).Order(8))
                  //.Add(ed => ed.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").TabName(EstateTabs.AccountingDt5).Visible(true).Order(9))
                  .Add(ed => ed.DealProps, ac => ac.Title("Реквизиты договора").TabName(EstateTabs.AccountingDt5).Visible(true).IsReadOnly(true).Order(20))

                  //.Add(ed => ed.IsRealEstate, ac => ac.Title("Признак недвижимого имущества").TabName(EstateTabs.AccountingDt5).Visible(true).Order(11))
                  //.Add(ed => ed.MOL, ac => ac.Title("МОЛ").TabName(EstateTabs.AccountingDt5).Visible(true).Order(12))
                  //.Add(ed => ed.DateOfReceipt, ac => ac.Title("Дата оприходования").TabName(EstateTabs.AccountingDt5).Visible(true).Order(13))
                  //.Add(ed => ed.ReceiptReason, ac => ac.Title("Причина поступления").TabName(EstateTabs.AccountingDt5).Visible(true).Order(14))
                  //.Add(ed => ed.LeavingDate, ac => ac.Title("Дата списания").TabName(EstateTabs.AccountingDt5).Visible(true).Order(15))
                  //.Add(ed => ed.LeavingReason, ac => ac.Title("Причина выбытия").TabName(EstateTabs.AccountingDt5).Visible(true).Order(16))

                  ////классификаторы
                  //.Add(ed => ed.EstateType, ac => ac.Title("Класс КС").TabName(EstateTabs.Classifiers6).Visible(true).Order(1))
                  //.Add(ed => ed.OKOFName, ac => ac.Title("Класс ОКОФ").TabName(EstateTabs.Classifiers6).Visible(true).Order(2))
                  //.Add(ed => ed.OKOFCode, ac => ac.Title("Код ОКОФ").TabName(EstateTabs.Classifiers6).Visible(true).Order(3))
                  //.Add(ed => ed.OKOFName2, ac => ac.Title("Класс ОКОФ 2").TabName(EstateTabs.Classifiers6).Visible(true).Order(4))
                  //.Add(ed => ed.OKOFCode2, ac => ac.Title("Код ОКОФ 2").TabName(EstateTabs.Classifiers6).Visible(true).Order(5))
                  //.Add(ed => ed.OKTMOCode, ac => ac.Title("Код ОКТМО").TabName(EstateTabs.Classifiers6).Visible(true).Order(6))
                  //.Add(ed => ed.OKTMOName, ac => ac.Title("ОКТМО").TabName(EstateTabs.Classifiers6).Visible(true).Order(7))
                  //.Add(ed => ed.OKTMORegion, ac => ac.Title("Регион по ОКТМО").TabName(EstateTabs.Classifiers6).Visible(true).Order(8))
                  //.Add(ed => ed.OKATO, ac => ac.Title("ОКАТО").TabName(EstateTabs.Classifiers6).Visible(true).Order(9))
                  //.Add(ed => ed.OKATORegion, ac => ac.Title("Регион по ОКАТО").TabName(EstateTabs.Classifiers6).Visible(true).Order(10))

                  ////стоимость
                  //.Add(ed => ed.UpdateDate, ac => ac.Title("Дата обновления информации").TabName(EstateTabs.Cost7).Visible(true).Order(1))
                  ////следующих атрибутов нет в протоколе, но мы их оставим, т.к. это похоже на ошибку, чтобы на вкладке был один единственный атрибут :)
                  //.Add(ed => ed.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(2))
                  //.Add(ed => ed.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(3))
                  //.Add(ed => ed.DepreciationCost, ac => ac.Title("Начисленная амортизация, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(4))
                  //.Add(ed => ed.Useful, ac => ac.Title("СПИ").TabName(EstateTabs.Cost7).Visible(true).Order(5))
                  //.Add(ed => ed.UsefulEnd, ac => ac.Title("Оставшийся срок использования").TabName(EstateTabs.Cost7).Visible(true).Order(6))
                  //.Add(ed => ed.LeavingCost, ac => ac.Title("Стоимость выбытия, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(7))
                  //.Add(ed => ed.UsefulEndDate, ac => ac.Title("Дата окончания СПИ").TabName(EstateTabs.Cost7).Visible(true).Order(8))
                  //.Add(ed => ed.MarketCost, ac => ac.Title("Рыночная стоимость, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(9))
                  //.Add(ed => ed.MarketDate, ac => ac.Title("Дата рыночной оценки").TabName(EstateTabs.Cost7).Visible(true).Order(10))
                  //.Add(ed => ed.AppraisalFileCard, ac => ac.Title("Реквизиты отчета об оценке").TabName(EstateTabs.Cost7).Visible(true).Order(11))

                  ////состояние
                  //.Add(ed => ed.InConservation, ac => ac.Title("Признак консервации").TabName(EstateTabs.State8).Visible(true).Order(1))
                  //.Add(ed => ed.ConservationFrom, ac => ac.Title("Дата начала консервации").TabName(EstateTabs.State8).Visible(true).Order(2))
                  //.Add(ed => ed.ConservationTo, ac => ac.Title("Дата окончания консервации").TabName(EstateTabs.State8).Visible(true).Order(3))
                  //.Add(ed => ed.Status, ac => ac.Title("Статус").TabName(EstateTabs.State8).Visible(true).Order(4))
                  //.Add(ed => ed.StartDate, ac => ac.Title("Дата начала").TabName(EstateTabs.State8).Visible(true).Order(5))
                  //.Add(ed => ed.EndDate, ac => ac.Title("Дата окончания").TabName(EstateTabs.State8).Visible(true).Order(6))

                  //.Add(ed => ed.SubjectName, ac => ac.Title("Контрагент по договору").TabName(EstateTabs.State8).Visible(true).Order(8))

                  //адрес
                  .Add(ed => ed.Address, ac => ac.Title("Адрес (описание)").TabName(EstateTabs.Address9).Visible(true).Order(1))
                 
                  .Add(ed => ed.RegionCode, ac => ac.Title("Код региона").TabName(EstateTabs.Address9).Visible(true).Order(2))
                  .Add(ed => ed.RegionName, ac => ac.Title("Регион").TabName(EstateTabs.Address9).Visible(true).Order(3))
                  //.Add(ed => ed.OKATORegionCode, ac => ac.Title("Код региона по ОКАТО").TabName(EstateTabs.Address9).Visible(true).Order(4))
                  //.Add(ed => ed.KLADRRegionCode, ac => ac.Title("Код региона по КЛАДР").TabName(EstateTabs.Address9).Visible(true).Order(5))                  
                                   

                   ////фотографии
                   //.Add(ed => ed.Images, ac => ac.Title("Фотографии").TabName(EstateTabs.Images13).Visible(true).Order(1))

                   //ссылки 
                   .Add(ed => ed.PropertyComplex, ac => ac.Title("ИК").TabName(EstateTabs.Links14).Visible(true).Order(1))

                  //.Add(ed => ed.Land, ac => ac.Title("Земельный участок").TabName(EstateTabs.Links14).Visible(true).Order(3))
                  .Add(ed => ed.Fake, ac => ac.TabName(EstateTabs.Links14).Visible(true).Order(4))

                  //информация о правах                   
                  .AddOneToManyAssociation<Right>("RealEstate_Rights",
                         y => y.TabName(EstateTabs.Rights10)
                         .Title("Права")
                         .Mnemonic("RealEstate_Rights")
                          .Create((uofw, entity, id) =>
                          {
                              entity.Estate = uofw.GetRepository<Estate>().Find(id);
                              
                          })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Estate = null;
                            
                         })
                         .Filter((uofw, q, id, oid) => q.Where(w => w.EstateID == id)).Visible(true).Order(1))

                   //ограничения/обременения
                   .AddOneToManyAssociation<Encumbrance>("RealEstateObject_Encumbrances",
                         y => y.TabName(EstateTabs.Encumbrances12)
                         .Title("Ограничения")
                         .Mnemonic("RealEstate_Encumbrances")
                         .Create((uofw, entity, id) =>
                         {
                             entity.Estate = uofw.GetRepository<Estate>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Estate = null;
                         })
                         .Filter((uofw, q, id, oid) => q.Where(w => w.EstateID == id)).Visible(true).Order(1))

                   .AddOneToManyAssociation<AccountingObject>("RealEstateObject_AccountingObjects",
                        editor => editor
                        .TabName(EstateTabs.OBU16)
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
                  .AddManyToMany("RealEstate_FileCards"
                      , typeof(FileCardAndEstate)
                      , typeof(IManyToManyLeftAssociation<>)
                      , ManyToManyAssociationType.Rigth
                      , y => y.TabName(EstateTabs.Links14).Visible(true).Order(2))               
                  ;
            return conf;

        }

        /// <summary>
        /// Конфигурация реестра ОНИ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<RealEstate> ListView_Default(this ViewModelConfigBuilder<RealEstate> conf)
        {
            return
                conf.ListView(x => x
                .Title("Объекты недвижимого имущества")
                .Toolbar(factory => factory.Add("GetEstateToolBar", "AdditionalProperty"))
                .Columns(col => col
                    .Add(c => c.Owner, ac => ac.Title("Балансодержатель").Visible(true).Order(10))
                    .Add(c => c.WhoUse, ac => ac.Title("Пользователь").Visible(true).Order(20))
                    .Add(c => c.Name, ac => ac.Title("Наименование БУ").Visible(true).Order(30))
                    .Add(c => c.RealEstateKind, ac => ac.Title("Вид объекта недвижимости").Visible(true).Order(40))
                    //.Add(c => c.ClassFixedAsset, ac => ac.Title("Класс БУ").Visible(true).Order(50))
                    .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер").Visible(true).Order(60))
                    .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер в старой БУС").Visible(true).Order(70))
                    //.Add(c => c.ClassFixedAsset, ac => ac.Title("Класс БУ").Visible(true).Order(80))
                    //.Add(c => c.DateOfReceipt, ac => ac.Title("Дата оприходования").Visible(true).Order(90))
                    //.Add(c => c.EstateType, ac => ac.Title("Класс КС").Visible(true).Order(100))
                    //.Add(c => c.OKTMORegion, ac => ac.Title("Регион по ОКТМО").Visible(true).Order(110))
                  
                    .Add(c => c.SibRegion, ac => ac.Title("Регион").Visible(true).Order(120))


                    //скрытые колонки
                    .Add(c => c.Fake, ac => ac.Title("Кадастровый объект'").Visible(false))
                    .Add(c => c.IsNonCoreAsset, ac => ac.Title("Является ННА").Visible(false))
                    //.Add(c => c.ExternalID, ac => ac.Title("Внутренний №").Visible(false))
                    //.Add(c => c.AccountNumber, ac => ac.Title("Счет").Visible(false))
                    //.Add(c => c.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").Visible(false))
                    .Add(c => c.IsRealEstate, ac => ac.Title("Признак недвижимого имущества").Visible(false))
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

                    //.Add(c => c.InConservation, ac => ac.Title("Признак консервации").Visible(false))
                    //.Add(c => c.ConservationFrom, ac => ac.Title("Дата начала консервации").Visible(false))
                    //.Add(c => c.ConservationTo, ac => ac.Title("Дата окончания консервации").Visible(false))
                    //.Add(c => c.Status, ac => ac.Title("Статус").Visible(false))
                    //.Add(c => c.StartDate, ac => ac.Title("Дата начала").Visible(false))
                    //.Add(c => c.EndDate, ac => ac.Title("Дата окончания").Visible(false))
                    .Add(c => c.DealProps, ac => ac.Title("Реквизиты договора").Visible(true).Order(130))
                    //.Add(c => c.SubjectName, ac => ac.Title("Контрагент по договору").Visible(false))


                    .Add(c => c.RegionCode, ac => ac.Title("Код региона").Visible(false))
                    //.Add(c => c.OKATORegionCode, ac => ac.Title("Код региона по ОКАТО").Visible(false))
                    //.Add(c => c.KLADRRegionCode, ac => ac.Title("Код региона по КЛАДР").Visible(false))                    

                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false))
                    )
                    .ColumnsFrom<AdditionalFeatures>(RealEstate_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID))
                    );

        }

        /// <summary>
        /// Конфигурация реестра ОНИ для мнемоники реестра, отображаемого при открытии пункта навигационного меню.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<RealEstate> ListView_RealEstateMenuList(this ViewModelConfigBuilder<RealEstate> conf)
        {
            return
                 conf.ListView_Default()
                 .ListView(l => l.IsMultiSelect(true));
        }

    }
}
