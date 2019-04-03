using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Уведомление об отсутствии сведений о субъекте
    /// </summary>
    public class Notice : BaseObject
    {
        public Notice() : base()
        {
            NoticeSubjs = new List<NoticeSubj>();
        }
        /// <summary>
        /// текст уведомления
        /// </summary>       
        public ICollection<NoticeSubj> NoticeSubjs { get; set; }

        /// <summary>
        /// Вид запрошенной информации
        /// </summary>        
        public string TypeInfoText { get; set; }

        public string Item { get; set; }

        /// <summary>
        /// информация о недееспособности из КУА
        /// </summary>       
        public string ArrestInfo { get; set; }

        
    }
}
