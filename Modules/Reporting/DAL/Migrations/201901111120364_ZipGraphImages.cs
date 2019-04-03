namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// DO NOT CHANGE THIS CLASS.
    /// THIS IS MIGRATION.
    /// Any changes will not be applied if migration already applied.
    /// </summary>
    public partial class ZipGraphImages : DbMigrationWithHistory
    {
        private string _schemaReports = "ReportService";
        private string strSP_pGetSocietyInfo = "ReportService.pGetSocietyInfo";
        
        public override void Up()
        {
            CreateStoredProcedure($"{strSP_pGetSocietyInfo}",p=>new {vIntId=p.Int()},
                "SELECT"+
                "   ShortName," +
                "   FullName, " +
                "   IDEUP " +
                "FROM [CorpProp.Subject].Society " +
                "WHERE ID=@vIntId",true);
        }
        
        public override void Down()
        {
            DropStoredProcedure(strSP_pGetSocietyInfo);
        }
    }
}
