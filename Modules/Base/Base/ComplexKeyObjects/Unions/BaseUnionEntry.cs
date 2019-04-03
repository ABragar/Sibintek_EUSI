using Base.Attributes;

namespace Base.ComplexKeyObjects.Unions
{

    public abstract class BaseUnionEntry<TUnionEntry> : IUnionEntry<TUnionEntry>
        where TUnionEntry : BaseUnionEntry<TUnionEntry>
    {
        [SystemProperty]
        public int ID { get; set; }


        [ListView]
        public string ExtraID { get; set; }

        [SystemProperty]
        public bool Hidden { get; set; }
    }
}