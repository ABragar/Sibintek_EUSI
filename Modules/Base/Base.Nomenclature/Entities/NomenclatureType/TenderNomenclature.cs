using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Nomenclature.Entities.Category;
using Newtonsoft.Json;

namespace Base.Nomenclature.Entities.NomenclatureType
{
    //public class TenderNomenclature : Nomenclature, ICategorizedItem
    //{
    //    #region ICategorizedItem
    //    [SystemProperty]
    //    public int CategoryID { get; set; }

    //    [JsonIgnore]
    //    [ForeignKey("CategoryID")]
    //    public virtual NomenclatureCategory Category_ { get; set; }

    //    HCategory ICategorizedItem.Category => this.Category_;

    //    #endregion
    //}
}
