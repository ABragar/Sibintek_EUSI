using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Entities;
using Base.Macros;
using Base.Macros.Entities;

namespace Base.UI.DetailViewSetting
{
    public class BaseEditorVmSetting : BaseObject
    {
        [MaxLength(255)]
        [DetailView(Name = "Поле", Required = true, Order = -1)]
        [PropertyDataType("EditorViewModel")]
        public string FieldName { get; set; }

        public BaseEditorVmSetting()
        {

        }

        public BaseEditorVmSetting(EditorViewModel editorViewModel)
        {
            FieldName = editorViewModel.PropertyName;
        }

        public virtual void Apply(EditorViewModel editor, Func<IEnumerable<ConditionItem>, bool> check)
        {
            throw new System.NotImplementedException();
        }
    }
}