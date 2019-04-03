using Base.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.NSI;
using CorpProp.Helpers;

namespace EUSI.Entities.NSI
{
    /// <summary>
    /// Ответственный по БЕ
    /// </summary>
    public class Responsible : DictObject
    {
        /// <summary>
        /// Фамилия, Имя, Отчество
        /// </summary>
        [DetailView(Name = "ФИО", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "ФИО", Visible = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string FIO { get; set; }

        /// <summary>
        /// Номер телефона ответственного
        /// </summary>
        [DetailView(Name = "Телефон", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "Телефон", Visible = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Phone { get; set; }

        /// <summary>
        /// Email ответственного
        /// </summary>
        [DetailView(Name = "Email", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "Email", Visible = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Email { get; set; }

        /// <summary>
        /// БЕ.
        /// </summary>
        [DetailView(Name = "БЕ", Visible = true, TabName = CaptionHelper.DefaultTabName)]
        [ListView(Name = "БЕ", Visible = true)]
        public virtual Consolidation Consolidation { get; set; }

        /// <summary>
        /// Получает или задает ИД БЕ.
        /// </summary>
        [DetailView(Visible = false)]
        public int? ConsolidationID { get; set; }

        /// <summary>
        /// Получает или задает зону ответственности БЕ.
        /// </summary>
        [DetailView("Код (Зона ответсвенности)"), ListView()]       
        public virtual ZoneResponsibility ZoneResponsibility { get; set; }

        /// <summary>
        /// Получает или задает ИД зоны ответсвенности БЕ.
        /// </summary>
        [DetailView(Visible = false)]
        public int? ZoneResponsibilityID { get; set; }

    }
}
