using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.UI.QueryFilter
{
    public class LambdaHelper
    {
        public static MethodInfo GetEnumerableSelect(Type source, Type result)
        {
            return EnumerableSelect.MakeGenericMethod(source, result);
        }

        private static MethodInfo EnumerableSelect { get; } = GetEnumerableSelectMethodInfo();

        private static MethodInfo GetEnumerableSelectMethodInfo()
        {
            Expression<Func<int, IEnumerable<bool>>> temp = x => Array.Empty<bool>().Select(f => !f);

            var body = (MethodCallExpression)temp.Body;
            return body.Method.GetGenericMethodDefinition();
        }
    }

    public class LambdaBuilder
    {
        private readonly LambdaBuilder _inner;
        private readonly Type _resultType;
        private readonly Func<Expression, Expression> _buildFunc;
        private Type _paramType;

        private LambdaBuilder(LambdaBuilder inner, Type resultType, Func<Expression, Expression> buildFunc)
        {
            _inner = inner;
            _paramType = inner._paramType;

            _resultType = resultType;
            _buildFunc = buildFunc;
        }

        public LambdaBuilder(Type paramType)
        {
            _paramType = paramType;
            _resultType = paramType;
        }


        public LambdaBuilder Create(Type resultType, Func<Expression, Expression> buildFunc)
        {
            if (buildFunc == null)
                throw new ArgumentNullException();

            return new LambdaBuilder(this, resultType, buildFunc);
        }

        public Type ParamType
        {
            get => _paramType;
            set => _paramType = value;
        }

        public Type ReturnType => _resultType;

        public Expression Build(Expression parameter)
        {
            if (_inner == null)
                return parameter;

            var exp = _inner.Build(parameter);

            return _buildFunc(exp);

        }

        public static LambdaExpression BuildLambda(ParameterExpression param, Type resultType, Expression body)
        {
            return Expression.Lambda(typeof(Func<,>).MakeGenericType(param.Type, resultType),
                body,
                false,
                param);
        }

        public static LambdaExpression BuildLambda(Type paramType,string paramName, Type resultType, Func<Expression,Expression> bodyFunc)
        {
            var param = Expression.Parameter(paramType, paramName);

            return BuildLambda(param, resultType, bodyFunc(param));
        }


        public LambdaExpression BuildLambda(string paramName)
        {
            var param = Expression.Parameter(_paramType, paramName);

            var body = Build(param);

            return BuildLambda(param, _resultType, body);
        }
    }
}