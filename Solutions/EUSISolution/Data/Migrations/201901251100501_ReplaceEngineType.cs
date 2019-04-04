namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReplaceEngineType : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "[EUSI.NSI].EngineType", newSchema: "CorpProp.NSI");
            AddColumn("[CorpProp.Estate].Vehicle", "EngineTypeID", c => c.Int());
            CreateIndex("[CorpProp.Estate].Vehicle", "EngineTypeID");
            AddForeignKey("[CorpProp.Estate].Vehicle", "EngineTypeID", "[CorpProp.NSI].EngineType", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("[CorpProp.Estate].Vehicle", "EngineTypeID", "[CorpProp.NSI].EngineType");
            DropIndex("[CorpProp.Estate].Vehicle", new[] { "EngineTypeID" });
            DropColumn("[CorpProp.Estate].Vehicle", "EngineTypeID");
            MoveTable(name: "[CorpProp.NSI].EngineType", newSchema: "EUSI.NSI");
        }
    }
}
