using Base.Attributes;

namespace Base.Macros.Entities.Rules
{
    public class RuleForMnemonic : Macros.Entities.Rules.Rule
    {
        [ListView]
        [DetailView("Мнемоника", ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Mnemonic)]
        public override string ObjectType { get; set; }
    }
}