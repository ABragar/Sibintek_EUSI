using System;

namespace Base.UI.ViewModal
{
    public class Aggregate
    {
        public string Property { get; set; }
        public AggregateType Type { get; set; }
        public string Template { get; set; }

        public Aggregate Copy()
        {
            return new Aggregate()
            {
                Property = this.Property,
                Type = this.Type,
                Template = this.Template
            };
        }
    }

    public enum AggregateType
    {
        Sum,
        Min,
        Max,
        Average,
        Count
    }
}