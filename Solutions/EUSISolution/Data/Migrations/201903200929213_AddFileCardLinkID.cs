namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileCardLinkID : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.Accounting].AccountingMoving", "FileCardLinkID", c => c.Int());
            Sql(@"                
                UPDATE mov
                SET mov.[FileCardLinkID] = doc.[ID]
                FROM [EUSI.Accounting].[AccountingMoving] mov
                INNER JOIN [EUSI.ManyToMany].[FileCardAndAccountingMoving] link 
                ON link.[Hidden] = 0 AND link.[ObjRigthId] = mov.[ID]
                INNER JOIN [CorpProp.Document].[FileCard] doc ON doc.[ID] = link.[ObjLeftId]
                WHERE ISNULL(mov.[FileCardLink],N'') <> N'' AND doc.[Name] = mov.[FileCardLink] AND mov.[FileCardLinkID] IS NULL
            ");
        }
        
        public override void Down()
        {
            DropColumn("[EUSI.Accounting].AccountingMoving", "FileCardLinkID");
        }
    }
}
