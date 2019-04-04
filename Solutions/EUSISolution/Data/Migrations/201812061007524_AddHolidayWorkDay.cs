namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHolidayWorkDay : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "[EUSI.NSI].HolidayWorkDay",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
                .Index(t => t.ID);

            this.Sql(@"
                INSERT INTO [CorpProp.NSI].[NSI](   
                       [Name]
                      ,[NSITypeID]     
                      ,[Mnemonic]     
                      ,[Oid]
                      ,[IsHistory]
                      ,[CreateDate]
                      ,[ActualDate]
                      ,[Hidden]
                      ,[SortOrder])
                VALUES (
                 N'Выходные и рабочие дни'
                , (SELECT TOP 1 nt.ID
                  FROM [CorpProp.NSI].[NSIType] nt
                  LEFT JOIN [CorpProp.Base].[DictObject] dd ON nt.ID = dd.ID
                  WHERE dd.Code = N'Local')
                , N'HolidayWorkDayMenu'
                , NEWID()
                , 0
                , GETDATE()
                , GETDATE()
                , 0
                , -1)
                ");
            
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.NSI].HolidayWorkDay", "ID", "[CorpProp.Base].DictObject");
            DropIndex("[EUSI.NSI].HolidayWorkDay", new[] { "ID" });
            DropTable("[EUSI.NSI].HolidayWorkDay");

            this.Sql(@" DELETE FROM [CorpProp.NSI].[NSI]
                        WHERE [Mnemonic] = N'HolidayWorkDayMenu'");
        }
    }
}
