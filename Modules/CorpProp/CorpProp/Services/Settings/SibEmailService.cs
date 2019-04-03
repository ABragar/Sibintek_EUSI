using Base;
using Base.DAL;
using Base.Extensions;
using Base.Mail.Entities;
using Base.Mail.Service;
using Base.Service;
using Base.Service.Log;
using Base.Settings;
using Base.Utils.Common;
using Base.Utils.Common.Caching;
using CorpProp.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using CorpProp.Extentions;
using Base.Security;
using Ambient = Base.Ambient;
using Base.Entities.Complex;
using CorpProp.Entities.Import;
using Exception = System.Exception;

namespace CorpProp.Services.Settings
{
    public interface ISibEmailService : IEmailService
    {
        bool SendNotice(
              IUnitOfWork uow
             , object obj
             , int? userID
             , string email
             , string tmpCode
             , BaseObject link = null
             );

        void SetNotificationStrategyByMnemonic(string mnemonic);

        void SetNotificationStrategy(DefaultNotificationStrategy notificationStrategy);

        void SendImportNotice(ITransactionUnitOfWork uow, List<ImportHistory> items, ref int sent, ref int noSent);
    }

    public class SibEmailService : EmailService, ISibEmailService
    {
        private readonly ISettingService<MailSetting> _emailSettingsService;
        private INotificationStrategy _notificationstrategy;

        public SibEmailService(ISettingService<MailSetting> emailSettingsService, IPathHelper pathHelper, ISimpleCacheWrapper cache, ILogService logService) :
            base(emailSettingsService, pathHelper, cache, logService)
        {
            _emailSettingsService = emailSettingsService;
        }

        public bool SendNotice(
             IUnitOfWork uow
            , object obj
            , int? userID
            , string email
            , string tmpCode
            , BaseObject link = null
            )
        {
            int fail = 0;

            if (_notificationstrategy == null)
            {
                _notificationstrategy = new DefaultNotificationStrategy();
                _notificationstrategy.Init(tmpCode);
            }

            if (userID != null && String.IsNullOrEmpty(email))
            {
                var sibUser = Ambient.AppContext.SecurityUser.GetSibUser(uow);
                if (sibUser != null)
                    email = sibUser.Email;
            }

            UserNotificationTemplate notificationTemplate = _notificationstrategy.GetNotificationTemplate(uow);

            UserNotification notice = null;
            if (notificationTemplate != null)
            {
                if (notificationTemplate.ByEmail)
                {
                    var emails = _notificationstrategy.GetEmails(notificationTemplate, userID, email).Where(IsValidEmail).ToList();
                    if (emails.Count == 0)
                    {
                        return false;
                    }
                    foreach (var to in emails)
                    {
                        notice = uow.GetRepository<UserNotification>().Create(notificationTemplate.CreateNotice(obj, userID, link));
                        notice.EmailRecipient = to;
                        fail = (SendNoticeByMail(notice)) ? 0 : 1;
                    }
                }
                else
                    uow.GetRepository<UserNotification>().Create(notificationTemplate.CreateNotice(obj, userID));
            }
            else
                return false;
            return (fail == 0);
        }

        public void SetNotificationStrategyByMnemonic(string mnemonic)
        {
            switch (mnemonic)
            {
                case "EstateRegistration":
                    {
                        this._notificationstrategy = new EstateRegistrationNotificationStrategy();
                        break;
                    }
                case "RentalOS":
                    {
                        this._notificationstrategy = new RentalOsNotificationstrategy();
                        break;
                    }
                default:
                    {
                        this._notificationstrategy = new DefaultNotificationStrategy();
                        break;
                    }
            }
        }

        public void SetNotificationStrategy(DefaultNotificationStrategy notificationStrategy)
        {
            this._notificationstrategy = notificationStrategy;
        }

        public void SendImportNotice(ITransactionUnitOfWork uow, List<ImportHistory> items, ref int sent, ref int noSent)
        {
            foreach (var item in items)
            {
                _notificationstrategy.Init(item);
                var template = _notificationstrategy.GetNotificationTemplate(uow);

                if (template == null)
                {
                    noSent++;
                    continue;
                }

                if (template.ByEmail)
                {
                    var emails = _notificationstrategy.GetEmails(template, null, item.ContactEmail);
                    var validEmails = emails.Where(IsValidEmail).ToList();
                    if (validEmails.Count == 0)
                    {
                        noSent++;
                        continue;
                    };

                    foreach (var to in validEmails)
                    {
                        var notice = template.CreateNotice(item, null);
                        notice.EmailRecipient = to;
                        notice = uow.GetRepository<UserNotification>().Create(notice);

                        if (SendNoticeByMail(notice))
                        {
                            sent++;
                            item.IsResultSentByEmail = true;
                            if (item.SentByEmailDate == null)
                            {
                                item.SentByEmailDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            noSent++;
                        };
                    }
                }
                else
                    uow.GetRepository<UserNotification>().Create(template.CreateNotice(item, null));
            }
        }

        private bool SendNoticeByMail(UserNotification notice)
        {
            int fail = 0;

            var mail = CreateMailMessage(
                notice.EmailRecipient
                , notice.Title
                , (notice.IsHTML) ? notice.HtmlBody : notice.Description
                , notice.IsHTML);

            var mailSetting = _emailSettingsService.Get();

            var client = new SmtpClient
            {
                Host = mailSetting.SmtpServerAddress,
                Port = mailSetting.SmtpServerPort,
                EnableSsl = mailSetting.SmtpUseSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            if (mailSetting.SmtpWithoutCredentials)
            {
                client.UseDefaultCredentials = false;
                client.Credentials = null;
            }
            else
                client.Credentials = new NetworkCredential(mailSetting.SmtpAccountLogin, mailSetting.SmtpAccountPassword);
            try
            {
                client.Send(mail);
                notice.IsSentByEmail = true;
            }
            catch (Exception ex)
            {
                fail = 1;
                notice.EmailSendError = ex.ToStringWithInner();
            }
            client.Dispose();
            return (fail == 0);
        }
    }
}