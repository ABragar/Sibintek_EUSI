namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddERControlDateAttributes : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/Sql/AddERControlDateAttributes_Up.sql");
        }
        
        public override void Down()
        {
           SqlFile("Migrations/Sql/AddERControlDateAttributes_Down.sql");
        }
    }
}
