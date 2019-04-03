using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Описание местоположения контура границы земельного участка
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ContourZUOut")]
    public class ContourZUOut
    {

        /// <summary>
        /// Номер контура
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", ElementName = "number_pp")]
        public string Number_pp { get; set; }
        /// <summary>
        /// Кадастровый номер участка, входящего в состав единого землепользования
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cad_number")]
        public string Cad_number { get; set; }
        /// <summary>
        /// Описание элементов контура (характерных точек контура)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("entity_spatial")]
        public EntitySpatialZUOut Entity_spatial { get; set; }
    }
}
