using Base.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Base
{
    /// <summary>
    /// Предоставляет методы и свойства историчности для обекта Системы.
    /// </summary>
    public interface IHistoryObject
    {        
        /// <summary>
        /// Получает или задает признак историчности записи.
        /// </summary>
        bool IsHistory { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия записи (актуальная дата).
        /// </summary>
        DateTime? ActualDate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия записи (дата неактуальности записи). 
        /// </summary>
        DateTime? NonActualDate { get; set; }       

        /// <summary>
        /// Метод инициации истории экземпляра объекта.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        void InitHistory(IUnitOfWork uow);
    }
}
