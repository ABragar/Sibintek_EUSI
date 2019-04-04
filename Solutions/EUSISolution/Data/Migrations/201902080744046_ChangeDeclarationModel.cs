namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDeclarationModel : DbMigration
    {
        public override void Up()
        {           
            AddColumn("[EUSI.NU].Declaration", "CorrectionNumbInt", c => c.Int());
            Sql(@"
                    UPDATE [EUSI.NU].Declaration
                    SET [CorrectionNumbInt] = CAST(ISNULL([CorrectionNumb],N'0') AS INT)
            ");
            DropColumn("[EUSI.NU].Declaration", "CorrectionNumb");
            AddColumn("[EUSI.NU].Declaration", "CorrectionNumb", c => c.Int());
            Sql(@"
                    UPDATE [EUSI.NU].Declaration
                    SET [CorrectionNumb] = [CorrectionNumbInt]
            ");
            DropColumn("[EUSI.NU].Declaration", "CorrectionNumbInt");
        }
        
        public override void Down()
        {            
            AddColumn("[EUSI.NU].Declaration", "CorrectionNumbStr", c => c.String());
            Sql(@"
                    UPDATE [EUSI.NU].Declaration
                    SET [CorrectionNumbStr] = CAST(ISNULL([CorrectionNumb],0) AS NVARCHAR(MAX))
            ");
            DropColumn("[EUSI.NU].Declaration", "CorrectionNumb");
            AddColumn("[EUSI.NU].Declaration", "CorrectionNumb", c => c.String());
            Sql(@"
                    UPDATE [EUSI.NU].Declaration
                    SET [CorrectionNumb] = [CorrectionNumbStr]
            ");
            DropColumn("[EUSI.NU].Declaration", "CorrectionNumbStr");
        }
    }
}
