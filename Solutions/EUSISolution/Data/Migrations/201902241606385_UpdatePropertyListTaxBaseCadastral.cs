namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePropertyListTaxBaseCadastral : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.NSI].PropertyListTaxBaseCadastral", "IsCadastralEstateUpdated", c => c.Boolean());
            AddColumn("[EUSI.NSI].PropertyListTaxBaseCadastral", "CadastralEstateUpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("[EUSI.NSI].PropertyListTaxBaseCadastral", "CadastralEstateUpdatedDate");
            DropColumn("[EUSI.NSI].PropertyListTaxBaseCadastral", "IsCadastralEstateUpdated");
        }
    }
}
