using Base;
using Base.Attributes;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{

    /// <summary>
    /// Представляет данные о характеристиках объекта.
    /// </summary>
    public class BaseParameter : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BaseParameter.
        /// </summary>
        public BaseParameter() : base()
        {

        }


        /// <summary>
        /// Площадь в кв. метрах
        /// </summary>
        [DetailView("Площадь в кв. метрах")]
        [ListView("Площадь в кв. метрах")]
        public decimal? Area { get; set; }
        /// <summary>
        /// Площадь застройки в квадратных метрах с округлением
        /// до 0,1 квадратного метра
        /// </summary>
        [DetailView("Площадь застройки в квадратных метрах с округлением")]
        [ListView("Площадь застройки в квадратных метрах с округлением")]
        public decimal? Built_up_area { get; set; }
        /// <summary>
        /// Протяженность в метрах с округлением до 1 метра
        /// </summary>
        [DetailView("Протяженность в метрах с округлением до 1 метра")]
        [ListView("Протяженность в метрах с округлением до 1 метра")]
        public decimal? Extension { get; set; }
        /// <summary>
        /// Глубина в метрах с округлением до 0,1 метра
        /// </summary>
        [DetailView("Глубина в метрах с округлением до 0,1 метра")]
        [ListView("Глубина в метрах с округлением до 0,1 метра")]
        public decimal? Depth { get; set; }
        /// <summary>
        /// Глубина залегания в метрах с округлением до 0,1 метра
        /// </summary>
        [DetailView("Глубина залегания в метрах с округлением до 0,1 метра")]
        [ListView("Глубина залегания в метрах с округлением до 0,1 метра")]
        public decimal? Occurence_depth { get; set; }
        /// <summary>
        /// Объем в кубических метрах с округлением до 1 кубического метра
        /// </summary>
        [DetailView("Объем в кубических метрах с округлением до 1 кубического метра")]
        [ListView("Объем в кубических метрах с округлением до 1 кубического метра")]
        public decimal? Volume { get; set; }
        /// <summary>
        /// Высота в метрах с округлением до 0,1 метра
        /// </summary>
        [DetailView("Высота в метрах с округлением до 0,1 метра")]
        [ListView("Высота в метрах с округлением до 0,1 метра")]
        public decimal? Height { get; set; }

        [SystemProperty]
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "ИД ОНИ", Visible = false)]
        public int? ObjectRecordID { get; set; }

        
        [DetailView(ReadOnly = true, Name = "ОНИ", Visible = false)]
        public ObjectRecord ObjectRecord { get; set; }


        [SystemProperty]
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true, Name = "ИД Выписки", Visible = false)]
        public int? ExtractID { get; set; }


        [DetailView(ReadOnly = true, Name = "Выписка", Visible = false)]
        public Extract Extract { get; set; }
    }
}
