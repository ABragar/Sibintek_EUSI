namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEstateInArchive : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Estate].Estate", "IsArchived", c => c.Boolean());
            AddColumn("[CorpProp.Estate].Estate", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("[CorpProp.Estate].Estate", "Comment");
            DropColumn("[CorpProp.Estate].Estate", "IsArchived");
        }
    }
}
