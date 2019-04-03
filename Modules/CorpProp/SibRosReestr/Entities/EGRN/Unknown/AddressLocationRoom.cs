using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Адрес (местоположение) помещения
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AddressLocationRoom")]
    public class AddressLocationRoom
    {

        /// <summary>
        /// Адрес (местоположение) помещения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address")]
        public AddressOksLocation Address { get; set; }
        /// <summary>
        /// Номер комнаты в квартире
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("room_number")]
        public string Room_number { get; set; }
    }
}
