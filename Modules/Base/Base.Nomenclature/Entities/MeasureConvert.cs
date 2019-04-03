using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.Attributes;
using Base.Nomenclature.Entities.NomenclatureType;
using BaseCatalog.Entities;

namespace Base.Nomenclature.Entities
{
    public class MeasureConvert : BaseObject
    {
        public int SourceID { get; set; }
        [ListView]
        [DetailView("Исходная единица измерения")]
        public virtual Measure Source { get; set; }


        public int DestID { get; set; }
        [ListView]
        [DetailView("Конечная единица измерения")]
        public virtual Measure Dest { get; set; }

        [ListView]
        [DetailView("Коэффициент")]
        public double Multiplier { get; set; }

        public int? TmcNomenclatureID { get; set; }

        [ListView]
        [DetailView("Номенклатура")]
        public virtual TmcNomenclature TmcNomenclature { get; set; }
    }
}
