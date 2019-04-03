using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Отказ в выдаче сведений.
    /// </summary>
    public class Refusal : BaseObject
    {
        /// <summary>
        /// Инициализирует  новый экземпляр класса Refusal.
        /// </summary>
        public Refusal() : base()
        {
            RefusalSubjs = new List<RefusalSubj>();
        }

        public ICollection<RefusalSubj> RefusalSubjs { get; set; }

        /// <summary>
        /// Вид запрошенной информации
        /// </summary>       
        public string TypeInfoText { get; set; }

       /// <summary>
       /// Запрошенный субъект.
       /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// текст отказа
        /// </summary>       
        public string TextRefusal { get; set; }
        /// <summary>
        /// тип отказа
        /// </summary>       
        public string RefusalType { get; set; }

        /// <summary>
        /// текст типа отказа
        /// </summary>       
        public string RefusalTypeText { get; set; }
    }
}
