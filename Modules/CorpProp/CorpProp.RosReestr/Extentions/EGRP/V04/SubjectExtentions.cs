using Base.DAL;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Extentions.EGRP.V04
{
    public static class SubjectExtentions
    {
        public static SubjectRecord FindOrCreateSubject(IUnitOfWork uow, SibRosReestr.EGRP.V04.ExtractSubj.TPerson obj)
        {
            SubjectRecord sr = null;

            if (obj == null) return null;

            if (!String.IsNullOrEmpty(obj.INN))
                sr = uow.GetRepository<SubjectRecord>().Filter(s => !s.Hidden && s.Inn == obj.INN).FirstOrDefault();

            if (sr == null)
                return CreateSubject(uow, obj);

            return sr;
        }
        public static SubjectRecord FindOrCreateSubject(IUnitOfWork uow, SibRosReestr.EGRP.V04.ExtractSubj.TOrganization obj)
        {
            SubjectRecord sr = null;

            if (obj == null) return null;

            if (!String.IsNullOrEmpty(obj.Item))
                sr = uow.GetRepository<SubjectRecord>().Filter(s => !s.Hidden && s.Inn == obj.Item).FirstOrDefault();

            if (sr == null && !String.IsNullOrEmpty(obj.Code_OGRN))
            {
                sr = uow.GetRepository<SubjectRecord>().Filter(s => !s.Hidden && s.Ogrn == obj.Code_OGRN).FirstOrDefault();
            }
            if (sr == null)
                return CreateSubject(uow, obj);

            return sr;
        }

        private static SubjectRecord CreateSubject(IUnitOfWork uow, SibRosReestr.EGRP.V04.ExtractSubj.TOrganization obj)
        {
            if (obj == null) return null;

            SubjectRecord sr = uow.GetRepository<SubjectRecord>().Create(new SubjectRecord());
            sr.Code_SP = ImportHelper.GetCodeEnum(obj.Code_SP);
            sr.Content = obj.Content;
            if (obj.ItemElementName == SibRosReestr.EGRP.V04.ExtractSubj.ItemChoiceType.INN)
                sr.Inn = obj.Item;
            sr.Ogrn = obj.Code_OGRN;
            sr.Date_state_reg = ImportHelper.GetDate(obj.RegDate);
            sr.Registration_organ = obj.AgencyRegistration;
            sr.Reg_address_subject = obj.Location?.Content;
            sr.Email = obj.Email;
            sr.Short_name = sr.Name = obj.Name;
            sr.Content = obj.Content;
            return sr;
        }

        private static SubjectRecord CreateSubject(IUnitOfWork uow, SibRosReestr.EGRP.V04.ExtractSubj.TPerson obj)
        {
            if (obj == null) return null;

            SubjectRecord sr = uow.GetRepository<SubjectRecord>().Create(new SubjectRecord());
            sr.Code_SP = ImportHelper.GetCodeEnum(obj.Code_SP);
            sr.Content = obj.Content;

            //Согласно замечанию 351 в модуле РР не хранится информация о физическом лице.

            //sr.Inn = obj.INN;
            //sr.Surname = obj.FIO?.Surname;
            //sr.FirstName = obj.FIO?.First;
            //sr.Patronymic = obj.FIO?.Patronymic;
            //sr.Reg_address_subject = obj.Location?.Content;
            //sr.Short_name = sr.Name = $"{sr.Surname} {sr.FirstName} {sr.Patronymic}";
            sr.Name = "Физическое лицо";
            sr.Full_name = sr.Name;
            sr.Short_name = sr.Name;
            return sr;
        }

        public static SubjectRecord FindOrCreateSubject(IUnitOfWork uow, SibRosReestr.EGRP.V04.ExtractSubj.TGovernance obj)
        {
            SubjectRecord sr = null;

            if (obj == null) return null;

            sr = uow.GetRepository<SubjectRecord>().Filter(s => !s.Hidden && s.Inn == obj.Name).FirstOrDefault();
            if (sr == null)
                return CreateSubject(uow, obj);

            return sr;
        }

        private static SubjectRecord CreateSubject(IUnitOfWork uow, SibRosReestr.EGRP.V04.ExtractSubj.TGovernance obj)
        {
            if (obj == null) return null;

            SubjectRecord sr = uow.GetRepository<SubjectRecord>().Create(new SubjectRecord());
            sr.Code_SP = ImportHelper.GetCodeEnum(obj.Code_SP);
            sr.Content = obj.Content;
            sr.Reg_address_subject = obj.Address;
            sr.Name = obj.Name;
            return sr;
        }
    }
}
