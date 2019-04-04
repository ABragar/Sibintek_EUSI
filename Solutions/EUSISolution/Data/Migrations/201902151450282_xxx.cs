namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xxx : DbMigration
    {
        public override void Up()
        {
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK1", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK2", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK3", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv1", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv2", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv3", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv3", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv2", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv1", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK3", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK2", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK1", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK", c => c.Decimal(precision: 18, scale: 4));
        }
    }
}
