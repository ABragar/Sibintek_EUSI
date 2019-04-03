using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Entities.Complex;

namespace Base.UI.ViewModal
{
    public class ViewModelConfigDto
    {
        public string Mnemonic { get; set; }
        public bool IsReadOnly { get; set; }
        public string Title { get; set; }
        public string TypeEntity { get; set; }
        public Icon Icon { get; set; }
        public LookupProperty LookupProperty { get; set; }
        public string[] SystemProperties { get; set; }
        public bool Preview { get; set; }
        public ListViewDto ListView { get; set; }
        public DetailViewDto DetailView { get; set; }
        public Dictionary<string, object> Ext { get; set; }
    }

    public class ListViewDto
    {
        public string Title { get; set; }
        public IEnumerable<ColumnDto> Columns { get; set; }
    }

    public class ColumnDto
    {
        public string PropertyName { get; set; }
        public bool Hidden { get; set; }
        public string DataType { get; set; }
        public string Type { get; set; }
    }

    public class DetailViewDto
    {
        public string Title { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool IsMaximaze { get; set; }
        public string WizardName { get; set; }
    }
}