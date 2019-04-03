using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Entities;

namespace Base.UI
{
    [MemberType(nameof(PresetRegistorWizard.Type),nameof(PresetRegistorWizard.Preset))]
    public class PresetRegistorWizard : Wizard.DecoratedWizardObject<PresetRegistor>
    {
        [MaxLength(255)]
        [DetailView(Name = "Наименование", Required = true)]
        public string Title { get; set; }

        [PropertyDataType(PropertyDataType.SelectFromBoundedList, Params = "List=PresetTypes")]
        [DetailView("Тип пресета")]
        public DescriptionLookupVm PresetType { get; set; }

        [DetailView(Visible = false)]
        public ICollection<DescriptionLookupVm> PresetTypes { get; set; } = new List<DescriptionLookupVm>();

        [DetailView("Наименование рабочего стола", Required = true)]
        public string For { get; set; }

        //NOTE: 2 поля ниже инициализируются в сервисе
        [DetailView(Visible = false)]
        [PropertyDataType(PropertyDataType.BtnOpenVm)]
        public Preset Preset { get; set; }

        [DetailView(Visible = false)]
        public string Type { get; set; }

        public override PresetRegistor GetObject()
        {
            return new PresetRegistor()
            {
                Title = Title,
                Type = Type,
                For = For,
                Preset = Preset,
            };
        }
    }
}
