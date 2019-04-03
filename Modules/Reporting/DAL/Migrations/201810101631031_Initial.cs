namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            string strDBO_SCHEMA = "dbo";
            string strSCHEMA_Reports = "ReportService";

            string strDate = String.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string newNameReportHistories = string.Format("{0}_{1}", "ReportHistories", strDate);
            string newNamePKReportHistories = string.Format("{0}_{1}", "PK_ReportService.ReportHistories", strDate);
            string newNameReports = string.Format("{0}_{1}", "Reports", strDate);
            string newNamePKReports = string.Format("{0}_{1}", "PK_ReportService.Reports", strDate);

            string strTBLReportHistories = "ReportHistories";
            string strCreate_ReportHistories = string.Format("{0}.{1}", strSCHEMA_Reports, strTBLReportHistories);
            string strTBLReports = "Reports";
            string strCreate_Reports = string.Format("{0}.{1}", strSCHEMA_Reports, strTBLReports);

            /*
            //Без предварительной проверки на присутствие таблиц
            Sql("ALTER TABLE[ReportService].[ReportHistories] DROP CONSTRAINT[FK_ReportService.ReportHistories_ReportService.Reports_ReportID]", true, null);
            Sql("ALTER TABLE[ReportService].[ReportHistories] DROP CONSTRAINT[PK_ReportService.ReportHistories]", true, null);
            Sql("DROP INDEX [IX_ReportID] ON [ReportService].[ReportHistories]", true, null);

            Sql("ALTER TABLE[ReportService].[Reports] DROP CONSTRAINT[PK_ReportService.Reports]", true, null);

            RenameTable("[ReportService].ReportHistories", newNameReportHistories, null);
            RenameTable("[ReportService].Reports", newNameReports, null);

            CreateIndex("[ReportService]." + newNameReports, "ID", true, newNamePKReports, true, null);
            AddPrimaryKey("[ReportService]." + newNameReportHistories, "ID", newNamePKReportHistories, true, null);
            AddForeignKey("[ReportService]." + newNameReportHistories, "ReportID", "[ReportService]." + newNameReports, "ID", cascadeDelete: false);
            */

            //Если таблица [ReportService].[ReportHistories] существовала, то отложим ее в историю
            Sql("DECLARE " +
                "@THETABLE varchar(100) " +
                ", @strDate varchar(100) " +
                ", @THETABLENewName varchar(100) " +
                ", @TABLE_SCHEMA  varchar(100) = 'ReportService' " +
                ", @TABLE_NAME   varchar(100) = 'Reports' " +
                "SET @THETABLE = @TABLE_SCHEMA + '.' + @TABLE_NAME " +
                "set @THETABLENewName = '" + newNameReports + "' " +
                "IF(EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @TABLE_SCHEMA AND  TABLE_NAME = @TABLE_NAME)) " +
                "BEGIN " +
                    "IF(EXISTS(select i.* from sys.indexes i where i.object_id = OBJECT_ID(@THETABLE) and i.name is not NULL)) " +
                    "BEGIN " +
                        "if (exists(select distinct o.name as 'PK_Name', po.name as 'DependentTable' from sys.objects as o " +
                                    "left join  sys.objects as po on o.parent_object_id = po.object_id where o.parent_object_id = OBJECT_ID(@THETABLE) and o.type = 'PK')) " +
                                "BEGIN " +
                                    "ALTER TABLE ReportService.ReportHistories	DROP CONSTRAINT [FK_ReportService.ReportHistories_ReportService.Reports_ReportID] " +
                                    "ALTER TABLE ReportService.Reports   DROP CONSTRAINT[PK_ReportService.Reports] " +
                        "END " +
                    "END " +
                    "exec sp_rename '[ReportService].[Reports]', '" + newNameReports + "' " +
                "END");
            //Если таблица [ReportService].[Reports] существовала, то отложим ее в историю
            Sql("DECLARE " +
                "@THETABLE varchar(100) " +
                ", @strDate varchar(100) " +
                ", @THETABLENewName varchar(100) " +
                ", @TABLE_SCHEMA varchar(100) = 'ReportService' " +
                ", @TABLE_NAME varchar(100) = 'ReportHistories' " +
                ", @Principal_TABLE_NAME varchar(100) = 'Reports' " +
                "SET @THETABLE = @TABLE_SCHEMA + '.' + @TABLE_NAME " +
                "set @THETABLENewName = '" + newNameReportHistories + "' " +
                "IF(EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @TABLE_SCHEMA AND  TABLE_NAME = @TABLE_NAME)) " +
                "BEGIN " +
                    "IF(EXISTS(select i.* from sys.indexes i where i.object_id = OBJECT_ID(@THETABLE) and i.name is not NULL)) " +
                    "BEGIN " +
                        "if (exists(select distinct cr.name as 'PrincipalTable', c.name as 'FK_Name' from sys.foreign_keys as c " +
                                    "left join sys.objects cr on c.referenced_object_id = cr.object_id " +
                                    "where c.parent_object_id = OBJECT_ID(@THETABLE) and cr.name = @Principal_TABLE_NAME)) " +
                                "BEGIN " +
                                    "ALTER TABLE[ReportService].[ReportHistories] DROP CONSTRAINT[FK_ReportService.ReportHistories_ReportService.Reports_ReportID] " +
                        "END " +
                        "if (exists(select distinct o.name as 'PK_Name', po.name as 'DependentTable' from sys.objects as o " +
                                    "left join  sys.objects as po on o.parent_object_id = po.object_id where o.parent_object_id = OBJECT_ID(@THETABLE) and o.type = 'PK')) " +
                        "BEGIN " +
                            "DROP INDEX IX_ReportID ON ReportService.ReportHistories " +
                            "ALTER TABLE ReportService.ReportHistories DROP CONSTRAINT[PK_ReportService.ReportHistories] " +
                        "END " +
                    "END " +
                "exec sp_rename '[ReportService].[ReportHistories]', '" + newNameReportHistories + "' " +
                "END");

            //Добавляем ключи для исторических таблиц 
            Sql(
                "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + strSCHEMA_Reports + "' AND  TABLE_NAME = '" + newNameReportHistories + "') and EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + strSCHEMA_Reports + "' AND  TABLE_NAME = '" + newNameReports + "')) " +
                "BEGIN " +
                "ALTER TABLE [ReportService].[" + newNameReports + "] ADD  CONSTRAINT [PK_ReportService." + newNameReports + "] PRIMARY KEY CLUSTERED ([ID] ASC) ON [PRIMARY] " +
                "ALTER TABLE[ReportService].[" + newNameReportHistories + "] ADD  CONSTRAINT[PK_ReportService." + newNameReportHistories + "] PRIMARY KEY CLUSTERED([ID] ASC) ON[PRIMARY] " +
                "ALTER TABLE[ReportService].[" + newNameReportHistories + "] WITH CHECK ADD CONSTRAINT[FK_ReportService." + newNameReportHistories + "_ReportService." + newNameReports + "_ReportID] FOREIGN KEY([ReportID]) " +
                "REFERENCES[ReportService].[" + newNameReports + "] ([ID]) ON DELETE CASCADE " +
                "ALTER TABLE[ReportService].[" + newNameReportHistories + "] CHECK CONSTRAINT[FK_ReportService." + newNameReportHistories + "_ReportService." + newNameReports + "_ReportID]" +
                "END"
            );

            //Создание новой структуры
            //Создаем таблицу ReportHistories
            CreateTable(
                strCreate_ReportHistories,
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    CreatorID = c.Int(nullable: false),
                    ReportID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey(strCreate_Reports, t => t.ReportID, cascadeDelete: true)
                .Index(t => t.ReportID);
            //Создаем таблицу Reports
            CreateTable(
                strCreate_Reports,
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    GuidId = c.Guid(nullable: false),
                    Name = c.String(),
                    Extension = c.String(),
                    Description = c.String(),
                    UserCategories = c.String(),
                    Code = c.String(),
                    Params = c.String(),
                    Hidden = c.Boolean(),
                    RelativePath = c.String(),
                    ReportType = c.String(),
                    Module = c.String(),
                    Number = c.String(),
                    ReportPublishCode = c.String(),
                    ReportVersion = c.String(),
                })
                .PrimaryKey(t => t.ID);

            //Наполняем ReportService.Reports из историических таблиц
            //из dbo.Reports
            Sql(
                "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + strSCHEMA_Reports + "' AND  TABLE_NAME = '" + newNameReports + "') and EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + strDBO_SCHEMA + "' AND  TABLE_NAME = '" + strTBLReports + "')) " +
                "BEGIN " +
                    "INSERT INTO [" + strSCHEMA_Reports + "].[" + newNameReports + "] (GuidId, Name, Extension, UserCategories, Description, Code, Params, Hidden, RelativePath, ReportType, Module, Number, ReportPublishCode, ReportVersion) " +
                    "SELECT GuidId, Name, Extension, UserCategories, Description, Code, Params, Hidden, RelativePath, ReportType, Module, Number, ReportPublishCode, ReportVersion FROM [" + strDBO_SCHEMA + "]." + strTBLReports + " " +
                "END");
            //ReportService.Reports_--Дата выполнения запуска службы--
            Sql(
                "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + strSCHEMA_Reports + "' AND  TABLE_NAME = '" + newNameReports + "') and EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '" + strSCHEMA_Reports + "' AND  TABLE_NAME = '" + strTBLReports + "')) " +
                "BEGIN " +
                    "INSERT INTO [" + strSCHEMA_Reports + "].[" + strTBLReports + "] (GuidId, Name, Extension, UserCategories, Description, Code, Params, Hidden, RelativePath, ReportType, Module, Number, ReportPublishCode, ReportVersion) " +
                    "SELECT GuidId, Name, Extension, UserCategories, Description, Code, Params, Hidden, RelativePath, ReportType, Module, Number, ReportPublishCode, ReportVersion FROM [" + strSCHEMA_Reports + "]." + newNameReports + " " +
                "END");


#if DEBUG
            /* // Uncomment this block if start to test windows auth on empty DB
            CreateTable(
                "[Security].[User]",
                p => new {
                    ID = p.Int(false, true),
                    CategoryID = p.Int(),
                    SysName = p.String()
                }).PrimaryKey(t => t.ID);
            CreateTable(
                "[dbo].[RoleUserCategories]",
                p => new {
                    ID = p.Int(false, true),
                    UserCategory_ID = p.Int(),
                    Role_ID = p.Int()
                }).PrimaryKey(t => t.ID);
            CreateTable(
                "[Security].[Role]",
                p => new {
                    ID = p.Int(false, true),
                    SystemRole = p.Int()
                }).PrimaryKey(t => t.ID);

            Sql("INSERT INTO [Security].[User] VALUES (NULL,N'SharovAV')");
*/
#endif
            //Откладываем в историю процедуру pGetUserInfo (если существует)
            string strSP = "pGetUserInfo";
            string strSPRename = string.Format("{0}_{1}", strSP, strDate);
            Sql(
                $"IF(EXISTS (" +
                $"SELECT * FROM sys.objects as o "+
                $"left join sys.schemas as s on o.schema_id = s.schema_id "+
                $"WHERE o.[name] = N'{strSP}' AND type in (N'P', N'PC') and s.name=N'ReportService'" +
                $") AND NOT EXISTS (" +
                $"SELECT * FROM sys.objects as o " +
                $"left join sys.schemas as s on o.schema_id = s.schema_id " +
                $"WHERE o.[name] = N'{strSPRename}' AND type in (N'P', N'PC') and s.name=N'ReportService'" +
                $"))" +
                $"BEGIN EXEC sp_rename 'ReportService.{strSP}', '{strSPRename}' END"
                );
            //Создаем процедуру pGetUserInfo
            CreateStoredProcedure("ReportService." + strSP, p => new { Login = p.String(255, true) },
                "SELECT" +
                "   IsAdmin= CONVERT(bit,CASE WHEN MIN(r.SystemRole)=0 THEN 1 ELSE 0 END)" +
                "   ,UserId = u.ID" +
                "   ,CategoryIds = CONVERT(nvarchar(MAX),u.CategoryID)" +
                " FROM [Security].[User] u" +
                " LEFT JOIN [dbo].[RoleUserCategories] ON [RoleUserCategories].UserCategory_ID=u.CategoryID" +
                " LEFT JOIN [Security].[Role] r ON r.ID=[RoleUserCategories].Role_ID" +
                " WHERE u.SysName=@Login" + 
                " GROUP BY u.ID, u.SysName, u.CategoryID"
            );


        }

        public override void Down()
        {
            DropForeignKey("ReportService.ReportHistories", "ReportID", "ReportService.Reports");
            DropIndex("ReportService.ReportHistories", new[] { "ReportID" });
            DropTable("ReportService.Reports");
            DropTable("ReportService.ReportHistories");
            DropStoredProcedure("ReportService.pGetUserInfo");
        }
    }
}
