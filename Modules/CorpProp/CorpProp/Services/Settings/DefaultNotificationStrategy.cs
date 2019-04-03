using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Base;
using Base.DAL;
using Base.Extensions;
using Base.Notification.Entities;
using CorpProp.Entities.Import;
using CorpProp.Entities.Settings;

namespace CorpProp.Services.Settings
{
    public class DefaultNotificationStrategy : INotificationStrategy
    {
        protected Dictionary<string, UserNotificationTemplate> UserNotificationTemplateCache = new Dictionary<string, UserNotificationTemplate>();

        protected string _templateCode;

        public void Init(ImportHistory importHistory)
        {
            _templateCode = importHistory.ImportErrorLogs.Any() ? GetFailCode() : GetSuccessCode();
        }

        public void Init(string templateCode)
        {
            _templateCode = templateCode;
        }

        public virtual UserNotificationTemplate GetNotificationTemplate(IUnitOfWork uow)
        {
            var templateCode = this.GetTemplateCode();

            if (string.IsNullOrEmpty(templateCode))
            {
                return null;
            }

            return UserNotificationTemplateCache.GetOrAdd<string, UserNotificationTemplate>(templateCode,
                x => uow.GetRepository<UserNotificationTemplate>()
                    .Filter(f => !f.Hidden
                                 && f.Code.ToLower() == templateCode)
                    .Include(inc => inc.Report)
                    .FirstOrDefault());
        }

        protected virtual string GetTemplateCode()
        {
            return _templateCode;
        }

        protected virtual string GetSuccessCode()
        {
            return @"ImportHistory_Success";
        }

        protected virtual string GetFailCode()
        {
            return @"ImportHistory_Fail";
        }

        public virtual string[] GetEmails(UserNotificationTemplate notificationTemplate, int? userID, string email)
        {
            var tEmail = string.Empty;
            if (!String.IsNullOrWhiteSpace(notificationTemplate?.Recipient))
            {
                tEmail = notificationTemplate.Recipient;
                //TFS: 15377
                if (!string.IsNullOrEmpty(notificationTemplate.Code) && (notificationTemplate?.Code.ToLower() == "rentalosimporthistory_fail" || notificationTemplate?.Code.ToLower() == "rentalosimporthistory_success"))
                {
                    tEmail += (!String.IsNullOrEmpty(email)) ? (";" + email) : "";
                }
            }

            //TODO: Доработать. Добавить в шаблоны признак "Добавить фиксированных получателей".
            //Если признак = True то добавлять к "email", если нет то отправлять только "Фиксированным получателям" (если они указаны), иначе только тем кто в "email"

            if (!String.IsNullOrEmpty(tEmail))
                email = tEmail;
            else
                email = (!String.IsNullOrWhiteSpace(email)) ? email : tEmail;
            return email.Split(';');
        }
    }
}