using Base.Attributes;
using CorpProp.Entities.Base;
using SubjectObject = CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Helpers;
using CorpProp.Entities.Document;
using Base.Utils.Common.Attributes;
using CorpProp.Attributes;
using CorpProp.Entities.Security;

namespace CorpProp.Entities.Law
{
    /// <summary>
    /// Представляет график государственной регистрации прекращения права.
    /// </summary>
    /// <remarks>
    /// График государственной регистрации прекращения прав, составленный ОГ.
    /// </remarks>
    [EnableFullTextSearch]
    public class ScheduleStateTerminate : TypeObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ScheduleStateTerminate.
        /// </summary>
        public ScheduleStateTerminate()
        {
        }

        /// <summary>
        /// Получает или задает исполнителя.
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Наименование", Order = 0, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        public int? ScheduleStateYearID { get; set; }

        [DetailView(Name = "ГГР на год", Order = 0, TabName = CaptionHelper.DefaultTabName)]
        public virtual ScheduleStateYear ScheduleStateYear { get; set; }

        /// <summary>
        /// Получает или задает год графика.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Год", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType("Sib_Year")]
        public int? Year { get; set; }

        /// <summary>
        /// Получает или задает ИД общества группы.
        /// </summary>
        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает общество группы.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Общество группы", Order = 2, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SubjectObject.Society Society { get; set; }

        /// <summary>
        /// Получает или задает сотрудника загрузившего данные.
        /// </summary>
        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Сотрудник загрузивший данные", Order = 3, TabName = CaptionHelper.DefaultTabName)]
        public virtual SibUser EmployeeUploadedData { get; set; }

        /// <summary>
        /// Получает или задает исполнителя.
        /// </summary>
        [ListView]
        [DetailView(Name = "ФИО исполнителя", Order = 4, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string Executor { get; set; }

        /// <summary>
        /// Получает или задает контактный телефон исполнителя.
        /// </summary>
        [ListView]
        [DetailView(Name = "Контактный тел. исполнителя", Order = 5, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorPhone { get; set; }

        /// <summary>
        /// Получает или задает e-mail исполнителя.
        /// </summary>
        [ListView]
        [DetailView(Name = "Эл. адрес исполнителя", Order = 6, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string ExecutorEmail { get; set; }


        /// <summary>
        /// Получает или задает e-mail ОГ.
        /// </summary>
        [ListView]
        [DetailView(Name = "Эл. адрес Общества", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        public string SocietyEmail { get; set; }

        /// <summary>
        /// Получает или задает дату графика.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Дата направления", Order = 8, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateSchedule { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса графика.
        /// </summary>
        public int? ScheduleStateRegistrationStatusID { get; set; }

        /// <summary>
        /// Получает или задает статус графика.
        /// </summary>
        
        [FullTextSearchProperty]
        [ListView]
        [DetailView("Статус", Order = 9, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual ScheduleStateRegistrationStatus ScheduleStateRegistrationStatus { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [ListView]
        [DetailView(Name = "Примечание к статусу", Order = 10, TabName = CaptionHelper.DefaultTabName)]
        [FullTextSearchProperty]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает ИД карточки документа.
        /// </summary>
        public int? FileCardID { get; set; }

        /// <summary>
        /// Получает или задает карточку документа.
        /// </summary>
        /// <remarks>
        /// Подтверждающий документ.
        /// </remarks>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Подтверждающий документ", Order = 11, TabName = CaptionHelper.DefaultTabName)]
        public virtual FileCard FileCard { get; set; }
    }
}
