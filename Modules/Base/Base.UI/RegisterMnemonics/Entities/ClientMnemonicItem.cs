using System.ComponentModel.DataAnnotations;
using Base.Attributes;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class ClientMnemonicItem : MnemonicItem
    {
        [MaxLength(255)]
        [DetailView("Баз. мнемоника", Required = true, Order = -50)]
        public string ParentMnemonic { get; set; }
    }
}