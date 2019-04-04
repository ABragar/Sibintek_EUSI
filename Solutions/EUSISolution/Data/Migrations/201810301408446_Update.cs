namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "[EUSI.NSI].ZoneResponsibility",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.NSI].ZoneResponsibility", "ID", "[CorpProp.Base].DictObject");
            DropIndex("[EUSI.NSI].ZoneResponsibility", new[] { "ID" });
            DropTable("[EUSI.NSI].ZoneResponsibility");
        }
    }
}
