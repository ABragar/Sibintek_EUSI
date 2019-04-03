using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.Translations
{
    public class TranslationMap
    {

        private struct MapKey
        {
            public readonly MemberInfo MemberInfo;
            public readonly Type Type;

            public MapKey(MemberInfo member_info, Type type)
            {
                MemberInfo = member_info;
                Type = type;
            }

            public override string ToString()
            {
                return $"{MemberInfo.ReflectedType}({Type?.ToString() ?? "null"},{MemberInfo.DeclaringType}) {MemberInfo.Name}";
            }
        }

        private class MapKeyComparer : IEqualityComparer<MapKey>
        {
            public bool Equals(MapKey x, MapKey y)
            {
              
            
                return x.MemberInfo == y.MemberInfo && (x.Type == null || y.Type == null || x.Type == y.Type);
            }

            public int GetHashCode(MapKey obj)
            {
                return obj.MemberInfo.GetHashCode();
            }
        }


        private readonly ConcurrentDictionary<MapKey, CompiledExpression> _items = new ConcurrentDictionary<MapKey, CompiledExpression>(new MapKeyComparer());

        public static TranslationMap DefaultMap = new TranslationMap();

        internal bool TryGet(MemberInfo member_info, Type type, out CompiledExpression result)
        {
            return _items.TryGetValue(new MapKey(member_info, type), out result);
        }



        public CompiledExpression<T, TResult> AddStrict<TStrict,T, TResult>(Expression<Func<T, TResult>> property,
            Expression<Func<T, TResult>> expression)
            where TStrict: T
        {
            

            if (!_items.TryAdd(new MapKey(((MemberExpression)property.Body).Member, typeof(TStrict)), new CompiledExpression(expression)))
                throw new InvalidOperationException();

            return new CompiledExpression<T, TResult>(expression);
        }

        public CompiledExpression<T, TResult> Add<T, TResult>(Expression<Func<T, TResult>> property, Expression<Func<T, TResult>> expression)
        {
            


            if (!_items.TryAdd(new MapKey(((MemberExpression)property.Body).Member, null), new CompiledExpression(expression)))
                throw new InvalidOperationException();

            return new CompiledExpression<T, TResult>(expression);
        }
    }
}