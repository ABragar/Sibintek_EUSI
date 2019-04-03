using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Request
{
    /// <summary>
    /// Представляет столбец запроса.
    /// </summary>
    [EnableFullTextSearch]
    public class RequestColumn : TypeObject, IRequestColumn
    {

        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [ListView]
        [DetailView(Name = "Название", TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает ИД типа данных. 
        /// </summary>         
        public int? TypeDataID { get; set; }

        /// <summary>
        /// Получает или задает тип данных.
        /// </summary>
        [ListView]
        [DetailView(Name = "Тип данных", TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual TypeData TypeData { get; set; }

        /// <summary>
        /// Получает или задает мин. длину текстового поля.
        /// </summary>
        [ListView]
        [DetailView(Name = "Мин. длина текстового поля", TabName = CaptionHelper.DefaultTabName)]        
        public int? MinLength { get; set; }

        /// <summary>
        /// Получает или задает макс. длину текстового поля.
        /// </summary>
        [ListView]
        [DetailView(Name = "Макс. длина текстового поля", TabName = CaptionHelper.DefaultTabName)]       
        public int? MaxLength { get; set; }


        /// <summary>
        /// Получает или задает мин. значение числового поля.
        /// </summary>
        [ListView]
        [DetailView(Name = "Мин. значение числового поля", TabName = CaptionHelper.DefaultTabName)]
        public int? MinValue { get; set; }

        /// <summary>
        /// Получает или задает макс. значение числового поля.
        /// </summary>
        [ListView]
        [DetailView(Name = "Макс. значение числового поля", TabName = CaptionHelper.DefaultTabName)]
        public int? MaxValue { get; set; }


        /// <summary>
        /// Получает или задает мин. дату.
        /// </summary>
        [ListView]
        [DetailView(Name = "Мин. дата", TabName = CaptionHelper.DefaultTabName)]
        public DateTime? MinDate { get; set; }

        /// <summary>
        /// Получает или задает макс. дату.
        /// </summary>
        [ListView]
        [DetailView(Name = "Макс. дата", TabName = CaptionHelper.DefaultTabName)]
        public DateTime? MaxDate { get; set; }


        /// <summary>
        /// Получает или задает список значений.
        /// </summary>
        /// <remarks>
        /// Список возможных значений, разделённых точками с запятой.
        /// </remarks>
        [ListView(Hidden = true)]
        [DetailView(Name = "Содержит список значений", TabName = CaptionHelper.DefaultTabName)]
        public bool HasItems { get; set; }


        /// <summary>
        /// Получает или задает признак обязательности заполнения.
        /// </summary>  
        [ListView(Hidden = true)]
        [DetailView(Name = "Обязателен к заполнению", TabName = CaptionHelper.DefaultTabName)]
        [DefaultValue(false)]
        public bool Required { get; set; }

        /// <summary>
        /// Получает или задает инструкцию по заполнению.
        /// </summary>
        [ListView(Hidden = true)]        
        [DetailView(Name = "Инструкция по заполнению", TabName = CaptionHelper.DefaultTabName)]       
        public string Instruction { get; set; }

        /// <summary>
        /// Получает или задает ИД содержания запроса.
        /// </summary>
        public int RequestContentID { get; set; }

        /// <summary>
        /// Получает или задает содержание запроса.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Содержание запроса", TabName = CaptionHelper.DefaultTabName, Required = true, Visible = false)]
        public virtual RequestContent RequestContent { get; set; }


        ///// <summary>
        ///// Получает или задает связанные ячейки ответов.
        ///// </summary>
        //[DetailView(Name = "Ячейки ответов", TabName = "[2]Ячейки ответов", HideLabel = true)]
        //public virtual ICollection<ResponseCell> ResponseCells { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса RequestColumn.
        /// </summary>
        public RequestColumn()
        {
            //ResponseCells = new List<ResponseCell>();
        }
    }
}
