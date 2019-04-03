using Base.DAL;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions.EGRN.Unknown
{
    public static class SubjectExtentions
    {
        public static SubjectRecord CreateSubject(
        IUnitOfWork uofw
       , object item)
        {
            if (item == null) return null;
            SubjectRecord sr = null;
            if (item is SibRosReestr.EGRN.Unknown.AnotherRight && ((SibRosReestr.EGRN.Unknown.AnotherRight)item).Another_type != null)
                sr = CreateAnotherSubject(uofw, ((SibRosReestr.EGRN.Unknown.AnotherRight)item).Another_type.Item);

            if (item is SibRosReestr.EGRN.Unknown.AnotherRestricted && ((SibRosReestr.EGRN.Unknown.AnotherRestricted)item).Another_type != null)
                sr = CreateAnotherSubject(uofw, ((SibRosReestr.EGRN.Unknown.AnotherRestricted)item).Another_type.Item);

            if (item is SibRosReestr.EGRN.Unknown.IndividualOut)
                sr = CreateIndividualSubject(uofw, item);

            if (item is SibRosReestr.EGRN.Unknown.LegalEntityOut)
                sr = CreateLegalSubject(uofw, item);

            if (item is SibRosReestr.EGRN.Unknown.PublicFormations)
                sr = CreatePublicSubject(uofw, item);

            return sr;
        }


        public static SubjectRecord CreateAnotherSubject(
            IUnitOfWork uofw
            , object item)
        {
            if (item == null) return null;

            SubjectRecord sr = new AnotherSubject();

            if (item is SibRosReestr.EGRN.Unknown.AparthouseOwners)
            {
                sr.Aparthouse_owners_name = ((SibRosReestr.EGRN.Unknown.AparthouseOwners)item).Aparthouse_owners_name;
                sr.Name = sr.Aparthouse_owners_name;
                sr.Short_name = sr.Name;
                sr.Full_name = sr.Name;
            }
            if (item is SibRosReestr.EGRN.Unknown.BondsHoldersOut)
            {
                sr.Bonds_number = ((SibRosReestr.EGRN.Unknown.BondsHoldersOut)item).Bonds_number;
                sr.Issue_date = ImportHelper.GetDate(((SibRosReestr.EGRN.Unknown.BondsHoldersOut)item).Issue_date);
                sr.Name = $"Владельцы облигаций {sr.Bonds_number}";
                sr.Short_name = sr.Name;
                sr.Full_name = sr.Name;
            }
            if (item is SibRosReestr.EGRN.Unknown.CertificatesHoldersOut)
            {
                sr.Certificate_name = ((SibRosReestr.EGRN.Unknown.CertificatesHoldersOut)item).Certificate_name;
                sr.Name = $"Владельцы ипотечных сертификатов участия {sr.Certificate_name}";
                sr.Short_name = sr.Name;
                sr.Full_name = sr.Name;
            }
            if (item is SibRosReestr.EGRN.Unknown.EquityParticipantsInfo)
            {
                sr.Equity_participants = ((SibRosReestr.EGRN.Unknown.EquityParticipantsInfo)item).Equity_participants;
                sr.Name = $"Участники долевого строительства {sr.Equity_participants}";
                sr.Short_name = sr.Name;
                sr.Full_name = sr.Name;
            }

            if (item is SibRosReestr.EGRN.Unknown.NotEquityParticipantsInfo)
            {
                sr.Not_equity_participants = ((SibRosReestr.EGRN.Unknown.NotEquityParticipantsInfo)item).Not_equity_participants;
                sr.Name = $"частники долевого строительства по договорам участия в долевом строительстве, которым не переданы объекты долевого строительства {sr.Not_equity_participants}";
                sr.Short_name = sr.Name;
                sr.Full_name = sr.Name;
            }

            if (item is SibRosReestr.EGRN.Unknown.InvestmentUnitOwnerOut)
            {
                sr.Investment_unit_name = ((SibRosReestr.EGRN.Unknown.InvestmentUnitOwnerOut)item).Investment_unit_name;
                sr.Name = $"Владельцы инвестиционных паев {sr.Investment_unit_name}";
                sr.Short_name = sr.Name;
                sr.Full_name = sr.Name;
            }
            if (item is SibRosReestr.EGRN.Unknown.OtherSubject)
            {
                sr.Name = ((SibRosReestr.EGRN.Unknown.OtherSubject)item).Name;
                sr.Print_text = ((SibRosReestr.EGRN.Unknown.OtherSubject)item).Print_text;
                sr.Registration_organ = ((SibRosReestr.EGRN.Unknown.OtherSubject)item).Registration_organ;
                sr.Short_name = ((SibRosReestr.EGRN.Unknown.OtherSubject)item).Short_name;
                sr.Comment = ((SibRosReestr.EGRN.Unknown.OtherSubject)item).Comment;
                sr.Email = ((SibRosReestr.EGRN.Unknown.OtherSubject)item).Contacts?.Email;
                sr.Mailing_addess = ((SibRosReestr.EGRN.Unknown.OtherSubject)item).Contacts?.Mailing_addess;
                sr.Full_name = sr.Name;

            }
            if (item is SibRosReestr.EGRN.Unknown.Partnership)
            {
                sr.Name = "Инвестиционное товарищество";
                string str = "";

                if (((SibRosReestr.EGRN.Unknown.Partnership)item).Partnership_participants != null)
                {
                    SibRosReestr.EGRN.Unknown.Partnership ps = ((SibRosReestr.EGRN.Unknown.Partnership)item);
                    foreach (var p in ps.Partnership_participants)
                    {
                        if (p.Legal_entity != null && p.Legal_entity.Entity != null && p.Legal_entity.Entity.Item != null)
                        {
                            LegalSubject ls = CreateLegalEntitySubject(uofw, p.Legal_entity.Entity.Item);
                            ls.Partner = sr;
                            str += (!String.IsNullOrEmpty(str)) ? $"; {ls.Name}" : ls.Name;
                        }

                    }
                }

                sr.Name += " " + str;
                sr.Short_name = sr.Name;
                sr.Full_name = sr.Name;
            }
            var findName = sr.Name;
            SubjectRecord find = uofw.GetRepository<AnotherSubject>().Filter(f => !f.Hidden && f.Name == findName).FirstOrDefault();
            if (find != null)            
                sr = find;
            else            
                uofw.GetRepository<AnotherSubject>().Create(sr as AnotherSubject);
            
            return sr;
        }

        public static SubjectRecord CreateIndividualSubject(
             IUnitOfWork uofw
            , object item)
        {
            if (item == null) return null;

            SubjectRecord sr = null;

            if (!(item is SibRosReestr.EGRN.Unknown.IndividualOut)) return null;

            sr = new IndividualSubject();

            SibRosReestr.EGRN.Unknown.IndividualOut sb = item as SibRosReestr.EGRN.Unknown.IndividualOut;
            sr.Individual_typeCode = sb.Individual_type?.Code;
            sr.Individual_typeName = sb.Individual_type?.Value;

            //Согласно замечанию 351 в модуле РР не хранится информация о физическом лице.

            //sr.Surname = sb.Surname;
            //sr.FirstName = sb.Name;
            //sr.Patronymic = sb.Patronymic;
            //sr.Birth_date = ImportHelper.GetDate(sb.Birth_date);
            //sr.Birth_place = sb.Birth_place;
            //if (sb.Citizenship != null && sb.Citizenship.Item != null)
            //{
            //    if (sb.Citizenship.Item is SibRosReestr.EGRN.Unknown.NoCitizenshipPerson)
            //    {
            //        sr.No_citizenship = ((SibRosReestr.EGRN.Unknown.NoCitizenshipPerson)(sb.Citizenship.Item)).No_citizenship;
            //    }
            //    if (sb.Citizenship.Item is SibRosReestr.EGRN.Unknown.PersonCitizenshipCountry)
            //    {
            //        sr.Citizenship_countryCode = ((SibRosReestr.EGRN.Unknown.PersonCitizenshipCountry)(sb.Citizenship.Item)).Citizenship_country?.Code;
            //        sr.Citizenship_countryName = ((SibRosReestr.EGRN.Unknown.PersonCitizenshipCountry)(sb.Citizenship.Item)).Citizenship_country?.Value;
            //    }
            //}
            //sr.Snils = sb.Snils;
            //sr.Email = sb.Contacts?.Email;
            //sr.Mailing_addess = sb.Contacts?.Mailing_addess;

            //sr.Name = $"{sr.Surname} {sr.FirstName} {sr.Patronymic}";
            sr.Name = "Физическое лицо";
            sr.Short_name = sr.Name;
            sr.Full_name = sr.Name;

            //var findName = sr.Name;
            //SubjectRecord find = uofw.GetRepository<IndividualSubject>().Filter(f => !f.Hidden && f.Name == findName).FirstOrDefault();
            //if (find != null)
            //    sr = find;
            //else
            sr = uofw.GetRepository<IndividualSubject>().Create(sr as IndividualSubject);                       
            return sr;
        }

        public static SubjectRecord CreateLegalSubject(
             IUnitOfWork uofw
            , object item)
        {
            if (item == null) return null;

            SubjectRecord sr = null;

            if (item is SibRosReestr.EGRN.Unknown.LegalEntityOut)
            {
                sr = new LegalSubject();

                SibRosReestr.EGRN.Unknown.LegalEntityOut sb = item as SibRosReestr.EGRN.Unknown.LegalEntityOut;
                sr.Email = sb.Contacts?.Email;
                sr.Mailing_addess = sb.Contacts?.Mailing_addess;
                sr.TypeCode = sb.Type?.Code;
                sr.TypeName = sb.Type?.Value;

                if (sb.Entity != null && sb.Entity.Item != null)
                {
                    if (sb.Entity.Item is SibRosReestr.EGRN.Unknown.GovementEntity)
                    {
                        sr.Name = ((SibRosReestr.EGRN.Unknown.GovementEntity)(sb.Entity.Item)).Full_name;
                        sr.Full_name = sr.Name;
                        sr.Short_name = sr.Name;
                        sr.Inn = ((SibRosReestr.EGRN.Unknown.GovementEntity)(sb.Entity.Item)).Inn;
                        sr.Ogrn = ((SibRosReestr.EGRN.Unknown.GovementEntity)(sb.Entity.Item)).Ogrn;
                    }

                    if (sb.Entity.Item is SibRosReestr.EGRN.Unknown.NotResidentOut)
                    {
                        sr.Date_state_reg = ImportHelper.GetDate(((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Date_state_reg);
                        sr.Incorporate_countryCode = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Incorporate_country?.Code;
                        sr.Incorporate_countryName = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Incorporate_country?.Value;
                        sr.Incorporation_formCode = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Incorporation_form?.Code;
                        sr.Incorporation_formName = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Incorporation_form?.Value;
                        sr.Inn = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Inn;
                        sr.Name = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Name;
                        sr.Registration_number = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Registration_number.ToString();
                        sr.Registration_organ = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Registration_organ;
                        sr.Reg_address_subject = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(sb.Entity.Item)).Reg_address_subject;
                        sr.Full_name = sr.Name;
                        sr.Short_name = sr.Name;
                    }

                    if (sb.Entity.Item is SibRosReestr.EGRN.Unknown.ResidentOut)
                    {
                        sr.Incorporation_formCode = ((SibRosReestr.EGRN.Unknown.ResidentOut)(sb.Entity.Item)).Incorporation_form?.Code;
                        sr.Incorporation_formName = ((SibRosReestr.EGRN.Unknown.ResidentOut)(sb.Entity.Item)).Incorporation_form?.Value;
                        sr.Inn = ((SibRosReestr.EGRN.Unknown.ResidentOut)(sb.Entity.Item)).Inn;
                        sr.Name = ((SibRosReestr.EGRN.Unknown.ResidentOut)(sb.Entity.Item)).Name;
                        sr.Ogrn = ((SibRosReestr.EGRN.Unknown.ResidentOut)(sb.Entity.Item)).Ogrn;
                        sr.Full_name = sr.Name;
                        sr.Short_name = sr.Short_name;
                    }
                }

                var findName = sr.Name;
                var finn = sr.Inn;
                LegalSubject find = uofw.GetRepository<LegalSubject>()
                    .Filter(f => !f.Hidden && f.Inn == finn && f.Name == findName)
                    .FirstOrDefault();

                if (find != null)
                    sr = find;
                else
                    sr = uofw.GetRepository<LegalSubject>().Create(sr as LegalSubject);
            }
         
            return sr;
        }

        public static LegalSubject CreateLegalEntitySubject(
             IUnitOfWork uofw
            , object item)
        {

            if (item != null)
            {
                LegalSubject sr = new LegalSubject();
                if (item is SibRosReestr.EGRN.Unknown.GovementEntity)
                {
                    sr.Name = ((SibRosReestr.EGRN.Unknown.GovementEntity)(item)).Full_name;
                    sr.Full_name = sr.Name;
                    sr.Short_name = sr.Name;
                    sr.Inn = ((SibRosReestr.EGRN.Unknown.GovementEntity)(item)).Inn;
                    sr.Ogrn = ((SibRosReestr.EGRN.Unknown.GovementEntity)(item)).Ogrn;
                }

                if (item is SibRosReestr.EGRN.Unknown.NotResidentOut)
                {
                    sr.Date_state_reg = ImportHelper.GetDate(((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Date_state_reg);
                    sr.Incorporate_countryCode = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Incorporate_country?.Code;
                    sr.Incorporate_countryName = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Incorporate_country?.Value;
                    sr.Incorporation_formCode = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Incorporation_form?.Code;
                    sr.Incorporation_formName = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Incorporation_form?.Value;
                    sr.Inn = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Inn;
                    sr.Name = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Name;
                    sr.Registration_number = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Registration_number.ToString();
                    sr.Registration_organ = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Registration_organ;
                    sr.Reg_address_subject = ((SibRosReestr.EGRN.Unknown.NotResidentOut)(item)).Reg_address_subject;
                    sr.Full_name = sr.Name;
                    sr.Short_name = sr.Name;
                }

                if (item is SibRosReestr.EGRN.Unknown.ResidentOut)
                {
                    sr.Incorporation_formCode = ((SibRosReestr.EGRN.Unknown.ResidentOut)(item)).Incorporation_form?.Code;
                    sr.Incorporation_formName = ((SibRosReestr.EGRN.Unknown.ResidentOut)(item)).Incorporation_form?.Value;
                    sr.Inn = ((SibRosReestr.EGRN.Unknown.ResidentOut)(item)).Inn;
                    sr.Name = ((SibRosReestr.EGRN.Unknown.ResidentOut)(item)).Name;
                    sr.Ogrn = ((SibRosReestr.EGRN.Unknown.ResidentOut)(item)).Ogrn;
                    sr.Full_name = sr.Name;
                    sr.Short_name = sr.Short_name;
                }

                var findName = sr.Name;
                var finn = sr.Inn;
                LegalSubject find = uofw.GetRepository<LegalSubject>()
                    .Filter(f => !f.Hidden && f.Inn == finn && f.Name == findName)
                    .FirstOrDefault();

                if (find != null)
                    sr = find;
                else
                    sr = uofw.GetRepository<LegalSubject>().Create(sr as LegalSubject);
                return sr;
            }
            else
                return null;
        }

        public static SubjectRecord CreatePublicSubject(
             IUnitOfWork uofw
            , object item)
        {
            if (item == null) return null;

            SubjectRecord sr = null;

            if (item is SibRosReestr.EGRN.Unknown.PublicFormations
                && ((SibRosReestr.EGRN.Unknown.PublicFormations)item).Public_formation_type != null
                && ((SibRosReestr.EGRN.Unknown.PublicFormations)item).Public_formation_type.Item != null)
            {
                sr = new PublicSubject();

                object sb = ((SibRosReestr.EGRN.Unknown.PublicFormations)item).Public_formation_type.Item;

                if (sb is SibRosReestr.EGRN.Unknown.ForeignPublic)
                {
                    sr.ForeignPublicCode = ((SibRosReestr.EGRN.Unknown.ForeignPublic)sb).Name?.Code;
                    sr.ForeignPublicName = ((SibRosReestr.EGRN.Unknown.ForeignPublic)sb).Name?.Value;
                    sr.Name = sr.ForeignPublicName;
                    sr.Short_name = sr.Name;
                    sr.Full_name = sr.Name;
                }

                if (sb is SibRosReestr.EGRN.Unknown.Municipality)
                {
                    sr.MunicipalityName = ((SibRosReestr.EGRN.Unknown.Municipality)sb).Name;
                    sr.Name = sr.MunicipalityName;
                    sr.Short_name = sr.MunicipalityName;
                    sr.Full_name = sr.MunicipalityName;
                }

                if (sb is SibRosReestr.EGRN.Unknown.Russia)
                {
                    sr.RussiaCode = ((SibRosReestr.EGRN.Unknown.Russia)sb).Name?.Code;
                    sr.RussiaName = ((SibRosReestr.EGRN.Unknown.Russia)sb).Name?.Value;
                    sr.Name = sr.RussiaName;
                    sr.Short_name = sr.RussiaName;
                    sr.Full_name = sr.RussiaName;
                }

                if (sb is SibRosReestr.EGRN.Unknown.SubjectOfRF)
                {
                    sr.SubjectOfRFCode = ((SibRosReestr.EGRN.Unknown.SubjectOfRF)sb).Name?.Code;
                    sr.SubjectOfRFName = ((SibRosReestr.EGRN.Unknown.SubjectOfRF)sb).Name?.Value;
                    sr.Name = sr.SubjectOfRFName;
                    sr.Short_name = sr.SubjectOfRFName;
                    sr.Full_name = sr.SubjectOfRFName;
                }

                if (sb is SibRosReestr.EGRN.Unknown.UnionState)
                {
                    sr.UnionStateName = ((SibRosReestr.EGRN.Unknown.UnionState)sb).Name;
                    sr.Name = sr.UnionStateName;
                    sr.Short_name = sr.UnionStateName;
                    sr.Full_name = sr.UnionStateName;
                }

            }

            var findName = sr.Name;            
            PublicSubject find = uofw.GetRepository<PublicSubject>()
                .Filter(f => !f.Hidden && f.Name == findName)
                .FirstOrDefault();

            if (find != null)
                sr = find;
            else
                sr = uofw.GetRepository<PublicSubject>().Create(sr as PublicSubject);
            return sr;
        }
    }
}
