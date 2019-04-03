using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Entities.Category
{
    public abstract class BaseNomCategory : HCategory
    {
        //[DetailView("Код", Required = true, Order = 0), ListView]
        //[MaxLength(255)]
        //public string Code { get; set; }

        [DetailView("Описание", Order = 10), ListView]
        public string Description { get; set; }
    }
}
