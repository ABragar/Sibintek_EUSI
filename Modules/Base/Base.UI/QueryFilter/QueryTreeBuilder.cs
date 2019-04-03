using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Base.DAL;
using Base.Extensions;
using Newtonsoft.Json.Linq;

namespace Base.UI.QueryFilter
{
    public class QueryTreeBuilder 
    {
        private readonly IQueryTreeService _service;

        public QueryTreeBuilder(IQueryTreeService service)
        {
            _service = service;
        }

        public List<string> Mnemonics { get; } = new List<string>(1);

        public TreeOperatorResult<LambdaExpression> BuildPridecate(JToken json, string mnemonic, Type type)
        {
            Mnemonics.Add(mnemonic);

            var filters = _service.GetFilters(mnemonic);

            var parameter = Expression.Parameter(type ?? filters.Type, "x");

            var body = Build(parameter, json, filters);

            return body.Modify(x =>
                {
                    if (x == null)
                        return null;

                    return LambdaBuilder.BuildLambda(parameter, typeof(bool), x);
                }
            );
        }

        public TreeOperatorResult<Expression> Build(Expression expression, JToken json, string mnemonic)
        {
            Mnemonics.Add(mnemonic);

            var filters = _service.GetFilters(mnemonic);

            var body = Build(expression, json, filters);

            return body;
        }

        private TreeOperatorResult<Expression> Build(Expression expression, JToken json, QueryTreeFilterModel filters)
        {
            if (json == null || !json.HasValues)
                return (Expression)null;

            var rule = (JObject)json;


            if (rule.TryGetValue("condition", out var condition))
            {
                return BuildGroup(expression, condition, (JArray)rule["rules"], filters);
            }

            return BuildRule(expression, rule, filters);

        }
        private TreeOperatorResult<Expression> BuildGroup(Expression expression, JToken condition, JArray rules, QueryTreeFilterModel filters)
        {
            TreeOperatorResult<Expression> result = (Expression)null;
            TreeOperatorResult<Expression> mutable = (Expression)null;

            Func<Expression, Expression, Expression> combine;

            switch (condition.Value<string>())
            {
                case "AND":
                    combine = Expression.AndAlso;
                    break;
                case "OR":
                    combine = Expression.OrElse;
                    break;
                default:
                    throw new InvalidOperationException();
            }


            Expression NullCombine(Expression l, Expression r) => l == null ? r : r == null ? l : combine(l, r);

            foreach (var rule in rules)
            {
                var exp = Build(expression, rule, filters);

                if (exp.Mutable)
                {
                    mutable = mutable.Combine(exp, (Func<Expression, Expression, Expression>) NullCombine);
                }
                else
                {
                    result = result.Combine(exp, (Func<Expression, Expression, Expression>) NullCombine);
                }

            }

            return result.Combine(mutable, (Func<Expression, Expression, Expression>) NullCombine);
        }

        private TreeOperatorResult<Expression> BuildRule(Expression expression, JObject json, QueryTreeFilterModel filters)
        {

            var id = json["id"].Value<string>();

            var filter = filters.Items.GetValueOrDefault(id, (QueryTreeFilterItemModel)null);

            if (filter == null)
            {
                throw new ArgumentException($"id: {id} not found in {filters.Mnemonic}");
            }

            var op = json["operator"].Value<string>();


            var treeOperator = filter.Operators.GetValueOrDefault(op, (QueryTreeOperator)null);

            if (treeOperator == null)
            {
                throw new ArgumentException($"id: {id} operator: {op} not supported in {filters.Mnemonic}");
            }

            return treeOperator.BuildResult(this, json["value"], expression);
        }
    }
}