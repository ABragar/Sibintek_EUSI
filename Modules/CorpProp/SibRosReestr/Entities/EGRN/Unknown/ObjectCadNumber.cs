using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Общие сведения об объекте недвижимости (кадастровый номер)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ObjectCadNumber")]
    public class ObjectCadNumber
    {

        /// <summary>
        /// Общие сведения (кадастровый номер)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("common_data")]
        public CadNumber Common_data { get; set; }
    }
}
