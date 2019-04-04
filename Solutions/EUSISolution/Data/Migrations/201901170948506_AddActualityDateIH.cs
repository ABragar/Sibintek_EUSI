namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActualityDateIH : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Import].ImportHistory", "ActualityDate", c => c.DateTime());
            AddColumn("[CorpProp.Import].ImportHistory", "CurrentFileUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("[CorpProp.Import].ImportHistory", "CurrentFileUser");
            DropColumn("[CorpProp.Import].ImportHistory", "ActualityDate");
        }
    }
}
