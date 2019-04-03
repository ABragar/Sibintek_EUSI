using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Справочник "Повышающий/понижающий коэффициент")
    /// </summary>
    [EnableFullTextSearch]
    public class BoostOrReductionFactor : DictObject
    {
        public BoostOrReductionFactor() : base()
        {

        }
        /// <summary>
        /// Значение
        /// </summary>
        [DetailView("Значение")]
        [ListView("Значение")]
        public decimal Value { get; set; }

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
