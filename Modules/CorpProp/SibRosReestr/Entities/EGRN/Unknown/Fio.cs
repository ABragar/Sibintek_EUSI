
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// ФИО
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("Fio")]
    public class Fio 
    {
        public Fio() { }

        /// <summary>
        /// Фамилия
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("surname")]
        public string Surname { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("patronymic")]
        public string Patronymic { get; set; }
    }
}
