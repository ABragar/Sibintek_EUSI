using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
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
    /// Представляет строку запроса.
    /// </summary>
    /// <remarks>
    /// Элемент запрашиваемой информации.
    /// </remarks>
    [EnableFullTextSearch]
    public class RequestRow : TypeObject, IRequestRow
    {
        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [ListView]
        [DetailView(Name = "Название", TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        ///// <summary>
        ///// Получает или задает ИД типа данных. 
        ///// </summary>         
        //public int? TypeDataID { get; set; }

        ///// <summary>
        ///// Получает или задает тип данных.
        ///// </summary>
        ////[ListView]
        //[DetailView(Name = "Тип данных", TabName = CaptionHelper.DefaultTabName, Required = true)]
        //public virtual TypeData TypeData { get; set; }

        /// <summary>
        /// Получает или задает содержание запроса.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Содержание запроса", TabName = CaptionHelper.DefaultTabName, Required = true, Visible = false)]
        public virtual RequestContent RequestContent { get; set; }
        /// <summary>
        /// Получает или задает ИД содержания запроса.
        /// </summary>
        public int RequestContentID { get; set; }
        /// <summary>
        /// Инициализирует новый экземпляр класса RequestRow.
        /// </summary>
        public RequestRow()
        {

        }
    }
}
