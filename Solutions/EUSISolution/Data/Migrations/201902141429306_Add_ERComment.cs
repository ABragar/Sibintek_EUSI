namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ERComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.Estate].EstateRegistrationRow", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("[EUSI.Estate].EstateRegistrationRow", "Comment");
        }
    }
}
