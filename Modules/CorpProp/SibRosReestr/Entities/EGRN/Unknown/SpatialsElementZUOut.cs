using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Элемент контура
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SpatialsElementZUOut")]
    public class SpatialsElementZUOut
    {
        public SpatialsElementZUOut()
        {
            Ordinates = new List<OrdinateZUOut>();
        }
        /// <summary>
        /// Список координат
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("ordinate", IsNullable = false, ElementName = "ordinates")]
        [System.Xml.Serialization.XmlArray("ordinates")]
        [System.Xml.Serialization.XmlArrayItem("ordinate", Type = typeof(OrdinateZUOut), IsNullable = false)]
        public List<OrdinateZUOut> Ordinates { get; set; }
    }
}
