namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMonitorControlVer1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Import].ImportHistory", "ConsolidationID", c => c.Int());
            AddColumn("[EUSI.Report].ReportMonitoring", "Mnemonic", c => c.String());
            CreateIndex("[CorpProp.Import].ImportHistory", "ConsolidationID");
            AddForeignKey("[CorpProp.Import].ImportHistory", "ConsolidationID", "[CorpProp.NSI].Consolidation", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("[CorpProp.Import].ImportHistory", "ConsolidationID", "[CorpProp.NSI].Consolidation");
            DropIndex("[CorpProp.Import].ImportHistory", new[] { "ConsolidationID" });
            DropColumn("[EUSI.Report].ReportMonitoring", "Mnemonic");
            DropColumn("[CorpProp.Import].ImportHistory", "ConsolidationID");
        }
    }
}
