using System.Collections.Generic;
using Base;
using Base.Attributes;

namespace Common.Data.Entities.Test
{
    public class TestObject3Category:HCategory
    {
        private ICategorizedItem _categorizedItemImplementation;

        [ListView]
        [DetailView]
        public string Field_1 { get; set; }

        public override HCategory Parent { get; }
        public override IEnumerable<HCategory> Children { get; }
    }
}