using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base.ComplexKeyObjects.Common;

namespace Base.ComplexKeyObjects.Unions.Implementation
{
    internal class UnionVisitor<TResult> : ExpressionVisitor
    {
        private static readonly MemberAssignment[] _default_bindings =
            GetDefaultBindings().ToArray();

        private ParameterExpression _new_parameter = null;

        private ParameterExpression _old_parameter = null;

        private MemberAssignment[] _overrides = null;


        private static IEnumerable<MemberAssignment> GetDefaultBindings()
        {

            return
                typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.CanRead && x.CanWrite)
                    .Select(
                        x => Expression.Bind(x, Expression.Constant(GetDefaultValue(x.PropertyType), x.PropertyType)));
        }


        private static object GetDefaultValue(Type type)
        {
            return
                Expression.Lambda<Func<object>>(Expression.Convert(Expression.Default(type), typeof(object)))
                    .Compile()
                    .Invoke();
        }

        public Expression<Func<T, TResult>> InitSelector<T>(Expression<Func<T, TResult>> selector, params SelectorOverride<T, TResult>[] overrides)
        {

            _overrides = overrides.Select(x => Expression.Bind(x.MethodInfo,
                ReplaceParameter(x.Lambda.Body,
                x.Lambda.Parameters.First(),
                selector.Parameters.First()))).ToArray();

            var result = VisitAndConvert(selector, "init_selector");

            _overrides = null;

            return result;
        }

        private Expression ReplaceParameter(Expression expr, ParameterExpression old_parameter, ParameterExpression new_parameter)
        {
            _old_parameter = old_parameter;
            _new_parameter = new_parameter;

            var result = Visit(expr);

            _new_parameter = null;
            _old_parameter = null;

            return result;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_old_parameter != null && node == _old_parameter)
                return _new_parameter;

            return node;
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            if (_overrides == null && node.NewExpression.Constructor.DeclaringType != typeof(TResult))
            {
                return base.VisitMemberInit(node);
            }

            return Expression.MemberInit(node.NewExpression, GetBindings(node));

        }

        private IEnumerable<MemberBinding> GetBindings(MemberInitExpression node)
        {

            return _default_bindings.Select(x =>
                _overrides.SingleOrDefault(b => b.Member == x.Member) ??
                node.Bindings.SingleOrDefault(b => b.Member == x.Member) ??
                x);
        }
    }
}