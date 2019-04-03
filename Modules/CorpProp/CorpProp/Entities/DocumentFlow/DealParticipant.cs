using Base;
using Base.Attributes;
using SubjectObject = CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Helpers;
using CorpProp.Entities.Base;
using System.ComponentModel;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.DocumentFlow
{
    /// <summary>
    /// Представляет стороны документа.
    /// </summary>
    [EnableFullTextSearch]
    public class DealParticipant : TypeObject
    {
        /// <summary>
        /// Получает или задает роль участия.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Роль участия", Order = 0, TabName = CaptionHelper.DefaultTabName, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string RoleInDeal { get; set; }

        /// <summary>
        /// Получает или задает признак действующий.
        /// </summary>
        /// <remarks>
        /// Отражает de facto ситуацию с занесением информации о правопреемстве в договорах.
        /// </remarks>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Действующий", Order = 1, TabName = CaptionHelper.DefaultTabName)]
        [DefaultValue(false)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Получает или задает ИД документа.
        /// </summary>
        public int? SibDealID { get; set; }

        /// <summary>
        /// Получает или задает сделку.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Сделка", Order = 2, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public virtual SibDeal SibDeal { get; set; }

        /// <summary>
        /// Получает или задает ИД делового партнера.
        /// </summary>
        public int? SubjectID { get; set; }

        /// <summary>
        /// Получает или задает делового партнера.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Деловой партнер", Order = 3, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public SubjectObject.Subject Subject { get; set; }
    }
}
