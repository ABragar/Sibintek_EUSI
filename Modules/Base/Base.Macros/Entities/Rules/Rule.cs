using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Rule;

namespace Base.Macros.Entities.Rules
{
    public abstract class Rule: BaseObject
    {
        [MaxLength(255)]
        [DetailView("Наименование", Required = true), ListView]
        public string Title { get; set; }
        public abstract string ObjectType { get; set; }

        [DetailView(Name = "Правило")]
        [PropertyDataType(PropertyDataType.BPObjectEditButton)]
        public virtual ICollection<RuleItem> Rules { get; set; }
    }
}