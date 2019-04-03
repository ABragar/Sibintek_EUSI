using Base.Attributes;
using Base.Entities;
using Base.Entities.Complex;

namespace Base.Contact.Entities
{
    public class ContactEmail : BaseEmail
    {
        public int ContactID { get; set; }
        public virtual BaseContact Contact { get; set; }        
    }
}