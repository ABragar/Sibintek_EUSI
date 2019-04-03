using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Request
{
    /// <summary>
    /// Представляет шаблон запроса.
    /// </summary>
    [Table("RequestTemplate")]
    public class RequestTemplate : RequestContent
    {
        /// <summary>
        /// Получает или задает связанные содержания запросов.
        /// </summary>
        public virtual ICollection<RequestContent> RequestContents { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класа RequestTemplate.
        /// </summary>
        public RequestTemplate() : base ()
        {
            RequestContents = new List<RequestContent>();
        }
    }
}
