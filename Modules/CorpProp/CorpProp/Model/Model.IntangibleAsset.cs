using Base;
using Base.UI;
using CorpProp.Entities.Estate;
using CorpProp.Services.Estate;
using System.Collections.Generic;
using System.Linq;
using CorpProp.Helpers;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.ManyToMany;
using Base.UI.Editors;
using CorpProp.Entities.Common;
using CorpProp.Extentions;

namespace CorpProp.Model
{
    /// <summary>
    /// Предоставляет методы конфигурации модели НМА.
    /// </summary>
    public static class IntangibleAssetModel
    {
        private const string IntangibleAsset_AdditionalFeatures = nameof(IntangibleAsset_AdditionalFeatures);
        
        /// <summary>
        /// Создает конфигурацию модели НМА по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<AdditionalFeatures>(IntangibleAsset_AdditionalFeatures);

            context.CreateVmConfig<IntangibleAsset>()
                   .Service<IIntangibleAssetService>()
                   .Title("Нематериальный актив")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.Name));

            context.CreateVmConfig<IntangibleAsset>("IntangibleAssetMenuList")
                     .Service<IIntangibleAssetService>()
                     .Title("Нематериальный актив")
                     .DetailView_Default()
                     .ListView_IntangibleAssetMenuList()
                     .LookupProperty(x => x.Text(c => c.Name));          
        }

        /// <summary>
        /// Конфигурация карточки НМА по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<IntangibleAsset> DetailView_Default(this ViewModelConfigBuilder<IntangibleAsset> conf)
        {
           
                conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                  //Доп. характеристики
                 .AddPartialEditor(IntangibleAsset_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID),
                        peb => peb.TabName(EstateTabs.Additionals20))

                .Add(e => e.Image, ac => ac.Title("Изображение").TabName(EstateTabs.GeneralInfo).Visible(true).Order(1))              
                .Add(e => e.IntangibleAssetType, ac => ac.Title("Тип НМА").TabName(EstateTabs.GeneralInfo).Visible(true).Order(3))
                .Add(e => e.RightRegNumber, ac => ac.Title("Регистрационный номер").TabName(EstateTabs.GeneralInfo).Visible(true).Order(4))
                .Add(e => e.Author, ac => ac.Title("Автор").TabName(EstateTabs.GeneralInfo).Visible(true).Order(5))
                .Add(e => e.SignType, ac => ac.Title("Тип товарного знака").TabName(EstateTabs.GeneralInfo).Visible(true).Order(6))
                .Add(e => e.RightHolder, ac => ac.Title("Правообладатель(и) - Полное наименование").TabName(EstateTabs.GeneralInfo).Visible(true).Order(7))
                .Add(e => e.RightHolderAddress, ac => ac.Title("Правообладатель(и) - Юридический адрес").TabName(EstateTabs.GeneralInfo).Visible(true).Order(8))
                .Add(e => e.RequestNumber, ac => ac.Title("Номер заявки в патентное бюро").TabName(EstateTabs.GeneralInfo).Visible(true).Order(9))
                .Add(e => e.PrioritySign, ac => ac.Title("Приоритет").TabName(EstateTabs.GeneralInfo).Visible(true).Order(10))
                .Add(e => e.RightDateFrom, ac => ac.Title("Дата регистрации в Гос.реестре").TabName(EstateTabs.GeneralInfo).Visible(true).Order(11))
                .Add(e => e.RightDateTo, ac => ac.Title("Срок действия регистрации").TabName(EstateTabs.GeneralInfo).Visible(true).Order(12))
                .Add(e => e.Licensee, ac => ac.Title("Лицензиат(ы)/Суб.лицензиаты").TabName(EstateTabs.GeneralInfo).Visible(true).Order(13))               
                .Add(e => e.Description, ac => ac.Title("Примечание").TabName(EstateTabs.GeneralInfo).Visible(true).Order(14))
                .Add(ed => ed.IsNonCoreAsset, ac => ac.Title("Является ННА").TabName(EstateTabs.GeneralInfo).Visible(true).Order(15))

                
                .AddManyToManyLeftAssociation<IntangibleAssetAndSibCountry>("IntangibleAsset_SibCountrys", y=>y.TabName("[002]Страны").Visible(true).Order(1))

                //.Add(ed => ed.ExternalID, ac => ac.Title("Внутренний системный номер в БУС").TabName(EstateTabs.AccountingDt3).Visible(true).Order(1))
                .Add(ed => ed.InventoryNumber, ac => ac.Title("Инвентарный номер").TabName(EstateTabs.AccountingDt3).Visible(true).Order(2))
                .Add(ed => ed.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").TabName(EstateTabs.AccountingDt3).Visible(true).Order(3))
                .Add(ed => ed.Owner, ac => ac.Title("Балансодержатель").TabName(EstateTabs.AccountingDt3).Visible(true).Order(4))
                .Add(ed => ed.MainOwner, ac => ac.Title("Собственник").TabName(EstateTabs.AccountingDt3).Visible(true).IsReadOnly(true).Order(5))
                //.Add(ed => ed.AccountNumber, ac => ac.Title("Счет").TabName(EstateTabs.AccountingDt3).Visible(true).Order(5))
                //.Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.AccountingDt3).Visible(true).Order(6))
                .Add(ed => ed.Name, ac => ac.Title("Наименование БУ").TabName(EstateTabs.AccountingDt3).Visible(true).Order(7))
                .Add(ed => ed.Description, ac => ac.Title("Описание").TabName(EstateTabs.AccountingDt3).Visible(true).Order(8))
                //.Add(ed => ed.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").TabName(EstateTabs.AccountingDt3).Visible(true).Order(9))
                //.Add(ed => ed.WhoUse, ac => ac.Title("Пользователь").TabName(EstateTabs.AccountingDt3).Visible(true).Order(10))               
                //.Add(ed => ed.MOL, ac => ac.Title("МОЛ").TabName(EstateTabs.AccountingDt3).Visible(true).Order(11))
                //.Add(ed => ed.DateOfReceipt, ac => ac.Title("Дата оприходования").TabName(EstateTabs.AccountingDt3).Visible(true).Order(12))
                //.Add(ed => ed.ReceiptReason, ac => ac.Title("Причина поступления").TabName(EstateTabs.AccountingDt3).Visible(true).Order(13))
                //.Add(ed => ed.LeavingDate, ac => ac.Title("Дата списания").TabName(EstateTabs.AccountingDt3).Visible(true).Order(14))
                //.Add(ed => ed.LeavingReason, ac => ac.Title("Причина выбытия").TabName(EstateTabs.AccountingDt3).Visible(true).Order(15))

                .Add(ed => ed.EstateType, ac => ac.Title("Класс КС").TabName(EstateTabs.Classifiers4).Visible(true).Order(1))
                //.Add(ed => ed.OKOFCode, ac => ac.Title("Код ОКОФ").TabName(EstateTabs.Classifiers4).Visible(true).Order(2))
                //.Add(ed => ed.OKOFName, ac => ac.Title("Класс ОКОФ").TabName(EstateTabs.Classifiers4).Visible(true).Order(3))
                //.Add(ed => ed.OKOFCode2, ac => ac.Title("Код ОКОФ 2").TabName(EstateTabs.Classifiers4).Visible(true).Order(4))
                //.Add(ed => ed.OKOFName2, ac => ac.Title("Класс ОКОФ 2").TabName(EstateTabs.Classifiers4).Visible(true).Order(5))
                //.Add(ed => ed.OKTMOCode, ac => ac.Title("Код ОКТМО").TabName(EstateTabs.Classifiers4).Visible(true).Order(6))
                //.Add(ed => ed.OKTMOName, ac => ac.Title("ОКТМО").TabName(EstateTabs.Classifiers4).Visible(true).Order(7))
                //.Add(ed => ed.OKTMORegion, ac => ac.Title("Регион по ОКТМО").TabName(EstateTabs.Classifiers4).Visible(true).Order(8))
                //.Add(ed => ed.OKATO, ac => ac.Title("ОКАТО").TabName(EstateTabs.Classifiers4).Visible(true).Order(9))
                //.Add(ed => ed.OKATORegion, ac => ac.Title("Регион по ОКАТО").TabName(EstateTabs.Classifiers4).Visible(true).Order(10))

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

               
                //.Add(ed => ed.Status, ac => ac.Title("Статус").TabName(EstateTabs.State6).Visible(true).Order(1))
                //.Add(ed => ed.StartDate, ac => ac.Title("Дата начала").TabName(EstateTabs.State6).Visible(true).Order(2))
                //.Add(ed => ed.EndDate, ac => ac.Title("Дата окончания").TabName(EstateTabs.State6).Visible(true).Order(3))
                //.Add(ed => ed.DealProps, ac => ac.Title("Реквизиты договора").TabName(EstateTabs.State6).Visible(true).Order(4))
                //.Add(ed => ed.SubjectName, ac => ac.Title("Контрагент по договору").TabName(EstateTabs.State6).Visible(true).Order(5))


                .AddOneToManyAssociation<AccountingObject>("IntangibleAsset_AccountingObjects",
                        editor => editor
                        .TabName(EstateTabs.OBU8)
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
               .Config.DetailView.Editors
               .AddManyToMany("IntangibleAsset_FileCards"
                  , typeof(FileCardAndEstate)
                  , typeof(IManyToManyLeftAssociation<>)
                  , ManyToManyAssociationType.Rigth
                  , y => y.TabName(EstateTabs.Links7).Visible(true).Order(1))
               ;
            return conf;

        }

        /// <summary>
        /// Конфигурация реестра НМА по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<IntangibleAsset> ListView_Default(this ViewModelConfigBuilder<IntangibleAsset> conf)
        {
            return
                conf.ListView(x => x
                .Title("Нематериальные активы")
                .Columns(col => col 
                    .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер").Visible(true).Order(1))
                    .Add(c => c.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").Visible(true).Order(2))
                    .Add(t => t.Name, h => h.Title("Наименование БУ").Visible(true).Order(3))
                     .Add(t => t.IntangibleAssetType, h => h.Title("Тип НМА").Visible(true).Order(4))
                     .Add(t => t.RightHolder, h => h.Title("Правообладатель(и) - Полное наименование").Visible(true).Order(5))
                     .Add(t => t.PrioritySign, h => h.Title("Приоритет").Visible(true).Order(5))
                     .Add(t => t.SignType, h => h.Title("Дата регистрации в Гос.реестре").Visible(true).Order(7))
                     .Add(t => t.SignType, h => h.Title("Срок действия регистрации").Visible(true).Order(8))
                    .Add(c => c.Owner, ac => ac.Title("Балансодержатель").Visible(true).Order(9))                   
                    //.Add(c => c.WhoUse, ac => ac.Title("Пользователь").Visible(true).Order(10))
                    //.Add(c => c.DateOfReceipt, ac => ac.Title("Дата оприходования").Visible(true).Order(11))                 
                    //.Add(c => c.OKTMORegion, ac => ac.Title("Регион по ОКТМО").Visible(true).Order(12))
                    //.Add(c => c.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").Visible(true).Order(13))
                    //.Add(c => c.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").Visible(true).Order(14))
                     .Add(t => t.ID, h => h.Title("Идентификатор").Visible(false).Order(15))
                )
                 .ColumnsFrom<AdditionalFeatures>(IntangibleAsset_AdditionalFeatures, nameof(IHasAdditionalFeature.AdditionalFeaturesID), nameof(IBaseObject.ID))
                );

        }

        /// <summary>
        /// Конфигурация реестра НМА для мнемоники реестра, отображаемого при открытии пункта навигационного меню.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<IntangibleAsset> ListView_IntangibleAssetMenuList(this ViewModelConfigBuilder<IntangibleAsset> conf)
        {
            return
                 conf.ListView_Default()
                 .ListView(l => l.Toolbar(factory => factory.Add("GetEstateToolBar", "AdditionalProperty")));
        }
    }
}
