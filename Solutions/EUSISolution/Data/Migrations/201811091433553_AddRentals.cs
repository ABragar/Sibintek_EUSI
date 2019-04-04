namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRentals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "[EUSI.Accounting].RentalOS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccountingObjectOid = c.Guid(),
                        ActRentDate = c.DateTime(),
                        AssetHolderRSBUID = c.Int(),
                        CadastralNumber = c.String(),
                        CadastralValue = c.Decimal(precision: 18, scale: 2),
                        Comments = c.String(),
                        ConsolidationID = c.Int(),
                        CostKindRentalPaymentsID = c.Int(),
                        DepositID = c.Int(),
                        DepreciationGroupID = c.Int(),
                        EUSINumber = c.Int(),
                        IndicationLicenceLandArea = c.String(),
                        InfrastructureExist = c.Boolean(),
                        InitialCost = c.Decimal(precision: 18, scale: 2),
                        InventoryArendaLand = c.String(),
                        InventoryNumber = c.String(),
                        LandPurposeID = c.Int(),
                        NameByDoc = c.String(),
                        ObjectLocationRent = c.String(),
                        OKOF2014ID = c.Int(),
                        ProprietorSubjectID = c.Int(),
                        RedemptionCost = c.Decimal(precision: 18, scale: 2),
                        RedemptionDate = c.DateTime(),
                        RentContractNumber = c.String(),
                        StateObjectRentID = c.Int(),
                        SubsoilUserID = c.Int(),
                        TakeOrPay = c.Boolean(),
                        TransactionKindID = c.Int(),
                        TransferRight = c.Boolean(),
                        Useful = c.String(),
                        UsefulEndLand = c.Int(),
                        CurrencyID = c.Int(),
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
                .ForeignKey("[CorpProp.NSI].AssetHolderRSBU", t => t.AssetHolderRSBUID)
                .ForeignKey("[CorpProp.NSI].Consolidation", t => t.ConsolidationID)
                .ForeignKey("[CorpProp.NSI].CostKindRentalPayments", t => t.CostKindRentalPaymentsID)
                .ForeignKey("[CorpProp.NSI].Deposit", t => t.DepositID)
                .ForeignKey("[CorpProp.NSI].DepreciationGroup", t => t.DepreciationGroupID)
                .ForeignKey("[CorpProp.NSI].LandPurpose", t => t.LandPurposeID)
                .ForeignKey("[CorpProp.NSI].OKOF2014", t => t.OKOF2014ID)
                .ForeignKey("[CorpProp.Subject].Subject", t => t.ProprietorSubjectID)
                .ForeignKey("[CorpProp.NSI].StateObjectRent", t => t.StateObjectRentID)
                .ForeignKey("[CorpProp.NSI].Consolidation", t => t.SubsoilUserID)
                .ForeignKey("[EUSI.NSI].TransactionKind", t => t.TransactionKindID)
                .ForeignKey("[CorpProp.NSI].Currency", t => t.CurrencyID)
                .Index(t => t.AssetHolderRSBUID)
                .Index(t => t.ConsolidationID)
                .Index(t => t.CostKindRentalPaymentsID)
                .Index(t => t.DepositID)
                .Index(t => t.DepreciationGroupID)
                .Index(t => t.LandPurposeID)
                .Index(t => t.OKOF2014ID)
                .Index(t => t.ProprietorSubjectID)
                .Index(t => t.StateObjectRentID)
                .Index(t => t.SubsoilUserID)
                .Index(t => t.TransactionKindID)
                .Index(t => t.CurrencyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.Accounting].RentalOS", "CurrencyID", "[CorpProp.NSI].Currency");
            DropForeignKey("[EUSI.Accounting].RentalOS", "TransactionKindID", "[EUSI.NSI].TransactionKind");
            DropForeignKey("[EUSI.Accounting].RentalOS", "SubsoilUserID", "[CorpProp.NSI].Consolidation");
            DropForeignKey("[EUSI.Accounting].RentalOS", "StateObjectRentID", "[CorpProp.NSI].StateObjectRent");
            DropForeignKey("[EUSI.Accounting].RentalOS", "ProprietorSubjectID", "[CorpProp.Subject].Subject");
            DropForeignKey("[EUSI.Accounting].RentalOS", "OKOF2014ID", "[CorpProp.NSI].OKOF2014");
            DropForeignKey("[EUSI.Accounting].RentalOS", "LandPurposeID", "[CorpProp.NSI].LandPurpose");
            DropForeignKey("[EUSI.Accounting].RentalOS", "DepreciationGroupID", "[CorpProp.NSI].DepreciationGroup");
            DropForeignKey("[EUSI.Accounting].RentalOS", "DepositID", "[CorpProp.NSI].Deposit");
            DropForeignKey("[EUSI.Accounting].RentalOS", "CostKindRentalPaymentsID", "[CorpProp.NSI].CostKindRentalPayments");
            DropForeignKey("[EUSI.Accounting].RentalOS", "ConsolidationID", "[CorpProp.NSI].Consolidation");
            DropForeignKey("[EUSI.Accounting].RentalOS", "AssetHolderRSBUID", "[CorpProp.NSI].AssetHolderRSBU");
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "CurrencyID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "TransactionKindID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "SubsoilUserID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "StateObjectRentID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "ProprietorSubjectID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "OKOF2014ID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "LandPurposeID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "DepreciationGroupID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "DepositID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "CostKindRentalPaymentsID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "ConsolidationID" });
            DropIndex("[EUSI.Accounting].RentalOS", new[] { "AssetHolderRSBUID" });
            DropTable("[EUSI.Accounting].RentalOS");
        }
    }
}
