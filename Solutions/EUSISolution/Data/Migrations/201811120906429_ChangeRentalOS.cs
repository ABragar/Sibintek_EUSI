namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRentalOS : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "[EUSI.Accounting].RentalOS", name: "CurrencyID", newName: "CurrencyID");
            RenameIndex(table: "[EUSI.Accounting].RentalOS", name: "IX_CurrencyID", newName: "IX_CurrencyID");
            AddColumn("[EUSI.Accounting].RentalOS", "AccountingObjectID", c => c.Int());
            CreateIndex("[EUSI.Accounting].RentalOS", "AccountingObjectID");
            AddForeignKey("[EUSI.Accounting].RentalOS", "AccountingObjectID", "[CorpProp.Accounting].AccountingObjectTbl", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.Accounting].RentalOS", "AccountingObjectID", "[CorpProp.Accounting].AccountingObjectTbl");
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "AccountingObjectID" });
            DropColumn("[EUSI.Accounting].RentalOS", "AccountingObjectID");
            RenameIndex(table: "[EUSI.Accounting].RentalOS", name: "IX_CurrencyID", newName: "IX_CurrencyID");
            RenameColumn(table: "[EUSI.Accounting].RentalOS", name: "CurrencyID", newName: "CurrencyID");
        }
    }
}
