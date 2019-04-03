using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Общие сведения об объекте недвижимости (земельном участке) (и реквизиты решения об изъятии)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ObjectLandAndWithdrawal")]
    public class ObjectLandAndWithdrawal
    {

        /// <summary>
        /// Общие сведения о земельном участке
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("common_data")]
        public CommonData Common_data { get; set; }
        /// <summary>
        /// Вид земельного участка
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("subtype")]
        public Dict Subtype { get; set; }
        /// <summary>
        /// Сведения о наличии решения об изъятии объекта недвижимости для государственных и муниципальных нужд
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("date_removed_cad_account")]
        public DateRemovedCadAccount Date_removed_cad_account { get; set; }
        /// <summary>
        /// Дата постановки по документу
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", ElementName = "reg_date_by_doc")]
        public System.DateTime Reg_date_by_doc { get; set; }
    }
}
