namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeERRow : DbMigration
    {
        public override void Up()
        {
            DropColumn("[EUSI.Estate].EstateRegistrationRow", "ObjectsCount");
        }
        
        public override void Down()
        {
            AddColumn("[EUSI.Estate].EstateRegistrationRow", "ObjectsCount", c => c.Int());
        }
    }
}
