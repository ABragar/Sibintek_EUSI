using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.BSC
{
    /// <summary>
    /// Данные из BSC системы.
    /// </summary>
    public class BCSData : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BCSData.
        /// </summary>
        public BCSData() : base() { }

        /// <summary>
        /// Получает или задает дату начала периода.
        /// </summary>
        [ListView]
        [DetailView("Начало периода")]
        public DateTime? StartDate { get; set; }


        /// <summary>
        /// Получает или задает дату окончания периода.
        /// </summary>
        [ListView]
        [DetailView("Окончание периода")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Получает или задает ИД БЕ.
        /// </summary>
        [SystemProperty]
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает БЕ.
        /// </summary>
        [ListView]
        [DetailView("БЕ")]
        public Consolidation Consolidation { get; set; }


        /// <summary>
        /// Получает или задает ИД группы ОС.
        /// </summary>
        [SystemProperty]
        public int? GroupConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает группу ОС.
        /// </summary>
        [ListView]
        [DetailView("Группа ОС/НМА")]
        [ForeignKey("GroupConsolidationID")]
        public PositionConsolidation GroupConsolidation { get; set; }

        /// <summary>
        /// Получает или задает ИД позиции консолидации.
        /// </summary>
        [SystemProperty]
        public int? PositionConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает позицию консолидации.
        /// </summary>
        [ListView]
        [DetailView("Позиция консолидации")]
        [ForeignKey("PositionConsolidationID")]
        public PositionConsolidation PositionConsolidation { get; set; }


        /// <summary>
        /// Получает или задает первоначальную стоимость в руб.
        /// </summary>       
        [ListView]
        [DetailView("Первоначальная стоимость по БУ, руб.")]
        [DefaultValue(0)]
        public decimal? InitialCostRSBU { get; set; }


        /// <summary>
        /// Получает или задает первоначальную стоимость в руб.
        /// </summary>       
        [ListView]
        [DetailView("Первоначальная стоимость по МСФО, руб.")]
        [DefaultValue(0)]
        public decimal? InitialCostMSFO { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость по БУ в руб.
        /// </summary>      
        [ListView]
        [DetailView("Остаточная стоимость по БУ, руб.")]
        [DefaultValue(0)]
        public decimal? ResidualCostRSBU { get; set; }

        /// <summary>
        /// Получает или задает остаточную стоимость в руб.
        /// </summary>      
        [ListView]
        [DetailView("Остаточная стоимость по МСФО, руб.")]
        [DefaultValue(0)]
        public decimal? ResidualCostMSFO { get; set; }

        /// <summary>
        /// Получает или задает начисленную амортизацию в руб.
        /// </summary>
        [ListView]
        [DetailView("Амортизация по БУ, руб.")]
        [DefaultValue(0)]
        public decimal? DepreciationCostRSBU { get; set; }

        /// <summary>
        /// Получает или задает начисленную амортизацию в руб.
        /// </summary>
        [ListView]
        [DetailView("Амортизация по МСФО, руб.")]
        [DefaultValue(0)]
        public decimal? DepreciationCostMSFO { get; set; }

        /// <summary>
        /// Получает или задает изменения первоначальной стоимости в руб. по РСБУ
        /// </summary>       
        [ListView]
        [DetailView("Первоначальная стоимость по БУ, руб. (изменения)")]
        [DefaultValue(0)]
        public decimal? DeltaInitialCostRSBU { get; set; }

        /// <summary>
        /// Получает или задает изменения первоначальной стоимости в руб. по РСБУ
        /// </summary>       
        [ListView]
        [DetailView("Первоначальная стоимость по МСФО, руб. (изменения)")]
        [DefaultValue(0)]
        public decimal? DeltaInitialCostMSFO { get; set; }

        /// <summary>
        /// Получает или задает изменения первоначальной стоимости в руб. по РСБУ
        /// </summary>       
        [ListView]
        [DetailView("Амортизация по БУ, руб. (изменения)")]
        [DefaultValue(0)]
        public decimal? DeltaDepreciationCostRSBU { get; set; }

        /// <summary>
        /// Получает или задает изменения первоначальной стоимости в руб. по РСБУ
        /// </summary>       
        [ListView]
        [DetailView("Амортизация по МСФО, руб. (изменения)")]
        [DefaultValue(0)]
        public decimal? DeltaDepreciationCostMSFO { get; set; }

    }
}
