using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{

    /// <summary>
    /// Представляет выписку ЕГРН на ОНИ - НЗС.
    /// </summary>
    public class ExtractNZS : ExtractObject
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса ExtractNZS.
        /// </summary>
        public ExtractNZS() : base()
        {

        }


        #region Характеристики объекта НЗС
        
        /// <summary>
        /// Степень готовности объекта незавершенного строительства в процентах
        /// </summary>
        [DetailView("Степень готовности объекта незавершенного строительства в процентах")]
        [ListView("Степень готовности объекта незавершенного строительства в процентах")]
        public string Degree { get; set; }
        

        #endregion
    }
}
