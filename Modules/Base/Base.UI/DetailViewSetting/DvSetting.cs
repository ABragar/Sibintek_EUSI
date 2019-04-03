using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Attributes;
using Base.Macros.Entities;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.UI.DetailViewSetting
{
    [EnableFullTextSearch]
    public abstract class DvSetting : HCategory
    {

        #region HCategory
        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual DvSetting Parent_ { get; set; }
        [JsonIgnore]
        public virtual ICollection<DvSetting> Children_ { get; set; }

        public override HCategory Parent
        {
            get { return this.Parent_; }
        }

        public override IEnumerable<HCategory> Children
        {
            get { return Children_ ?? new List<DvSetting>(); }
        }
        #endregion
        

        [DetailView(TabName = "[1]Поля")]
        public virtual ICollection<EditorVmSetting> Fields { get; set; } = new List<EditorVmSetting>();

        public void Apply(IReadOnlyCollection<EditorViewModel> editors, Func<IEnumerable<ConditionItem>, bool> check)
        {
            if (editors == null) return;

            foreach (var editorVmSetting in Fields)
            {
                var editor = editors.FirstOrDefault(x => x.PropertyName == editorVmSetting.FieldName);

                if (editor != null)
                    editorVmSetting.Apply(editor, check);
            }

        }
    }
}
