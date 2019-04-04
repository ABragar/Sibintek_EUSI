namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAddonOKOFs : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.NSI].AddonOKOF", "OKOF94ID", c => c.Int());
            AddColumn("[EUSI.NSI].AddonOKOF2014", "OKOF2014ID", c => c.Int());
            CreateIndex("[CorpProp.NSI].AddonOKOF", "OKOF94ID");
            CreateIndex("[EUSI.NSI].AddonOKOF2014", "OKOF2014ID");
            AddForeignKey("[CorpProp.NSI].AddonOKOF", "OKOF94ID", "[CorpProp.NSI].OKOF94", "ID");
            AddForeignKey("[EUSI.NSI].AddonOKOF2014", "OKOF2014ID", "[CorpProp.NSI].OKOF2014", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.NSI].AddonOKOF2014", "OKOF2014ID", "[CorpProp.NSI].OKOF2014");
            DropForeignKey("[CorpProp.NSI].AddonOKOF", "OKOF94ID", "[CorpProp.NSI].OKOF94");
            DropIndex("[EUSI.NSI].AddonOKOF2014", new[] { "OKOF2014ID" });
            DropIndex("[CorpProp.NSI].AddonOKOF", new[] { "OKOF94ID" });
            DropColumn("[EUSI.NSI].AddonOKOF2014", "OKOF2014ID");
            DropColumn("[CorpProp.NSI].AddonOKOF", "OKOF94ID");
        }
    }
}
