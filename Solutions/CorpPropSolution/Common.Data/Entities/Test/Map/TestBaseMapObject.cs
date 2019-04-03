using System.Collections.Generic;
using Base;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.EntityFrameworkTypes.Complex;
using Base.Map;
using Base.Utils.Common.Attributes;

namespace Common.Data.Entities.Test.Map
{
    public class TestBaseMapObject : BaseObject, IGeoObject, ISuperObject<TestBaseMapObject>
    {
        public TestBaseMapObject()
        {
            Location = new Location();
        }

        #region IGeoObject

        [DetailView(Name = "Название", Order = 1), ListView]
        [FullTextSearchProperty]
        public string Title { get; set; }

        [DetailView(Name = "Описание", Order = 2), ListView]
        [FullTextSearchProperty]
        public string Description { get; set; }

        [DetailView(TabName = "[1]Местоположение на карте", Order = 3), ListView]
        public Location Location { get; set; }

        #endregion IGeoObject

        [ListView]
        public string ExtraID { get; } = null;

        public ICollection<TestSimpleMapObject> Many { get; set; }
    }
}