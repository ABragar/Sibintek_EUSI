using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Муниципальное образование
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AddressCity")]
    public class AddressCity
    {

        [System.Xml.Serialization.XmlElementAttribute("fias")]
        public string Fias { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("okato")]
        public string Okato { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("kladr")]
        public string Kladr { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("oktmo")]
        public string Oktmo { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("postal_code")]
        public string Postal_code { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("region")]
        public Dict Region { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("district")]
        public District District { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("city")]
        public City City { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("urban_district")]
        public UrbanDistrict Urban_district { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("soviet_village")]
        public SovietVillage Soviet_village { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("locality")]
        public Locality Locality { get; set; }
    }
}
