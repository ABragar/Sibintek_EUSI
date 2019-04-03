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
    /// Справочник "Объекты с признаком высокой энергетической эффективности (количественный показатель)".
    /// </summary>
    public class HighEnergyEfficientFacilityKP : DictObject
    {
        [DetailView(Name = "Код ОКОФ2")]
        [ListView]
        public string CodeOKOF2 { get; set; }

        [DetailView(Name = "Наименование класса ОКОФ2")]
        [ListView]
        public string ClassNameOKOF2 { get; set; }

        [DetailView(Name = "Существенные характеристики объекта")] 
        [ListView]
        [PropertyDataType(PropertyDataType.Text)]
        public string EssentialCharacteristics { get; set; }

        [DetailView(Name = "КП наименование")]
        [ListView]
        public string KPName { get; set; }

        [DetailView(Name = "КП единица измерения")]
        [ListView]
        public string KPUnit { get; set; }

        [DetailView(Name = "КП значение")]
        [ListView]
        public string KPValue { get; set; }
    }
}
