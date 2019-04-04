namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OGCalculatedField : DbMigration
    {
        public override void Up()
        {                       
            AlterColumn("[CorpProp.Analyze.Accounting].RecordBudgetLine", "DateOfValue", c => c.DateTime());
            Sql(@"

CREATE VIEW [CorpProp.Subject].[SocietyCalculatedField]
AS


SELECT soc.[ID]
      ,soc.[ShortName]
      ,soc.[FullName]
      ,soc.[ShortNameEng]
      ,soc.[FullNameEng]
      ,soc.[SDP]
      ,soc.[KSK]
      ,soc.[SubjectTypeID]
      ,soc.[SubjectKindID]
      ,soc.[CountryID]
      ,soc.[AddressActualString]
      ,soc.[AddressLegalString]
      ,soc.[Phone]
      ,soc.[Email]
      ,soc.[OGRN]
      ,soc.[OGRNIP]
      ,soc.[OKVEDID]
      ,soc.[INN]
      ,soc.[KPP]
      ,soc.[OKPO]
      ,soc.[OKATOID]
      ,soc.[DateRegistration]
      ,soc.[HeadName]
      ,soc.[HeadPosition]
      ,soc.[IsSubjectMSP]
      ,soc.[Description]
      ,soc.[ResponsableForResponseID]
      ,soc.[IDEUP]
      ,soc.[IsSocietyKey]
      ,soc.[IsSocietyJoint]
      ,soc.[IsSocietyResident]
      ,soc.[IsSocietyControlled]
      ,soc.[IsSociety]
      ,soc.[CurrencyID]
      ,soc.[ConsolidationUnitID]
      ,soc.[BusinessSegment]
      ,soc.[BusinessBlockID]
      ,soc.[BusinessDirection]
      ,soc.[ProductionBlock]
      ,soc.[ShareInEquity]
      ,soc.[ShareInVotingRights]
      ,soc.[SizeAuthorizedCapital]
      ,soc.[BeneficialShareInCapital]
      ,soc.[BeneficialShareInVotingRights]
      ,soc.[BCA]
      ,soc.[ChA]
      ,soc.[SalesRevenue]
      ,soc.[NetProfit]
      ,soc.[AggregateFinancialResultOfThePeriod]
      ,soc.[DateInclusionInGroup]
      ,soc.[DateExclusionFromGroup]
      ,soc.[DateInclusionInPerimeter]
      ,soc.[BaseInclusionInPerimeterID]
      ,soc.[DateExclusionFromPerimeter]
      ,soc.[IsExclusionFromPerimeter]
      ,soc.[BaseExclusionFromPerimeterID]
      ,soc.[SoleExecutiveBodyName]
      ,soc.[SoleExecutiveBodyPost]
      ,soc.[SoleExecutiveBodyDateFrom]
      ,soc.[SoleExecutiveBodyDateTo]
      ,soc.[Curator]
      ,soc.[ActualKindActivityID]
      ,soc.[FederalDistrictID]
      ,soc.[RegionID]
      ,soc.[City]
      ,soc.[IsKOUControl]
      ,soc.[IsShareControl]
      ,soc.[IsSoleExecutiveBodyControl]
      ,soc.[OPFID]
      ,soc.[UnitOfCompanyID]
      ,soc.[SocietyPredecessorsCount]
      ,soc.[DirectShare]
      ,soc.[DirectShareInVotingRights]
      ,soc.[DirectParticipantCount]
      ,soc.[DirectParticipantList]
      ,soc.[DataDateFrom]
      ,soc.[DataDateTo]
	  
      ,isnull(socEstate.cEstate,0) as 'CountInventoryObject'
      ,isnull(socEstate.sInitialCostSumOBU,0.0) as 'InitialCostInventoryObject'
      ,isnull(socEstate.sResidualCostSumOBU,0.0) as 'ResidualCostInventoryObject'
      ,isnull(socRealEstate.cEstate,0) as 'CountRealEstate'
      ,isnull(socRealEstateRight.rcEstate,0) as 'CountRealEstateRight'
      ,isnull(socRealEstate.sInitialCostSumOBU,0.0) as 'InitialCostRealEstate'
      ,isnull(socRealEstate.sResidualCostSumOBU,0.0) as 'ResidualCostRealEstate'
      ,isnull(socRealEstateNotRight.cEstate,0) as 'CountRealEstateNotRight'
      ,isnull(socRealEstateNotRight.sInitialCostSumOBU,0.0) as 'InitialCostRealEstateNotRight'
      ,isnull(socRealEstateNotRight.sResidualCostSumOBU,0.0) as 'ResidualCostRealEstateNotRight'
      ,isnull(socMovableEstate.cEstate,0) as 'CountMovableEstate'
      ,isnull(socMovableEstate.sInitialCostSumOBU,0.0) as 'InitialCostMovableEstate'
      ,isnull(socMovableEstate.sResidualCostSumOBU,0.0) as 'ResidualCostMovableEstate'
      ,isnull(socLandEstate.cEstate,0) as 'CountLandEstate'
      ,isnull(socLandEstate.sInitialCostSumOBU,0.0) as 'InitialCostLandEstate'
      ,isnull(socLandEstate.sResidualCostSumOBU,0.0) as 'ResidualCostLandEstate'
      ,isnull(socLandEstate.sCadastralValue,0.0) as 'CadastralValueLandEstate'
      ,isnull(socLandRentEstate.cRent,0) as 'CountRentalLandEstate'
      ,soc.[Oid]
      ,soc.[IsHistory]
      ,soc.[CreateDate]
      ,soc.[ActualDate]
      ,soc.[NonActualDate]
      ,soc.[ImportUpdateDate]
      ,soc.[ImportDate]
      ,soc.[Hidden]
	  ,soc.[SortOrder]
      ,soc.[RowVersion]

FROM [CorpProp.Subject].[Society] as soc
  left join (
				select 
				society.ID as 'SocID'
				, count(estate.id) as 'cEstate'
				, sum(isnull(calc.InitialCostSumOBU,0.0)) as 'sInitialCostSumOBU'
				, sum(isnull(calc.ResidualCostSumOBU,0.0)) as 'sResidualCostSumOBU' 
				from [CorpProp.Estate].Estate as estate
				inner join [CorpProp.Estate].InventoryObject as inv on estate.ID=inv.ID
				left join [CorpProp.Estate].EstateCalculatedField as calc on estate.CalculateID=calc.ID
				inner join [CorpProp.Subject].Society as society on calc.OwnerID=society.ID
				group by society.ID) as socEstate on soc.ID = socEstate.SocID 
left join (
				select 
				society.ID as 'SocID'
				, count(estate.id) as 'cEstate'
				, sum(isnull(calc.InitialCostSumOBU,0.0)) as 'sInitialCostSumOBU'
				, sum(isnull(calc.ResidualCostSumOBU,0.0)) as 'sResidualCostSumOBU' 
				from [CorpProp.Estate].Estate as estate
				inner join [CorpProp.Estate].RealEstate as realEs on estate.ID=realEs.ID
				left join [CorpProp.Estate].EstateCalculatedField as calc on estate.CalculateID=calc.ID
				inner join [CorpProp.Subject].Society as society on calc.OwnerID=society.ID
				group by society.ID) as socRealEstate on soc.ID = socRealEstate.SocID 
left join (
				select 
				society.ID as 'SocID'
				, count(estate.id) as 'cEstate'
				, count(tRight.id) as 'rcEstate'
				, sum(isnull(calc.InitialCostSumOBU,0.0)) as 'sInitialCostSumOBU'
				, sum(isnull(calc.ResidualCostSumOBU,0.0)) as 'sResidualCostSumOBU' 
				from [CorpProp.Estate].Estate as estate
				inner join [CorpProp.Estate].RealEstate as realEs on estate.ID=realEs.ID
				inner join [CorpProp.Estate].EstateCalculatedField as calc on estate.CalculateID=calc.ID
				inner join [CorpProp.Subject].Society as society on calc.OwnerID=society.ID
				left join [CorpProp.Law].[Right] as tRight on realEs.ID=tRight.EstateID and calc.OwnerID=tRight.SocietyID
				group by society.ID) as socRealEstateRight on soc.ID = socRealEstateRight.SocID

left join (
				select 
				society.ID as 'SocID'
				, count(estate.id) as 'cEstate'
				, sum(isnull(calc.InitialCostSumOBU,0.0)) as 'sInitialCostSumOBU'
				, sum(isnull(calc.ResidualCostSumOBU,0.0)) as 'sResidualCostSumOBU' 
				from [CorpProp.Estate].Estate as estate
				inner join [CorpProp.Estate].RealEstate as realEs on estate.ID=realEs.ID
				inner join [CorpProp.Estate].EstateCalculatedField as calc on estate.CalculateID=calc.ID
				inner join [CorpProp.Subject].Society as society on calc.OwnerID=society.ID
				left outer join [CorpProp.Law].[Right] as tRight on realEs.ID=tRight.EstateID and calc.OwnerID=tRight.SocietyID
				where tRight.ID is null
				group by society.ID
			)  as socRealEstateNotRight on soc.ID = socRealEstateNotRight.SocID

left join (
				select 
				society.ID as 'SocID'
				, count(estate.id) as 'cEstate'
				, sum(isnull(calc.InitialCostSumOBU,0.0)) as 'sInitialCostSumOBU'
				, sum(isnull(calc.ResidualCostSumOBU,0.0)) as 'sResidualCostSumOBU'
				from [CorpProp.Estate].Estate as estate
				inner join [CorpProp.Estate].MovableEstate as realEs on estate.ID=realEs.ID
				left join [CorpProp.Estate].EstateCalculatedField as calc on estate.CalculateID=calc.ID
				inner join [CorpProp.Subject].Society as society on calc.OwnerID=society.ID
				group by society.ID) as socMovableEstate on soc.ID = socMovableEstate.SocID

left join (
				select 
				society.ID as 'SocID'
				, count(estate.id) as 'cEstate'
				, sum(isnull(calc.InitialCostSumOBU,0.0)) as 'sInitialCostSumOBU'
				, sum(isnull(calc.ResidualCostSumOBU,0.0)) as 'sResidualCostSumOBU'
				, sum(isnull(cadEs.CadastralValue,0.0)) as 'sCadastralValue'
				from [CorpProp.Estate].Estate as estate
				inner join [CorpProp.Estate].Cadastral as cadEs on estate.ID=cadEs.ID
				inner join [CorpProp.Estate].Land as realEs on cadEs.ID=realEs.ID
				left join [CorpProp.Estate].EstateCalculatedField as calc on estate.CalculateID=calc.ID
				inner join [CorpProp.Subject].Society as society on calc.OwnerID=society.ID
				group by society.ID) as socLandEstate on soc.ID = socLandEstate.SocID

left join (
				select 
				society.ID as 'SocID'
				,count(estate.id) as 'cEstate'
				,count(d.ID) as 'cRent'
				, sum(isnull(calc.InitialCostSumOBU,0.0)) as 'sInitialCostSumOBU'
				, sum(isnull(calc.ResidualCostSumOBU,0.0)) as 'sResidualCostSumOBU'
				, sum(isnull(cadEs.CadastralValue,0.0)) as 'sCadastralValue'
				from [CorpProp.Estate].Estate as estate
				inner join [CorpProp.Accounting].AccountingObject as ao on estate.ID=ao.EstateID
				inner join [CorpProp.Estate].Cadastral as cadEs on estate.ID=cadEs.ID
				inner join [CorpProp.Estate].Land as realEs on cadEs.ID=realEs.ID
				left join [CorpProp.Estate].EstateCalculatedField as calc on estate.CalculateID=calc.ID and calc.OwnerID is not null and calc.OwnerID <> isnull(calc.WhoUseID,0)
				inner join [CorpProp.Subject].Society as society on calc.OwnerID=society.ID
				left join [CorpProp.Base].DictObject as d on ao.AccountingStatusID=d.ID and  (d.Code in ('052', '52', '106') or d.Code like '%InRental%')
				group by society.ID) as socLandRentEstate on soc.ID = socLandRentEstate.SocID
            ");
        }
        
        public override void Down()
        {
            Sql(@"if exists (select * from dbo.sysobjects as sysobj
                    inner join sys.objects as obj on sysobj.id=obj.object_id
                    left join sys.schemas as objschema on obj.schema_id=objschema.schema_id
                    where sysobj.xtype = 'V' and sysobj.name = N'SocietyCalculatedField' and objschema.name = N'CorpProp.Subject')
                  DROP VIEW [CorpProp.Subject].[SocietyCalculatedField];");
            AlterColumn("[CorpProp.Analyze.Accounting].RecordBudgetLine", "DateOfValue", c => c.DateTime(nullable: false));
            
        }
    }
}
