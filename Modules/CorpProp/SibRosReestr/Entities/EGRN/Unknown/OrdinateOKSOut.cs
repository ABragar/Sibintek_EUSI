using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Координаты контура ОКС
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("OrdinateOKSOut")]
    public class OrdinateOKSOut
    {

        /// <summary>
        /// Координата X
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("x")]
        public decimal X { get; set; }
        /// <summary>
        /// Координата Y
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("y")]
        public decimal Y { get; set; }
        /// <summary>
        /// Координата Z
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("z")]
        public decimal Z { get; set; }
        /// <summary>
        /// Номер точки (порядок обхода)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", ElementName = "ord_nmb")]
        public string Ord_nmb { get; set; }
        /// <summary>
        /// Номер точки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("num_geopoint")]
        public string Num_geopoint { get; set; }
        /// <summary>
        /// Погрешность
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("delta_geopoint")]
        public decimal Delta_geopoint { get; set; }
        /// <summary>
        /// Радиус
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("r")]
        public decimal R { get; set; }
    }
}
