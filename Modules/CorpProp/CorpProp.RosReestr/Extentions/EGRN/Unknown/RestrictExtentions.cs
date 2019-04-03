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
    /// <summary>
    /// Методы расширения для импорта объекта выписки - обременение в модуль Росреестра.
    /// </summary>
    public static class RestrictExtentions
    {
        #region SibRosReestr.EGRN.Unknown

        public static void Import(
            this RestrictRecord rr
          , SibRosReestr.EGRN.Unknown.RestrictRecordType obj
          , ref ImportHolder holder
          )
        {
            try
            {
                if (obj == null) return;
                rr.Registration_date = ImportHelper.GetDate(obj.Record_info?.Registration_date);
                if (obj.Restrictions_encumbrances_data != null)
                {
                    rr.RegNumber = obj.Restrictions_encumbrances_data.Restriction_encumbrance_number;
                    rr.TypeCode = obj.Restrictions_encumbrances_data.Restriction_encumbrance_type?.Code;
                    rr.TypeName = obj.Restrictions_encumbrances_data.Restriction_encumbrance_type?.Value;
                    rr.StartDate = ImportHelper.GetDate(obj.Restrictions_encumbrances_data.Period?.Period_info?.Start_date);
                    rr.EndDate = ImportHelper.GetDate(obj.Restrictions_encumbrances_data.Period?.Period_info?.End_date);
                    rr.Term = obj.Restrictions_encumbrances_data.Period?.Period_info?.Deal_validity_time;
                    rr.Servitude_condition = obj.Restrictions_encumbrances_data.Additional_encumbrance_info?.Servitude?.Servitude_condition;
                    rr.Servitude_kindCode = obj.Restrictions_encumbrances_data.Additional_encumbrance_info?.Servitude?.Servitude_kind?.Code;
                    rr.Servitude_kindName = obj.Restrictions_encumbrances_data.Additional_encumbrance_info?.Servitude?.Servitude_kind?.Value;
                    rr.CreateRestrictRights(holder.UnitOfWork, obj.Restrictions_encumbrances_data.Restricting_rights);
                }
                rr.Expropriation_info_type = obj.State_expropriation?.Expropriation_info?.Expropriation_info_type;
                rr.Origin_content = obj.State_expropriation?.Expropriation_info?.Origin_content;
                rr.SetRestrictParties(holder.UnitOfWork, obj.Restrict_parties?.Restricted_rights_parties);
                rr.SetRestrictDocuments(holder.UnitOfWork, obj.Underlying_documents);
                rr.SetRestrictDeals(holder.UnitOfWork, obj.Third_party_consents);


            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }

        private static void CreateRestrictRights(
           this RestrictRecord rr
         , IUnitOfWork uow
         , List<SibRosReestr.EGRN.Unknown.RightRecordNumber> objs
         )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                Entities.RightRecordNumber rn =
                    uow.GetRepository<Entities.RightRecordNumber>()
                    .Create(new Entities.RightRecordNumber());
                rn.RestrictRecord = rr;
                rn.Number = item.Number;
                rn.Right_number = item.Right_number;
                if (!String.IsNullOrEmpty(rn.Right_number))
                    rr.RightsStr += (!String.IsNullOrEmpty(rr.RightsStr)) ? $"; {rn.Right_number}" : rn.Right_number;
            }
        }



        public static void SetRestrictParties(
             this RestrictRecord rr
          , IUnitOfWork uow
          , List<SibRosReestr.EGRN.Unknown.RestrictedRightsPartyOut> objs
         )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                Entities.RestrictedRightsPartyOut pr =
                    uow.GetRepository<Entities.RestrictedRightsPartyOut>()
                    .Create(new Entities.RestrictedRightsPartyOut());
                pr.RestrictRecord = rr;
                pr.Subject = SubjectExtentions.CreateSubject(uow, item.Subject?.Item);
                pr.TypeCode = item.Type?.Code;
                pr.TypeName = item.Type?.Value;
                pr.Inn = pr.Subject?.Inn;
                pr.Name = pr.Subject?.Name;
                pr.Short_name = pr.Subject?.Short_name;
                pr.Ogrn = pr.Subject?.Ogrn;
            }
        }

        public static void SetRestrictDeals(
            this RestrictRecord rr
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
                dd.RestrictRecord = rr;
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

            }
        }

        public static void SetRestrictDocuments(
             this RestrictRecord rr
           , IUnitOfWork uow
           , List<SibRosReestr.EGRN.Unknown.UnderlyingDocumentOutAll> objs
          )
        {
            if (objs == null) return;
            foreach (var item in objs)
            {
                DocumentRecord doc =
                    uow.GetRepository<DocumentRecord>()
                    .Create(new DocumentRecord());
                doc.Extract = rr.Extract;
                doc.RestrictRecord = rr;
                doc.TypeCode = item.Document_code?.Code;
                doc.TypeName = item.Document_code?.Value;
                doc.DocumentType = $"{doc.TypeCode} {doc.TypeName}";
                doc.Name = item.Document_name;
                doc.Series = item.Document_series;
                doc.Number = item.Document_number;
                doc.DocDate = ImportHelper.GetDate(item.Document_date);
                doc.Issuer = item.Document_issuer;
                doc.Fullname_posts_person = item.Fullname_posts_person;
                doc.Notarize_date = ImportHelper.GetDate(item.Doc_notarized?.Notarize_date);
                doc.Notary_action_num = item.Doc_notarized?.Notary_action_num;
                doc.Notary_name = item.Doc_notarized?.Notary_name;

            }
        }



        public static void Import(
                 this RestrictRecord rr
               , SibRosReestr.EGRN.Unknown.RestrictRecordLandType obj
               , ref ImportHolder holder
               )
        {
            try
            {
                if (obj == null) return;
                rr.Registration_date = ImportHelper.GetDate(obj.Record_info?.Registration_date);
                if (obj.Restrictions_encumbrances_data != null)
                {
                    rr.RegNumber = obj.Restrictions_encumbrances_data.Restriction_encumbrance_number;
                    rr.TypeCode = obj.Restrictions_encumbrances_data.Restriction_encumbrance_type?.Code;
                    rr.TypeName = obj.Restrictions_encumbrances_data.Restriction_encumbrance_type?.Value;
                    if (obj.Restrictions_encumbrances_data.Period != null && obj.Restrictions_encumbrances_data.Period.Item != null)

                    {
                        if (obj.Restrictions_encumbrances_data.Period.Item is SibRosReestr.EGRN.Unknown.PeriodInfoType)
                        {
                            SibRosReestr.EGRN.Unknown.PeriodInfoType pp =
                                obj.Restrictions_encumbrances_data.Period.Item as SibRosReestr.EGRN.Unknown.PeriodInfoType;
                            rr.StartDate = ImportHelper.GetDate(pp.Start_date);
                            rr.EndDate = ImportHelper.GetDate(pp.End_date);
                            rr.Term = pp.Deal_validity_time;
                        }
                        //TODO: item.Restrictions_encumbrances_data.Period.Item
                    }

                    rr.Servitude_condition = obj.Restrictions_encumbrances_data.Additional_encumbrance_info?.Servitude?.Servitude_condition;
                    rr.Servitude_kindCode = obj.Restrictions_encumbrances_data.Additional_encumbrance_info?.Servitude?.Servitude_kind?.Code;
                    rr.Servitude_kindName = obj.Restrictions_encumbrances_data.Additional_encumbrance_info?.Servitude?.Servitude_kind?.Value;
                    rr.CreateRestrictRights(holder.UnitOfWork, obj.Restrictions_encumbrances_data.Restricting_rights);
                }

                rr.SetRestrictParties(holder.UnitOfWork, obj.Restrict_parties?.Restricted_rights_parties);
                rr.SetRestrictDocuments(holder.UnitOfWork, obj.Underlying_documents);
                rr.SetRestrictDeals(holder.UnitOfWork, obj.Third_party_consents);

            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
        }


        #endregion
    }
}
