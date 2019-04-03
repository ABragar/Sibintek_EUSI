using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.Entities.Complex
{
    [ComplexType]
    public class KadastrObject
    {
        public KadastrTypeEnum KadastrTypeEnum { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }        
    }
    
    [UiEnum]
    public enum KadastrTypeEnum
    {
        [UiEnumValue("Неопределен")]
        Unknown = 0,
        [UiEnumValue("Регион")]
        Region = 1,
        [UiEnumValue("Земельный участок")]
        Parcel = 2,
        [UiEnumValue("Квартал")]
        Quarter = 3,
        [UiEnumValue("Район")]
        District = 4,
        [UiEnumValue("Объект недвижимости")]
        Estate = 5
    }
}
