using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Разрешенное использование земельного участка
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AllowedUse")]
    public class AllowedUse
    {

        /// <summary>
        /// Установленное разрешенное использование земельного участка
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("permitted_use_established")]
        public PermittedUseEstablished Permitted_use_established { get; set; }
    }
}
