using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;

namespace Base.UI.RegisterMnemonics.Entities
{
    public class MnemonicItem : BaseObject, ISuperObject<MnemonicItem>
    {
        [ListView("Тип", Order = -100)]
        public string ExtraID { get; }

        [MaxLength(255)]
        [UniqueIndex("Mnemonic")]
        [DetailView("Мнемоника", Required = true, Order = -10), ListView]        
        public string Mnemonic { get; set; }

        [ListView("Описание")]
        public string Description { get; set; }
    }
}