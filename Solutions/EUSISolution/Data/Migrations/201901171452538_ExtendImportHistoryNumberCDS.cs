namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ExtendImportHistoryNumberCDS : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Import].ImportHistory", "NumberCDS", c => c.String());
            SqlFile("Migrations/SQL/Add_EstateRegistrationImportHistoryTemplate.sql");
        }

        public override void Down()
        {
            DropColumn("[CorpProp.Import].ImportHistory", "NumberCDS");
            this.Sql(@"DELETE
                       FROM [CorpProp.Settings].[UserNotificationTemplate]
                       WHERE [Code] IN (N'EstateRegistrationImportHistory_Fail')
                    ");
        }
    }
}