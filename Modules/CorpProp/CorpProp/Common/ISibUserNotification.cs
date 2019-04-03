using Base.DAL;
using CorpProp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Common
{
    /// <summary>
    /// Предоставляет методы для кастомной рассылки уведомлений пользователям.
    /// </summary>
    public interface ISibUserNotification
    {
        /// <summary>
        /// Отправка уведомления.
        /// </summary>
        /// <param name="uow">Сессия.</param>      
        /// <param name="ids">Идентификаторы объектов уведомления.</param>
        void SendUserNotification(IUnitOfWork uow, int[] ids);

        /// <summary>
        /// Отправка уведомления по шаблону.
        /// </summary>
        /// <param name="uow">Сессия.</param>
        /// <param name="ids">Идентификаторы объектов уведомления.</param>
        /// <param name="template">Код шаблона уведомления.</param>
        void SendNotification(IUnitOfWork uow, int[] ids, string email, string template);
    }
}
