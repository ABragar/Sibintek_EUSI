using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base.UI.Service;

namespace Base.UI.ViewModal
{
    public class PreviewFieldsFactory<T> where T : class
    {
        private readonly List<PreviewField> _fields;

        public PreviewFieldsFactory(List<PreviewField> fields)
        {
            _fields = fields;
        }

        public PreviewFieldsFactory<T> Add<TValue>(Expression<Func<T, TValue>> expression, Action<PreviewFieldBuilder<T>> action = null)
        {
            var exp = expression.Body as MemberExpression;

            if (exp == null || exp.Expression.NodeType != ExpressionType.Parameter)
                throw new Exception("propertyExpression");

            var propInfo = exp.Member as PropertyInfo; //TODO: With custom Preview config there are no info

            var field = _fields.FirstOrDefault(x => x.PropertyName == propInfo.Name);

            if (field == null)
            {
                var editor = ViewModelConfigFactory.CreateEditor<T>(propInfo);

                field = editor.ToObject<PreviewField>();

                _fields.Add(field);
            }

            action?.Invoke(new PreviewFieldBuilder<T>(field));

            return this;
        }
    }
}