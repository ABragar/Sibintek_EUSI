using System.Collections.Generic;
using System.Linq;
using Base.Attributes;
using Base.UI.ViewModal;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class DeatilViewEx : MnemonicEx
    {
        [DetailView("Поля")]
        public virtual ICollection<EditorEx> Editors { get; set; }

        private IDictionary<string, EditorEx> _editors;

        public override void Visit(ConfigEditor configEditor)
        {
            if(configEditor.PropertyName == null) return;

            if (_editors == null)
                _editors = Editors?.ToDictionary(x => x.PropertyName, x => x);

            if(_editors == null || !_editors.ContainsKey(configEditor.PropertyName)) return;

            var editorEx = _editors[configEditor.PropertyName];

            configEditor.Title = editorEx.Title;
            configEditor.Description = editorEx.Description;
            configEditor.Visible = editorEx.Visible;
            configEditor.IsReadOnly = editorEx.IsReadOnly;
            configEditor.IsRequired = editorEx.IsRequired;
            configEditor.TabName = editorEx.TabName;

            if (editorEx.Order != null)
                configEditor.SortOrder = editorEx.Order.Value;
        }
    }
}