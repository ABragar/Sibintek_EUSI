using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base.Attributes;

namespace Base.UI
{
    public class EditorBuilder<T> where T : class
    {
        private readonly EditorViewModel _editor;

        public EditorBuilder(EditorViewModel editor)
        {
            this._editor = editor;
        }

        public EditorBuilder<T> Mnemonic(string mnemonic)
        {
            _editor.Mnemonic = mnemonic;
            return this;
        }

        public EditorBuilder<T> Title(string title)
        {
            _editor.Title = title;
            return this;
        }

        public EditorBuilder<T> Description(string description)
        {
            _editor.Description = description;
            return this;
        }

        public EditorBuilder<T> IsLabelVisible(bool isLabelVisible)
        {
            _editor.IsLabelVisible = isLabelVisible;
            return this;
        }

        public EditorBuilder<T> EditorTemplate(string editorTemplate)
        {
            if (editorTemplate == null)
                throw new ArgumentNullException(nameof(editorTemplate));

            _editor.EditorTemplate = editorTemplate;
            _editor.PropertyDataType = PropertyDataType.Custom;
            return this;
        }

        public object TabName(object info1)
        {
            throw new NotImplementedException();
        }

        public EditorBuilder<T> TabName(string tabName)
        {
            _editor.TabName = tabName;
            return this;
        }

        public EditorBuilder<T> Group(string groupName)
        {
            _editor.Group = groupName;
            return this;
        }

        public EditorBuilder<T> GroupOrder(int groupOrder)
        {
            _editor.GroupOrder = groupOrder;
            return this;
        }

        public EditorBuilder<T> IsReadOnly(bool readOnly = true)
        {
            _editor.IsReadOnly = readOnly;
            return this;
        }

        public EditorBuilder<T> IsRequired(bool req)
        {
            _editor.IsRequired = req;
            return this;
        }

        public EditorBuilder<T> WizardName(string wizardName)
        {
            _editor.WizardName = wizardName;
            return this;
        }

        public EditorBuilder<T> Visible(bool visible)
        {
            _editor.Visible = visible;
            return this;
        }

        public EditorBuilder<T> Order(int order)
        {
            _editor.SortOrder = order;
            return this;
        }

        public EditorBuilder<T> AddParam(string key, string val)
        {
            if (_editor.EditorTemplateParams == null)
                _editor.EditorTemplateParams = new Dictionary<string, string>();

            if (!_editor.EditorTemplateParams.ContainsKey(key))
                _editor.EditorTemplateParams.Add(key, val);

            return this;
        }

        public EditorBuilder<T> AddParentKey(string val)
        {
            if (_editor.EditorTemplateParams == null)
                _editor.EditorTemplateParams = new Dictionary<string, string>();

            _editor.EditorTemplateParams.Add(EditorViewModel.ParentKey, val);

            return this;
        }

        public EditorBuilder<T> AddChildKey(string val)
        {
            if (_editor.EditorTemplateParams == null)
                _editor.EditorTemplateParams = new Dictionary<string, string>();

            _editor.EditorTemplateParams.Add(EditorViewModel.ChildKey, val);

            return this;
        }

        public EditorBuilder<T> DataType(PropertyDataType propertyDataType)
        {
            _editor.PropertyDataType = propertyDataType;

            if (_editor.PropertyDataType != PropertyDataType.Custom)
                _editor.PropertyDataTypeName = _editor.PropertyDataType.ToString();

            return this;
        }

        public EditorBuilder<T> CascadeFrom<TValue>(Expression<Func<T, TValue>> expression, bool lockDependentsIfParentEmpty = true)
        {
            var exp = expression.Body as MemberExpression;
            if (exp == null)
                throw new ArgumentNullException(nameof(exp));

            var propInfo = exp.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentNullException(nameof(propInfo));

            _editor.CascadeFrom = new CascadeFrom
            {
                Field = propInfo.Name,
                LockDependentsIfParentEmpty = lockDependentsIfParentEmpty
            };

            if (typeof(T).GetProperties().Any(x => x.Name == $"{propInfo.Name}ID"))
            {
                _editor.CascadeFrom.IdField = $"{propInfo.Name}ID";
            }
            else if (typeof(T).GetProperties().Any(x => x.Name == $"{propInfo.Name}Id"))
            {
                _editor.CascadeFrom.IdField = $"{propInfo.Name}Id";
            }

            return this;
        }

        public EditorBuilder<T> CascadeFrom<TValue>(Expression<Func<T, TValue>> expression,
            Expression<Func<T, int?>> idField, bool lockDependentsIdParentEmpty = true)
        {
            var exp = expression.Body as MemberExpression;
            if (exp == null)
                throw new ArgumentNullException(nameof(exp));

            var propInfo = exp.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentNullException(nameof(propInfo));

            _editor.CascadeFrom = new CascadeFrom
            {
                Field = propInfo.Name,
                LockDependentsIfParentEmpty = lockDependentsIdParentEmpty
            };

            var expIdField = idField.Body as MemberExpression;

            if (expIdField == null)
                throw new Exception("Bad cascade Id property expression");

            var idProp = expIdField.Member as PropertyInfo;
            if (idProp == null)

                throw new Exception("Bad cascade Id property expression");

            _editor.CascadeFrom.IdField = idProp.Name;

            return this;
        }

        #region Sib

        //TODO: добавить обработку onChange на все редакторы
        //пока работает установка onChange для редакторов Editor\Common\BaseObjectOne
        public EditorBuilder<T> OnChangeClientScript(string script)
        {
            _editor.OnChangeClientScript = script;
            return this;
        }

        public EditorBuilder<T> OnClientEditorChange(string script)
        {
            _editor.OnClientEditorChange = script;
            return this;
        }

        public EditorBuilder<T> SetCustomsParams(string script)
        {
            _editor.SetCustomParams = script;
            return this;
        }

        #endregion Sib
    }
}