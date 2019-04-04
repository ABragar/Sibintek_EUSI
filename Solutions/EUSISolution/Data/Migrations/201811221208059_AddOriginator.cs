namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOriginator : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.NSI].EstateRegistrationOriginator", "ContactEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("[EUSI.NSI].EstateRegistrationOriginator", "ContactEmail");
        }
    }
}
