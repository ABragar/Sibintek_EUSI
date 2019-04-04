namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRightRegEndDate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "[CorpProp.Estate].Cadastral", name: "RighRegDate", newName: "RightRegDate");
            AddColumn("[CorpProp.Estate].Cadastral", "RightRegEndDate", c => c.DateTime());
            AddColumn("[CorpProp.Accounting].AccountingObjectTbl", "RightRegEndDate", c => c.DateTime());
            SqlFile("Migrations/SQL/AddRightRegEndDate_Up.sql");
        }
        
        public override void Down()
        {            
            DropColumn("[CorpProp.Accounting].AccountingObjectTbl", "RightRegEndDate");
            DropColumn("[CorpProp.Estate].Cadastral", "RightRegEndDate");           
            RenameColumn(table: "[CorpProp.Estate].Cadastral", name: "RightRegDate", newName: "RighRegDate");
            SqlFile("Migrations/SQL/AddRightRegEndDate_Down.sql");
        }
    }
}
