using System;
using System.Linq.Expressions;

namespace Base.Translations
{
    public static class DefaultTranslationOf<T>
    {
        public static CompiledExpression<T, TResult> Property<TResult>(Expression<Func<T, TResult>> property, Expression<Func<T, TResult>> expression)
        {
            return TranslationMap.DefaultMap.Add(property, expression);
        }

        public static IncompletePropertyTranslation<TResult> Property<TResult>(Expression<Func<T, TResult>> property)
        {
            return new IncompletePropertyTranslation<TResult>(property);
        }


        public class IncompletePropertyTranslation<TResult>
        {
            private readonly Expression<Func<T, TResult>> _property;

            internal IncompletePropertyTranslation(Expression<Func<T, TResult>> property)
            {
                _property = property;
            }

            public CompiledExpression<T, TResult> Is(Expression<Func<T, TResult>> expression)
            {
                return Property(_property, expression);
            }
        }
    }
}
