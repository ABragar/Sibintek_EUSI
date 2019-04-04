namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMailSetting : DbMigration
    {
        public override void Up()
        {   
            AddColumn("Mail.MailSetting", "SmtpWithoutCredentials", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {           
            DropColumn("Mail.MailSetting", "SmtpWithoutCredentials");            
        }
    }
}
