using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Contact.Entities;

namespace Base.Mail.Entities
{
    public class MessageTemplate
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<BaseContact> To { get; set; } 
    }
}
