using Base.DAL;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorpProp.RosReestr.Extentions.EGRP.V06
{
    public static class DocExtentions
    {
        #region V06
        public static void Import(
             this DocumentRecord doc
           , SibRosReestr.EGRP.V06.ExtractSubj.TDocRight obj
           , ref ImportHolder holder
           )
        {
            try
            {
                if (obj == null) return;

                doc.ID_Document = obj.ID_Document;
                doc.Content = obj.Content;
                doc.TypeCode = ImportHelper.GetCodeEnum(obj.Type_Document);
                doc.TypeName = ImportHelper.GetNameEnum(obj.Type_Document);
                doc.Name = obj.Name;
                doc.Series = obj.Series;
                doc.Number = obj.Number;
                doc.DocDate = ImportHelper.GetDate(obj.Date);
                doc.Issuer = obj.IssueOrgan;
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }
        #endregion
    }
}
