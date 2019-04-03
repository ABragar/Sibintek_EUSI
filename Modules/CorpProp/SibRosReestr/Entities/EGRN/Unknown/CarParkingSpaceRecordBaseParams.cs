using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объекте недвижимости - машино-месте
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CarParkingSpaceRecordBaseParams")]
    public class CarParkingSpaceRecordBaseParams
    {

        /// <summary>
        /// Даты государственной регистрации
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("record_info")]
        public RecordInfo Record_info { get; set; }
        /// <summary>
        /// Общие сведения об объекте недвижимости
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("object")]
        public ObjectCommonData Object { get; set; }
        /// <summary>
        /// Сведения об объектах (связь с кадастровыми номерами)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cad_links")]
        public CadLinksCarParkingSpaceBase Cad_links { get; set; }
        /// <summary>
        /// Характеристики машино-места
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("params")]
        public Area Params { get; set; }
        /// <summary>
        /// Адрес (местоположение) машино-места
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address_room")]
        public AddressLocationCarParkingSpace Address_room { get; set; }
        /// <summary>
        /// Местоположение машино-места в объекте недвижимости (планы)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("location_in_build")]
        public LevelAll Location_in_build { get; set; }
        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cost")]
        public Cost Cost { get; set; }
        /// <summary>
        /// Особые отметки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("special_notes")]
        public string Special_notes { get; set; }
    }
}
