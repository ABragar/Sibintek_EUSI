using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Описание местоположения контуров границы земельного участка
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ContoursZUOut")]
    public class ContoursZUOut
    {
        public ContoursZUOut()
        {
            Contours = new List<ContourZUOut>();
        }
        /// <summary>
        /// Контуры границы
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("contour", IsNullable = false, ElementName = "contours")]
        [System.Xml.Serialization.XmlArray("contours")]
        [System.Xml.Serialization.XmlArrayItem("contour", Type = typeof(ContourZUOut), IsNullable = false)]
        public List<ContourZUOut> Contours { get; set; }
    }
}
