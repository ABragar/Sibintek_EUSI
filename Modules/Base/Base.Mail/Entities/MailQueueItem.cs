using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Entities.Complex;

namespace Base.Mail.Entities
{
    public class MailQueueItem : BaseObject
    {
        public string Title { get; set; }
        public string Body { get; set; }        
        public string To { get; set; }
        public DateTime? Date { get; set; }
        public bool Processed { get; set; }
        public LinkBaseObject Entity { get; set; } = new LinkBaseObject();
        public string Source { get; set; }
    }
}
