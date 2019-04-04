namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_ERComplitedTemplate : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/Change_ERComplitedTemplate_Up.sql");
        }

        public override void Down()
        {
            SqlFile("Migrations/SQL/Change_ERComplitedTemplate_Down.sql");
        }
    }
}
