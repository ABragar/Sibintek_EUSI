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
    [System.Xml.Serialization.XmlRootAttribute("CadLinksLandIncludedOld")]
    public class CadLinksLandIncludedOld
    {
        public CadLinksLandIncludedOld()
        {
            Included_objects = new List<CadNumber>();
            Old_numbers = new List<OldNumber>();
        }
        /// <summary>
        /// Кадастровые номера расположенных в пределах земельного участка объектов недвижимости
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("included_object", IsNullable = false, ElementName = "included_objects")]
        [XmlArray("included_objects")]
        [XmlArrayItem("included_object", Type = typeof(CadNumber), IsNullable = false)]
        public List<CadNumber> Included_objects { get; set; }
        /// <summary>
        /// Ранее присвоенные номера
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("old_number", IsNullable = false, ElementName = "old_numbers")]
        [XmlArray("old_numbers")]
        [XmlArrayItem("old_number", Type = typeof(OldNumber), IsNullable = false)]
        public List<OldNumber> Old_numbers { get; set; }
    }
}
