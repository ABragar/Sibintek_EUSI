namespace Data.EF
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;

    public partial class ChangeExportOSTemplate : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                DECLARE @templateId INT = (SELECT ID FROM [CorpProp.Export].ExportTemplate et WHERE et.Code = 'ExportOS')

                DECLARE @NewColumns TABLE([value] NVARCHAR(max), [column] INT)
                INSERT INTO @NewColumns
                VALUES 
					                (N'RowNumber',0)
	                                ,( N'ExternalID',1)
	                                ,( N'SubNumber',2)
	                                ,(N'CreatingFromER',3)
	                                ,( N'CreatingFromERPosition',4)
	                                ,( N'EUSINumber',5),
	                                ( N'ConsolidationCode',6),
	                                ( N'ConsolidationName',7),
	                                (N'ReceiptReason',8),
	                                (N'Contragent',9),
	                                (N'NameByDoc',10),
	                                (N'PrimaryDocDate',11),
	                                ( N'PrimaryDocNumber',12),
	                                ( N'StartDateUse',13),
	                                ( N'CadastralNumber',14),
	                                ( N'SibCountry',15),
	                                ( N'SibFederalDistrict',16),
	                                ( N'Region',17),
	                                ( N'SibCityNSI',18),
	                                ( N'Address',19),
	                                ( N'VehicleRegDate',20),
	                                ( N'YearOfIssue',21),
	                                ( N'VehicleCategory',22),
	                                ( N'DieselEngine',23),
	                                ( N'PowerUnit',24),
	                                ( N'Power',25),
	                                ( N'SerialNumber',26),
	                                ( N'EngineSize',27),
	                                ( N'Model',28),
	                                ( N'VehicleRegNumber',29)

                DELETE FROM [CorpProp.Export].ExportTemplateItem 
                WHERE [ExportTemplateID] = @templateId AND [IsColumnMap] = 1

                INSERT INTO [CorpProp.Export].ExportTemplateItem ([IsHistory], [Hidden], [SortOrder], [IsColumnMap], [Row], [Column],  [ExportTemplateID], [ActualDate], [CreateDate], [Oid], [Value])
                SELECT 0, 0, 0, 1, 1, [column], @templateId, GETDATE(), GETDATE(), NEWID(), [Value]
                FROM @NewColumns
            ");

            string connect = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "EUSI");
            var resName = "EUSI.Resources.MovingTemplates.ExportOS.xlsx";
            byte[] file;
            using (Stream stream = assembly.GetManifestResourceStream(resName))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            string createFileDbScript = "UPDATE [CorpProp.Document].[FileDB] " +
                "SET content = @File " +
                "WHERE [ID] IN (SELECT ID FROM [CorpProp.Export].ExportTemplate et WHERE et.Code = 'ExportOS')";

            using (var conn = new SqlConnection(connect))
            {
                conn.Open();
                using (var sqlWrite = new SqlCommand(createFileDbScript, conn))
                {
                    sqlWrite.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;
                    sqlWrite.ExecuteNonQuery();
                }
            }
        }
        
        public override void Down()
        {
        }
    }
}
