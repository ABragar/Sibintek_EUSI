using Base;
using Base.Attributes;
using Base.Translations;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Law;
using CorpProp.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    /// <summary>
    /// История миграции данных.
    /// </summary>
    [EnableFullTextSearch]
    public class MigrateHistory : BaseObject
    {
        
        /// <summary>
        /// Инициализирует новый экземпляр класса MigrateHistory.
        /// </summary>   
        public MigrateHistory()
        {
            MigrateDate = DateTime.Now;
        }
               
       

        /// <summary>
        /// Получает или задает дату/время миграции.
        /// </summary>             
        [PropertyDataType(PropertyDataType.DateTime)]
        [DetailView("Дата/Время")]
        [ListView]
        public DateTime? MigrateDate { get; set; }
        

        /// <summary>
        /// Получает или задает ИД профиля пользователя.
        /// </summary>
        [SystemProperty]
        public int? SibUserID { get; set; }

        /// <summary>
        /// Получает или задает профиль пользователя.
        /// </summary>
        [SystemProperty]
        [DetailView("Пользователь")]
        [ListView]
        public virtual SibUser SibUser { get; set; }
               
        /// <summary>
        /// Получает или задает ИД выписки.
        /// </summary>
        [SystemProperty]
        public int? ExtractID { get; set; }

        /// <summary>
        /// Получает или задает выписку.
        /// </summary>
        [DetailView("Выписка")]
        [ListView]
        public virtual Extract Extract { get; set; }

        /// <summary>
        /// Получает или задает текст результата миграции.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView("Результат")]
        [ListView]
        public string ResultText { get; set; }

    }
}
