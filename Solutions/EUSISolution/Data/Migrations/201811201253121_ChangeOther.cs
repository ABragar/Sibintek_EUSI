namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOther : DbMigration
    {
        public override void Up()
        {
            AddColumn("[CorpProp.Accounting].AccountingObjectTbl", "IsArchived", c => c.Boolean());
            SqlFile("Migrations/Sql/AccountingObject_IsArchived_Up.sql");
        }

        public override void Down()
        {
            DropColumn("[CorpProp.Accounting].AccountingObjectTbl", "IsArchived");
            SqlFile("Migrations/Sql/AccountingObject_IsArchived_Down.sql");
        }
    }
}
