using System;
using System.Collections.Generic;

namespace Base.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                return;

            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}