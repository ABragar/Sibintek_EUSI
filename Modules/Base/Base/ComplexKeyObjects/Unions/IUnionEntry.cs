namespace Base.ComplexKeyObjects.Unions
{
    public interface IUnionEntry<TUnionEntry> : IComplexKeyObject
        where TUnionEntry : class, IUnionEntry<TUnionEntry>
    {
        bool Hidden { get; }
    }
}