using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Nomenclature.Entities.Category;
using Base.Translations;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.Nomenclature.Entities.NomenclatureType
{
    public class ServicesNomenclature : Nomenclature, ICategorizedItem
    {
        //    private static readonly CompiledExpression<ServicesNomenclature, string> _codeNomenclature =
        //DefaultTranslationOf<ServicesNomenclature>.Property(x => x.CodeNomenclature).Is(x => x.Category_ != null ? x.Category_.Code + "." + x.ID : "");

        //[DetailView("Код", ReadOnly = true), ListView]
        //[SystemProperty]
        //[FullTextSearchProperty]
        //public string CodeNomenclature => _codeNomenclature.Evaluate(this);

        #region ICategorizedItem
        [SystemProperty]
        public int CategoryID { get; set; }

        [JsonIgnore]
        [ForeignKey("CategoryID")]
        public virtual ServicesCategory Category_ { get; set; }

        HCategory ICategorizedItem.Category => this.Category_;

        #endregion
    }
}
