using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Полные сведения о физическом лице
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("IndividualOut")]
    public class IndividualOut
    {

        /// <summary>
        /// Тип физического лица
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("individual_type")]
        public Dict Individual_type { get; set; }
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
        /// <summary>
        /// Дата рождения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "birth_date")]
        public System.DateTime Birth_date { get; set; }
        /// <summary>
        /// Место рождения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("birth_place")]
        public string Birth_place { get; set; }
        /// <summary>
        /// Гражданство
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("citizenship")]
        public Citizenship Citizenship { get; set; }
        /// <summary>
        /// СНИЛС
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("snils")]
        public string Snils { get; set; }
        /// <summary>
        /// Документ, удостоверяющий личность
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("identity_doc")]
        public PersonDocument Identity_doc { get; set; }
        /// <summary>
        /// Контактная информация
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("contacts")]
        public Contacts Contacts { get; set; }
    }
}
