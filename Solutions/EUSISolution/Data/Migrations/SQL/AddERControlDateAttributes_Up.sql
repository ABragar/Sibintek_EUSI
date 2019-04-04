ALTER TABLE  [EUSI.Estate].[EstateRegistration]
ADD ERControlDateAttributesID INT NULL
GO


if exists (select * from dbo.sysobjects as sysobj
inner join sys.objects as obj on sysobj.id=obj.object_id
left join sys.schemas as objschema on obj.schema_id=objschema.schema_id
where sysobj.xtype = 'U' and sysobj.name = N'ERControlDateAttributes' and objschema.name = N'EUSI.Estate')
 DROP TABLE [EUSI.Estate].[ERControlDateAttributes] 
 GO
 CREATE TABLE [EUSI.Estate].[ERControlDateAttributes](
	[ID] [int] IDENTITY (1, 1) NOT NULL,
	[DateCDS] [datetime] NULL,
	[DateСreation] [datetime] NULL,
	[DateToVerify] [datetime] NULL,
	[DateVerification] [datetime] NULL,
	[DateRejection] [datetime] NULL,
	[DateToСlarify] [datetime] NULL,
	[Hidden] [bit] NOT NULL,
	[SortOrder] [FLOAT] NOT NULL,
	[RowVersion] [ROWVERSION] NOT NULL,
 CONSTRAINT [PK_dbo.ERControlDateAttributes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [EUSI.Estate].[ERControlDateAttributes] ON

INSERT INTO [EUSI.Estate].[ERControlDateAttributes]
  ( [ID],
	[DateCDS],
	[DateСreation],
	[DateToVerify],
	[DateVerification],
	[DateRejection],
	[DateToСlarify],
	[Hidden],
	[SortOrder] )
SELECT 
	[ID],
	[DateCDS],
	[DateСreation],
	[DateToVerify] ,
	[DateVerification] ,
	[DateRejection],
	[DateToСlarify],
	[Hidden],
	[SortOrder]
FROM [EUSI.Estate].[EstateRegistration]
GO

SET IDENTITY_INSERT [EUSI.Estate].[ERControlDateAttributes]  OFF

ALTER TABLE [EUSI.Estate].[EstateRegistration] DISABLE TRIGGER [TR_EstateRegistrationNumber]
GO

UPDATE [EUSI.Estate].[EstateRegistration]
SET ERControlDateAttributesID = ID
GO

ALTER TABLE [EUSI.Estate].[EstateRegistration] ENABLE TRIGGER [TR_EstateRegistrationNumber]
GO
ALTER TABLE [EUSI.Estate].[EstateRegistration] WITH NOCHECK
    ADD CONSTRAINT [FK_[EUSI.Estate]].EstateRegistration_EUSI.Estate.ERControlDateAttributes_ERControlDateAttributesID] FOREIGN KEY ([ERControlDateAttributesID]) REFERENCES [EUSI.Estate].[ERControlDateAttributes] ([ID]);
GO
ALTER TABLE [EUSI.Estate].[EstateRegistration] WITH CHECK CHECK CONSTRAINT [FK_[EUSI.Estate]].EstateRegistration_EUSI.Estate.ERControlDateAttributes_ERControlDateAttributesID];
GO
ALTER TABLE  [EUSI.Estate].[EstateRegistration]
DROP COLUMN [DateCDS],
	[DateСreation],
	[DateToVerify] ,
	[DateVerification] ,
	[DateRejection] ,
	[DateToСlarify]
GO