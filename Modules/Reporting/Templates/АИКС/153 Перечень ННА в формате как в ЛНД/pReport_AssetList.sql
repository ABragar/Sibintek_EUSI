CREATE PROC [dbo].[pReport_AssetList]
@vintAssetListID	INT,
@vintYear	INT,
@vstrSociety	NVARCHAR(255)
  AS
  -- =============================================
-- Author:		Sharov Alexey
-- Create date: 18.01.2018
-- Description:	Retrive data to report NonCoreAsset by AssetList
-- 
-- Parametrs:
--    @vintAssetListID  - ID перечня (must be NULL)
--    @vintYear  		- Год по которому выберется перечень
--    @vstrSociety		- IDEUP для ОГ
-- 
-- History:
--    Author		   Date		Description
--    SharovAV		18.01.2018	Create the procedure
--    SharovAV		19.01.2018	Fix NonCoreAssetNameComplex add case expression. Add filters.
-- =============================================


-- rs.1
SELECT 
RowNum = ROW_NUMBER() OVER(ORDER BY Asset.[NameAsset] ASC)
,Asset.[AssetOwnerName]
,Asset.[NonCoreAssetName]
,NonCoreAssetNameComplex= 
						CASE 
							WHEN InventoryObject.PropertyComplexID IS NULL THEN Asset.[NonCoreAssetName]
							ELSE PropertyComplex.Name
						END
,Asset.[NnamePropertyPartComplex]
,Asset.[NameAsset]
,Asset.[InventoryNumber]
,Asset.[LocationAssetText]
,Asset.[PropertyCharacteristics]
,Asset.[EncumbranceText]
,Asset.[NonCoreAssetOwnerCategoryID]
,Asset.[Supervising]
,Asset.[InitialCost]
,Asset.[ResidualCost]
,Asset.[AnnualRevenueFromUseWithoutVAT]
,Asset.[AnnualCostContentWithoutVAT]
,Asset.[IndicativeValuationWithoutVAT]
,Asset.[IndicativeValuationIncludingVAT]
,Asset.[IndicativeVAT]
,Asset.[MarketValuationWithoutVAT]
,Asset.[MarketValuationIncludingVAT]
,Asset.[MarketValuationVAT]
,Asset.[NoOtherObjectsOnLand]
,Asset.[ProposedActionsMethodImplementation]
,Asset.[BudgetProposedProcedure]
,Asset.[PublicationExpense]
,Asset.[AppraisalExpense]
,Asset.[BiddingOrganizersBenefits]
,Asset.[SellingResidualCost]
,Asset.[OtherExpenses]
,Asset.[ForecastPeriod]
,Asset.[RationaleProposals]
,Asset.[Description]
,Asset.[StrategicDiscrepancy]
,Asset.[NonCoreBusiness],NonCoreAssetNameComplex= 
						CASE 
							WHEN InventoryObject.PropertyComplexID IS NULL THEN Asset.[NonCoreAssetName]
							ELSE PropertyComplex.Name
						END
,Asset.[InexpedientInvestment]
,Asset.[PreservationOfCompetitiveAdvantages]
,Asset.[LackOfStrategicGoals]
,Asset.[WorthSelling]
,Asset.[InexpedientMaintence]
,Asset.[ReputationLoss]
,Asset.[SpecialNonCoreAssetCriteria]
,Asset.[TaskDate]
,Asset.[LeavingDate]
,Asset.[EGRNNumber]
,Asset.[JustificationOfAuthorityDocumentID]
,Asset.[ApprovedListDocumentID]
,Asset.[WorkingGroupConclusionDocumentID]
,Asset.[CreateDate]
,Asset.[ActualDate]
,Asset.[NonActualDate]
,Asset.[ImportUpdateDate]
,Asset.[ImplementationWay_ID]
FROM [CorpProp.Asset].[NonCoreAssetList] AS AssetList
LEFT JOIN [CorpProp.Base].[DictObject] AS DictObject ON (DictObject.ID=AssetList.NonCoreAssetListStateID)
LEFT JOIN [CorpProp.Asset].[NonCoreAssetAndList] AS NonCoreAssetAndList ON (NonCoreAssetAndList.ObjRigthId=AssetList.ID)
LEFT JOIN [CorpProp.Asset].NonCoreAsset AS Asset ON (Asset.ID=NonCoreAssetAndList.ObjLeftId)
LEFT JOIN [CorpProp.Subject].[Society] AS Society ON (Society.ID=AssetList.SocietyID)
LEFT JOIN [CorpProp.Estate].InventoryObject AS InventoryObject ON InventoryObject.ID=Asset.EstateObjectID
LEFT JOIN [CorpProp.Estate].PropertyComplex AS PropertyComplex ON PropertyComplex.ID=InventoryObject.PropertyComplexID
WHERE 
(AssetList.Hidden IS NULL OR AssetList.Hidden=0)
AND (NonCoreAssetAndList.Hidden IS NULL OR NonCoreAssetAndList.Hidden=0)
AND (Asset.Hidden IS NULL OR Asset.Hidden=0) 
AND AssetList.ID=ISNULL(@vintAssetListID,AssetList.ID)
AND AssetList.Year=@vintYear
AND Society.IDEUP=@vstrSociety

