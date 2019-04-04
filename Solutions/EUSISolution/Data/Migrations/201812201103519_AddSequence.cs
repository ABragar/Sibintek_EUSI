namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSequence : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/AddSequence_Up.sql");
        }

        public override void Down()
        {
            SqlFile("Migrations/SQL/AddSequence_Down.sql");
        }
    }
}