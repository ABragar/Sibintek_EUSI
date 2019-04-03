using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.ComplexKeyObjects.Common
{
    public class SelectorOverride<T, TResult>
    {
        internal MemberInfo MethodInfo;

        internal LambdaExpression Lambda;

        public static SelectorOverride<T, TResult> Create<TProperty>(Expression<Func<TResult, TProperty>> property,
            Expression<Func<T, TProperty>> selector)
        {
            return new SelectorOverride<T, TResult>
            {
                MethodInfo = RemoveConvertVisitor.Instance.VisitAndConvert((MemberExpression)property.Body, "selector_override").Member,
                Lambda = RemoveConvertVisitor.Instance.VisitAndConvert(selector, "selector_override")
            };
        }

    }
}