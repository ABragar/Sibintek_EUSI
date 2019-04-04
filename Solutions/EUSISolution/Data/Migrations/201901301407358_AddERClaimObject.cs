namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddERClaimObject : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.Estate].EstateRegistration", "ClaimObjectID", c => c.Int());
            CreateIndex("[EUSI.Estate].EstateRegistration", "ClaimObjectID");
            AddForeignKey("[EUSI.Estate].EstateRegistration", "ClaimObjectID", "[EUSI.Estate].EstateRegistrationRow", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.Estate].EstateRegistration", "ClaimObjectID", "[EUSI.Estate].EstateRegistrationRow");
            DropIndex("[EUSI.Estate].EstateRegistration", new[] { "ClaimObjectID" });
            DropColumn("[EUSI.Estate].EstateRegistration", "ClaimObjectID");
        }
    }
}
