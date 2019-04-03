using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Доля в праве общей долевой собственности пропорциональна размеру общей площади помещений, машино-мест, не переданных участникам долевого строительства, а также помещений, машино-мест
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("BuilderShareWithObject")]
    public class BuilderShareWithObject
    {
        public BuilderShareWithObject()
        {
            Info_objects = new List<CadNumber>();
        }
        /// <summary>
        /// Доля в праве общей долевой собственности пропорциональна размеру общей площади помещений, машино-мест, не переданных участникам долевого строительства, а также помещений, машино-мест
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("builder_share_with_object_info")]
        public string Builder_share_with_object_info { get; set; }
        /// <summary>
        /// Сведения о помещениях, машино-местах
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("info_object", IsNullable = false, ElementName = "info_objects")]
        [XmlArray("info_objects")]
        [XmlArrayItem("info_object", Type = typeof(CadNumber), IsNullable = false)]
        public List<CadNumber> Info_objects { get; set; }
    }
}
