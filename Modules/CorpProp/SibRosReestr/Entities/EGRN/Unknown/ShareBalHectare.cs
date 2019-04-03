using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Размер доли в праве в балло-гектарах
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ShareBalHectare")]
    public class ShareBalHectare
    {

        /// <summary>
        /// Балло-гектары
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("bal_hectare")]
        public decimal Bal_hectare { get; set; }
    }
}
