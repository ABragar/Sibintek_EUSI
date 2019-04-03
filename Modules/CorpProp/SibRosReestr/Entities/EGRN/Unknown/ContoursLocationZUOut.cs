using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Описание местоположения (и кадастровый номер ЗУ)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ContoursLocationZUOut")]
    public class ContoursLocationZUOut
    {

        /// <summary>
        /// Общие сведения (кадастровый номер)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("object")]
        public ObjectCadNumber Object { get; set; }
        /// <summary>
        /// Описание местоположения контура границы
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("contours_location")]
        public ContoursZUOut Contours_location { get; set; }
    }
}
