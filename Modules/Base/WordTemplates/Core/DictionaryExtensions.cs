using System;
using System.Collections.Generic;

namespace WordTemplates.Core
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            Func<TValue> func)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = func();
                dictionary.Add(key,value);
            }

            return value;
        }
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
    Func<TKey,TValue> func)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = func(key);
                dictionary.Add(key, value);
            }

            return value;
        }


        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return default(TValue);
        }
    }
}
