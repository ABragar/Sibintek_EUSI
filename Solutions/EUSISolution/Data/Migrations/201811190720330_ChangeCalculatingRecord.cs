namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCalculatingRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.Accounting].CalculatingRecord", "PeriodCalculatedNU", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("[EUSI.Accounting].CalculatingRecord", "PeriodCalculatedNU");
        }
    }
}
