using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.Request
{
    /// <summary>
    /// Получает или задает список значений.
    /// </summary>
    /// <remarks>
    /// Список возможных значений.
    /// </remarks>
    public class RequestColumnItems: TypeObject
    {
        public int RequestColumnID { get; set; }
        [DetailView("Значение колонки запроса")]
        [ListView("Значение колонки запроса")]
        public string Item { get; set; }

        [DetailView("Столбец запроса", ReadOnly = true)]
        [ListView("Колонка запроса")]
        public virtual RequestColumn RequestColumn { get; set; }
    }
}
