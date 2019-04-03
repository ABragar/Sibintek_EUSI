using Base.DAL;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.Law;
using CorpProp.Entities.Mapping;
using CorpProp.Entities.NSI;
using CorpProp.Entities.ProjectActivity;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CorpProp.DefaultData
{
    public class FillDataStrategy : IFillDataStrategy<DefaultDataHolder>
    {
        /// <summary>
        /// Создает в репозиториях дефолтные знаечния, прочитанные из десериализованного объекта DefaultDataHolder.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="data">Десериализованный объект, содержащий дефолтные данные.</param>
        public void FillData(IDefaultDataHelper _dataHelper, IUnitOfWork uow, DefaultDataHolder data)
        {
            if (data == null) return;
            try
            {
                //дефолтные статусы проекта
                if (data.SibProjectStatuses != null) _dataHelper.CreateDictItem<SibProjectStatus>(uow, data.SibProjectStatuses);
                //дефолтные статусы задач
                if (data.SibTaskStatuses != null) _dataHelper.CreateDictItem<SibTaskStatus>(uow, data.SibTaskStatuses);
                //дефолтные статусы отчетов по задачам
                if (data.SibTaskReportStatuses != null) _dataHelper.CreateDictItem<SibTaskReportStatus>(uow, data.SibTaskReportStatuses);
                //дефолтные типы ННА
                if (data.NonCoreAssetTypes != null) _dataHelper.CreateDictItem<NonCoreAssetType>(uow, data.NonCoreAssetTypes);
                //дефолтные типы  оценки объектов
                if (data.AppraisalTypes != null) _dataHelper.CreateDictItem<AppraisalType>(uow, data.AppraisalTypes);

                if (data.AppraisalPurposes != null) _dataHelper.CreateDictItem<AppraisalPurpose>(uow, data.AppraisalPurposes);

                if (data.AppraisalGoals != null) _dataHelper.CreateDictItem<AppraisalGoal>(uow, data.AppraisalGoals);

                if (data.NonCoreAssetAppraisalTypes != null) _dataHelper.CreateDictItem<NonCoreAssetAppraisalType>(uow, data.NonCoreAssetAppraisalTypes);

                if (data.ScheduleStateRegistrationStatuses != null) _dataHelper.CreateDictItem<ScheduleStateRegistrationStatus>(uow, data.ScheduleStateRegistrationStatuses);

                if (data.RealEstateKinds != null) _dataHelper.CreateDictItem<RealEstateKind>(uow, data.RealEstateKinds);

                if (data.StatusConstructions != null) _dataHelper.CreateDictItem<StatusConstruction>(uow, data.StatusConstructions);

                if (data.LayingTypes != null) _dataHelper.CreateDictItem<LayingType>(uow, data.LayingTypes);

                if (data.StageOfCompletions != null) _dataHelper.CreateDictItem<StageOfCompletion>(uow, data.StageOfCompletions);

                if (data.ClassFixedAssets != null) _dataHelper.CreateDictItem<ClassFixedAsset>(uow, data.ClassFixedAssets);

                if (data.NonCoreAssetListKinds != null) _dataHelper.CreateDictItem<NonCoreAssetListKind>(uow, data.NonCoreAssetListKinds);

                if (data.NonCoreAssetListTypes != null) _dataHelper.CreateDictItem<NonCoreAssetListType>(uow, data.NonCoreAssetListTypes);

                if (data.ImplementationWays != null) _dataHelper.CreateDictItem<ImplementationWay>(uow, data.ImplementationWays);

                if (data.NonCoreAssetSaleAcceptTypes != null) _dataHelper.CreateDictItem<NonCoreAssetSaleAcceptType>(uow, data.NonCoreAssetSaleAcceptTypes);

                if (data.NonCoreAssetListItemStates != null) _dataHelper.CreateDictItem<NonCoreAssetListItemState>(uow, data.NonCoreAssetListItemStates);

                if (data.NonCoreAssetListStates != null) _dataHelper.CreateDictItem<NonCoreAssetListState>(uow, data.NonCoreAssetListStates);

                if (data.NonCoreAssetInventoryStates != null) _dataHelper.CreateDictItem<NonCoreAssetInventoryState>(uow, data.NonCoreAssetInventoryStates);

                if (data.NonCoreAssetInventoryTypes != null) _dataHelper.CreateDictItem<NonCoreAssetInventoryType>(uow, data.NonCoreAssetInventoryTypes);

                if (data.NonCoreAssetSaleStatuses != null) _dataHelper.CreateDictItem<NonCoreAssetSaleStatus>(uow, data.NonCoreAssetSaleStatuses);

                if (data.RegistrationBasiss != null) _dataHelper.CreateDictItem<RegistrationBasis>(uow, data.RegistrationBasiss);

                if (data.LeavingReasons != null) _dataHelper.CreateDictItem<LeavingReason>(uow, data.LeavingReasons);

                if (data.IntangibleAssetTypes != null) _dataHelper.CreateDictItem<IntangibleAssetType>(uow, data.IntangibleAssetTypes);

                if (data.RightKinds != null) _dataHelper.CreateDictItem<RightKind>(uow, data.RightKinds);

                if (data.EncumbranceTypes != null) _dataHelper.CreateDictItem<EncumbranceType>(uow, data.EncumbranceTypes);

                //if (data.ProductionBlocks != null) _dataHelper.CreateDictItem<ProductionBlock>(uow, data.ProductionBlocks);

                if (data.PropertyComplexKinds != null) _dataHelper.CreateDictItem<PropertyComplexKind>(uow, data.PropertyComplexKinds);

                if (data.ShipTypes != null) _dataHelper.CreateDictItem<ShipType>(uow, data.ShipTypes);

                if (data.ShipClasses != null) _dataHelper.CreateDictItem<ShipClass>(uow, data.ShipClasses);

                if (data.AircraftKinds != null) _dataHelper.CreateDictItem<AircraftKind>(uow, data.AircraftKinds);

                if (data.AircraftTypes != null) _dataHelper.CreateDictItem<AircraftType>(uow, data.AircraftTypes);

                if (data.FeatureTypes != null) _dataHelper.CreateDictItem<FeatureType>(uow, data.FeatureTypes);

                //if (data.SibCountries != null) _dataHelper.CreateDictItem<SibCountry>(uow, data.SibCountries);

                //if (data.SibFederalDistricts != null) _dataHelper.CreateDictItem<SibFederalDistrict>(uow, data.SibFederalDistricts);

                //if (data.SibRegions != null) _dataHelper.CreateDictItem<SibRegion>(uow, data.SibRegions);

                //if (data.SibCityNSIs != null && data.SibCityNSIs.Any())
                //    _dataHelper.CreateDictItem<SibCityNSI>(uow, data.SibCityNSIs);

                if (data.SubjectTypes != null) _dataHelper.CreateDictItem<SubjectType>(uow, data.SubjectTypes);

                if (data.DocKinds != null) _dataHelper.CreateDictItem<DocKind>(uow, data.DocKinds);

                if (data.DocTypeOperations != null) _dataHelper.CreateDictItem<DocTypeOperation>(uow, data.DocTypeOperations);

                if (data.InvestmentTypes != null) _dataHelper.CreateDictItem<InvestmentType>(uow, data.InvestmentTypes);

                if (data.DocStatuses != null) _dataHelper.CreateDictItem<DocStatus>(uow, data.DocStatuses);

                if (data.SibDealStatuses != null) _dataHelper.CreateDictItem<SibDealStatus>(uow, data.SibDealStatuses);

                if (data.ReceiptReasons != null) _dataHelper.CreateDictItem<ReceiptReason>(uow, data.ReceiptReasons);

                if (data.AccountingStatuses != null) _dataHelper.CreateDictItem<AccountingStatus>(uow, data.AccountingStatuses);

                //if (data.GroundCategories != null) _dataHelper.CreateDictItem<GroundCategory>(uow, data.GroundCategories);

                if (data.ExtractFormats != null) _dataHelper.CreateDictItem<ExtractFormat>(uow, data.ExtractFormats);

                if (data.ExtractTypes != null) _dataHelper.CreateDictItem<ExtractType>(uow, data.ExtractTypes);

                if (data.RequestStatuses != null) _dataHelper.CreateDictItem<RequestStatus>(uow, data.RequestStatuses);

                if (data.ResponseStatuses != null) _dataHelper.CreateDictItem<ResponseStatus>(uow, data.ResponseStatuses);
                //if (data.SibMeasures != null) _dataHelper.CreateDictItem<SibMeasure>(uow, data.SibMeasures);

                //if (data.VehicleTypes != null) _dataHelper.CreateDictItem<VehicleType>(uow, data.VehicleTypes);

                //if (data.OPFS != null) _dataHelper.CreateDictItem<OPF>(uow, data.OPFS);

                //if (data.Currencies != null) _dataHelper.CreateDictItem<Currency>(uow, data.Currencies);

                if (data.RightHolderKinds != null) _dataHelper.CreateDictItem<RightHolderKind>(uow, data.RightHolderKinds);

                if (data.SibProjectTypes != null) _dataHelper.CreateDictItem<SibProjectType>(uow, data.SibProjectTypes);

                if (data.ShipAssignments != null) _dataHelper.CreateDictItem<ShipAssignment>(uow, data.ShipAssignments);

                if (data.OwnershipTypes != null) _dataHelper.CreateDictItem<OwnershipType>(uow, data.OwnershipTypes);

                if (data.LandTypes != null) _dataHelper.CreateDictItem<LandType>(uow, data.LandTypes);

                if (data.IntangibleAssetStatuses != null) _dataHelper.CreateDictItem<IntangibleAssetStatus>(uow, data.IntangibleAssetStatuses);

                if (data.EstateAppraisalTypes != null) _dataHelper.CreateDictItem<EstateAppraisalType>(uow, data.EstateAppraisalTypes);

                //if (data.EstateTypes != null) _dataHelper.CreateDictItem<EstateType>(uow, data.EstateTypes);

                if (data.InformationSources != null) _dataHelper.CreateDictItem<InformationSource>(uow, data.InformationSources);

                if (data.BaseExclusionFromPerimeters != null) _dataHelper.CreateDictItem<BaseExclusionFromPerimeter>(uow, data.BaseExclusionFromPerimeters);

                if (data.BaseInclusionInPerimeters != null) _dataHelper.CreateDictItem<BaseInclusionInPerimeter>(uow, data.BaseInclusionInPerimeters);

                //if (data.BusinessAreas != null) _dataHelper.CreateDictItem<BusinessArea>(uow, data.BusinessAreas);

                //if (data.BusinessBlocks != null) _dataHelper.CreateDictItem<BusinessBlock>(uow, data.BusinessBlocks);

                //if (data.BusinessDirections != null) _dataHelper.CreateDictItem<BusinessDirection>(uow, data.BusinessDirections);

                //if (data.BusinessSegments != null) _dataHelper.CreateDictItem<BusinessSegment>(uow, data.BusinessSegments);

                //if (data.BusinessUnits != null && data.BusinessUnits.Any()) _dataHelper.CreateDictItem(uow, data.BusinessUnits);

                if (data.SubjectKinds != null) _dataHelper.CreateDictItem<SubjectKind>(uow, data.SubjectKinds);

                //if (data.Consolidations != null) _dataHelper.CreateDictItem<Consolidation>(uow, data.Consolidations);
                               
                if (data.TypesData != null) _dataHelper.CreateDictItem<TypeData>(uow, data.TypesData);

                if (data.TypesData != null) _dataHelper.CreateDictItem<ResponseRowState>(uow, data.ResponseRowState);

                if (data.EstateRulesCteations != null) _dataHelper.CreateDefaultItem<EstateRulesCteation>(uow, data.EstateRulesCteations);

                if (data.ContragentKinds != null) _dataHelper.CreateDictItem<ContragentKind>(uow, data.ContragentKinds);

                //if (data.UnitOfCompanys != null) _dataHelper.CreateDictItem<UnitOfCompany>(uow, data.UnitOfCompanys);

                if (data.NonCoreAssetStatuses != null) _dataHelper.CreateDictItem<NonCoreAssetStatus>(uow, data.NonCoreAssetStatuses);

                if (data.RosReestrTypeEstates != null) _dataHelper.CreateDefaultItem<RosReestrTypeEstate>(uow, data.RosReestrTypeEstates);

                if (data.AppTypes != null) _dataHelper.CreateDictItem<AppType>(uow, data.AppTypes);

                if (data.SuccessionTypes != null) _dataHelper.CreateDictItem<SuccessionType>(uow, data.SuccessionTypes);

                if (data.NSITypes != null) _dataHelper.CreateDictItem<NSIType>(uow, data.NSITypes);

                if (data.NSIs != null) _dataHelper.CreateNSI(uow, data.NSIs);

                if (data.CardFolders != null) _dataHelper.CreateDefaultItem<CardFolder>(uow, data.CardFolders);

                if (data.HistoricalSettingss != null) _dataHelper.CreateDefaultItem(uow, data.HistoricalSettingss);

                if (data.LandPurposes != null)
                    _dataHelper.CreateDictItem<LandPurpose>(uow, data.LandPurposes);

                if (data.DivisibleTypes != null)
                    _dataHelper.CreateDictItem<DivisibleType>(uow, data.DivisibleTypes);

                //if (data.EnergyLabels != null)
                //    _dataHelper.CreateDictItem<EnergyLabel>(uow, data.EnergyLabels);

                if (data.DepreciationGroups != null)
                    _dataHelper.CreateDictItem<DepreciationGroup>(uow, data.DepreciationGroups);

                if (data.EstateMovableNSI != null) _dataHelper.CreateDictItem<EstateMovableNSI>(uow, data.EstateMovableNSI);

                //if (data.PeriodNUs != null)
                //    _dataHelper.CreateDictItem<PeriodNU>(uow, data.PeriodNUs);

                //if (data.PositionConsolidations != null)
                //    _dataHelper.CreateDictItem<PositionConsolidation>(uow, data.PositionConsolidations);

                //if (data.OKTMOs != null)
                //    _dataHelper.CreateDictItem<OKTMO>(uow, data.OKTMOs);

                //if (data.OKOF2014s != null)
                //    _dataHelper.CreateDictItem<OKOF2014>(uow, data.OKOF2014s);

                //if (data.OKOF94s != null)
                //    _dataHelper.CreateDictItem<OKOF94>(uow, data.OKOF94s);

                //if (data.OKATOs != null)
                //    _dataHelper.CreateDictItem<OKATO>(uow, data.OKATOs);

                if (data.StateObjectRSBUs != null)
                    _dataHelper.CreateDictItem<StateObjectRSBU>(uow, data.StateObjectRSBUs);

                if (data.StateObjectMSFOs != null)
                    _dataHelper.CreateDictItem<StateObjectMSFO>(uow, data.StateObjectMSFOs);

                if (data.RentTypeRSBUs != null)
                    _dataHelper.CreateDictItem<RentTypeRSBU>(uow, data.RentTypeRSBUs);

                if (data.RentTypeMSFOs != null)
                    _dataHelper.CreateDictItem<RentTypeMSFO>(uow, data.RentTypeMSFOs);

                if (data.DepreciationMethodRSBUs != null)
                    _dataHelper.CreateDictItem<DepreciationMethodRSBU>(uow, data.DepreciationMethodRSBUs);

                if (data.DepreciationMethodMSFOs != null)
                    _dataHelper.CreateDictItem<DepreciationMethodMSFO>(uow, data.DepreciationMethodMSFOs);

                if (data.DepreciationMethodNUs != null)
                    _dataHelper.CreateDictItem<DepreciationMethodNU>(uow, data.DepreciationMethodNUs);

                if (data.TypeAccountings != null)
                    _dataHelper.CreateDictItem<TypeAccounting>(uow, data.TypeAccountings);

                if (data.TaxBases != null)
                    _dataHelper.CreateDictItem<TaxBase>(uow, data.TaxBases);

                if (data.TaxRateTypes != null)
                    _dataHelper.CreateDictItem<TaxRateType>(uow, data.TaxRateTypes);

                //if (data.VehicleLabels != null)
                //    _dataHelper.CreateDictItem<VehicleLabel>(uow, data.VehicleLabels);

                //if (data.VehicleModels != null) _dataHelper.CreateDictItem<VehicleModel>(uow, data.VehicleModels);

                if (data.BoostOrReductionFactor != null) { _dataHelper.CreateDictItem(uow, data.BoostOrReductionFactor); }

                //if (data.TaxReportPeriods != null) { _dataHelper.CreateDictItem(uow, data.TaxReportPeriods); }

                //if (data.TaxPeriod != null) { _dataHelper.CreateDictItem(uow, data.TaxPeriod); }

                //if (IsNotNullOrEmpty(data.DecisionsDetailss)) { _dataHelper.CreateDictItem(uow, data.DecisionsDetailss); }

                //if (IsNotNullOrEmpty(data.DecisionsDetailsLands)) { _dataHelper.CreateDictItem(uow, data.DecisionsDetailsLands); }

                //if (IsNotNullOrEmpty(data.DecisionsDetailsTSs)) { _dataHelper.CreateDictItem(uow, data.DecisionsDetailsTSs); }

                //if (IsNotNullOrEmpty(data.TaxRates)) { _dataHelper.CreateDictItem(uow, data.TaxRates); }

                //if (IsNotNullOrEmpty(data.TaxRateLands)) { _dataHelper.CreateDictItem(uow, data.TaxRateLands); }

                //if (IsNotNullOrEmpty(data.TaxRateTSs)) { _dataHelper.CreateDictItem(uow, data.TaxRateTSs); }

                //if (IsNotNullOrEmpty(data.TaxExemptions)) { _dataHelper.CreateDictItem(uow, data.TaxExemptions); }

                //if (IsNotNullOrEmpty(data.TaxFederalExemptions)) { _dataHelper.CreateDictItem(uow, data.TaxFederalExemptions); }

                //if (IsNotNullOrEmpty(data.TaxFederalExemptionLands)) { _dataHelper.CreateDictItem(uow, data.TaxFederalExemptionLands); }

                //if (IsNotNullOrEmpty(data.TaxFederalExemptionTSs)) { _dataHelper.CreateDictItem(uow, data.TaxFederalExemptionTSs); }

                //if (IsNotNullOrEmpty(data.TaxRegionExemptions)) { _dataHelper.CreateDictItem(uow, data.TaxRegionExemptions); }

                //if (IsNotNullOrEmpty(data.TaxRegionExemptionLands)) { _dataHelper.CreateDictItem(uow, data.TaxRegionExemptionLands); }

                //if (IsNotNullOrEmpty(data.TaxRegionExemptionTSs)) { _dataHelper.CreateDictItem(uow, data.TaxRegionExemptionTSs); }

                //if (IsNotNullOrEmpty(data.TaxExemptionLands)) { _dataHelper.CreateDictItem(uow, data.TaxExemptionLands); }

                //if (IsNotNullOrEmpty(data.TaxExemptionTSs)) { _dataHelper.CreateDictItem(uow, data.TaxExemptionTSs); }


                //if (data.TaxVehicleKindCodes != null) { _dataHelper.CreateDictItem(uow, data.TaxVehicleKindCodes); }

                if (data.AccountingSystems != null) { _dataHelper.CreateDictItem(uow, data.AccountingSystems); }
                if (data.ImportHistoryStates != null) { _dataHelper.CreateDictItem(uow, data.ImportHistoryStates); }

                //if (data.Deposits != null) { _dataHelper.CreateDictItem(uow, data.Deposits); }

                //if (data.WellCategorys != null) { _dataHelper.CreateDictItem(uow, data.WellCategorys); }

                //if (data.AddonAttributeGroundCategorys != null) { _dataHelper.CreateDictItem(uow, data.AddonAttributeGroundCategorys); }

                //if (data.TermOfPymentTaxRates != null) { _dataHelper.CreateDictItem(uow, data.TermOfPymentTaxRates); }

                //if (data.TermOfPymentTaxRateLands != null) { _dataHelper.CreateDictItem(uow, data.TermOfPymentTaxRateLands); }

                //if (data.TermOfPymentTaxRateTSs != null) { _dataHelper.CreateDictItem(uow, data.TermOfPymentTaxRateTSs); }

                //if (data.SubsoilUsers != null) { _dataHelper.CreateDictItem(uow, data.SubsoilUsers); }

                //if (data.ObjectLocationRents != null) { _dataHelper.CreateDictItem(uow, data.ObjectLocationRents); }

                //if (data.CostKindRentalPaymentss != null) { _dataHelper.CreateDictItem(uow, data.CostKindRentalPaymentss); }

                //if (data.AssetHolderRSBUs != null) { _dataHelper.CreateDictItem(uow, data.AssetHolderRSBUs); }

                //if (data.StateObjectRents != null) { _dataHelper.CreateDictItem(uow, data.StateObjectRents); }


                uow.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }

        }

        private static bool IsNotNullOrEmpty<T>(List<T> defaultCollection)
        {
            return defaultCollection != null && defaultCollection.Any();
        }

        /// <summary>
        /// Проходится по данным десериализованного объекта и инициирует добавление их в БД.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="data">Десериализованный объект, содержащий дефолтные данные.</param>
        public void FillContext(IDefaultDataHelper _dataHelper, DbContext context, DefaultDataHolder data)
        {
            if (data == null) return;
            try
            {
                if (data.SibProjectStatuses != null)
                    _dataHelper.AddDictObjects<SibProjectStatus>(context, data.SibProjectStatuses);

                if (data.SibTaskStatuses != null)
                    _dataHelper.AddDictObjects<SibTaskStatus>(context, data.SibTaskStatuses);

                if (data.SibTaskReportStatuses != null)
                    _dataHelper.AddDictObjects<SibTaskReportStatus>(context, data.SibTaskReportStatuses);

                context.SaveChanges();
            }
            catch (Exception ex) { System.Diagnostics.Trace.TraceError(ex.ToString()); }

        }
    }
}
