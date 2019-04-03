using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник стадий готовности материального объекта.
    /// </summary>
    /// <remarks>
    /// Внутренний справочник: временный (запланирован к строительству/строится, но не поставлен на учёт); 
    /// НЗС; основное средство; выбывшее основное средство.
    /// </remarks>
    [EnableFullTextSearch]
    public class StageOfCompletion : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса StageOfCompletion.
        /// </summary>
        public StageOfCompletion()
        {

        }
    }
}
