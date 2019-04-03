using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Helpers.Import.Extentions;
using Base.DAL;
using CorpProp.Entities.Import;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;

namespace CorpProp.RosReestr.Extentions.EGRP.V04
{
    /// <summary>
    /// Методы расширения для импорта объекта выписки - право в модуль Росреестра.
    /// </summary>
    public static class RightExtentions
    {
        #region V04
        /// <summary>
        /// Импорт записи о праве из обекта xml выписки.
        /// </summary>
        /// <param name="right"></param>
        /// <param name="obj"></param>
        /// <param name="holder"></param>
        public static void Import(
              this RightRecord right
            , SibRosReestr.EGRP.V04.ExtractSubj.TOpenRegistration obj
            , SubjectRecord sr
            , ref ImportHolder holder
            )
        {
            try
            {
                if (obj == null) return;

                right.ID_Record = obj.ID_Record;
                right.MdfDate = obj.MdfDate;
                right.RegNumber = obj.RegNumber;
                right.RightTypeCode = ImportHelper.GetCodeEnum(obj.Type);
                right.RightTypeName = obj.Name;                    
                right.RegDateStr = obj.RegDate;
                right.RegDate = ImportHelper.GetDate(obj.RegDate);
                right.EndDateStr = obj.EndDate;
                right.EndDate = ImportHelper.GetDate(obj.EndDate);
                right.SetShareV04(obj.Item);
                right.CreateDocs(obj.DocFound, ref holder);
                right.SetHolder(sr, holder.UnitOfWork);
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        private static void SetHolder(this RightRecord right, SubjectRecord sr, IUnitOfWork uow)
        {

            if (sr == null) return;
            right.SubjectRecord = sr;
            right.RightHoldersStr = $"{sr?.Inn} {sr?.Name}";
            RightHolder rh = uow.GetRepository<RightHolder>().Create(new RightHolder());
            rh.Extract = right.Extract as ExtractSubj;
            rh.Inn = sr?.Inn;
            rh.Name = sr.Name;
            rh.Ogrn = sr.Ogrn;
            rh.RightRecord = right;
            rh.SubjectRecord = sr;
            rh.Short_name = sr?.Short_name;

        }


        /// <summary>
        /// Устанавливает долю.
        /// </summary>      
        private static void SetShareV04(
             this RightRecord rr
           , object obj
            )
        {
            if (obj == null) return;
            if (obj is SibRosReestr.EGRP.V04.ExtractSubj.TOpenRegistrationShare)
            {
                rr.Numerator = ImportHelper.GetInt(((SibRosReestr.EGRP.V04.ExtractSubj.TOpenRegistrationShare)obj).Numerator);
                rr.Denominator = ImportHelper.GetInt(((SibRosReestr.EGRP.V04.ExtractSubj.TOpenRegistrationShare)obj).Denominator);
            }
            else
                rr.ShareText = obj.ToString();
        }

        /// <summary>
        /// Инициирует создание документов-основания.
        /// </summary>
        /// <param name="rr"></param>
        /// <param name="objs"></param>
        /// <param name="holder"></param>
        private static void CreateDocs(
             this RightRecord rr
           , List<SibRosReestr.EGRP.V04.ExtractSubj.TDocRight> objs
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
            this RightRecord rr
          , SibRosReestr.EGRP.V04.ExtractSubj.TDocRight obj
          , ref ImportHolder holder
           )
        {
            if (obj == null) return;
            try
            {
                DocumentRecord doc = holder.UnitOfWork.GetRepository<DocumentRecord>().Create(new DocumentRecord());
                doc.Import(obj, ref holder);
                doc.RightRecord = rr;
                doc.Extract = rr.Extract;
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        #endregion//V04


    }
}
