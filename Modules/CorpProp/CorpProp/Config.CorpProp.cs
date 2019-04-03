using Base.DAL;
using Base.DAL.EF;
using CorpProp.Entities.Access;
using CorpProp.Model.Partial.Entities.SeparLand;
using CorpProp.Entities.Common;

namespace CorpProp
{
    public partial class CorpPropConfig
    {
        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            InitEstates<TContext>(config);
            config.Context(EFRepositoryFactory<TContext>.Instance)

            #region CorpProp

            #region CorpProp.Entities.Accounting

                        .Entity<CorpProp.Entities.Accounting.AccountingEstateRightView>()
                        .Entity<CorpProp.Entities.Accounting.AccountingObjectTbl>()
                        .Entity<CorpProp.Entities.Accounting.AccountingObject>(
                         ss => ss.Save(xx => xx
                        .SaveOneObject(x => x.AccountingStatus)
                        .SaveOneObject(x => x.AddonAttributeGroundCategory)
                        .SaveOneObject(x => x.AddonOKOF)
                        .SaveOneObject(x => x.AircraftKind)
                        .SaveOneObject(x => x.AircraftType)
                        .SaveOneObject(x => x.BusinessArea)
                        .SaveOneObject(x => x.ClassFixedAsset)
                        .SaveOneObject(x => x.Consolidation)
                        .SaveOneObject(x => x.Contragent)
                        .SaveOneObject(x => x.DeadWeightUnit)
                        .SaveOneObject(x => x.DecisionsDetails)
                        .SaveOneObject(x => x.DecisionsDetailsLand)
                        .SaveOneObject(x => x.DecisionsDetailsTS)
                        .SaveOneObject(x => x.Deposit)
                        .SaveOneObject(x => x.DepreciationGroup)
                        .SaveOneObject(x => x.DepreciationMethodMSFO)
                        .SaveOneObject(x => x.DepreciationMethodNU)
                        .SaveOneObject(x => x.DepreciationMethodRSBU)
                        .SaveOneObject(x => x.DivisibleType)
                        .SaveOneObject(x => x.DraughtHardUnit)
                        .SaveOneObject(x => x.DraughtLightUnit)
                        .SaveOneObject(x => x.EcoKlass)
                        .SaveOneObject(x => x.ENAOF)
                        .SaveOneObject(x => x.EnergyLabel)
                        .SaveOneObject(x => x.EstateDefinitionType)
                        .SaveOneObject(x => x.Estate)
                        .SaveOneObject(x => x.EstateMovableNSI)
                        .SaveOneObject(x => x.EstateType)
                        .SaveOneObject(x => x.GroundCategory)
                        .SaveOneObject(x => x.IntangibleAssetType)
                        .SaveOneObject(x => x.LandPurpose)
                        .SaveOneObject(x => x.LandType)
                        .SaveOneObject(x => x.LayingType)
                        .SaveOneObject(x => x.LeavingReason)
                        .SaveOneObject(x => x.LengthUnit)
                        .SaveOneObject(x => x.LessorSubject)
                        .SaveOneObject(x => x.LicenseArea)
                        .SaveOneObject(x => x.MainOwner)
                        .SaveOneObject(x => x.MostHeightUnit)
                        .SaveOneObject(x => x.OKATO)
                        .SaveOneObject(x => x.OKATORegion)
                        .SaveOneObject(x => x.OKOF2014)
                        .SaveOneObject(x => x.OKOF94)
                        .SaveOneObject(x => x.OKTMO)
                        .SaveOneObject(x => x.OKTMORegion)
                        .SaveOneObject(x => x.Owner)
                        .SaveOneObject(x => x.OwnershipType)
                        .SaveOneObject(x => x.PermittedUseKind)
                        .SaveOneObject(x => x.PositionConsolidation)
                        .SaveOneObject(x => x.PowerUnit)
                        .SaveOneObject(x => x.ProprietorSubject)
                        .SaveOneObject(x => x.ReceiptReason)
                        .SaveOneObject(x => x.Region)
                        .SaveOneObject(x => x.RentTypeMSFO)
                        .SaveOneObject(x => x.RentTypeRSBU)
                        .SaveOneObject(x => x.RightKind)
                        .SaveOneObject(x => x.RSBUAccountNumber)
                        .SaveOneObject(x => x.ShipClass)
                        .SaveOneObject(x => x.ShipType)
                        .SaveOneObject(x => x.SibCityNSI)
                        .SaveOneObject(x => x.SibCountry)
                        .SaveOneObject(x => x.SibFederalDistrict)
                        .SaveOneObject(x => x.SibMeasure)
                        .SaveOneObject(x => x.SSR)
                        .SaveOneObject(x => x.SSRTerminate)
                        .SaveOneObject(x => x.StateObjectMSFO)
                        .SaveOneObject(x => x.StateObjectRSBU)
                        .SaveOneObject(x => x.TaxBaseEstate)
                        .SaveOneObject(x => x.TaxBase)
                        .SaveOneObject(x => x.TaxExemption)
                        .SaveOneObject(x => x.TaxFreeLand)
                        .SaveOneObject(x => x.TaxFreeTS)
                        .SaveOneObject(x => x.TaxLower)
                        .SaveOneObject(x => x.TaxLowerLand)
                        .SaveOneObject(x => x.TaxLowerTS)
                        .SaveOneObject(x => x.TaxRateLower)
                        .SaveOneObject(x => x.TaxRateLowerLand)
                        .SaveOneObject(x => x.TaxRateLowerTS)
                        .SaveOneObject(x => x.TaxRateType)
                        .SaveOneObject(x => x.TaxVehicleKindCode)
                        .SaveOneObject(x => x.VehicleCategory)
                        .SaveOneObject(x => x.VehicleClass)
                        .SaveOneObject(x => x.VehicleLabel)
                        .SaveOneObject(x => x.Model)
                        .SaveOneObject(x => x.VehicleTenureType)
                        .SaveOneObject(x => x.VehicleType)
                        .SaveOneObject(x => x.WellCategory)
                        .SaveOneObject(x => x.WhoUse)
                        .SaveOneObject(x => x.WidthUnit)
                    )
                )
                        .Entity<CorpProp.Entities.Accounting.BIK>()

            #endregion CorpProp.Entities.Accounting

            #region CorpProp.Entities.Asset

                .Entity<CorpProp.Entities.Asset.NonCoreAssetTbl>()
                .Entity<CorpProp.Entities.Asset.NonCoreAsset>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetList>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetAndListTbl>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetAndList>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetSale>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetSaleAccept>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetSaleOffer>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetStatus>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetInventory>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetInventoryType>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetOwnerCategory>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetListState>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetSaleStatus>()
                .Entity<CorpProp.Entities.Asset.NonCoreAssetInventoryState>()

            #endregion CorpProp.Entities.Asset

            #region CorpProp.Entities.Base

                    .Entity<CorpProp.Entities.Base.DictObject>()

            #endregion CorpProp.Entities.Base

            #region CorpProp.Entities.CorporateGovernance

                        .Entity<CorpProp.Entities.CorporateGovernance.Appraisal>(e => e.Save(s => s.SaveOneObject(x => x.SibRegion)
                                                                                                    .SaveOneObject(x => x.Customer)
                                                                                                    .SaveOneObject(x => x.Owner)
                                                                                                    .SaveOneObject(x => x.Appraiser)
                                                                                                    .SaveOneObject(x => x.Deal)

                                                                                ))
                        .Entity<CorpProp.Entities.CorporateGovernance.AppraisalType>()

                        .Entity<CorpProp.Entities.CorporateGovernance.EstateAppraisal>(e => e.Save(s => s.SaveOneObject(x => x.Appraisal)
                                                                                                         .SaveOneObject(x => x.AppraisalType)
                                                                                                         .SaveOneObject(x => x.EstateAppraisalType)
                                                                                       ))
                        .Entity<CorpProp.Entities.CorporateGovernance.IndicateEstateAppraisalView>()
                        .Entity<CorpProp.Entities.CorporateGovernance.Investment>()
                        .Entity<CorpProp.Entities.CorporateGovernance.InvestmentType>()
                        .Entity<CorpProp.Entities.CorporateGovernance.Predecessor>()
                        .Entity<CorpProp.Entities.CorporateGovernance.Shareholder>()
                        .Entity<CorpProp.Entities.CorporateGovernance.SuccessionType>()
                        .Entity<CorpProp.Entities.CorporateGovernance.AppraiserDataFinYear>()
                        .Entity<CorpProp.Entities.CorporateGovernance.AppraisalOrgData>()

            #endregion CorpProp.Entities.CorporateGovernance

            #region CorpProp.Entities.Document

                        .Entity<CorpProp.Entities.Document.CardFolder>()
                        .Entity<CorpProp.Entities.Document.FileCard>(builder => builder.Save(saver => saver.SaveOneObject(card => card.FileCardPermission)
                                                                                                           .SaveOneObject(card => card.FileCardType)))
                        .Entity<CorpProp.Entities.Document.FileCardMany>(builder => builder.Save(saver => saver.SaveOneObject(card => card.PersonFullName)
                                                                                                               .SaveOneObject(card => card.FileCardPermission)
                                                                                                               .SaveOneObject(card => card.FileCardType)))
                        .Entity<CorpProp.Entities.Document.FileCardOne>(builder => builder.Save(saver => saver.SaveOneObject(one => one.FileData)
                                                                                                              .SaveOneObject(card => card.PersonFullName)
                                                                                                              .SaveOneObject(card => card.FileCardType)
                                                                                                              .SaveOneObject(card => card.FileCardPermission)))
                        .Entity<CorpProp.Entities.Document.FileCardPermission>()
                        .Entity<CorpProp.Entities.Document.ViewSettingsByMnemonic>()
                        .Entity<CorpProp.Entities.Document.FileCardType>()
                        .Entity<CorpProp.Entities.Document.FileDB>()

            #endregion CorpProp.Entities.Document

            #region CorpProp.Entities.DocumentFlow

                        .Entity<CorpProp.Entities.DocumentFlow.SibDeal>(builder => builder.Save(saver => saver.SaveOneObject(deal => deal.SourseInformation)
                                                                                                              .SaveOneObject(deal => deal.DocType)
                                                                                                              .SaveOneObject(deal => deal.Currency)
                                                                                                           ))
                        .Entity<CorpProp.Entities.DocumentFlow.SibDealStatus>()
                        .Entity<CorpProp.Entities.DocumentFlow.DealParticipant>()
                        .Entity<CorpProp.Entities.DocumentFlow.Doc>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocKind>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocStatus>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocTask>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocType>()
                        .Entity<CorpProp.Entities.DocumentFlow.DocTypeOperation>()

            #endregion CorpProp.Entities.DocumentFlow

            #region CorpProp.Entities.Export

                        .Entity<CorpProp.Entities.Export.ExportTemplate>(en => en.Save(s => s
                        .SaveOneObject(ss => ss.File)
                        .SaveOneToMany(ss => ss.Items)))
                        .Entity<CorpProp.Entities.Export.ExportTemplateItem>(en => en.Save(s => s.SaveOneObject(ss => ss.ExportTemplate)))

            #endregion CorpProp.Entities.Export

            #region CorpProp.Entities.FIAS

                    .Entity<CorpProp.Entities.FIAS.SibAddress>()
                    .Entity<CorpProp.Entities.FIAS.SibCountry>()
                    .Entity<CorpProp.Entities.FIAS.SibRegion>()

            #endregion CorpProp.Entities.FIAS

                     .Entity<CorpProp.Entities.History.HistoricalSettings>()

            #region Import

                    .Entity<CorpProp.Entities.Import.ImportHistory>(ent => ent.Save(sav => sav
                    .SaveOneObject(x => x.SibUser)
                    .SaveOneObject(x => x.Society)
                    .SaveOneObject(x => x.ImportHistoryState)))
                    .Entity<CorpProp.Entities.Import.ImportErrorLog>()
                    .Entity<CorpProp.Entities.Import.ImportObject>()
                    .Entity<CorpProp.Entities.Import.ImportTemplate>()
                    .Entity<CorpProp.Entities.Import.ImportTemplateItem>()

            #endregion Import

            #region CorpProp.Entities.Law

                        .Entity<CorpProp.Entities.Law.DuplicateRightView>()
                        .Entity<CorpProp.Entities.Law.Encumbrance>()
                        .Entity<CorpProp.Entities.Law.EncumbranceType>()
                        .Entity<CorpProp.Entities.Law.Extract>()
                        .Entity<CorpProp.Entities.Law.ExtractFormat>()
                        .Entity<CorpProp.Entities.Law.ExtractItem>()
                        .Entity<CorpProp.Entities.Law.ExtractType>()
                        .Entity<CorpProp.Entities.Law.IntangibleAssetRight>()
                        .Entity<CorpProp.Entities.Law.RegistrationBasis>()
                        .Entity<CorpProp.Entities.Law.Right>()
                        .Entity<CorpProp.Entities.Law.RightCostView>()
                        .Entity<CorpProp.Entities.Law.RightKind>()
                        .Entity<CorpProp.Entities.Law.RightType>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateRegistration>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateRegistrationRecord>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateRegistrationStatus>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateTerminate>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateTerminateRecord>()
                        .Entity<CorpProp.Entities.Law.ScheduleStateYear>()

            #endregion CorpProp.Entities.Law

            #region CorpProp.Entities.Mapping

                        .Entity<CorpProp.Entities.Mapping.EstateRulesCteation>()
                        .Entity<CorpProp.Entities.Mapping.AccountingEstates>(x => x.Save(saver => saver.SaveOneObject(obj => obj.ClassFixedAsset)))
                        .Entity<CorpProp.Entities.Mapping.Mapping>()
                        .Entity<CorpProp.Entities.Mapping.ExternalMappingSystem>()
                        .Entity<CorpProp.Entities.Mapping.OKOFEstates>(x => x.Save(saver => saver.SaveOneObject(obj => obj.OKOF2014)))
                        .Entity<CorpProp.Entities.Mapping.RosReestrTypeEstate>()

            #endregion CorpProp.Entities.Mapping

            #region CorpProp.Entities.NSI

            #region EUSI

                         .Entity<CorpProp.Entities.NSI.BoostOrReductionFactor>()
                         .Entity<CorpProp.Entities.NSI.DecisionsDetails>()
                         .Entity<CorpProp.Entities.NSI.DecisionsDetailsLand>()
                         .Entity<CorpProp.Entities.NSI.DecisionsDetailsTS>()
                         .Entity<CorpProp.Entities.NSI.Deposit>()
                         .Entity<CorpProp.Entities.NSI.DepreciationMethodRSBU>()
                         .Entity<CorpProp.Entities.NSI.DepreciationMethodMSFO>()
                         .Entity<CorpProp.Entities.NSI.DepreciationMethodNU>()
                         .Entity<CorpProp.Entities.NSI.DivisibleType>()
                         .Entity<CorpProp.Entities.NSI.EcoKlass>()
                         .Entity<CorpProp.Entities.NSI.ENAOF>()
                         .Entity<CorpProp.Entities.NSI.EnergyLabel>()
                         .Entity<CorpProp.Entities.NSI.HighEnergyEfficientFacility>()
                         .Entity<CorpProp.Entities.NSI.HighEnergyEfficientFacilityKP>()
                         .Entity<CorpProp.Entities.NSI.EstateDefinitionType>()
                         .Entity<CorpProp.Entities.NSI.EstateMovableNSI>()
                         .Entity<CorpProp.Entities.NSI.GroupConsolidationMSFO>()
                         .Entity<CorpProp.Entities.NSI.GroupConsolidationRSBU>()
                         .Entity<CorpProp.Entities.NSI.LandPurpose>()
                         .Entity<CorpProp.Entities.NSI.LicenseArea>()
                         .Entity<CorpProp.Entities.NSI.PeriodNU>()
                         .Entity<CorpProp.Entities.NSI.PermittedUseKind>()
                         .Entity<CorpProp.Entities.NSI.PositionConsolidation>()
                         .Entity<CorpProp.Entities.NSI.SubPositionConsolidation>()
                         .Entity<CorpProp.Entities.NSI.RentTypeMSFO>()
                         .Entity<CorpProp.Entities.NSI.RentTypeRSBU>()
                         .Entity<CorpProp.Entities.NSI.SibCityNSI>()
                         .Entity<CorpProp.Entities.NSI.StateObjectMSFO>()
                         .Entity<CorpProp.Entities.NSI.StateObjectRSBU>()
                         .Entity<CorpProp.Entities.NSI.StructurePlan>()
                         .Entity<CorpProp.Entities.NSI.TaxFreeLand>()
                         .Entity<CorpProp.Entities.NSI.TaxFreeTS>()
                         .Entity<CorpProp.Entities.NSI.TaxLower>()
                         .Entity<CorpProp.Entities.NSI.TaxLowerLand>()
                         .Entity<CorpProp.Entities.NSI.TaxLowerTS>()
                         .Entity<CorpProp.Entities.NSI.TaxRateLower>()
                         .Entity<CorpProp.Entities.NSI.TaxRateLowerLand>()
                         .Entity<CorpProp.Entities.NSI.TaxRateLowerTS>()
                         .Entity<CorpProp.Entities.NSI.TaxRateType>()
                         .Entity<CorpProp.Entities.NSI.TermOfPymentTaxRate>()
                         .Entity<CorpProp.Entities.NSI.TermOfPymentTaxRateLand>()
                         .Entity<CorpProp.Entities.NSI.TermOfPymentTaxRateTS>()
                         .Entity<CorpProp.Entities.NSI.VehicleClass>()
                         .Entity<CorpProp.Entities.NSI.VehicleLabel>()
                         .Entity<CorpProp.Entities.NSI.VehicleModel>()

                         .Entity<CorpProp.Entities.NSI.CostKindRentalPayments>()
                         .Entity<CorpProp.Entities.NSI.SubsoilUser>()
                         .Entity<CorpProp.Entities.NSI.ObjectLocationRent>()
                         .Entity<CorpProp.Entities.NSI.AssetHolderRSBU>()
                         .Entity<CorpProp.Entities.NSI.StateObjectRent>()

            #endregion EUSI

                        .Entity<CorpProp.Entities.NSI.AccountingMovingType>()
                        .Entity<CorpProp.Entities.NSI.AccountingStatus>()
                        .Entity<CorpProp.Entities.NSI.AccountingSystem>()
                        .Entity<CorpProp.Entities.NSI.ActualKindActivity>()
                        .Entity<CorpProp.Entities.NSI.AddonAttributeGroundCategory>()
                        .Entity<CorpProp.Entities.NSI.AppraisalGoal>()
                        .Entity<CorpProp.Entities.NSI.AppraisalPurpose>()
                        .Entity<CorpProp.Entities.NSI.AppType>()

                        .Entity<CorpProp.Entities.NSI.BaseExclusionFromPerimeter>()
                        .Entity<CorpProp.Entities.NSI.BaseInclusionInPerimeter>()
                        .Entity<CorpProp.Entities.NSI.BasisForInvestments>()
                        .Entity<CorpProp.Entities.NSI.BusinessBlock>()
                        .Entity<CorpProp.Entities.NSI.BusinessArea>()
                        .Entity<CorpProp.Entities.NSI.BusinessDirection>()
                        .Entity<CorpProp.Entities.NSI.BusinessSegment>()
                        .Entity<CorpProp.Entities.NSI.BusinessUnit>()
                        .Entity<CorpProp.Entities.NSI.ClassFixedAsset>()
                        .Entity<CorpProp.Entities.NSI.Consolidation>(e => e.Save(s => s.SaveOneObject(x => x.TypeAccounting)))
                        .Entity<CorpProp.Entities.NSI.ContourType>()
                        .Entity<CorpProp.Entities.NSI.ContragentKind>()
                        .Entity<CorpProp.Entities.NSI.Currency>()
                        .Entity<CorpProp.Entities.NSI.DealType>()
                        .Entity<CorpProp.Entities.NSI.DepreciationGroup>()

                        .Entity<CorpProp.Entities.NSI.EstateAppraisalType>()
                        .Entity<CorpProp.Entities.NSI.ExchangeRate>(e => e.Save(s => s.SaveOneObject(x => x.Currency)))
                        .Entity<CorpProp.Entities.NSI.FeatureType>()
                        .Entity<CorpProp.Entities.NSI.GroundCategory>()
                        .Entity<CorpProp.Entities.NSI.ImplementationWay>()
                        .Entity<CorpProp.Entities.NSI.ImportHistoryState>()
                        .Entity<CorpProp.Entities.NSI.InformationSource>()
                        .Entity<CorpProp.Entities.NSI.IntangibleAssetRightType>()
                        .Entity<CorpProp.Entities.NSI.IntangibleAssetStatus>()
                        .Entity<CorpProp.Entities.NSI.IntangibleAssetType>()
                        .Entity<CorpProp.Entities.NSI.EstateType>()
                        .Entity<CorpProp.Entities.NSI.HolidaysCalendar>()
                        .Entity<CorpProp.Entities.NSI.LandType>()
                        .Entity<CorpProp.Entities.NSI.LayingType>()
                        .Entity<CorpProp.Entities.NSI.LeavingReason>()
                        .Entity<CorpProp.Entities.NSI.SibLocation>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetAppraisalType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetListKind>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetListType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetSaleAcceptType>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetType>()
                        .Entity<CorpProp.Entities.NSI.NSI>()
                        .Entity<CorpProp.Entities.NSI.NSIType>()
                        .Entity<CorpProp.Entities.NSI.OKATO>()
                        .Entity<CorpProp.Entities.NSI.OKOF94>()
                        .Entity<CorpProp.Entities.NSI.OKOF2014>()
                        .Entity<CorpProp.Entities.NSI.AddonOKOF>(ss => ss.Save(xx => xx.SaveOneObject(x => x.OKOF94)))
                        .Entity<CorpProp.Entities.NSI.OKOFS>()
                        .Entity<CorpProp.Entities.NSI.OKOGU>()
                        .Entity<CorpProp.Entities.NSI.OKOPF>()
                        .Entity<CorpProp.Entities.NSI.OKPO>()
                        .Entity<CorpProp.Entities.NSI.OKTMO>()
                        .Entity<CorpProp.Entities.NSI.OPF>()
                        .Entity<CorpProp.Entities.NSI.OwnershipType>()
                        .Entity<CorpProp.Entities.NSI.ProductionBlock>()
                        .Entity<CorpProp.Entities.NSI.PropertyComplexKind>()
                        .Entity<CorpProp.Entities.NSI.PropertyComplexIOType>()
                        .Entity<CorpProp.Entities.NSI.RealEstateKind>()
                        .Entity<CorpProp.Entities.NSI.ReceiptReason>()
                        .Entity<CorpProp.Entities.NSI.RequestStatus>()
                        .Entity<CorpProp.Entities.NSI.ResponseStatus>()
                        .Entity<CorpProp.Entities.NSI.RealEstatePurpose>()
                        .Entity<CorpProp.Entities.NSI.RightHolderKind>()
                        .Entity<CorpProp.Entities.NSI.RSBU>()
                        .Entity<CorpProp.Entities.NSI.ShipAssignment>()
                        .Entity<CorpProp.Entities.NSI.SibBank>()
                        .Entity<CorpProp.Entities.NSI.SibFederalDistrict>()
                        .Entity<CorpProp.Entities.NSI.SibKBK>()
                        .Entity<CorpProp.Entities.NSI.SibMeasure>()
                        .Entity<CorpProp.Entities.NSI.SibProjectType>()
                        .Entity<CorpProp.Entities.NSI.SignType>()
                        .Entity<CorpProp.Entities.NSI.SibOKVED>()
                        .Entity<CorpProp.Entities.NSI.SocietyCategory1>()
                        .Entity<CorpProp.Entities.NSI.SocietyCategory2>()
                        .Entity<CorpProp.Entities.NSI.SocietyDept>()
                        .Entity<CorpProp.Entities.NSI.SourceInformation>()
                        .Entity<CorpProp.Entities.NSI.SourceInformationType>()
                        .Entity<CorpProp.Entities.NSI.StageOfCompletion>()
                        .Entity<CorpProp.Entities.NSI.StatusConstruction>()
                        .Entity<CorpProp.Entities.NSI.TaxBase>()
                        .Entity<CorpProp.Entities.NSI.TaxNumberInSheet>()
                        .Entity<CorpProp.Entities.NSI.TenureType>()
                        .Entity<CorpProp.Entities.NSI.TypeAccounting>()
                        .Entity<CorpProp.Entities.NSI.TypeData>()
                        .Entity<CorpProp.Entities.NSI.UnitOfCompany>()
                        .Entity<CorpProp.Entities.NSI.VehicleKindCode>()
                        .Entity<CorpProp.Entities.NSI.WellCategory>()
                        .Entity<CorpProp.Entities.NSI.NonCoreAssetListItemState>()
                        .Entity<CorpProp.Entities.NSI.DictObjectState>()
                        .Entity<CorpProp.Entities.NSI.DictObjectStatus>()
                        .Entity<CorpProp.Entities.NSI.ResponseRowState>()
                        .Entity<CorpProp.Entities.NSI.TaxVehicleKindCode>()
                        .Entity<CorpProp.Entities.NSI.TaxName>()
                        .Entity<CorpProp.Entities.NSI.TaxReportPeriod>()
                        .Entity<CorpProp.Entities.NSI.TaxPeriod>()
                        .Entity<CorpProp.Entities.NSI.TaxRate>()
                        .Entity<CorpProp.Entities.NSI.TaxExemption>()
                        .Entity<CorpProp.Entities.NSI.TaxFederalExemption>()
                        .Entity<CorpProp.Entities.NSI.TaxRegionExemption>()
                        .Entity<CorpProp.Entities.NSI.TaxRateLand>()
                        .Entity<CorpProp.Entities.NSI.TaxExemptionLand>()
                        .Entity<CorpProp.Entities.NSI.TaxFederalExemptionLand>()
                        .Entity<CorpProp.Entities.NSI.TaxRegionExemptionLand>()
                        .Entity<CorpProp.Entities.NSI.TaxRateTS>()
                        .Entity<CorpProp.Entities.NSI.TaxExemptionTS>()
                        .Entity<CorpProp.Entities.NSI.TaxFederalExemptionTS>()
                        .Entity<CorpProp.Entities.NSI.TaxRegionExemptionTS>()
                        .Entity<CorpProp.Entities.NSI.TaxDeductionTS>()

            #endregion CorpProp.Entities.NSI

            #region CorpProp.Entities.ProjectActivity

                        .Entity<CorpProp.Entities.ProjectActivity.SibProject>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibProjectStatus>()
                        //.Entity<CorpProp.Entities.ProjectActivity.SibProjectTemplate>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTask>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskReport>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskReportStatus>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskStatus>()
                        //.Entity<CorpProp.Entities.ProjectActivity.SibTaskTemplate>()
                        .Entity<CorpProp.Entities.ProjectActivity.SibTaskGanttDependency>()

            #endregion CorpProp.Entities.ProjectActivity

            #region CorpProp.Entities.Security

                        .Entity<CorpProp.Entities.Security.SibRole>()
                        .Entity<CorpProp.Entities.Security.SibUser>(e => e.Save(s => s.SaveOneObject(x => x.Society)))
                        .Entity<CorpProp.Entities.Security.ObjectPermission>(e => e.Save(s => s
                        .SaveOneObject(x => x.TypePermission)
                        .SaveOneObject(x => x.Role)))

            #endregion CorpProp.Entities.Security

            #region CorpProp.Entities.Subject

                        .Entity<CorpProp.Entities.Subject.Appraiser>()
                        .Entity<CorpProp.Entities.Subject.BankingDetail>()
                        .Entity<CorpProp.Entities.Subject.Society>(e => e.Save(s => s
                .SaveOneObject(x => x.ActualKindActivity)
                .SaveOneObject(x => x.BaseExclusionFromPerimeter)
                .SaveOneObject(x => x.BaseInclusionInPerimeter)
                .SaveOneObject(x => x.BusinessBlock)
                .SaveOneObject(x => x.ConsolidationUnit)
                .SaveOneObject(x => x.Country)
                .SaveOneObject(x => x.Currency)
                .SaveOneObject(x => x.FederalDistrict)
                .SaveOneObject(x => x.OKATO)
                .SaveOneObject(x => x.OKVED)
                .SaveOneObject(x => x.OPF)
                .SaveOneObject(x => x.Region)
                .SaveOneObject(x => x.ResponsableForResponse)
                .SaveOneObject(x => x.SubjectKind)
                .SaveOneObject(x => x.SubjectType)
                .SaveOneObject(x => x.UnitOfCompany)

            ))
                        
                        .Entity<CorpProp.Entities.Subject.SocietyCalculatedField>()
                        .Entity<CorpProp.Entities.Subject.Subject>()
                        .Entity<CorpProp.Entities.Subject.SubjectActivityKind>()
                        .Entity<CorpProp.Entities.Subject.SubjectKind>()
                        .Entity<CorpProp.Entities.Subject.SubjectType>()

            #endregion CorpProp.Entities.Subject

            #region CorpProp.Entities.Settings

                        .Entity<CorpProp.Entities.Settings.SibNotification>()
                        .Entity<CorpProp.Entities.Settings.ExportImportSettings>(entity =>
                        entity.Save(sav => sav.SaveOneObject(s => s.AccountingSystem)))
                        .Entity<CorpProp.Entities.Settings.UserNotification>()
                        .Entity<CorpProp.Entities.Settings.UserNotificationTemplate>(entity =>
                              entity.Save(sav => sav.SaveOneObject(s => s.NotificationGroup)
                                                    .SaveOneObject(s => s.Report)))
                        .Entity<CorpProp.Entities.Settings.NotificationGroup>()

            #endregion CorpProp.Entities.Settings

            #region SibPermission

            .Entity<SibPermission>()

            #endregion SibPermission

            #endregion CorpProp

            #region ManyToMany

                .Entity<CorpProp.Entities.ManyToMany.AccountingObjectAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.CadastralAndExtract>()
                .Entity<CorpProp.Entities.ManyToMany.EstateAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.EstateAndEstateAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndAccountingObject>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndCertificateRight>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndDoc>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndExtract>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndLegalRight>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndNonCoreAsset>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndRequest>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndRequestContent>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndResponse>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateYear>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateRegistrationRecord>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndScheduleStateTerminateRecord>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardOneAndFileCardMany>()
                .Entity<CorpProp.Entities.ManyToMany.FileCardAndSibProject>()
                .Entity<CorpProp.Entities.ManyToMany.IKAndLand>()
                .Entity<CorpProp.Entities.ManyToMany.IntangibleAssetAndSibCountry>()

                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndDeal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndFileCard>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndRight>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskAndSibUser>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndAppraisal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndDeal>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndEstate>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndFileCard>()
                .Entity<CorpProp.Entities.ManyToMany.SibTaskReportAndRight>()

                .Entity<CorpProp.Entities.ManyToMany.RequestAndSociety>()
                .Entity<CorpProp.Entities.ManyToMany.ResponseAndSibUser>(s => s.Save(os => os.SaveOneObject(obj => obj.ObjLeft).SaveOneObject(obj => obj.ObjRigth)))

                .Entity<CorpProp.Entities.Asset.NonCoreAssetListItemAndNonCoreAssetSaleAccept>()

                .Entity<CorpProp.Entities.ManyToMany.SibUserTerritory>()
                .Entity<CorpProp.Entities.ManyToMany.SocietyAgents>()
                .Entity<CorpProp.Entities.ManyToMany.SocietySubsidiaries>()

            #endregion ManyToMany

            #region SeparLand

            .Entity<SeparLand>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.Estate)
                    .SaveOneObject(x => x.Parent)
                ))
            .Entity<SeparLandAccountingDt5>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.Owner)
                    .SaveOneObject(x => x.ClassFixedAsset)
                    .SaveOneObject(x => x.BusinessArea)
                    .SaveOneObject(x => x.WhoUse)
                    .SaveOneObject(x => x.ReceiptReason)
                    .SaveOneObject(x => x.LeavingReason)
                ))
            .Entity<SeparLandAddress9>()
            .Entity<SeparLandCharacteristics2>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.GroundCategory)
                    .SaveOneObject(x => x.RealEstateKind)
                    .SaveOneObject(x => x.LandType)
                ))
            .Entity<SeparLandClassifiers6>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.EstateType)
                    .SaveOneObject(x => x.OKTMORegion)
                    .SaveOneObject(x => x.OKATO)
                    .SaveOneObject(x => x.OKATORegion)
                ))
            .Entity<SeparLandCost7>()
            .Entity<SeparLandGeneralData>()
            .Entity<SeparLandLinks14>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.PropertyComplex)
                    .SaveOneObject(x => x.Land)
                    .SaveOneObject(x => x.Fake)
                ))
            .Entity<SeparLandOtherLinks3>()
            .Entity<SeparLandState8>(s => s.Save(ss => ss
                    .SaveOneObject(x => x.Status)
                ))
            .Entity<SeparLandRight10>()
            .Entity<SeparLandAdditionals>()

            #endregion SeparLand

            ;

            InitRequest<TContext>(config);
        }
    }
}
