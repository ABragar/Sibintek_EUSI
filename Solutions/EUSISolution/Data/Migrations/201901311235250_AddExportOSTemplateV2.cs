namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExportOSTemplateV2 : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/AddExportOSTemplate_Up.sql");
        }
        
        public override void Down()
        {
            SqlFile("Migrations/SQL/AddExportOSTemplate_Down.sql");
        }
    }
}
