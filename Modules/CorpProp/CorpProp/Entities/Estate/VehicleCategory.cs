using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;
using Base.Attributes;
using CorpProp.Entities.NSI;

namespace CorpProp.Entities.Estate
{

    /// <summary>
    /// Представляет справочник категорий траспортных средств.
    /// </summary>
    [EnableFullTextSearch]
    public class VehicleCategory : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса VehicleCategory.
        /// </summary>
        public VehicleCategory()
        {

        }

        /// <summary>
        /// Получает или задает ИД Классa ТС.
        /// </summary>
        public int? VehicleLabelID { get; set; }

        /// <summary>
        /// Получает или задает Класс ТС.
        /// </summary>
        [ListView("Класс ТС")]
        [DetailView("Класс ТС")]
        public VehicleLabel VehicleLabel { get; set; }
    }
}
