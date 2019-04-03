using System;
using System.Linq.Expressions;
using System.Reflection;
using Base.Entities.Complex;

namespace Base.UI.ViewModal
{
    public class LookupPropertyBuilder<T> where T : class
    {
        private readonly LookupProperty _lookupProperty;

        public LookupPropertyBuilder(LookupProperty lookupProperty)
        {
            _lookupProperty = lookupProperty;
        }

        public LookupPropertyBuilder<T> Image(Expression<Func<T, FileData>> expression)
        {
            _lookupProperty.Image = ((MemberExpression)expression.Body).Member.Name;
            return this;
        }

        public LookupPropertyBuilder<T> NoImage()
        {
            _lookupProperty.Image = null;
            return this;
        }

        //public LookupPropertyBuilder<T> Icon(Expression<Func<T, Icon>> expression)
        //{
        //    _lookupProperty.Icon = GetPropertyName(expression);
        //    return this;
        //}

        public LookupPropertyBuilder<T> Icon(Expression<Func<T, Icon>> expression)
        {
            _lookupProperty.Icon = ((MemberExpression)expression.Body).Member.Name;
            return this;
        }

        public LookupPropertyBuilder<T> Text<TValue>(Expression<Func<T, TValue>> expression)
        {
            _lookupProperty.Text = ((MemberExpression)expression.Body).Member.Name;
            return this;
        }
    }
}
