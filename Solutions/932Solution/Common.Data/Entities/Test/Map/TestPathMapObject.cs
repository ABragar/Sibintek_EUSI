using System.Collections.Generic;
using Base.Attributes;

namespace Common.Data.Entities.Test.Map
{
    public class TestPathMapObject : TestBaseMapObject
    {
        [DetailView(Name = "Тестовые точечные объекты (Many To Many)", Order = 14), ListView]
        public virtual ICollection<TestMarkerMapObject> MarkerItems { get; set; }
    }
}