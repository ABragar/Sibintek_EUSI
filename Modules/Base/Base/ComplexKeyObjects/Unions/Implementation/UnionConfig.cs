using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Base.ComplexKeyObjects.Common;
using Base.DAL;

namespace Base.ComplexKeyObjects.Unions.Implementation
{
    public class UnionConfig<TUnionEntry> : IQuerySource<TUnionEntry>
        where TUnionEntry : class, IUnionEntry<TUnionEntry>
    {
        private static readonly IQueryable<TUnionEntry> Empty = Enumerable.Empty<TUnionEntry>().AsQueryable();


        private readonly IList<IQuerySource<TUnionEntry>> _items = new List<IQuerySource<TUnionEntry>>();

        private readonly UnionVisitor<TUnionEntry> _visitor = new UnionVisitor<TUnionEntry>();
        public void AddItem<TItem>(IQuerySource<TItem> source, Expression<Func<TItem, TUnionEntry>> selector,
            Expression<Func<TItem, bool>> filter,
            params SelectorOverride<TItem, TUnionEntry>[] overrides)
        {
            _items.Add(new UnionConfigItem<TItem>(source, _visitor.InitSelector(selector, overrides), filter));
        }

        public IQueryable<TUnionEntry> GetQuery(IUnitOfWork unit_of_work)
        {
            IQueryable<TUnionEntry> q = null;

            foreach (var item in _items)
            {
                q = q?.Concat(item.GetQuery(unit_of_work)) ?? item.GetQuery(unit_of_work);
            }

            return q ?? Empty;

        }

        private class UnionConfigItem<TItem> : IQuerySource<TUnionEntry>
        {
            private readonly IQuerySource<TItem> _source;
            private readonly Expression<Func<TItem, bool>> _filter;
            private readonly Expression<Func<TItem, TUnionEntry>> _selector;

            public UnionConfigItem(IQuerySource<TItem> source, Expression<Func<TItem, TUnionEntry>> selector, Expression<Func<TItem, bool>> filter)
            {
                if (selector == null)
                    throw new ArgumentNullException(nameof(selector));

                _selector = selector;
                _filter = filter;
                _source = source;
            }

            public IQueryable<TUnionEntry> GetQuery(IUnitOfWork unit_of_work)
            {
                var q = _source.GetQuery(unit_of_work);

                if (_filter != null)
                    q = q.Where(_filter);

                return q.Select(_selector);
            }
        }
    }
}