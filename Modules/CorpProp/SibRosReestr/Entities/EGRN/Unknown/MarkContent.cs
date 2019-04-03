
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Содержание отметки
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("MarkContent")]
    public class MarkContent 
    {
        public MarkContent() { }
        /// <summary>
        /// Содержание отметки при возникновении
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("origin_mark")]
        public OriginContent Origin_mark { get; set; }
    }
}
