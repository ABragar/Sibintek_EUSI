

using Base.Service;

namespace Base.ComplexKeyObjects.Unions
{
    public interface IUnionService<TUnionEntry> : IQueryService<TUnionEntry>
        where TUnionEntry : class, IUnionEntry<TUnionEntry>
    {

    }
}