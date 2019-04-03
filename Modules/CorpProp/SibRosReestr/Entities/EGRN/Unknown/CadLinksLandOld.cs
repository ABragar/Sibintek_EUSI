using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объектах (связь с кадастровыми номерами)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CadLinksLandOld")]
    public class CadLinksLandOld
    {
        public CadLinksLandOld()
        {
            Land_cad_numbers = new List<CadNumber>();
            Old_numbers = new List<OldNumber>();
        }

        /// <summary>
        /// Кадастровые номера иных объектов недвижимости (земельных участков), в пределах которых расположен объект недвижимости
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("land_cad_number", IsNullable = false, ElementName = "land_cad_numbers")]
        [XmlArray("land_cad_numbers")]
        [XmlArrayItem("land_cad_number", Type = typeof(CadNumber), IsNullable = false)]
        public List<CadNumber> Land_cad_numbers { get; set; }
        /// <summary>
        /// Ранее присвоенные номера
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("old_number", IsNullable = false, ElementName = "old_numbers")]
        [XmlArray("old_numbers")]
        [XmlArrayItem("old_number", Type = typeof(OldNumber), IsNullable = false)]
        public List<OldNumber> Old_numbers { get; set; }
    }
}
