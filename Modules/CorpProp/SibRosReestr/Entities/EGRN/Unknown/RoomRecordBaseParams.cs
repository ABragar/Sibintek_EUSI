using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Сведения об объекте недвижимости - помещении
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RoomRecordBaseParams")]
    public class RoomRecordBaseParams
    {
        public RoomRecordBaseParams()
        {
            Location_in_build = new List<LevelAll>();
            Object_parts = new List<ObjectPartNumberRestrictions>();

        }
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
        public CadLinksRoom Cad_links { get; set; }
        /// <summary>
        /// Характеристики помещения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("params")]
        public ParamsRoomBase Params { get; set; }
        /// <summary>
        /// Адрес помещения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address_room")]
        public AddressLocationRoom Address_room { get; set; }
        /// <summary>
        /// Местоположение помещения в объекте недвижимости (планы)
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("level", IsNullable = false, ElementName = "location_in_build")]
        [System.Xml.Serialization.XmlArray("location_in_build")]
        [System.Xml.Serialization.XmlArrayItem("level", Type = typeof(LevelAll), IsNullable = false)]
        public List<LevelAll> Location_in_build { get; set; }
        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("cost")]
        public Cost Cost { get; set; }
        /// <summary>
        /// Сведения о частях помещения
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("object_part", IsNullable = false, ElementName = "object_parts")]
        [System.Xml.Serialization.XmlArray("object_parts")]
        [System.Xml.Serialization.XmlArrayItem("object_part", Type = typeof(ObjectPartNumberRestrictions), IsNullable = false)]
        public List<ObjectPartNumberRestrictions> Object_parts { get; set; }
        /// <summary>
        /// Особые отметки
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("special_notes")]
        public string Special_notes { get; set; }
    }
}
