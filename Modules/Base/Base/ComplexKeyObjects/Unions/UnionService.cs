using System;
using System.Linq;
using System.Linq.Expressions;
using Base.ComplexKeyObjects.Common;
using Base.ComplexKeyObjects.Unions.Implementation;
using Base.DAL;

namespace Base.ComplexKeyObjects.Unions
{
    public class UnionService<TUnionEntry> : IUnionService<TUnionEntry>
        where TUnionEntry : class, IUnionEntry<TUnionEntry>
    {
        private readonly UnionConfig<TUnionEntry> _union_config;

        public UnionService(UnionConfig<TUnionEntry> union_config)
        {
            _union_config = union_config;
        }

        private static readonly Expression<Func<TUnionEntry, bool>> where_hidden =
            RemoveConvertVisitor.Instance.VisitAndConvert<Expression<Func<TUnionEntry, bool>>>(x => x.Hidden, "where_hidden");

        private static readonly Expression<Func<TUnionEntry, bool>> where_not_hidden =
            RemoveConvertVisitor.Instance.VisitAndConvert<Expression<Func<TUnionEntry, bool>>>(x => !x.Hidden, "where_not_hidden");

        public virtual IQueryable<TUnionEntry> GetAll(IUnitOfWork unit_of_work, bool? hidden)
        {
            var q = _union_config.GetQuery(unit_of_work);

            if (hidden != null)
                q = q.Where(hidden.Value ? where_hidden : where_not_hidden);

            return q.OrderBy(x => 1);
        }
    }
}