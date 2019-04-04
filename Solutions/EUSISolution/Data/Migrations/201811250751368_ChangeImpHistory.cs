namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeImpHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Import].ImportErrorLog", "ExcelBE", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("[CorpProp.Import].ImportErrorLog", "ExcelBE");
        }
    }
}
