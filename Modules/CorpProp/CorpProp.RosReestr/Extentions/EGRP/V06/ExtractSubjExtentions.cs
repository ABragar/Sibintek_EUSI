using Base.DAL;
using CorpProp.RosReestr.Entities;
using CorpProp.RosReestr.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Entities.Import;

namespace CorpProp.RosReestr.Extentions.EGRP.V06
{
    /// <summary>
    /// Предоставляет методы расширения для импорта выписки ЮЛ на ОНИ Росреестра.
    /// </summary>
    public static class ExtractSubjExtentions
    {
        #region V06
        /// <summary>
        /// Инициация импорта xml-выписки в модуль Росреестра.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="ext"></param>
        /// <param name="holder"></param>
        public static void ImportV06(
              this ExtractSubj extract
            , SibRosReestr.EGRP.V06.ExtractSubj.Extract ext
            , ref ImportHolder holder
            )
        {
            extract.SetServisInfo(ext.EDocument);
            if (ext.ReestrExtract == null) return;

            extract.SetDeclarAttribute(ext.ReestrExtract.DeclarAttribute);
            extract.SetExtractPeriod(ext.ReestrExtract.ExtractPeriod);

            if (ext.ReestrExtract.NoticeSubj != null)
                extract.CreateNotice(ext.ReestrExtract.NoticeSubj, holder.UnitOfWork);

            if (ext.ReestrExtract.RefusalSubj != null)
                extract.CreateRefusal(ext.ReestrExtract.RefusalSubj, holder.UnitOfWork);

            if (ext.ReestrExtract.ExtractSubjectRights != null && ext.ReestrExtract.ExtractSubjectRights.ExtractSubj != null)
            {
                //if (ext.ReestrExtract.ExtractSubjectRights.ExtractSubj.Count == 1)
                //{
                //    extract.SetSubject(ext.ReestrExtract.ExtractSubjectRights.ExtractSubj[0].Subject, holder.UnitOfWork);
                //    extract.CreateObjects(ext.ReestrExtract.ExtractSubjectRights.ExtractSubj[0].ObjectRight, ref holder);
                //}
                //else
                //{
                //    string err = "Файл содержит информацию о нескольких субъектах.";
                //    holder.ImportHistory.ImportErrorLogs.AddError(null, null, "", err, CorpProp.Helpers.ErrorType.System);
                //    holder.Report += err;
                //}

                extract.SetSubject(ext.ReestrExtract.ExtractSubjectRights.ExtractSubj[0].Subject, holder.UnitOfWork);

                foreach (var item in ext.ReestrExtract.ExtractSubjectRights.ExtractSubj)
                {
                    ExtractSubj fake = new ExtractSubj();
                    fake.SetSubject(item.Subject, holder.UnitOfWork);
                    extract.CreateObjects(item.ObjectRight, fake.SubjectRecord, ref holder);
                }

            }
        }

        /// <summary>
        /// Устанавливает сервисную информацию.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="ext"></param>
        /// <param name="holder"></param>
        private static void SetServisInfo(
            this ExtractSubj extract
            , SibRosReestr.EGRP.V06.ExtractSubj.TServisInf info
        )
        {
            if (info == null) return;
            extract.SetSender(info.Sender);
            extract.SetRecipient(info.Recipient);
            extract.CodeType = info.CodeType;
            extract.Version = info.Version;
            extract.Scope = ImportHelper.GetCodeEnum(info.Scope);
        }

        /// <summary>
        /// Устанавливает информацию об отправителе.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="sender"></param>
        /// <param name="holder"></param>
        private static void SetSender(
            this ExtractSubj extract
            , SibRosReestr.EGRP.V06.ExtractSubj.TServisInfSender sender
        )
        {
            if (sender == null) return;
            extract.SenderKod = sender.Kod;
            extract.SenderName = sender.Name;
            extract.RegionCode = ImportHelper.GetCodeEnum(sender.Region);
            extract.RegionName = ImportHelper.GetNameEnum(sender.Region);
            extract.DateUpload = ImportHelper.GetDate(sender.Date_Upload);
            extract.Appointment = sender.Appointment;
            extract.FIO = sender.FIO;
            extract.EMail = sender.E_Mail;
            extract.Telephone = sender.Telephone;
        }

        /// <summary>
        /// Устанавливает информацию о получаетеле.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="recipient"></param>
        /// <param name="holder"></param>
        private static void SetRecipient(
            this ExtractSubj extract
            , SibRosReestr.EGRP.V06.ExtractSubj.TServisInfRecipient recipient
        )
        {
            if (recipient == null) return;
            extract.RecipientKod = recipient.Kod;
            extract.RecipientName = recipient.Name;
        }

        /// <summary>
        /// Устанавливает атрибуты итогового запроса.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="obj"></param>
        private static void SetDeclarAttribute(
           this ExtractSubj extract
           , SibRosReestr.EGRP.V06.ExtractSubj.TExtrAttribut obj
        )
        {
            if (obj == null) return;
            extract.ReceivName = obj.ReceivName;
            extract.Representativ = obj.Representativ;
            extract.ReceivAdress = obj.ReceivAdress;
            extract.ID_KUVI = obj.ID_KUVI;
            extract.ExtractTypeCode = ImportHelper.GetCodeEnum(obj.ExtractTypeCode);
            extract.ExtractTypeText = obj.ExtractTypeText;
            extract.ExtractNumber = obj.ExtractNumber;
            extract.ExtractDate = ImportHelper.GetDate(obj.ExtractDate);
            extract.NumberRequest = obj.RequeryNumber;
            extract.RequeryDate = obj.RequeryDate;
            extract.DateRequest = ImportHelper.GetDate(obj.RequeryDate);
            extract.OfficeNumber = obj.OfficeNumber;
            extract.OfficeDate = obj.OfficeDate;
            extract.DateReceipt = ImportHelper.GetDate(obj.OfficeDate);
            extract.ExtractCount = obj.ExtractCount;
            extract.NoticeCount = obj.NoticeCount;
            extract.RefuseCount = obj.RefuseCount;
            extract.Registrator = obj.Registrator;
            extract.Name = $"{extract.ExtractNumber} {extract.ExtractDate?.ToString("d")}";
        }

        /// <summary>
        /// Устанавливает период.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="obj"></param>
        private static void SetExtractPeriod(
             this ExtractSubj extract
           , SibRosReestr.EGRP.V06.ExtractSubj.ExtractReestrExtractExtractPeriod obj
            )
        {
            if (obj == null) return;
            extract.ExtractPeriodSDate = obj.ExtractPeriodSDate;
            extract.ExtractPeriodEDate = obj.ExtractPeriodEDate;

            extract.StartDate = ImportHelper.GetDate(obj.ExtractPeriodSDate);
            extract.EndDate = ImportHelper.GetDate(obj.ExtractPeriodEDate);
        }

        /// <summary>
        /// Создает уведомление об отсутсвии сведений.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="obj"></param>
        /// <param name="uow"></param>
        private static void CreateNotice(
              this ExtractSubj extract
           , SibRosReestr.EGRP.V06.ExtractSubj.TNoticeSubj obj
           , IUnitOfWork uow
            )
        {
            if (obj == null || obj.NoticeSubj == null) return;

            Notice not = uow.GetRepository<Notice>().Create(new Notice());
            foreach (var item in obj.NoticeSubj)
            {
                NoticeSubj ns = uow.GetRepository<NoticeSubj>().Create(new NoticeSubj());
                ns.Notice = not;
                ns.TypeInfoText = item.TypeInfoText?.ToString();
                ns.ArrestInfo = item.ArrestInfo;
                if (item.Item != null)
                {
                    if (item.Item is SibRosReestr.EGRP.V06.ExtractSubj.TSubject)
                        ns.Item = ImportHelper.GetSubjectInfo(item.Item);
                    else
                        ns.Item = item.Item.ToString();
                }
                not.TypeInfoText += (!String.IsNullOrEmpty(ns.TypeInfoText)) ? ("; " + ns.TypeInfoText) : ns.TypeInfoText;
                not.ArrestInfo += (!String.IsNullOrEmpty(ns.ArrestInfo)) ? ("; " + ns.ArrestInfo) : ns.ArrestInfo;
                not.Item += (!String.IsNullOrEmpty(ns.Item)) ? ("; " + ns.Item) : ns.Item;
            }
            extract.Notice = not;
        }

        /// <summary>
        /// Создает отказ в выдаче сведений.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="obj"></param>
        /// <param name="uow"></param>
        private static void CreateRefusal(
            this ExtractSubj extract
         , SibRosReestr.EGRP.V06.ExtractSubj.TRefusalSubj obj
         , IUnitOfWork uow
          )
        {
            if (obj == null || obj.RefusalSubj == null) return;

            Refusal not = uow.GetRepository<Refusal>().Create(new Refusal());
            foreach (var item in obj.RefusalSubj)
            {
                RefusalSubj ns = uow.GetRepository<RefusalSubj>().Create(new RefusalSubj());
                ns.Refusal = not;
                ns.TypeInfoText = item.TypeInfoText?.ToString();
                ns.TextRefusal = item.TextRefusal;
                ns.RefusalType = ImportHelper.GetCodeEnum(item.RefusalType);
                ns.RefusalTypeText = item.RefusalTypeText;
                if (item.Item != null)
                {
                    if (item.Item is SibRosReestr.EGRP.V06.ExtractSubj.TSubject)
                        ns.Item = ImportHelper.GetSubjectInfo(item.Item);
                    else
                        ns.Item = item.Item.ToString();
                }
                not.TypeInfoText += (!String.IsNullOrEmpty(ns.TypeInfoText)) ? ("; " + ns.TypeInfoText) : ns.TypeInfoText;
                not.TextRefusal += (!String.IsNullOrEmpty(ns.TextRefusal)) ? ("; " + ns.TextRefusal) : ns.TextRefusal;
                not.Item += (!String.IsNullOrEmpty(ns.Item)) ? ("; " + ns.Item) : ns.Item;
            }
            extract.Refusal = not;
        }

        /// <summary>
        /// Устанавливает информацию о субъекте выписки.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="obj"></param>
        /// <param name="uow"></param>
        private static void SetSubject(
              this ExtractSubj extract
         , SibRosReestr.EGRP.V06.ExtractSubj.TSubject obj
         , IUnitOfWork uow
        )
        {
            if (obj == null || obj.Item == null) return;

            if (obj == null || obj.Item == null) return;
            extract.SetSubjectRecord(uow, obj);
            extract.SetSociety(uow, obj);

        }
        private static void SetSociety(
         this ExtractSubj extract
        , IUnitOfWork uow
        , SibRosReestr.EGRP.V06.ExtractSubj.TSubject obj
     )
        {
            if (obj == null || obj.Item == null) return;

            var item = obj.Item;
            if (item is SibRosReestr.EGRP.V06.ExtractSubj.TOrganization)
            {
                SibRosReestr.EGRP.V06.ExtractSubj.TOrganization org =
                    item as SibRosReestr.EGRP.V06.ExtractSubj.TOrganization;
                if (org == null) return;
                extract.Society = ImportHelper.FindSociety(uow, org.Item, org.Code_OGRN);
            }
            if (item is SibRosReestr.EGRP.V06.ExtractSubj.TPerson)
            {
                SibRosReestr.EGRP.V06.ExtractSubj.TPerson pers =
                    item as SibRosReestr.EGRP.V06.ExtractSubj.TPerson;
                if (pers == null) return;
                extract.Society = ImportHelper.FindSociety(uow, pers.INN, "");
            }
        }

        private static void SetSubjectRecord(
           this ExtractSubj extract
           , IUnitOfWork uow
           , SibRosReestr.EGRP.V06.ExtractSubj.TSubject obj
      )
        {
            if (obj == null || obj.Item == null) return;
            var item = obj.Item;
            if (item is SibRosReestr.EGRP.V06.ExtractSubj.TOrganization)
            {
                SibRosReestr.EGRP.V06.ExtractSubj.TOrganization org =
                    item as SibRosReestr.EGRP.V06.ExtractSubj.TOrganization;
                if (org == null) return;
                extract.SubjectRecord = SubjectExtentions.FindOrCreateSubject(uow, org);
            }
            if (item is SibRosReestr.EGRP.V06.ExtractSubj.TPerson)
            {
                SibRosReestr.EGRP.V06.ExtractSubj.TPerson pers =
                    item as SibRosReestr.EGRP.V06.ExtractSubj.TPerson;
                if (pers == null) return;
                extract.SubjectRecord = SubjectExtentions.FindOrCreateSubject(uow, pers);
            }
            if (extract.SubjectRecord != null)
            {
                extract.SubjectINN = extract.SubjectRecord.Inn;
                extract.SubjectName = extract.SubjectRecord.Name;
            }
        }
        /// <summary>
        /// Создает ОНИ, права, ограничения и т.п.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="objs"></param>
        /// <param name="history"></param>
        private static void CreateObjects(
              this ExtractSubj extract
            , List<SibRosReestr.EGRP.V06.ExtractSubj.ExtractReestrExtractExtractSubjectRightsExtractSubjObjectRight> objs
            , SubjectRecord sr
            , ref ImportHolder holder
         )
        {
            if (objs == null) return;
            //TODO: исправить создание и сохранение тысяч объектов в одной сессии
            // пока грузим по 100
            var count = objs.Count();
            //if (count > 101)
            //    count = 101;
            for( int i = 0; i < count; i++)
            //foreach (var item in objs)
            {
                var item = objs[i];
                RightRecord rr = extract.CreateRight(item.Registration, sr, ref holder);
                ObjectRecord obj = extract.CreateObject(item.Object, ref holder);
                rr.ObjectRecord = obj;
                extract.CreateEncumbrances(rr, obj, item.Encumbrance, ref holder);

                extract.CountRights++;
                extract.CountObjects++;
            }
        }

        /// <summary>
        /// Создает запись о праве.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="obj"></param>
        /// <param name="holder"></param>
        /// <returns></returns>
        private static RightRecord CreateRight(
              this ExtractSubj extract
             , SibRosReestr.EGRP.V06.ExtractSubj.TOpenRegistration obj
            , SubjectRecord sr
             , ref ImportHolder holder
            )
        {
            if (obj == null) return null;
            try
            {
                RightRecord rr = holder.UnitOfWork.GetRepository<RightRecord>().Create(new RightRecord());
                rr.Extract = extract;
                rr.Import(obj,sr, ref holder);
                return rr;
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
            return null;
        }


        /// <summary>
        /// Создает ОНИ.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="obj"></param>
        /// <param name="holder"></param>
        /// <returns></returns>
        private static ObjectRecord CreateObject(
             this ExtractSubj extract
            , SibRosReestr.EGRP.V06.ExtractSubj.TObject obj
            , ref ImportHolder holder
           )
        {
            if (obj == null) return null;
            try
            {
                ObjectRecord rr = holder.UnitOfWork.GetRepository<ObjectRecord>().Create(new ObjectRecord());
                rr.Extract = extract;
                rr.Import(obj, ref holder);

                return rr;
            }
            catch (Exception ex)
            {
                holder.ImportHistory.ImportErrorLogs.AddError(ex);
            }
            return null;
        }


        /// <summary>
        /// Создает ограничения/обременения.
        /// </summary>
        /// <param name="extract"></param>
        /// <param name="rr"></param>
        /// <param name="oni"></param>
        /// <param name="objs"></param>
        /// <param name="holder"></param>
        private static void CreateEncumbrances(
             this ExtractSubj extract
            , RightRecord rr
            , ObjectRecord oni
            , List<SibRosReestr.EGRP.V06.ExtractSubj.TRightSubjEncumbrance> objs
            , ref ImportHolder holder
           )
        {
            if (objs == null || rr == null || oni == null) return;

            foreach (var item in objs)
            {
                try
                {
                    RestrictRecord enc = holder.UnitOfWork.GetRepository<RestrictRecord>().Create(new RestrictRecord());
                    enc.Import(item, ref holder);
                    enc.Extract = extract;
                    enc.ObjectRecord = oni;
                    enc.RightRecord = rr;
                }
                catch (Exception ex)
                {
                    holder.ImportHistory.ImportErrorLogs.AddError(ex);
                }
            }
        }

        #endregion//V06
    }
}
