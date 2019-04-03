using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Base.Contact.Entities;
using Base.Contact.Service.Concrete;
using Base.DAL;
using Base.Entities.Complex;
using Base.Enums;
using Base.Helpers;
using Base.Mail.Entities;
using Base.Service;
using Base.Settings;
using Base.UI.Service;
using Base.Utils.Common.Maybe;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using MailFolder = Base.Mail.Entities.MailFolder;
using AppContext = Base.Ambient.AppContext;

namespace Base.Mail.DAL
{
    public class MailClient : IMailClient
    {
        private readonly ISettingService<MailSetting> _settingService;
        private readonly IFileSystemService _fileSystemService;
        private readonly IContactService _contactService;
        private readonly IPresetService<ProfileMailSettings> _profileMailSettingsPresetService;


        public MailClient(ISettingService<MailSetting> settingService, IFileSystemService fileSystemService, IContactService contactService, IPresetService<ProfileMailSettings> profileMailSettingsPresetService)
        {
            _settingService = settingService;
            _fileSystemService = fileSystemService;
            _contactService = contactService;
            _profileMailSettingsPresetService = profileMailSettingsPresetService;
        }

        public async Task Send(IUnitOfWork uow, MailMessageViewModel msg)
        {
            var message = new MimeMessage();

            using (var client = await GetSmtpClient(uow))
            {
                message.From.Add(new MailboxAddress(AppContext.SecurityUser.ProfileInfo.FullName, AppContext.SecurityUser.ProfileInfo.Email));

                //foreach (var userTo in msg.To)
                //{
                //    var user = _userService.Get(uow, userTo.ID);

                //    if (user != null)
                //        message.To.Add(new MailboxAddress(user.FullName, user.Profile?.GetPrimaryEmail()));
                //}

                //foreach (var userCc in msg.Cc)
                //{
                //    var user = _userService.Get(uow, userCc.ID);

                //    if (user != null)
                //        message.Cc.Add(new MailboxAddress(user.FullName, user.Profile?.GetPrimaryEmail()));
                //}

                message.Subject = msg.Subject;

                var builder = new BodyBuilder { HtmlBody = msg.Body };

                message.Body = builder.ToMessageBody();

                client.Send(message);

                client.Disconnect(true);
            }

        }
        

        public void SystemSend(IEnumerable<string> mailto, string caption, string body)
        {
            var message = new MimeMessage();
            var settings = GetSystemSetting();
            using (var client = GetSystemSmtpClient(settings))
            {
                message.From.Add(new MailboxAddress(settings.SmtpAccountLogin, settings.EmailFrom));
                message.To.AddRange(mailto.Select(x => new MailboxAddress(x, x)));
                message.Subject = caption;
                var bodyBuilder = new BodyBuilder() { HtmlBody = body };

                message.Body = bodyBuilder.ToMessageBody();
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public async Task<IEnumerable<MailFolder>> GetFolders(IUnitOfWork uow, string parentFolder = null)
        {
            var settings = await GetSettings(uow);
            using (var imapClient = GetImapClient(settings))
            {
                var folder = parentFolder != null ? imapClient.GetFolder(parentFolder) : imapClient.GetFolder(imapClient.PersonalNamespaces[0]);

                var folders = folder.GetSubfolders();

                if (folders == null)
                    throw new Exception("Не удалось получить папки");

                folders = folders.OrderByDescending(x =>
                        x.Attributes == FolderAttributes.Inbox ||
                        x.Attributes == FolderAttributes.Sent ||
                        x.Attributes == FolderAttributes.Drafts ||
                        x.Attributes == FolderAttributes.Junk ||
                        x.Attributes == FolderAttributes.Trash);

                var retFolders = new List<MailFolder>();

                int id = 0;

                InitFolder(folders, retFolders, ref id);

                return retFolders;
            }
        }

        public async void MoveMessage(IUnitOfWork uow, string folder, string newFolder, uint messageId)
        {
            var settings = await GetSettings(uow);
            using (var client = GetImapClient(settings))
            {
                var oldFolder = client.GetFolder(folder);
                oldFolder.Open(FolderAccess.ReadWrite);

                var destFolder = client.GetFolder(newFolder);

                oldFolder.MoveTo(new UniqueId(messageId), destFolder);
                oldFolder.Close();
            }
        }

        public async Task<byte[]> GetFile(IUnitOfWork uow, string folderName, uint messageId, string fileName)
        {
            var settings = await GetSettings(uow);
            using (var client = GetImapClient(settings))
            {
                var folder = client.GetFolder(folderName);
                folder.Open(FolderAccess.ReadOnly);

                var id = new UniqueId(messageId);

                var message = folder.GetMessage(id);

                folder.Close();

                var file = message.Attachments.FirstOrDefault(x => x.ContentDisposition.FileName == fileName);
                if (file == null)
                    throw new FileNotFoundException($"Файл {fileName} не найден ");
                using (var stream = new MemoryStream())
                {
                    file.WriteTo(stream);
                    byte[] bytes = stream.GetBuffer();
                    return bytes;
                }
            }
        }

        public async void DeleteMessage(IUnitOfWork uow, string folderName, uint messageId)
        {
            var settings = await GetSettings(uow);
            using (var client = GetImapClient(settings))
            {
                var folder = client.GetFolder(folderName);
                folder.Open(FolderAccess.ReadWrite);

                var uid = new UniqueId(messageId);

                var msg = folder.GetMessage(uid);

                if (msg == null)
                    throw new Exception("Не найдено сообщение");

                folder.AddFlags(uid, MessageFlags.Deleted, false);

                folder.Expunge(new List<UniqueId>() { uid });
                folder.Close(true);
            }
        }

        public async void SetFlags(IUnitOfWork uow, string folderName, uint messageId, string flags)
        {
            var settings = await GetSettings(uow);

            MessageFlags f;
            Enum.TryParse(flags, true, out f);

            using (var client = GetImapClient(settings))
            {
                var folder = client.GetFolder(folderName);
                if (folder == null)
                    throw new Exception($"Папка {folderName} не найдена");


                folder.Open(FolderAccess.ReadWrite);
                folder.SetFlags(new UniqueId(messageId), f, false);
                folder.Close(true);

            }
        }

        public async void RemoveFlags(IUnitOfWork uow, string folderName, uint messageId, string flags)
        {
            var settings = await GetSettings(uow);

            MessageFlags f;
            Enum.TryParse(flags, true, out f);

            using (var client = GetImapClient(settings))
            {
                var folder = client.GetFolder(folderName);
                if (folder == null)
                    throw new Exception($"Папка {folderName} не найдена");

                folder.Open(FolderAccess.ReadWrite);
                folder.RemoveFlags(new UniqueId(messageId), f, false);
                folder.Close(true);
            }
        }

        public async Task<int> GetFolderCount(IUnitOfWork uow, string folderName)
        {
            var settings = await GetSettings(uow);
            using (var client = GetImapClient(settings))
            {
                var folder = client.GetFolder(folderName);
                if (folder == null)
                    throw new Exception($"Папка {folderName} не найдена");

                if (CanSelect(folder))
                {
                    folder.Status(StatusItems.Unread | StatusItems.Count);

                    return folder.Unread;
                }

                return 0;
            }
        }

        public async Task<MailMessageViewModel> GetMessage(IUnitOfWork uow, string folderPath, int id)
        {
            var settings = await GetSettings(uow);

            using (var client = GetImapClient(settings))
            {
                var folder = client.GetFolder(folderPath);

                if (folder == null)
                    throw new Exception($"Папка {folderPath} не найдена ");

                folder.Open(FolderAccess.ReadOnly);

                var mimeMessage = folder.GetMessage(id);

                if (mimeMessage == null)
                    throw new Exception($"Письмо под индексом {id} не найдено");

                var from = mimeMessage.From.Mailboxes.First();

                var contacts = _contactService.GetcontactByMail(uow, @from.Address);

                var contact = contacts.Any() ? contacts.First() : new Employee
                {
                    Title = @from.Name ?? @from.Address,
                    Emails = new List<ContactEmail>() { new ContactEmail() { Email = @from.Address } },
                };


                MailMessageViewModel m = new MailMessageViewModel
                {
                    Index = id,
                    UniqueId = mimeMessage.MessageId,
                    Body = mimeMessage.HtmlBody ?? mimeMessage.TextBody,
                    Subject = mimeMessage.Subject,
                    To = new List<string>(),
                    Cc = new List<string>(),
                    HasAttachments = mimeMessage.Attachments.Any(),
                    Date = mimeMessage.Date.DateTime,
                    Attachments = mimeMessage.Attachments.Select(x => LoadFileFromMail(uow, x)).ToList(),
                    From = new MailFrom(mimeMessage.From.Mailboxes.First()),
                    Contact = contact,
                };

                folder.Close();

                return m;
            }
        }

        public async Task<MailClientResult> GetMessages(IUnitOfWork uow, string folderPath, int page, int take, MailSorting sorting = null)
        {
            var result = new MailClientResult();
            var settings = await GetSettings(uow);
            using (var client = GetImapClient(settings))
            {
                var folder = client.GetFolder(folderPath);

                if (!CanSelect(folder))
                {
                    result.Messages = null;
                    result.Total = 0;
                    return result;
                }

                folder.Open(FolderAccess.ReadOnly);

                int min = folder.Count - take * page + 1;
                min = min > 0 ? min : 0;
                int max = min + take;

                var folderItems = folder.Fetch(min, max, MessageSummaryItems.Full | MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure);

                var msgs = folderItems.Select(messageSummary => new Entities.MessageSummary
                {
                    Index = messageSummary.Index,
                    Unread = (messageSummary.Flags & MessageFlags.Seen) == 0,
                    Flagged = (messageSummary.Flags & MessageFlags.Flagged) != 0,
                    UniqueId = messageSummary.UniqueId.Id,
                    Date = messageSummary.Date.DateTime,
                    To = messageSummary.Envelope.To.Select(x => x.Name).ToList(),
                    From = new MailFrom(messageSummary.Envelope.From.Mailboxes.FirstOrDefault()),
                    Cc = messageSummary.Envelope.Cc.Mailboxes.Select(x => x.Address).ToList(),
                    HasAttachments = messageSummary.Attachments.Any(),
                    Subject = messageSummary.Envelope.With(x => x.Subject)
                }).ToList();

                result.Messages = msgs.ToList().OrderByDescending(x => x.Date);
                result.Total = folder.Count;

                return result;
            }
        }

        #region Private

        private FileData LoadFileFromMail(IUnitOfWork uow, MimeEntity attachment) //TODO : Доработать файл не читается и сохораняется дохуя раз
        {
            using (var stream = new MemoryStream())
            {
                var mimePart = (MimePart)attachment;

                mimePart.ContentObject.DecodeTo(stream);

                Exception error;
                var fileData = _fileSystemService.SaveFile(stream, attachment.ContentType.MediaSubtype, out error);

                if (error != null)
                    throw error;

                fileData.FileName = attachment.ContentDisposition.FileName;

                uow.GetRepository<FileData>().Create(fileData);
                uow.SaveChanges();

                return fileData;
            }
        }

        private MailSetting GetSystemSetting()
        {
            var setting = _settingService.Get();

            if (setting == null)
                throw new Exception("MailClient => setting == null");

            return setting;
        }

        private async Task<ProfileMailSettings> GetSettings(IUnitOfWork uow)
        {
            return await _profileMailSettingsPresetService.GetAsync("Mail");
        }

        private ImapClient GetImapClient(ProfileMailSettings setting)
        {
            var imapClient = new ImapClient();

            imapClient.Connect(setting.ServerAddress, setting.ServerPort, setting.UseSsl);

            imapClient.Authenticate(setting.AccountLogin, setting.AccountPassword);

            return imapClient;
        }

        private static SmtpClient GetSystemSmtpClient(MailSetting setting)
        {
            var client = new SmtpClient();

            if (string.IsNullOrEmpty(setting.SmtpServerAddress))
            {
                throw new Exception("Не задан SMTP адрес сервера почты");
            }

            if (setting.SmtpServerPort == 0)
            {
                throw new Exception("Не задан SMTP порт сервера почты");
            }

            client.Connect(setting.SmtpServerAddress, setting.SmtpServerPort, setting.SmtpUseSsl);

            client.AuthenticationMechanisms.Remove("XOAUTH2");

            client.Authenticate(setting.SmtpAccountLogin, setting.SmtpAccountPassword);

            return client;
        }

        private async Task<SmtpClient> GetSmtpClient(IUnitOfWork uow)
        {
            var client = new SmtpClient();
            var settings = await GetSettings(uow);

            client.Connect(settings.SmtpServerAddress, settings.SmtpServerPort, settings.SmtpUseSsl);

            client.AuthenticationMechanisms.Remove("XOAUTH2");

            client.Authenticate(settings.AccountLogin, settings.AccountPassword);

            return client;
        }

        private void InitFolder(IEnumerable<IMailFolder> folders, ICollection<MailFolder> retFolders, ref int id, MailFolder parent = null)
        {
            foreach (var src in folders)
            {
                id++;
                // src.Open(FolderAccess.ReadOnly);        
                if ((src.Attributes & FolderAttributes.NoSelect) == 0)
                    src.Status(StatusItems.Unread | StatusItems.Count);

                var dest = new MailFolder
                {
                    ID = id,
                    Unread = src.Unread,
                    FullName = src.FullName,
                    Name = src.Name,
                    Count = src.Count,
                    Type = parent?.Type ?? (MailFolderType)src.Attributes,
                    Icon = new Icon() { Value = "glyphicon glyphicon-folder-closed" }
                };

                if (src.Attributes.HasFlag(FolderAttributes.Inbox))
                {
                    dest.Icon = new Icon()
                    {
                        Value = "glyphicon glyphicon-inbox-in"
                    };

                    dest.Name = "Входящие";
                }
                else if (src.Attributes.HasFlag(FolderAttributes.Sent))
                {
                    dest.Icon = new Icon()
                    {
                        Value = "glyphicon glyphicon-send"
                    };

                    dest.Name = "Отправленные";
                }
                else if (src.Attributes.HasFlag(FolderAttributes.Drafts))
                {
                    dest.Icon = new Icon()
                    {
                        Value = "glyphicon glyphicon-edit"
                    };

                    dest.Name = "Черновики";
                }
                else if (src.Attributes.HasFlag(FolderAttributes.Junk))
                {
                    dest.Icon = new Icon()
                    {
                        Value = "glyphicon glyphicon-bug"
                    };

                    dest.Unread = 0;

                    dest.Name = "Спам";
                }
                else if (src.Attributes.HasFlag(FolderAttributes.Trash))
                {
                    dest.Icon = new Icon()
                    {
                        Value = "glyphicon glyphicon-bin"
                    };

                    dest.Unread = 0;

                    dest.Name = "Корзина";
                }


                if (parent != null)
                {
                    dest.ParentID = parent.ID;
                    parent.hasChildren = true;
                }

                retFolders.Add(dest);
                InitFolder(src.GetSubfolders(), retFolders, ref id, dest);
            }
        }

        private bool CanSelect(IMailFolder folder)
        {
            if ((folder.Attributes & FolderAttributes.NoSelect) != 0)
                return false;
            return true;
        }

        #endregion
    }

    public class MailClientResult
    {
        public int Total { get; set; }

        public IEnumerable<Entities.MessageSummary> Messages { get; set; }
    }

    public static class EmailSearchFactory
    {
        public static OrderBy[] GetOrderBy(MailSorting sorting)
        {
            OrderBy order = OrderBy.Date;

            if (sorting != null)
            {
                if (sorting.Field == "Date")
                {
                    order = sorting.Direction == OrderDirection.Ask ? OrderBy.Date : OrderBy.ReverseDate;
                }

                else if (sorting.Field == "From")
                {
                    order = sorting.Direction == OrderDirection.Ask ? OrderBy.From : OrderBy.ReverseFrom;

                }
                else if (sorting.Field == "Subject")
                {
                    order = sorting.Direction == OrderDirection.Ask ? OrderBy.Subject : OrderBy.ReverseSubject;
                }
                else if (sorting.Field == "Flagged")
                {
                    //
                }
            }

            return new OrderBy[] { order };
        }
    }
}