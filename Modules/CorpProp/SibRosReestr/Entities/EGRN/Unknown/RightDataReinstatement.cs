using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Общие сведения о правах (в том числе восстановление прав)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RightDataReinstatement")]
    public class RightDataReinstatement
    {

        /// <summary>
        /// Вид зарегистрированного вещного права
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("right_type")]
        public Dict Right_type { get; set; }
        /// <summary>
        /// Номер регистрации вещного права
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("right_number")]
        public string Right_number { get; set; }
        /// <summary>
        /// Размер доли в праве
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("shares")]
        public Shares Shares { get; set; }
        /// <summary>
        /// Описание доли текстом
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("share_description")]
        public string Share_description { get; set; }
        /// <summary>
        /// Восстановление права на основании судебного акта
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("reinstatement")]
        public ReinstatementDate Reinstatement { get; set; }
    }
}
