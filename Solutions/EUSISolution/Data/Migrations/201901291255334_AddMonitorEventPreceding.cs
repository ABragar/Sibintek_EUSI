namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMonitorEventPreceding : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "[EUSI.ManyToMany].MonitorEventPreceding",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ObjLeftId = c.Int(nullable: false),
                        ObjRigthId = c.Int(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                        SortOrder = c.Double(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[EUSI.NSI].ReportMonitoringEventType", t => t.ObjLeftId)
                .ForeignKey("[EUSI.NSI].ReportMonitoringEventType", t => t.ObjRigthId)
                .Index(t => t.ObjLeftId)
                .Index(t => t.ObjRigthId);

            SqlFile("Migrations/SQL/AddMonitorEventPreceding.sql");
        }
        
        public override void Down()
        {           

            Sql(@"
                IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = N'pReport_GetClosingMonitorList')
                DROP PROCEDURE [dbo].[pReport_GetClosingMonitorList]
                GO
                CREATE PROCEDURE [dbo].[pReport_GetClosingMonitorList]
                @vintConsolidationId INT,
                @vdatReportDate DATE
                AS
                /*DECLARE 
                @vintConsolidationId INT = 1,
                @vdatReportDate DATE ='10.23.2018'*/
                SELECT 
                SortIndex=RMET.SortIndex,
                ProcessStep= /*case when RMonitor.ReportName is not null then RMonitor.ReportName else*/ RMETDict.Name,-- end,
                ResponsibleOfExecution= isnull(UserBaseProfile.LastName,'') + ' ' + isnull(UserBaseProfile.FirstName,'') + ' ' + isnull(UserBaseProfile.MiddleName,'') , -- ФИО полное,
                IsPassed= case when RMonitor.IsValid is not null then RMonitor.IsValid else 'False' end, -- Контроль пройден
                Result = ResDict.[Name],
                [Version]=RMonitor.IterationIndex,
                DateOfPlan= DATEFROMPARTS( YEAR(@vdatReportDate), MONTH(@vdatReportDate),  RMET.PlanDayOfMonth),
                DateOfFact=RMonitor.ImportDateTime,
                Deviation=DATEDIFF(DAY,DATEFROMPARTS( YEAR(@vdatReportDate), MONTH(@vdatReportDate),  RMET.PlanDayOfMonth),RMonitor.ImportDateTime),
                Comment=RMonitor.Comment,
                EventId= RMonitor.ID,
                RMonitor.StartDate,
                RMonitor.EndDate
                FROM [CorpProp.NSI].Consolidation Cons
                INNER JOIN [CorpProp.Base].DictObject ConsolidationUnit ON Cons.ID=ConsolidationUnit.ID
                INNER JOIN [EUSI.ManyToMany].ConsolidationAndReportMonitoringEventType CARMET ON CARMET.ObjLeftId=Cons.ID AND isnull(CARMET.Hidden,0)=0
                LEFT JOIN [EUSI.NSI].ReportMonitoringEventType RMET ON RMET.ID=CARMET.ObjRigthId
                LEFT JOIN [CorpProp.Base].DictObject RMETDict ON RMET.ID=RMETDict.ID
                LEFT JOIN [EUSI.Report].ReportMonitoring RMonitor ON RMonitor.ReportMonitoringEventTypeID=RMET.ID AND RMonitor.ConsolidationID=Cons.ID AND @vdatReportDate BETWEEN RMonitor.StartDate AND RMonitor.EndDate
                LEFT JOIN [CorpProp.Base].DictObject ResDict ON RMonitor.ReportMonitoringResultID = ResDict.ID
                LEFT JOIN [CorpProp.Security].[SibUser] as UserProfile ON UserProfile.ID=RMonitor.SibUserID
                LEFT JOIN [Security].[BaseProfile] as UserBaseProfile ON UserBaseProfile.ID=UserProfile.ID
                LEFT JOIN [Security].[User] as UserSecurity ON UserSecurity.ID=UserProfile.UserID 
                WHERE Cons.ID=@vintConsolidationId and isnull(RMonitor.Hidden,0)<>1 and isnull(RMETDict.Hidden,0)<>1
                order by RMET.SortIndex 
                RETURN 0
            ");

            DropForeignKey("[EUSI.ManyToMany].MonitorEventPreceding", "ObjRigthId", "[EUSI.NSI].ReportMonitoringEventType");
            DropForeignKey("[EUSI.ManyToMany].MonitorEventPreceding", "ObjLeftId", "[EUSI.NSI].ReportMonitoringEventType");
            DropIndex("[EUSI.ManyToMany].MonitorEventPreceding", new[] { "ObjRigthId" });
            DropIndex("[EUSI.ManyToMany].MonitorEventPreceding", new[] { "ObjLeftId" });
            DropTable("[EUSI.ManyToMany].MonitorEventPreceding");
        }
    }
}
