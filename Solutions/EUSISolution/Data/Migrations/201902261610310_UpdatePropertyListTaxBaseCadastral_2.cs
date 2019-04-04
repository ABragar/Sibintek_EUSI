namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePropertyListTaxBaseCadastral_2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"  PRINT N'The following operation was generated from a refactoring log file fcbfe1fe-fa2a-4d50-94d6-40704821d285';
                    PRINT N'Rename [EUSI.NSI].[PropertyListTaxBaseCadastral].[DocNumberAndDate] to ApprovingDocNumber';
                    GO
                    EXECUTE sp_rename @objname = N'[EUSI.NSI].[PropertyListTaxBaseCadastral].[DocNumberAndDate]', @newname = N'ApprovingDocNumber', @objtype = N'COLUMN';
                    GO
                    PRINT N'Starting rebuilding table [EUSI.NSI].[PropertyListTaxBaseCadastral]...';
                    GO
                    BEGIN TRANSACTION;
                    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
                    SET XACT_ABORT ON;
                    CREATE TABLE [EUSI.NSI].[tmp_ms_xx_PropertyListTaxBaseCadastral] (
                        [ID]                         INT            NOT NULL,
                        [CadastralNumber]            NVARCHAR (MAX) NULL,
                        [RoomCadastralNumber]        NVARCHAR (MAX) NULL,
                        [ConditionalNumber]          NVARCHAR (MAX) NULL,
                        [ApprovingDocNumber]         NVARCHAR (MAX) NULL,
                        [ApprovingDocDate]           DATETIME       NULL,
                        [SibRegionID]                INT            NULL,
                        [Address]                    NVARCHAR (MAX) NULL,
                        [IsCadastralEstateUpdated]   BIT            NULL,
                        [CadastralEstateUpdatedDate] DATETIME       NULL,
                        CONSTRAINT [tmp_ms_xx_constraint_PK_[EUSI.NSI]].PropertyListTaxBaseCadastral1] PRIMARY KEY CLUSTERED ([ID] ASC)
                    );
                    IF EXISTS (SELECT TOP 1 1 
                               FROM   [EUSI.NSI].[PropertyListTaxBaseCadastral])
                        BEGIN
                            INSERT INTO [EUSI.NSI].[tmp_ms_xx_PropertyListTaxBaseCadastral] ([ID], [CadastralNumber], [RoomCadastralNumber], [ConditionalNumber], [ApprovingDocNumber], [Address], [IsCadastralEstateUpdated], [CadastralEstateUpdatedDate])
                            SELECT   [ID],
                                     [CadastralNumber],
                                     [RoomCadastralNumber],
                                     [ConditionalNumber],
                                     [ApprovingDocNumber],
                                     [Address],
                                     [IsCadastralEstateUpdated],
                                     [CadastralEstateUpdatedDate]
                            FROM     [EUSI.NSI].[PropertyListTaxBaseCadastral]
                            ORDER BY [ID] ASC;
                        END
                    DROP TABLE [EUSI.NSI].[PropertyListTaxBaseCadastral];
                    EXECUTE sp_rename N'[EUSI.NSI].[tmp_ms_xx_PropertyListTaxBaseCadastral]', N'PropertyListTaxBaseCadastral';
                    EXECUTE sp_rename N'[EUSI.NSI].[tmp_ms_xx_constraint_PK_[EUSI.NSI]].PropertyListTaxBaseCadastral1]', N'PK_[EUSI.NSI].PropertyListTaxBaseCadastral', N'OBJECT';
                    COMMIT TRANSACTION;
                    SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
                    GO
                    PRINT N'Creating [EUSI.NSI].[PropertyListTaxBaseCadastral].[IX_ID]...';
                    GO
                    CREATE NONCLUSTERED INDEX [IX_ID]
                        ON [EUSI.NSI].[PropertyListTaxBaseCadastral]([ID] ASC);
                    GO
                    -- Refactoring step to update target server with deployed transaction logs
                    IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'fcbfe1fe-fa2a-4d50-94d6-40704821d285')
                    INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('fcbfe1fe-fa2a-4d50-94d6-40704821d285')
                    GO
                    PRINT N'Creating [EUSI.NSI].[FK_[EUSI.NSI]].PropertyListTaxBaseCadastral_[CorpProp.Base]].DictObject_ID]...';
                    GO
                    ALTER TABLE [EUSI.NSI].[PropertyListTaxBaseCadastral] WITH NOCHECK
                        ADD CONSTRAINT [FK_[EUSI.NSI]].PropertyListTaxBaseCadastral_[CorpProp.Base]].DictObject_ID] FOREIGN KEY ([ID]) REFERENCES [CorpProp.Base].[DictObject] ([ID]);
                    GO
                    PRINT N'Creating [EUSI.NSI].[PropertyListTaxBaseCadastral].[IX_SibRegionID]...';
                    GO
                    CREATE NONCLUSTERED INDEX [IX_SibRegionID]
                        ON [EUSI.NSI].[PropertyListTaxBaseCadastral]([SibRegionID] ASC);
                    GO
                    PRINT N'Creating [EUSI.NSI].[FK_[EUSI.NSI]].PropertyListTaxBaseCadastral_[CorpProp.FIAS]].SibRegion_SibRegionID]...';
                    GO
                    ALTER TABLE [EUSI.NSI].[PropertyListTaxBaseCadastral] WITH NOCHECK
                        ADD CONSTRAINT [FK_[EUSI.NSI]].PropertyListTaxBaseCadastral_[CorpProp.FIAS]].SibRegion_SibRegionID] FOREIGN KEY ([SibRegionID]) REFERENCES [CorpProp.FIAS].[SibRegion] ([ID]);
                    GO
                    PRINT N'Checking existing data against newly created constraints';
                    GO
                    ALTER TABLE [EUSI.NSI].[PropertyListTaxBaseCadastral] WITH CHECK CHECK CONSTRAINT [FK_[EUSI.NSI]].PropertyListTaxBaseCadastral_[CorpProp.FIAS]].SibRegion_SibRegionID];
                    GO
                    PRINT N'Update complete.';
                    GO");
        }
        
        public override void Down()
        {
            Sql(@"  update t
		            set t.ApprovingDocNumber = t.ApprovingDocNumber + ' ' + CONVERT(nvarchar(30), isnull(t.ApprovingDocDate, ''), 121)
		            FROM [EUSI.NSI].[PropertyListTaxBaseCadastral] as t where t.ApprovingDocDate is not null
		            GO
		            BEGIN TRANSACTION
                    SET QUOTED_IDENTIFIER ON
                    SET ARITHABORT ON
                    SET NUMERIC_ROUNDABORT OFF
                    SET CONCAT_NULL_YIELDS_NULL ON
                    SET ANSI_NULLS ON
                    SET ANSI_PADDING ON
                    SET ANSI_WARNINGS ON
                    COMMIT
                    BEGIN TRANSACTION
                    GO
                    ALTER TABLE [EUSI.NSI].PropertyListTaxBaseCadastral
	                    DROP CONSTRAINT [FK_[EUSI.NSI]].PropertyListTaxBaseCadastral_[CorpProp.FIAS]].SibRegion_SibRegionID]
                    GO
                    ALTER TABLE [CorpProp.FIAS].SibRegion SET (LOCK_ESCALATION = TABLE)
                    GO
                    COMMIT
                    BEGIN TRANSACTION
                    GO
                    EXECUTE sp_rename N'[EUSI.NSI].PropertyListTaxBaseCadastral.ApprovingDocNumber', N'Tmp_DocNumberAndDate', 'COLUMN' 
                    GO
                    EXECUTE sp_rename N'[EUSI.NSI].PropertyListTaxBaseCadastral.Tmp_DocNumberAndDate', N'DocNumberAndDate', 'COLUMN' 
                    GO
                    DROP INDEX IX_SibRegionID ON [EUSI.NSI].PropertyListTaxBaseCadastral
                    GO
                    ALTER TABLE [EUSI.NSI].PropertyListTaxBaseCadastral
	                    DROP COLUMN ApprovingDocDate, SibRegionID
                    GO
                    ALTER TABLE [EUSI.NSI].PropertyListTaxBaseCadastral SET (LOCK_ESCALATION = TABLE)
                    GO
                    COMMIT");
        }
    }
}
