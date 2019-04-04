namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnalyzeModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "[CorpProp.Analyze.Subject].AnalyzeSociety",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OwnerID = c.Int(),
                        SocietyStatus = c.String(),
                        AvailabilityOfLicenses = c.String(),
                        NetAssets = c.Decimal(precision: 18, scale: 2),
                        NumberOfEmployeesByStaffingSchedule = c.Int(),
                        PresenceOfSafetyOfOfficialDocuments = c.String(),
                        LeadingShareholderRegister = c.String(),
                        MaintainingTheAccountingFunctionInTheSSC = c.String(),
                        AuditorOfCompany = c.String(),
                        BoardOfDirectors = c.String(),
                        CompositionOfManagementBoard = c.String(),
                        IsJoint = c.Boolean(),
                        Oid = c.Guid(nullable: false),
                        IsHistory = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(),
                        ActualDate = c.DateTime(),
                        NonActualDate = c.DateTime(),
                        ImportUpdateDate = c.DateTime(),
                        ImportDate = c.DateTime(),
                        Hidden = c.Boolean(nullable: false),
                        SortOrder = c.Double(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Subject].Society", t => t.OwnerID)
                .Index(t => t.OwnerID);
            
            CreateTable(
                "[CorpProp.Analyze.Accounting].FinancialIndicatorItem",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OwnerID = c.Int(),
                        FinancialIndicatorID = c.Int(),
                        FinancialIndValue = c.Decimal(precision: 18, scale: 2),
                        Oid = c.Guid(nullable: false),
                        IsHistory = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(),
                        ActualDate = c.DateTime(),
                        NonActualDate = c.DateTime(),
                        ImportUpdateDate = c.DateTime(),
                        ImportDate = c.DateTime(),
                        Hidden = c.Boolean(nullable: false),
                        SortOrder = c.Double(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Analyze.NSI].FinancialIndicator", t => t.FinancialIndicatorID)
                .ForeignKey("[CorpProp.Subject].Society", t => t.OwnerID)
                .Index(t => t.OwnerID)
                .Index(t => t.FinancialIndicatorID);
            
            CreateTable(
                "[CorpProp.Analyze.Accounting].RecordBudgetLine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OwnerID = c.Int(),
                        BudgetLineID = c.Int(),
                        DateOfValue = c.DateTime(nullable: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Oid = c.Guid(nullable: false),
                        IsHistory = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(),
                        ActualDate = c.DateTime(),
                        NonActualDate = c.DateTime(),
                        ImportUpdateDate = c.DateTime(),
                        ImportDate = c.DateTime(),
                        Hidden = c.Boolean(nullable: false),
                        SortOrder = c.Double(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Analyze.NSI].BudgetLine", t => t.BudgetLineID)
                .ForeignKey("[CorpProp.Subject].Society", t => t.OwnerID)
                .Index(t => t.OwnerID)
                .Index(t => t.BudgetLineID);
            
            CreateTable(
                "[CorpProp.Analyze.Accounting].BankAccount",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SocietyID = c.Int(),
                        AccountCount = c.Int(),
                        BankName = c.String(),
                        BIK = c.String(),
                        Addres = c.String(),
                        AccountType = c.String(),
                        CurrencyID = c.Int(),
                        AvgOfDay = c.Decimal(precision: 18, scale: 2),
                        OperationCount = c.Int(),
                        Oid = c.Guid(nullable: false),
                        IsHistory = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(),
                        ActualDate = c.DateTime(),
                        NonActualDate = c.DateTime(),
                        ImportUpdateDate = c.DateTime(),
                        ImportDate = c.DateTime(),
                        Hidden = c.Boolean(nullable: false),
                        SortOrder = c.Double(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.NSI].Currency", t => t.CurrencyID)
                .ForeignKey("[CorpProp.Subject].Society", t => t.SocietyID)
                .Index(t => t.SocietyID)
                .Index(t => t.CurrencyID);
            
            CreateTable(
                "[CorpProp.Analyze.NSI].BudgetLine",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "[CorpProp.Analyze.NSI].FinancialIndicator",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
                .Index(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("[CorpProp.Analyze.NSI].FinancialIndicator", "ID", "[CorpProp.Base].DictObject");
            DropForeignKey("[CorpProp.Analyze.NSI].BudgetLine", "ID", "[CorpProp.Base].DictObject");
            DropForeignKey("[CorpProp.Analyze.Accounting].BankAccount", "SocietyID", "[CorpProp.Subject].Society");
            DropForeignKey("[CorpProp.Analyze.Accounting].BankAccount", "CurrencyID", "[CorpProp.NSI].Currency");
            DropForeignKey("[CorpProp.Analyze.Accounting].RecordBudgetLine", "OwnerID", "[CorpProp.Subject].Society");
            DropForeignKey("[CorpProp.Analyze.Accounting].RecordBudgetLine", "BudgetLineID", "[CorpProp.Analyze.NSI].BudgetLine");
            DropForeignKey("[CorpProp.Analyze.Accounting].FinancialIndicatorItem", "OwnerID", "[CorpProp.Subject].Society");
            DropForeignKey("[CorpProp.Analyze.Accounting].FinancialIndicatorItem", "FinancialIndicatorID", "[CorpProp.Analyze.NSI].FinancialIndicator");
            DropForeignKey("[CorpProp.Analyze.Subject].AnalyzeSociety", "OwnerID", "[CorpProp.Subject].Society");
            DropIndex("[CorpProp.Analyze.NSI].FinancialIndicator", new[] { "ID" });
            DropIndex("[CorpProp.Analyze.NSI].BudgetLine", new[] { "ID" });
            DropIndex("[CorpProp.Analyze.Accounting].BankAccount", new[] { "CurrencyID" });
            DropIndex("[CorpProp.Analyze.Accounting].BankAccount", new[] { "SocietyID" });
            DropIndex("[CorpProp.Analyze.Accounting].RecordBudgetLine", new[] { "BudgetLineID" });
            DropIndex("[CorpProp.Analyze.Accounting].RecordBudgetLine", new[] { "OwnerID" });
            DropIndex("[CorpProp.Analyze.Accounting].FinancialIndicatorItem", new[] { "FinancialIndicatorID" });
            DropIndex("[CorpProp.Analyze.Accounting].FinancialIndicatorItem", new[] { "OwnerID" });
            DropIndex("[CorpProp.Analyze.Subject].AnalyzeSociety", new[] { "OwnerID" });
            DropTable("[CorpProp.Analyze.NSI].FinancialIndicator");
            DropTable("[CorpProp.Analyze.NSI].BudgetLine");
            DropTable("[CorpProp.Analyze.Accounting].BankAccount");
            DropTable("[CorpProp.Analyze.Accounting].RecordBudgetLine");
            DropTable("[CorpProp.Analyze.Accounting].FinancialIndicatorItem");
            DropTable("[CorpProp.Analyze.Subject].AnalyzeSociety");
        }
    }
}
