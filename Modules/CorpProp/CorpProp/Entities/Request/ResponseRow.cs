using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Request
{

    /// <summary>
    /// Представляет запись в ответе, содержащая значения по всем столбцам запроса.
    /// </summary>
    [EnableFullTextSearch]
    public class ResponseRow : TypeObject
    {
        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Примечание", TabName = CaptionHelper.DefaultTabName)]
        public string Description { get; set; }


        /// <summary>
        /// Получает или задает ИД ответа.
        /// </summary>
        public int? ResponseID { get; set; }

        /// <summary>
        /// Получает или задает ответ.
        /// </summary>
        [ListView]
        [DetailView(Name = "Ответ", TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual Response Response { get; set; }



        ///// <summary>
        ///// Получает или задает ячейки ответа.
        ///// </summary>
        //[DetailView(Name = "Ячейки ответа", TabName = "[1]Ячейки ответа", HideLabel = true)]
        //public virtual ICollection<ResponseCell> ResponseCells { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса ResponseRow.
        /// </summary>
        public ResponseRow()
        {
            //ResponseCells = new List<ResponseCell>();
        }
    }
}
