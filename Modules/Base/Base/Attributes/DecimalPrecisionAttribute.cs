using System;

namespace Base.Attributes
{
    /// <summary>
    /// Представляет атрибут для указания десятичному свойству точности и масштаба.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DecimalPrecisionAttribute : Attribute
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DecimalPrecisionAttribute.
        /// </summary>
        /// <param name="precision">Точность.</param>
        /// <param name="scale">Масштаб.</param>
        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
        }

        /// <summary>
        /// Получает или задает точность.
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// Получает или задает масштаб.
        /// </summary>
        public byte Scale { get; set; }
    }
}