namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstAddonOKOF2014 : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "[EUSI.NSI].AddonOKOF2014", newSchema: "CorpProp.NSI");
            AddColumn("[CorpProp.Estate].Estate", "AddonOKOF2014ID", c => c.Int());
            CreateIndex("[CorpProp.Estate].Estate", "AddonOKOF2014ID");
            AddForeignKey("[CorpProp.Estate].Estate", "AddonOKOF2014ID", "[CorpProp.NSI].AddonOKOF2014", "ID");
            //актуализация SQL- процедур
            SqlFile("Migrations/SQL/EstAddonOKOF2014_Up.sql");

        }

        public override void Down()
        {
            DropForeignKey("[CorpProp.Estate].Estate", "AddonOKOF2014ID", "[CorpProp.NSI].AddonOKOF2014");
            DropIndex("[CorpProp.Estate].Estate", new[] { "AddonOKOF2014ID" });
            DropColumn("[CorpProp.Estate].Estate", "AddonOKOF2014ID");
            MoveTable(name: "[CorpProp.NSI].AddonOKOF2014", newSchema: "EUSI.NSI");
        }
    }
}
