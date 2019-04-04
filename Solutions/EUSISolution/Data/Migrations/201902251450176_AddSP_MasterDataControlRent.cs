namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSP_MasterDataControlRent : DbMigration
    {
        public override void Up()
        {
            SqlFile("Migrations/SQL/Add_SP_MasterDataControl_Rent.sql");
        }

        public override void Down()
        {
            Sql(@"IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'pReport_MasterDataControl_Rent') DROP PROC[dbo].[pReport_MasterDataControl_Rent]");
        }
    }
}
