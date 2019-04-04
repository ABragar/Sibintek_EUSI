namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDecimals : DbMigration
    {
        public override void Up()
        {
            AlterColumn("[CorpProp.Subject].Society", "ShareInEquity", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            AlterColumn("[CorpProp.Subject].Society", "ShareInVotingRights", c => c.Decimal(nullable: false, precision: 18, scale: 8));
            AlterColumn("[CorpProp.CorporateGovernance].Investment", "NumberShares", c => c.Decimal(nullable: false, precision: 28, scale: 4));
            AlterColumn("[CorpProp.CorporateGovernance].Investment", "NominalShares", c => c.Decimal(nullable: false, precision: 31, scale: 8));
            AlterColumn("[CorpProp.CorporateGovernance].Investment", "SharePrice", c => c.Decimal(nullable: false, precision: 31, scale: 8));
            AlterColumn("[CorpProp.CorporateGovernance].Shareholder", "ShareMarket", c => c.Decimal(precision: 18, scale: 10));
            AlterColumn("[CorpProp.CorporateGovernance].Shareholder", "ShareMarketInvest", c => c.Decimal(precision: 18, scale: 10));
            AlterColumn("[CorpProp.CorporateGovernance].Shareholder", "NumberOrdinaryShares", c => c.Decimal(precision: 28, scale: 4));
            AlterColumn("[CorpProp.CorporateGovernance].Shareholder", "CostNominalOrdinaryShares", c => c.Decimal(precision: 31, scale: 8));
            AlterColumn("[CorpProp.CorporateGovernance].Shareholder", "CostActualOrdinaryShares", c => c.Decimal(precision: 31, scale: 8));
            AlterColumn("[CorpProp.CorporateGovernance].Shareholder", "CostNominalPreferredShares", c => c.Decimal(precision: 31, scale: 8));
            AlterColumn("[CorpProp.CorporateGovernance].Shareholder", "CostActualPreferredShares", c => c.Decimal(precision: 31, scale: 8));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK1", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK2", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorK3", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv1", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv2", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKv3", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKl", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKl1", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKl2", c => c.Decimal(precision: 18, scale: 4));
            AlterColumn("[EUSI.Accounting].AccountingCalculatedField", "FactorKl3", c => c.Decimal(precision: 18, scale: 4));
        }
        
        public override void Down()
        {

        }
    }
}
