using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Субъект Российской Федерации
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SubjectOfRF")]
    public class SubjectOfRF
    {

        /// <summary>
        /// Наименование субъекта Российской Федерации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public Dict Name { get; set; }
    }
}
