
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Справочник НСИ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Dict")]
    public class Dict 
    {
        public Dict() { }
        /// <summary>
        /// Код справочника НСИ
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("code")]
        public string Code { get; set; }
        /// <summary>
        /// Текстовое значение, соответствующее коду справочника НСИ
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public string Value { get; set; }
    }
}
