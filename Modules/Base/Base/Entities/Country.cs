using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Utils.Common.Attributes;

namespace Base.Entities
{
    [EnableFullTextSearch]
    [ViewModelConfig(Title = "Страны", Icon = "halfling halfling-globe")]
    public class Country : BaseObject
    {
        [FullTextSearchProperty]
        [DetailView(Name = "2-буквенный код", Required = true, Order = 0)]
        [ListView]
        [MaxLength(2)]
        public string Alpha2Code { get; set; }

        [DetailView(Name = "Цифровой код", Required = true, Order = 1)]
        [ListView]
        public int NumericCode { get; set; }

        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Required = true, Order = 2)]
        [ListView]
        public string Title { get; set; }
    }
}