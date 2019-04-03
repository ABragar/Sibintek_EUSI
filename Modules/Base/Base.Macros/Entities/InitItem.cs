using System;
using Base.Attributes;

namespace Base.Macros.Entities
{
    [Serializable]
    public class InitItem : BaseObject
    {
        [SystemProperty]
        public MacroType MacroType { get; set; }

        [SystemProperty]
        public string Member { get; set; }

        [SystemProperty]
        public string Value { get; set; }
    }

    public class ConditionItem : InitItem
    {
        [SystemProperty]
        public string Operator { get; set; } = "==";

        [SystemProperty]
        public ConditionItemSource Source { get; set; }
    }

    public enum ConditionItemSource
    {
        FromObj =0,
        Value = 1,
    }
}
