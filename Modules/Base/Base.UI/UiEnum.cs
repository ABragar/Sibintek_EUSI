using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Entities.Complex;
using Base.Security;

namespace Base.UI
{
    public class UiEnum : BaseObject
    {
        [MaxLength(255)]
        [SystemProperty]
        [DetailView("Тип", ReadOnly = true), ListView]
        public string Type { get; set; }

        [MaxLength(255)]
        [DetailView("Наименование", Required = true), ListView]
        public string Title { get; set; }

        [DetailView("Значения")]
        public virtual ICollection<UiEnumValue> Values { get; set; }
    }

    public class UiEnumValue : BaseObject
    {
        public UiEnumValue()
        {
            Icon = new Icon();
        }

        [MaxLength(255)]
        [SystemProperty]
        [DetailView("Значение", ReadOnly = true), ListView]
        public string Value { get; set; }
        [MaxLength(255)]
        [DetailView("Наименование"), ListView]
        public string Title { get; set; }
        [DetailView("Иконка"), ListView]
        public Icon Icon { get; set; }
    }
}
