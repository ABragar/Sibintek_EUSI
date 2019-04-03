using Base.Attributes;
using Base.Entities;
using Base.Entities.Complex;

namespace Base.Contact.Entities
{
    public class ContactPhone : BaseObject
    {
        public int ContactID { get; set; }
        public virtual BaseContact Contact { get; set; }

        [ListView]
        [DetailView("Телефон")]
        public Phone Phone { get; set; } = new Phone();
    }
}