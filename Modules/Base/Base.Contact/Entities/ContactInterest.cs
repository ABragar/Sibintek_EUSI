using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Entities.Complex;

namespace Base.Contact.Entities
{
    public class ContactInterest: BaseObject, ISuperObject<ContactInterest>
    {
        public ContactInterest()
        {
            this.Icon = new Icon();
        }       


        [MaxLength(255)]
        [ListView]
        [DetailView("Наименование", Required = true, Order = 3)]
        public string Title { get; set; }

        [DetailView("Иконка", Order = 2)]
        public Icon Icon { get; set; }

        public virtual ICollection<BaseContact> Contacts { get; set; } = new List<BaseContact>();

        public string ExtraID { get; } = null;
    }
}
