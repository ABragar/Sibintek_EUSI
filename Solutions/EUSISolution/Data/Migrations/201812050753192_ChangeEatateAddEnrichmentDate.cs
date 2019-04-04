namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEatateAddEnrichmentDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Estate].Estate", "EnrichmentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("[CorpProp.Estate].Estate", "EnrichmentDate");
        }
    }
}
