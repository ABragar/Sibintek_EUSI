namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNoticeTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Settings].UserNotification", "TemplateID", c => c.Int());
            CreateIndex("[CorpProp.Settings].UserNotification", "TemplateID");
            AddForeignKey("[CorpProp.Settings].UserNotification", "TemplateID", "[CorpProp.Settings].UserNotificationTemplate", "ID");
            SqlFile("Migrations/SQL/Add_DraftOSNotificationTemplate.sql");
        }
        
        public override void Down()
        {
            DropForeignKey("[CorpProp.Settings].UserNotification", "TemplateID", "[CorpProp.Settings].UserNotificationTemplate");
            DropIndex("[CorpProp.Settings].UserNotification", new[] { "TemplateID" });
            DropColumn("[CorpProp.Settings].UserNotification", "TemplateID");

            this.Sql(@"  DELETE
                         FROM [CorpProp.Settings].[UserNotificationTemplate]
                         WHERE [Code] IN (N'OS_DraftOSPassBuss_BUS', N'OS_DraftOSPassBuss_Originator')

                         DELETE 
                         FROM [CorpProp.Settings].[NotificationGroup]
                         WHERE [Name] = N'Контроли по данным БУ'
                    ");
        }
    }
}
