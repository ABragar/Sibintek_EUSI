using System.Text;

namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredProcedure : DbMigrationWithHistory
    {
        private string _schemaReports = "ReportService";
        private string strSP_pGetSocietyInfo = "ReportService.pGetSocietyInfo";
        private string strSP_pGraphBySociety = "ReportService.pGraphBySociety";
        
        public override void Up()
        {
            DropStoredProcedure(strSP_pGetSocietyInfo);
            CreateStoredProcedure(strSP_pGetSocietyInfo,p=>new {vIntId=p.Int(null), vStrIDEUP=p.String()},
                Get_strSP_pGetSocietyInfo()
                ,true);
            
            CreateStoredProcedure(strSP_pGraphBySociety,p=>new {SocietyID=p.Int()},
                Get_strSP_pGraphBySociety(), 
                true
                );
        }
        
        public override void Down()
        {
            DropStoredProcedure(strSP_pGetSocietyInfo);
            DropStoredProcedure(strSP_pGraphBySociety);
        }

        private string Get_strSP_pGraphBySociety()
        {
            StringBuilder sb=new StringBuilder();
            sb.AppendLine("IF (SELECT COUNT(ID) FROM [CorpProp.CorporateGovernance].Shareholder WHERE SocietyRecipientID=@SocietyID and (isnull(hidden,0)=0 and isnull(IsHistory,0)=0))=1 ")
                .AppendLine("BEGIN  ")
                .AppendLine(" ")
                .AppendLine("WITH ")
                .AppendLine(" Rec (SocietyShareholderID, SocietyRecipientID, R, ShareMarket, Parents, isLoop,Lvl) ")
                .AppendLine(" AS ( ")
                .AppendLine("   SELECT  ")
                .AppendLine("		SocietyShareholderID ")
                .AppendLine("		,SocietyRecipientID ")
                .AppendLine("		,'1' ")
                .AppendLine("		,ShareMarket ")
                .AppendLine("		,'|'+CAST(ID AS VARCHAR(MAX))+'|' ")
                .AppendLine("		,0  ")
                .AppendLine("		,1 ")
                .AppendLine("   FROM [CorpProp.CorporateGovernance].Shareholder (nolock) ")
                .AppendLine("   WHERE SocietyRecipientID=@SocietyID and (isnull(hidden,0)=0 and isnull(IsHistory,0)=0) ")
                .AppendLine("   UNION ALL ")
                .AppendLine("   SELECT  ")
                .AppendLine("		Shareholder.SocietyShareholderID ")
                .AppendLine("		,Shareholder.SocietyRecipientID ")
                .AppendLine("		,'2' ")
                .AppendLine("		,Shareholder.ShareMarket ")
                .AppendLine("		,Rec.Parents+CAST(Shareholder.ID AS VARCHAR(MAX))+'|' ")
                .AppendLine("		,CASE WHEN Rec.Parents LIKE '%|' + CAST(Shareholder.ID AS VARCHAR(MAX)) + '|%' THEN 1 ELSE 0 END ")
                .AppendLine("		,Rec.Lvl+1 ")
                .AppendLine("    FROM  [CorpProp.CorporateGovernance].Shareholder (nolock) ")
                .AppendLine("	INNER JOIN Rec ON Rec.SocietyShareholderID = Shareholder.SocietyRecipientID AND Rec.isLoop=0 ")
                .AppendLine("	WHERE isnull(Shareholder.IsHistory,0)=0 AND isnull(Shareholder.Hidden,0)=0 ")
                .AppendLine("   ) ")
                .AppendLine("     ")
                .AppendLine("SELECT DISTINCT Rec.ShareMarket ")
                .AppendLine(",SocietyShareholderID, SocietyRecipientID ")
                .AppendLine(" ,Recipient=soRecipient.ShortName ")
                .AppendLine(" ,Shareholder = soShareholder.ShortName  ")
                .AppendLine(" --,R ")
                .AppendLine(" FROM Rec  ")
                .AppendLine(" LEFT JOIN [CorpProp.Subject].Society soShareholder ON soShareholder.ID=Rec.SocietyShareholderID ")
                .AppendLine(" LEFT JOIN [CorpProp.Subject].Society soRecipient ON soRecipient.ID=Rec.SocietyRecipientID ")
                .AppendLine(" END ");
            return sb.ToString();
        }

        private string Get_strSP_pGetSocietyInfo()
        {
            StringBuilder sb=new StringBuilder();
            sb.AppendLine("IF(@vIntId IS NOT NULL) ")
                .AppendLine("    SELECT   ShortName,   FullName,    IDEUP ")
                .AppendLine("    FROM [CorpProp.Subject].Society ")
                .AppendLine("    WHERE ID=@vIntId ")
                .AppendLine("IF(@vIntId IS NULL AND @vStrIDEUP IS NOT NULL) ")
                .AppendLine("    SELECT   ShortName,   FullName,    IDEUP ")
                .AppendLine("    FROM [CorpProp.Subject].Society ")
                .AppendLine("    WHERE IDEUP=@vStrIDEUP AND IsHistory!=1 AND Hidden!=1");
            return sb.ToString();
        }
    }
}
