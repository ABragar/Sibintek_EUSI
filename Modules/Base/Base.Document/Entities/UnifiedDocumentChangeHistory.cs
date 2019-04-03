using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Security;

namespace Base.Document.Entities
{
    public class UnifiedDocumentChangeHistory : BaseObject
    {
        public int? FileId { get; set; }

        [ListView]
        [DetailView("Файл", Order = 1, Required = true)]
        [PropertyDataType(PropertyDataType.File)]
        public virtual FileData File { get; set; }

        [ListView]
        [PropertyDataType(PropertyDataType.DateTime)]
        [DetailView("Дата изменения", Order = 2)]
        public DateTime ChangedDate { get; set; }


        public int? EditorUserId { get; set; }

        [ListView]
        [DetailView("Изменил файл", Order = 3)]
        public virtual User EditorUser { get; set; }
    }
}
