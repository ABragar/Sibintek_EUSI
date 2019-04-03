using System.Collections.Generic;
using Base.Attributes;
using Base.UI;

namespace Base.Document.Entities
{
    public class Contract : BaseDocument
    {
        [DetailView("Статус", Order = 10), ListView]
        public ContractStatus Status { get; set; }

        [DetailView(TabName = "[1]Номенклатура")]
        public virtual ICollection<ContractNomenclature> Nomenclature { get; set; } = new List<ContractNomenclature>();

        [DetailView("Сумма контракта")]
        public decimal Amount { get; set; }

        [DetailView("Контракт подписан")]
        public bool IsSigned { get; set; }
    }

    [UiEnum]
    public enum ContractStatus
    {
        [UiEnumValue("Подготовка")]
        Preparation,
        [UiEnumValue("Ожидает подписания")]
        PendingSigning,
        [UiEnumValue("Подписан")]
        Signed,
        [UiEnumValue("Отменен")]
        Canceled,
    }

    public class ContractNomenclature : BaseObject
    {
        public int NomenclatureID { get; set; }
        [DetailView("Номенклатура", Required = true), ListView]
        public virtual Nomenclature.Entities.NomenclatureType.Nomenclature Nomenclature { get; set; }

        [DetailView("Кол-во", Required = true), ListView]
        public int Quantity { get; set; }
    }
}
