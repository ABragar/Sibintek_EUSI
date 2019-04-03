using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using System.ComponentModel.DataAnnotations;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.Contact.Entities
{
    [EnableFullTextSearch]
    public class OkvedType : BaseObject
    {
        private static readonly CompiledExpression<OkvedType, string> _title =
            DefaultTranslationOf<OkvedType>.Property(x => x.Title).Is(x => x.Code + ": " + x.Name);

        [DetailView("Код", Required = true)]
        [MaxLength(20)]
        public string Code { get; set; }

        [DetailView("Наименование", Required = true)]
        [MaxLength(255)]
        public string Name { get; set; }

        [FullTextSearchProperty]
        [ListView("Наименование")]
        public string Title => _title.Evaluate(this);
    }
}
