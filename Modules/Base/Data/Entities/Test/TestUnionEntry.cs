using Base.Attributes;
using Base.ComplexKeyObjects.Unions;

namespace Data.Entities.Test
{
    public class TestUnionEntry : BaseUnionEntry<TestUnionEntry>
    {
        [ListView("jopa")]
        public string Name { get; set; }

        [ListView("jj")]
        public string Description { get; set; }


        [ListView("jj5454")]
        public int TestNull { get; set; }
    }
}