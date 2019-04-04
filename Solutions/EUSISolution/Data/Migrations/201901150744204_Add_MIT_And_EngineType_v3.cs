namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MIT_And_EngineType_v3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "[EUSI.NSI].MITDictionary",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Brand = c.String(),
                        EngineType = c.String(),
                        EngineCapacity = c.String(),
                        MaxAge = c.Int(),
                        LowBoundRange = c.Decimal(precision: 18, scale: 2),
                        UpBoundRange = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "[EUSI.NSI].EngineType",
                c => new
                    {
                        ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("[CorpProp.Base].DictObject", t => t.ID)
                .Index(t => t.ID);
                       

            AddColumn("[CorpProp.NSI].BoostOrReductionFactor", "MaxAge", c => c.Int());
            AddColumn("[CorpProp.NSI].BoostOrReductionFactor", "LowBoundRange", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("[CorpProp.NSI].BoostOrReductionFactor", "UpBoundRange", c => c.Decimal(precision: 18, scale: 2));

            SqlFile("Migrations/SQL/Add_MITDictionary_And_EngineType.sql");
            SqlFile("Migrations/SQL/Seed_BoostOrReductionFactor.sql");
        }
        
        public override void Down()
        {
            Sql(@"DELETE FROM [CorpProp.NSI].NSI
                  WHERE [Mnemonic] IN(N'EngineTypeMenu', N'MITDictionaryMenu')");

            DropForeignKey("[EUSI.NSI].EngineType", "ID", "[CorpProp.Base].DictObject");
            DropForeignKey("[EUSI.NSI].MITDictionary", "ID", "[CorpProp.Base].DictObject");
            DropIndex("[EUSI.NSI].EngineType", new[] { "ID" });
            DropIndex("[EUSI.NSI].MITDictionary", new[] { "ID" });
            DropColumn("[CorpProp.NSI].BoostOrReductionFactor", "UpBoundRange");
            DropColumn("[CorpProp.NSI].BoostOrReductionFactor", "LowBoundRange");
            DropColumn("[CorpProp.NSI].BoostOrReductionFactor", "MaxAge");
            DropTable("[EUSI.NSI].EngineType");
            DropTable("[EUSI.NSI].MITDictionary");
        }
    }
}
