using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using EUSI.Entities.NSI;

namespace EUSI.Entities.Mapping
{
    /// <summary>
    /// Мэппинг кодов "Способ поступления" с "Вид объекта заявки"
    /// </summary>
    [EnableFullTextSearch]
    public class ERTypeERReceiptReason : TypeObject
    {
        /// <summary>
        /// Получает или задает ИД вида объекта заявки.
        /// </summary>
        [SystemProperty]
        public int? ERTypeID { get; set; }

        /// <summary>
        /// Получает или задает вид объекта заявки.
        /// </summary>
        [DetailView("Вид объекта заявки")]
        [FullTextSearchProperty]
        [ListView]
        public EstateRegistrationTypeNSI ERType { get; set; }

        /// <summary>
        /// Получает или задает ИД способа поступления.
        /// </summary>
        [SystemProperty]
        public int? ERReceiptReasonID { get; set; }

        /// <summary>
        /// Получает или задает способ поступления.
        /// </summary>
        [DetailView("Способ поступления")]
        [FullTextSearchProperty]
        [ListView]
        public ERReceiptReason ERReceiptReason { get; set; }
    }
}
