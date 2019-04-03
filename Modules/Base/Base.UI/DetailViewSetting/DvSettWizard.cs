using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;

namespace Base.UI.DetailViewSetting
{
    public class DvSettWizard : Wizard.DecoratedWizardObject<DvSettingForType>
    {
        public override DvSettingForType GetObject()
        {
            return new DvSettingForType()
            {
                ObjectType = ObjectType,
                Name = Title,
                Fields = Editors,
                Parent_ = Parent,
                ParentID = ParentID
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

        [DetailView("Поля")]
        public ICollection<EditorVmSetting> Editors { get; set; } = new List<EditorVmSetting>();

        public DvSetting Parent { get; set; }
        [SystemProperty]
        public int? ParentID { get; set; }
    }
}
