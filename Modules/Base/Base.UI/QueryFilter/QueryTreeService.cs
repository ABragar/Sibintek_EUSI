using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Base.Ambient;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using Base.UI.Editors;
using Base.UI.Filter;
using Base.UI.Service;
using Base.UI.ViewModal;
using Base.Utils.Common;
using Newtonsoft.Json.Linq;

namespace Base.UI.QueryFilter
{
    public class QueryTreeService : IQueryTreeService
    {
        private readonly IViewModelConfigService _modelConfigService;
        private readonly QueryTreeServiceOptions _options;
        public static Expression OperatorInValues;

        public QueryTreeService(IViewModelConfigService modelConfigService, QueryTreeServiceOptions options)
        {
            _modelConfigService = modelConfigService;
            _options = options;
        }

        public QueryTreeFilterModel GetFilters(string mnemonic)
        {
            var config = _modelConfigService.Get(mnemonic);

            var view = config.DetailView;

            var result = new QueryTreeFilterModel()
            {
                Title = config.Title,
                Mnemonic = mnemonic,
                Type = config.TypeEntity,
                Items = GetFilters(config, view.Editors).GroupBy(x => x.Id).Select(x => x.First())
                    .ToDictionary(x => x.Id)
            };

            return result;
        }

        public IEnumerable<QueryTreeItemViewModel> GetAggregatableProperties(string mnemonic)
        {
            var editors = _modelConfigService.Get(mnemonic).DetailView.Editors;
            return editors.Where(e =>
                    e.PropertyType.Name != nameof(IBaseObject.ID) && e.PropertyType.IsPrimitive &&
                    e.PropertyType.IsNumericType())
                .Select(e => new QueryTreeItemViewModel { Id = e.PropertyName, Label = e.Title });
        }

        public IEnumerable<QueryTreeFilterItemModel> GetFilters(ViewModelConfig config,
            IEnumerable<EditorViewModel> props)
        {
            // Предпологаем что поля без явных имен не используются
            props = props.Where(x => !string.Equals(x.Title, x.PropertyName, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
            var childEditors = props.Where(p => p.EditorTemplate == "PartialEditor").SelectMany(p =>
                GetFilterItems(p.ViewModelConfig, p.ViewModelConfig.DetailView.Editors).Select(e =>
                {
                    e.Id = $"{p.ViewModelConfig.TypeEntity.Name}_{e.Id}";
                    return e;
                }));
            var parentEditors = GetFilterItems(config, props.Where(p => p.EditorTemplate != "PartialEditor"));

            return childEditors.Union(parentEditors);
        }

        public IEnumerable<QueryTreeFilterItemModel> GetFilterItems(ViewModelConfig config,
            IEnumerable<EditorViewModel> props)
        {
            var result = props.Select(x =>
            {
                var item = new QueryTreeFilterItemModel()
                {
                    PropertyName = x.PropertyName,
                    IsRequired = x.IsRequired,
                    SystemData = x.Mnemonic ?? x.ViewModelConfig?.Mnemonic,
                    Label = x.Title,
                    Type = x.PropertyType
                };

                var operators = GetOperatorGroups(config.TypeEntity, item, x)
                    .SelectMany(g => g).ToArray();

                item.Operators = operators.ToDictionary(o => o.Kind.ToString());

                return item;
            });

            return result.Where(x => x.Id != null && x.Operators.Count > 0);
        }

        private static readonly IDictionary<Type, PrimitiveType> PrimitiveTypes = new Dictionary<Type, PrimitiveType>()
        {
            {typeof(int), new PrimitiveType("integer", EqualsOperators, ComparableOperator)},
            {typeof(short), new PrimitiveType("integer", EqualsOperators, ComparableOperator)},
            {typeof(byte), new PrimitiveType("integer", EqualsOperators, ComparableOperator)},
            {typeof(long), new PrimitiveType("integer", EqualsOperators, ComparableOperator)},
            {typeof(double), new PrimitiveType("double", ComparableOperator)},
            {typeof(decimal), new PrimitiveType("double", EqualsOperators, ComparableOperator)},
            {typeof(float), new PrimitiveType("double", ComparableOperator)},
            {typeof(string), new PrimitiveType("string", EqualsOperators, StringOperator)},
            {typeof(bool), new PrimitiveType("boolean", EqualsOperators)},
            {typeof(DateTime), new PrimitiveType("datetime", EqualsOperators, ComparableOperator)},
        };

        private class PrimitiveType
        {
            private readonly Func<LambdaBuilder, IEnumerable<QueryTreeOperator>>[] _operators;

            public PrimitiveType(
                string primitiveName,
                params Func<LambdaBuilder, IEnumerable<QueryTreeOperator>>[] operators)
            {
                PrimitiveName = primitiveName;
                _operators = operators;
            }

            public string PrimitiveName { get; }

            public Func<LambdaBuilder, IEnumerable<QueryTreeOperator>> Operators =>
                x => _operators.SelectMany(o => o(x));
        }

        public IEnumerable<IEnumerable<QueryTreeOperator>> GetOperatorGroups(Type rootType,
            QueryTreeFilterItemModel item, EditorViewModel editor)
        {
            var type = item.Type;
            var baseType = item.Type;
            var builder = new LambdaBuilder(rootType);

            if (item.PropertyName != null)
            {
                item.Id = item.PropertyName;

                var method = builder.Create(item.Type, b => Expression.Property(b, b.Type, item.Id));

                var isNullable = GetIsNullable(type, ref baseType);
                var isBaseObject = false;

                if (GetIsCollection(type, ref baseType))
                {
                    Type baseEasyType = null;

                    if (GetIsEasyCollectionEntry(baseType, ref baseEasyType))
                    {
                        var select = new LambdaBuilder(baseType).Create(baseEasyType, x => Expression.Property(
                            x,
                            baseType,
                            nameof(EasyCollectionEntry<BaseObject>.Object))).BuildLambda("e");


                        var nested = method.Create(typeof(IQueryable<>).MakeGenericType(baseEasyType),
                            x => Expression.Call(typeof(Queryable), nameof(Queryable.Select),
                                new[] { baseType, baseEasyType }, x, select));

                        item.Plugins.Add("QueryTree");
                        yield return NestedCollectionOperators(nested, item.SystemData, baseEasyType);
                    }
                    else
                    {
                        item.Plugins.Add("QueryTree");
                        yield return NestedCollectionOperators(method, item.SystemData, baseType);
                    }
                }
                else if (baseType == typeof(int) && GetIsBaseObject(rootType) &&
                         item.PropertyName == nameof(BaseObject.ID))
                {
                    if (rootType == _options.UserType)
                    {
                        item.Plugins.Add("UserId");
                        item.SystemData = editor.ParentViewModelConfig.Mnemonic;

                        yield return UserIdOperators(method);
                    }
                    else
                    {
                        item.Plugins.Add("BaseObjectId");
                        item.SystemData = editor.ParentViewModelConfig.Mnemonic;

                        yield return EqualsOperators(method);
                    }
                }
                else if (baseType == typeof(DateTime))
                {
                    item.Plugins.Add("DateTime");
                    item.SystemData = editor.PropertyDataTypeName;

                    yield return EqualsOperators(method).Concat(ComparableOperator(method));
                }
                else if (baseType == typeof(bool))
                {
                    item.Plugins.Add("Boolean");
                    item.SystemData = editor.PropertyDataTypeName;

                    yield return EqualsOperators(method);
                }
                else if (PrimitiveTypes.TryGetValue(baseType, out var primitive))
                {
                    item.PrimitiveType = primitive.PrimitiveName;
                    item.Plugins.Add("InAndNotIn");
                    yield return primitive.Operators(method);
                }
                else if (item.SystemData != null)
                {
                    isBaseObject = GetIsBaseObject(baseType);

                    item.Plugins.Add("QueryTree");
                    yield return NestedOperators(method, item.SystemData);
                }
                else if (baseType.IsEnum)
                {
                    item.Plugins.Add("Enum");
                    item.SystemData = baseType.GetTypeName();
                    yield return EqualsOperators(method);
                }

                if (!item.IsRequired && (isNullable || isBaseObject || item.Type == typeof(string)))
                {
                    yield return NullableOperators(method);
                }
            }
            else
            {
                yield return GetNotPropertyOperatorGroups(rootType, item, editor).SelectMany(x => x);
            }
        }

        private IQueryService<object> GetQueryService(string mnemonic)
        {
            var config = _modelConfigService.Get(mnemonic);
            return config.GetService<IQueryService<object>>();
        }

        private IQueryService<object> GetQueryService(Type type)
        {
            var config = _modelConfigService.Get(type);
            return config.GetService<IQueryService<object>>();
        }

        private ConstantExpression GetQueryConstant(IQueryTreeBuilderContext context, IQueryService<object> service)
        {
            var query = context.Get(service);

            return Expression.Constant(query);
        }

        private IEnumerable<IEnumerable<QueryTreeOperator>> GetNotPropertyOperatorGroups(Type rootType,
            QueryTreeFilterItemModel item, EditorViewModel editor)
        {
            var id = new LambdaBuilder(rootType).Create(typeof(int),
                x => Expression.Property(x, nameof(BaseObject.ID)));
            Func<IQueryTreeBuilderContext, LambdaBuilder> filterFunc;
            if (editor is OneToManyAssociationEditor oneToMany)
            {
                if (oneToMany.FilterExpression != null)
                {
                    var service = GetQueryService(item.SystemData);

                    filterFunc = context => new LambdaBuilder(rootType).Create(item.Type, x =>
                    {
                        var uow = context.GetUow();
                        var uowExpr = Expression.Constant(uow);
                        var query = context.Get(service).GetInnerQuery();

                        var newExpr =
                            new FilterExpressionVisitor((ParameterExpression)x, query.Expression, uowExpr)
                                .ChangeParameters(oneToMany.FilterExpression);

                        return newExpr.Body;
                    });

                    item.Id = editor.SysName;
                    item.Plugins.Add("QueryTree");

                    yield return NestedCollectionOperators(filterFunc, item.SystemData, item.Type);

                    yield return CollectionCountOperators(filterFunc, item.Type);

                    item.Plugins.Add("AggregateFuncs");
                    yield return CollectionAggregateOperators(filterFunc, item.SystemData, item.Type);
                }
            }
            else if (editor is ManyToManyAssociationEditor manyToMany)
            {
                var manyToManyType = manyToMany.ManyToManyType;

                var isLeft = manyToMany.AssociationType == ManyToManyAssociationType.Left;

                var objectKey = GetManyToManyProperty(manyToManyType, isLeft);

                var joinKey = GetManyToManyProperty(manyToManyType, !isLeft);

                var queryType = GetManyToManyPropertyType(manyToManyType, !isLeft);

                filterFunc = context => id.Create(typeof(bool), x =>
                {
                    var joinQueryKey = LambdaBuilder.BuildLambda(queryType, "q", typeof(int),
                        y => Expression.Property(y, nameof(BaseObject.ID)));

                    var resultSelector = JoinLeftSelector(queryType, manyToManyType);

                    var manyToManyService = GetQueryService(manyToMany.ManyToManyType);

                    var queryService = GetQueryService(item.SystemData);

                    var whereLambdaExpr = LambdaBuilder.BuildLambda(objectKey.Parameters[0], typeof(bool),
                        Expression.Equal(objectKey.Body, x));

                    var manyToManyQuery = context.Get(manyToManyService).GetInnerQuery().Expression;
                    ;
                    var query = context.Get(queryService).GetInnerQuery().Expression;
                    
                    var inner = Expression.Call(typeof(Queryable), nameof(Queryable.Where), new[] { manyToManyType },
                        manyToManyQuery, whereLambdaExpr);

                    return Expression.Call(typeof(Queryable), nameof(Queryable.Join),
                        new[] { queryType, manyToManyType, typeof(int), queryType }, query, inner, joinQueryKey, joinKey,
                        resultSelector);
                });

                item.Id = editor.SysName;
                item.Plugins.Add("QueryTree");

                yield return NestedCollectionOperators(filterFunc, item.SystemData, queryType);

                yield return CollectionCountOperators(filterFunc, queryType);

                item.Plugins.Add("AggregateFuncs");
                yield return CollectionAggregateOperators(filterFunc, item.SystemData, queryType);
            }
        }

        public class FilterExpressionVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;
            private readonly Expression _query;
            private readonly Expression _uow;

            public FilterExpressionVisitor(ParameterExpression parameter, Expression query, Expression uow)
            {
                _parameter = parameter;
                _query = query;
                _uow = uow;
            }

            public LambdaExpression ChangeParameters(Expression expression)
            {
                return (LambdaExpression)Visit(expression);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                return Expression.Lambda<T>(Visit(node.Body), node.Parameters);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (node.Name == "q")
                    return _query;
                if (node.Name == "uow")
                    return _uow;
                if (node.Name == "id")
                    return Expression.Property(_parameter, nameof(BaseObject.ID));
                if (node.Name == "oid")
                    return Expression.Property(_parameter, "Oid");
                return node;
            }
        }

        private static Type GetManyToManyPropertyType(Type type, bool left)
        {
            var name = left
                ? nameof(ManyToManyAssociation<BaseObject, BaseObject>.ObjLeft)
                : nameof(ManyToManyAssociation<BaseObject, BaseObject>.ObjRigth);

            return type.GetProperty(name)?.PropertyType;
        }

        private static LambdaExpression GetManyToManyProperty(Type type, bool left)
        {
            var name = left
                ? nameof(ManyToManyAssociation<BaseObject, BaseObject>.ObjLeftId)
                : nameof(ManyToManyAssociation<BaseObject, BaseObject>.ObjRigthId);
            return LambdaBuilder.BuildLambda(type, "m", typeof(int), x => Expression.Property(x, name));
        }

        private static LambdaExpression JoinLeftSelector(Type leftType, Type rigthType)
        {
            var leftParameter = Expression.Parameter(leftType, "left");
            var rigthParameter = Expression.Parameter(rigthType, "rigth");

            return Expression.Lambda(typeof(Func<,,>).MakeGenericType(leftType, rigthType, leftType),
                leftParameter,
                leftParameter,
                rigthParameter);
        }

        private static IEnumerable<QueryTreeOperator> NullableOperators(LambdaBuilder method)
        {
            var nullConstant = Expression.Constant(null, method.ReturnType);

            var isNull = method.Create(typeof(bool), x => Expression.Equal(x, nullConstant));

            var isNotNull = method.Create(typeof(bool), x => Expression.NotEqual(x, nullConstant));

            yield return new QueryTreeOperator(OperatorKind.is_null, (b, j, x) => isNull.Build(x));

            yield return new QueryTreeOperator(OperatorKind.is_not_null, (b, j, x) => isNotNull.Build(x));
        }

        private static QueryTreeOperator CreatePrimitiveTreeOperator(
            OperatorKind kind,
            LambdaBuilder method,
            Func<Expression, Expression, Expression> expressionFunc)
        {
            return new QueryTreeOperator(kind,
                (b, j, x) => GetPrimitiveTreeOperatorResult(method, expressionFunc, j, x));
        }

        private static TreeOperatorResult<Expression> GetPrimitiveTreeOperatorResult(LambdaBuilder method,
            Func<Expression, Expression, Expression> expressionFunc, JToken j, Expression x)
        {
            method.ParamType = x.Type;
            var cc = Expression.Constant(j.ToObject(method.ReturnType), method.ReturnType);

            var result = method.Create(typeof(bool), e => expressionFunc(e, cc));

            return result.Build(x);
        }

        private static IEnumerable<QueryTreeOperator> NestedOperators(LambdaBuilder method, string mnemonic)
        {
            var inResult = new QueryTreeOperator(OperatorKind.collection_in,
                (b, j, x) =>
                {
                    var param = method.Build(x);

                    return b.Build(param, j, mnemonic);
                });

            yield return inResult;

            yield return inResult.Modify(OperatorKind.collection_not_in, Expression.Not);
        }

        private static IEnumerable<QueryTreeOperator> NestedCollectionOperators(
            TreeOperatorResult<LambdaBuilder> method, string mnemonic, Type queryType)
        {
            var inOperator = new QueryTreeOperator(OperatorKind.collection_in,
                (b, j, x) =>
                {
                    var nestedPredicate = b.BuildPridecate(j, mnemonic, null);

                    if (!nestedPredicate.Mutable && nestedPredicate.GetValue() == null)
                        return method.Modify(l => l.Create(typeof(bool),
                            o => Expression.Call(
                                typeof(Queryable),
                                nameof(Queryable.Any),
                                new[] { queryType },
                                o
                            )).Build(x));


                    return method.Combine(nestedPredicate,
                        (l, predicate) =>
                        {
                            if (predicate == null)
                                return l.Create(typeof(bool),
                                    o => Expression.Call(
                                        typeof(Queryable),
                                        nameof(Queryable.Any),
                                        new[] { queryType },
                                        o
                                    )).Build(x);


                            var nested = l.Create(typeof(bool),
                                o => Expression.Call(
                                    typeof(Queryable),
                                    nameof(Queryable.Any),
                                    new[] { queryType },
                                    o,
                                    predicate));

                            return nested.Build(x);
                        });
                });

            yield return inOperator;
            yield return inOperator.Modify(OperatorKind.collection_not_in, Expression.Not);
        }

        private static IEnumerable<QueryTreeOperator> CollectionCountOperators(
            TreeOperatorResult<LambdaBuilder> joinMethod, Type queryType)
        {
            var countExp = joinMethod.Modify(l => l.Create(typeof(int),
                o => Expression.Call(
                    typeof(Queryable),
                    nameof(Queryable.Count),
                    new[] { queryType },
                    o
                )));

            yield return new QueryTreeOperator(OperatorKind.quantity_1,
                (b, j, x) =>
                {
                    return countExp.Modify(l =>
                        l.Create(typeof(bool), o => Expression.Equal(o, Expression.Constant(1))).Build(x));
                });

            yield return new QueryTreeOperator(OperatorKind.quantity_equal,
                (b, j, x) =>
                {
                    return countExp.Modify(l =>
                        l.Create(typeof(bool), o => Expression.Equal(o, Expression.Constant(j.ToObject<int>()))).Build(x));
                });

            yield return new QueryTreeOperator(OperatorKind.quantity_less,
                (b, j, x) =>
                {
                    return countExp.Modify(l =>
                        l.Create(typeof(bool), o => Expression.LessThan(o, Expression.Constant(j.ToObject<int>()))).Build(x));
                });

            yield return new QueryTreeOperator(OperatorKind.quantity_greater,
                (b, j, x) =>
                {
                    return countExp.Modify(l =>
                        l.Create(typeof(bool), o => Expression.GreaterThan(o, Expression.Constant(j.ToObject<int>()))).Build(x));
                });

            yield return new QueryTreeOperator(OperatorKind.quantity_from_to,
                (b, j, x) =>
                {
                    return countExp.Modify(l =>
                        l.Create(typeof(bool),
                            o => Expression.AndAlso(
                                Expression.GreaterThanOrEqual(o, Expression.Constant(j[0].ToObject<int>())),
                                Expression.LessThanOrEqual(o, Expression.Constant(j[1].ToObject<int>())))
                        ).Build(x));
                });
        }

        private static readonly Dictionary<string, Func<Expression, Expression, Expression>> _operatorsExpressions =
            new Dictionary<string, Func<Expression, Expression, Expression>>
            {
                {"less", Expression.LessThan},
                {"greater", Expression.GreaterThan}
            };

        private static IEnumerable<QueryTreeOperator> CollectionAggregateOperators(
            TreeOperatorResult<LambdaBuilder> joinMethod, string mnemonic, Type queryType)
        {
            yield return new QueryTreeOperator(OperatorKind.aggregate_sum,
                (b, j, x) =>
                {
                    var paramExp = Expression.Parameter(queryType, "p");
                    var property = queryType.GetProperty(j.Value<string>("id"));
                    var propertyExp =
                        Expression.Lambda(Expression.Property(paramExp, j.Value<string>("id")), paramExp);

                    var sumExp = joinMethod.Modify(l => l.Create(property.PropertyType,
                        o => Expression.Call(
                            typeof(Queryable),
                            nameof(Queryable.Sum),
                            new[] { queryType },
                            o,
                            propertyExp
                        )));

                    var @operator = j.Value<string>("operator");

                    return sumExp.Modify(l =>
                        l.Create(typeof(bool),
                            o => _operatorsExpressions[@operator](o,
                                Expression.Convert(Expression.Constant(j.Value<double>("value")),
                                    property.PropertyType))).Build(x));
                });

            yield return new QueryTreeOperator(OperatorKind.aggregate_average,
                (b, j, x) =>
                {
                    var paramExp = Expression.Parameter(queryType, "p");
                    var property = queryType.GetProperty(j.Value<string>("id"));
                    var propertyExp =
                        Expression.Lambda(Expression.Property(paramExp, j.Value<string>("id")), paramExp);

                    var averageExp = joinMethod.Modify(l => l.Create(property.PropertyType,
                        o => Expression.Call(
                            typeof(Queryable),
                            nameof(Queryable.Average),
                            new[] { queryType },
                            o,
                            propertyExp
                        )));

                    var @operator = j.Value<string>("operator");

                    return averageExp.Modify(l =>
                        l.Create(typeof(bool),
                            o => _operatorsExpressions[@operator](o,
                                Expression.Constant(j.Value<double>("value")))).Build(x));
                        
                });

            yield return new QueryTreeOperator(OperatorKind.aggregate_max,
                (b, j, x) =>
                {
                    var paramExp = Expression.Parameter(queryType, "p");
                    var property = queryType.GetProperty(j.Value<string>("id"));
                    var propertyExp =
                        Expression.Lambda(Expression.Property(paramExp, j.Value<string>("id")), paramExp);

                    var maxExp = joinMethod.Modify(l => l.Create(property.PropertyType,
                        o => Expression.Call(
                            typeof(Queryable),
                            nameof(Queryable.Max),
                            new[] { queryType, property.PropertyType },
                            o,
                            propertyExp
                        )));

                    var @operator = j.Value<string>("operator");

                    return maxExp.Modify(l =>
                        l.Create(typeof(bool),
                            o => _operatorsExpressions[@operator](o,
                                Expression.Convert(Expression.Constant(j.Value<double>("value")),
                                    property.PropertyType))).Build(x));

                });

            yield return new QueryTreeOperator(OperatorKind.aggregate_min,
                (b, j, x) =>
                {
                    var paramExp = Expression.Parameter(queryType, "p");
                    var property = queryType.GetProperty(j.Value<string>("id"));
                    var propertyExp =
                        Expression.Lambda(Expression.Property(paramExp, j.Value<string>("id")), paramExp);

                    var minExp = joinMethod.Modify(l => l.Create(property.PropertyType,
                        o => Expression.Call(
                            typeof(Queryable),
                            nameof(Queryable.Min),
                            new[] { queryType, property.PropertyType },
                            o,
                            propertyExp
                        )));

                    var @operator = j.Value<string>("operator");

                    return minExp.Modify(l =>
                        l.Create(typeof(bool),
                            o => _operatorsExpressions[@operator](o,
                                Expression.Convert(Expression.Constant(j.Value<double>("value")),
                                    property.PropertyType))).Build(x));
                });
        }

        private static IEnumerable<QueryTreeOperator> UserIdOperators(LambdaBuilder method)
        {
            QueryTreeOperator CreateOperator(OperatorKind kind, Func<Expression, Expression, Expression> expression)
            {
                return new QueryTreeOperator(kind, (b, j, x) =>
                {
                    if ("current_user".Equals((j as JValue)?.Value))
                    {
                        Func<IQueryTreeBuilderContext, Expression> result = ctx =>
                        {
                            var userId = ctx.GetUserId();
                            if (userId == null)
                                return Expression.Constant(false, typeof(bool));


                            var userIdConst = Expression.Constant(userId.Value, method.ReturnType);

                            var userIdExpression = method.Create(typeof(bool), e => expression(e, userIdConst));

                            return userIdExpression.Build(x);
                        };

                        return result;
                    }

                    return GetPrimitiveTreeOperatorResult(method, expression, j, x);
                });
            }

            yield return CreateOperator(OperatorKind.equal, Expression.Equal);
            yield return CreateOperator(OperatorKind.not_equal, Expression.NotEqual);
        }

        private static IEnumerable<QueryTreeOperator> EqualsOperators(LambdaBuilder method)
        {
            yield return CreatePrimitiveTreeOperator(OperatorKind.equal, method, Expression.Equal);
            yield return CreatePrimitiveTreeOperator(OperatorKind.not_equal, method, Expression.NotEqual);
        }

        private static Expression GetInExpression(Expression valueExp, JToken j, Type returnType)
        {
            if (j == null)
                throw new ArgumentNullException(nameof(j), "Значение расширенного фильтра не может быть пустым.");
            var id = j.ToObject<Guid>();

            var toStringExp = typeof(object).GetMethod("ToString");

            var param = Expression.Parameter(typeof(OperatorInValues), "x");
            var where = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                new[]
                {
                    typeof(OperatorInValues)
                },
                OperatorInValues,
                Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(param, "IdForValue"),
                        Expression.Constant(id)), param));

            var select = Expression.Call(typeof(Queryable),
                nameof(Queryable.Select),
                new[]
                {
                    typeof(OperatorInValues),
                    typeof(string)
                }, where,
                Expression.Lambda<Func<OperatorInValues, string>>(Expression.Property(param, "Value"),
                    param));

            return Expression.Call(typeof(Queryable), nameof(Queryable.Contains), new[] { typeof(string) }, select, Expression.Call(valueExp, toStringExp));
        }

        private static IEnumerable<QueryTreeOperator> ComparableOperator(LambdaBuilder method)
        {
            yield return CreatePrimitiveTreeOperator(OperatorKind.greater, method, Expression.GreaterThan);

            yield return CreatePrimitiveTreeOperator(OperatorKind.greater_or_equal, method,
                Expression.GreaterThanOrEqual);

            yield return CreatePrimitiveTreeOperator(OperatorKind.less, method, Expression.LessThan);

            yield return CreatePrimitiveTreeOperator(OperatorKind.less_or_equal, method, Expression.LessThanOrEqual);

            var betweenOperator = new QueryTreeOperator(OperatorKind.between, (b, j, x) =>
            {
                var firstValue = Expression.Constant(j[0].ToObject(method.ReturnType), method.ReturnType);
                var secondValue = Expression.Constant(j[1].ToObject(method.ReturnType), method.ReturnType);

                var result = method.Create(typeof(bool), e =>
                    Expression.AndAlso(Expression.GreaterThanOrEqual(e, firstValue),
                        Expression.LessThanOrEqual(e, secondValue)));

                return result.Build(x);
            });

            yield return betweenOperator;

            yield return betweenOperator.Modify(OperatorKind.not_between, Expression.Not);

            var inOperator = new QueryTreeOperator(OperatorKind.@in, (b, j, x) =>
            {
                var result = method.Create(typeof(bool), e => GetInExpression(e, j, method.ReturnType));
                return result.Build(x);
            });

            yield return inOperator;

            yield return inOperator.Modify(OperatorKind.not_in, Expression.Not);
        }

        private static IEnumerable<QueryTreeOperator> StringOperator(LambdaBuilder method)
        {
            if (method.ReturnType != typeof(string))
                throw new ArgumentException();


            var begin = CreatePrimitiveTreeOperator(OperatorKind.begins_with, method, (x, y) =>
                Expression.Call(x, nameof(string.StartsWith), null, y));

            yield return begin;
            yield return begin.Modify(OperatorKind.not_begins_with, Expression.Not);

            var end = CreatePrimitiveTreeOperator(OperatorKind.ends_with, method, (x, y) =>
                Expression.Call(x, nameof(string.EndsWith), null, y));

            yield return end;
            yield return end.Modify(OperatorKind.not_ends_with, Expression.Not);

            var contains = CreatePrimitiveTreeOperator(OperatorKind.contains, method, (x, y) =>
                Expression.Call(x, nameof(string.Contains), null, y));

            yield return contains;
            yield return contains.Modify(OperatorKind.not_contains, Expression.Not);

            var inOperator = new QueryTreeOperator(OperatorKind.@in, (b, j, x) =>
            {
                var result = method.Create(typeof(bool), e => GetInExpression(e, j, method.ReturnType));
                return result.Build(x);
            });

            yield return inOperator;

            yield return inOperator.Modify(OperatorKind.not_in, Expression.Not);
        }

        private static bool GetIsEasyCollectionEntry(Type type, ref Type result)
        {
            do
            {
                if (GetGenericArgument(type, typeof(EasyCollectionEntry<>), 0, ref result))
                    return true;
                type = type.BaseType;
            } while (type != null);

            return false;
        }

        private static bool GetIsCollection(Type type, ref Type result)
        {
            if (type == typeof(string) || type == typeof(byte[]))
                return false;

            var types = type.GetInterfaces()
                .Where(x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(x => x.GetGenericArguments()[0]).Take(2).ToArray();

            if (types.Length == 1)
            {
                result = types[0];
                return true;
            }

            return false;
        }

        private static bool GetIsBaseObject(Type type)
        {
            return typeof(BaseObject).IsAssignableFrom(type);
        }

        private static bool GetGenericArgument(Type type, Type generic, int index, ref Type result)
        {
            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == generic)
            {
                result = type.GetGenericArguments()[0];
                return true;
            }

            return false;
        }

        private static bool GetIsNullable(Type type, ref Type result)
        {
            return GetGenericArgument(type, typeof(Nullable<>), 0, ref result);
        }
    }
}