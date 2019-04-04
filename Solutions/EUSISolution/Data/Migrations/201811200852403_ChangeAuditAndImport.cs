namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAuditAndImport : DbMigration
    {
        public override void Up()
        {
            AddColumn("Audit.AuditItem", "UserLogin", c => c.String());
            AddColumn("[CorpProp.Import].ImportErrorLog", "EusiNumber", c => c.String());
            AddColumn("[CorpProp.Import].ImportErrorLog", "InventoryNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("[CorpProp.Import].ImportErrorLog", "InventoryNumber");
            DropColumn("[CorpProp.Import].ImportErrorLog", "EusiNumber");
            DropColumn("Audit.AuditItem", "UserLogin");
        }
    }
}
