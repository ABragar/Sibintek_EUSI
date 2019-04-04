namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCalcRecord : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.Accounting].CalculatingRecord", "Oid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("[EUSI.Accounting].CalculatingRecord", "Oid");
        }
    }
}
