using Base.Service;
using Base.Service.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Base.Mail.Entities;
using Base.Settings;
using Base.Utils.Common.Caching;
using MailMessage = System.Net.Mail.MailMessage;

namespace Base.Mail.Service
{    
    public class EmailService : IEmailService
    {

        private readonly ISettingService<MailSetting> _emailSettingsService;
        private readonly IPathHelper _pathHelper;
        private readonly ISimpleCacheWrapper _cache;
        private readonly ILogService _logService;


        public EmailService(ISettingService<MailSetting> emailSettingsService, IPathHelper pathHelper, ISimpleCacheWrapper cache, ILogService logService)
        {
            _emailSettingsService = emailSettingsService;
            _pathHelper = pathHelper;

            _cache = cache;
            _logService = logService;
        }

        public bool SendMail(string mailto, string caption, string message, bool isBodyHtml = false)
        {
            return SendMail(new[] { mailto }, caption, message, isBodyHtml) > 0;
        }

        public int SendMail(IEnumerable<string> mailto, string caption, string message, bool isBodyHtml = false)
        {
            int count = 0;

            var mails = mailto.Where(IsValidEmail).Select(to => CreateMailMessage(to, caption, message, isBodyHtml)).ToList();

            var mailSetting = _emailSettingsService.Get();

            var client = new SmtpClient
            {
                Host = mailSetting.SmtpServerAddress,
                Port = mailSetting.SmtpServerPort,
                EnableSsl = mailSetting.UseSsl,
                Credentials = new NetworkCredential(mailSetting.SmtpAccountLogin, mailSetting.SmtpAccountPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            //_logService.Log(String.Format("SendMail -- mails.count: {0}", mails.Count()));

            foreach (var mail in mails)
            {
                try
                {
                    client.Send(mail);

                    count++;
                }
                catch (Exception)
                {
                    //_logService.Log(String.Format("SendMail -- Mail: {0}; Error: {1}", mail, e.Message));
                }
            }

            client.Dispose();

            return count;
        }

        protected MailMessage CreateMailMessage(string mailto, string caption, string message, bool isBodyHtml)
        {

            string strErr = "";
            try
            {
                var mailSetting = _emailSettingsService.Get();

                if (mailSetting == null)
                    strErr += "Настройка с почтовым сервером отсутствует.";
                if (mailSetting != null && mailSetting.SmtpServerAddress == null)
                    strErr += (!String.IsNullOrEmpty(strErr)) ? Environment.NewLine : "" + "В настройках связи с почтовым сервером отсутствует информация об адресе сервера.";
                if (mailSetting != null && mailSetting.SmtpServerPort == null)
                    strErr += (!String.IsNullOrEmpty(strErr)) ? Environment.NewLine : "" + "В настройках связи с почтовым сервером отсутствует информация о порте соединения с сервером.";
                if (mailSetting != null && mailSetting.UseSsl == null)
                    strErr += (!String.IsNullOrEmpty(strErr)) ? Environment.NewLine : "" + "В настройках связи с почтовым сервером отсутствует информация об типе аутентификации с сервером.";




                var mail = new MailMessage
                {
                    From = String.IsNullOrEmpty(mailSetting.Title)
                        ? new MailAddress(mailSetting.EmailFrom)
                        : new MailAddress(mailSetting.EmailFrom, mailSetting.Title),
                    Subject = caption,
                    Body = message,
                    IsBodyHtml = isBodyHtml,
                    BodyEncoding = Encoding.UTF8,
                    HeadersEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };
                mail.To.Add(mailto);

                return mail;
            }
            catch (Exception ex)
            {
                strErr = strErr + Environment.NewLine + ex.Message;
                throw new Exception(strErr);
            }
        }


        public bool SendMail(string mailto, string caption, string templateName, Dictionary<string, string> placeholders, bool isBodyHtml = false)
        {
            return SendMail(new[] { mailto }, caption, templateName, placeholders, isBodyHtml) > 0;
        }

        public int SendMail(IEnumerable<string> mailto, string caption, string templateName, Dictionary<string, string> placeholders, bool isBodyHtml = false)
        {
            string body = BuildBody(templateName, placeholders);

            return SendMail(mailto, caption, body, isBodyHtml);
        }

        private string BuildBody(string templateName, Dictionary<string, string> placeholders)
        {
            AddUrlsToDictionary(placeholders);

            string template = (GetTemplateContent(templateName)).ReplacePlaceholders(placeholders).DeleteImages();

            return template;
        }


        private static readonly CacheAccessor<string> TemplateGroup = new CacheAccessor<string>(TimeSpan.FromHours(5));

        private string GetTemplateContent(string templateName)
        {

            return _cache.GetOrAdd(TemplateGroup, templateName, () =>
            {

                FileInfo fi = new FileInfo(Path.Combine(_pathHelper.GetAppDataDirectory(), "Templates", "Email", templateName + ".html"));


                return fi.Exists ? File.ReadAllText(fi.FullName) : "";

            });



        }

        private void AddUrlsToDictionary(IDictionary<string, string> dic)
        {
            //if (!dic.ContainsKey("AdminUrl")) dic.Add("AdminUrl", _urlHelper.AdminUrl);
            //if (!dic.ContainsKey("PublicUrl")) dic.Add("PublicUrl", _urlHelper.PublicUrl);
        }

        public static bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }

    public static class CustomMailerExtensions
    {
        public static string ReplacePlaceholders(this string template, Dictionary<string, string> dic)
        {
            return dic.Keys.Aggregate(template, (current, key) => current.Replace(String.Format("[{0}]", key), dic[key]));
        }

        public static string DeleteImages(this string template)
        {
            return Regex.Replace(template, @"<img[^>]*>", String.Empty, RegexOptions.IgnoreCase);
        }
    }
}