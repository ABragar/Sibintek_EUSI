using Base.DAL;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions.EGRN.Unknown
{
    public static class DocExtentions
    {

        private static void Import(
              this DocumentRecord doc
            , SibRosReestr.EGRN.Unknown.DocRequisiteMain obj
           )
        {
            doc.TypeCode = obj.Document_code?.Code;
            doc.TypeName = obj.Document_code?.Value;
            doc.DocumentType = $"{doc.TypeCode} {doc.TypeName}";
            doc.Number = obj.Document_number;
            doc.DocDate = ImportHelper.GetDate(obj.Document_date);
            doc.Issuer = obj.Document_issuer;
            doc.Name = $"{doc.TypeName} {doc.Number}";
        }

        public static DocumentRecord CreateRemoveAccountDocument(
            IUnitOfWork uofw
           , SibRosReestr.EGRN.Unknown.DocRequisiteMain item
           )
        {
            if (item == null) return null;
            DocumentRecord doc =
                uofw.GetRepository<DocumentRecord>()
                .Create(new DocumentRecord());
            doc.Import(item);
            return doc;

        }

    }
}
