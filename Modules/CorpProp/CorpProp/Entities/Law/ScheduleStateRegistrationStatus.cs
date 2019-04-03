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
    /// Представляет справочник статусов для графиков государственной регистрации прав, составленный ОГ.
    /// </summary>
    /// <remarks>
    /// Справочник: предложен ОГ, согласован ДС, в процессе выполнения, полностью выполнен - не только эти статусы
    /// </remarks>
    [EnableFullTextSearch]
    public class ScheduleStateRegistrationStatus : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ScheduleStateRegistrationStatus.
        /// </summary>
        public ScheduleStateRegistrationStatus()
        {

        }
    }
}
