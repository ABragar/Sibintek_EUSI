using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Base.Translations
{
    public static class ExpressiveExtensions
    {

        public static Expression WithTranslations(Expression expression)
        {
            return WithTranslations(expression, TranslationMap.DefaultMap);
        }

        public static Expression WithTranslations(Expression expression, TranslationMap map)
        {
            return new TranslatingVisitor(map).Visit(expression);
        }

        private static void EnsureTypeInitialized(Type type)
        {
            try
            {
                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            }
            catch (TypeInitializationException)
            {
            }
        }

        private class TranslatingVisitor : ExpressionVisitor
        {
            private readonly Stack<KeyValuePair<ParameterExpression, Expression>> _bindings = new Stack<KeyValuePair<ParameterExpression, Expression>>();
            private readonly TranslationMap _map;

            internal TranslatingVisitor(TranslationMap map)
            {
                _map = map;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                EnsureTypeInitialized(node.Member.DeclaringType);
                CompiledExpression ce;

                if (_map.TryGet(node.Member, node.Expression?.Type, out ce))
                    return VisitCompiledExpression(ce, node.Expression);

                return base.VisitMember(node);
            }

            private Expression VisitCompiledExpression(CompiledExpression ce, Expression expression)
            {
                _bindings.Push(new KeyValuePair<ParameterExpression, Expression>(ce.Parameter, expression));

                var body = Visit(ce.Body);
                _bindings.Pop();
                return body;
            }

            protected override Expression VisitParameter(ParameterExpression parameter)
            {
                var keyValuePair = _bindings.FirstOrDefault(b => b.Key == parameter);
                return Visit(keyValuePair.Value) ?? parameter;
            }
        }
    }
}