using Base;
using Base.UI;
using CorpProp.Entities.Accounting;
using EUSI.Entities.Accounting;
using EUSI.Services.Accounting;
using System;
using System.Linq;
using EUSI.Helpers;
using CorpProp.Entities.ManyToMany;
using Base.UI.Editors;
using Base.UI.ViewModal;
using EUSI.Entities.ManyToMany;
using CorpProp.Entities.NSI;
using System.Collections.Generic;

namespace EUSI.Model
{
    public static class AccountingModel
    {
        public static void Init(IInitializerContext context, IViewModelConfigService viewModelConfigService)
        {
            //изменение всех конфигов ОБУ
            var configs = context.GetVmConfigs()
                .Where(w => w.Value != null && Type.Equals(w.Value.TypeEntity, typeof(AccountingObject)))
                .DefaultIfEmpty()
                .Select(s => s.Value)
                .ToList();

            foreach (var conf in configs)
            {
                context.ModifyVmConfig<AccountingObject>(conf.Mnemonic)
                    .Title("ОС/НМА")
                    .Service<IAccountingObjectExtService>()
                    .DetailView_Default()
                    .ListView_Default()
                    .ChangePropNames()
                    .IsReadOnly(false);
            }

            foreach (var editor in context.GetVmConfigs()
                       .Select(s => s.Value)
                       .SelectMany(c => c.DetailView.Editors)
                       .Where(e =>
                       Type.Equals(e.EditorType, typeof(AccountingObject))
                       && (e is OneToManyAssociationEditor || e is ManyToManyAssociationEditor))
                       )
            {
                editor.Title = editor.Title.Replace("ОБУ", "ОС/НМА");
                editor.TabName = editor.Title.Replace("ОБУ", "ОС/НМА");
            }

            context.CreateVmConfig<DraftOS>()
                   .Title("Контроль полученных данных по прототипам ОС/НМА из БУС")
                   .Service<IDraftOSService>()
                   .ListView(lv => lv
                   .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                   .OnClientEditRow(@"mnemonic = 'AccountingObject';")
                   .Toolbar(tl => tl.Add("GetDraftOSToolbar", "Custom",
                        a => a.Title("Параметры").IsAjax(true).ListParams(l => l.Add("customID", "[ID]"))))
                   );

            context.CreateVmConfig<DraftOS>("DraftOSPassBus")
               .Title("Контроль отправки уведомлений по прототипам ОС/НМА")
               .Service<IDraftOSPassBusService>()
               .ListView(lv => lv
                   .Columns(c => c.Add(x => x.Comment, ac => ac.Visible(true)))
                   .Columns(c => c.Add(x => x.NotifyOriginator, ac => ac.Visible(true)))
                   .Columns(c => c.Add(x => x.NotifyOriginatorDate, ac => ac.Visible(true)))
                   .Columns(c => c.Add(x => x.NotifyBUS, ac => ac.Visible(true)))
                   .Columns(c => c.Add(x => x.NotifyBUSDate, ac => ac.Visible(true)))
                   .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                   .OnClientEditRow(@"mnemonic = 'AccountingObject';")
                   .IsMultiSelect(true)
               )
               .IsReadOnly();

            context.CreateVmConfigOnBase<AccountingObject>(nameof(AccountingObject), "ArchivedObu")
                .Title("ОС/НМА помеченные на удаление")
                .Service<IArchivedObuExtService>()
                .DetailView(dv => dv.Title("ОС/НМА помеченный на удаление"))
                .ListView(lv => lv.Title("ОС/НМА помеченные на удаление"))
                ;

            context.CreateVmConfigOnBase<AccountingObject>(nameof(AccountingObject), "NoImportDataOS")
                  .Title("Контроль передачи данных по действующим ОС/НМА из БУС")
                  .Service<NoImportDataOSService>()
                  .DetailView_Default()
                  .ListView(lv => lv
                      .Title("Контроль передачи данных по действующим ОС/НМА из БУС")
                      .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
                      .Columns(cols => cols.Clear()
                          .Add(c => c.EUSINumber, ac => ac.Visible(true))
                          .Add(c => c.InventoryNumber, ac => ac.Visible(true))
                          .Add(c => c.Consolidation, ac => ac.Visible(true))
                          .Add(c => c.NameEUSI, ac => ac.Visible(true))
                          .Add(c => c.StateObjectRSBU, ac => ac.Visible(true))

                           .Add(c => c.ReceiptReason, ac => ac.Visible(false))
                           .Add(c => c.OKTMO, ac => ac.Visible(false))
                           .Add(c => c.NameByDoc, ac => ac.Visible(false))
                           .Add(c => c.OKOF2014, ac => ac.Visible(false).Title("ОКОФ"))
                           .Add(c => c.EstateMovableNSI, ac => ac.Visible(false))
                           .Add(c => c.ExternalID, ac => ac.Visible(false))
                           .Add(c => c.Name, ac => ac.Visible(false).Title("Наименование объекта (в соответствии с учетной системой)"))
                           .Add(c => c.InServiceDate, ac => ac.Visible(false))
                           .Add(c => c.InitialCost, ac => ac.Visible(false))
                           .Add(c => c.SibCountry, ac => ac.Visible(false))
                           .Add(c => c.SibFederalDistrict, ac => ac.Visible(false))
                           .Add(c => c.Region, ac => ac.Visible(false))
                           .Add(c => c.SibCityNSI, ac => ac.Visible(false))
                           .Add(c => c.Address, ac => ac.Visible(false))
                           .Add(c => c.LeavingDate, ac => ac.Visible(false))
                           .Add(c => c.Department, ac => ac.Visible(false))
                           .Add(c => c.MOL, ac => ac.Visible(false))
                           .Add(c => c.PositionConsolidation, ac => ac.Visible(false))
                           .Add(c => c.DepreciationMethodRSBU, ac => ac.Visible(false))
                           .Add(c => c.Useful, ac => ac.Visible(false))
                           .Add(c => c.StartDateUse, ac => ac.Visible(false))
                           .Add(c => c.ResidualCost, ac => ac.Visible(false))
                           .Add(c => c.DepreciationMultiplierForNU, ac => ac.Visible(false))
                           .Add(c => c.SPPCode, ac => ac.Visible(false))
                           .Add(c => c.DepreciationGroup, ac => ac.Visible(false))
                           .Add(c => c.LessorSubject, ac => ac.Visible(false))
                           .Add(c => c.ProprietorSubject, ac => ac.Visible(false))
                  ))
                  .DetailView(dt =>
                        dt.Title("Контроль передачи данных по действующим ОС/НМА из БУС"))
                  .IsReadOnly();

            context.CreateVmConfig<MigrateOS>()
                .Title("Миграция ОС/НМА")
                .Service<IMigrateOSService>()
                .ListView(lv => lv
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete }));

            context.CreateVmConfig<RentalOS>()
                .Title("ФСД (аренда)")
                .Service<IRentalOSService>()
                .ListView(lv => lv
                .HiddenActions(new[] { LvAction.Create, LvAction.Delete }))
                .IsReadOnly(true);

            context.CreateVmConfig<RentalOSMoving>()
               .Title("ФСД (движения аренда)")
               .Service<IRentalOSMovingService>()
               .ListView(lv => lv
               .HiddenActions(new[] { LvAction.Create, LvAction.Delete }))
               .IsReadOnly(true);

            context.CreateVmConfig<RentalOSState>()
               .Title("ФСД (состояния аренда)")
               .Service<IRentalOSStateService>()
               .ListView(lv => lv
               .HiddenActions(new[] { LvAction.Create, LvAction.Delete }))
               .IsReadOnly(true);

            context.CreateVmConfig<RentalOS>("OS_Rental")
              .Title("ИР-Аренда")
              .Service<IRentalOSService>()
              .DetailView(dv => dv.Editors(eds => eds.Clear()
                    .Add(ed => ed.OKOF2014)
                    .Add(ed => ed.ProprietorSubject)
                    .Add(ed => ed.RentContractNumber)
                    .Add(ed => ed.CostKindRentalPayments)
                    .Add(ed => ed.LandPurpose)
                    .Add(ed => ed.UsefulEndLand)
                    .Add(ed => ed.InventoryArendaLand)
                    .Add(ed => ed.IndicationLicenceLandArea)
                    .Add(ed => ed.InfrastructureExist)
                    .Add(ed => ed.AssetHolderRSBU)
                    .Add(ed => ed.TransferRight)
                    .Add(ed => ed.RedemptionDate)
                    .Add(ed => ed.RedemptionCost)
                    .Add(ed => ed.Deposit)
                    .Add(ed => ed.ObjectLocationRent)
                    .Add(ed => ed.InitialCost)
                    .Add(ed => ed.Currency)
                    .Add(ed => ed.TakeOrPay)
                    .Add(ed => ed.SubsoilUser)
                    .Add(ed => ed.TransactionKind)
                    .Add(ed => ed.StateObjectRent)
                    .Add(ed => ed.Comments)
                ))
              .ListView(lv => lv
              .HiddenActions(new[] { LvAction.Create, LvAction.Delete }))
              .IsReadOnly(true);
        }

        public static ViewModelConfigBuilder<AccountingObject> DetailView_Default(this ViewModelConfigBuilder<AccountingObject> conf)
        {
            //Для окраски полей в зависимости от источника данных.
            //см. WebUI\Views\Standart\DetailView\Editor\Common\Editor.cshtml
            var Source = "Source";
            var ER = "ER";
            var BU = "BU";

            return
              conf.DetailView(dv => dv
              .Title("ОС/НМА")
              .Editors(eds => eds.Clear()
              //общие
              .Add(ed => ed.Estate, ac => ac.Title("Номер ЕУСИ").Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.NameEUSI, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.Consolidation, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, ER))
              .Add(ed => ed.ReceiptReason, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, ER))
              .Add(ed => ed.Contragent, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, ER))
              .Add(ed => ed.EstateDefinitionType, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.IntangibleAssetType, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.NameByDoc, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.Name, ac => ac.Title("Наименование объекта (в соответствии с учетной системой)").Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.InventoryNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.SubNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.AccountNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.SPPCode, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.ExternalID, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.CadastralNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.ActRentDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.StartDateUse, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.InServiceDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.InServiceDateMSFO, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.LeavingDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.LeavingReason, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.ShipRegDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.EstateMovableNSI, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().IsRequired(true).AddParam(Source, BU))
              .Add(ed => ed.Department, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.MOL, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.SibCountry, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.SibFederalDistrict, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.Region, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.SibCityNSI, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.Address, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.FactAddress, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.StateObjectRSBU, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.StateObjectMSFO, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.ShareRightNumerator, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.ShareRightDenominator, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.BusinessArea, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.SPPItem, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.ConservationDocInfo, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.ConservationReturnInfo, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.AccountLedgerLUS, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.Comment, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1))
              .Add(ed => ed.IsArchived, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())

              //без вкладки, кидаем в общие
              .Add(ed => ed.OutOfBalance, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly())
              .Add(ed => ed.PrimaryDocNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, ER))
              .Add(ed => ed.PrimaryDocDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.General1).IsReadOnly().AddParam(Source, ER))

              //классификаторы
              .Add(ed => ed.OKTMO, ac => ac.Visible(true).TabName(AccountingObjectTabs.Class2).IsReadOnly().IsRequired(true).AddParam(Source, BU))
              .Add(ed => ed.OKOF2014, ac => ac.Visible(true).TabName(AccountingObjectTabs.Class2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.DepreciationGroup, ac => ac.Visible(true).TabName(AccountingObjectTabs.Class2).IsReadOnly().AddParam(Source, BU).IsRequired(true))
              .Add(ed => ed.PositionConsolidation, ac => ac.Visible(true).TabName(AccountingObjectTabs.Class2).IsReadOnly().IsRequired(true).AddParam(Source, BU))
              .Add(ed => ed.AddonOKOF, ac => ac.Visible(true).TabName(AccountingObjectTabs.Class2).IsReadOnly().IsRequired(true).AddParam(Source, BU))

              //специфические
              .Add(ed => ed.PermittedUseKind, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.AddonAttributeGroundCategory, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.PermittedByDoc, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.GroundCategory, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.LandType, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.LandPurpose, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.CadRegDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.RegNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).AddParam(Source, BU).IsReadOnly())
              .Add(ed => ed.RightRegDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).AddParam(Source, BU).IsReadOnly())
              .Add(ed => ed.RightRegEndDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).AddParam(Source, BU).IsReadOnly())
              .Add(ed => ed.Area, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.Year, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.BuildingLength, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.PipelineLength, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.VehicleClass, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.TaxVehicleKindCode, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.YearOfIssue, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.VehicleCategory, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.DieselEngine, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.VehicleLabel, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.SibMeasure, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().Title("Единицы измерения налоговой базы. Транспортный налог").AddParam(Source, BU))
              .Add(ed => ed.Power, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU).Title("Налоговая база. \"Транспортный налог (мощность ТС, валовая вместимость, паспортная стат. тяга, единица ТС)\""))
              .Add(ed => ed.SerialNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.EngineSize, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.Model, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.VehicleAverageCost, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.VehicleRegNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.VehicleTaxFactor, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().IsRequired(true).AddParam(Source, BU))
              .Add(ed => ed.IsSocial, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.IsCultural, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.ShipRegNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.InOtherSystem, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.DivisibleType, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.Deposit, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.LicenseArea, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().IsRequired(true).AddParam(Source, BU))
              .Add(ed => ed.WellCategory, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.Bush, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.Well, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.IsInvestment, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.ForSale, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.CostForSale, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.RentTerm, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.TransferRight, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.RedemptionDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.RedemptionCost, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.Explanation, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly())
              .Add(ed => ed.EcoKlass, ac => ac.Visible(true).TabName(AccountingObjectTabs.Special2).IsReadOnly().AddParam(Source, BU))

               //стоимостные
               .Add(ed => ed.InitialCost, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.InitialCostNU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.InitialCostMSFO, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.ResidualCost, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.ResidualCostNU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.ResidualCostEstimate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.ResidualCostMSFO, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.Useful, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().IsRequired(true).AddParam(Source, BU))
               .Add(ed => ed.UsefulForNU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().IsRequired(true).AddParam(Source, BU))
               .Add(ed => ed.UsefulForMSFO, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.UsefulEnd, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.UsefulEndNU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.UsefulEndMSFO, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.DepreciationMethodRSBU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().IsRequired(true).AddParam(Source, BU))
               .Add(ed => ed.DepreciationMethodNU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().IsRequired(true).AddParam(Source, BU))
               .Add(ed => ed.DepreciationMethodMSFO, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.DepreciationMultiplierForNU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().IsRequired(true).AddParam(Source, BU))
               .Add(ed => ed.CadastralValue, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly())
               .Add(ed => ed.CadastralValueAppDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly())
               .Add(ed => ed.AnnualCostAvg, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.VehicleMarketCost, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly())
               .Add(ed => ed.EstimatedAmount, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.EstimatedAmountWriteOffTerm, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.EstimatedAmountWriteOffStart, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.IXODepreciation, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.IXOPSt, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.DepreciationCostNU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.AccumulatedDepreciationRSBU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Cost3).IsReadOnly().AddParam(Source, BU))

               //договоры
               .Add(ed => ed.LessorSubject, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly())
               .Add(ed => ed.ProprietorSubject, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly())
               .Add(ed => ed.RentTypeRSBU, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly())
               .Add(ed => ed.RentTypeMSFO, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly())
               .Add(ed => ed.RentContractNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.RentContractNumberSZVD, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.RentContractDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly().AddParam(Source, BU))
               .Add(ed => ed.ContractNumber, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly().AddParam(Source, ER))
               .Add(ed => ed.ContractDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Contract4).IsReadOnly().AddParam(Source, ER))

              //налоги
              .Add(ed => ed.VehicleRegDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.VehicleDeRegDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateType, ac => ac.Visible(false).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.DecisionsDetails, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.DecisionsDetailsTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.DecisionsDetailsLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.Benefit, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.IsEnergy, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.EnergyLabel, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.EnergyDocsExist, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.BenefitApplyForEnergy, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.BenefitDocsExist, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.IsInvestmentProgramm, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxCadastralIncludeDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxCadastralIncludeDoc, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxBaseEstate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxBase, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.BenefitApply, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.BenefitApplyTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.BenefitApplyLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateValue, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateValueTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateValueLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemption, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxFreeTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxFreeLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxLower, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxLowerTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxLowerLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateLower, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateLowerTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateLowerLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionReasonEstate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionReasonEstateTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionReasonEstateLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionReason, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionReasonTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionReasonLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateWithExemptionEstate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxRateWithExemptionEstateTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxRateWithExemptionEstateLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxRateWithExemption, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateWithExemptionTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxRateWithExemptionLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionStartDateEstate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionStartDateEstateTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionStartDateEstateLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionStartDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionStartDateTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionStartDateLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionEndDateEstate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionEndDateEstateTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionEndDateEstateLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly())
              .Add(ed => ed.TaxExemptionEndDate, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionEndDateTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxExemptionEndDateLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.IFNS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxLowerPercent, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxLowerPercentTS, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))
              .Add(ed => ed.TaxLowerPercentLand, ac => ac.Visible(true).TabName(AccountingObjectTabs.Tax5).IsReadOnly().AddParam(Source, BU))

              //движения
              .AddOneToManyAssociation<AccountingMoving>("AccountingObjectExt_AccountingMovings_RSBU",
                        editor => editor
                        .TabName(AccountingObjectTabs.MovingsRSBU9)
                        .Mnemonic("AccMovingRSBU")
                        .Title("Регистр движений РСБУ")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                  .Filter((uofw, q, id, oid) =>
                      q.Where(w =>
                      w.Angle != null && w.Angle.Code == "RSBU" &&
                      w.AccountingObject != null
                      && w.AccountingObject.Oid == oid)))

               .AddOneToManyAssociation<AccountingMoving>("AccountingObjectExt_AccountingMovings_MSFO",
                        editor => editor
                        .TabName(AccountingObjectTabs.MovingsMSFO10)
                        .Mnemonic("AccMovingMSFO")
                        .Title("Регистр движений МСФО")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                  .Filter((uofw, q, id, oid) =>
                      q.Where(w =>
                      w.Angle != null && w.Angle.Code == "MSFO" &&
                      w.AccountingObject != null
                      && w.AccountingObject.Oid == oid)))

               .AddOneToManyAssociation<AccountingMoving>("AccountingObjectExt_AccountingMovings_NU",
                        editor => editor
                        .TabName(AccountingObjectTabs.MovingsNU12)
                        .Mnemonic("AccMovingNU")
                        .Title("Регистр движений НУ")
                        .IsReadOnly(true)
                        .IsLabelVisible(false)
                  .Filter((uofw, q, id, oid) =>
                      q.Where(w =>
                      w.Angle != null && w.Angle.Code == "NU" &&
                      w.AccountingObject != null
                      && w.AccountingObject.Oid == oid)))

               .AddOneToManyAssociation<AccountingMovingMSFO>("AccountingObject_Debit01",
                        editor => editor
                        .TabName(AccountingObjectTabs.Movings11)
                        .Mnemonic("Debit01")
                        .Title("Дебетование 01")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                .Filter((uofw, q, id, oid) =>
                      q.Where(w =>
                      w.TypeMovingMSFO == TypeMovingMSFO.Debit01 &&
                      w.AccountingObject != null
                      && w.AccountingObject.Oid == oid)))

               .AddOneToManyAssociation<AccountingMovingMSFO>("AccountingObject_Credit01",
                        editor => editor
                        .TabName(AccountingObjectTabs.Movings11)
                        .Mnemonic("Credit01")
                        .Title("Кредитование 01")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                  .Filter((uofw, q, id, oid) =>
                      q.Where(w =>
                      w.TypeMovingMSFO == TypeMovingMSFO.Credit01 &&
                      w.AccountingObject != null
                      && w.AccountingObject.Oid == oid)))

               .AddOneToManyAssociation<AccountingMovingMSFO>("AccountingObject_Depreciation01",
                        editor => editor
                        .TabName(AccountingObjectTabs.Movings11)
                        .Mnemonic("Depreciation01")
                        .Title("Амортизация 01")
                        .IsReadOnly(true)
                        .IsLabelVisible(true)
                  .Filter((uofw, q, id, oid) =>
                      q.Where(w =>
                      w.TypeMovingMSFO == TypeMovingMSFO.Depreciation01
                      && w.AccountingObject != null
                      && w.AccountingObject.Oid == oid)))

                .AddOneToManyAssociation<AccountingCalculatedField>("AccountingObject_AccountingCalculatedField",
                        editor => editor
                        .Mnemonic("AccountingFields")
                        .IsReadOnly(false)
                        .Title("Расчеты")
                        .TabName("Расчеты")
                        .IsLabelVisible(false)
                        .Filter((uofw, q, id, oid) =>
                        q.Where(w => w.AccountingObjectID == id).OrderByDescending(o => o.CalculateDate).GroupBy(g => g.Year).Select(s => s.FirstOrDefault())
                        ))

              //Перечень первичных документов
              .AddManyToManyRigthAssociation<FileCardAndAccountingObject>("AccountingObjectExt_FileCards",
                        editor => editor
                        .TabName(AccountingObjectTabs.Files8)
                        .IsReadOnly(true))

              //заявки
              .AddManyToManyLeftAssociation<AccountingObjectAndEstateRegistrationObject>("AccountingObject_EstateRegistrations",
                        editor => editor
                        .TabName("[15]Заявки ЕУСИ"))

              .AddPartialEditor("OS_Rental", nameof(IBaseObject.ID), nameof(RentalOS.AccountingObjectID),
                        peb => peb.TabName("[17]ИР-Аренда"))

              ).Editors(eds => eds.ResetOrder())
              .DefaultSettings((uow, obj, model) =>
              {
                  if (obj.EstateDefinitionTypeID == null) return;
                  var estate = uow.GetRepository<CorpProp.Entities.Estate.Estate>()
                  .Filter(f => f.ID == obj.EstateID)
                  .FirstOrDefault();

                  var estType = estate.GetType();
                  var isReals = estType.IsReals();
                  var isLand = estType.IsLand();
                  var isTS = estType.IsTS();
                  var isOthers = estType.IsOthers();
                  var isNKS = estType.IsNKS();
                  var isNMA = estType.IsNMA();

                  model.Visible(ed => ed.AddonAttributeGroundCategory, isLand);
                  model.Visible(ed => ed.AddonOKOF, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.Address, isReals || isLand || isNKS);
                  model.Visible(ed => ed.AnnualCostAvg, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.Area, isReals || isLand || isNKS);
                  model.Visible(ed => ed.Benefit, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.BenefitApply, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.BenefitApplyForEnergy, isOthers);
                  model.Visible(ed => ed.BenefitApplyLand, isLand);
                  model.Visible(ed => ed.BenefitApplyTS, isTS);
                  model.Visible(ed => ed.BenefitDocsExist, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.BuildingLength, isReals || isOthers || isNKS);
                  model.Visible(ed => ed.Bush, isReals || isOthers);
                  model.Visible(ed => ed.CadastralNumber, isReals || isLand);
                  model.Visible(ed => ed.CadastralValue, isReals || isLand);
                  model.Visible(ed => ed.CadRegDate, isReals || isLand);
                  model.Visible(ed => ed.ConservationDocInfo, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.ConservationReturnInfo, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.ContainmentVolume, isReals || isOthers);
                  model.Visible(ed => ed.DecisionsDetails, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.DecisionsDetailsLand, isLand);
                  model.Visible(ed => ed.DecisionsDetailsTS, isTS);
                  model.Visible(ed => ed.DepreciationCostNU, isReals || isTS || isOthers || isNMA || isNKS);
                  model.Visible(ed => ed.DepreciationGroup, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.DepreciationMethodMSFO, isReals || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.DepreciationMethodNU, isReals || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.DepreciationMethodRSBU, isReals || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.DepreciationMultiplierForNU, isReals || isLand || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.DieselEngine, isTS);
                  model.Visible(ed => ed.DivisibleType, isReals || isTS || isOthers || isNMA || isNKS);
                  model.Visible(ed => ed.EcoKlass, isTS);
                  model.Visible(ed => ed.EnergyDocsExist, isOthers);
                  model.Visible(ed => ed.EnergyLabel, isOthers);
                  model.Visible(ed => ed.EngineSize, isTS);
                  model.Visible(ed => ed.EstateDefinitionType, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.EstateMovableNSI, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.EstateType, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.EstimatedAmount, isReals || isLand || isTS || isOthers);
                  model.Visible(ed => ed.EstimatedAmountWriteOffStart, isReals || isLand || isTS || isOthers);
                  model.Visible(ed => ed.EstimatedAmountWriteOffTerm, isReals || isLand || isTS || isOthers);
                  model.Visible(ed => ed.FactAddress, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.GroundCategory, isLand);
                  model.Visible(ed => ed.InOtherSystem, isTS);
                  model.Visible(ed => ed.IntangibleAssetType, isNMA);
                  model.Visible(ed => ed.InventoryNumber, isReals || isLand || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.IsCultural, isReals || isLand);
                  model.Visible(ed => ed.IsEnergy, isOthers);
                  model.Visible(ed => ed.IsInvestmentProgramm, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.LandPurpose, isLand);
                  model.Visible(ed => ed.LandType, isLand);
                  model.Visible(ed => ed.Model, isTS);
                  model.Visible(ed => ed.MOL, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.OKTMO, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.OwnershipType, isReals || isLand || isNKS);
                  model.Visible(ed => ed.PermittedUseKind, isReals || isLand || isNKS);
                  model.Visible(ed => ed.PipelineLength, isReals || isOthers || isNKS);
                  model.Visible(ed => ed.Power, isTS);
                  model.Visible(ed => ed.Region, isReals || isLand || isTS || isNKS);
                  model.Visible(ed => ed.RentContractNumberSZVD, isReals || isTS || isOthers || isNMA || isNKS);
                  model.Visible(ed => ed.ResidualCostEstimate, isReals || isLand || isTS || isOthers);
                  model.Visible(ed => ed.RegNumber, isReals || isLand || isNKS);
                  model.Visible(ed => ed.RightRegDate, isReals || isLand || isNKS);
                  model.Visible(ed => ed.RightRegEndDate, isReals || isLand || isNKS);
                  model.Visible(ed => ed.SerialNumber, isTS);
                  model.Visible(ed => ed.ShareRightDenominator, isReals || isLand || isTS || isOthers);
                  model.Visible(ed => ed.ShareRightNumerator, isReals || isLand || isTS || isOthers);
                  model.Visible(ed => ed.ShipRegDate, isTS);
                  model.Visible(ed => ed.ShipRegNumber, isTS);
                  model.Visible(ed => ed.SibCityNSI, isReals || isLand || isNKS);
                  model.Visible(ed => ed.SibFederalDistrict, isReals || isLand || isTS || isNKS);
                  model.Visible(ed => ed.SibMeasure, isTS);
                  model.Visible(ed => ed.SPPCode, isNKS);
                  model.Visible(ed => ed.SPPItem, isNKS);
                  model.Visible(ed => ed.StartDateUse, isNKS);
                  model.Visible(ed => ed.SubNumber, isReals || isLand || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.TaxBase, isReals || isLand || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxBaseEstate, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxCadastralIncludeDate, isReals || isLand);
                  model.Visible(ed => ed.TaxCadastralIncludeDoc, isReals || isLand);
                  model.Visible(ed => ed.TaxExemption, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemption, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemption, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemptionTS, isTS);
                  model.Visible(ed => ed.TaxExemptionTS, isTS);
                  model.Visible(ed => ed.TaxExemptionTS, isTS);
                  model.Visible(ed => ed.TaxExemptionLand, isLand);
                  model.Visible(ed => ed.TaxExemptionLand, isLand);
                  model.Visible(ed => ed.TaxExemptionLand, isLand);
                  model.Visible(ed => ed.TaxExemptionEndDate, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemptionEndDateEstate, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemptionEndDateEstateLand, isLand);
                  model.Visible(ed => ed.TaxExemptionEndDateEstateTS, isTS);
                  model.Visible(ed => ed.TaxExemptionEndDateLand, isLand);
                  model.Visible(ed => ed.TaxExemptionEndDateTS, isTS);
                  model.Visible(ed => ed.TaxExemptionReason, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemptionReasonEstate, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemptionReasonEstateLand, isLand);
                  model.Visible(ed => ed.TaxExemptionReasonEstateTS, isTS);
                  model.Visible(ed => ed.TaxExemptionReasonLand, isLand);
                  model.Visible(ed => ed.TaxExemptionReasonTS, isTS);
                  model.Visible(ed => ed.TaxExemptionStartDate, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemptionStartDateEstate, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxExemptionStartDateEstateLand, isLand);
                  model.Visible(ed => ed.TaxExemptionStartDateEstateTS, isTS);
                  model.Visible(ed => ed.TaxExemptionStartDateLand, isLand);
                  model.Visible(ed => ed.TaxExemptionStartDateTS, isTS);
                  model.Visible(ed => ed.TaxFreeLand, isLand);
                  model.Visible(ed => ed.TaxFreeTS, isTS);
                  model.Visible(ed => ed.TaxLower, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxLower, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxLowerLand, isLand);
                  model.Visible(ed => ed.TaxLowerPercent, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxLowerPercentLand, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxLowerPercentTS, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxLowerTS, isTS);
                  model.Visible(ed => ed.TaxRateLower, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxRateLowerLand, isLand);
                  model.Visible(ed => ed.TaxRateLowerTS, isTS);
                  model.Visible(ed => ed.TaxRateType, false);
                  model.Visible(ed => ed.TaxRateValue, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxRateValueLand, isLand);
                  model.Visible(ed => ed.TaxRateValueTS, isTS);
                  model.Visible(ed => ed.TaxRateWithExemption, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxRateWithExemptionEstate, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxRateWithExemptionEstateLand, isLand);
                  model.Visible(ed => ed.TaxRateWithExemptionEstateTS, isReals || isTS || isOthers || isNKS);
                  model.Visible(ed => ed.TaxRateWithExemptionLand, isLand);
                  model.Visible(ed => ed.TaxRateWithExemptionTS, isTS);
                  model.Visible(ed => ed.TaxVehicleKindCode, isTS);
                  model.Visible(ed => ed.Useful, isReals || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.UsefulEnd, isReals || isTS || isOthers || isNMA || isNKS);
                  model.Visible(ed => ed.UsefulEndMSFO, isReals || isTS || isOthers || isNMA || isNKS);
                  model.Visible(ed => ed.UsefulEndNU, isReals || isTS || isOthers || isNMA || isNKS);
                  model.Visible(ed => ed.UsefulForMSFO, isReals || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.UsefulForNU, isReals || isTS || isOthers || isNMA);
                  model.Visible(ed => ed.UsesKind, isReals || isLand || isNKS);
                  model.Visible(ed => ed.VehicleAverageCost, isTS);
                  model.Visible(ed => ed.VehicleCategory, isTS);
                  model.Visible(ed => ed.VehicleClass, isTS);
                  model.Visible(ed => ed.VehicleDeRegDate, isTS);
                  model.Visible(ed => ed.VehicleLabel, isTS);
                  model.Visible(ed => ed.VehicleMarketCost, isReals || isLand || isTS);
                  model.Visible(ed => ed.VehicleRegDate, isTS);
                  model.Visible(ed => ed.VehicleRegNumber, isTS);
                  model.Visible(ed => ed.VehicleTaxFactor, isTS);
                  model.Visible(ed => ed.Well, isReals || isOthers);
                  model.Visible(ed => ed.WellCategory, isReals || isOthers);
                  model.Visible(ed => ed.Year, isReals || isLand || isTS || isOthers);
                  model.Visible(ed => ed.YearOfIssue, isTS);
              })

              );
        }

        public static ViewModelConfigBuilder<AccountingObject> ListView_Default(this ViewModelConfigBuilder<AccountingObject> conf)
        {
            return
                conf.ListView(lv => lv
#if DEBUG
               //.HiddenActions(new[] { LvAction.Create, LvAction.Delete })
#else
               .HiddenActions(new[] { LvAction.Create, LvAction.Delete })
#endif
               .Title("ОС/НМА")
                    .Columns(cols => cols.Clear()
                    .Add(c => c.EUSINumber, ac => ac.Visible(true))
                    .Add(c => c.NameEUSI, ac => ac.Visible(true))
                    .Add(c => c.Consolidation, ac => ac.Visible(true))
                    .Add(c => c.ReceiptReason, ac => ac.Visible(true))
                    .Add(c => c.OKTMO, ac => ac.Visible(true))
                    .Add(c => c.NameByDoc, ac => ac.Visible(true))
                    .Add(c => c.OKOF2014, ac => ac.Visible(true).Title("ОКОФ"))
                    .Add(c => c.EstateMovableNSI, ac => ac.Visible(true))
                    .Add(c => c.InventoryNumber, ac => ac.Visible(true))
                    .Add(c => c.SubNumber, ac => ac.Visible(true))
                    .Add(c => c.AccountNumber, ac => ac.Visible(true))
                    .Add(c => c.ExternalID, ac => ac.Visible(true))
                    .Add(c => c.Name, ac => ac.Visible(true).Title("Наименование объекта (в соответствии с учетной системой)"))
                    .Add(c => c.InServiceDate, ac => ac.Visible(true))
                    .Add(c => c.InitialCost, ac => ac.Visible(true))
                    .Add(c => c.ResidualCost, ac => ac.Visible(true))
                    .Add(c => c.StateObjectRSBU, ac => ac.Visible(true))
                    .Add(c => c.SibCountry, ac => ac.Visible(true))
                    .Add(c => c.SibFederalDistrict, ac => ac.Visible(true))
                    .Add(c => c.Region, ac => ac.Visible(true))
                    .Add(c => c.SibCityNSI, ac => ac.Visible(true))
                    .Add(c => c.Address, ac => ac.Visible(true))
                    .Add(c => c.LeavingDate, ac => ac.Visible(true))
                    .Add(c => c.ShipRegDate, ac => ac.Visible(true))
                    .Add(c => c.Department, ac => ac.Visible(true))
                    .Add(c => c.MOL, ac => ac.Visible(true))
                    .Add(c => c.PositionConsolidation, ac => ac.Visible(true))
                    .Add(c => c.DepreciationMethodRSBU, ac => ac.Visible(true))
                    .Add(c => c.Useful, ac => ac.Visible(true))
                    .Add(c => c.UsefulEnd, ac => ac.Visible(true))
                    .Add(c => c.TransferBUSDate, ac => ac.Visible(true))
                    .Add(c => c.AccountLedgerLUS, ac => ac.Visible(true))
                    .Add(c => c.AccumulatedDepreciationRSBU, ac => ac.Visible(true))
                    .Add(c => c.CreatingFromER, ac => ac.Visible(true))
                    .Add(c => c.CreatingFromERPosition, ac => ac.Visible(true))

                    .Add(c => c.ID, ac => ac.Visible(false))
                    .Add(c => c.Owner, ac => ac.Visible(false))
                    .Add(c => c.WhoUse, ac => ac.Visible(false))
                    .Add(c => c.MainOwner, ac => ac.Visible(false))
                    .Add(c => c.Comment, ac => ac.Visible(false))
                    .Add(c => c.IsArchived, ac => ac.Visible(false))

                    .Add(c => c.ActRentDate, ac => ac.Visible(false))
                    .Add(c => c.AddonAttributeGroundCategory, ac => ac.Visible(false))
                    .Add(c => c.AddonOKOF, ac => ac.Visible(false))
                    .Add(c => c.AnnualCostAvg, ac => ac.Visible(false))
                    .Add(c => c.Area, ac => ac.Visible(false))
                    .Add(c => c.Benefit, ac => ac.Visible(false))
                    .Add(c => c.BenefitApply, ac => ac.Visible(false))
                    .Add(c => c.BenefitApplyForEnergy, ac => ac.Visible(false))
                    .Add(c => c.BenefitApplyLand, ac => ac.Visible(false))
                    .Add(c => c.BenefitApplyTS, ac => ac.Visible(false))
                    .Add(c => c.BenefitDocsExist, ac => ac.Visible(false))
                    .Add(c => c.BuildingLength, ac => ac.Visible(false))
                    .Add(c => c.Bush, ac => ac.Visible(false))
                    .Add(c => c.BusinessArea, ac => ac.Visible(false))
                    .Add(c => c.CadastralNumber, ac => ac.Visible(false))
                    .Add(c => c.CadastralValue, ac => ac.Visible(false))
                    .Add(c => c.CadRegDate, ac => ac.Visible(false))
                    .Add(c => c.RegNumber, ac => ac.Visible(false))
                    .Add(c => c.RightRegDate, ac => ac.Visible(false))
                    .Add(c => c.RightRegEndDate, ac => ac.Visible(false))
                    .Add(c => c.ConservationDocInfo, ac => ac.Visible(false))
                    .Add(c => c.ConservationReturnInfo, ac => ac.Visible(false))
                    .Add(c => c.ContainmentVolume, ac => ac.Visible(false))
                    .Add(c => c.Contragent, ac => ac.Visible(false))
                    .Add(c => c.CostForSale, ac => ac.Visible(false))
                    .Add(c => c.DecisionsDetails, ac => ac.Visible(false))
                    .Add(c => c.DecisionsDetailsLand, ac => ac.Visible(false))
                    .Add(c => c.DecisionsDetailsTS, ac => ac.Visible(false))
                    .Add(c => c.Deposit, ac => ac.Visible(false))
                    .Add(c => c.DepreciationCost, ac => ac.Visible(false))
                    .Add(c => c.DepreciationCostNU, ac => ac.Visible(false))
                    .Add(c => c.DepreciationGroup, ac => ac.Visible(false))
                    .Add(c => c.DepreciationMethodMSFO, ac => ac.Visible(false))
                    .Add(c => c.DepreciationMethodNU, ac => ac.Visible(false))
                    .Add(c => c.DepreciationMultiplierForNU, ac => ac.Visible(false))
                    .Add(c => c.DieselEngine, ac => ac.Visible(false))
                    .Add(c => c.DivisibleType, ac => ac.Visible(false))
                    .Add(c => c.EcoKlass, ac => ac.Visible(false))
                    .Add(c => c.EncumbranceExist, ac => ac.Visible(false))
                    .Add(c => c.EnergyDocsExist, ac => ac.Visible(false))
                    .Add(c => c.EnergyLabel, ac => ac.Visible(false))
                    .Add(c => c.EngineSize, ac => ac.Visible(false))
                    .Add(c => c.EstateDefinitionType, ac => ac.Visible(false))
                    .Add(c => c.EstateType, ac => ac.Visible(false))
                    .Add(c => c.EstimatedAmount, ac => ac.Visible(false))
                    .Add(c => c.EstimatedAmountWriteOffStart, ac => ac.Visible(false))
                    .Add(c => c.EstimatedAmountWriteOffTerm, ac => ac.Visible(false))
                    .Add(c => c.Explanation, ac => ac.Visible(false))
                    .Add(c => c.FactAddress, ac => ac.Visible(false))
                    .Add(c => c.ForSale, ac => ac.Visible(false))
                    .Add(c => c.GroundCategory, ac => ac.Visible(false))
                    .Add(c => c.IFNS, ac => ac.Visible(false))
                    .Add(c => c.InitialCostMSFO, ac => ac.Visible(false))
                    .Add(c => c.InitialCostNU, ac => ac.Visible(false))
                    .Add(c => c.InOtherSystem, ac => ac.Visible(false))
                    .Add(c => c.InServiceDateMSFO, ac => ac.Visible(false))
                    .Add(c => c.IntangibleAssetType, ac => ac.Visible(false))
                    .Add(c => c.IsCultural, ac => ac.Visible(false))
                    .Add(c => c.IsEnergy, ac => ac.Visible(false))
                    .Add(c => c.IsInvestment, ac => ac.Visible(false))
                    .Add(c => c.IsInvestmentProgramm, ac => ac.Visible(false))
                    .Add(c => c.IsNonCoreAsset, ac => ac.Visible(false))
                    .Add(c => c.IsSocial, ac => ac.Visible(false))
                    .Add(c => c.IXODepreciation, ac => ac.Visible(false))
                    .Add(c => c.IXOPSt, ac => ac.Visible(false))
                    .Add(c => c.LandPurpose, ac => ac.Visible(false))
                    .Add(c => c.LandType, ac => ac.Visible(false))
                    .Add(c => c.LeavingReason, ac => ac.Visible(false))
                    .Add(c => c.LessorSubject, ac => ac.Visible(false))
                    .Add(c => c.LicenseArea, ac => ac.Visible(false))
                    .Add(c => c.Model, ac => ac.Visible(false))
                    .Add(c => c.OwnershipType, ac => ac.Visible(false))
                    .Add(c => c.PermittedByDoc, ac => ac.Visible(false))
                    .Add(c => c.PermittedUseKind, ac => ac.Visible(false))
                    .Add(c => c.PipelineLength, ac => ac.Visible(false))
                    .Add(c => c.Power, ac => ac.Visible(false).Title("Налоговая база. \"Транспортный налог (мощность ТС, валовая вместимость, паспортная стат. тяга, единица ТС)\""))
                    .Add(c => c.ProprietorSubject, ac => ac.Visible(false))
                    .Add(c => c.RedemptionCost, ac => ac.Visible(false))
                    .Add(c => c.RedemptionDate, ac => ac.Visible(false))
                    .Add(c => c.RentContractDate, ac => ac.Visible(false))
                    .Add(c => c.RentContractNumber, ac => ac.Visible(false))
                    .Add(c => c.RentContractNumberSZVD, ac => ac.Visible(false))
                    .Add(c => c.RentTerm, ac => ac.Visible(false))
                    .Add(c => c.RentTypeMSFO, ac => ac.Visible(false))
                    .Add(c => c.RentTypeRSBU, ac => ac.Visible(false))
                    .Add(c => c.ResidualCostEstimate, ac => ac.Visible(false))
                    .Add(c => c.ResidualCostMSFO, ac => ac.Visible(false))
                    .Add(c => c.ResidualCostNU, ac => ac.Visible(false))
                    .Add(c => c.SerialNumber, ac => ac.Visible(false))
                    .Add(c => c.ShareRightDenominator, ac => ac.Visible(false))
                    .Add(c => c.ShareRightNumerator, ac => ac.Visible(false))
                    .Add(c => c.ShipRegNumber, ac => ac.Visible(false))
                    .Add(c => c.SibMeasure, ac => ac.Visible(false).Title("Единицы измерения налоговой базы. Транспортный налог"))
                    .Add(c => c.SPPCode, ac => ac.Visible(false))
                    .Add(c => c.SPPItem, ac => ac.Visible(false))
                    .Add(c => c.StartDateUse, ac => ac.Visible(false))
                    .Add(c => c.StateObjectMSFO, ac => ac.Visible(false))
                    .Add(c => c.TaxBase, ac => ac.Visible(false))
                    .Add(c => c.TaxBaseEstate, ac => ac.Visible(false))
                    .Add(c => c.TaxCadastralIncludeDate, ac => ac.Visible(false))
                    .Add(c => c.TaxCadastralIncludeDoc, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionEndDate, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionEndDateEstate, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionEndDateEstateLand, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionEndDateEstateTS, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionEndDateLand, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionEndDateTS, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionReason, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionReasonEstate, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionReasonEstateLand, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionReasonEstateTS, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionReasonLand, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionReasonTS, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionStartDate, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionStartDateEstate, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionStartDateEstateLand, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionStartDateEstateTS, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionStartDateLand, ac => ac.Visible(false))
                    .Add(c => c.TaxExemptionStartDateTS, ac => ac.Visible(false))
                    .Add(c => c.TaxFreeLand, ac => ac.Visible(false))
                    .Add(c => c.TaxFreeTS, ac => ac.Visible(false))
                    .Add(c => c.TaxLower, ac => ac.Visible(false))
                    .Add(c => c.TaxLowerLand, ac => ac.Visible(false))
                    .Add(c => c.TaxLowerPercent, ac => ac.Visible(false))
                    .Add(c => c.TaxLowerPercentLand, ac => ac.Visible(false))
                    .Add(c => c.TaxLowerPercentTS, ac => ac.Visible(false))
                    .Add(c => c.TaxLowerTS, ac => ac.Visible(false))
                    .Add(c => c.TaxRateLower, ac => ac.Visible(false))
                    .Add(c => c.TaxRateLowerLand, ac => ac.Visible(false))
                    .Add(c => c.TaxRateLowerTS, ac => ac.Visible(false))
                    .Add(c => c.TaxRateType, ac => ac.Visible(false))
                    .Add(c => c.TaxRateValue, ac => ac.Visible(false))
                    .Add(c => c.TaxRateValueLand, ac => ac.Visible(false))
                    .Add(c => c.TaxRateValueTS, ac => ac.Visible(false))
                    .Add(c => c.TaxRateWithExemption, ac => ac.Visible(false))
                    .Add(c => c.TaxRateWithExemptionEstate, ac => ac.Visible(false))
                    .Add(c => c.TaxRateWithExemptionEstateLand, ac => ac.Visible(false))
                    .Add(c => c.TaxRateWithExemptionEstateTS, ac => ac.Visible(false))
                    .Add(c => c.TaxRateWithExemptionLand, ac => ac.Visible(false))
                    .Add(c => c.TaxRateWithExemptionTS, ac => ac.Visible(false))
                    .Add(c => c.TaxVehicleKindCode, ac => ac.Visible(false))
                    .Add(c => c.TransferRight, ac => ac.Visible(false))
                    .Add(c => c.UsefulEndMSFO, ac => ac.Visible(false))
                    .Add(c => c.UsefulEndNU, ac => ac.Visible(false))
                    .Add(c => c.UsefulForMSFO, ac => ac.Visible(false))
                    .Add(c => c.UsefulForNU, ac => ac.Visible(false))
                    .Add(c => c.VehicleAverageCost, ac => ac.Visible(false))
                    .Add(c => c.VehicleCategory, ac => ac.Visible(false))
                    .Add(c => c.VehicleClass, ac => ac.Visible(false))
                    .Add(c => c.VehicleDeRegDate, ac => ac.Visible(false))
                    .Add(c => c.VehicleLabel, ac => ac.Visible(false))
                    .Add(c => c.VehicleRegDate, ac => ac.Visible(false))
                    .Add(c => c.VehicleRegNumber, ac => ac.Visible(false))
                    .Add(c => c.VehicleTaxFactor, ac => ac.Visible(false))
                    .Add(c => c.VehicleMarketCost, ac => ac.Visible(false))
                    .Add(c => c.Well, ac => ac.Visible(false))
                    .Add(c => c.WellCategory, ac => ac.Visible(false))
                    .Add(c => c.Year, ac => ac.Visible(false))
                    .Add(c => c.YearOfIssue, ac => ac.Visible(false))
                    .Add(c => c.OutOfBalance, ac => ac.Visible(false))
                    .Add(c => c.ContractNumber, ac => ac.Visible(false))
                    .Add(c => c.ContractDate, ac => ac.Visible(false))
                    .Add(c => c.PrimaryDocNumber, ac => ac.Visible(false))
                    .Add(c => c.PrimaryDocDate, ac => ac.Visible(false))

                ));
        }

        /// <summary>
        /// Изменяет значения наименований свойств ОБУ.
        /// </summary>
        public static ViewModelConfigBuilder<AccountingObject> ChangePropNames(this ViewModelConfigBuilder<AccountingObject> conf)
        {
            var props = new Dictionary<string, string>()
            {
                    { nameof(AccountingObject.AccountLedgerLUS), "Счет главной книги ЛУС" },
                    { nameof(AccountingObject.AccountNumber), "Номер счета" },
                    { nameof(AccountingObject.AccumulatedDepreciationRSBU), "Амортизация (накопленная) по РСБУ, руб." },
                    { nameof(AccountingObject.ActRentDate), "Дата получения объекта аренды (дата акта приемки-передачи)"},
                    { nameof(AccountingObject.AddonAttributeGroundCategory), "Доп. признак категории земель"},
                    { nameof(AccountingObject.AddonOKOF), "Доп. Код ОКОФ"},
                    { nameof(AccountingObject.Address), "Адрес"},
                    { nameof(AccountingObject.AnnualCostAvg), "Среднегодовая стоимость имущества"},
                    { nameof(AccountingObject.Area), "Площадь объекта недвижимости, кв.м."},
                    { nameof(AccountingObject.Benefit), "Льготируемый объект"},
                    { nameof(AccountingObject.BenefitApply), "Применение льготы. Налог на имущество "},
                    { nameof(AccountingObject.BenefitApplyForEnergy), "Применение льготы для энегроэффективного оборудования"},
                    { nameof(AccountingObject.BenefitApplyLand), "Применение льготы. Земельный налог"},
                    { nameof(AccountingObject.BenefitApplyTS), "Применение льготы. Транспортный налог"},
                    { nameof(AccountingObject.BenefitDocsExist), "Наличие документов, подтверждающих  применение льготы"},
                    { nameof(AccountingObject.BuildingLength), "Длина линейного сооружения, м."},
                    { nameof(AccountingObject.Bush), "№ куста "},
                    { nameof(AccountingObject.BusinessArea), "Бизнес-сфера"},
                    { nameof(AccountingObject.CadastralNumber), "Кадастровый номер"},
                    { nameof(AccountingObject.CadastralValue), "Кадастровая стоимость, руб."},
                    { nameof(AccountingObject.CadRegDate), "Дата постановки на государственный кадастровый учет"},
                    { nameof(AccountingObject.ConservationDocInfo), "Дата и № документа о переводе объекта на консервацию"},
                    { nameof(AccountingObject.ConservationReturnInfo), "Дата и № документа о возвращении объекта из консервации"},
                    { nameof(AccountingObject.Consolidation), "БЕ"},
                    { nameof(AccountingObject.Contragent), "Поставщик"},
                    { nameof(AccountingObject.Comment), "Комментарий"},
                    { nameof(AccountingObject.CostForSale), "Предполагаемые затраты на продажу"},
                    { nameof(AccountingObject.DecisionsDetails), "Реквизиты решения органа субъектов/муниципальных образований по налогу на имущество"},
                    { nameof(AccountingObject.DecisionsDetailsLand), "Реквизиты решения органа субъектов/муниципальных образований  по земельному налогу"},
                    { nameof(AccountingObject.DecisionsDetailsTS), "Реквизиты решения органа субъектов/муниципальных образований  по транспортному налогу"},
                    { nameof(AccountingObject.Department), "Подразделение"},
                    { nameof(AccountingObject.Deposit), "Месторождение (номер)"},
                    { nameof(AccountingObject.DepreciationCostNU), "Амортизация (накопленная) по НУ, руб."},
                    { nameof(AccountingObject.DepreciationGroup), "Амортизационная группа НУ"},
                    { nameof(AccountingObject.DepreciationMethodMSFO), "Метод амортизации (МСФО)"},
                    { nameof(AccountingObject.DepreciationMethodNU), "Метод амортизации (НУ)"},
                    { nameof(AccountingObject.DepreciationMethodRSBU), "Метод амортизации (РСБУ)"},
                    { nameof(AccountingObject.DepreciationMultiplierForNU), "Коэффициент ускоренной амортизации для НУ"},
                    { nameof(AccountingObject.DieselEngine), "Дизельный двигатель"},
                    { nameof(AccountingObject.DivisibleType), "Отделимое/неотделимое имущество"},
                    { nameof(AccountingObject.EcoKlass), "Экологический класс"},
                    { nameof(AccountingObject.EnergyDocsExist), "Наличие документов, подтверждающих энергоэффективность оборудования"},
                    { nameof(AccountingObject.EnergyLabel), "Класс энергетической эффективности "},
                    { nameof(AccountingObject.EngineSize), "Объем двигателя, л."},
                    { nameof(AccountingObject.EstateDefinitionType), "Тип Объекта имущества"},
                    { nameof(AccountingObject.EstateMovableNSI), "Признак движимое/недвижимое имущество по данным БУ"},
                    { nameof(AccountingObject.EstimatedAmount), "Сумма оценочного обязательства в стоимости ОС"},
                    { nameof(AccountingObject.EstimatedAmountWriteOffStart), "Дата начала списания оценочного обязательства в стоимости ОС"},
                    { nameof(AccountingObject.EstimatedAmountWriteOffTerm), "Срок списания оценочного обязательства в стоимости ОС"},
                    { nameof(AccountingObject.EUSINumber), "Номер ЕУСИ"},
                    { nameof(AccountingObject.Explanation), "Пояснение к операции"},
                    { nameof(AccountingObject.ExternalID), "Уникальный номер ОС в учетной системе"},
                    { nameof(AccountingObject.FactAddress), "Фактическое местонахождение объекта"},
                    { nameof(AccountingObject.ForSale), "Признак имущества, предназначенного для продажи"},
                    { nameof(AccountingObject.GroundCategory), "Код категории земель"},
                    { nameof(AccountingObject.IFNS), "Код ИФНС"},
                    { nameof(AccountingObject.InitialCost), "Первоначальная стоимость объекта по БУ, руб."},
                    { nameof(AccountingObject.InitialCostMSFO), "Первоначальная стоимость объекта по МСФО, руб."},
                    { nameof(AccountingObject.InitialCostNU), "Первоначальная стоимость объекта по НУ, руб. "},
                    { nameof(AccountingObject.InOtherSystem), "Учет в системе взимания платы «ПЛАТОН»"},
                    { nameof(AccountingObject.InServiceDate), "Дата ввода в эксплуатацию"},
                    { nameof(AccountingObject.InServiceDateMSFO), "Дата ввода в эксплуатацию МСФО"},
                    { nameof(AccountingObject.IntangibleAssetType), "Вид НМА"},
                    { nameof(AccountingObject.InventoryNumber), "Инвентарный номер"},
                    { nameof(AccountingObject.IsCultural), "Отнесение к категории памятников истории и культуры"},
                    { nameof(AccountingObject.IsEnergy), "Энергоэффективное оборудование"},
                    { nameof(AccountingObject.IsInvestment), "Признак инвестиционного имущества"},
                    { nameof(AccountingObject.IsInvestmentProgramm), "Имущество, созданное по инвестиционной программе (в соответствии с программой развития регионов)"},
                    { nameof(AccountingObject.IsSocial), "Признак объекта социально-культурного или бытового назначения"},                   
                    { nameof(AccountingObject.IXODepreciation), "Код показателя ИКСО Амортизация"},
                    { nameof(AccountingObject.IXOPSt), "Код показателя ИКСО Первоначальная стоимость"},
                    { nameof(AccountingObject.LandPurpose), "Назначение ЗУ"},
                    { nameof(AccountingObject.LandType), "Тип ЗУ"},
                    { nameof(AccountingObject.LeavingDate), "Дата выбытия"},
                    { nameof(AccountingObject.LeavingReason), "Причина выбытия"},
                    { nameof(AccountingObject.LessorSubject), "Арендатор (Лизингополучатель) / Пользователь по договору"},
                    { nameof(AccountingObject.LicenseArea), "Лицензионный участок"},
                    { nameof(AccountingObject.Model), "Марка ТС"},
                    { nameof(AccountingObject.MOL), "МОЛ"},
                    { nameof(AccountingObject.Name), "Наименование объекта (в соответствии с учетной системой)"},
                    { nameof(AccountingObject.NameByDoc), "Наименование объекта (в соответствии с документами)"},
                    { nameof(AccountingObject.NameEUSI), "Наименование ЕУСИ"},
                    { nameof(AccountingObject.OKOF2014), "ОКОФ"},
                    { nameof(AccountingObject.OKTMO), "Код ОКТМО"},
                    { nameof(AccountingObject.PermittedUseKind), "Вид разрешенного использования"},
                    { nameof(AccountingObject.PipelineLength), "Длина трубопровода, м."},
                    { nameof(AccountingObject.PositionConsolidation), "Позиция консолидации"},
                    { nameof(AccountingObject.Power), "Мощность ТС"},
                    { nameof(AccountingObject.ProprietorSubject), "Арендодатель (Лизингодатель) / Собственник по договору*"},
                    { nameof(AccountingObject.ReceiptReason), "Способ поступления"},
                    { nameof(AccountingObject.RedemptionCost), "Выкупная стоимость объекта аренды (в валюте договора)"},
                    { nameof(AccountingObject.RedemptionDate), "Предполагаемая дата выкупа объекта аренды"},
                    { nameof(AccountingObject.Region), "Субъект РФ/Регион"},
                    { nameof(AccountingObject.RentContractDate), "Дата договора аренды"},
                    { nameof(AccountingObject.RentContractNumber), "Номер договора аренды"},
                    { nameof(AccountingObject.RentContractNumberSZVD), "Номер договора аренды СЦВД"},
                    { nameof(AccountingObject.RentTerm), "Предполагаемый срок аренды с учетом дальнейшей пролонгации и бизнес-плана (дата окончания аренды)"},
                    { nameof(AccountingObject.RentTypeMSFO), "Тип аренды МСФО"},
                    { nameof(AccountingObject.RentTypeRSBU), "Тип аренды РСБУ"},
                    { nameof(AccountingObject.ResidualCost), "Остаточная стоимость объекта по БУ, руб."},
                    { nameof(AccountingObject.ResidualCostEstimate), "Остаточная стоимость оценочного обязательства, руб."},
                    { nameof(AccountingObject.ResidualCostMSFO), "Остаточная стоимость объекта по МСФО, руб."},
                    { nameof(AccountingObject.ResidualCostNU), "Остаточная стоимость объекта по НУ, руб."},
                    { nameof(AccountingObject.RegNumber), "Номер записи гос. регистрации"},
                    { nameof(AccountingObject.RightRegDate), "Дата гос. регистрации права"},
                    { nameof(AccountingObject.RightRegEndDate), "Дата гос. регистрации прекращения права"},
                    { nameof(AccountingObject.SerialNumber), "Серийный номер/Заводской номер/ВИН"},
                    { nameof(AccountingObject.ShareRightDenominator), "Доля в праве (знаменатель доли)"},
                    { nameof(AccountingObject.ShareRightNumerator), "Доля в праве (числитель доли)"},
                    { nameof(AccountingObject.ShipRegDate), "Дата включения в Российский международный реестр судов"},
                    { nameof(AccountingObject.ShipRegNumber), "Номер в Российском международном реестре судов"},
                    { nameof(AccountingObject.SibCityNSI), "Город/Населенный пункт"},
                    { nameof(AccountingObject.SibCountry), "Страна"},
                    { nameof(AccountingObject.SibFederalDistrict), "Федеральный округ РФ"},
                    { nameof(AccountingObject.SibMeasure), "Единицы измерения налоговой базы. Транспортный налог"},
                    { nameof(AccountingObject.SPPCode), "Код БИП"},
                    { nameof(AccountingObject.SPPItem), "СПП элемент"},
                    { nameof(AccountingObject.StartDateUse), "Дата начала использования (НКС)"},
                    { nameof(AccountingObject.StateObjectMSFO), "Состояние объекта МСФО"},
                    { nameof(AccountingObject.StateObjectRSBU), "Состояние объекта РСБУ "},
                    { nameof(AccountingObject.SubNumber), "Субномер"},
                    { nameof(AccountingObject.TaxBase), "Выбор базы налогообложения"},
                    { nameof(AccountingObject.TaxBaseEstate), "Выбор базы налогообложения (ЕУСИ)"},
                    { nameof(AccountingObject.TaxCadastralIncludeDate), "Дата включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения"},
                    { nameof(AccountingObject.TaxCadastralIncludeDoc), "Номер документа включения в перечень объектов, учитываемых по кадастровой стоимости для целей налогообложения"},
                    { nameof(AccountingObject.TaxExemptionEndDate), "Дата окончания действия льготных условий налогообложения. Налог на имущество"},
                    { nameof(AccountingObject.TaxExemptionEndDateEstate), "Дата окончания действия льготных условий налогообложения. Налог на имущество (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionEndDateEstateLand), "Дата окончания действия льготных условий налогообложения. Земельный налог (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionEndDateEstateTS), "Дата окончания действия льготных условий налогообложения. Транспортный налог (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionEndDateLand), "Дата окончания действия льготных условий налогообложения. Земельный налог"},
                    { nameof(AccountingObject.TaxExemptionEndDateTS), "Дата окончания действия льготных условий налогообложения. Транспортный налог"},
                    { nameof(AccountingObject.TaxExemptionReason), "Причина налоговой льготы. Налог на имущество"},
                    { nameof(AccountingObject.TaxExemptionReasonEstate), "Причина налоговой льготы. Налог на имущество (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionReasonEstateLand), "Причина налоговой льготы. Земельный налог (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionReasonEstateTS), "Причина налоговой льготы. Транспортный налог (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionReasonLand), "Причина налоговой льготы. Земельный налог"},
                    { nameof(AccountingObject.TaxExemptionReasonTS), "Причина налоговой льготы. Транспортный налог"},
                    { nameof(AccountingObject.TaxExemptionStartDate), "Дата начала действия льготных условий налогообложения. Налог на имущество"},
                    { nameof(AccountingObject.TaxExemptionStartDateEstate), "Дата начала действия льготных условий налогообложения. Налог на имущество (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionStartDateEstateLand), "Дата начала действия льготных условий налогообложения. Земельный налог (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionStartDateEstateTS), "Дата начала действия льготных условий налогообложения. Транспортный налог (ЕУСИ)"},
                    { nameof(AccountingObject.TaxExemptionStartDateLand), "Дата начала действия льготных условий налогообложения. Земельный налог"},
                    { nameof(AccountingObject.TaxExemptionStartDateTS), "Дата начала действия льготных условий налогообложения. Транспортный налог"},
                    { nameof(AccountingObject.TaxFreeLand), "Код налоговой льготы в виде освобождения от налогообложения. Земельный налог"},
                    { nameof(AccountingObject.TaxFreeTS), "Код налоговой льготы в виде освобождения от налогообложения. Транспортный налог"},
                    { nameof(AccountingObject.TaxLowerLand), "Код налоговой льготы в виде уменьшения суммы налога. Земельный налог"},
                    { nameof(AccountingObject.TaxLowerPercent), "Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот). Налог на имущество"},
                    { nameof(AccountingObject.TaxLowerPercentLand), "Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот). Налог на землю"},
                    { nameof(AccountingObject.TaxLowerPercentTS), "Процент, уменьшающий исчисленную сумму налога на имущество (указанный в законе субъекта РФ о предоставлении льгот). Транспортный налог"},
                    { nameof(AccountingObject.TaxLowerTS), "Код налоговой льготы в виде уменьшения суммы налога. Транспортный налог"},
                    { nameof(AccountingObject.TaxRateLower), "Код налоговой льготы в виде понижения налоговой ставки. Налог на имущество"},
                    { nameof(AccountingObject.TaxRateLowerLand), "Код налоговой льготы в виде снижения налоговой ставки. Земельный налог"},
                    { nameof(AccountingObject.TaxRateLowerTS), "Код налоговой льготы в виде снижения налоговой ставки. Транспортный налог"},
                    { nameof(AccountingObject.TaxRateType), "Наименование налога"},
                    { nameof(AccountingObject.TaxRateValue), "Налоговая ставка в соответствии с локальным учетом. Налог на имущество"},
                    { nameof(AccountingObject.TaxRateValueLand), "Налоговая ставка в соответствии с локальным учетом. Земельный налог"},
                    { nameof(AccountingObject.TaxRateValueTS), "Налоговая ставка в соответствии с локальным учетом. Транспортный налог"},
                    { nameof(AccountingObject.TaxRateWithExemption), "Налоговая ставка с учетом применяемых льгот, %. Налог на имущество"},
                    { nameof(AccountingObject.TaxRateWithExemptionEstate), "Налоговая ставка с учетом применяемых льгот, %. Налог на имущество (ЕУСИ)"},
                    { nameof(AccountingObject.TaxRateWithExemptionEstateLand), "Налоговая ставка с учетом применяемых льгот, %. Земельный налог (ЕУСИ)"},
                    { nameof(AccountingObject.TaxRateWithExemptionEstateTS), "Налоговая ставка с учетом применяемых льгот, % (ЕУСИ). Налог на имущество (ЕУСИ)"},
                    { nameof(AccountingObject.TaxRateWithExemptionLand), "Налоговая ставка с учетом применяемых льгот, %. Земельный налог"},
                    { nameof(AccountingObject.TaxRateWithExemptionTS), "Налоговая ставка с учетом применяемых льгот, %. Транспортный налог"},
                    { nameof(AccountingObject.TaxVehicleKindCode), "Код вида ТС"},
                    { nameof(AccountingObject.TransferRight), "Предусмотрена ли договором передача права собственности (да /нет)"},
                    { nameof(AccountingObject.Useful), "СПИ по РСБУ, мес."},
                    { nameof(AccountingObject.UsefulEnd), "Оставшийся срок службы по бухгалтерскому учету (месяцев)"},
                    { nameof(AccountingObject.UsefulEndMSFO), "Оставшийся срок службы по МСФО (месяцев)"},
                    { nameof(AccountingObject.UsefulEndNU), "Оставшийся срок службы по налоговому учету (месяцев) "},
                    { nameof(AccountingObject.UsefulForMSFO), "СПИ по МСФО, мес."},
                    { nameof(AccountingObject.UsefulForNU), "СПИ по НУ, мес."},
                    { nameof(AccountingObject.UsesKind), "Разрешенное использование"},
                    { nameof(AccountingObject.VehicleAverageCost), "Средняя стоимость ТС, руб."},
                    { nameof(AccountingObject.VehicleCategory), "Категория ТС"},
                    { nameof(AccountingObject.VehicleClass), "Единый классификатор транспортных средств"},
                    { nameof(AccountingObject.VehicleDeRegDate), "Дата снятия с учета ТС в государственных органах"},
                    { nameof(AccountingObject.VehicleLabel), "Класс ТС"},
                    { nameof(AccountingObject.VehicleMarketCost), "Рыночная стоимость, руб."},
                    { nameof(AccountingObject.VehicleRegDate), "Дата регистрации ТС в государственных органах"},
                    { nameof(AccountingObject.VehicleRegNumber), "Номер госрегистрации ТС"},
                    { nameof(AccountingObject.VehicleTaxFactor), "Повышающий коэффициент расчета транспортного налога"},
                    { nameof(AccountingObject.Well), "№ скважины "},
                    { nameof(AccountingObject.WellCategory), "Категория скважины"},
                    { nameof(AccountingObject.Year), "Год постройки"},
                    { nameof(AccountingObject.YearOfIssue), "Год выпуска ТС"},
            };

            foreach (var ed in conf.Config.DetailView.Editors)
            {
                if (!String.IsNullOrEmpty(ed.PropertyName) && props.Any(f => f.Key == ed.PropertyName))
                    ed.Title = props[ed.PropertyName];
            }

            foreach (var col in conf.Config.ListView.Columns)
            {
                if (!String.IsNullOrEmpty(col.PropertyName) && props.Any(f => f.Key == col.PropertyName))
                    col.Title = props[col.PropertyName];
            }

            return conf;
        }
    }
}