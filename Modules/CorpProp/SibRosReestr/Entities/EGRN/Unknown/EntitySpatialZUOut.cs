using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Описание элементов контура (характерных точек контура)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("EntitySpatialZUOut")]
    public class EntitySpatialZUOut
    {
        public EntitySpatialZUOut()
        {
            Spatials_elements = new List<SpatialsElementZUOut>();
        }

        /// <summary>
        /// Система координат
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("sk_id")]
        public string Sk_id { get; set; }
        /// <summary>
        /// Элементы контура
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("spatial_element", IsNullable = false, ElementName = "spatials_elements")]
        [System.Xml.Serialization.XmlArray("spatials_elements")]
        [System.Xml.Serialization.XmlArrayItem("spatial_element", Type = typeof(SpatialsElementZUOut), IsNullable = false)]
        public List<SpatialsElementZUOut> Spatials_elements { get; set; }
    }
}
