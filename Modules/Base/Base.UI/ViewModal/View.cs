using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace Base.UI.ViewModal
{
    public abstract class View
    {
        private readonly ConcurrentDictionary<string, LambdaExpression> _selectLambdaExpressions
            = new ConcurrentDictionary<string, LambdaExpression>();


        private readonly ConcurrentDictionary<string, Delegate> _selectDelegates
            = new ConcurrentDictionary<string, Delegate>();

        public abstract IEnumerable<PropertyViewModel> Props { get; }

        public virtual Type TypeEntity => Config.TypeEntity;

        public ViewModelConfig Config { get; set; }

        private LambdaExpression GetSelector(IEnumerable<string> props = null, Type elementType = null)
        {
            var properties = (props ?? Props.Select(x => x.PropertyName))
                .Where(x => Ambient.AppContext.SecurityUser.PropertyCanRead(TypeEntity, x));

            var enumerable = properties as string[] ?? properties.ToArray();

            var itType = elementType ?? TypeEntity;
            var key = string.Join(";", enumerable.OrderBy(x => x)) + itType.ToString();
            return _selectLambdaExpressions.GetOrAdd(key, x =>
            {
                var builder =
                    new SelectBuilder(this, new SelectStringBuilder()).Where(enumerable)
                        .WriteAllProperties(this is DetailView)
                        .WriteDiscriminator(!(this is DetailView));

                string select = builder.Build();

                return DynamicExpression.ParseLambda(itType, (Type)null, select, builder.Parameters);
            });
        }

        private Delegate GetDelegateSelector(IEnumerable<string> props = null)
        {
            var properties = props ?? Props.Select(x => x.PropertyName);

            var enumerable = properties as string[] ?? properties.ToArray();

            string key = string.Join(";", enumerable.OrderBy(x => x));

            return _selectDelegates.GetOrAdd(key, x =>
            {
                var builder = new SelectBuilder(this, new SelectStringBuilder()).Where(enumerable)
                    .CheckNullCollection()
                    .WriteAllProperties(this is DetailView)
                    .WriteDiscriminator(!(this is DetailView));

                string select = builder.Build();

                var lambda = DynamicExpression.ParseLambda(TypeEntity, (Type)null, select, builder.Parameters);

                return lambda.Compile();

            });
        }

        public virtual IQueryable Select(IQueryable q, string[] props = null)
        {
            var s = GetSelector(props, q.ElementType);

            var e = Expression.Call(typeof(Queryable), "Select",
                new Type[]
                {
                    //Config.TypeEntity,
                    q.ElementType,
                    s.Body.Type
                },
                q.Expression, Expression.Quote(s));

            var r = q.Provider.CreateQuery(e);

            return r;
        }


        public object SelectObj(object obj)
        {
            var s = GetDelegateSelector();

            return s.DynamicInvoke(obj);
        }

        public virtual T Copy<T>(T view = null) where T : View, new()
        {
            var v = view ?? new T();

            return v;
        }
    }
}