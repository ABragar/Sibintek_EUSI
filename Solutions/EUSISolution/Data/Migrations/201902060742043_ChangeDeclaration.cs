namespace Data.EF
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDeclaration : DbMigration
    {
        public override void Up()
        {
            AddColumn("[EUSI.NU].Declaration", "FileCardID", c => c.Int());
            CreateIndex("[EUSI.NU].Declaration", "FileCardID");
            AddForeignKey("[EUSI.NU].Declaration", "FileCardID", "[CorpProp.Document].FileCard", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("[EUSI.NU].Declaration", "FileCardID", "[CorpProp.Document].FileCard");
            DropIndex("[EUSI.NU].Declaration", new[] { "FileCardID" });
            DropColumn("[EUSI.NU].Declaration", "FileCardID");
        }
    }
}
