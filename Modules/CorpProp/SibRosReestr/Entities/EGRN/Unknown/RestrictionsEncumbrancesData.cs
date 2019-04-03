using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Общие сведения об ограничениях прав и обременениях объекта недвижимости (в том числе ограничиваемые права, ограничиваемые сделки)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RestrictionsEncumbrancesData")]
    public class RestrictionsEncumbrancesData
    {
        public RestrictionsEncumbrancesData()
        {
            Restricting_rights = new List<RightRecordNumber>();
        }
        /// <summary>
        /// Номер регистрации ограничения права или обременения объекта недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("restriction_encumbrance_number")]
        public string Restriction_encumbrance_number { get; set; }
        /// <summary>
        /// Вид зарегистрированного ограничения права или обременения объекта недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("restriction_encumbrance_type")]
        public Dict Restriction_encumbrance_type { get; set; }
        /// <summary>
        /// Срок, на который установлено ограничение прав и обременение объекта недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("period")]
        public PeriodType Period { get; set; }
        /// <summary>
        /// Дополнительная информация в зависимости от вида зарегистрированного ограничения права или обременения объекта недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("additional_encumbrance_info")]
        public AdditionalEncumbranceInfoType Additional_encumbrance_info { get; set; }
        /// <summary>
        /// Ограничиваемые права
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("restricting_right", IsNullable = false, ElementName = "restricting_rights")]
        [System.Xml.Serialization.XmlArray("restricting_rights")]
        [System.Xml.Serialization.XmlArrayItem("restricting_right", Type = typeof(RightRecordNumber), IsNullable = false)]
        public List<RightRecordNumber> Restricting_rights { get; set; }
    }
}
