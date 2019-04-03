using Base;
using Base.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Settings
{
    /// <summary>
    /// Представляет группу уведомлений.
    /// </summary>
    public class NotificationGroup : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NotificationGroup.
        /// </summary>
        public NotificationGroup() : base()
        {

        }

        /// <summary>
        /// Получает или задает наименование группы.
        /// </summary>
        [DetailView("Наименование"), ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }
    }
}
