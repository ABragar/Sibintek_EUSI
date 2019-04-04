namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeEstateTaxes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("[CorpProp.Estate].EstateTaxes", "Benefit", c => c.Boolean());
            AlterColumn("[CorpProp.Estate].EstateTaxes", "BenefitApplyForEnergy", c => c.Boolean());
            AlterColumn("[CorpProp.Estate].EstateTaxes", "BenefitDocsExist", c => c.Boolean());
            AlterColumn("[CorpProp.Estate].EstateTaxes", "IsEnergy", c => c.Boolean());
            AlterColumn("[CorpProp.Estate].EstateTaxes", "IsInvestmentProgramm", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("[CorpProp.Estate].EstateTaxes", "IsInvestmentProgramm", c => c.Boolean(nullable: false));
            AlterColumn("[CorpProp.Estate].EstateTaxes", "IsEnergy", c => c.Boolean(nullable: false));
            AlterColumn("[CorpProp.Estate].EstateTaxes", "BenefitDocsExist", c => c.Boolean(nullable: false));
            AlterColumn("[CorpProp.Estate].EstateTaxes", "BenefitApplyForEnergy", c => c.Boolean(nullable: false));
            AlterColumn("[CorpProp.Estate].EstateTaxes", "Benefit", c => c.Boolean(nullable: false));
        }
    }
}
