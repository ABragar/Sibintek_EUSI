using CorpProp.Entities.Document;
using CorpProp.RosReestr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    public static class MigrateFileCard
    {

        public static void Migrate(this FileCardOne file, DocumentRecord doc)
        {
            file.DocumentID = doc.ID_Document;
            file.Name = doc.Name;
            file.Description = doc.Content;
            file.Number = doc.Number;
            file.SerialNumber = doc.Series;
            file.DocTypeCode = doc.TypeCode;
            file.DocTypeName = doc.TypeName;
            file.Issuer = doc.Issuer;
            file.ExecutorName = doc.Notary_name;            
            file.DateCard = doc.DocDate;
            file.ExecutorName = doc.Fullname_posts_person;
            if (file.CategoryID == 0)
                file.CategoryID = 1;          
            
        }
    }
}
