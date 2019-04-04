namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResponsibleAddZoneResponsibility : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.NSI].Responsible", "ZoneResponsibilityID", c => c.Int());
            CreateIndex("[EUSI.NSI].Responsible", "ZoneResponsibilityID");
            AddForeignKey("[EUSI.NSI].Responsible", "ZoneResponsibilityID", "[EUSI.NSI].ZoneResponsibility", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.NSI].Responsible", "ZoneResponsibilityID", "[EUSI.NSI].ZoneResponsibility");
            DropIndex("[EUSI.NSI].Responsible", new[] { "ZoneResponsibilityID" });
            DropColumn("[EUSI.NSI].Responsible", "ZoneResponsibilityID");
        }
    }
}
