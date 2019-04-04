namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveBEExcel : DbMigration
    {
        public override void Up()
        {
            DropColumn("[CorpProp.Import].ImportErrorLog", "ExcelBE");
        }
        
        public override void Down()
        {
            AddColumn("[CorpProp.Import].ImportErrorLog", "ExcelBE", c => c.String());
        }
    }
}
