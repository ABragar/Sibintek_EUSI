namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableAuditForER : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/EnableAuditForER.sql");
        }
        
        public override void Down()
        {
        }
    }
}
