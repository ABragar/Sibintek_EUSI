using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Nomenclature.Entities.Category;
using Base.Translations;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.Nomenclature.Entities.NomenclatureType
{
    [EnableFullTextSearch]
    public class TmcNomenclature : Nomenclature, ICategorizedItem
    {
        //private static readonly CompiledExpression<TmcNomenclature, string> _codeNomenclature =
        //    DefaultTranslationOf<TmcNomenclature>.Property(x => x.CodeNomenclature)
        //        .Is(x => x.Category_ != null ? x.Category_.Code + "." + x.ID : "");

        //[DetailView("Код", ReadOnly = true, Order = -9), ListView]
        //[SystemProperty]
        //[FullTextSearchProperty]
        //public string CodeNomenclature => _codeNomenclature.Evaluate(this);

        #region ICategorizedItem

        [SystemProperty]
        public int CategoryID { get; set; }

        [JsonIgnore]
        [ForeignKey("CategoryID")]
        public virtual TmcCategory Category_ { get; set; }

        public int? Okpd2ID { get; set; }

        [DetailView("Код ОКПД2"), ListView]
        public virtual OKPD2 Okpd2 { get; set; }

        HCategory ICategorizedItem.Category => this.Category_;

        #endregion

        public int GroupAccountingID { get; set; }

        [DetailView("Группа учета"), ListView]
        public virtual GroupAccounting GroupAccounting { get; set; }

        [DetailView("Дата изменения", Order = 100, ReadOnly = true)]
        public DateTime? ChangeDate { get; set; }

        //[DetailView("Не используется в заявке", Order = 200)]
        //public bool NotUsedApplication { get; set; }
    }
}