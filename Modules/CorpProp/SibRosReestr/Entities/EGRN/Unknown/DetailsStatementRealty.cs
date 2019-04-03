
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Реквизиты выписки (по объекту недвижимоти)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DetailsStatementRealty")]
    public class DetailsStatementRealty 
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса DetailsStatementRealty.
        /// </summary>
        public DetailsStatementRealty()
        {

        }

        /// <summary>
        /// Группа верхних реквизитов
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("group_top_requisites")]
        public DetailsStatementRealtyGroup_top_requisites Group_top_requisites { get; set; }
        /// <summary>
        /// Группа нижних реквизитов
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("group_lower_requisites")]
        public DetailsStatementRealtyGroup_lower_requisites Group_lower_requisites { get; set; }
    }
}
