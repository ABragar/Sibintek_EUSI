using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Адрес (местоположение) объекта недвижимости - сооружения, объекта незавершенного строительства, единого недвижимого комплекса
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AddressLocationConstruction")]
    public class AddressLocationConstruction
    {
        public AddressLocationConstruction()
        {
            Locations = new List<LocationCity>();
        }
        /// <summary>
        /// Тип адреса
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address_type")]
        public Dict Address_type { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address")]
        public AddressMain Address { get; set; }
        /// <summary>
        /// Наименования субъектов Российской Федерации, муниципальных образований, населенных пунктов (при наличии)
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("location", IsNullable = false, ElementName = "locations")]
        [XmlArray("locations")]
        [XmlArrayItem("location", Type = typeof(LocationCity), IsNullable = false)]
        public List<LocationCity> Locations { get; set; }
    }
}
