using Base.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Справочник "Объекты с признаком высокой энергетической эффективности".
    /// </summary>
    public class HighEnergyEfficientFacility : DictObject
    {
        [DetailView(Name = "Код ОКОФ2")]
        [ListView]
        public string CodeOKOF2 { get; set; }

        [DetailView(Name = "Наименование класса ОКОФ2")]
        [ListView]
        public string ClassNameOKOF2 { get; set; }

        [DetailView(Name = "Качественная характеристика объекта")]
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string QualityCharacteristic { get; set; }
    }
}
