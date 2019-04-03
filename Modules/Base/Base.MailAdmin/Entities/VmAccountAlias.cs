using Base.Attributes;

namespace Base.MailAdmin.Entities
{
    public class VmAccountAlias : BaseObject
    {
        [DetailView, ListView]
        public virtual string Name { get; set; }
    }
}