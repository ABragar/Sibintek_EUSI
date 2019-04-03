using Base;
using Base.UI;
using Base.UI.ViewModal;
using CorpProp.Entities.Document;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Law;
using CorpProp.Services.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Helpers;
using CorpProp.Entities.ManyToMany;
using Base.UI.Editors;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Common;
using CorpProp.Extentions;

namespace CorpProp.Model
{
    /// <summary>
    /// Предоставляет методы конфигурации модели НЗС.
    /// </summary>
    public static class UnfinishedConstructionModel
    {
        private const string UnfinishedConstruction_AdditionalFeatures = nameof(UnfinishedConstruction_AdditionalFeatures);

        /// <summary>
        /// Создает конфигурацию модели НЗС по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<AdditionalFeatures>(UnfinishedConstruction_AdditionalFeatures);

            context.CreateVmConfig<UnfinishedConstruction>()
                   .Service<IUnfinishedConstructionService>()
                   .Title("Объект незавершенного строительства")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.Name));

            context.CreateVmConfig<UnfinishedConstruction>("UnfinishedConstructionMenuList")
                     .Service<IUnfinishedConstructionService>()
                     .Title("Объект незавершенного строительства")
                     .DetailView_Default()
                     .ListView_UnfinishedConstructionMenuList()
                     .LookupProperty(x => x.Text(c => c.Name));

        }

        /// <summary>
        /// Конфигурация карточки НЗС по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<UnfinishedConstruction> DetailView_Default(this ViewModelConfigBuilder<UnfinishedConstruction> conf)
        {
            
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors                   
                  //Доп. характеристики
                  .AddPartialEditor(UnfinishedConstruction_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID),
                        peb => peb.TabName(EstateTabs.Additionals20))        
                  //общая информация
                  .Add(ed => ed.IsNonCoreAsset, ac => ac.Title("Является ННА").TabName(EstateTabs.GeneralInfo).Visible(true).Order(10))
                  .Add(ed => ed.IsTaxCadastral, ac => ac.Title("Налог с кад.стоимости").TabName(EstateTabs.GeneralInfo).Visible(true).Order(20))
                  .Add(ed => ed.IsPropertyComplex, ac => ac.Title("ОИ как имущественный комплекс").TabName(EstateTabs.GeneralInfo).Visible(true).Order(30))
                    .Add(ed => ed.Parent, ac => ac.Title("Вышестоящий объект имущества").TabName(EstateTabs.GeneralInfo).Visible(true).Order(40))
                  .Add(ed => ed.CadastralNumber, ac => ac.Title("Кадастровый номер").TabName(EstateTabs.GeneralInfo).IsRequired(true).Visible(true).Order(50))
                  .Add(ed => ed.BlocksNumber, ac => ac.Title("Номер кадастрового квартала").TabName(EstateTabs.GeneralInfo).Visible(true).Order(60))
                  .Add(ed => ed.Name, ac => ac.Title("Наименование БУ").TabName(EstateTabs.GeneralInfo).Visible(true).Order(61))
                  .Add(ed => ed.NameByRight, ac => ac.Title("Наименование по данным ЕГРН").TabName(EstateTabs.GeneralInfo).IsRequired(false).Visible(true).Order(70))
                  .Add(ed => ed.RealEstateKind, ac => ac.Title("Вид объекта недвижимости").TabName(EstateTabs.GeneralInfo).Visible(true).Order(80))
                  //.Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.GeneralInfo).Visible(true).Order(90))

                  .Add(ed => ed.Address, ac => ac.Title("Адрес (местоположение)").TabName(EstateTabs.GeneralInfo).Visible(true).Order(100))
                  .Add(ed => ed.CadastralValue, ac => ac.Title("Кадастровая стоимость").TabName(EstateTabs.GeneralInfo).Visible(true).Order(110))
                  .Add(ed => ed.SpecialMarks, ac => ac.Title("Особые отметки").TabName(EstateTabs.GeneralInfo).Visible(true).Order(120))
                  .Add(ed => ed.RegDate, ac => ac.Title("Дата постановки на учет/регистрации").TabName(EstateTabs.GeneralInfo).Visible(true).Order(130))
                  .Add(ed => ed.DeRegDate, ac => ac.Title("Дата снятия с учета/регистрации").TabName(EstateTabs.GeneralInfo).Visible(true).Order(140))
                  .Add(ed => ed.OtherCadastralNumber, ac => ac.Title("Кадастровые номера иных ОНИ (ЗУ)").TabName(EstateTabs.GeneralInfo)
                  .Description("Кадастровые номера иных объектов недвижимости (земельных участков), в пределах которых расположен объект недвижимости").Visible(true).Order(150))
                  
                  .Add(ed => ed.OldRegNumbers, ac => ac.Title("Ранее присвоенные номера").TabName(EstateTabs.GeneralInfo).Visible(true).Order(160))
                  .Add(ed => ed.CadastralNumberLand, ac => ac.Title("Кадастровый номер ЗУ").TabName(EstateTabs.GeneralInfo)
                  .Description("Кадастровый номер объекта недвижимости (земельного участка), в пределах которого расположен объект недвижимости").Visible(true).Order(170))
                  .Add(ed => ed.Bush, ac => ac.Title("Куст").TabName(EstateTabs.GeneralInfo).Visible(true).Order(180))
                  .Add(ed => ed.Well, ac => ac.Title("Скважина").TabName(EstateTabs.GeneralInfo).Visible(true).Order(190))
                  

                   //характеристики
                   .Add(ed => ed.Preparedness, ac => ac.Title("Степень готовности в %").TabName(EstateTabs.Characteristics2)
                   .Description("Степень готовности объекта незавершенного строительства в %").Visible(true).Order(1))
                   .Add(ed => ed.AppointmentOnPlans, ac => ac.Title("Проектируемое назначение").TabName(EstateTabs.Characteristics2).Visible(true).Order(2))
                   .Add(ed => ed.Area, ac => ac.Title("Площадь в кв. метрах").TabName(EstateTabs.Characteristics2).Visible(true).Order(3))
                   .Add(ed => ed.BuildingArea, ac => ac.Title("Площадь застройки в кв. метрах").TabName(EstateTabs.Characteristics2)
                   .Description("Площадь застройки в квадратных метрах (с округлением до 0,1 квадратного метра)").Visible(true).Order(4))
                   .Add(ed => ed.Extension, ac => ac.Title("Протяженность в метрах").TabName(EstateTabs.Characteristics2)
                   .Description("Протяженность в метрах с округлением до 1 метра").Visible(true).Order(5))
                   .Add(ed => ed.Depth, ac => ac.Title("Глубина в метрах").TabName(EstateTabs.Characteristics2)
                   .Description("Глубина в метрах с округлением до 0,1 метра").Visible(true).Order(6))
                   .Add(ed => ed.DepthOf, ac => ac.Title("Глубина залегания в метрах").TabName(EstateTabs.Characteristics2)
                   .Description("Глубина залегания в метрах с округлением до 0,1 метра").Visible(true).Order(7))
                   .Add(ed => ed.Volume, ac => ac.Title("Объем в куб. метрах").TabName(EstateTabs.Characteristics2)
                   .Description("Объем в кубических метрах с округлением до 1 кубического метра").Visible(true).Order(8))
                   .Add(ed => ed.Height, ac => ac.Title("Высота в метрах").TabName(EstateTabs.Characteristics2)
                   .Description("Высота в метрах с округлением до 0,1 метра").Visible(true).Order(9))

                   //координаты
                   .AddOneToManyAssociation<Coordinate>("UnfinishedConstruction_Coordinates",
                         y => y.TabName(EstateTabs.Coordinates3)
                         .Title("Координаты")
                         .Create((uofw, entity, id) =>
                         {
                             entity.Cadastral = uofw.GetRepository<Cadastral>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Cadastral = null;
                         })
                         .Filter((uofw, q, id, oid) => q.Where(w => w.CadastralID == id)).Visible(true).Order(1))

                  ////данные БУ
                  //.Add(ed => ed.ExternalID, ac => ac.Title("Внутренний №").TabName(EstateTabs.AccountingDt3).Visible(true).Order(1))
                  //.Add(ed => ed.InventoryNumber, ac => ac.Title("Инвентарный номер").TabName(EstateTabs.AccountingDt3).Visible(true).Order(2))
                  //.Add(ed => ed.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").TabName(EstateTabs.AccountingDt3).Visible(true).Order(3))
                  .Add(ed => ed.Owner, ac => ac.Title("Балансодержатель").TabName(EstateTabs.AccountingDt3).Visible(true).IsReadOnly(true).Order(4))
                  .Add(ed => ed.MainOwner, ac => ac.Title("Собственник").TabName(EstateTabs.AccountingDt3).Visible(true).IsReadOnly(true).Order(5))
                  //.Add(ed => ed.AccountNumber, ac => ac.Title("Счет").TabName(EstateTabs.AccountingDt3).Visible(true).Order(5))
                  //.Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.AccountingDt3).Visible(true).Order(6))
                  //.Add(ed => ed.Name, ac => ac.Title("Наименование БУ").TabName(EstateTabs.AccountingDt3).Visible(true).Order(7))
                  //.Add(ed => ed.Description, ac => ac.Title("Описание").TabName(EstateTabs.AccountingDt3).Visible(true).Order(8))
                  //.Add(ed => ed.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").TabName(EstateTabs.AccountingDt3).Visible(true).Order(9))
                  .Add(ed => ed.WhoUse, ac => ac.Title("Пользователь").TabName(EstateTabs.AccountingDt3).Visible(true).IsReadOnly(true).Order(10))
                  //.Add(ed => ed.IsRealEstate, ac => ac.Title("Признак недвижимого имущества").TabName(EstateTabs.AccountingDt3).Visible(true).Order(11))
                  //.Add(ed => ed.MOL, ac => ac.Title("МОЛ").TabName(EstateTabs.AccountingDt3).Visible(true).Order(12))
                  //.Add(ed => ed.DateOfReceipt, ac => ac.Title("Дата оприходования").TabName(EstateTabs.AccountingDt3).Visible(true).Order(13))
                  //.Add(ed => ed.ReceiptReason, ac => ac.Title("Причина поступления").TabName(EstateTabs.AccountingDt3).Visible(true).Order(14))
                  //.Add(ed => ed.LeavingDate, ac => ac.Title("Дата списания").TabName(EstateTabs.AccountingDt3).Visible(true).Order(15))
                  //.Add(ed => ed.LeavingReason, ac => ac.Title("Причина выбытия").TabName(EstateTabs.AccountingDt3).Visible(true).Order(16))
                  .Add(ed => ed.DealProps, ac => ac.Title("Реквизиты договора").TabName(EstateTabs.AccountingDt3).Visible(true).IsReadOnly(true).Order(20))

                  // //классификаторы
                  //.Add(ed => ed.EstateType, ac => ac.Title("Класс КС").TabName(EstateTabs.Classifiers3).Visible(true).Order(1))                  
                  //.Add(ed => ed.OKOFName, ac => ac.Title("Класс ОКОФ").TabName(EstateTabs.Classifiers3).Visible(true).Order(2))
                  //.Add(ed => ed.OKOFCode, ac => ac.Title("Код ОКОФ").TabName(EstateTabs.Classifiers3).Visible(true).Order(3))
                  //.Add(ed => ed.OKOFName2, ac => ac.Title("Класс ОКОФ 2").TabName(EstateTabs.Classifiers3).Visible(true).Order(4))
                  //.Add(ed => ed.OKOFCode2, ac => ac.Title("Код ОКОФ 2").TabName(EstateTabs.Classifiers3).Visible(true).Order(5))                  
                  //.Add(ed => ed.OKTMOCode, ac => ac.Title("Код ОКТМО").TabName(EstateTabs.Classifiers3).Visible(true).Order(6))
                  //.Add(ed => ed.OKTMOName, ac => ac.Title("ОКТМО").TabName(EstateTabs.Classifiers3).Visible(true).Order(7))
                  //.Add(ed => ed.OKTMORegion, ac => ac.Title("Регион по ОКТМО").TabName(EstateTabs.Classifiers3).Visible(true).Order(8))
                  //.Add(ed => ed.OKATO, ac => ac.Title("ОКАТО").TabName(EstateTabs.Classifiers3).Visible(true).Order(9))
                  //.Add(ed => ed.OKATORegion, ac => ac.Title("Регион по ОКАТО").TabName(EstateTabs.Classifiers3).Visible(true).Order(10))

                  ////стоимость
                  //.Add(ed => ed.UpdateDate, ac => ac.Title("Дата обновления информации").TabName(EstateTabs.Cost4).Visible(true).Order(1))
                  ////следующих атрибутов нет в протоколе, но мы их оставим, т.к. это похоже на ошибку, чтобы на вкладке был один единственный атрибут :)
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

                  ////состояние
                  //.Add(ed => ed.InConservation, ac => ac.Title("Признак консервации").TabName(EstateTabs.State5).Visible(true).Order(1))
                  //.Add(ed => ed.ConservationFrom, ac => ac.Title("Дата начала консервации").TabName(EstateTabs.State5).Visible(true).Order(2))
                  //.Add(ed => ed.ConservationTo, ac => ac.Title("Дата окончания консервации").TabName(EstateTabs.State5).Visible(true).Order(3))
                  //.Add(ed => ed.Status, ac => ac.Title("Статус").TabName(EstateTabs.State5).Visible(true).Order(4))
                  //.Add(ed => ed.StartDate, ac => ac.Title("Дата начала").TabName(EstateTabs.State5).Visible(true).Order(5))
                  //.Add(ed => ed.EndDate, ac => ac.Title("Дата окончания").TabName(EstateTabs.State5).Visible(true).Order(6))

                  //.Add(ed => ed.SubjectName, ac => ac.Title("Контрагент по договору").TabName(EstateTabs.State5).Visible(true).Order(8))

                  //адрес
                  .Add(ed => ed.Address, ac => ac.Title("Адрес (описание)").TabName(EstateTabs.Address6).Visible(true).Order(1))
                  .Add(ed => ed.AddressID, ac => ac.Title("Идентификатор адреса").TabName(EstateTabs.Address6).Visible(true).Order(2))
                  .Add(ed => ed.RegionCode, ac => ac.Title("Код региона").TabName(EstateTabs.Address6).Visible(true).Order(3))
                  .Add(ed => ed.RegionName, ac => ac.Title("Регион").TabName(EstateTabs.Address6).Visible(true).Order(4))
                  //.Add(ed => ed.OKATORegionCode, ac => ac.Title("Код региона по ОКАТО").TabName(EstateTabs.Address6).Visible(true).Order(5))
                  //.Add(ed => ed.KLADRRegionCode, ac => ac.Title("Код региона по КЛАДР").TabName(EstateTabs.Address6).Visible(true).Order(6))
                  .Add(ed => ed.District, ac => ac.Title("Район").TabName(EstateTabs.Address6).Visible(true).Order(7))
                  .Add(ed => ed.City, ac => ac.Title("Город").TabName(EstateTabs.Address6).Visible(true).Order(8))
                  .Add(ed => ed.Locality, ac => ac.Title("Поселок").TabName(EstateTabs.Address6).Visible(true).Order(9))
                  .Add(ed => ed.Street, ac => ac.Title("Улица").TabName(EstateTabs.Address6).Visible(true).Order(10))
                  .Add(ed => ed.House, ac => ac.Title("Дом").TabName(EstateTabs.Address6).Visible(true).Order(11))
                  .Add(ed => ed.Location, ac => ac.TabName("Местоположение").Visible(true).Order(12))

                  //информация о правах   
                  //.Add(ed => ed.RighHolder, ac => ac.Title("Правообладатель").TabName(EstateTabs.Rights7))
                  //.Add(ed => ed.CadastralNumber, ac => ac.Title("Кадастровый (условный) номер").TabName(EstateTabs.Rights7))
                  .AddOneToManyAssociation<Right>("UnfinishedConstruction_Rights",
                         y => y.TabName(EstateTabs.Rights7)
                         .Title("Права")
                         .Mnemonic("Cadastral_Rights")
                          .Create((uofw, entity, id) =>
                          {
                              entity.Estate = uofw.GetRepository<Estate>().Find(id);
                             
                          })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Estate = null;
                             
                         })
                         .Filter((uofw, q, id, oid) => q.Where(w => w.EstateID == id)).Visible(true).Order(1))

                //  .AddOneToManyAssociation<FileCard>("UnfinishedConstruction_RightDocs",
                //         y => y.TabName(EstateTabs.RightInfo11)
                //         .Title("Основание гос.регистрации")
                //         .Mnemonic("Cadastral_RightDocs")
                //         .IsReadOnly()
                //         .Filter((uow, q, id) => {
                //             return uow.GetRepository<FileCardAndLegalRight>()
                //                .Filter(f => !f.Hidden && f.ObjRigth != null && !f.ObjRigth.Hidden &&
                //                f.ObjRigth.EstateID == id && f.ObjLeft != null)
                //                .Select(p => p.ObjLeft);
                //         }
                //).Visible(true).Order(1))

                   //ограничения/обременения
                   .AddOneToManyAssociation<Encumbrance>("UnfinishedConstruction_Encumbrances",
                         y => y.TabName(EstateTabs.Encumbrances9)                         
                         .Mnemonic("Cadastral_Encumbrances")
                         .Create((uofw, entity, id) =>
                         {
                             entity.Estate = uofw.GetRepository<Estate>().Find(id);
                         })
                         .Delete((uofw, entity, id) =>
                         {
                             entity.Estate = null;
                         })
                         .Filter((uofw, q, id, oid) => q.Where(w => w.EstateID == id)).Visible(true).Order(1))

                   ////фотографии
                   //.Add(ed => ed.Images, ac => ac.Title("Фотографии").TabName(EstateTabs.Images10).Visible(true).Order(1))

                   ////ссылки
                   //.Add(ed => ed.PropertyComplex, ac => ac.Title("ИК").TabName(EstateTabs.Links11).Visible(true).Order(1))
                   //.Add(ed => ed.Land, ac => ac.Title("Земельный участок").TabName(EstateTabs.Links11).Visible(true).Order(3))
                   .Add(ed => ed.Fake, ac => ac.TabName(EstateTabs.Links11).Visible(true).Order(2))

                   .AddOneToManyAssociation<AccountingObject>("UnfinishedConstruction_AccountingObjects",
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
                  .AddManyToMany("UnfinishedConstruction_FileCards"
                  , typeof(FileCardAndEstate)
                  , typeof(IManyToManyLeftAssociation<>)
                  , ManyToManyAssociationType.Rigth
                  , y => y.TabName(EstateTabs.Links11).Visible(true).Order(4))

                 
                   //выписки                 
                   .AddManyToMany("UnfinishedConstruction_Extracts"
                      , typeof(CadastralAndExtract)
                      , typeof(IManyToManyRightAssociation<>)
                      , ManyToManyAssociationType.Left
                      , y => y.TabName(EstateTabs.Extract15).Visible(true).Order(1))
                  ;
            return conf;

        }

        /// <summary>
        /// Конфигурация реестра НЗС по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<UnfinishedConstruction> ListView_Default(this ViewModelConfigBuilder<UnfinishedConstruction> conf)
        {
            return
                conf.ListView(x => x
                .Title("Объекты незавершенного строительства")
                .DataSource(ds => ds.Filter(f => !f.IsFake))
                .Columns(col => col
                    .Add(c => c.Owner, ac => ac.Title("Балансодержатель").Visible(true).Order(10))
                    .Add(c => c.WhoUse, ac => ac.Title("Пользователь").Visible(true).Order(20))
                    .Add(c => c.Name, ac => ac.Title("Наименование БУ").Visible(true).Order(30))
                    .Add(c => c.CadastralNumber, ac => ac.Title("Кадастровый номер").Visible(true).Order(40))
                    .Add(c => c.RealEstateKind, ac => ac.Title("Вид объекта недвижимости").Visible(true).Order(50))
                    //.Add(c => c.ClassFixedAsset, ac => ac.Title("Класс БУ").Visible(true).Order(60))
                    .Add(c => c.CadastralValue, ac => ac.Title("Кадастровая стоимость").Visible(true).Order(70))
                    .Add(c => c.RegDate, ac => ac.Title("Дата постановки на учет/регистрации").Visible(true).Order(80))
                    .Add(c => c.DeRegDate, ac => ac.Title("Дата снятия с учета/регистрации").Visible(true).Order(90))
                    .Add(c => c.CadastralNumberLand, ac => ac.Title("Кад. номер ЗУ").Visible(true).Order(100))
                    .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер").Visible(true).Order(110))
                    .Add(c => c.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").Visible(true).Order(120))
                    
                    
                    //.Add(c => c.ClassFixedAsset, ac => ac.Title("Класс БУ").Visible(true).Order(11))


                    .Add(c => c.IsRealEstate, ac => ac.Title("Признак недвижимого имущества").Visible(true).Order(140))
                    //.Add(c => c.DateOfReceipt, ac => ac.Title("Дата оприходования").Visible(true).Order(150))
                    //.Add(c => c.EstateType, ac => ac.Title("Класс КС").Visible(true).Order(160))
                    //.Add(c => c.OKTMORegion, ac => ac.Title("Регион по ОКТМО").Visible(true).Order(170))
                    .Add(c => c.RighHolder, ac => ac.Title("Правообладатель").Visible(true).Order(180))

                     //скрытые колонки
                     .Add(c => c.Fake, ac => ac.Title("Кадастровый объект'").Visible(false))
                    .Add(c => c.Bush, ac => ac.Title("Куст").Visible(false))
                    .Add(c => c.Well, ac => ac.Title("Скважина").Visible(false))
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
                    //.Add(c => c.OKATORegion, ac => ac.Title("Регион по ОКАТО").Visible(false))
                    //.Add(c => c.UpdateDate, ac => ac.Title("Дата обновления информации").Visible(false))
                  
                    //.Add(c => c.InConservation, ac => ac.Title("Признак консервации").Visible(false))
                    //.Add(c => c.ConservationFrom, ac => ac.Title("Дата начала консервации").Visible(false))
                    //.Add(c => c.ConservationTo, ac => ac.Title("Дата окончания консервации").Visible(false))
                    //.Add(c => c.Status, ac => ac.Title("Статус").Visible(false))
                    //.Add(c => c.StartDate, ac => ac.Title("Дата начала").Visible(false))
                    //.Add(c => c.EndDate, ac => ac.Title("Дата окончания").Visible(false))
                    .Add(c => c.DealProps, ac => ac.Title("Реквизиты договора").Visible(true).Order(190))
                    //.Add(c => c.SubjectName, ac => ac.Title("Контрагент по договору").Visible(false))

                    .Add(c => c.AddressID, ac => ac.Title("Идентификатор адреса").Visible(false))
                    .Add(c => c.RegionCode, ac => ac.Title("Код региона").Visible(false))
                    //.Add(c => c.OKATORegionCode, ac => ac.Title("Код региона по ОКАТО").Visible(false))
                    //.Add(c => c.KLADRRegionCode, ac => ac.Title("Код региона по КЛАДР").Visible(false))
                    .Add(c => c.District, ac => ac.Title("Район").Visible(false))
                    .Add(c => c.City, ac => ac.Title("Город").Visible(false))
                    .Add(c => c.Locality, ac => ac.Title("Поселок").Visible(false))
                    .Add(c => c.Street, ac => ac.Title("Улица").Visible(false))
                    .Add(c => c.House, ac => ac.Title("Дом").Visible(false))

                    .Add(c => c.RightKindCode, ac => ac.Title("Код вида права").Visible(false))
                    .Add(c => c.ShareText, ac => ac.Title("Доля в праве").Visible(false)) 
                   
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false))

                )
                .ColumnsFrom<AdditionalFeatures>(UnfinishedConstruction_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID))
                );

        }

        /// <summary>
        /// Конфигурация реестра НЗС для мнемоники реестра, отображаемого при открытии пункта навигационного меню.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<UnfinishedConstruction> ListView_UnfinishedConstructionMenuList(this ViewModelConfigBuilder<UnfinishedConstruction> conf)
        {
            return
                 conf.ListView_Default()
                 .ListView(l => l
                             .Toolbar(factory => factory.Add("GetEstateToolBar", "AdditionalProperty"))
                             .IsMultiSelect(true));
        }

    }
}
