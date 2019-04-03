using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Уведомление об отсутствии сведений
    /// </summary>
    public class NoticeSubj : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NoticeSubj.
        /// </summary>
        public NoticeSubj() : base()
        {

        }

        /// <summary>
        /// Вид запрошенной информации
        /// </summary>        
        public string TypeInfoText { get; set; }

        public string Item { get; set; }

        /// <summary>
        /// информация о недееспособности из КУА
        /// </summary>       
        public string ArrestInfo { get; set; }

        public int? NoticeID { get; set; }

        public virtual Notice Notice { get;set;}
    }
}
