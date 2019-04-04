namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOSExtView : DbMigration
    {
        public override void Up()
        {            
            AddColumn("[CorpProp.Import].ImportErrorLog", "ErrorCode", c => c.String());

            Sql(@"IF OBJECT_ID(N'[EUSI.Accounting].[AccountingObjectExtView]') IS NOT NULL 
                  DROP VIEW [EUSI.Accounting].[AccountingObjectExtView]
                  GO
                  CREATE VIEW [EUSI.Accounting].[AccountingObjectExtView]
                        AS
                    SELECT a.*
                    , e.Number
                    , Consolidation.Code AS ConsolidationCode
                    , StateObjectRSBU.Code as StateObjectRSBUCode
                    FROM[CorpProp.Accounting].[AccountingObject] a
                    LEFT JOIN[CorpProp.Estate].Estate e on a.EstateID = e.ID
                    LEFT JOIN(SELECT d.ID, Code FROM[CorpProp.NSI].Consolidation c INNER JOIN[CorpProp.Base].DictObject d  on  d.ID = c.ID) Consolidation on a.ConsolidationID = Consolidation.ID
                    LEFT JOIN(SELECT d.ID, Code FROM[CorpProp.NSI].StateObjectRSBU s INNER JOIN[CorpProp.Base].DictObject d  on  d.ID = s.ID) StateObjectRSBU on a.StateObjectRSBUID = StateObjectRSBU.ID
                    ");

            Sql(@"if exists (select * from dbo.sysobjects where Name = N'TR_EstateCalculatedField_Ins' and xtype = 'TR')
                DROP TRIGGER [CorpProp.Estate].[TR_EstateCalculatedField_Ins]");

            Sql(@"
                   CREATE TRIGGER [CorpProp.Estate].[TR_EstateCalculatedField_Ins]
                    ON [CorpProp.Estate].[Estate] AFTER INSERT
                    AS
                    BEGIN

	                INSERT INTO [CorpProp.Estate].[EstateCalculatedField]
	                (
		                [ChildObjectsCount]
		                ,[InitialCostSumOBU]
		                ,[ResidualCostSumOBU]
		                ,[InitialCostSumNU]
		                ,[ResidualCostSumNU]
		                ,[EstateID]
		                ,[Hidden]
		                ,[SortOrder])
		                select
		                0 as 'ChildObjectsCount'
                        ,0.00 as 'InitialCostSumOBU'
                        ,0.00 as 'ResidualCostSumOBU'
                        ,0.00 as 'InitialCostSumNU'
                        ,0.00 as 'ResidualCostSumNU'
                        ,est2.ID as 'EstateID'
                        ,0 as 'Hidden'
                        ,0 as 'SortOrder'
		                FROM Inserted  as est2 
		                left outer join [CorpProp.Estate].[EstateCalculatedField] as calc on est2.[CalculateID]=calc.[ID]
		                where calc.ID is null


		                update est2 set est2.CalculateID=calc.ID
		                FROM [CorpProp.Estate].Estate  as est2 
		                left outer join [CorpProp.Estate].[EstateCalculatedField] as calc on est2.ID=calc.EstateID
		                where est2.CalculateID is null
                    END
            ");
        }
        
        public override void Down()
        {           
            DropColumn("[CorpProp.Import].ImportErrorLog", "ErrorCode");
            Sql(@"IF OBJECT_ID(N'[EUSI.Accounting].[AccountingObjectExtView]') IS NOT NULL 
                  DROP VIEW [EUSI.Accounting].[AccountingObjectExtView]");
        }
    }
}
