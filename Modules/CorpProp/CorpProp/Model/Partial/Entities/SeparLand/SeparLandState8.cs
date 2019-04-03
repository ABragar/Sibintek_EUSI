using Base;
using Base.Attributes;
using CorpProp.Entities.NSI;
using System;

namespace CorpProp.Model.Partial.Entities.SeparLand
{
    public class SeparLandState8 : BaseObject, IChildrenModel
    {
        [SystemProperty]
        public int? ParentID { get; set; }

        [SystemProperty]
        public int? AccountingStatusID { get; set; }

        /// <summary>
        /// Получает или задает статус.
        /// </summary> 
        [DetailView(Visible = false)]
        public AccountingStatus Status { get; set; }

        /// <summary>
        /// Получает или задает дату начала.
        /// </summary>       
        [DetailView(Visible = false)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания.
        /// </summary>       
        [DetailView(Visible = false)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Получает или задает реквизиты договора.
        /// </summary>        
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string DealProps { get; set; }

        /// <summary>
        /// Контрагент по договору.
        /// </summary>       
        [PropertyDataType(PropertyDataType.Text)]
        [DetailView(Visible = false)]
        public string SubjectName { get; set; }
    }
}
