namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Programmability : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_OwningMonthCountTS.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_AccountingCalculated_Estate.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_AccountingCalculated_Land.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_AccountingCalculated_Land_Extended.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_AccountingCalculated_Vehicle.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_AccountingCalculated_Vehicle_Extended.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_CheckMovementsAndStates.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_Create_AccountingCalculated_Estate.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_Create_AccountingCalculated_Land.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_Create_AccountingCalculated_Vehicle.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_GetBeByObuList.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_GetDeclarationsEstateByIFNS.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_GetDeclarationsLandByIFNS.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_GetDeclarationsTSByIFNS.sql");
            SqlFile("Migrations/SQL/Up/UP_2019_02_26_pReport_MasterDataControl.sql");
            SqlFile("Migrations/SQL/Up/Up_2019_02_26_pReport_ServiceEffectiveness.sql");
      
    
        }

        public override void Down()
        {
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_OwningMonthCountTS.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_AccountingCalculated_Estate.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_AccountingCalculated_Land.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_AccountingCalculated_Land_Extended.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_AccountingCalculated_Vehicle.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_AccountingCalculated_Vehicle_Extended.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_CheckMovementsAndStates.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_Create_AccountingCalculated_Estate.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_Create_AccountingCalculated_Land.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_Create_AccountingCalculated_Vehicle.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_GetBeByObuList.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_GetDeclarationsEstateByIFNS.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_GetDeclarationsLandByIFNS.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_GetDeclarationsTSByIFNS.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_MasterDataControl.sql");
            SqlFile("Migrations/SQL/Down/Down_2019_02_26_pReport_ServiceEffectiveness.sql");
        }
    }
}
