using Base;
using Base.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public abstract class PropertyObject : BaseObject
    {
        [DetailView("ID", Required = true)]
        [ListView]
        public int NumberID { get; set; }

        [DetailView("ID ЕУСИ")]
        [ListView]
        public int NumberEUSI { get; set; }

        [DetailView("Наименование", Required = true)]
        [ListView]
        [MaxLength(255)]
        public string Name { get; set; }

        [DetailView("Примечание")]
        [ListView]
        public string Notes { get; set; }


        //public int? ObjectRecordID { get; set; }

        //[DetailView("Объект", Required = true)]
        //[ListView]
        //public virtual ObjectRecord ObjectRecord { get; set; }


        //public int? EstimationID { get; set; }

        //[DetailView("Объект", Required = true)]
        //[ListView]
        //public virtual Estimation Estimation { get; set; }

        //public int? DealID { get; set; }

        //[DetailView("Сделка", Required = true)]
        //[ListView]
        //public virtual Deal Deal { get; set; }

        //public int? RightsAndEncumbrancesID { get; set; }

        //[DetailView("Права и обязанности", Required = true)]
        //[ListView]
        //public virtual RightsAndEncumbrances RightsAndEncumbrances { get; set; }

        //public int? DocumentID { get; set; }

        //[DetailView("Документ", Required = true)]
        //[ListView]
        //public virtual Document Document { get; set; }
    }
}
