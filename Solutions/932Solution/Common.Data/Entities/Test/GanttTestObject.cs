using System;
using Base;
using Base.Attributes;
using Base.Translations;

namespace Common.Data.Entities.Test
{
    public class GanttTestObject : BaseObject, IGantt
    {
        private static readonly CompiledExpression<GanttTestObject, int> _orderId =
            DefaultTranslationOf<GanttTestObject>.Property(x => x.OrderId).Is(x => (int) x.SortOrder);

        [SystemProperty]
        public int TestObjectID { get; set; }

        public TestObject TestObject { get; set; }

        [ListView]
        [DetailView("Наименование")]
        public string Title { get; set; }

        [ListView]
        [DetailView("Начало")]
        public DateTime Start { get; set; }

        [ListView]
        [DetailView("Конец")]
        public DateTime End { get; set; }

        [DetailView(Visible = false)]
        [ListView]
        public int OrderId => _orderId.Evaluate(this);

        [DetailView(Visible = false)]
        [ListView]
        public decimal PercentComplete { get; set; } = 1;
    }
}