using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Law;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ProjectActivity
{
    /// <summary>
    /// Представляет отчет по задаче.
    /// </summary>
    [EnableFullTextSearch]
    public class SibTaskReport : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibTaskReport.
        /// </summary>
        public SibTaskReport()
        {
        }

        /// <summary>
        /// Получает или задает дату отчёта.
        /// </summary>
        [DetailView(Name = "Дата отчёта", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DateReport { get; set; }

        /// <summary>
        /// Получает или задает текст отчёта
        /// </summary>
        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Текст отчёта", Order = 2, TabName = CaptionHelper.DefaultTabName)]
        public string TextReport { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса отчета
        /// </summary>
        public int? StatusID { get; set; }

        /// <summary>
        /// Получает или задает статус отчета.
        /// </summary>       
        [ListView]
        [DetailView(Name = "Статус отчета", Order = 3, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SibTaskReportStatus Status { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса отчета
        /// </summary>
        public int? TaskID { get; set; }

        /// <summary>
        /// Получает или задает задачу.
        /// </summary>       
        [ListView]
        [DetailView(Name = "Задача", Order = 4, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SibTask Task { get; set; }
    }
}
