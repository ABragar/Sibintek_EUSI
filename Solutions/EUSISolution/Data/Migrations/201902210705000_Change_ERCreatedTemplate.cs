namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_ERCreatedTemplate : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/Change_ERCreatedTemplate_Up.sql");
        }

        public override void Down()
        {
            SqlFile("Migrations/SQL/Change_ERCreatedTemplate_Down.sql");
        }
    }
}
