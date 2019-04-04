namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class separateEstateTaxes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("[CorpProp.Estate].InventoryObject", "DecisionsDetailsID", "[CorpProp.NSI].DecisionsDetails");
            DropForeignKey("[CorpProp.Estate].InventoryObject", "DecisionsDetailsLandID", "[CorpProp.NSI].DecisionsDetailsLand");
            DropForeignKey("[CorpProp.Estate].InventoryObject", "DecisionsDetailsTSID", "[CorpProp.NSI].DecisionsDetailsTS");
            DropForeignKey("[CorpProp.Estate].InventoryObject", "EnergyLabelID", "[CorpProp.NSI].EnergyLabel");
            DropForeignKey("[CorpProp.Estate].InventoryObject", "TaxBaseID", "[CorpProp.NSI].TaxBase");
            DropForeignKey("[CorpProp.Estate].InventoryObject", "TaxExemptionID", "[CorpProp.NSI].TaxExemption");
            DropForeignKey("[CorpProp.Estate].InventoryObject", "TaxRateTypeID", "[CorpProp.NSI].TaxRateType");
            DropIndex("[CorpProp.Estate].InventoryObject", new[] { "DecisionsDetailsID" });
            DropIndex("[CorpProp.Estate].InventoryObject", new[] { "DecisionsDetailsLandID" });
            DropIndex("[CorpProp.Estate].InventoryObject", new[] { "DecisionsDetailsTSID" });
            DropIndex("[CorpProp.Estate].InventoryObject", new[] { "EnergyLabelID" });
            DropIndex("[CorpProp.Estate].InventoryObject", new[] { "TaxBaseID" });
            DropIndex("[CorpProp.Estate].InventoryObject", new[] { "TaxExemptionID" });
            DropIndex("[CorpProp.Estate].InventoryObject", new[] { "TaxRateTypeID" });
            CreateTable(
                "[CorpProp.Estate].EstateTaxes",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    TaxesOfID = c.Int(nullable: false),
                    Benefit = c.Boolean(nullable: false),
                    BenefitApplyForEnergy = c.Boolean(nullable: false),
                    BenefitDocsExist = c.Boolean(nullable: false),
                    DecisionsDetailsID = c.Int(),
                    DecisionsDetailsLandID = c.Int(),
                    DecisionsDetailsTSID = c.Int(),
                    EnergyDocsExist = c.Boolean(),
                    EnergyLabelID = c.Int(),
                    IsEnergy = c.Boolean(nullable: false),
                    IsInvestmentProgramm = c.Boolean(nullable: false),
                    TaxBaseID = c.Int(),
                    TaxCadastralIncludeDate = c.DateTime(),
                    TaxCadastralIncludeDoc = c.String(),
                    TaxExemptionEndDate = c.DateTime(),
                    TaxExemptionEndDateLand = c.DateTime(),
                    TaxExemptionEndDateTS = c.DateTime(),
                    TaxExemptionID = c.Int(),
                    TaxExemptionReason = c.String(),
                    TaxExemptionReasonLand = c.String(),
                    TaxExemptionReasonTS = c.String(),
                    TaxExemptionStartDate = c.DateTime(),
                    TaxExemptionStartDateLand = c.DateTime(),
                    TaxExemptionStartDateTS = c.DateTime(),
                    TaxRateTypeID = c.Int(),
                    TaxRateWithExemption = c.Decimal(precision: 18, scale: 2),
                    TaxRateWithExemptionLand = c.Decimal(precision: 18, scale: 2),
                    TaxRateWithExemptionTS = c.Decimal(precision: 18, scale: 2),
                    Hidden = c.Boolean(nullable: false, defaultValue: false),
                    SortOrder = c.Double(nullable: false, defaultValue: -1),
                    RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.NSI].DecisionsDetails", t => t.DecisionsDetailsID)
                .ForeignKey("[CorpProp.NSI].DecisionsDetailsLand", t => t.DecisionsDetailsLandID)
                .ForeignKey("[CorpProp.NSI].DecisionsDetailsTS", t => t.DecisionsDetailsTSID)
                .ForeignKey("[CorpProp.NSI].EnergyLabel", t => t.EnergyLabelID)
                .ForeignKey("[CorpProp.NSI].TaxBase", t => t.TaxBaseID)
                .ForeignKey("[CorpProp.Estate].InventoryObject", t => t.TaxesOfID)
                .ForeignKey("[CorpProp.NSI].TaxExemption", t => t.TaxExemptionID)
                .ForeignKey("[CorpProp.NSI].TaxRateType", t => t.TaxRateTypeID)
                .Index(t => t.TaxesOfID)
                .Index(t => t.DecisionsDetailsID)
                .Index(t => t.DecisionsDetailsLandID)
                .Index(t => t.DecisionsDetailsTSID)
                .Index(t => t.EnergyLabelID)
                .Index(t => t.TaxBaseID)
                .Index(t => t.TaxExemptionID)
                .Index(t => t.TaxRateTypeID);

            MigrateEstateTaxData();
            AlterAccountingObjectView();

            DropColumn("[CorpProp.Estate].InventoryObject", "Benefit");
            DropColumn("[CorpProp.Estate].InventoryObject", "BenefitApplyForEnergy");
            DropColumn("[CorpProp.Estate].InventoryObject", "BenefitDocsExist");
            DropColumn("[CorpProp.Estate].InventoryObject", "DecisionsDetailsID");
            DropColumn("[CorpProp.Estate].InventoryObject", "DecisionsDetailsLandID");
            DropColumn("[CorpProp.Estate].InventoryObject", "DecisionsDetailsTSID");
            DropColumn("[CorpProp.Estate].InventoryObject", "EnergyDocsExist");
            DropColumn("[CorpProp.Estate].InventoryObject", "EnergyLabelID");
            DropColumn("[CorpProp.Estate].InventoryObject", "IsEnergy");
            DropColumn("[CorpProp.Estate].InventoryObject", "IsInvestmentProgramm");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxBaseID");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxCadastralIncludeDate");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxCadastralIncludeDoc");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionEndDate");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionEndDateLand");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionEndDateTS");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionID");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionReason");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionReasonLand");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionReasonTS");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionStartDate");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionStartDateLand");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionStartDateTS");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxRateTypeID");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxRateWithExemption");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxRateWithExemptionLand");
            DropColumn("[CorpProp.Estate].InventoryObject", "TaxRateWithExemptionTS");
        }

        private void MigrateEstateTaxData()
        {
            var migrateEstateTaxDataScript = @"
INSERT INTO [CorpProp.Estate].EstateTaxes (TaxesOfID,
                    Benefit,
                    BenefitApplyForEnergy ,
                    BenefitDocsExist,
                    DecisionsDetailsID ,
                    DecisionsDetailsLandID ,
                    DecisionsDetailsTSID ,
                    EnergyDocsExist,
                    EnergyLabelID ,
                    IsEnergy,
                    IsInvestmentProgramm ,
                    TaxBaseID ,
                    TaxCadastralIncludeDate ,
                    TaxCadastralIncludeDoc,
                    TaxExemptionEndDate ,
                    TaxExemptionEndDateLand ,
                    TaxExemptionEndDateTS ,
                    TaxExemptionID,
                    TaxExemptionReason ,
                    TaxExemptionReasonLand ,
                    TaxExemptionReasonTS ,
                    TaxExemptionStartDate,
                    TaxExemptionStartDateLand,
                    TaxExemptionStartDateTS ,
                    TaxRateTypeID,
                    TaxRateWithExemption,
                    TaxRateWithExemptionLand ,
                    TaxRateWithExemptionTS)

select              ID as TaxesOfID,
                    Benefit,
                    BenefitApplyForEnergy ,
                    BenefitDocsExist,
                    DecisionsDetailsID ,
                    DecisionsDetailsLandID ,
                    DecisionsDetailsTSID ,
                    EnergyDocsExist,
                    EnergyLabelID ,
                    IsEnergy,
                    IsInvestmentProgramm ,
                    TaxBaseID ,
                    TaxCadastralIncludeDate ,
                    TaxCadastralIncludeDoc,
                    TaxExemptionEndDate ,
                    TaxExemptionEndDateLand ,
                    TaxExemptionEndDateTS ,
                    TaxExemptionID,
                    TaxExemptionReason ,
                    TaxExemptionReasonLand ,
                    TaxExemptionReasonTS ,
                    TaxExemptionStartDate,
                    TaxExemptionStartDateLand,
                    TaxExemptionStartDateTS ,
                    TaxRateTypeID,
                    TaxRateWithExemption,
                    TaxRateWithExemptionLand ,
                    TaxRateWithExemptionTS
FROM [CorpProp.Estate].InventoryObject";

            this.Sql(migrateEstateTaxDataScript);
        }

        private void AlterAccountingObjectView()
        {
            var alterScript = @"
if exists (select * from dbo.sysobjects as sysobj
inner join sys.objects as obj on sysobj.id=obj.object_id
left join sys.schemas as objschema on obj.schema_id=objschema.schema_id
where sysobj.xtype = 'V' and sysobj.name = N'AccountingObject' and objschema.name = N'CorpProp.Accounting')
DROP VIEW [CorpProp.Accounting].[AccountingObject];
GO

CREATE VIEW [CorpProp.Accounting].[AccountingObject]
	AS
SELECT
 [AccountingStatusID] = obu.[AccountingStatusID]
, [AccountLedgerLUS] = obu.[AccountLedgerLUS]
, [AccountNumber] = obu.[AccountNumber]
, [AccumulatedDepreciationRSBU] = obu.[AccumulatedDepreciationRSBU]
, [ActRentDate] = obu.[ActRentDate]
, [ActualDate] = obu.[ActualDate]
, [AddonAttributeGroundCategoryID] = land.[AddonAttributeGroundCategoryID]
, [AddonOKOFID] = obu.[AddonOKOFID]
, [Address] = obu.[Address]
, [AircraftAppointment] = obu.[AircraftAppointment]
, [AircraftKindID] = obu.[AircraftKindID]
, [AircraftTypeID] = obu.[AircraftTypeID]
, [AirtcraftLocation] = obu.[AirtcraftLocation]
, [AnnualCostAvg] = obu.[AnnualCostAvg]
, [Area] = cad.[Area]
, [BatchNumber] = obu.[BatchNumber]
, [Benefit] = ISNULL(tax.[Benefit],0)
, [BenefitApply] = obu.[BenefitApply]
, [BenefitApplyForEnergy] = ISNULL(tax.[BenefitApplyForEnergy], 0)
, [BenefitApplyLand] = obu.[BenefitApplyLand]
, [BenefitApplyTS] = obu.[BenefitApplyTS]
, [BenefitDocsExist] = ISNULL(tax.[BenefitDocsExist], 0)
, [BuildingArea] = obu.[BuildingArea]
, [BuildingCadastralNumber] = obu.[BuildingCadastralNumber]
, [BuildingDescription] = obu.[BuildingDescription]
, [BuildingFloor] = obu.[BuildingFloor]
, [BuildingFullArea] = obu.[BuildingFullArea]
, [BuildingLength] = inv.[BuildingLength]
, [BuildingName] = obu.[BuildingName]
, [BuildingUnderground] = obu.[BuildingUnderground]
, [BuildPlace] = obu.[BuildPlace]
, [BuildYear] = obu.[BuildYear]
, [Bush] = cad.[Bush]
, [BusinessAreaID] = obu.[BusinessAreaID]
, [CadastralNumber] = obu.[CadastralNumber]
, [CadastralValue] = cad.[CadastralValue]
, [CadRegDate] = cad.[RegDate]
, [ClassFixedAssetID] = obu.[ClassFixedAssetID]
, [ConservationDocInfo] = obu.[ConservationDocInfo]
, [ConservationFrom] = obu.[ConservationFrom]
, [ConservationReturnInfo] = obu.[ConservationReturnInfo]
, [ConservationTo] = obu.[ConservationTo]
, [ConsolidationID] = obu.[ConsolidationID]
, [ContainmentVolume] = obu.[ContainmentVolume]
, [ContractDate] = obu.[ContractDate]
, [ContractNumber] = obu.[ContractNumber]
, [ContragentID] = obu.[ContragentID]
, [Comment] = obu.[Comment]
, [CostForSale] = obu.[CostForSale]
, [CreateDate] = obu.[CreateDate]
, [DateInclusion] = obu.[DateInclusion]
, [DateOfReceipt] = obu.[DateOfReceipt]
, [DeadWeight] = obu.[DeadWeight]
, [DeadWeightUnitID] = obu.[DeadWeightUnitID]
, [DealProps] = obu.[DealProps]
, [DecisionsDetailsID] = tax.[DecisionsDetailsID]
, [DecisionsDetailsLandID] = tax.[DecisionsDetailsLandID]
, [DecisionsDetailsTSID] = tax.[DecisionsDetailsTSID]
, [Department] = obu.[Department]
, [DepositID] = inv.[DepositID]
, [DepreciationCost] = obu.[DepreciationCost]
, [DepreciationCostNU] = obu.[DepreciationCostNU]
, [DepreciationGroupID] = obu.[DepreciationGroupID]
, [DepreciationMethodMSFOID] = obu.[DepreciationMethodMSFOID]
, [DepreciationMethodNUID] = obu.[DepreciationMethodNUID]
, [DepreciationMethodRSBUID] = obu.[DepreciationMethodRSBUID]
, [DepreciationMultiplierForNU] = obu.[DepreciationMultiplierForNU]
, [DepthWell] = obu.[DepthWell]
, [Description] = obu.[Description]
, [DieselEngine] = obu.[DieselEngine]
, [DivisibleTypeID] = obu.[DivisibleTypeID]
, [DocumentNumber] = obu.[DocumentNumber]
, [DraughtHard] = obu.[DraughtHard]
, [DraughtHardUnitID] = obu.[DraughtHardUnitID]
, [DraughtLight] = obu.[DraughtLight]
, [DraughtLightUnitID] = obu.[DraughtLightUnitID]
, [EcoKlassID] = obu.[EcoKlassID]
, [ENAOFID] = obu.[ENAOFID]
, [EncumbranceExist] = obu.[EncumbranceExist]
, [EndDate] = obu.[EndDate]
, [EnergyDocsExist] = ISNULL(tax.[EnergyDocsExist], 0)
, [EnergyLabelID] = tax.[EnergyLabelID]
, [EngineNumber] = obu.[EngineNumber]
, [EngineSize] = obu.[EngineSize]
, [EstateDefinitionTypeID] = est.[EstateDefinitionTypeID]
, [EstateID] = obu.[EstateID]
, [EstateMovableNSIID] = obu.[EstateMovableNSIID]
, [EstateTypeID] = obu.[EstateTypeID]
, [EstimatedAmount] = obu.[EstimatedAmount]
, [EstimatedAmountWriteOffStart] = obu.[EstimatedAmountWriteOffStart]
, [EstimatedAmountWriteOffTerm] = obu.[EstimatedAmountWriteOffTerm]
, [Explanation] = obu.[Explanation]
, [ExternalID] = obu.[ExternalID]
, [FactAddress] = inv.[FactAddress]
, [ForSale] = obu.[ForSale]
, [GliderNumber] = obu.[GliderNumber]
, [GroundArea] = obu.[GroundArea]
, [GroundCadastralNumber] = obu.[GroundCadastralNumber]
, [GroundCategoryID] = land.[GroundCategoryID]
, [GroundFullArea] = obu.[GroundFullArea]
, [GroundName] = obu.[GroundName]
, [GroundNumber] = obu.[GroundNumber]
, [Harbor] = obu.[Harbor]
, [Height] = obu.[Height]
, [Hidden] = obu.[Hidden]
, [ID] = obu.[ID]
, [IFNS] = obu.[IFNS]
, [ImportDate] = obu.[ImportDate]
, [ImportUpdateDate] = obu.[ImportUpdateDate]
, [InConservation] = obu.[InConservation]
, [InitialCost] = obu.[InitialCost]
, [InitialCostMSFO] = obu.[InitialCostMSFO]
, [InitialCostNU] = obu.[InitialCostNU]
, [InOtherSystem] = obu.[InOtherSystem]
, [InServiceDate] = obu.[InServiceDate]
, [InServiceDateMSFO] = obu.[InServiceDateMSFO]
, [IntangibleAssetTypeID] = nma.[IntangibleAssetTypeID]
, [InventoryNumber] = obu.[InventoryNumber]
, [InventoryNumber2] = obu.[InventoryNumber2]
, [IsCultural] = ISNULL(inv.[IsCultural], 0)
, [IsDispute] = obu.[IsDispute]
, [IsEnergy] = ISNULL(tax.[IsEnergy], 0)
, [IsHistory] = obu.[IsHistory]
, [IsInvestment] = obu.[IsInvestment]
, [IsInvestmentProgramm] = ISNULL(tax.[IsInvestmentProgramm], 0)
, [IsRealEstate] = obu.[IsRealEstate]
, [IsRealEstateImpl] = obu.[IsRealEstateImpl]
, [IsSocial] = ISNULL(inv.[IsSocial], 0)
, [IXODepreciation] = obu.[IXODepreciation]
, [IXOPSt] = obu.[IXOPSt]
, [LandPurposeID] = land.[LandPurposeID]
, [LandTypeID] = land.[LandTypeID]
, [LayingTypeID] = obu.[LayingTypeID]
, [LeavingCost] = obu.[LeavingCost]
, [LeavingDate] = obu.[LeavingDate]
, [LeavingReasonID] = obu.[LeavingReasonID]
, [Length] = obu.[Length]
, [LengthUnitID] = obu.[LengthUnitID]
, [LessorSubjectID] = inv.[LessorSubjectID]
, [LicenseAreaID] = obu.[LicenseAreaID]
, [MainEngineCount] = obu.[MainEngineCount]
, [MainEnginePower] = obu.[MainEnginePower]
, [MainEngineType] = obu.[MainEngineType]
, [MainOwnerID] = obu.[MainOwnerID]
, [MakerName] = obu.[MakerName]
, [MarketCost] = est.[MarketCost]
, [MarketDate] = obu.[MarketDate]
, [Material] = obu.[Material]
, [Model2] = obu.[Model2]
, [MOL] = obu.[MOL]
, [MostHeight] = obu.[MostHeight]
, [MostHeightUnitID] = obu.[MostHeightUnitID]
, [Name] = obu.[Name]
, [NameByDoc] = est.[NameByDoc]
, [NameEUSI] = est.[NameEUSI]
, [NonActualDate] = obu.[NonActualDate]
, [Oid] = obu.[Oid]
, [OKATOID] = obu.[OKATOID]
, [OKATORegionID] = obu.[OKATORegionID]
, [OKOF2014ID] = obu.[OKOF2014ID]
, [OKOF94ID] = obu.[OKOF94ID]
, [OKOFCode] = obu.[OKOFCode]
, [OKOFCode2] = obu.[OKOFCode2]
, [OKOFName] = obu.[OKOFName]
, [OKOFName2] = obu.[OKOFName2]
, [OKTMOCode] = obu.[OKTMOCode]
, [OKTMOID] = obu.[OKTMOID]
, [OKTMOName] = obu.[OKTMOName]
, [OKTMORegionID] = obu.[OKTMORegionID]
, [OldHarbor] = obu.[OldHarbor]
, [OldName] = obu.[OldName]
, [OwnerID] = obu.[OwnerID]
, [OwnershipTypeID] = obu.[OwnershipTypeID]
, [PermittedByDoc] = obu.[PermittedByDoc]
, [PermittedUseKindID] = rr.[PermittedUseKindID]
, [PipelineLength] = inv.[PipelineLength]
, [PositionConsolidationID] = obu.[PositionConsolidationID]
, [Power] = obu.[Power]
, [PowerUnitID] = obu.[PowerUnitID]
, [PrimaryDocNumber] = obu.[PrimaryDocNumber]
, [PrimaryDocDate] = obu.[PrimaryDocDate]
, [ProductionDate] = obu.[ProductionDate]
, [PropertyComplexName] = obu.[PropertyComplexName]
, [ProprietorSubjectID] = inv.[ProprietorSubjectID]
, [PropulsionNumber] = obu.[PropulsionNumber]
, [ReceiptReasonID] = obu.[ReceiptReasonID]
, [RedemptionCost] = obu.[RedemptionCost]
, [RedemptionDate] = obu.[RedemptionDate]
, [RegionID] = obu.[RegionID]
, [RegNumber] = obu.[RegNumber]
, [RentContractDate] = obu.[RentContractDate]
, [RentContractNumber] = obu.[RentContractNumber]
, [RentContractNumberSZVD] = obu.[RentContractNumberSZVD]
, [RentTerm] = obu.[RentTerm]
, [RentTypeMSFOID] = obu.[RentTypeMSFOID]
, [RentTypeRSBUID] = obu.[RentTypeRSBUID]
, [ResidualCost] = obu.[ResidualCost]
, [ResidualCostEstimate] = obu.[ResidualCostEstimate]
, [ResidualCostMSFO] = obu.[ResidualCostMSFO]
, [ResidualCostNU] = obu.[ResidualCostNU]
, [RightAddress] = obu.[RightAddress]
, [RightKindID] = obu.[RightKindID]
, [RightRegDate] = obu.[RightRegDate]
, [RightRegion] = obu.[RightRegion]
, [RowVersion] = obu.[RowVersion]
, [RSBUAccountNumberID] = obu.[RSBUAccountNumberID]
, [SeatingCapacity] = obu.[SeatingCapacity]
, [SerialName] = obu.[SerialName]
, [SerialNumber] = obu.[SerialNumber]
, [ShareRightDenominator] = inv.[ShareRightDenominator]
, [ShareRightNumerator] = inv.[ShareRightNumerator]
, [ShellMaterial] = obu.[ShellMaterial]
, [ShipAppointment] = obu.[ShipAppointment]
, [ShipClassID] = obu.[ShipClassID]
, [ShipName] = obu.[ShipName]
, [ShipRegDate] = obu.[ShipRegDate]
, [ShipRegNumber] = obu.[ShipRegNumber]
, [ShipTypeID] = obu.[ShipTypeID]
, [SibCityNSIID] = obu.[SibCityNSIID]
, [SibCountryID] = obu.[SibCountryID]
, [SibFederalDistrictID] = obu.[SibFederalDistrictID]
, [SibMeasureID] = obu.[SibMeasureID]
, [SignNumber] = obu.[SignNumber]
, [SortOrder] = obu.[SortOrder]
, [SPPCode] = inv.[SPPCode]
, [SPPItem] = obu.[SPPItem]
, [SSRID] = obu.[SSRID]
, [SSRTerminateID] = obu.[SSRTerminateID]
, [StartDate] = obu.[StartDate]
, [StartDateUse] = obu.[StartDateUse]
, [State] = obu.[State]
, [StateChangeDate] = obu.[StateChangeDate]
, [StateObjectMSFOID] = obu.[StateObjectMSFOID]
, [StateObjectRSBUID] = obu.[StateObjectRSBUID]
, [Storage] = obu.[Storage]
, [SubjectCode] = obu.[SubjectCode]
, [SubjectName] = obu.[SubjectName]
, [SubNumber] = obu.[SubNumber]
, [Tax] = obu.[Tax]
, [TaxBaseEstateID] = tax.[TaxBaseID]
, [TaxBaseID] = obu.[TaxBaseID]
, [TaxCadastralIncludeDate] = tax.[TaxCadastralIncludeDate]
, [TaxCadastralIncludeDoc] = tax.[TaxCadastralIncludeDoc]
, [TaxExemptionEndDate] = obu.[TaxExemptionEndDate]
, [TaxExemptionEndDateEstate] = tax.[TaxExemptionEndDate]
, [TaxExemptionEndDateEstateLand] = tax.[TaxExemptionEndDateLand]
, [TaxExemptionEndDateEstateTS] = tax.[TaxExemptionEndDateTS]
, [TaxExemptionEndDateLand] = obu.[TaxExemptionEndDateLand]
, [TaxExemptionEndDateTS] = obu.[TaxExemptionEndDateTS]
, [TaxExemptionID] = obu.[TaxExemptionID]
, [TaxExemptionLandID] = obu.[TaxExemptionLandID]
, [TaxExemptionTSID] = obu.[TaxExemptionTSID]
, [TaxExemptionReason] = obu.[TaxExemptionReason]
, [TaxExemptionReasonEstate] = tax.[TaxExemptionReason]
, [TaxExemptionReasonEstateLand] = tax.[TaxExemptionReasonLand]
, [TaxExemptionReasonEstateTS] = tax.[TaxExemptionReasonTS]
, [TaxExemptionReasonLand] = obu.[TaxExemptionReasonLand]
, [TaxExemptionReasonTS] = obu.[TaxExemptionReasonTS]
, [TaxExemptionStartDate] = obu.[TaxExemptionStartDate]
, [TaxExemptionStartDateEstate] = tax.[TaxExemptionStartDate]
, [TaxExemptionStartDateEstateLand] = tax.[TaxExemptionStartDateLand]
, [TaxExemptionStartDateEstateTS] = tax.[TaxExemptionStartDateTS]
, [TaxExemptionStartDateLand] = obu.[TaxExemptionStartDateLand]
, [TaxExemptionStartDateTS] = obu.[TaxExemptionStartDateTS]
, [TaxFreeLandID] = obu.[TaxFreeLandID]
, [TaxFreeTSID] = obu.[TaxFreeTSID]
, [TaxLowerID] = obu.[TaxLowerID]
, [TaxLowerLandID] = obu.[TaxLowerLandID]
, [TaxLowerPercent] = obu.[TaxLowerPercent]
, [TaxLowerPercentLand] = obu.[TaxLowerPercentLand]
, [TaxLowerPercentTS] = obu.[TaxLowerPercentTS]
, [TaxLowerTSID] = obu.[TaxLowerTSID]
, [TaxRateLowerID] = obu.[TaxRateLowerID]
, [TaxRateLowerLandID] = obu.[TaxRateLowerLandID]
, [TaxRateLowerTSID] = obu.[TaxRateLowerTSID]
, [TaxRateTypeID] = tax.[TaxRateTypeID]
, [TaxRateValue] = obu.[TaxRateValue]
, [TaxRateValueLand] = obu.[TaxRateValueLand]
, [TaxRateValueTS] = obu.[TaxRateValueTS]
, [TaxRateWithExemption] = obu.[TaxRateWithExemption]
, [TaxRateWithExemptionEstate] = tax.[TaxRateWithExemption]
, [TaxRateWithExemptionEstateLand] = tax.[TaxRateWithExemptionLand]
, [TaxRateWithExemptionEstateTS] = tax.[TaxRateWithExemptionTS]
, [TaxRateWithExemptionLand] = obu.[TaxRateWithExemptionLand]
, [TaxRateWithExemptionTS] = obu.[TaxRateWithExemptionTS]
, [TaxVehicleKindCodeID] = veh.[TaxVehicleKindCodeID]
, [TechInspectInterval] = obu.[TechInspectInterval]
, [TransferRight] = obu.[TransferRight]
, [TransferBUSDate] = obu.[TransferBUSDate]
, [UpdateDate] = obu.[UpdateDate]
, [Useful] = obu.[Useful]
, [UsefulEnd] = obu.[UsefulEnd]
, [UsefulEndDate] = obu.[UsefulEndDate]
, [UsefulEndMSFO] = obu.[UsefulEndMSFO]
, [UsefulEndNU] = obu.[UsefulEndNU]
, [UsefulForMSFO] = obu.[UsefulForMSFO]
, [UsefulForNU] = obu.[UsefulForNU]
, [UsesKind] = cad.[UsesKind]
, [VehicleAverageCost] = veh.[AverageCost]
, [VehicleCategoryID] = obu.[VehicleCategoryID]
, [VehicleClassID] = veh.[VehicleClassID]
, [VehicleDeRegDate] = veh.[DeRegDate]
, [VehicleLabelID] = veh.[VehicleLabelID]
, [VehicleMarketCost] = veh.[VehicleMarketCost]
, [VehicleModelID] = obu.[VehicleModelID]
, [VehicleRegDate] = obu.[VehicleRegDate]
, [VehicleRegNumber] = obu.[VehicleRegNumber]
, [VehicleTaxFactor] = obu.[VehicleTaxFactor]
, [VehicleTenureTypeID] = obu.[VehicleTenureTypeID]
, [VehicleTypeID] = obu.[VehicleTypeID]
, [Well] = cad.[Well]
, [WellCategoryID] = cad.[WellCategoryID]
, [WhoUseID] = obu.[WhoUseID]
, [Width] = obu.[Width]
, [WidthUnitID] = obu.[WidthUnitID]
, [Wood] = obu.[Wood]
, [Year] = rr.[YearCommissionings]
, [YearOfIssue] = obu.[YearOfIssue]

FROM [CorpProp.Accounting].[AccountingObjectTbl] obu
LEFT JOIN [CorpProp.Estate].[Estate] est on obu.EstateID = est.ID
LEFT JOIN [CorpProp.Estate].[InventoryObject] inv on obu.EstateID = inv.ID
LEFT JOIN [CorpProp.Estate].[EstateTaxes] tax on inv.ID = tax.TaxesOfID
LEFT JOIN [CorpProp.Estate].[RealEstate] rr on obu.EstateID = rr.ID
LEFT JOIN [CorpProp.Estate].[Cadastral] cad on obu.EstateID = cad.ID
LEFT JOIN [CorpProp.Estate].[Land] land on obu.EstateID = land.ID
LEFT JOIN [CorpProp.Estate].[Vehicle] veh on veh.ID = obu.EstateID
LEFT JOIN [CorpProp.Estate].[IntangibleAsset] nma on nma.ID = obu.EstateID

GO

CREATE TRIGGER [CorpProp.Accounting].[TR_AccountingObject_Ins] ON [CorpProp.Accounting].[AccountingObject]
   INSTEAD OF INSERT
AS
BEGIN
	INSERT INTO [CorpProp.Accounting].[AccountingObjectTbl] (
  [AccountingStatusID]
, [AccountLedgerLUS]
, [AccountNumber]
, [AccumulatedDepreciationRSBU]
, [ActRentDate]
, [ActualDate]
, [AddonAttributeGroundCategoryID]
, [AddonOKOFID]
, [Address]
, [AircraftAppointment]
, [AircraftKindID]
, [AircraftTypeID]
, [AirtcraftLocation]
, [AnnualCostAvg]
, [Area]
, [BatchNumber]
, [Benefit]
, [BenefitApply]
, [BenefitApplyForEnergy]
, [BenefitApplyLand]
, [BenefitApplyTS]
, [BenefitDocsExist]
, [BuildingArea]
, [BuildingCadastralNumber]
, [BuildingDescription]
, [BuildingFloor]
, [BuildingFullArea]
, [BuildingLength]
, [BuildingName]
, [BuildingUnderground]
, [BuildPlace]
, [BuildYear]
, [Bush]
, [BusinessAreaID]
, [CadastralNumber]
, [CadastralValue]
, [CadRegDate]
, [ClassFixedAssetID]
, [ConservationDocInfo]
, [ConservationFrom]
, [ConservationReturnInfo]
, [ConservationTo]
, [ConsolidationID]
, [ContainmentVolume]
, [ContractDate]
, [ContractNumber]
, [ContragentID]
, [Comment]
, [CostForSale]
, [CreateDate]
, [DateInclusion]
, [DateOfReceipt]
, [DeadWeight]
, [DeadWeightUnitID]
, [DealProps]
, [DecisionsDetailsID]
, [DecisionsDetailsLandID]
, [DecisionsDetailsTSID]
, [Department]
, [DepositID]
, [DepreciationCost]
, [DepreciationCostNU]
, [DepreciationGroupID]
, [DepreciationMethodMSFOID]
, [DepreciationMethodNUID]
, [DepreciationMethodRSBUID]
, [DepreciationMultiplierForNU]
, [DepthWell]
, [Description]
, [DieselEngine]
, [DivisibleTypeID]
, [DocumentNumber]
, [DraughtHard]
, [DraughtHardUnitID]
, [DraughtLight]
, [DraughtLightUnitID]
, [EcoKlassID]
, [ENAOFID]
, [EncumbranceExist]
, [EndDate]
, [EnergyDocsExist]
, [EnergyLabelID]
, [EngineNumber]
, [EngineSize]
, [EstateDefinitionTypeID]
, [EstateID]
, [EstateMovableNSIID]
, [EstateTypeID]
, [EstimatedAmount]
, [EstimatedAmountWriteOffStart]
, [EstimatedAmountWriteOffTerm]
, [Explanation]
, [ExternalID]
, [FactAddress]
, [ForSale]
, [GliderNumber]
, [GroundArea]
, [GroundCadastralNumber]
, [GroundCategoryID]
, [GroundFullArea]
, [GroundName]
, [GroundNumber]
, [Harbor]
, [Height]
, [Hidden]
, [IFNS]
, [ImportDate]
, [ImportUpdateDate]
, [InConservation]
, [InitialCost]
, [InitialCostMSFO]
, [InitialCostNU]
, [InOtherSystem]
, [InServiceDate]
, [InServiceDateMSFO]
, [IntangibleAssetTypeID]
, [InventoryNumber]
, [InventoryNumber2]
, [IsCultural]
, [IsDispute]
, [IsEnergy]
, [IsHistory]
, [IsInvestment]
, [IsInvestmentProgramm]
, [IsRealEstate]
, [IsRealEstateImpl]
, [IsSocial]
, [IXODepreciation]
, [IXOPSt]
, [LandPurposeID]
, [LandTypeID]
, [LayingTypeID]
, [LeavingCost]
, [LeavingDate]
, [LeavingReasonID]
, [Length]
, [LengthUnitID]
, [LessorSubjectID]
, [LicenseAreaID]
, [MainEngineCount]
, [MainEnginePower]
, [MainEngineType]
, [MainOwnerID]
, [MakerName]
, [MarketDate]
, [Material]
, [Model2]
, [MOL]
, [MostHeight]
, [MostHeightUnitID]
, [Name]
, [NameByDoc]
, [NameEUSI]
, [NonActualDate]
, [Oid]
, [OKATOID]
, [OKATORegionID]
, [OKOF2014ID]
, [OKOF94ID]
, [OKOFCode]
, [OKOFCode2]
, [OKOFName]
, [OKOFName2]
, [OKTMOCode]
, [OKTMOID]
, [OKTMOName]
, [OKTMORegionID]
, [OldHarbor]
, [OldName]
, [OwnerID]
, [OwnershipTypeID]
, [PermittedByDoc]
, [PermittedUseKindID]
, [PipelineLength]
, [PositionConsolidationID]
, [Power]
, [PowerUnitID]
, [PrimaryDocNumber]
, [PrimaryDocDate]
, [ProductionDate]
, [PropertyComplexName]
, [ProprietorSubjectID]
, [PropulsionNumber]
, [ReceiptReasonID]
, [RedemptionCost]
, [RedemptionDate]
, [RegionID]
, [RegNumber]
, [RentContractDate]
, [RentContractNumber]
, [RentContractNumberSZVD]
, [RentTerm]
, [RentTypeMSFOID]
, [RentTypeRSBUID]
, [ResidualCost]
, [ResidualCostEstimate]
, [ResidualCostMSFO]
, [ResidualCostNU]
, [RightAddress]
, [RightKindID]
, [RightRegDate]
, [RightRegion]
, [RSBUAccountNumberID]
, [SeatingCapacity]
, [SerialName]
, [SerialNumber]
, [ShareRightDenominator]
, [ShareRightNumerator]
, [ShellMaterial]
, [ShipAppointment]
, [ShipClassID]
, [ShipName]
, [ShipRegDate]
, [ShipRegNumber]
, [ShipTypeID]
, [SibCityNSIID]
, [SibCountryID]
, [SibFederalDistrictID]
, [SibMeasureID]
, [SignNumber]
, [SortOrder]
, [SPPCode]
, [SPPItem]
, [SSRID]
, [SSRTerminateID]
, [StartDate]
, [StartDateUse]
, [State]
, [StateChangeDate]
, [StateObjectMSFOID]
, [StateObjectRSBUID]
, [Storage]
, [SubjectCode]
, [SubjectName]
, [SubNumber]
, [Tax]
, [TaxBaseID]
, [TaxCadastralIncludeDate]
, [TaxCadastralIncludeDoc]
, [TaxExemptionEndDate]
, [TaxExemptionEndDateLand]
, [TaxExemptionEndDateTS]
, [TaxExemptionID]
, [TaxExemptionReason]
, [TaxExemptionReasonLand]
, [TaxExemptionReasonTS]
, [TaxExemptionStartDate]
, [TaxExemptionStartDateLand]
, [TaxExemptionStartDateTS]
, [TaxFreeLandID]
, [TaxFreeTSID]
, [TaxLowerID]
, [TaxLowerLandID]
, [TaxLowerPercent]
, [TaxLowerPercentLand]
, [TaxLowerPercentTS]
, [TaxLowerTSID]
, [TaxRateLowerID]
, [TaxRateLowerLandID]
, [TaxRateLowerTSID]
, [TaxRateTypeID]
, [TaxRateValue]
, [TaxRateValueLand]
, [TaxRateValueTS]
, [TaxRateWithExemption]
, [TaxRateWithExemptionLand]
, [TaxRateWithExemptionTS]
, [TaxVehicleKindCodeID]
, [TechInspectInterval]
, [TransferRight]
, [TransferBUSDate]
, [UpdateDate]
, [Useful]
, [UsefulEnd]
, [UsefulEndDate]
, [UsefulEndMSFO]
, [UsefulEndNU]
, [UsefulForMSFO]
, [UsefulForNU]
, [UsesKind]
, [VehicleAverageCost]
, [VehicleCategoryID]
, [VehicleClassID]
, [VehicleDeRegDate]
, [VehicleLabelID]
, [VehicleMarketCost]
, [VehicleModelID]
, [VehicleRegDate]
, [VehicleRegNumber]
, [VehicleTaxFactor]
, [VehicleTenureTypeID]
, [VehicleTypeID]
, [Well]
, [WellCategoryID]
, [WhoUseID]
, [Width]
, [WidthUnitID]
, [Wood]
, [Year]
, [YearOfIssue]

	)
    SELECT
  obu.[AccountingStatusID]
, obu.[AccountLedgerLUS]
, obu.[AccountNumber]
, obu.[AccumulatedDepreciationRSBU]
, obu.[ActRentDate]
, obu.[ActualDate]
, obu.[AddonAttributeGroundCategoryID]
, obu.[AddonOKOFID]
, obu.[Address]
, obu.[AircraftAppointment]
, obu.[AircraftKindID]
, obu.[AircraftTypeID]
, obu.[AirtcraftLocation]
, obu.[AnnualCostAvg]
, obu.[Area]
, obu.[BatchNumber]
, ISNULL(obu.[Benefit] , 0)
, ISNULL(obu.[BenefitApply] , 0)
, ISNULL(obu.[BenefitApplyForEnergy] , 0)
, ISNULL(obu.[BenefitApplyLand] , 0)
, ISNULL(obu.[BenefitApplyTS] , 0)
, ISNULL(obu.[BenefitDocsExist] , 0)
, obu.[BuildingArea]
, obu.[BuildingCadastralNumber]
, obu.[BuildingDescription]
, obu.[BuildingFloor]
, obu.[BuildingFullArea]
, obu.[BuildingLength]
, obu.[BuildingName]
, obu.[BuildingUnderground]
, obu.[BuildPlace]
, obu.[BuildYear]
, obu.[Bush]
, obu.[BusinessAreaID]
, obu.[CadastralNumber]
, obu.[CadastralValue]
, obu.[CadRegDate]
, obu.[ClassFixedAssetID]
, obu.[ConservationDocInfo]
, obu.[ConservationFrom]
, obu.[ConservationReturnInfo]
, obu.[ConservationTo]
, obu.[ConsolidationID]
, obu.[ContainmentVolume]
, obu.[ContractDate]
, obu.[ContractNumber]
, obu.[ContragentID]
, obu.[Comment]
, obu.[CostForSale]
, obu.[CreateDate]
, obu.[DateInclusion]
, obu.[DateOfReceipt]
, obu.[DeadWeight]
, obu.[DeadWeightUnitID]
, obu.[DealProps]
, obu.[DecisionsDetailsID]
, obu.[DecisionsDetailsLandID]
, obu.[DecisionsDetailsTSID]
, obu.[Department]
, obu.[DepositID]
, obu.[DepreciationCost]
, obu.[DepreciationCostNU]
, obu.[DepreciationGroupID]
, obu.[DepreciationMethodMSFOID]
, obu.[DepreciationMethodNUID]
, obu.[DepreciationMethodRSBUID]
, obu.[DepreciationMultiplierForNU]
, obu.[DepthWell]
, obu.[Description]
, ISNULL(obu.[DieselEngine] , 0)
, obu.[DivisibleTypeID]
, obu.[DocumentNumber]
, obu.[DraughtHard]
, obu.[DraughtHardUnitID]
, obu.[DraughtLight]
, obu.[DraughtLightUnitID]
, obu.[EcoKlassID]
, obu.[ENAOFID]
, ISNULL(obu.[EncumbranceExist] , 0)
, obu.[EndDate]
, ISNULL(obu.[EnergyDocsExist] , 0)
, obu.[EnergyLabelID]
, obu.[EngineNumber]
, obu.[EngineSize]
, obu.[EstateDefinitionTypeID]
, obu.[EstateID]
, obu.[EstateMovableNSIID]
, obu.[EstateTypeID]
, obu.[EstimatedAmount]
, obu.[EstimatedAmountWriteOffStart]
, obu.[EstimatedAmountWriteOffTerm]
, obu.[Explanation]
, obu.[ExternalID]
, obu.[FactAddress]
, ISNULL(obu.[ForSale] , 0)
, obu.[GliderNumber]
, obu.[GroundArea]
, obu.[GroundCadastralNumber]
, obu.[GroundCategoryID]
, obu.[GroundFullArea]
, obu.[GroundName]
, obu.[GroundNumber]
, obu.[Harbor]
, obu.[Height]
, obu.[Hidden]
, obu.[IFNS]
, obu.[ImportDate]
, obu.[ImportUpdateDate]
, ISNULL(obu.[InConservation] , 0)
, obu.[InitialCost]
, obu.[InitialCostMSFO]
, obu.[InitialCostNU]
, ISNULL(obu.[InOtherSystem] , 0)
, obu.[InServiceDate]
, obu.[InServiceDateMSFO]
, obu.[IntangibleAssetTypeID]
, obu.[InventoryNumber]
, obu.[InventoryNumber2]
, ISNULL(obu.[IsCultural] , 0)
, ISNULL(obu.[IsDispute] , 0)
, ISNULL(obu.[IsEnergy] , 0)
, ISNULL(obu.[IsHistory] , 0)
, ISNULL(obu.[IsInvestment] , 0)
, ISNULL(obu.[IsInvestmentProgramm] , 0)
, ISNULL(obu.[IsRealEstate] , 0)
, obu.[IsRealEstateImpl]
, ISNULL(obu.[IsSocial] , 0)
, obu.[IXODepreciation]
, obu.[IXOPSt]
, obu.[LandPurposeID]
, obu.[LandTypeID]
, obu.[LayingTypeID]
, obu.[LeavingCost]
, obu.[LeavingDate]
, obu.[LeavingReasonID]
, obu.[Length]
, obu.[LengthUnitID]
, obu.[LessorSubjectID]
, obu.[LicenseAreaID]
, obu.[MainEngineCount]
, obu.[MainEnginePower]
, obu.[MainEngineType]
, obu.[MainOwnerID]
, obu.[MakerName]
, obu.[MarketDate]
, obu.[Material]
, obu.[Model2]
, obu.[MOL]
, obu.[MostHeight]
, obu.[MostHeightUnitID]
, obu.[Name]
, obu.[NameByDoc]
, obu.[NameEUSI]
, obu.[NonActualDate]
, obu.[Oid]
, obu.[OKATOID]
, obu.[OKATORegionID]
, obu.[OKOF2014ID]
, obu.[OKOF94ID]
, obu.[OKOFCode]
, obu.[OKOFCode2]
, obu.[OKOFName]
, obu.[OKOFName2]
, obu.[OKTMOCode]
, obu.[OKTMOID]
, obu.[OKTMOName]
, obu.[OKTMORegionID]
, obu.[OldHarbor]
, obu.[OldName]
, obu.[OwnerID]
, obu.[OwnershipTypeID]
, obu.[PermittedByDoc]
, obu.[PermittedUseKindID]
, obu.[PipelineLength]
, obu.[PositionConsolidationID]
, obu.[Power]
, obu.[PowerUnitID]
, obu.[PrimaryDocNumber]
, obu.[PrimaryDocDate]
, obu.[ProductionDate]
, obu.[PropertyComplexName]
, obu.[ProprietorSubjectID]
, obu.[PropulsionNumber]
, obu.[ReceiptReasonID]
, obu.[RedemptionCost]
, obu.[RedemptionDate]
, obu.[RegionID]
, obu.[RegNumber]
, obu.[RentContractDate]
, obu.[RentContractNumber]
, obu.[RentContractNumberSZVD]
, obu.[RentTerm]
, obu.[RentTypeMSFOID]
, obu.[RentTypeRSBUID]
, obu.[ResidualCost]
, obu.[ResidualCostEstimate]
, obu.[ResidualCostMSFO]
, obu.[ResidualCostNU]
, obu.[RightAddress]
, obu.[RightKindID]
, obu.[RightRegDate]
, obu.[RightRegion]
, obu.[RSBUAccountNumberID]
, obu.[SeatingCapacity]
, obu.[SerialName]
, obu.[SerialNumber]
, obu.[ShareRightDenominator]
, obu.[ShareRightNumerator]
, obu.[ShellMaterial]
, obu.[ShipAppointment]
, obu.[ShipClassID]
, obu.[ShipName]
, obu.[ShipRegDate]
, obu.[ShipRegNumber]
, obu.[ShipTypeID]
, obu.[SibCityNSIID]
, obu.[SibCountryID]
, obu.[SibFederalDistrictID]
, obu.[SibMeasureID]
, obu.[SignNumber]
, obu.[SortOrder]
, obu.[SPPCode]
, obu.[SPPItem]
, obu.[SSRID]
, obu.[SSRTerminateID]
, obu.[StartDate]
, obu.[StartDateUse]
, obu.[State]
, obu.[StateChangeDate]
, obu.[StateObjectMSFOID]
, obu.[StateObjectRSBUID]
, obu.[Storage]
, obu.[SubjectCode]
, obu.[SubjectName]
, obu.[SubNumber]
, ISNULL(obu.[Tax] , 0)
, obu.[TaxBaseID]
, obu.[TaxCadastralIncludeDate]
, obu.[TaxCadastralIncludeDoc]
, obu.[TaxExemptionEndDate]
, obu.[TaxExemptionEndDateLand]
, obu.[TaxExemptionEndDateTS]
, obu.[TaxExemptionID]
, obu.[TaxExemptionReason]
, obu.[TaxExemptionReasonLand]
, obu.[TaxExemptionReasonTS]
, obu.[TaxExemptionStartDate]
, obu.[TaxExemptionStartDateLand]
, obu.[TaxExemptionStartDateTS]
, obu.[TaxFreeLandID]
, obu.[TaxFreeTSID]
, obu.[TaxLowerID]
, obu.[TaxLowerLandID]
, obu.[TaxLowerPercent]
, obu.[TaxLowerPercentLand]
, obu.[TaxLowerPercentTS]
, obu.[TaxLowerTSID]
, obu.[TaxRateLowerID]
, obu.[TaxRateLowerLandID]
, obu.[TaxRateLowerTSID]
, obu.[TaxRateTypeID]
, obu.[TaxRateValue]
, obu.[TaxRateValueLand]
, obu.[TaxRateValueTS]
, obu.[TaxRateWithExemption]
, obu.[TaxRateWithExemptionLand]
, obu.[TaxRateWithExemptionTS]
, obu.[TaxVehicleKindCodeID]
, obu.[TechInspectInterval]
, obu.[TransferRight]
, obu.[TransferBUSDate]
, obu.[UpdateDate]
, obu.[Useful]
, obu.[UsefulEnd]
, obu.[UsefulEndDate]
, obu.[UsefulEndMSFO]
, obu.[UsefulEndNU]
, obu.[UsefulForMSFO]
, obu.[UsefulForNU]
, obu.[UsesKind]
, obu.[VehicleAverageCost]
, obu.[VehicleCategoryID]
, obu.[VehicleClassID]
, obu.[VehicleDeRegDate]
, obu.[VehicleLabelID]
, obu.[VehicleMarketCost]
, obu.[VehicleModelID]
, obu.[VehicleRegDate]
, obu.[VehicleRegNumber]
, obu.[VehicleTaxFactor]
, obu.[VehicleTenureTypeID]
, obu.[VehicleTypeID]
, obu.[Well]
, obu.[WellCategoryID]
, obu.[WhoUseID]
, obu.[Width]
, obu.[WidthUnitID]
, ISNULL(obu.[Wood] , 0)
, obu.[Year]
, obu.[YearOfIssue]

	FROM inserted obu

	SELECT ID
	FROM [CorpProp.Accounting].[AccountingObjectTbl]
	WHERE @@ROWCOUNT > 0 AND ID = scope_identity()

END
GO

CREATE TRIGGER [CorpProp.Accounting].[TR_AccountingObject_Upd] on [CorpProp.Accounting].[AccountingObject]
INSTEAD OF UPDATE
AS
BEGIN

MERGE INTO [CorpProp.Accounting].[AccountingObjectTbl] tbl
   USING ( SELECT * FROM inserted) obu
   ON (tbl.ID = obu.ID)
   WHEN MATCHED THEN
	UPDATE
	SET
  tbl.[AccountingStatusID] = obu.[AccountingStatusID]
, tbl.[AccountLedgerLUS] = obu.[AccountLedgerLUS]
, tbl.[AccountNumber] = obu.[AccountNumber]
, tbl.[AccumulatedDepreciationRSBU] = obu.[AccumulatedDepreciationRSBU]
, tbl.[ActRentDate] = obu.[ActRentDate]
, tbl.[ActualDate] = obu.[ActualDate]
, tbl.[AddonAttributeGroundCategoryID] = obu.[AddonAttributeGroundCategoryID]
, tbl.[AddonOKOFID] = obu.[AddonOKOFID]
, tbl.[Address] = obu.[Address]
, tbl.[AircraftAppointment] = obu.[AircraftAppointment]
, tbl.[AircraftKindID] = obu.[AircraftKindID]
, tbl.[AircraftTypeID] = obu.[AircraftTypeID]
, tbl.[AirtcraftLocation] = obu.[AirtcraftLocation]
, tbl.[AnnualCostAvg] = obu.[AnnualCostAvg]
, tbl.[Area] = obu.[Area]
, tbl.[BatchNumber] = obu.[BatchNumber]
, tbl.[Benefit] = ISNULL(obu.[Benefit] , 0)
, tbl.[BenefitApply] = ISNULL(obu.[BenefitApply] , 0)
, tbl.[BenefitApplyForEnergy] = ISNULL(obu.[BenefitApplyForEnergy] , 0)
, tbl.[BenefitApplyLand] = ISNULL(obu.[BenefitApplyLand] , 0)
, tbl.[BenefitApplyTS] = ISNULL(obu.[BenefitApplyTS] , 0)
, tbl.[BenefitDocsExist] = ISNULL(obu.[BenefitDocsExist] , 0)
, tbl.[BuildingArea] = obu.[BuildingArea]
, tbl.[BuildingCadastralNumber] = obu.[BuildingCadastralNumber]
, tbl.[BuildingDescription] = obu.[BuildingDescription]
, tbl.[BuildingFloor] = obu.[BuildingFloor]
, tbl.[BuildingFullArea] = obu.[BuildingFullArea]
, tbl.[BuildingLength] = obu.[BuildingLength]
, tbl.[BuildingName] = obu.[BuildingName]
, tbl.[BuildingUnderground] = obu.[BuildingUnderground]
, tbl.[BuildPlace] = obu.[BuildPlace]
, tbl.[BuildYear] = obu.[BuildYear]
, tbl.[Bush] = obu.[Bush]
, tbl.[BusinessAreaID] = obu.[BusinessAreaID]
, tbl.[CadastralNumber] = obu.[CadastralNumber]
, tbl.[CadastralValue] = obu.[CadastralValue]
, tbl.[CadRegDate] = obu.[CadRegDate]
, tbl.[ClassFixedAssetID] = obu.[ClassFixedAssetID]
, tbl.[ConservationDocInfo] = obu.[ConservationDocInfo]
, tbl.[ConservationFrom] = obu.[ConservationFrom]
, tbl.[ConservationReturnInfo] = obu.[ConservationReturnInfo]
, tbl.[ConservationTo] = obu.[ConservationTo]
, tbl.[ConsolidationID] = obu.[ConsolidationID]
, tbl.[ContainmentVolume] = obu.[ContainmentVolume]
, tbl.[ContractDate] = obu.[ContractDate]
, tbl.[ContractNumber] = obu.[ContractNumber]
, tbl.[ContragentID] = obu.[ContragentID]
, tbl.[Comment] = obu.[Comment]
, tbl.[CostForSale] = obu.[CostForSale]
, tbl.[CreateDate] = obu.[CreateDate]
, tbl.[DateInclusion] = obu.[DateInclusion]
, tbl.[DateOfReceipt] = obu.[DateOfReceipt]
, tbl.[DeadWeight] = obu.[DeadWeight]
, tbl.[DeadWeightUnitID] = obu.[DeadWeightUnitID]
, tbl.[DealProps] = obu.[DealProps]
, tbl.[DecisionsDetailsID] = obu.[DecisionsDetailsID]
, tbl.[DecisionsDetailsLandID] = obu.[DecisionsDetailsLandID]
, tbl.[DecisionsDetailsTSID] = obu.[DecisionsDetailsTSID]
, tbl.[Department] = obu.[Department]
, tbl.[DepositID] = obu.[DepositID]
, tbl.[DepreciationCost] = obu.[DepreciationCost]
, tbl.[DepreciationCostNU] = obu.[DepreciationCostNU]
, tbl.[DepreciationGroupID] = obu.[DepreciationGroupID]
, tbl.[DepreciationMethodMSFOID] = obu.[DepreciationMethodMSFOID]
, tbl.[DepreciationMethodNUID] = obu.[DepreciationMethodNUID]
, tbl.[DepreciationMethodRSBUID] = obu.[DepreciationMethodRSBUID]
, tbl.[DepreciationMultiplierForNU] = obu.[DepreciationMultiplierForNU]
, tbl.[DepthWell] = obu.[DepthWell]
, tbl.[Description] = obu.[Description]
, tbl.[DieselEngine] = ISNULL(obu.[DieselEngine] , 0)
, tbl.[DivisibleTypeID] = obu.[DivisibleTypeID]
, tbl.[DocumentNumber] = obu.[DocumentNumber]
, tbl.[DraughtHard] = obu.[DraughtHard]
, tbl.[DraughtHardUnitID] = obu.[DraughtHardUnitID]
, tbl.[DraughtLight] = obu.[DraughtLight]
, tbl.[DraughtLightUnitID] = obu.[DraughtLightUnitID]
, tbl.[EcoKlassID] = obu.[EcoKlassID]
, tbl.[ENAOFID] = obu.[ENAOFID]
, tbl.[EncumbranceExist] = ISNULL(obu.[EncumbranceExist] , 0)
, tbl.[EndDate] = obu.[EndDate]
, tbl.[EnergyDocsExist] = ISNULL(obu.[EnergyDocsExist] , 0)
, tbl.[EnergyLabelID] = obu.[EnergyLabelID]
, tbl.[EngineNumber] = obu.[EngineNumber]
, tbl.[EngineSize] = obu.[EngineSize]
, tbl.[EstateDefinitionTypeID] = obu.[EstateDefinitionTypeID]
, tbl.[EstateID] = obu.[EstateID]
, tbl.[EstateMovableNSIID] = obu.[EstateMovableNSIID]
, tbl.[EstateTypeID] = obu.[EstateTypeID]
, tbl.[EstimatedAmount] = obu.[EstimatedAmount]
, tbl.[EstimatedAmountWriteOffStart] = obu.[EstimatedAmountWriteOffStart]
, tbl.[EstimatedAmountWriteOffTerm] = obu.[EstimatedAmountWriteOffTerm]
, tbl.[Explanation] = obu.[Explanation]
, tbl.[ExternalID] = obu.[ExternalID]
, tbl.[FactAddress] = obu.[FactAddress]
, tbl.[ForSale] = ISNULL(obu.[ForSale] , 0)
, tbl.[GliderNumber] = obu.[GliderNumber]
, tbl.[GroundArea] = obu.[GroundArea]
, tbl.[GroundCadastralNumber] = obu.[GroundCadastralNumber]
, tbl.[GroundCategoryID] = obu.[GroundCategoryID]
, tbl.[GroundFullArea] = obu.[GroundFullArea]
, tbl.[GroundName] = obu.[GroundName]
, tbl.[GroundNumber] = obu.[GroundNumber]
, tbl.[Harbor] = obu.[Harbor]
, tbl.[Height] = obu.[Height]
, tbl.[Hidden] = obu.[Hidden]
, tbl.[IFNS] = obu.[IFNS]
, tbl.[ImportDate] = obu.[ImportDate]
, tbl.[ImportUpdateDate] = obu.[ImportUpdateDate]
, tbl.[InConservation] = ISNULL(obu.[InConservation] , 0)
, tbl.[InitialCost] = obu.[InitialCost]
, tbl.[InitialCostMSFO] = obu.[InitialCostMSFO]
, tbl.[InitialCostNU] = obu.[InitialCostNU]
, tbl.[InOtherSystem] = ISNULL(obu.[InOtherSystem] , 0)
, tbl.[InServiceDate] = obu.[InServiceDate]
, tbl.[InServiceDateMSFO] = obu.[InServiceDateMSFO]
, tbl.[IntangibleAssetTypeID] = obu.[IntangibleAssetTypeID]
, tbl.[InventoryNumber] = obu.[InventoryNumber]
, tbl.[InventoryNumber2] = obu.[InventoryNumber2]
, tbl.[IsCultural] = ISNULL(obu.[IsCultural] , 0)
, tbl.[IsDispute] = ISNULL(obu.[IsDispute] , 0)
, tbl.[IsEnergy] = ISNULL(obu.[IsEnergy] , 0)
, tbl.[IsHistory] = ISNULL(obu.[IsHistory] , 0)
, tbl.[IsInvestment] = ISNULL(obu.[IsInvestment] , 0)
, tbl.[IsInvestmentProgramm] = ISNULL(obu.[IsInvestmentProgramm] , 0)
, tbl.[IsRealEstate] = ISNULL(obu.[IsRealEstate] , 0)
, tbl.[IsRealEstateImpl] = obu.[IsRealEstateImpl]
, tbl.[IsSocial] = ISNULL(obu.[IsSocial] , 0)
, tbl.[IXODepreciation] = obu.[IXODepreciation]
, tbl.[IXOPSt] = obu.[IXOPSt]
, tbl.[LandPurposeID] = obu.[LandPurposeID]
, tbl.[LandTypeID] = obu.[LandTypeID]
, tbl.[LayingTypeID] = obu.[LayingTypeID]
, tbl.[LeavingCost] = obu.[LeavingCost]
, tbl.[LeavingDate] = obu.[LeavingDate]
, tbl.[LeavingReasonID] = obu.[LeavingReasonID]
, tbl.[Length] = obu.[Length]
, tbl.[LengthUnitID] = obu.[LengthUnitID]
, tbl.[LessorSubjectID] = obu.[LessorSubjectID]
, tbl.[LicenseAreaID] = obu.[LicenseAreaID]
, tbl.[MainEngineCount] = obu.[MainEngineCount]
, tbl.[MainEnginePower] = obu.[MainEnginePower]
, tbl.[MainEngineType] = obu.[MainEngineType]
, tbl.[MainOwnerID] = obu.[MainOwnerID]
, tbl.[MakerName] = obu.[MakerName]
, tbl.[MarketDate] = obu.[MarketDate]
, tbl.[Material] = obu.[Material]
, tbl.[Model2] = obu.[Model2]
, tbl.[MOL] = obu.[MOL]
, tbl.[MostHeight] = obu.[MostHeight]
, tbl.[MostHeightUnitID] = obu.[MostHeightUnitID]
, tbl.[Name] = obu.[Name]
, tbl.[NameByDoc] = obu.[NameByDoc]
, tbl.[NameEUSI] = obu.[NameEUSI]
, tbl.[NonActualDate] = obu.[NonActualDate]
, tbl.[Oid] = obu.[Oid]
, tbl.[OKATOID] = obu.[OKATOID]
, tbl.[OKATORegionID] = obu.[OKATORegionID]
, tbl.[OKOF2014ID] = obu.[OKOF2014ID]
, tbl.[OKOF94ID] = obu.[OKOF94ID]
, tbl.[OKOFCode] = obu.[OKOFCode]
, tbl.[OKOFCode2] = obu.[OKOFCode2]
, tbl.[OKOFName] = obu.[OKOFName]
, tbl.[OKOFName2] = obu.[OKOFName2]
, tbl.[OKTMOCode] = obu.[OKTMOCode]
, tbl.[OKTMOID] = obu.[OKTMOID]
, tbl.[OKTMOName] = obu.[OKTMOName]
, tbl.[OKTMORegionID] = obu.[OKTMORegionID]
, tbl.[OldHarbor] = obu.[OldHarbor]
, tbl.[OldName] = obu.[OldName]
, tbl.[OwnerID] = obu.[OwnerID]
, tbl.[OwnershipTypeID] = obu.[OwnershipTypeID]
, tbl.[PermittedByDoc] = obu.[PermittedByDoc]
, tbl.[PermittedUseKindID] = obu.[PermittedUseKindID]
, tbl.[PipelineLength] = obu.[PipelineLength]
, tbl.[PositionConsolidationID] = obu.[PositionConsolidationID]
, tbl.[Power] = obu.[Power]
, tbl.[PowerUnitID] = obu.[PowerUnitID]
, tbl.[PrimaryDocNumber] = obu.[PrimaryDocNumber]
, tbl.[PrimaryDocDate] = obu.[PrimaryDocDate]
, tbl.[ProductionDate] = obu.[ProductionDate]
, tbl.[PropertyComplexName] = obu.[PropertyComplexName]
, tbl.[ProprietorSubjectID] = obu.[ProprietorSubjectID]
, tbl.[PropulsionNumber] = obu.[PropulsionNumber]
, tbl.[ReceiptReasonID] = obu.[ReceiptReasonID]
, tbl.[RedemptionCost] = obu.[RedemptionCost]
, tbl.[RedemptionDate] = obu.[RedemptionDate]
, tbl.[RegionID] = obu.[RegionID]
, tbl.[RegNumber] = obu.[RegNumber]
, tbl.[RentContractDate] = obu.[RentContractDate]
, tbl.[RentContractNumber] = obu.[RentContractNumber]
, tbl.[RentContractNumberSZVD] = obu.[RentContractNumberSZVD]
, tbl.[RentTerm] = obu.[RentTerm]
, tbl.[RentTypeMSFOID] = obu.[RentTypeMSFOID]
, tbl.[RentTypeRSBUID] = obu.[RentTypeRSBUID]
, tbl.[ResidualCost] = obu.[ResidualCost]
, tbl.[ResidualCostEstimate] = obu.[ResidualCostEstimate]
, tbl.[ResidualCostMSFO] = obu.[ResidualCostMSFO]
, tbl.[ResidualCostNU] = obu.[ResidualCostNU]
, tbl.[RightAddress] = obu.[RightAddress]
, tbl.[RightKindID] = obu.[RightKindID]
, tbl.[RightRegDate] = obu.[RightRegDate]
, tbl.[RightRegion] = obu.[RightRegion]
, tbl.[RSBUAccountNumberID] = obu.[RSBUAccountNumberID]
, tbl.[SeatingCapacity] = obu.[SeatingCapacity]
, tbl.[SerialName] = obu.[SerialName]
, tbl.[SerialNumber] = obu.[SerialNumber]
, tbl.[ShareRightDenominator] = obu.[ShareRightDenominator]
, tbl.[ShareRightNumerator] = obu.[ShareRightNumerator]
, tbl.[ShellMaterial] = obu.[ShellMaterial]
, tbl.[ShipAppointment] = obu.[ShipAppointment]
, tbl.[ShipClassID] = obu.[ShipClassID]
, tbl.[ShipName] = obu.[ShipName]
, tbl.[ShipRegDate] = obu.[ShipRegDate]
, tbl.[ShipRegNumber] = obu.[ShipRegNumber]
, tbl.[ShipTypeID] = obu.[ShipTypeID]
, tbl.[SibCityNSIID] = obu.[SibCityNSIID]
, tbl.[SibCountryID] = obu.[SibCountryID]
, tbl.[SibFederalDistrictID] = obu.[SibFederalDistrictID]
, tbl.[SibMeasureID] = obu.[SibMeasureID]
, tbl.[SignNumber] = obu.[SignNumber]
, tbl.[SortOrder] = obu.[SortOrder]
, tbl.[SPPCode] = obu.[SPPCode]
, tbl.[SPPItem] = obu.[SPPItem]
, tbl.[SSRID] = obu.[SSRID]
, tbl.[SSRTerminateID] = obu.[SSRTerminateID]
, tbl.[StartDate] = obu.[StartDate]
, tbl.[StartDateUse] = obu.[StartDateUse]
, tbl.[State] = obu.[State]
, tbl.[StateChangeDate] = obu.[StateChangeDate]
, tbl.[StateObjectMSFOID] = obu.[StateObjectMSFOID]
, tbl.[StateObjectRSBUID] = obu.[StateObjectRSBUID]
, tbl.[Storage] = obu.[Storage]
, tbl.[SubjectCode] = obu.[SubjectCode]
, tbl.[SubjectName] = obu.[SubjectName]
, tbl.[SubNumber] = obu.[SubNumber]
, tbl.[Tax] = ISNULL(obu.[Tax] , 0)
, tbl.[TaxBaseID] = obu.[TaxBaseID]
, tbl.[TaxCadastralIncludeDate] = obu.[TaxCadastralIncludeDate]
, tbl.[TaxCadastralIncludeDoc] = obu.[TaxCadastralIncludeDoc]
, tbl.[TaxExemptionEndDate] = obu.[TaxExemptionEndDate]
, tbl.[TaxExemptionEndDateLand] = obu.[TaxExemptionEndDateLand]
, tbl.[TaxExemptionEndDateTS] = obu.[TaxExemptionEndDateTS]
, tbl.[TaxExemptionID] = obu.[TaxExemptionID]
, tbl.[TaxExemptionReason] = obu.[TaxExemptionReason]
, tbl.[TaxExemptionReasonLand] = obu.[TaxExemptionReasonLand]
, tbl.[TaxExemptionReasonTS] = obu.[TaxExemptionReasonTS]
, tbl.[TaxExemptionStartDate] = obu.[TaxExemptionStartDate]
, tbl.[TaxExemptionStartDateLand] = obu.[TaxExemptionStartDateLand]
, tbl.[TaxExemptionStartDateTS] = obu.[TaxExemptionStartDateTS]
, tbl.[TaxFreeLandID] = obu.[TaxFreeLandID]
, tbl.[TaxFreeTSID] = obu.[TaxFreeTSID]
, tbl.[TaxLowerID] = obu.[TaxLowerID]
, tbl.[TaxLowerLandID] = obu.[TaxLowerLandID]
, tbl.[TaxLowerPercent] = obu.[TaxLowerPercent]
, tbl.[TaxLowerPercentLand] = obu.[TaxLowerPercentLand]
, tbl.[TaxLowerPercentTS] = obu.[TaxLowerPercentTS]
, tbl.[TaxLowerTSID] = obu.[TaxLowerTSID]
, tbl.[TaxRateLowerID] = obu.[TaxRateLowerID]
, tbl.[TaxRateLowerLandID] = obu.[TaxRateLowerLandID]
, tbl.[TaxRateLowerTSID] = obu.[TaxRateLowerTSID]
, tbl.[TaxRateTypeID] = obu.[TaxRateTypeID]
, tbl.[TaxRateValue] = obu.[TaxRateValue]
, tbl.[TaxRateValueLand] = obu.[TaxRateValueLand]
, tbl.[TaxRateValueTS] = obu.[TaxRateValueTS]
, tbl.[TaxRateWithExemption] = obu.[TaxRateWithExemption]
, tbl.[TaxRateWithExemptionLand] = obu.[TaxRateWithExemptionLand]
, tbl.[TaxRateWithExemptionTS] = obu.[TaxRateWithExemptionTS]
, tbl.[TaxVehicleKindCodeID] = obu.[TaxVehicleKindCodeID]
, tbl.[TechInspectInterval] = obu.[TechInspectInterval]
, tbl.[TransferRight] = obu.[TransferRight]
, tbl.[TransferBUSDate] = obu.[TransferBUSDate]
, tbl.[UpdateDate] = obu.[UpdateDate]
, tbl.[Useful] = obu.[Useful]
, tbl.[UsefulEnd] = obu.[UsefulEnd]
, tbl.[UsefulEndDate] = obu.[UsefulEndDate]
, tbl.[UsefulEndMSFO] = obu.[UsefulEndMSFO]
, tbl.[UsefulEndNU] = obu.[UsefulEndNU]
, tbl.[UsefulForMSFO] = obu.[UsefulForMSFO]
, tbl.[UsefulForNU] = obu.[UsefulForNU]
, tbl.[UsesKind] = obu.[UsesKind]
, tbl.[VehicleAverageCost] = obu.[VehicleAverageCost]
, tbl.[VehicleCategoryID] = obu.[VehicleCategoryID]
, tbl.[VehicleClassID] = obu.[VehicleClassID]
, tbl.[VehicleDeRegDate] = obu.[VehicleDeRegDate]
, tbl.[VehicleLabelID] = obu.[VehicleLabelID]
, tbl.[VehicleMarketCost] = obu.[VehicleMarketCost]
, tbl.[VehicleModelID] = obu.[VehicleModelID]
, tbl.[VehicleRegDate] = obu.[VehicleRegDate]
, tbl.[VehicleRegNumber] = obu.[VehicleRegNumber]
, tbl.[VehicleTaxFactor] = obu.[VehicleTaxFactor]
, tbl.[VehicleTenureTypeID] = obu.[VehicleTenureTypeID]
, tbl.[VehicleTypeID] = obu.[VehicleTypeID]
, tbl.[Well] = obu.[Well]
, tbl.[WellCategoryID] = obu.[WellCategoryID]
, tbl.[WhoUseID] = obu.[WhoUseID]
, tbl.[Width] = obu.[Width]
, tbl.[WidthUnitID] = obu.[WidthUnitID]
, tbl.[Wood] = ISNULL(obu.[Wood] , 0)
, tbl.[Year] = obu.[Year]
, tbl.[YearOfIssue] = obu.[YearOfIssue]

;
END
GO
";

            this.Sql(alterScript);
        }

        public override void Down()
        {
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxRateWithExemptionTS", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxRateWithExemptionLand", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxRateWithExemption", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxRateTypeID", c => c.Int());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionStartDateTS", c => c.DateTime());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionStartDateLand", c => c.DateTime());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionStartDate", c => c.DateTime());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionReasonTS", c => c.String());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionReasonLand", c => c.String());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionReason", c => c.String());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionID", c => c.Int());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionEndDateTS", c => c.DateTime());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionEndDateLand", c => c.DateTime());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxExemptionEndDate", c => c.DateTime());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxCadastralIncludeDoc", c => c.String());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxCadastralIncludeDate", c => c.DateTime());
            AddColumn("[CorpProp.Estate].InventoryObject", "TaxBaseID", c => c.Int());
            AddColumn("[CorpProp.Estate].InventoryObject", "IsInvestmentProgramm", c => c.Boolean(nullable: false));
            AddColumn("[CorpProp.Estate].InventoryObject", "IsEnergy", c => c.Boolean(nullable: false));
            AddColumn("[CorpProp.Estate].InventoryObject", "EnergyLabelID", c => c.Int());
            AddColumn("[CorpProp.Estate].InventoryObject", "EnergyDocsExist", c => c.Boolean());
            AddColumn("[CorpProp.Estate].InventoryObject", "DecisionsDetailsTSID", c => c.Int());
            AddColumn("[CorpProp.Estate].InventoryObject", "DecisionsDetailsLandID", c => c.Int());
            AddColumn("[CorpProp.Estate].InventoryObject", "DecisionsDetailsID", c => c.Int());
            AddColumn("[CorpProp.Estate].InventoryObject", "BenefitDocsExist", c => c.Boolean(nullable: false));
            AddColumn("[CorpProp.Estate].InventoryObject", "BenefitApplyForEnergy", c => c.Boolean(nullable: false));
            AddColumn("[CorpProp.Estate].InventoryObject", "Benefit", c => c.Boolean(nullable: false));
            DropForeignKey("[CorpProp.Estate].EstateTaxes", "TaxRateTypeID", "[CorpProp.NSI].TaxRateType");
            DropForeignKey("[CorpProp.Estate].EstateTaxes", "TaxExemptionID", "[CorpProp.NSI].TaxExemption");
            DropForeignKey("[CorpProp.Estate].EstateTaxes", "TaxesOfID", "[CorpProp.Estate].InventoryObject");
            DropForeignKey("[CorpProp.Estate].EstateTaxes", "TaxBaseID", "[CorpProp.NSI].TaxBase");
            DropForeignKey("[CorpProp.Estate].EstateTaxes", "EnergyLabelID", "[CorpProp.NSI].EnergyLabel");
            DropForeignKey("[CorpProp.Estate].EstateTaxes", "DecisionsDetailsTSID", "[CorpProp.NSI].DecisionsDetailsTS");
            DropForeignKey("[CorpProp.Estate].EstateTaxes", "DecisionsDetailsLandID", "[CorpProp.NSI].DecisionsDetailsLand");
            DropForeignKey("[CorpProp.Estate].EstateTaxes", "DecisionsDetailsID", "[CorpProp.NSI].DecisionsDetails");
            DropIndex("[CorpProp.Estate].EstateTaxes", new[] { "TaxRateTypeID" });
            DropIndex("[CorpProp.Estate].EstateTaxes", new[] { "TaxExemptionID" });
            DropIndex("[CorpProp.Estate].EstateTaxes", new[] { "TaxBaseID" });
            DropIndex("[CorpProp.Estate].EstateTaxes", new[] { "EnergyLabelID" });
            DropIndex("[CorpProp.Estate].EstateTaxes", new[] { "DecisionsDetailsTSID" });
            DropIndex("[CorpProp.Estate].EstateTaxes", new[] { "DecisionsDetailsLandID" });
            DropIndex("[CorpProp.Estate].EstateTaxes", new[] { "DecisionsDetailsID" });
            DropIndex("[CorpProp.Estate].EstateTaxes", new[] { "TaxesOfID" });
            DropTable("[CorpProp.Estate].EstateTaxes");
            CreateIndex("[CorpProp.Estate].InventoryObject", "TaxRateTypeID");
            CreateIndex("[CorpProp.Estate].InventoryObject", "TaxExemptionID");
            CreateIndex("[CorpProp.Estate].InventoryObject", "TaxBaseID");
            CreateIndex("[CorpProp.Estate].InventoryObject", "EnergyLabelID");
            CreateIndex("[CorpProp.Estate].InventoryObject", "DecisionsDetailsTSID");
            CreateIndex("[CorpProp.Estate].InventoryObject", "DecisionsDetailsLandID");
            CreateIndex("[CorpProp.Estate].InventoryObject", "DecisionsDetailsID");
            AddForeignKey("[CorpProp.Estate].InventoryObject", "TaxRateTypeID", "[CorpProp.NSI].TaxRateType", "ID");
            AddForeignKey("[CorpProp.Estate].InventoryObject", "TaxExemptionID", "[CorpProp.NSI].TaxExemption", "ID");
            AddForeignKey("[CorpProp.Estate].InventoryObject", "TaxBaseID", "[CorpProp.NSI].TaxBase", "ID");
            AddForeignKey("[CorpProp.Estate].InventoryObject", "EnergyLabelID", "[CorpProp.NSI].EnergyLabel", "ID");
            AddForeignKey("[CorpProp.Estate].InventoryObject", "DecisionsDetailsTSID", "[CorpProp.NSI].DecisionsDetailsTS", "ID");
            AddForeignKey("[CorpProp.Estate].InventoryObject", "DecisionsDetailsLandID", "[CorpProp.NSI].DecisionsDetailsLand", "ID");
            AddForeignKey("[CorpProp.Estate].InventoryObject", "DecisionsDetailsID", "[CorpProp.NSI].DecisionsDetails", "ID");
        }
    }
}