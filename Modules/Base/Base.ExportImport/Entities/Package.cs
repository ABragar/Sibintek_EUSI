using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Base.ExportImport.Entities
{
    public class Package : BaseObject
    {
        [ListView]
        [DetailView("Наименование")]
        [MaxLength(255)]
        public string Title { get; set; }

        [ListView]
        [DetailView("Тип")]
        [PropertyDataType("ListBaseObjects")]
        public string ObjectType { get; set; }

        public int? TransformID { get; set; }
        [DetailView("xslt")]
        public virtual FileData Transform { get; set; }
    }
}
