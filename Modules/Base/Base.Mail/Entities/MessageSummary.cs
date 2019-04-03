using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using MimeKit;

namespace Base.Mail.Entities
{
    public class MessageSummary
    {
        public int Index { get; set; }
        public DateTime Date { get; set; }
        public uint UniqueId { get; set; }        
        public List<string> To { get; set; }        
        public List<string> Cc { get; set; }
        public MailFrom From { get; set; }
        public string Subject { get; set; }
        public bool HasAttachments { get; set; }
        public bool Unread { get; set; }
        public bool Flagged { get; set; }
    }

    public class MailFrom
    {
        public string Title { get; set; }
        public string Email { get; set; }

        public MailFrom(MailboxAddress address)
        {
            if (address != null)
            {
                Title = address.Name ?? address.Address;
                Email = address.Address;
            }
            else
            {
                Title = "No title";
                Email = "No email";
            }
        }
    }

    
}