using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Местоположение земельного участка относительно ориентира
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LocationByARefPoint")]
    public class LocationByARefPoint
    {

        /// <summary>
        /// Ориентир расположен в границах участка
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("in_boundaries_mark")]
        public bool In_boundaries_mark { get; set; }
        /// <summary>
        /// Наименование ориентира
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ref_point_name")]
        public string Ref_point_name { get; set; }
        /// <summary>
        /// Расположение относительно ориентира
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("location_description")]
        public string Location_description { get; set; }
    }
}
