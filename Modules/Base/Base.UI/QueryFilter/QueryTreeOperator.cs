using System;
using System.Linq.Expressions;
using Base.DAL;
using Newtonsoft.Json.Linq;

namespace Base.UI.QueryFilter
{
    public class QueryTreeOperator
    {
        private readonly Func<QueryTreeBuilder, JToken, Expression, TreeOperatorResult<Expression>> _buildFunc;
        public OperatorKind Kind { get; }

        public QueryTreeOperator(OperatorKind kind, Func<QueryTreeBuilder, JToken, Expression, TreeOperatorResult<Expression>> buildFunc)
        {
            _buildFunc = buildFunc;
            Kind = kind;
        }

        public TreeOperatorResult<Expression> BuildResult(QueryTreeBuilder builder, JToken json, Expression expression)
        {
            return _buildFunc(builder, json, expression);
        }

        public QueryTreeOperator Modify(OperatorKind kind, Func<Expression, Expression> modifyFunc)
        {
            return new QueryTreeOperator(kind, (c, v, x) => _buildFunc(c, v, x).Modify(modifyFunc));
        }
    }

}