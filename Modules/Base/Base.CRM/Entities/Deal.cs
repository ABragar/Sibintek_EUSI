using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Document.Entities;
using Base.Entities.Complex;
using Base.Utils.Common.Attributes;

namespace Base.CRM.Entities
{
    public class Deal : BaseDocument, IDeal
    {
        public int? StatusID { get; set; }
        [DetailView("Статус", Order = 10, ReadOnly = true), ListView]
        public virtual DealStatus Status { get; set; }

        [DetailView("\"Холодная\" сделка", Order = 11)]
        public bool IsCold { get; set; }

        [DetailView(TabName = "[1]Номенклатура")]
        public virtual ICollection<DealNomenclature> Nomenclature { get; set; } = new List<DealNomenclature>();

        [DetailView(TabName = "[2]Версионность", ReadOnly = true)]
        public virtual ICollection<DealNomenclatureVersion> NomenclatureVersion { get; set; }

        [DetailView(TabName = "[3]Скидки")]
        public virtual ICollection<DealDiscount> DealDiscounts { get; set; } = new List<DealDiscount>();

        [DetailView("Сумма")]
        public decimal Amount { get; set; }
    }

    public class DealNomenclature : BaseObject
    {
        public int NomenclatureID { get; set; }
        [DetailView("Номенклатура", Required = true), ListView]
        public virtual Nomenclature.Entities.NomenclatureType.Nomenclature Nomenclature { get; set; }

        [DetailView("Кол-во", Required = true), ListView]
        public int Amount { get; set; }
    }

    public class DealNomenclatureVersion : BaseObject
    {
        public int? NomenclatureID { get; set; }
        [DetailView("Номенклатура", Required = true), ListView]
        public virtual Nomenclature.Entities.NomenclatureType.Nomenclature Nomenclature { get; set; }

        [DetailView("Кол-во", Required = true), ListView]
        public int Amount { get; set; }

        public int? DocumentStatusID { get; set; }
        [DetailView("Статус", Order = 20, ReadOnly = true), ListView]
        public virtual DealStatus DocumentStatus { get; set; }



    }

    [EnableFullTextSearch]
    public class DealSource : BaseObject
    {
        public DealSource()
        {
            this.Icon = new Icon();
        }

        [MaxLength(255)]
        [DetailView("Наименование"), ListView]
        public string Title { get; set; }

        [DetailView("Иконка"), ListView]
        public Icon Icon { get; set; }
    }

    public class DealStatus : BaseObject
    {
        public DealStatus()
        {
            Icon = new Icon();
        }

        [SystemProperty]
        [DetailView("Иконка"), ListView]
        public Icon Icon { get; set; }

        [DetailView("Наименование", Required = true), ListView]
        public string Title { get; set; }

        [DetailView("Отображать на воронке продаж"), ListView]
        public bool IsFunnel { get; set; } = true;
    }

    //TODO ЧТО ЗА ХРЕНЬ
    public class DealDiscount : EasyCollectionEntry<DiscountBase<BaseObject>>
    {
        public int? DealID { get; set; }
        public virtual Deal Deal { get; set; }
    }
}