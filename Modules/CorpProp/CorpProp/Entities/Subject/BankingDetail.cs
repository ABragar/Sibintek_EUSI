using Base.Attributes;
using CorpProp.Entities.Base;
using SubjectObject = CorpProp.Entities.Subject;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Subject
{
    /// <summary>
    /// Представляет банковские реквизиты
    /// </summary>
    [EnableFullTextSearch]
    public class BankingDetail : TypeObject
    {
        /// <summary>
        /// Получает или задает наименование банка.
        /// </summary>
        [DetailView(Name = "Название банка", Order = 2, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "Название банка", Order = 2)]
        [PropertyDataType(PropertyDataType.Text)]
        public string FullName { get; set; }

        /// <summary>
        /// Получает или задает Расчетный счет.
        /// </summary>
        [DetailView(Name = "Расчетный счет", Order = 3, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "Расчетный счет", Order = 3)]
        [PropertyDataType(PropertyDataType.Text)]
        public string BankAccount { get; set; }

        /// <summary>
        /// Получает или задает корреспондентский счет.
        /// </summary>
        [DetailView(Name = "Корреспондентский счет", Order = 4, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "Корреспондентский счет", Order = 4)]
        [PropertyDataType(PropertyDataType.Text)]
        public string KorBankAccount { get; set; }

        /// <summary>
        /// Получает или задает БИК.
        /// </summary>
        [DetailView(Name = "БИК", Order = 5, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "БИК", Order = 5)]
        [PropertyDataType(PropertyDataType.Text)]
        public string BIK { get; set; }

        /// <summary>
        /// Получает или задает дату начала действия.
        /// </summary>

        [DetailView(Name = "Действует с", Order = 6, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "Действует с", Order = 6)]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия.
        /// </summary>
        [DetailView(Name = "Действует по", Order = 7, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Name = "Действует по", Order = 7)]
        public DateTime? DateTo { get; set; }


        /// <summary>
        /// Получает или задает ИД делового партнера.
        /// </summary>
        public int? SubjectID { get; set; }


        /// <summary>
        /// Получает или задает делового партнера.
        /// </summary>
        [DetailView(Name = "Деловой партнер", Order = 0, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "Деловой партнер", Order = 0, Hidden = true)]
        public SubjectObject.Subject Subject { get; set; }

    }
}
