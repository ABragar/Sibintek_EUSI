using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Городской район
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("UrbanDistrict")]
    public class UrbanDistrict
    {

        /// <summary>
        /// Тип
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("type_urban_district")]
        public string Type_urban_district { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name_urban_district")]
        public string Name_urban_district { get; set; }
    }
}
