using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FankySheet.Internal
{
    public static class EnumerableExtensions
    {

        public static IEnumerable<TElement> AppendBefore<TElement>(this IEnumerable<TElement> source, TElement element)
        {
            yield return element;

            foreach (var el in source)
            {
                yield return el;
            }
        }


        private class PartContext<TElement>
        {
            public int Count;

            public bool IsSourceEnd;

            public IEnumerator<TElement> Enumerator;


        }

        private class Part<TElement> : IEnumerable<TElement>
        {
            private readonly PartContext<TElement> _context;

            public Part(PartContext<TElement> context)
            {
                _context = context;
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                if (_is_part_end)
                    throw new InvalidOperationException();


                yield return _context.Enumerator.Current;

                while (MoveNext())
                {
                    yield return _context.Enumerator.Current;
                }
            }

            private bool _is_part_end = false;


            private int _index = 0;


            public void MoveToEnd()
            {

                while (!_is_part_end)
                {
                    if (!_context.Enumerator.MoveNext())
                    {
                        _context.IsSourceEnd = true;
                        _is_part_end = true;
                        return;
                    }
                    if (++_index >= _context.Count)
                    {
                        _is_part_end = true;
                        return;
                    }
                }
            }

            private bool MoveNext()
            {
                if (_context.Enumerator.MoveNext())
                {
                    if (++_index < _context.Count)
                    {
                        return true;
                    }

                    _is_part_end = true;
                    return false;
                }

                _context.IsSourceEnd = true;
                _is_part_end = true;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {

                return GetEnumerator();
            }
        }

        public static IEnumerable<IEnumerable<TElement>> PartByCount<TElement>(this IEnumerable<TElement> source, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));


            using (var enumerator = source.GetEnumerator())
            {
                var is_source_end = !enumerator.MoveNext();

                if (is_source_end)
                    yield break;

                var context = new PartContext<TElement>()
                {
                    Enumerator = enumerator,
                    Count = count,
                    IsSourceEnd = false
                };

                while (true)
                {
                    

                    if (context.IsSourceEnd)
                        yield break;

                    var part = new Part<TElement>(context);

                    yield return part;

                    part.MoveToEnd();
                }
            }
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source,
            int begin,
            Func<TSource, int,TResult> selector)
        {
            var index = begin;
            return source.Select(x =>
            {
                var i = begin++;
                return selector(x, i);
            });

        }
    }


    public class Part<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly IEnumerable<TElement> _source;

        public Part(TKey key, IEnumerable<TElement> source)
        {
            _source = source;
            Key = key;
        }


        public IEnumerator<TElement> GetEnumerator()
        {
            foreach (var element in _source)
            {
                yield return element;
            }
                ;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var element in _source)
            {
                yield return element;
            }
        }

        public TKey Key { get; }
    }

}