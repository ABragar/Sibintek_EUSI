using Base;
using Base.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CorpProp.Entities.Base;
using CorpProp.Entities.ProjectActivity;

namespace CorpProp.Entities.DocumentFlow
{
    /// <summary>
    /// Связь регистрационной карточки документа и задачи
    /// </summary>
    [EnableFullTextSearch]
    public class DocTask : TypeObject
    {
        /// <summary>
        /// Идентификатор документа
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [ForeignKey("Doc")]
        public int? DocID { get; set; }

        /// <summary>
        /// Документ
        /// </summary>        
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Документ", Order = 0)]
        public virtual Doc Doc { get; set; }

        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(Visible = false)]
        [ForeignKey("Task")]
        public int? TaskID { get; set; }
        
        /// <summary>
        /// Задача
        /// </summary>       
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Задача", Order = 1)]
        public virtual SibTask Task { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса DocTask.
        /// </summary>
        public DocTask()
        {

        }
    }
}
