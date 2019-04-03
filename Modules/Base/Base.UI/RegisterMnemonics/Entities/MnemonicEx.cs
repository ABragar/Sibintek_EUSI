using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Translations;
using Base.UI.ViewModal;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class MnemonicEx : BaseObject, ISuperObject<MnemonicEx>, IViewModelConfigVisitor
    {
        private static readonly CompiledExpression<MnemonicEx, string> _mnemonic =
            DefaultTranslationOf<MnemonicEx>.Property(x => x.Mnemonic).Is(x => x.MnemonicItem != null ? x.MnemonicItem.Mnemonic : null);

        [SystemProperty]
        public int MnemonicItemID { get; set; }
        [SystemProperty]
        public string Mnemonic => _mnemonic.Evaluate(this);
        public MnemonicItem MnemonicItem { get; set; }

        [ListView("“ËÔ")]
        public string ExtraID { get; }

        public virtual void Visit(ConfigTitle configTitle)
        {

        }

        public virtual void Visit(ConfigListViewFilter listConfigListViewFilter)
        {

        }

        public virtual void Visit(ConfigEditor configEditor)
        {

        }

        public virtual void Visit(ConfigColumn configColumn)
        {

        }

        public virtual void Visit(ConfigListView configListView)
        {

        }
    }
}