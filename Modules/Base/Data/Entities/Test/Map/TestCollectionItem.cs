using Base;
using Base.Attributes;
using System;

namespace Data.Entities.Test.Map
{
    public class TestCollectionItem : BaseObject
    {
        [DetailView(Name = "Наименование"), ListView]
        public string Title { get; set; }

        [SystemProperty]
        [DetailView(Name = "Описание"), ListView]
        public string Description { get; set; }

        [SystemProperty]
        [DetailView(Name = "Дата"), ListView]
        public DateTime? Date { get; set; }

        [SystemProperty]
        [DetailView(Name = "Количество"), ListView]
        public int Count { get; set; }
    }
}