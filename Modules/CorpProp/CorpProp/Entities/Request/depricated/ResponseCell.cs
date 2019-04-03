//using Base.Attributes;
//using CorpProp.Entities.Base;
//using CorpProp.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CorpProp.Entities.Request
//{
    ///// <summary>
    ///// Представляет ячейку - значение конкретного столбца запроса в конкретном ответе.
    ///// </summary>
    //    [EnableFullTextSearch]
//public class ResponseCell : TypeObject
    //{

    //    ///// <summary>
    //    ///// Получает илии задает значение.
    //    ///// </summary>
    //    ///// <remarks>
    //    ///// Подчиняется ограничениям на тип и формат данных, заданным в соответствующем столбце запроса.
    //    ///// </remarks>
    //    //[ListView()]
    //    //[DetailView(Name = "Значение", TabName = CaptionHelper.DefaultTabName, Visible = false)]
    //    //public object Value { get; set; }

    //    //TODO: отображать знаечние ячейки ответа как объект.
    //    /// <summary>
    //    /// Получает илии задает значение.
    //    /// </summary>
    //    /// <remarks>
    //    /// Подчиняется ограничениям на тип и формат данных, заданным в соответствующем столбце запроса.
    //    /// </remarks>
    //    [ListView()]
    //    [DetailView(Name = "Значение", TabName = CaptionHelper.DefaultTabName)]
    //    public string StrValue { get; set; }

    //    /// <summary>
    //    /// Получает или задает ИД строки ответа.
    //    /// </summary>
    //    public int? ResponseRowID { get; set; }


    //    /// <summary>
    //    /// Получает или задает строку ответа.
    //    /// </summary>
    //    [ListView(Hidden = true)]
    //    [DetailView(Name = "Строка ответа", TabName = CaptionHelper.DefaultTabName, Required = true)]
    //    public virtual ResponseRow ResponseRow { get; set; }

    //    /// <summary>
    //    /// Получает или задает ИД столбца запроса.
    //    /// </summary>
    //    public int? RequestColumnID { get; set; }

    //    /// <summary>
    //    /// Получает или задает столбец запроса.
    //    /// </summary>
    //    [ListView(Hidden = true)]
    //    [DetailView(Name = "Столбец запроса", TabName = CaptionHelper.DefaultTabName, Required = true)]
    //    public virtual RequestColumn RequestColumn { get; set; }

    //    /// <summary>
    //    /// Инициализирует новы йэкземпляр класса ResponseCell.
    //    /// </summary>
    //    public ResponseCell()
    //    {

    //    }
    //}
//}
