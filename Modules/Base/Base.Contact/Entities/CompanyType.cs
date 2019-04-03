using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.Contact.Entities
{
    public class CompanyType : BaseObject
    {
        [DetailView("Наименование"), ListView]
        [MaxLength(255)]
        public string Title { get; set; }
    }
}
