using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет справочник типов для графиков государственной регистрации прав, составленный ОГ.
    /// </summary>
    /// <remarks>
    /// Cправочник: вновь вводимых объектов, ранее введённых объектов, прекращения прав.
    /// </remarks>
    [EnableFullTextSearch]
    public class ScheduleStateRegistrationType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ScheduleStateRegistrationType.
        /// </summary>
        public ScheduleStateRegistrationType()
        {

        }
    }
}
