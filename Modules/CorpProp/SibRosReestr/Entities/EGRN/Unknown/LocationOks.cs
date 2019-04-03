using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Местоположение ОКС
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LocationOks")]
    public class LocationOks
    {

        /// <summary>
        /// OKATO
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("okato")]
        public string Okato { get; set; }
        /// <summary>
        /// OKTMO
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("oktmo")]
        public string Oktmo { get; set; }
        /// <summary>
        /// Код региона
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("region")]
        public Dict Region { get; set; }
        /// <summary>
        /// Описание местоположения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("position_description")]
        public string Position_description { get; set; }
    }
}
