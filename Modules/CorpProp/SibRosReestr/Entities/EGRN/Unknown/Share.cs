using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Размер доли в праве
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Share")]
    public class Share
    {

        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", ElementName = "numerator")]
        public string Numerator { get; set; }
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", ElementName = "denominator")]
        public string Denominator { get; set; }
    }
}
