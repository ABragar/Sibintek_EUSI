using System;
using System.Linq;
using System.Linq.Expressions;
using Base.Attributes;
using Base.DAL;

namespace Base.UI
{
    public class OneToManyAssociationEditorBuilder<T, TEntity> where T : class where TEntity: IBaseObject
    {
        private readonly OneToManyAssociationEditor _editor;

        public OneToManyAssociationEditorBuilder(OneToManyAssociationEditor editor)
        {
            _editor = editor;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Title(string title)
        {
            _editor.Title = title;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> IsLabelVisible(bool isLabelVisible)
        {
            _editor.IsLabelVisible = isLabelVisible;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> TabName(string tabName)
        {
            _editor.TabName = tabName;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Visible(bool visible)
        {
            _editor.Visible = visible;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Order(int order)
        {
            _editor.SortOrder = order;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Filter(
            Expression<Func<IUnitOfWork, IQueryable<TEntity>, int, Guid, IQueryable<TEntity>>> func)
        {
            _editor.Filter = (uofw, q, id, oid) => func.Compile()(uofw, (IQueryable<TEntity>)q, id, oid);
            _editor.FilterExpression = func;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> FilterExtended(
            Func<IUnitOfWork, IQueryable<TEntity>, int, Guid, IQueryable<TEntity>> func)
        {
            _editor.Filter = (uofw, q, id, oid) => func(uofw, (IQueryable<TEntity>)q, id, oid);
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Create(Action<IUnitOfWork, TEntity, int> action)
        {
            _editor.Create = (uofw, obj, id) => action(uofw, (TEntity)obj, id);
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Delete(Action<IUnitOfWork, TEntity, int> action)
        {
            _editor.Delete = (uofw, obj, id) => action(uofw, (TEntity)obj, id);
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Type(EditorAssociationType type)
        {
            _editor.Type = type;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> EditorTemplate(string editorTemplate)
        {
            _editor.PropertyDataType = PropertyDataType.Custom;
            _editor.EditorTemplate = editorTemplate;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> IsReadOnly(bool val = true)
        {
            _editor.IsReadOnly = val;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Mnemonic(string val)
        {
            _editor.Mnemonic = val;
            return this;
        }

        public OneToManyAssociationEditorBuilder<T, TEntity> Controller(string val)
        {
            _editor.Controller = val;
            return this;
        }
    }
}