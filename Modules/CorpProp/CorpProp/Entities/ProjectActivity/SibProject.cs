using Base;
using Base.Attributes;
using Base.Task.Entities;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ProjectActivity
{
    /// <summary>
    /// Представляет проект.
    /// </summary>
    [EnableFullTextSearch]
    public class SibProject : BaseTaskCategory
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibProject.
        /// </summary>
        public SibProject()
        {
        }

        /// <summary>
        /// Получает или задает номер проекта.
        /// </summary>
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Name = "Номер проекта", Order = 2)]
        [DetailView(Order = 2, Name = "Номер проекта", TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public string ProjectNumber { get; set; } 

        /// <summary>
        /// Получает или задает дату начала проекта.
        /// </summary>
        [DetailView(Name = "Дата начала проекта", Order =3, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания проекта.
        /// </summary>
        [DetailView(Name = "Дата окончания проекта", Order = 4, TabName = CaptionHelper.DefaultTabName)]
        public DateTime? DateTo { get; set; }

        public int? SibProjectTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип проекта
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Name = "Тип проекта", Order = 5, TabName = CaptionHelper.DefaultTabName)]
        public virtual SibProjectType SibProjectType { get; set; }

        /// <summary>
        /// Получает или задает ИД статуса проекта
        /// </summary>
        public int? StatusID { get; set; }

        /// <summary>
        /// Получает или задает статус проекта.
        /// </summary>       
        [ListView]
        [DetailView(Name = "Статус проекта", Order = 6, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SibProjectStatus Status { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView(Name = "Примечание", Order = 7, TabName = CaptionHelper.DefaultTabName)]
        public string Description { get; set; }

        /// <summary>
        /// Получает или задает признак шаблона проекта.
        /// </summary>
        [SystemProperty]
        [DefaultValue(false)]
        public bool IsTemplate { get; set; }

        /// <summary>
        /// Получает или задает ИД шаблон проекта.
        /// </summary>
        public int? TemplateID { get; set; }

        /// <summary>
        /// Получает или задает шаблон проекта.
        /// </summary>
        [DetailView(Name = "Шаблон проекта", Order = 9, TabName = CaptionHelper.DefaultTabName, Visible = false)]
        public virtual SibProject Template { get; set; }

        /// <summary>
        /// Получает или задает ИД пользователя.
        /// </summary>
        public int? InitiatorID { get; set; }

        /// <summary>
        /// Получает или задает пользователя.
        /// </summary>
        /// <remarks>
        /// Инициатор проекта.
        /// </remarks>
        [DetailView(Name = "Инициатор", Order = 10, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public virtual SibUser Initiator { get; set; }
    }
}
