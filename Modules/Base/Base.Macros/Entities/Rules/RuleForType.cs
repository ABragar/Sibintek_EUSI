using Base.Attributes;

namespace Base.Macros.Entities.Rules
{
    public class RuleForType: Macros.Entities.Rules.Rule
    {
        [ListView]
        [DetailView("Тип", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.ObjectType)]
        public override string ObjectType { get; set; }
    }
}
