namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameTemplateIH : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "[CorpProp.Import].ImportHistory", name: "TempateName", newName: "TemplateName");           
            AddColumn("[EUSI.Accounting].AccountingMoving", "FileCardLink", c => c.String());
            
        }
        
        public override void Down()
        {
            RenameColumn(table: "[CorpProp.Import].ImportHistory", name: "TemplateName", newName: "TempateName");
            DropColumn("[EUSI.Accounting].AccountingMoving", "FileCardLink");
            
        }
    }
}
