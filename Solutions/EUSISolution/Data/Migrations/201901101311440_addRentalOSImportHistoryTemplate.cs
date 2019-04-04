namespace Data.EF
{
    using System.Data.Entity.Migrations;

    public partial class addRentalOSImportHistoryTemplate : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/Add_RentalOSImportHistoryTemplates.sql");
        }

        public override void Down()
        {
            this.Sql(@"DELETE
                       FROM [CorpProp.Settings].[UserNotificationTemplate]
                       WHERE [Code] IN (N'RentalOSImportHistory_Success', N'RentalOSImportHistory_Fail')
                    ");
        }
    }
}