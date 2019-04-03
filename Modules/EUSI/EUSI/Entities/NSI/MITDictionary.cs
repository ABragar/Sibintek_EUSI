using Base.Attributes;
using CorpProp.Entities.Base;

namespace EUSI.Entities.NSI
{
    /// <summary>
    /// Справочник Минпромторга (Кп ТС)
    /// </summary>
    public class MITDictionary : DictObject
    {
        /// <summary>
        /// Марка
        /// </summary>
        [DetailView(Name = "Марка")]
        [ListView(Name = "Марка")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Brand { get; set; }

        /// <summary>
        /// Тип двигателя
        /// </summary>
        [DetailView(Name = "Тип двигателя")]
        [ListView(Name = "Тип двигателя")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EngineType { get; set; }

        /// <summary>
        /// Объем двигателя
        /// </summary>
        [DetailView(Name = "Объем двигателя")]
        [ListView(Name = "Объем двигателя")]
        [PropertyDataType(PropertyDataType.Text)]
        public string EngineCapacity { get; set; }

        /// <summary>
        /// Максимальный возраст
        /// </summary>
        [DetailView(Name = "Максимальный возраст")]
        [ListView(Name = "Максимальный возраст")]
        public int? MaxAge { get; set; }

        /// <summary>
        /// Нижняя граница диапазона (млн. руб.)
        /// </summary>
        [DetailView(Name = "Нижняя граница диапазона (млн. руб.)")]
        [ListView(Name = "Нижняя граница диапазона (млн. руб.)")]
        public decimal? LowBoundRange { get; set; }

        /// <summary>
        /// Верхняя граница диапазона (млн. руб.)
        /// </summary>
        [DetailView(Name = "Верхняя граница диапазона (млн. руб.)")]
        [ListView(Name = "Верхняя граница диапазона (млн. руб.)")]
        public decimal? UpBoundRange { get; set; }
    }
}
