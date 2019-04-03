using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Macros.Entities.Rules;
using Base.UI.Wizard;

namespace Base.App.Macros
{
    public class RuleForTypeWizard: DecoratedWizardObject<RuleForType>
    {
        public override RuleForType GetObject()
        {
            return new RuleForType()
            {
                ObjectType = ObjectType,
                Title = Title,
                Rules = Rules
            };
        }

        [DetailView("Тип", Required = true)]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public string ObjectType { get; set; }

        [MaxLength(255)]
        [DetailView("Наименование", Required = true)]
        public string Title { get; set; }

        [DetailView("Тип")]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public string Type => this.ObjectType;

        [DetailView("Правило")]

        [PropertyDataType(PropertyDataType.BPObjectEditButton)]

        public ICollection<RuleItem> Rules { get; set; }
    }
}