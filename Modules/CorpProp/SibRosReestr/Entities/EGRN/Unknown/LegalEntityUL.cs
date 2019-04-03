using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Юридическое лицо (российское, иностранное  юридическое лицо)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LegalEntityUL")]
    public class LegalEntityUL
    {

        /// <summary>
        /// Юридическое лицо (российское, иностранное  юридическое лицо)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("entity")]
        public EntityUL Entity { get; set; }
    }
}
