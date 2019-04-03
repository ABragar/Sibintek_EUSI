using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Helpers.Import.Extentions;
using Base.DAL;
using CorpProp.Entities.Import;

using System;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;

namespace CorpProp.RosReestr.Extentions.EGRN.Unknown
{
    /// <summary>
    /// Методы расширения для импорта объекта выписки - право в модуль Росреестра.
    /// </summary>
    public static class RightExtentions
    {
        #region SibRosReestr.EGRN.Unknown

        public static void Import(
         this RightRecord rr
        , SibRosReestr.EGRN.Unknown.RightRecordsBaseParamsRight_record obj
        , ref ImportHolder holder
        )
        {
            if (obj == null) return;
            try
            {
                rr.RegDate = ImportHelper.GetDate(obj.Record_info?.Registration_date);
                if (obj.Right_data != null)
                {
                    rr.RightTypeCode = obj.Right_data.Right_type?.Code;
                    rr.RightTypeName = obj.Right_data.Right_type?.Value;
                    rr.RegNumber = obj.Right_data.Right_number;
                    rr.SetShares(obj.Right_data.Shares);
                    rr.ShareText = obj.Right_data.Share_description;
                    rr.ReinstatementDate = ImportHelper.GetDate(obj.Right_data.Reinstatement?.Prev_registration_date);
                }
                rr.SetHolders(holder.UnitOfWork, obj.Right_holders);
                rr.CreateDocs(holder.UnitOfWork, obj.Underlying_documents);
                rr.CreateDeals(holder.UnitOfWork, obj.Third_party_consents);
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        private static void SetShares(
              this RightRecord rr
            , SibRosReestr.EGRN.Unknown.Shares obj
        )
        {
            if (obj == null || obj.Item == null) return;
            if (obj.Item is SibRosReestr.EGRN.Unknown.BuilderShare)
                rr.Builder_share_info = ((SibRosReestr.EGRN.Unknown.BuilderShare)(obj.Item)).Builder_share_info;
            if (obj.Item is SibRosReestr.EGRN.Unknown.BuilderShareWithObject)
            {
                SibRosReestr.EGRN.Unknown.BuilderShareWithObject item = obj.Item as SibRosReestr.EGRN.Unknown.BuilderShareWithObject;
                rr.Builder_share_with_object_info = item.Builder_share_with_object_info;
                if (item.Info_objects != null)
                    foreach (var pl in item.Info_objects)
                    {
                        rr.Info_objectsCadNumbers += (!String.IsNullOrEmpty(rr.Info_objectsCadNumbers)) ? $"; {pl.Cad_number}" : pl.Cad_number;
                    }
            }
            if (obj.Item is SibRosReestr.EGRN.Unknown.RoomOwnersShare)
                rr.Room_owners_share_info = ((SibRosReestr.EGRN.Unknown.RoomOwnersShare)(obj.Item)).Room_owners_share_info;
            if (obj.Item is SibRosReestr.EGRN.Unknown.Share)
            {
                rr.Numerator = ImportHelper.GetInt(((SibRosReestr.EGRN.Unknown.Share)(obj.Item)).Numerator);
                rr.Denominator = ImportHelper.GetInt(((SibRosReestr.EGRN.Unknown.Share)(obj.Item)).Denominator);
            }
            if (obj.Item is SibRosReestr.EGRN.Unknown.ShareBalHectare)
                rr.Bal_hectare = ((SibRosReestr.EGRN.Unknown.ShareBalHectare)(obj.Item)).Bal_hectare;
            if (obj.Item is SibRosReestr.EGRN.Unknown.ShareHectare)
                rr.Hectare = ((SibRosReestr.EGRN.Unknown.ShareHectare)(obj.Item)).Hectare;
            if (obj.Item is SibRosReestr.EGRN.Unknown.ShareUnknown)
            {
                rr.Proportion_cad_number = ((SibRosReestr.EGRN.Unknown.ShareUnknown)(obj.Item)).Proportion_cad_number;
                rr.Share_Share_description = ((SibRosReestr.EGRN.Unknown.ShareUnknown)(obj.Item)).Share_description;
            }

        }


        private static void SetHolders(
             this RightRecord rr
            , IUnitOfWork uow
            , List<SibRosReestr.EGRN.Unknown.RightHolderOut> objs
            )
        {
            if (objs == null) return;
            string str = "";
            for (int i = 0; i < objs.Count; i++)
            {
                SibRosReestr.EGRN.Unknown.RightHolderOut hh = objs[i];
                SubjectRecord sr = SubjectExtentions.CreateSubject(uow, hh.Item);
                RightHolder rh =
                    uow.GetRepository<RightHolder>()
                    .Create(new RightHolder());
                rh.RightRecord = rr;
                rh.Extract = rr.Extract;
                rh.SubjectRecord = sr;
                rh.Inn = sr.Inn;
                rh.Ogrn = sr.Ogrn;
                rh.Name = sr.Name;
                rh.Short_name = sr.Short_name;
                str += (!String.IsNullOrEmpty(str)) ? $"; {rh.Inn} {rh.Name}" : $"{rh.Inn} {rh.Name}";
                if (sr != null && sr is IndividualSubject)
                    rr.PersonHolder = true;
            }
            rr.RightHoldersStr = str;
        }

        public static void CreateDocs(
              this RightRecord rr
            , IUnitOfWork uow
            , List<SibRosReestr.EGRN.Unknown.UnderlyingDocumentOut> objs
            )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                DocumentRecord doc =
                    uow.GetRepository<DocumentRecord>()
                    .Create(new DocumentRecord());
                doc.Extract = rr.Extract;
                doc.RightRecord = rr;
                doc.TypeCode = item.Document_code?.Code;
                doc.TypeName = item.Document_code?.Value;
                doc.DocumentType = $"{doc.TypeCode} {doc.TypeName}";
                doc.Name = item.Document_name;
                doc.Series = item.Document_series;
                doc.Number = item.Document_number;
                doc.DocDate = ImportHelper.GetDate(item.Document_date);
                doc.Issuer = item.Document_issuer;

            }
        }




        public static void CreateDeals(
              this RightRecord rr
            , IUnitOfWork uow
            , List<SibRosReestr.EGRN.Unknown.ThirdPartyConsent> objs
           )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                DealRecord dd =
                    uow.GetRepository<DealRecord>()
                    .Create(new DealRecord());
                dd.Extract = rr.Extract;
                dd.RightRecord = rr;
                dd.ObjectRecord = rr.ObjectRecord;
                if (item.Law != null)
                {
                    dd.Section = item.Law.Section;
                    dd.Paragraph = item.Law.Paragraph;
                    dd.Article = item.Law.Article;
                    dd.Law_date = item.Law.Law_date;
                    dd.LawNumber = item.Law.Number;
                    dd.LawName = item.Law.Name;
                }
                if (item.Mark_content != null && item.Mark_content.Origin_mark != null)
                    dd.Origin_content = item.Mark_content.Origin_mark.Origin_content;
                dd.SetDissenting(uow, item.Dissenting_entities);
                dd.Extract = rr.Extract;
                dd.ObjectRecord = rr.ObjectRecord;
            }
        }


        #endregion
    }
}
