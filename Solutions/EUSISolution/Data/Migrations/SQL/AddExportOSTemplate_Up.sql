INSERT INTO [CorpProp.Export].[ExportTemplate] 
	([IsHistory], [Hidden], [SortOrder], [Name], [Code], [Mnemonic], [FileName], [StartRow], [StartColumn], [ActualDate], [CreateDate], [Oid])
VALUES (0, 0, 0, N'Экспорт данных об ОС/НМА', 'ExportOS', 'AccountingObject', N'ExportOS.xlsx', 4, 1, GETDATE(), GETDATE(), NEWID());

DECLARE @exportTemplateID INT = @@IDENTITY;

INSERT INTO [CorpProp.Export].[ExportTemplateItem] 
	([IsHistory], [Hidden], [SortOrder], [IsColumnMap], [Row], [Column],  [ExportTemplateID], [ActualDate], [CreateDate], [Oid], [Value])
VALUES 
	(0, 0, 0, 1, 1, 0, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'RowNumber'),
	(0, 0, 0, 1, 1, 1, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'CreatingFromER'),
	(0, 0, 0, 1, 1, 2, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'CreatingFromERPosition'),
	(0, 0, 0, 1, 1, 3, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'EUSINumber'),
	(0, 0, 0, 1, 1, 4, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'ConsolidationCode'),
	(0, 0, 0, 1, 1, 5, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'ConsolidationName'),
	(0, 0, 0, 1, 1, 6, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'ReceiptReason'),
	(0, 0, 0, 1, 1, 7, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'Contragent'),
	(0, 0, 0, 1, 1, 8, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'NameByDoc'),
	(0, 0, 0, 1, 1, 9, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'PrimaryDocDate'),
	(0, 0, 0, 1, 1, 10, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'PrimaryDocNumber'),
	(0, 0, 0, 1, 1, 11, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'StartDateUse'),
	(0, 0, 0, 1, 1, 12, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'CadastralNumber'),
	(0, 0, 0, 1, 1, 13, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'SibCountry'),
	(0, 0, 0, 1, 1, 14, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'SibFederalDistrict'),
	(0, 0, 0, 1, 1, 15, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'Region'),
	(0, 0, 0, 1, 1, 16, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'SibCityNSI'),
	(0, 0, 0, 1, 1, 17, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'Address'),
	(0, 0, 0, 1, 1, 18, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'VehicleRegDate'),
	(0, 0, 0, 1, 1, 19, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'YearOfIssue'),
	(0, 0, 0, 1, 1, 20, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'VehicleCategory'),
	(0, 0, 0, 1, 1, 21, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'DieselEngine'),
	(0, 0, 0, 1, 1, 22, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'PowerUnit'),
	(0, 0, 0, 1, 1, 23, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'Power'),
	(0, 0, 0, 1, 1, 24, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'SerialNumber'),
	(0, 0, 0, 1, 1, 25, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'EngineSize'),
	(0, 0, 0, 1, 1, 26, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'Model'),
	(0, 0, 0, 1, 1, 27, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'VehicleRegNumber'),
	(0, 0, 0, 1, 1, 28, @exportTemplateID, GETDATE(), GETDATE(), NEWID(), 'ExternalID')