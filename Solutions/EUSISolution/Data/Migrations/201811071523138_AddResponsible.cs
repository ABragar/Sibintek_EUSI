namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResponsible : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "[EUSI.NSI].Responsible",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        FIO = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        ConsolidationID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
                .ForeignKey("[CorpProp.NSI].Consolidation", t => t.ConsolidationID)
                .Index(t => t.ID)
                .Index(t => t.ConsolidationID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.NSI].Responsible", "ConsolidationID", "[CorpProp.NSI].Consolidation");
            DropForeignKey("[EUSI.NSI].Responsible", "ID", "[CorpProp.Base].DictObject");
            DropIndex("[EUSI.NSI].Responsible", new[] { "ConsolidationID" });
            DropIndex("[EUSI.NSI].Responsible", new[] { "ID" });
            DropTable("[EUSI.NSI].Responsible");
        }
    }
}
