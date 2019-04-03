using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объектах (связь с кадастровыми номерами (помещение))
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CadLinksRoom")]
    public class CadLinksRoom
    {
        public CadLinksRoom()
        {
            Old_numbers = new List<OldNumber>();

        }
        /// <summary>
        /// Кадастровый номер объекта недвижимости (здания, сооружения), в пределах которого расположено помещение
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("parent_cad_number")]
        public CadNumber Parent_cad_number { get; set; }
        /// <summary>
        /// Ранее присвоенные номера
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("old_number", IsNullable = false, ElementName = "old_numbers")]
        [XmlArray("old_numbers")]
        [XmlArrayItem("old_number", Type = typeof(OldNumber), IsNullable = false)]
        public List<OldNumber> Old_numbers { get; set; }
        /// <summary>
        /// Кадастровый номер квартиры, в котором расположена комната
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("parent_room_cad_number")]      
        public CadNumber Parent_room_cad_number { get; set; }
    }
}
