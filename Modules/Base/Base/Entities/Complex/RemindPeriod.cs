using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;

namespace Base.Entities.Complex
{
    /// <summary>
    /// Значаение через которое пользователь получит напоминание
    /// </summary>
    [ComplexType]
    public class RemindPeriod
    {
        /// <summary>
        /// Значение в указаном типе
        /// </summary>
        [SystemProperty]
        public int? RemindValue { get; set; }

        /// <summary>
        /// Тип отсчета(дней, часов, недель)
        /// </summary>
        [SystemProperty]
        public RemindValueType RemindValueType { get; set; } = RemindValueType.Day;
    }

    [UiEnum]
    public enum RemindValueType
    {
        [UiEnumValue("Часов")]
        Hour = 0,
        [UiEnumValue("Дней")]
        Day = 1,        
        [UiEnumValue("Недель")]
        Week = 2
    }

}
