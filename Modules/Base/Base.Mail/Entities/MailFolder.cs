using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Entities.Complex;
using Base.UI;
using Base.Attributes;
using Newtonsoft.Json;

namespace Base.Mail.Entities
{
    public class MailFolder : ITreeNodeIcon
    {       
        public int ID { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        [DetailView(Name = "Иконка")]
        public Icon Icon { get; set; }

        public int? ParentID { get; set; }

        public ICollection<MailFolder> Childrens { get; set; }

        [SystemProperty]
        public int Count { get; set; }

        public MailFolderType Type { get; set; }

        // ReSharper disable once InconsistentNaming
        public bool hasChildren { get; set; }
        public int Unread { get; set; }
    }

    [Flags]
    public enum MailFolderType
    {
        None = 0,
        NoInferiors = 1,
        NoSelect = 2,
        Marked = 4,
        Unmarked = 8,
        NonExistent = 16,
        Subscribed = 32,
        Remote = 64,
        HasChildren = 128,
        HasNoChildren = 256,
        All = 512,
        Archive = 1024,
        Drafts = 2048,
        Flagged = 4096,
        Inbox = 8192,
        Junk = 16384,
        Sent = 32768,
        Trash = 65536,
    }
}
