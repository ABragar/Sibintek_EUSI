using System;
using System.Collections.Generic;
using System.Linq;
using Base.Contact.Entities;
using Base.Utils.Common.Maybe;

namespace Base.Mail.Entities
{
    public class MailMessageViewModel
    {
        public virtual FileData Image { get; set; }
        public int Index { get; set; }                      
        public DateTime Date { get; set; }
        public string UniqueId { get; set; }        
        public MailFrom From { get; set; }
        public string ToFirst => To.With(x => x.FirstOrDefault());
        public List<string> To { get; set; }
        public string CcFirst => Cc.With(x => x.FirstOrDefault());
        public List<string> Cc { get; set; }        
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool HasAttachments { get; set; }
        public List<FileData> Attachments { get; set; }
        public BaseContact Contact { get; set; }
    }

    public class MailMessageAttachment
    {
        public string FileName { get; set; }
        public Uri Uri { get; set; }
    }

    [Flags]
    public enum MailMessageFlags
    {
        None = 0,
        Seen = 1,
        Answered = 2,
        Flagged = 4,
        Deleted = 8,
        Draft = 16,
        Recent = 32,
        UserDefined = 64,
        Junk = 128,
        Hidden = 256
    }
}
