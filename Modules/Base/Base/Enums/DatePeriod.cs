using Base.Attributes;

namespace Base.Enums
{
    [UiEnum]
    public enum DatePeriod
    {
        [UiEnumValue("День")]
        Day,
        [UiEnumValue("Неделя")]
        Week,
        [UiEnumValue("Месяц")]
        Month,
        [UiEnumValue("Квартал")]
        Quarter,
        [UiEnumValue("Год")]
        Year
    }
}
