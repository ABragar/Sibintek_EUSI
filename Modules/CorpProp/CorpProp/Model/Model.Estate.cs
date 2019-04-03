using Base;
using Base.UI;
using CorpProp.Entities.Estate;
using CorpProp.Services.Estate;
using System.Linq;
using CorpProp.Helpers;
using CorpProp.Entities.ManyToMany;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Common;

namespace CorpProp.Model
{
    /// <summary>
    /// Представляет модель объектов имущества.
    /// </summary>
    public static class EstateModel
    {
       
        /// <summary>
        /// Инициализация моделей объектов имущества.
        /// </summary>
        /// <param name="context"></param>
        public static void Init(IInitializerContext context)
        {
            CreateModelConfig(context);
            CorpProp.Model.AircraftModel.CreateModelConfig(context);
            CorpProp.Model.BuildingStructureModel.CreateModelConfig(context);
            CorpProp.Model.CadastralModel.CreateModelConfig(context);
            CorpProp.Model.CarParkingSpaceModel.CreateModelConfig(context);            
            CorpProp.Model.IntangibleAssetModel.CreateModelConfig(context);
            CorpProp.Model.InventoryObjectModel.CreateModelConfig(context);
            CorpProp.Model.LandModel.CreateModelConfig(context);
            CorpProp.Model.MovableEstateModel.CreateModelConfig(context);
            CorpProp.Model.NonCadastralModel.CreateModelConfig(context);
            CorpProp.Model.PropertyComplexModel.CreateModelConfig(context);
            CorpProp.Model.RealEstateModel.CreateModelConfig(context);
            CorpProp.Model.RealEstateComplexModel.CreateModelConfig(context);
            CorpProp.Model.RoomModel.CreateModelConfig(context);
            CorpProp.Model.ShipModel.CreateModelConfig(context);
            CorpProp.Model.SpaceShipModel.CreateModelConfig(context);
            CorpProp.Model.UnfinishedConstructionModel.CreateModelConfig(context);
            CorpProp.Model.VehicleModel.CreateModelConfig(context);
            CorpProp.Model.PropertyComplexIOModel.CreateModelConfig(context);

        }


        /// <summary>
        /// Создает конфигурацию модели объекта имущества по умолчанию.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static void CreateModelConfig(this IInitializerContext context)
        {
            context.CreateVmConfig<AdditionalFeatures>("AdditionalFeatures")
                .DetailView(builder => builder.Editors(factory => { }
                ));

            context.CreateVmConfig<AdditionalFeatures>("Estate_AdditionalFeatures")
                .DetailView(builder => builder.Editors(factory => { }
                ));

            context.CreateVmConfig<Estate>()
                   .Service<IEstateService>()
                   .Title("Объект имущества")
                   .DetailView_Default()
                   .ListView_Default()
                   .LookupProperty(x => x.Text(c => c.Name));
        }

        /// <summary>
        /// Конфигурация карточки объекта имущества по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Estate> DetailView_Default(this ViewModelConfigBuilder<Estate> conf)
        {
            return
               conf.DetailView(x => x
               .Title(conf.Config.Title)
               .Editors(editors => editors
                  //общая информация
                  .Add(ed => ed.Name, ac => ac.Title("Наименование").TabName(EstateTabs.GeneralInfo).IsRequired(true))
                  //.Add(ed => ed.EstateType, ac => ac.Title("Класс КС").TabName(EstateTabs.GeneralInfo))
                  .Add(ed => ed.Description, ac => ac.Title("Описание").TabName(EstateTabs.GeneralInfo))

                  ////данные БУ
                  //.Add(ed => ed.ExternalID, ac => ac.Title("Внутренний №").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.InventoryNumber, ac => ac.Title("Инвентарный номер").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").TabName(EstateTabs.AccountingData3))
                  .Add(ed => ed.Owner, ac => ac.Title("Балансодержатель").TabName(EstateTabs.AccountingData3).Visible(true).IsReadOnly(true).Order(40))
                  .Add(ed => ed.WhoUse, ac => ac.Title("Пользователь").TabName(EstateTabs.AccountingData3).Visible(true).IsReadOnly(true).Order(50))
                  //.Add(ed => ed.AccountNumber, ac => ac.Title("Счет").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.ClassFixedAsset, ac => ac.Title("Класс БУ").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.AccountingName, ac => ac.Title("Наименование").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.AccountingDescription, ac => ac.Title("Описание").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").TabName(EstateTabs.AccountingData3))
                  .Add(ed => ed.DealProps, ac => ac.Title("Реквизиты договора").TabName(EstateTabs.AccountingData3).Visible(true).IsReadOnly(true).Order(60))

                  //.Add(ed => ed.MOL, ac => ac.Title("МОЛ").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.DateOfReceipt, ac => ac.Title("Дата оприходования").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.ReceiptReason, ac => ac.Title("Причина поступления").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.LeavingDate, ac => ac.Title("Дата списания").TabName(EstateTabs.AccountingData3))
                  //.Add(ed => ed.LeavingReason, ac => ac.Title("Причина выбытия").TabName(EstateTabs.AccountingData3))

                  ////классификаторы
                  //.Add(ed => ed.EstateType, ac => ac.Title("Класс КС").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKOFCode, ac => ac.Title("Код ОКОФ").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKOFName, ac => ac.Title("Класс ОКОФ").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKOFCode2, ac => ac.Title("Код ОКОФ 2").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKOFName2, ac => ac.Title("Класс ОКОФ 2").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKTMOCode, ac => ac.Title("Код ОКТМО").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKTMOName, ac => ac.Title("ОКТМО").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKTMORegion, ac => ac.Title("Регион по ОКТМО").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKATO, ac => ac.Title("ОКАТО").TabName(EstateTabs.Classifiers3))
                  //.Add(ed => ed.OKATORegion, ac => ac.Title("Регион по ОКАТО").TabName(EstateTabs.Classifiers3))

                  ////стоимость
                  //.Add(ed => ed.UpdateDate, ac => ac.Title("Дата обновления информации").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.DepreciationCost, ac => ac.Title("Начисленная амортизация, руб.").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.Useful, ac => ac.Title("СПИ").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.UsefulEnd, ac => ac.Title("Оставшийся срок использования").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.LeavingCost, ac => ac.Title("Стоимость выбытия, руб.").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.UsefulEndDate, ac => ac.Title("Дата окончания СПИ").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.MarketCost, ac => ac.Title("Рыночная стоимость, руб.").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.MarketDate, ac => ac.Title("Дата рыночной оценки").TabName(EstateTabs.Cost4))
                  //.Add(ed => ed.AppraisalFileCard, ac => ac.Title("Реквизиты отчета об оценке").TabName(EstateTabs.Cost4))

                  ////состояние
                  //.Add(ed => ed.InConservation, ac => ac.Title("Признак консервации").TabName(EstateTabs.State5))
                  //.Add(ed => ed.ConservationFrom, ac => ac.Title("Дата начала консервации").TabName(EstateTabs.State5))
                  //.Add(ed => ed.ConservationTo, ac => ac.Title("Дата окончания консервации").TabName(EstateTabs.State5))
                  //.Add(ed => ed.Status, ac => ac.Title("Статус").TabName(EstateTabs.State5))
                  //.Add(ed => ed.StartDate, ac => ac.Title("Дата начала").TabName(EstateTabs.State5))
                  //.Add(ed => ed.EndDate, ac => ac.Title("Дата окончания").TabName(EstateTabs.State5))

                  //.Add(ed => ed.SubjectName, ac => ac.Title("Контрагент по договору").TabName(EstateTabs.State5))


                  .AddManyToManyRigthAssociation<FileCardAndEstate>("Estate_FileCards", y => y.TabName(EstateTabs.Links5))
                 
                  ////фотографии
                  //.Add(ed => ed.Images, ac => ac.Title("Фотографии").TabName(EstateTabs.Links5))

                  //.Add(ed => ed.Land, ac => ac.Title("Земельный участок").TabName(EstateTabs.Links5))

                  .AddOneToManyAssociation<AccountingObject>("Estate_AccountingObjects",
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
                );

        }

        /// <summary>
        /// Конфигурация реестра объекта имущества по умолчанию.
        /// </summary>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static ViewModelConfigBuilder<Estate> ListView_Default(this ViewModelConfigBuilder<Estate> conf)
        {
            return
            conf.ListView(x => x
                .Title("объекты имущества")
                .Columns(col => col
                    .Add(c => c.Owner, ac => ac.Title("Балансодержатель").Visible(true).Order(10))
                    .Add(c => c.WhoUse, ac => ac.Title("Пользователь").Visible(true).Order(20))
                    .Add(c => c.Name, ac => ac.Title("Наименование БУ").Visible(true).Order(30))
                    .Add(c => c.InventoryNumber, ac => ac.Title("Инвентарный номер").Visible(true).Order(40))
                    .Add(c => c.InventoryNumber2, ac => ac.Title("Инвентарный номер в старой БУС").Visible(true).Order(50))
                    
                    //.Add(c => c.ClassFixedAsset, ac => ac.Title("Класс БУ").Visible(true).Order(50))
                    //.Add(c => c.DateOfReceipt, ac => ac.Title("Дата оприходования").Visible(true).Order(13))
                    //.Add(c => c.EstateType, ac => ac.Title("Класс КС").Visible(true).Order(14))
                    //.Add(c => c.OKTMORegion, ac => ac.Title("Регион по ОКТМО").Visible(true).Order(15))
                    //.Add(c => c.InitialCost, ac => ac.Title("Первоначальная стоимость, руб.").Visible(true).Order(16))
                    //.Add(c => c.ResidualCost, ac => ac.Title("Остаточная стоимость, руб.").Visible(true).Order(17))

                    ////скрытые колонки      
                    //.Add(c => c.ExternalID, ac => ac.Title("Внутренний №").Visible(false))
                    //.Add(c => c.AccountNumber, ac => ac.Title("Счет").Visible(false))
                    //.Add(c => c.BusinessArea, ac => ac.Title("Обособленное подразделение/БС").Visible(false))
                    .Add(c => c.IsNonCoreAsset, ac => ac.Title("Признак ННА").Visible(true).Order(60))
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
                    //.Add(c => c.DepreciationCost, ac => ac.Title("Начисленная амортизация, руб.").Visible(false))
                    //.Add(c => c.InConservation, ac => ac.Title("Признак консервации").Visible(false))
                    //.Add(c => c.ConservationFrom, ac => ac.Title("Дата начала консервации").Visible(false))
                    //.Add(c => c.ConservationTo, ac => ac.Title("Дата окончания консервации").Visible(false))
                    //.Add(c => c.Status, ac => ac.Title("Статус").Visible(false))
                    //.Add(c => c.StartDate, ac => ac.Title("Дата начала").Visible(false))
                    //.Add(c => c.EndDate, ac => ac.Title("Дата окончания").Visible(false))
                    .Add(c => c.DealProps, ac => ac.Title("Реквизиты договора").Visible(true).Order(60))
                    //.Add(c => c.SubjectName, ac => ac.Title("Контрагент по договору").Visible(false))
                    .Add(c => c.ID, ac => ac.Title("Идентификатор").Visible(false))

                ));
        }       
    }
}
