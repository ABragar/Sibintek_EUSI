using System.Collections.Generic;
using Base;
using Base.Attributes;
using Base.EntityFrameworkTypes.Complex;
using Base.Map;
using Base.Utils.Common.Attributes;

namespace Common.Data.Entities.Test.Map
{
    [EnableFullTextSearch]
    public class TestSimpleMapObject : BaseObject, IGeoObject
    {
        [DetailView(Name = "Title", Order = 1), ListView]
        [FullTextSearchProperty]
        public string Title { get; set; }

        [DetailView(Name = "Description", Order = 1), ListView]
        [FullTextSearchProperty]
        public string Description { get; set; }

        [DetailView(Name = "Location", Order = 1), ListView(Visible = false)]
        public Location Location { get; set; } = new Location();

        [DetailView(Name = "Easy", Order = 1), ListView]
        public virtual ICollection<TestEasy> Easy { get; set; }

        [DetailView(Name = "Many", Order = 1), ListView]
        public virtual ICollection<TestBaseMapObject> Many { get; set; }
    }


    public class TestEasy : EasyCollectionEntry<TestBaseMapObject>
    {
    }
}