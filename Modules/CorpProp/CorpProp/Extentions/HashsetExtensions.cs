using System.Collections.Generic;

namespace CorpProp.Extentions
{
    public static class HashsetExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
