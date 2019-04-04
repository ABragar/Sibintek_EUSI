namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportMonitoringResult : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "[EUSI.NSI].ReportMonitoringResult",
               c => new
               {
                   ID = c.Int(nullable: false),
               })
               .PrimaryKey(t => t.ID)
               .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
               .Index(t => t.ID);


            CreateTable(
                "[EUSI.ManyToMany].MonitorEventTypeAndResult",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsManualPick = c.Boolean(nullable: false, defaultValue: false),
                        ObjLeftId = c.Int(nullable: false),
                        ObjRigthId = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                        SortOrder = c.Double(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[EUSI.NSI].ReportMonitoringEventType", t => t.ObjLeftId)
                .ForeignKey("[EUSI.NSI].ReportMonitoringResult", t => t.ObjRigthId)
                .Index(t => t.ObjLeftId)
                .Index(t => t.ObjRigthId);

            AddColumn("[EUSI.Report].ReportMonitoring", "ReportMonitoringResultID", c => c.Int());
            CreateIndex("[EUSI.Report].ReportMonitoring", "ReportMonitoringResultID");
            AddForeignKey("[EUSI.Report].ReportMonitoring", "ReportMonitoringResultID", "[EUSI.NSI].ReportMonitoringResult", "ID");

            SqlFile("Migrations/SQL/AddReportMonitoringResult_Up.sql");

            DropColumn("[EUSI.Report].ReportMonitoring", "ResultText");
        }
        
        public override void Down()
        {
            AddColumn("[EUSI.Report].ReportMonitoring", "ResultText", c => c.String());
            DropForeignKey("[EUSI.NSI].ReportMonitoringResult", "ID", "[CorpProp.Base].DictObject");
            DropForeignKey("[EUSI.ManyToMany].MonitorEventTypeAndResult", "ObjRigthId", "[EUSI.NSI].ReportMonitoringResult");
            DropForeignKey("[EUSI.ManyToMany].MonitorEventTypeAndResult", "ObjLeftId", "[EUSI.NSI].ReportMonitoringEventType");
            DropForeignKey("[EUSI.Report].ReportMonitoring", "ReportMonitoringResultID", "[EUSI.NSI].ReportMonitoringResult");
            DropIndex("[EUSI.NSI].ReportMonitoringResult", new[] { "ID" });
            DropIndex("[EUSI.ManyToMany].MonitorEventTypeAndResult", new[] { "ObjRigthId" });
            DropIndex("[EUSI.ManyToMany].MonitorEventTypeAndResult", new[] { "ObjLeftId" });
            DropIndex("[EUSI.Report].ReportMonitoring", new[] { "ReportMonitoringResultID" });

            SqlFile("Migrations/SQL/AddReportMonitoringResult_Down.sql");

            DropColumn("[EUSI.Report].ReportMonitoring", "ReportMonitoringResultID");
            
            DropTable("[EUSI.ManyToMany].MonitorEventTypeAndResult");
            DropTable("[EUSI.NSI].ReportMonitoringResult");
        }
    }
}
