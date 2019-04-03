using Base.DAL;
using Base.DAL.EF;
using CorpProp.Entities.NSI;
using EUSI.Entities.Accounting;
using EUSI.Entities.BSC;
using EUSI.Entities.Estate;
using EUSI.Entities.Import;
using EUSI.Entities.ManyToMany;
using EUSI.Entities.Mapping;
using EUSI.Entities.NSI;
using EUSI.Entities.NU;
using EUSI.Entities.Report;

namespace EUSI
{
    public class EUSIConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
                  .Entity<AccountingCalculatedField>(entity => entity.Save(sav => sav
                  .SaveOneObject(s => s.Consolidation)
                  .SaveOneObject(s => s.Declaration)
                  .SaveOneObject(s => s.TaxBase)
                  .SaveOneObject(s => s.TaxReportPeriod)
                  ))
                  .Entity<CalculatingRecord>()
                  .Entity<CalculatingError>()
                  .Entity<AccountingMoving>(builder => builder.Save(saver => saver
                  .SaveOneObject(registration => registration.AccountingObject)
                  .SaveOneObject(registration => registration.Angle)
                  .SaveOneObject(registration => registration.Consolidation)
                  .SaveOneObject(registration => registration.LoadType)
                  .SaveOneObject(registration => registration.MovingType)
                  ))
                  .Entity<BalanceReconciliationReport>()

                  .Entity<AccountingMovingMSFO>(builder => builder.Save(saver => saver
                  .SaveOneObject(registration => registration.AccountingObject)
                  .SaveOneObject(registration => registration.Angle)
                  .SaveOneObject(registration => registration.Consolidation)
                  .SaveOneObject(registration => registration.LoadType)
                  .SaveOneObject(registration => registration.MovingType)

                  .SaveOneObject(registration => registration.DepGroupDebit)
                  .SaveOneObject(registration => registration.DepGroupCredit)
                  .SaveOneObject(registration => registration.BusinessAreaDebit)
                  .SaveOneObject(registration => registration.BusinessAreaCredit)
                  .SaveOneObject(registration => registration.OKOFDebit)
                  .SaveOneObject(registration => registration.OKOFCredit)
                  ))

                  .Entity<BCSData>(builder => builder.Save(saver => saver
                  .SaveOneObject(registration => registration.Consolidation)
                  .SaveOneObject(registration => registration.GroupConsolidation)
                  .SaveOneObject(registration => registration.PositionConsolidation)
                  ))

                  .Entity<Declaration>()
                  .Entity<DeclarationCalcEstate>()
                  .Entity<DeclarationEstate>()
                  .Entity<DeclarationLand>()
                  .Entity<DeclarationVehicle>()
                  .Entity<DeclarationRow>(builder => builder.Save(saver => saver
                  .SaveOneObject(obj => obj.Declaration)
                  ))

                  .Entity<ReportMonitoring>(builder => builder.Save(saver => saver
                  .SaveOneObject(obj => obj.SibUser)
                  .SaveOneObject(obj => obj.Consolidation)
                  .SaveOneObject(obj => obj.ReportMonitoringEventType)
                  .SaveOneObject(obj => obj.ReportMonitoringResult)
                  ))

                  .Entity<EstateRegistration>(builder => builder.Save(saver => saver
                  .SaveOneObject(registration => registration.Society)
                  .SaveOneObject(registration => registration.ERType)
                  .SaveOneObject(registration => registration.ERReceiptReason)
                  .SaveOneObject(registration => registration.Contragent)
                  .SaveOneObject(registration => registration.Consolidation)
                  .SaveOneObject(registration => registration.State)
                  .SaveOneObject(registration => registration.Originator)
                  .SaveOneObject(registration => registration.FileCard)
                  .SaveOneObject(registration => registration.ERControlDateAttributes)
                  .SaveOneObject(registration => registration.ClaimObject)
                  ))
                  .Entity<EstateRegistrationRow>(builder => builder.Save(saver => saver

                  .SaveOneObject(registration => registration.EstateDefinitionType)
                  .SaveOneObject(registration => registration.EstateRegistration)
                  .SaveOneObject(registration => registration.EstateType)
                  .SaveOneObject(registration => registration.IntangibleAssetType)
                  .SaveOneObject(registration => registration.SibCityNSI)
                  .SaveOneObject(registration => registration.SibCountry)
                  .SaveOneObject(registration => registration.SibFederalDistrict)
                  .SaveOneObject(registration => registration.SibRegion)
                  .SaveOneObject(registration => registration.VehicleCategory)
                  .SaveOneObject(registration => registration.VehicleModel)
                  .SaveOneObject(registration => registration.VehiclePowerMeasure)
                  ))
                  .Entity<AddonOKOF2014>(e => e.Save(s => s.SaveOneObject(x => x.OKOF2014)))
                  .Entity<Angle>()
                  .Entity<ERReceiptReason>()
                  .Entity<EstateRegistrationStateNSI>()
                  .Entity<EstateRegistrationTypeNSI>()
                  .Entity<ZoneResponsibility>()
                  .Entity<Responsible>(e => e.Save(s => s
                    .SaveOneObject(x => x.Consolidation)
                    .SaveOneObject(x => x.ZoneResponsibility)
                    ))
                  .Entity<EstateRegistrationOriginator>(e => e.Save(s => s.SaveOneObject(x => x.Consolidation)))

                  .Entity<LoadType>()
                  .Entity<MovingType>()
                  .Entity<OKOFClassNSI>()
                  .Entity<PropertyListTaxBaseCadastral>(e => e.Save(s => s.SaveOneObject(x => x.SibRegion)))
                  .Entity<ReportMonitoringEventType>(e => e.Save(s => s.SaveOneObject(x => x.EventPeriodicity)))
                  .Entity<TransactionKind>()

                  .Entity<ExternalImportLog>(e => e.Save(s => s.SaveOneObject(x => x.Society)))

                  .Entity<FileCardAndAccountingMoving>(entity => entity.Save(s => s.SaveOneObject(x => x.ObjLeft).SaveOneObject(x => x.ObjRigth)))
                  .Entity<FileCardAndEstateRegistrationObject>(entity => entity.Save(s => s.SaveOneObject(x => x.ObjLeft).SaveOneObject(x => x.ObjRigth)))
                  .Entity<EstateAndEstateRegistrationObject>(entity => entity.Save(s => s.SaveOneObject(x => x.ObjLeft).SaveOneObject(x => x.ObjRigth)))
                  .Entity<AccountingObjectAndEstateRegistrationObject>(entity => entity.Save(s => s.SaveOneObject(x => x.ObjLeft).SaveOneObject(x => x.ObjRigth)))
                  .Entity<ConsolidationAndReportMonitoringEventType>(entity => entity.Save(s => s.SaveOneObject(x => x.ObjLeft).SaveOneObject(x => x.ObjRigth)))
                  .Entity<MonitorEventPreceding>(entity => entity.Save(s => s.SaveOneObject(x => x.ObjLeft).SaveOneObject(x => x.ObjRigth)))
                  .Entity<MonitorEventTypeAndResult>(entity => entity.Save(s => s.SaveOneObject(x => x.ObjLeft).SaveOneObject(x => x.ObjRigth)))

                  .Entity<RentalOS>(builder => builder.Save(saver => saver
                  .SaveOneObject(registration => registration.AssetHolderRSBU)
                  .SaveOneObject(registration => registration.Consolidation)
                  .SaveOneObject(registration => registration.CostKindRentalPayments)
                  .SaveOneObject(registration => registration.Deposit)
                  .SaveOneObject(registration => registration.DepreciationGroup)
                  .SaveOneObject(registration => registration.LandPurpose)
                  .SaveOneObject(registration => registration.OKOF2014)
                  .SaveOneObject(registration => registration.ProprietorSubject)
                  .SaveOneObject(registration => registration.StateObjectRent)
                  .SaveOneObject(registration => registration.SubsoilUser)
                  .SaveOneObject(registration => registration.TransactionKind)
                  .SaveOneObject(registration => registration.Currency)
                  ))
                 .Entity<ERControlDateAttributes>()
                 .Entity<HolidayWorkDay>()
                 .Entity<MITDictionary>()
                 .Entity<EngineType>()
                 .Entity<ReportMonitoringResult>()
                 .Entity<Periodicity>()

            #region Mapping

                  .Entity<ERTypeERReceiptReason>(entity => entity.Save(s => s.SaveOneObject(x => x.ERType).SaveOneObject(x => x.ERReceiptReason)))
                  .Entity<EstateTypesMapping>(entity => entity.Save(s => s.SaveOneObject(x => x.EstateDefinitionType).SaveOneObject(x => x.EstateType)));

            #endregion Mapping
        }
    }
}