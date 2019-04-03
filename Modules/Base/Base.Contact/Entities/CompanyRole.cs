using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Entities.Complex;

namespace Base.Contact.Entities
{
    public class CompanyRole : BaseObject
    {
        public CompanyRole()
        {
            Icon = new Icon();
        }

        [ListView]
        [DetailView("Наименование", Required = true)]
        public string Title { get; set; }

        [DetailView("Иконка")]
        public Icon Icon { get; set; }
    }
}
