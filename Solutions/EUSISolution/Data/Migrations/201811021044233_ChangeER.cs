namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeER : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Settings].UserNotificationTemplate", "Recipient", c => c.String());
            AddColumn("[EUSI.Estate].EstateRegistration", "QuickClose", c => c.Boolean(nullable: false, defaultValue : false));
            AddColumn("[EUSI.Estate].EstateRegistrationRow", "Position", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("[EUSI.Estate].EstateRegistration", "QuickClose");
            DropColumn("[CorpProp.Settings].UserNotificationTemplate", "Recipient");
            DropColumn("[EUSI.Estate].EstateRegistrationRow", "Position");
        }

        
    }
}
