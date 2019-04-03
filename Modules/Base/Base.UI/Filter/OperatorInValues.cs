using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Utils.Common.Attributes;

namespace Base.UI.Filter
{
    [EnableFullTextSearch]
    public class OperatorInValues: BaseObject
    {
        [SystemProperty]
        public Guid IdForValue { get; set; }

        [SystemProperty]
        public Guid MnemonicFilterOid { get; set; }

        [FullTextSearchProperty]
        [ListView(Name = "Значения", Sortable = true, Visible = true)]
        [DetailView(Name="Значение")]
        public string Value { get; set; }  
    }
}
