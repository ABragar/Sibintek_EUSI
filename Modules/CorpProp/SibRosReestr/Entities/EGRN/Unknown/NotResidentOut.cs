using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Иностранное юридическое лицо
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("NotResidentOut")]
    public class NotResidentOut
    {

        /// <summary>
        /// Организационно-правовая форма
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("incorporation_form")]
        public Dict Incorporation_form { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Страна регистрации (инкорпорации)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("incorporate_country")]
        public Dict Incorporate_country { get; set; }
        /// <summary>
        /// Регистрационный номер
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("registration_number")]
        public int Registration_number { get; set; }
        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "date_state_reg")]
        public System.DateTime Date_state_reg { get; set; }
        /// <summary>
        /// Наименование регистрирующего органа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("registration_organ")]
        public string Registration_organ { get; set; }
        /// <summary>
        /// Адрес (местонахождение) в стране регистрации (инкорпорации)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("reg_address_subject")]
        public string Reg_address_subject { get; set; }
        /// <summary>
        /// ИНН
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("inn")]
        public string Inn { get; set; }
    }
}
