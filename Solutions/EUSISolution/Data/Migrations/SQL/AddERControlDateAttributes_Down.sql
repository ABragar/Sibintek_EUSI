
ALTER TABLE  [EUSI.Estate].[EstateRegistration]
ADD [DateCDS] [datetime] NULL,
	[DateСreation] [datetime] NULL,
	[DateToVerify] [datetime] NULL,
	[DateVerification] [datetime] NULL,
	[DateRejection] [datetime] NULL,
	[DateToСlarify] [datetime] NULL
GO

ALTER TABLE [EUSI.Estate].[EstateRegistration] DISABLE TRIGGER [TR_EstateRegistrationNumber]
GO

UPDATE [EUSI.Estate].[EstateRegistration]
SET [DateCDS] = (SELECT DateCDS FROM [EUSI.Estate].[ERControlDateAttributes] AS era WHERE era.ID = [EUSI.Estate].[EstateRegistration].ID),
	[DateСreation] = (SELECT DateСreation FROM [EUSI.Estate].[ERControlDateAttributes] AS era WHERE era.ID = [EUSI.Estate].[EstateRegistration].ID),
	[DateToVerify] = (SELECT DateToVerify FROM [EUSI.Estate].[ERControlDateAttributes] AS era WHERE era.ID = [EUSI.Estate].[EstateRegistration].ID),
	[DateVerification] = (SELECT DateVerification FROM [EUSI.Estate].[ERControlDateAttributes] AS era WHERE era.ID = [EUSI.Estate].[EstateRegistration].ID),
	[DateRejection] = (SELECT DateRejection FROM [EUSI.Estate].[ERControlDateAttributes] AS era WHERE era.ID = [EUSI.Estate].[EstateRegistration].ID),
	[DateToСlarify] = (SELECT DateToСlarify FROM [EUSI.Estate].[ERControlDateAttributes] AS era WHERE era.ID = [EUSI.Estate].[EstateRegistration].ID)	
GO

ALTER TABLE [EUSI.Estate].[EstateRegistration] ENABLE TRIGGER [TR_EstateRegistrationNumber]
GO
if exists (select * from dbo.sysobjects as sysobj
inner join sys.objects as obj on sysobj.id=obj.object_id
left join sys.schemas as objschema on obj.schema_id=objschema.schema_id
where sysobj.xtype = 'U' and sysobj.name = N'ERControlDateAttributes' and objschema.name = N'EUSI.Estate')
DROP TABLE [EUSI.Estate].[ERControlDateAttributes]
GO
ALTER TABLE  [EUSI.Estate].[EstateRegistration]
DROP COLUMN ERControlDateAttributesID
GO

ALTER TABLE [EUSI.Estate].[EstateRegistration]
    DROP CONSTRAINT [FK_[EUSI.Estate]].EstateRegistration_EUSI.Estate.ERControlDateAttributes_ERControlDateAttributesID]
GO