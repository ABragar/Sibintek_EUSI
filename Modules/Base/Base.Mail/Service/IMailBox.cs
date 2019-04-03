using System.Collections.Generic;
using Base.Mail.Entities;

namespace Base.Mail.Service
{
    public interface IMailBox
    {
        void AddFlags(int[] ids, MailMessageFlags flags);
        MailFolder GetFolder(int id);
        IEnumerable<MailFolder> GetFolders();
        MailMessageViewModel GetMessage(int id);
        IEnumerable<MailMessageViewModel> GetMessages(int folderId, int skip = 0, int take = -1, string search = null);
        void RemoveFlags(int[] ids, MailMessageFlags flags);
        void Send(MailMessageViewModel msg);
        void Send(IEnumerable<string> mailto, string caption, string body);
    }
}