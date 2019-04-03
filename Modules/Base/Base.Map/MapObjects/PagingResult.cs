using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.MapObjects
{
    [JsonObject]
    public class PagingResult<T> : IReadOnlyCollection<T>
    {
        public static readonly PagingResult<T> Empty = new PagingResult<T>
        {
            Items = new T[0]
        };

        public IReadOnlyCollection<T> Items { get; set; }

        public int Count => Items?.Count ?? 0;

        public int PageSize { get; set; }

        public int Page { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((float)TotalCount / PageSize) : 0;

        public IEnumerator<T> GetEnumerator()
        {
            return Items?.GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}