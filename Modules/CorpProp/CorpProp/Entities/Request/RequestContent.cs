using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.Security;
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
    /// Представляет содержание запроса.
    /// </summary>
    /// <remarks>
    /// Содержание запроса (общее для всех получателей).
    /// </remarks>
    [EnableFullTextSearch]
    public class RequestContent : TypeObject, IRequest
    {

        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [ListView]
        [DetailView(Name = "Наименование", TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает текст.
        /// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Текст", TabName = CaptionHelper.DefaultTabName)]        
        public string Text { get; set; }

        //Убран за ненадобностью.
        ///// <summary>
        ///// Получает или задает общий срок ответа.
        ///// </summary>
        ////[ListView]
        //[DetailView(Name = "Общий срок ответа", TabName = CaptionHelper.DefaultTabName)]
        //public DateTime? Term { get; set; }

        ///// <summary>
        ///// Получает или задает связанные столбцы запроса.
        ///// </summary>       
        //[DetailView(Name = "Столбцы запроса", TabName = "[1]Столбцы запроса", HideLabel = true)]
        //public virtual ICollection<RequestColumn> RequestColumns { get; set; }

                

        ///// <summary>
        ///// Получает или задает связанные строки запроса.
        ///// </summary>        
        //[DetailView(Name = "Строки запроса", TabName = "[2]Строки запроса", HideLabel = true)]
        //public virtual ICollection<RequestRow> RequestRows { get; set; }

        /// <summary>
        /// Получает или задает ИД шаблона запроса.
        /// </summary>        
        public int? RequestTemplateID { get; set; }

        /// <summary>
        /// Получает или задает шаблона запроса.
        /// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Шаблон запроса", TabName = CaptionHelper.DefaultTabName)]
        public virtual RequestTemplate RequestTemplate { get; set; }

        
        /// <summary>
        /// Получает или задает автора.
        /// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Автор содержания запроса", TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SibUser Author { get; set; }

      

        /// <summary>
        /// Получает или задает связанные запросы.
        /// </summary>
        [DetailView(Name = "Запросы", TabName = "[4]Запросы", HideLabel = true)]
        public virtual ICollection<Request> Requests { get; set; }



        /// <summary>
        /// Инициализирует новый экземпляр класса RequestContent.
        /// </summary>
        public RequestContent()
        {
            //RequestColumns = new List<RequestColumn>();
            //RequestRows = new List<RequestRow>();
         
            Requests = new List<Request>();
        }
    }
}
