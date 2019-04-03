using Base.DAL;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Extentions.EGRP.V06;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions
{
    /// <summary>
    /// Методы расширения для импорта объекта выписки - обременение в модуль Росреестра.
    /// </summary>
    public static class RestrictExtentions
    {

        #region V06
        /// <summary>
        ///  Импорт записи об обременении из обекта xml выписки.
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="obj"></param>
        /// <param name="holder"></param>
        public static void Import(
             this RestrictRecord rr
           , SibRosReestr.EGRP.V06.ExtractSubj.TRightSubjEncumbrance obj
           , ref ImportHolder holder
           )
        {
            try
            {
                if (obj == null) return;

                rr.ID_Record = obj.ID_Record;
                rr.MdfDate = obj.MdfDate;
                rr.RegNumber = obj.RegNumber;
                rr.TypeCode = ImportHelper.GetCodeEnum(obj.Type);
                rr.TypeName = obj.Name;
                rr.ShareText = obj.ShareText;
                rr.RegDateStr = obj.RegDate;
                rr.RegDate = ImportHelper.GetDate(obj.RegDate);
                rr.Started = obj.Duration?.Started;
                rr.StartDate = ImportHelper.GetDate(obj.Duration?.Started);
                rr.Stopped = obj.Duration?.Stopped;
                rr.EndDate = ImportHelper.GetDate(obj.Duration?.Stopped);
                rr.Term = obj.Duration?.Term;
                rr.SetOwnersV06(obj.Items);
                rr.CreateDocs(obj.DocFound, ref holder);
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Устанавливает выгодоприобритателей или участников долевого строительства.
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="objs"></param>
        private static void SetOwnersV06(
             this RestrictRecord rr
           , List<object> objs
          )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                if (item is SibRosReestr.EGRP.V06.ExtractSubj.TSubject)
                {
                    if (!String.IsNullOrEmpty(rr.Owner))
                        rr.Owner += "; ";
                    rr.Owner += ImportHelper.GetSubjectInfo(((SibRosReestr.EGRP.V06.ExtractSubj.TSubject)item));
                }

                else
                {
                    if (!String.IsNullOrEmpty(rr.AllShareOwner))
                        rr.AllShareOwner += "; ";
                    rr.AllShareOwner += item.ToString();
                }
            }
        }

        /// <summary>
        /// Инициирует создание документов-основания.
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="objs"></param>
        /// <param name="holder"></param>
        private static void CreateDocs(
             this RestrictRecord rr
           , List<SibRosReestr.EGRP.V06.ExtractSubj.TDocRight> objs
           , ref ImportHolder holder
            )
        {
            if (objs == null) return;
            try
            {
                foreach (var item in objs)
                {
                    rr.CreateDoc(item, ref holder);
                }
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Создает документ основания.
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="obj"></param>
        /// <param name="holder"></param>
        private static void CreateDoc(
            this RestrictRecord rr
          , SibRosReestr.EGRP.V06.ExtractSubj.TDocRight obj
          , ref ImportHolder holder
           )
        {
            if (obj == null) return;
            try
            {
                DocumentRecord doc = holder.UnitOfWork.GetRepository<DocumentRecord>().Create(new DocumentRecord());
                doc.RestrictRecord = rr;
                doc.Extract = rr.Extract;
                doc.Import(obj, ref holder);
               
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }
        #endregion//V06

    }
}
