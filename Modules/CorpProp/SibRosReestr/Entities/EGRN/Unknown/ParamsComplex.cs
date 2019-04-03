using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Характеристики комплекса
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ParamsComplex")]
    public class ParamsComplex
    {

        /// <summary>
        /// Назначение
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("purpose")]
        public string Purpose { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public string Name { get; set; }
    }
}
