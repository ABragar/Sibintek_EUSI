using Base.Attributes;
using Newtonsoft.Json;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Base;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Document;

namespace CorpProp.Entities.Request
{
    /// <summary>
    /// Представляет запрос информации.
    /// </summary>
    [EnableFullTextSearch]
    public class Request : TypeObject
    {
        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Название", TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает индивидуальный текст запроса.
        /// </summary>
        /// <remarks>
        /// Добавляется к общему или заменяет его.
        /// </remarks>
        [ListView(Hidden = true)]
        //[DetailView(Name = "Индивидуальный текст запроса", TabName = CaptionHelper.DefaultTabName)]
        [PropertyDataType(PropertyDataType.Text)]
        public string UniqueText { get; set; }

        // Неактуально
        ///// <summary>
        ///// Получает или задает признак замены общего текста.
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Заменять общий текст", TabName = CaptionHelper.DefaultTabName)]
        //[DefaultValue(false)]
        //public bool ReplaceText { get; set; }

        /// <summary>
        /// Получает или задает индивидуальный срок ответа.
        /// </summary>
        [ListView]
        [DetailView(Name = "Общий срок ответа", TabName = CaptionHelper.DefaultTabName, ReadOnly = false)]
        public DateTime? UniqueTerm { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса запроса.
        /// </summary>
        public int? RequestStatusID { get; set; }

        /// <summary>
        /// Получает или задает статус запроса.
        /// </summary>
        [ListView]
        [DetailView(Name = "Статус запроса", TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual RequestStatus RequestStatus { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Примечание", TabName = CaptionHelper.DefaultTabName)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает ИД содержания запроса.
        /// </summary>
        public int? RequestContentID { get; set; }

        /// <summary>
        /// Получает или задает содержание запроса.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Содержание запроса", TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual RequestContent RequestContent { get; set; }

        /// <summary>
        /// Получает или задает ИД родительского запроса.
        /// </summary>
        [JsonIgnore]
        public int? ParentID { get; set; }

        /// <summary>
        /// Получает или задает родительский запрос.
        /// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(Name = "Родительский запрос", TabName = CaptionHelper.DefaultTabName)]
        public virtual Request Parent { get; set; }


        /// <summary>
        /// Получает или задает связанные дочерние запросы (повторные отправки).
        /// </summary>
        //[DetailView(Name = "Дочерние запросы", TabName = "[2]Повторная отправка", HideLabel = true)]
        public virtual ICollection<Request> Children { get; set; }

        public int? ResponsibleID { get; set; }

        /// <summary>
        /// Получает или задает автора запроса.
        /// </summary>
        [DetailView(Name = "Автор запроса", TabName = CaptionHelper.DefaultTabName, Required = true)]        
        public virtual SibUser Responsible { get; set; }


        [ListView(Visible = false)]
        [DetailView(Name = "Дата официального запроса", TabName ="[2]Документы")]       
        public DateTime? RequestDate { get; set; }

        [ListView(Visible = false)]
        [DetailView(Name = "Номер официального запроса", TabName = "[2]Документы")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RequestNumber { get; set; }

        [SystemProperty]
        public int? RequestFileID { get; set; }

        [ListView(Visible = false)]
        [DetailView(Name = "Копия официального запроса", TabName = "[2]Документы")]  
        [ForeignKey("RequestFileID")]
        public FileCard RequestFile { get; set; }


        /// <summary>
        /// Инициализирует новый экземпляр класса Request.
        /// </summary>
        public Request()
        {
            Children = new List<Request>();
            //Executors = new List<SibUser>();
            //Responses = new List<Response>();
        }
    }

    public class RequestAndSibUserManyToManyAssociation: ManyToManyAssociation<Request, SibUser>
    {

    }
}
