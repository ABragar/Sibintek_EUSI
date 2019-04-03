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
using Base.Attributes;
using CorpProp.Helpers;
using CorpProp.Entities.ManyToMany;
using Base.UI.Editors;
using CorpProp.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Extentions;
using CorpProp.Model.Partial.Entities.SeparLand;

namespace CorpProp.Model
{
    /// <summary>
    /// Предоставляет методы конфигурации модели земельного участка.
    /// </summary>
    public static class SeparLandModel
    {
        #region Partial Mnemonic Names
        private const string SeparLandGeneralDataMnemonic = "SeparLandGeneralData";
        private const string SeparLandCharacteristics2Mnemonic = "SeparLandCharacteristics2";
        private const string SeparLandOtherLinks3Mnemonic = "SeparLandOtherLinks3";
        private const string SeparLandAccountingDt5Mnemonic = "SeparLandAccountingDt5";
        private const string SeparLandClassifiers6Mnemonic = "SeparLandClassifiers6";
        private const string SeparLandCost7Mnemonic = "SeparLandCost7";
        private const string SeparLandState8Mnemonic = "SeparLandState8";
        private const string SeparLandAddress9Mnemonic = "SeparLandAddress9";
        private const string SeparLandRight10Mnemonic = "SeparLandRight10";
        private const string SeparLandLinks14Mnemonic = "SeparLandLinks14";
        private const string SeparLandAdditionalsMnemonic = "SeparLandAdditionals";
        #endregion

        /// <summary>
        /// Создает конфигурацию модели земельного участка по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            PreparePartialMnemonics(context);

            // !NOTE: Must be created after partial mnemonics
            context.CreateVmConfig<SeparLand>("SeparLand")
                   //.Service<ILandService>()
                   .Title("Земельный участок (Раздельные модели)")
                   .Separ_ListView_Default()
                   .Separ_DetailView_Default()
                   .LookupProperty(x => x.Text(c => c.Name));

            //context.CreateVmConfig<Land>("SeparLandMenuList")
            //         //.Service<ILandService>()
            //         .Title("Земельный участок")
            //         .Partial_DetailView_Default()
            //         .ListView_LandMenuList()
            //         .LookupProperty(x => x.Text(c => c.Name));

            //context.CreateVmConfig<Land>("SeparLandMenuList")
            //       //.Service<ILandService>()
            //       .DetailView(builder => builder.Editors(factory => factory

            //                                             ));




            #region SeparLandMenuList
            // TODO понять что это и зачем
            //context.CreateVmConfig<Land>("SeparLandMenuList")
            //       //.Service<ILandService>()
            //       .DetailView(builder => builder.Editors(factory => factory

            //                                             ));
            //context.CreateVmConfig<Land>("SeparLandMenuList")
            //       //.Service<ILandService>()
            //       .DetailView(builder => builder.Editors(factory => factory

            //                                             ));
            //context.CreateVmConfig<Land>("SeparLandMenuList")
            //       //.Service<ILandService>()
            //       .DetailView(builder => builder.Editors(factory => factory

            //                                             ));
            #endregion
        }

        private static void PreparePartialMnemonics(IInitializerContext context)
        {
            #region Mnemonic SeparLandGeneralData           

            context.CreateVmConfig<SeparLandGeneralData>(SeparLandGeneralDataMnemonic)
            //.Service<ILandService>()
            #region настройка DetailView
                .DetailView(builder => builder.Editors(factory => factory
                    // общие данные
                    .Add(ed => ed.NameByRight, ac => ac.Title("Наименование по данным ЕГРН").TabName(EstateTabs.GeneralData).IsRequired(true).Visible(true).Order(1))
                    .Add(ed => ed.IsNonCoreAsset, ac => ac.Title("Является ННА").TabName(EstateTabs.GeneralData).Visible(true).Order(2))
                    .Add(ed => ed.CadastralNumber, ac => ac.Title("Кадастровый номер").TabName(EstateTabs.GeneralData).IsRequired(true).Visible(true).Visible(true).Order(3))
                    .Add(ed => ed.BlocksNumber, ac => ac.Title("Номер кадастрового квартала").TabName(EstateTabs.GeneralData).Visible(true).Order(4))
                    .Add(ed => ed.Address, ac => ac.Title("Адрес (местоположение)").TabName(EstateTabs.GeneralData).Visible(true).Order(5))
                    .Add(ed => ed.SpecialMarks, ac => ac.Title("Особые отметки").TabName(EstateTabs.GeneralData).Visible(true).Order(6))
                    .Add(ed => ed.RegDate, ac => ac.Title("Дата постановки на учет/регистрации").TabName(EstateTabs.GeneralData).Visible(true).Order(7))
                    .Add(ed => ed.DeRegDate, ac => ac.Title("Дата снятия с учета/регистрации").TabName(EstateTabs.GeneralData).Visible(true).Order(8))
                    .Add(ed => ed.Confiscation, ac => ac.Title("Сведения об изъятии").TabName(EstateTabs.GeneralData)
                        .Description("Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд").Visible(true).Order(9))
                    .Add(ed => ed.RegDateByDoc, ac => ac.Title("Дата постановки на кад.учет по документу").TabName(EstateTabs.GeneralData).Visible(true).Order(10))
                    .Add(ed => ed.GroundCadastralNumber, ac => ac.Title("Номер гос. учета в лесном реестре").TabName(EstateTabs.GeneralData).Visible(true).Order(11))
                    .Add(ed => ed.Wood, ac => ac.Title("Лесной участок").TabName(EstateTabs.GeneralData).Visible(true).Order(12))
                    .Add(ed => ed.IsTaxCadastral, ac => ac.Title("Налог с кад.стоимости").TabName(EstateTabs.GeneralData).Visible(true).Order(13))
            ).DefaultSettings((uow, r, commonEditorViewModel) =>
                {
                    var ncaObjects = uow.GetRepository<CorpProp.Entities.Asset.NonCoreAsset>().Filter(f => f.EstateObjectID == r.ID).ToList();

                    if (ncaObjects.Count > 0)
                    {
                        foreach (var ncaObj in ncaObjects)
                        {
                            List<CorpProp.Entities.Asset.NonCoreAssetAndList> ncaRows = uow.GetRepository<CorpProp.Entities.Asset.NonCoreAssetAndList>().Filter(f => f.ObjLeftId == ncaObj.ID && !f.Hidden).ToList<CorpProp.Entities.Asset.NonCoreAssetAndList>();

                            if (ncaRows.Count > 0)
                            {
                                foreach (var row in ncaRows)
                                {
                                    var list = uow.GetRepository<CorpProp.Entities.Asset.NonCoreAssetList>().Find(row.ObjRigthId);
                                    if (list.NonCoreAssetListState != null && list.NonCoreAssetListState.Code == "103")
                                    {
                                        commonEditorViewModel.ReadOnly(ro => ro.IsNonCoreAsset);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }))
            #endregion
            #region настройка ListView
            .ListView(x => x
                .Title("Земельные участки (SeparLand)")
                .Columns(cols => cols
                    .Add(c => c.CadastralNumber, ac => ac.Visible(true).Order(6))
                    .Add(c => c.Address, ac => ac.Visible(true).Order(7))
                    .Add(c => c.RegDate, ac => ac.Visible(true).Order(9))
                    .Add(c => c.DeRegDate, ac => ac.Visible(true).Order(10))

                    //скрытые колонки
                    .Add(c => c.IsNonCoreAsset, ac => ac.Visible(true).Order(6))
                    .Add(c => c.BlocksNumber, ac => ac.Visible(false))
                    .Add(c => c.DeRegDate, ac => ac.Visible(false))
            ));
            #endregion

            #endregion

            #region Mnemonic SeparLandCharacteristics2
            context.CreateVmConfig<SeparLandCharacteristics2>(SeparLandCharacteristics2Mnemonic)
                   //.Service<ILandService>()
            #region DetailView
                   .DetailView(builder => builder.Editors(factory => factory
                        //характеристики
                        .Add(ed => ed.GroundCategory, ac => ac.Title("Категория земель").TabName(EstateTabs.Characteristics2).Visible(true).Order(1))
                        .Add(ed => ed.PermittedByDoc, ac => ac.Title("Вид разрешенного использования по документу").TabName(EstateTabs.Characteristics2).Visible(true).Order(2))
                        .Add(ed => ed.PermittedLandUse, ac => ac.Title("Вид разрешенного использования (старое)").TabName(EstateTabs.Characteristics2)
                            .Description("Вид разрешенного использования земельного участка в соответствии с ранее использовавшимся классификатором").Visible(true).Order(3))
                        .Add(ed => ed.PermittedLandUseMer, ac => ac.Title("Вид разрешенного использования(приказ №540 от 01.09.14)").TabName(EstateTabs.Characteristics2)
                            .Description("Вид разрешенного использования земельного участка в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540").Visible(true).Order(4))
                        .Add(ed => ed.PermittesGradRegNumbBorder, ac => ac.Title("Реестровый номер границы(текст)").TabName(EstateTabs.Characteristics2).Visible(true).Order(5))
                        .Add(ed => ed.PermittesGradLandUse, ac => ac.Title("Вид разрешенного использования (по градостроительному регламенту)").TabName(EstateTabs.Characteristics2)
                            .Description("Вид разрешенного использования в соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540").Visible(true).Order(6))
                        .Add(ed => ed.UsesKind, ac => ac.Title("Разрешенное использование (текстовое описание)").TabName(EstateTabs.Characteristics2).Visible(true).Order(7))
                        .Add(ed => ed.Area, ac => ac.Title("Площадь в кв. метрах").TabName(EstateTabs.Characteristics2).Visible(true).Order(8))
                        .Add(ed => ed.UseArea, ac => ac.Title("Эксплуатируемая площадь в кв. метрах").TabName(EstateTabs.Characteristics2).Visible(true).Order(9))
                        .Add(ed => ed.ActualValue, ac => ac.Title("Кадастровая стоимость").TabName(EstateTabs.Characteristics2).Visible(true).Order(10))
                        //сведения о частях
                        .AddOneToManyAssociation<CadastralPart>("SeparLand_CadastralParts", y => y.TabName(EstateTabs.Characteristics2)
                            .Title("Сведения о частях")
                            .Create((uofw, entity, id) =>
                            {
                                entity.Cadastral = uofw.GetRepository<Cadastral>().Find(id);
                            })
                            .Delete((uofw, entity, id) =>
                            {
                                entity.Cadastral = null;
                            })
                            .Filter((uofw, q, id, oid) => q.Where(w => w.CadastralID == id)).Visible(true).Order(11))

                        .Add(ed => ed.RealEstateKind, ac => ac.Title("Вид объекта недвижимости").TabName(EstateTabs.Characteristics2).Visible(true).Order(12))
                        .Add(ed => ed.LandType, ac => ac.Title("Вид земельного участка").TabName(EstateTabs.Characteristics2).Visible(true).Order(13))
                   ))
            #endregion
            #region ListView
                .ListView(x => x
                    .Title(EstateTabs.Characteristics2)
                    .Columns(cols => cols
                        .Add(c => c.LandType, ac => ac.Title("Вид земельного участка").Visible(true).Order(3))
                        .Add(c => c.GroundCategory, ac => ac.Title("Категория земель").Visible(true).Order(4))
                        .Add(c => c.PermittedByDoc, ac => ac.Title("Вид разрешенного использования по документу").Visible(true).Order(5))
                        .Add(c => c.Area, ac => ac.Title("Площадь").Visible(true).Order(8))

                        //скрытые колонки
                        .Add(c => c.PermittesGradRegNumbBorder, ac => ac.Title("Реестровый номер границы(текст)").Visible(false))
                    ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandOtherLinks3
            context.CreateVmConfig<SeparLandOtherLinks3>(SeparLandOtherLinks3Mnemonic)
                //.Service<ILandService>()
            #region DetailView
                .DetailView(builder => builder.Editors(factory => factory
                    //связь с иными кадастровыми объектами
                    .Add(ed => ed.OtherCadastralNumber, ac => ac.Title("Кадастровые номера иных ОНИ (ЗУ)").TabName(EstateTabs.OtherLinks3)
                        .Description("Кадастровые номера иных объектов недвижимости (земельных участков), в пределах которых расположен объект недвижимости").Visible(true).Order(1))
                    .Add(ed => ed.OldRegNumbers, ac => ac.Title("Ранее присвоенные номера").TabName(EstateTabs.OtherLinks3).Visible(true).Order(2))
                ))
            #endregion
            #region ListView
                //.ListView(x => x
                //    .Title(EstateTabs.OtherLinks3)
                //    .Columns(cols => cols

                //        //скрытые колонки
                //        .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false))
                //        .Add(c => c.ParentID, ac => ac.Visible(false))
                //    ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandAccountingDt5
            context.CreateVmConfig<SeparLandAccountingDt5>(SeparLandAccountingDt5Mnemonic)
                //.Service<ILandService>()
            #region DetailView
                .DetailView(builder => builder.Editors(factory => factory
                    //данные БУ
                    .Add(ed => ed.ExternalID, ac => ac.Title("Внутренний №").TabName(EstateTabs.AccountingDt5).Visible(true).Order(1))
                    .Add(ed => ed.InventoryNumber, ac => ac.Title("Инвентарный номер").TabName(EstateTabs.AccountingDt5).Visible(true).Order(2))
                    .Add(ed => ed.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").TabName(EstateTabs.AccountingDt5).Visible(true).Order(3))
                    .Add(ed => ed.Owner, ac => ac.Title("Балансодержатель").TabName(EstateTabs.AccountingDt5).Visible(true).Order(4))
                    .Add(ed => ed.AccountNumber, ac => ac.Title("Счет").TabName(EstateTabs.AccountingDt5).Visible(true).Order(5))
                    .Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.AccountingDt5).Visible(true).Order(6))
                    .Add(ed => ed.Name, ac => ac.Title("Наименование БУ").TabName(EstateTabs.AccountingDt5).Visible(true).Order(7))
                    .Add(ed => ed.Description, ac => ac.Title("Описание").TabName(EstateTabs.AccountingDt5).Visible(true).Order(8))
                    .Add(ed => ed.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").TabName(EstateTabs.AccountingDt5).Visible(true).Order(9))
                    .Add(ed => ed.WhoUse, ac => ac.Title("Пользователь").TabName(EstateTabs.AccountingDt5).Visible(true).Order(10))
                    .Add(ed => ed.IsRealEstate, ac => ac.Title("Признак недвижимого имущества").TabName(EstateTabs.AccountingDt5).Visible(true).Order(11))
                    .Add(ed => ed.MOL, ac => ac.Title("МОЛ").TabName(EstateTabs.AccountingDt5).Visible(true).Order(12))
                    .Add(ed => ed.DateOfReceipt, ac => ac.Title("Дата оприходования").TabName(EstateTabs.AccountingDt5).Visible(true).Order(13))
                    .Add(ed => ed.ReceiptReason, ac => ac.Title("Причина поступления").TabName(EstateTabs.AccountingDt5).Visible(true).Order(14))
                    .Add(ed => ed.LeavingDate, ac => ac.Title("Дата списания").TabName(EstateTabs.AccountingDt5).Visible(true).Order(15))
                    .Add(ed => ed.LeavingReason, ac => ac.Title("Причина выбытия").TabName(EstateTabs.AccountingDt5).Visible(true).Order(16))
                ))
            #endregion
            #region ListView
                .ListView(x => x
                    .Title(EstateTabs.AccountingDt5)
                    .Columns(cols => cols
                        .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер").Visible(true).Order(1))
                        .Add(c => c.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").Visible(true).Order(2))
                        .Add(c => c.Name, ac => ac.Title("Наименование БУ").Visible(true).Order(6))
                        .Add(c => c.Owner, ac => ac.Title("Балансодержатель").Visible(true).Order(11))
                        .Add(c => c.WhoUse, ac => ac.Title("Пользователь").Visible(true).Order(12))
                        .Add(c => c.DateOfReceipt, ac => ac.Title("Дата оприходования").Visible(true).Order(13))

                        //скрытые колонки
                        .Add(c => c.ExternalID, ac => ac.Title("Внутренний №").Visible(false))
                        .Add(c => c.AccountNumber, ac => ac.Title("Счет").Visible(false))
                        .Add(c => c.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").Visible(false))
                        .Add(c => c.ReceiptReason, ac => ac.Title("Причина поступления").Visible(false))
                        .Add(c => c.LeavingDate, ac => ac.Title("Дата списания").Visible(false))
                        .Add(c => c.LeavingReason, ac => ac.Title("Причина выбытия").Visible(false))
                ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandClassifiers6
            context.CreateVmConfig<SeparLandClassifiers6>(SeparLandClassifiers6Mnemonic)
                //.Service<ILandService>()
            #region DetailView                
                .DetailView(builder => builder.Editors(factory => factory
                    //классификаторы
                    .Add(ed => ed.EstateType, ac => ac.Title("Класс КС").TabName(EstateTabs.Classifiers6).Visible(true).Order(1))
                    .Add(ed => ed.OKOFName, ac => ac.Title("Класс ОКОФ").TabName(EstateTabs.Classifiers6).Visible(true).Order(2))
                    .Add(ed => ed.OKOFCode, ac => ac.Title("Код ОКОФ").TabName(EstateTabs.Classifiers6).Visible(true).Order(3))
                    .Add(ed => ed.OKOFName2, ac => ac.Title("Класс ОКОФ 2").TabName(EstateTabs.Classifiers6).Visible(true).Order(4))
                    .Add(ed => ed.OKOFCode2, ac => ac.Title("Код ОКОФ 2").TabName(EstateTabs.Classifiers6).Visible(true).Order(5))
                    .Add(ed => ed.OKTMOCode, ac => ac.Title("Код ОКТМО").TabName(EstateTabs.Classifiers6).Visible(true).Order(6))
                    .Add(ed => ed.OKTMOName, ac => ac.Title("ОКТМО").TabName(EstateTabs.Classifiers6).Visible(true).Order(7))
                    .Add(ed => ed.OKTMORegion, ac => ac.Title("Регион по ОКТМО").TabName(EstateTabs.Classifiers6).Visible(true).Order(8))
                    .Add(ed => ed.OKATO, ac => ac.Title("ОКАТО").TabName(EstateTabs.Classifiers6).Visible(true).Order(9))
                    .Add(ed => ed.OKATORegionCode, ac => ac.Title("Код региона по ОКАТО (по данным БУ)").TabName(EstateTabs.Classifiers6).Visible(true).Order(10))
                    .Add(ed => ed.OKATORegion, ac => ac.Title("Регион по ОКАТО").TabName(EstateTabs.Classifiers6).Visible(true).Order(11))
                ))
            #endregion
            #region ListView
                .ListView(x => x
                    .Title(EstateTabs.Classifiers6)
                    .Columns(cols => cols
                        .Add(c => c.OKTMORegion, ac => ac.Title("Регион по ОКТМО").Visible(true).Order(14))

                        //скрытые колонки
                        .Add(c => c.OKOFCode, ac => ac.Title("Код ОКОФ").Visible(false))
                        .Add(c => c.OKOFName, ac => ac.Title("Класс ОКОФ").Visible(false))
                        .Add(c => c.OKOFCode2, ac => ac.Title("Код ОКОФ 2").Visible(false))
                        .Add(c => c.OKOFName2, ac => ac.Title("Класс ОКОФ 2").Visible(false))
                        .Add(c => c.OKTMOCode, ac => ac.Title("Код ОКТМО").Visible(false))
                        .Add(c => c.OKTMOName, ac => ac.Title("ОКТМО").Visible(false))
                        .Add(c => c.OKATO, ac => ac.Title("ОКАТО").Visible(false))
                        .Add(c => c.OKATORegion, ac => ac.Title("Регион по ОКАТО").Visible(false))
                        .Add(c => c.OKATORegionCode, ac => ac.Title("Код региона по ОКАТО (по данным Росреестра)").Visible(false))
                ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandCost7
            context.CreateVmConfig<SeparLandCost7>(SeparLandCost7Mnemonic)
                //.Service<ILandService>()
            #region DetailView
                .DetailView(builder => builder.Editors(factory => factory
                    //стоимость
                    .Add(ed => ed.UpdateDate, ac => ac.Title("Дата обновления информации").TabName(EstateTabs.Cost7).Visible(true).Order(1))
                    .Add(ed => ed.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(2))
                    .Add(ed => ed.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(3))
                    .Add(ed => ed.DepreciationCost, ac => ac.Title("Начисленная амортизация, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(4))
                    .Add(ed => ed.Useful, ac => ac.Title("СПИ").TabName(EstateTabs.Cost7).Visible(true).Order(5))
                    .Add(ed => ed.UsefulEnd, ac => ac.Title("Оставшийся срок использования").TabName(EstateTabs.Cost7).Visible(true).Order(6))
                    .Add(ed => ed.LeavingCost, ac => ac.Title("Стоимость выбытия, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(7))
                    .Add(ed => ed.UsefulEndDate, ac => ac.Title("Дата окончания СПИ").TabName(EstateTabs.Cost7).Visible(true).Order(8))
                    .Add(ed => ed.MarketCost, ac => ac.Title("Рыночная стоимость, руб.").TabName(EstateTabs.Cost7).Visible(true).Order(9))
                    .Add(ed => ed.MarketDate, ac => ac.Title("Дата рыночной оценки").TabName(EstateTabs.Cost7).Visible(true).Order(10))
                    .Add(ed => ed.AppraisalFileCard, ac => ac.Title("Реквизиты отчета об оценке").TabName(EstateTabs.Cost7).Visible(true).Order(11))
                ))
            #endregion
            #region ListView
                .ListView(x => x
                    .Title(EstateTabs.Cost7)
                    .Columns(cols => cols
                        .Add(c => c.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").Visible(true).Order(15))
                        .Add(c => c.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").Visible(true).Order(16))

                        //скрытые колонки
                        .Add(c => c.UpdateDate, ac => ac.Title("Дата обновления информации").Visible(false))
                        .Add(c => c.DepreciationCost, ac => ac.Title("Начисленная амортизация, руб.").Visible(false))
                ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandState8
            context.CreateVmConfig<SeparLandState8>(SeparLandState8Mnemonic)
                //.Service<ILandService>()
            #region DetailView
                .DetailView(builder => builder.Editors(factory => factory
                    //состояние
                    .Add(ed => ed.Status, ac => ac.Title("Статус ОБУ").TabName(EstateTabs.State8).Visible(true).Order(1))
                    .Add(ed => ed.StartDate, ac => ac.Title("Дата начала").TabName(EstateTabs.State8).Visible(true).Order(2))
                    .Add(ed => ed.EndDate, ac => ac.Title("Дата окончания").TabName(EstateTabs.State8).Visible(true).Order(3))
                    .Add(ed => ed.DealProps, ac => ac.Title("Реквизиты договора").TabName(EstateTabs.State8).Visible(true).Order(4))
                    .Add(ed => ed.SubjectName, ac => ac.Title("Контрагент по договору").TabName(EstateTabs.State8).Visible(true).Order(5))
                ))
            #endregion
            #region ListView
                .ListView(x => x
                    .Title(EstateTabs.State8)
                    .Columns(cols => cols

                        //скрытые колонки
                        .Add(c => c.Status, ac => ac.Title("Статус ОБУ").Visible(false))
                        .Add(c => c.StartDate, ac => ac.Title("Дата начала").Visible(false))
                        .Add(c => c.EndDate, ac => ac.Title("Дата окончания").Visible(false))
                        .Add(c => c.DealProps, ac => ac.Title("Реквизиты договора").Visible(false))
                        .Add(c => c.SubjectName, ac => ac.Title("Контрагент по договору").Visible(false))

                ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandAddress9
            context.CreateVmConfig<SeparLandAddress9>(SeparLandAddress9Mnemonic)
                //.Service<ILandService>()
            #region DetailView
                .DetailView(builder => builder.Editors(factory => factory
                    //адрес
                    .Add(ed => ed.Address, ac => ac.Title("Адрес (описание)").Visible(true).Order(1))
                    .Add(ed => ed.AddressID, ac => ac.Title("Идентификатор адреса").Visible(true).Order(2))
                    .Add(ed => ed.RegionCode, ac => ac.Title("Код региона (по данным Росреестра)").Visible(true).Order(3))
                    .Add(ed => ed.RegionName, ac => ac.Title("Наименование Региона (по данным Росреестра)").Visible(true).Order(4))
                    .Add(ed => ed.OKATORegionCode, ac => ac.Title("Код региона по ОКАТО (по данным Росреестра)").Visible(true).Order(5))
                    .Add(ed => ed.KLADRRegionCode, ac => ac.Title("Код региона по КЛАДР").Visible(true).Order(6))
                    .Add(ed => ed.District, ac => ac.Title("Район").Visible(true).Order(7))
                    .Add(ed => ed.City, ac => ac.Title("Город").Visible(true).Order(8))
                    .Add(ed => ed.Locality, ac => ac.Title("Поселок").Visible(true).Order(9))
                    .Add(ed => ed.Street, ac => ac.Title("Улица").Visible(true).Order(10))
                    .Add(ed => ed.House, ac => ac.Title("Дом").Visible(true).Order(11))
                    .Add(ed => ed.Location, ac => ac.Title("Местоположение").Visible(true).Order(12))
                ))
            #endregion
            #region ListView
                .ListView(x => x
                    .Title(EstateTabs.Address9)
                    .Columns(cols => cols
                        .Add(c => c.RegionName, ac => ac.Title("Наименование Региона (по данным Росреестра)").Visible(true).Order(18))

                        //скрытые колонки
                        .Add(c => c.AddressID, ac => ac.Title("Идентификатор адреса").Visible(false))
                        .Add(c => c.RegionCode, ac => ac.Title("Код региона (по данным Росреестра)").Visible(false))
                        .Add(c => c.KLADRRegionCode, ac => ac.Title("Код региона по КЛАДР").Visible(false))
                        .Add(c => c.District, ac => ac.Title("Район").Visible(false))
                        .Add(c => c.City, ac => ac.Title("Город").Visible(false))
                        .Add(c => c.Locality, ac => ac.Title("Поселок").Visible(false))
                        .Add(c => c.Street, ac => ac.Title("Улица").Visible(false))
                        .Add(c => c.House, ac => ac.Title("Дом").Visible(false))
                ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandRight10
            context.CreateVmConfig<SeparLandRight10>(SeparLandRight10Mnemonic)
                //.Service<ILandService>()
            #region DetailView
                //.DetailView(builder => builder.Editors(factory => factory
                //    //адрес
                //    .Add(ed => ed.RighHolder, ac => ac.Title("Правообладатель").TabName(EstateTabs.Rights10).Visible(true).Order(1))
                //))
            #endregion
            #region ListView
                .ListView(x => x
                    .Title(EstateTabs.Rights10)
                    .Columns(cols => cols
                        .Add(c => c.RighHolder, ac => ac.Title("Правообладатель").Visible(true).Order(17))
                        .Add(c => c.RighKindAndShare, ac => ac.Title("Вид права, доля в праве").Visible(true).Order(19))
                        .Add(c => c.RighRegDate, ac => ac.Title("Дата гос.регистрации").Visible(true).Order(20))
                        .Add(c => c.RightRegNumber, ac => ac.Title("Номер гос.регистрации").Visible(true).Order(21))

                        //скрытые колонки
                        .Add(c => c.RightKindCode, ac => ac.Title("Код вида права").Visible(false))
                        .Add(c => c.ShareText, ac => ac.Title("Доля в праве").Visible(false))
                ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandLinks14
            context.CreateVmConfig<SeparLandLinks14>(SeparLandLinks14Mnemonic)
                //.Service<ILandService>()
                .DetailView(builder => builder.Editors(factory => factory
                    //ссылки - в атрибутивном составе их нет, но без них никак.
                    .Add(ed => ed.PropertyComplex, ac => ac.Title("ИК").TabName(EstateTabs.Links14).Visible(true).Order(1))
                    .Add(ed => ed.Land, ac => ac.Title("Земельный участок").TabName(EstateTabs.Links14).Visible(true).Order(3))
                    .Add(ed => ed.Fake, ac => ac.TabName(EstateTabs.Links14).Visible(true).Order(4))
                ))
            #region ListView
                .ListView(x => x
                    .Title(EstateTabs.Links14)
                    .Columns(cols => cols
                        //скрытые колонки
                        .Add(c => c.Fake, ac => ac.Title("Кадастровый объект'").Visible(false))
                        .Add(c => c.PropertyComplex, ac => ac.Title("ИК").Visible(false))
                ))
            #endregion
                ;
            #endregion

            #region Mnemonic SeparLandAdditionals
            context.CreateVmConfig<SeparLandAdditionals>(SeparLandAdditionalsMnemonic);
            #endregion
        }

        /// <summary>
        /// Конфигурация карточки ЗУ по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<SeparLand> Separ_DetailView_Default(this ViewModelConfigBuilder<SeparLand> conf)
        {
            conf.DetailView(x => x.Title(conf.Config.Title)
             .Editors(editors => editors
                 //.Clear()
                 .Add(entity => entity.Number, editorBuilder => editorBuilder.Visible(true).TabName(EstateTabs.GeneralInfo).Order(-200))
                 .Add(entity => entity.IsPropertyComplex, h => h.Order(1).TabName(EstateTabs.GeneralInfo).Visible(true).IsReadOnly(false))
                 .Add(entity => entity.Parent, editorBuilder => editorBuilder.Order(2).TabName(EstateTabs.GeneralInfo))
                 // TODO EstateStatus ломает фронт
                 //.Add(entity => entity.EstateStatus, editorBuilder => editorBuilder.Order(3).Visible(true).TabName(EstateTabs.GeneralInfo).IsReadOnly(true))
                 .Add(entity => entity.Estate, editorBuilder => editorBuilder.Order(4).TabName(EstateTabs.GeneralInfo))
                 .Add<PartialEditor>("PartialEditor1", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.GeneralData).Mnemonic(SeparLandGeneralDataMnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(BaseObject.ID)}"))
                 .Add<PartialEditor>("PartialEditor3", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.Characteristics2).Mnemonic(SeparLandCharacteristics2Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))
                 .Add<PartialEditor>("PartialEditor4", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.OtherLinks3).Mnemonic(SeparLandOtherLinks3Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))

                 //координаты
                 .AddOneToManyAssociation<Coordinate>("SeparLand_Coordinates",
                       y => y.TabName(EstateTabs.Coordinates4)
                       .Title("Координаты")
                       .IsLabelVisible(false)
                       .Create((uofw, entity, id) =>
                       {
                           entity.Cadastral = uofw.GetRepository<Cadastral>().Find(id);
                       })
                       .Delete((uofw, entity, id) =>
                       {
                           entity.Cadastral = null;
                       })
                       .Filter((uofw, q, id, oid) => q.Where(w => w.CadastralID == id)).Visible(true).Order(1))

                .Add<PartialEditor>("PartialEditor5", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.AccountingDt5).Mnemonic(SeparLandAccountingDt5Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))
                .Add<PartialEditor>("PartialEditor6", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.Classifiers6).Mnemonic(SeparLandClassifiers6Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))
                .Add<PartialEditor>("PartialEditor7", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.Cost7).Mnemonic(SeparLandCost7Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))
                .Add<PartialEditor>("PartialEditor8", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.State8).Mnemonic(SeparLandState8Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))
                .Add<PartialEditor>("PartialEditor9", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.Address9).Mnemonic(SeparLandAddress9Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))
                // TODO Это падает в Form.cshtml
                //.Add<PartialEditor>(SeparLandRight10Mnemonic, a => a.DataType(PropertyDataType.Custom).Title(" ").TabName("[051]Информация о правах").Mnemonic(SeparLandRight10Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))
                //.Add<PartialEditor>(SeparLandAdditionalsMnemonic, a => a.DataType(PropertyDataType.Custom).Title(" ").TabName("[052]Дополнительные атрибуты").Mnemonic(SeparLandAdditionalsMnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))

                //информация о правах   
                //.Add(ed => ed.RighHolder, ac => ac.Title("Правообладатель").TabName(EstateTabs.Rights10))
                //.Add(ed => ed.CadastralNumber, ac => ac.Title("Кадастровый (условный) номер").TabName(EstateTabs.Rights10))
                .AddOneToManyAssociation<Right>("SeparLand_Rights",
                       y => y.TabName(EstateTabs.Rights10)
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
                       .Filter((uofw, q, id, oid) => q.Where(w => w.EstateID == id))
                       .Visible(true).Order(1))

                 //.AddOneToManyAssociation<FileCard>("SeparLand_RightDocs",
                 //      y => y.TabName(EstateTabs.RightInfo11)
                 //      .Title("Права")
                 //      .Mnemonic("Cadastral_RightDocs")
                 //      .IsReadOnly()
                 //      .Filter((uow, q, id) => {
                 //          return uow.GetRepository<FileCardAndLegalRight>()
                 //             .Filter(f => !f.Hidden && f.ObjRigth != null && !f.ObjRigth.Hidden &&
                 //             f.ObjRigth.EstateID == id && f.ObjLeft != null)
                 //             .Select(p => p.ObjLeft);
                 //      }
                 // ).Visible(true).Order(1))

                 //ограничения/обременения
                 .AddOneToManyAssociation<Encumbrance>("SeparLand_Encumbrances",
                       y => y.TabName(EstateTabs.Encumbrances12)
                       .Mnemonic("Cadastral_Encumbrances")
                       .Create((uofw, entity, id) =>
                       {
                           entity.Estate = uofw.GetRepository<Estate>().Find(id);
                       })
                       .Delete((uofw, entity, id) =>
                       {
                           entity.Estate = null;
                       })
                       .Filter((uofw, q, id, oid) => q.Where(w => w.EstateID == id))
                       .Visible(true).Order(1))

                 ////фотографии
                 //.Add(ed => ed.Images, ac => ac.Title("Фотографии").TabName(EstateTabs.Images13))

                 .Add<PartialEditor>("PartialEditor10", a => a.DataType(PropertyDataType.Custom).Title(" ").TabName(EstateTabs.Links14).Mnemonic(SeparLandLinks14Mnemonic).AddParam("link", $"child.{nameof(IChildrenModel.ParentID)}=parent.{nameof(IBaseObject.ID)}"))
                    .AddOneToManyAssociation<AccountingObject>("SeparLand_AccountingObjects",
                        editor => editor
                            .TabName(EstateTabs.OBU16)
                            .IsReadOnly(true)
                            .Title("Объекты БУ")
                            .IsLabelVisible(false)
                            .Order(1)
                            .Filter((uofw, q, id, oid) =>
                                q.Where(w => w.Estate != null && w.Estate.Oid == oid)))
                 )
                 
                 )
                //документы
                //TODO: Список документов и фото, следующих связанных объектов: объект имущества, кадастровый объект, запись о праве, сделки. Наименование док-та - ссылка на карточку док-та
                .Config.DetailView.Editors
                .AddManyToMany("SeparLand_FileCards"
                    , typeof(FileCardAndEstate)
                    , typeof(IManyToManyLeftAssociation<>)
                    , ManyToManyAssociationType.Rigth
                    , y => y.TabName(EstateTabs.Links14).Visible(true).Order(2))


                //выписки                 
                .AddManyToMany("SeparLand_Extracts"
                    , typeof(CadastralAndExtract)
                    , typeof(IManyToManyRightAssociation<>)
                    , ManyToManyAssociationType.Left
                    , y => y.TabName(EstateTabs.Extract15).Visible(true).Order(1))
                ;
            return conf;

        }

        /// <summary>
        /// Конфигурация реестра ЗУ по умолчанию.
        /// </summary>
        /// <param name="configBuilder"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<SeparLand> Separ_ListView_Default(this ViewModelConfigBuilder<SeparLand> configBuilder)
        {
            return configBuilder
                .ListView(x => x
                    .Title("Земельные участки (SeparLand)")
                    //.Toolbar(factory => factory.Add("GetToolBar", "AdditionalProperty"))
                    .DataSource(ds => ds.Filter(f => !f.IsFake))
                    .Columns(cols => cols
                        .Add(c => c.Name, ac => ac.Title("Наименование").Visible(true).Order(1))
                    )
                    .ColumnsFrom<SeparLandGeneralData>(SeparLandGeneralDataMnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandCharacteristics2>(SeparLandCharacteristics2Mnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandAccountingDt5>(SeparLandAccountingDt5Mnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandClassifiers6>(SeparLandClassifiers6Mnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandCost7>(SeparLandCost7Mnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandState8>(SeparLandState8Mnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandAddress9>(SeparLandAddress9Mnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandRight10>(SeparLandRight10Mnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandLinks14>(SeparLandLinks14Mnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                    .ColumnsFrom<SeparLandAdditionals>(SeparLandAdditionalsMnemonic, nameof(BaseObject.ID), nameof(IChildrenModel.ParentID))
                );
        }

        /// <summary>
        /// Конфигурация реестра ЗУ для мнемоники реестра, отображаемого при открытии пункта навигационного меню.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Land> Partial_ListView_LandMenuList(this ViewModelConfigBuilder<Land> conf)
        {
            return
                 conf.ListView_Default()
                 .ListView(l => l.IsMultiSelect(true));
        }

    }
}
