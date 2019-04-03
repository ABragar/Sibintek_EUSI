using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет кадастровую стоимость.
    /// </summary>
    [EnableFullTextSearch]
    public class CadastralValue : TypeObject
    {

        /// <summary>
        /// Получает или задает стоимость.
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Cтоимость, руб.", TabName = "[0]Кадастровая стоимость", Required = true)]        
        public decimal? Value { get; set; }

        /// <summary>
        /// Получает или задает дату актуальности.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата актуальности",
        TabName = "[0]Кадастровая стоимость")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Получает или задает дату актуальности.
        /// </summary>
        [DetailView(Name = "Дата начала действия",
        TabName = "[0]Кадастровая стоимость")]
        [FullTextSearchProperty]
        [ListView]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Получает или задает дату актуальности.
        /// </summary>
        [DetailView(Name = "Дата окончания действия",
        TabName = "[0]Кадастровая стоимость")]
        [FullTextSearchProperty]
        [ListView]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Получает или задает ИД источника информации.
        /// </summary>
        public int? InformationSourceID { get; set; }

        /// <summary>
        /// Получает или задает источник информации.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Источник информации", TabName = "[0]Кадастровая стоимость"/*, Required = true*/)]
        public virtual InformationSource InformationSource { get; set; }


        /// <summary>
        /// Получает или задает ИД кадастрового объекта.
        /// </summary>
        public int? CadastralID { get; set; }

        /// <summary>
        /// Получает или задает кадатсровый объект.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Кадастровый объект", TabName = "[0]Кадастровая стоимость", Required = true)]       
        public virtual Cadastral Cadastral { get; set; }





        /// <summary>
        /// Инициализирует новый экземпляр класса CadastralValue.
        /// </summary>
        public CadastralValue() : base()
        {

        }
    }
}
