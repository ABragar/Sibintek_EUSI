using System;

namespace Base.UI.Editors
{
    public class ManyToManyAssociationEditor : EditorViewModel
    {
        public ManyToManyAssociationEditor()
        {

        }

        public ManyToManyAssociationEditor(Type manyToManyType, ManyToManyAssociationType associationType)
        {
            ManyToManyType = manyToManyType;
            AssociationType = associationType;
        }

        private EditorAssociationType _type;

        public EditorAssociationType Type
        {
            get { return _type; }
            set
            {
                PropertyDataTypeName = $"ManyToManyAssociation_{value}";
                _type = value;
            }
        }

        public ManyToManyAssociationType AssociationType { get; protected set; }

        public Type ManyToManyType { get; protected set; }

        public override T Copy<T>(T propertyView = default(T))
        {
            var editor = propertyView as ManyToManyAssociationEditor ??
                         new ManyToManyAssociationEditor(ManyToManyType, AssociationType);
            editor.ManyToManyType = ManyToManyType;
            editor.AssociationType = AssociationType;
            editor.Type = Type;
            return base.Copy(editor as T);
        }
    }

    public enum ManyToManyAssociationType
    {
        Left,
        Rigth
    }
}

     
