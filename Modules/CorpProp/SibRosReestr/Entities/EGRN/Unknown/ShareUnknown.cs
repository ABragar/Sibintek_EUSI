using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Не указан размер доли в праве общей долевой собственности на общее имущество, в том числе на земельный участок, собственников помещений, машино-мест в здании, если объектом недвижимости является помещение, машино-место в здании
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ShareUnknown")]
    public class ShareUnknown
    {

        /// <summary>
        /// Доля в праве общей долевой собственности пропорциональна размеру общей площади
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("share_description")]
        public string Share_description { get; set; }
        /// <summary>
        /// Кадастровый номер для расчета пропорций
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("proportion_cad_number")]
        public string Proportion_cad_number { get; set; }
    }
}
