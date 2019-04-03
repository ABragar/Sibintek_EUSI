using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Разрешенное использование по градостроительному регламенту
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("PermittesUsesGradReg")]
    public class PermittesUsesGradReg
    {

        /// <summary>
        /// Реестровый номер границы
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("reg_numb_border")]
        public string Reg_numb_border { get; set; }
        /// <summary>
        /// Вид разрешенного использования соответствии с классификатором, утвержденным приказом Минэкономразвития России от 01.09.2014 № 540
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("land_use")]
        public Dict Land_use { get; set; }
        /// <summary>
        /// Разрешенное использование (текстовое описание)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("permitted_use_text")]
        public string Permitted_use_text { get; set; }
    }
}
