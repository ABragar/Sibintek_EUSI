using Base.Attributes;

namespace Base.MailAdmin.Entities
{
    public class VmForwardingList : BaseObject
    {
        [DetailView, ListView]
        public virtual string Name { get; set; }
    }
}