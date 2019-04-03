using System;
using System.Collections.Generic;
using Base.Attributes;
using WebApi.Proxies.Models;

namespace Base.MailAdmin.Entities
{
    public class MailAccount: BaseObject
    {
        [DetailView(ReadOnly = true)]
        public virtual string Name { get; set; }

        [DetailView(ReadOnly = true)]
        public virtual string AccountId { get; set; }

        [DetailView(ReadOnly = true)]
        public virtual DateTime? CreatedDate { get; set; }

        [DetailView(ReadOnly = true)]
        public virtual DateTime? LastLogonDate { get; set; }

        [DetailView]
        public virtual AccountStatus? Status { get; set; }

        [DetailView(ReadOnly = true)]
        public virtual long Size { get; set; }
        [DetailView]
        public virtual long Quota { get; set; }

        [DetailView(ReadOnly = true)]
        public IEnumerable<VmAccountAlias> AliasList { get; set; }

        [DetailView]
        public IEnumerable<VmForwardingList> ForwardingList { get; set; }
    }
}