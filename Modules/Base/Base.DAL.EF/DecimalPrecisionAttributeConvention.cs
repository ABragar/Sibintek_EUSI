using Base.Attributes;
using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Base.DAL.EF
{
    /// <summary>
    /// Кастомный класс соглашения настройки атрибута <DecimalPrecisionAttribute> для десятичных свойств.
    /// </summary>
    public class DecimalPrecisionAttributeConvention
        : PrimitivePropertyAttributeConfigurationConvention<DecimalPrecisionAttribute>
    {
        /// <summary>
        /// Переопределяет применние данного соглашения к свойству.
        /// </summary>
        /// <param name="configuration">Конфигурация свойства.</param>
        /// <param name="attribute">Атрибут типа DecimalPrecisionAttribute.</param>
        public override void Apply(ConventionPrimitivePropertyConfiguration configuration, DecimalPrecisionAttribute attribute)
        {
            if (attribute.Precision < 1 || attribute.Precision > 38)
            {
                throw new InvalidOperationException("Precision must be between 1 and 38.");
            }

            if (attribute.Scale > attribute.Precision)
            {
                throw new InvalidOperationException("Scale must be between 0 and the Precision value.");
            }

            configuration.HasPrecision(attribute.Precision, attribute.Scale);
        }
    }
}