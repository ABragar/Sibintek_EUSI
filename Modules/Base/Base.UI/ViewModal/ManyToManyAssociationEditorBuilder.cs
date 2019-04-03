using System;
using System.Linq;
using Base.Attributes;
using Base.DAL;
using Base.UI.Editors;

namespace Base.UI
{
    public class ManyToManyAssociationEditorBuilder
    {
        private readonly ManyToManyAssociationEditor _editor;

        public ManyToManyAssociationEditorBuilder(ManyToManyAssociationEditor editor)
        {
            _editor = editor;
        }

        public ManyToManyAssociationEditorBuilder Title(string title)
        {
            _editor.Title = title;
            return this;
        }

        public ManyToManyAssociationEditorBuilder IsLabelVisible(bool isLabelVisible)
        {
            _editor.IsLabelVisible = isLabelVisible;
            return this;
        }

        public ManyToManyAssociationEditorBuilder TabName(string tabName)
        {
            _editor.TabName = tabName;
            return this;
        }

        public ManyToManyAssociationEditorBuilder Visible(bool visible)
        {
            _editor.Visible = visible;
            return this;
        }

        public ManyToManyAssociationEditorBuilder Order(int order)
        {
            _editor.SortOrder = order;
            return this;
        }
      
        public ManyToManyAssociationEditorBuilder Type(EditorAssociationType type)
        {
            _editor.Type = type;
            return this;
        }

        public ManyToManyAssociationEditorBuilder EditorTemplate(string editorTemplate)
        {
            _editor.PropertyDataType = PropertyDataType.Custom;
            _editor.EditorTemplate = editorTemplate;
            return this;
        }

        public ManyToManyAssociationEditorBuilder IsReadOnly(bool val = true)
        {
            _editor.IsReadOnly = val;
            return this;
        }

        public ManyToManyAssociationEditorBuilder Mnemonic(string val)
        {
            _editor.Mnemonic = val;
            return this;
        }
    }
}