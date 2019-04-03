using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Основные характеристики помещения
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ParamsRoomBase")]
    public class ParamsRoomBase
    {
        public ParamsRoomBase()
        {
            Permitted_uses = new List<PermittedUse>();

        }

        /// <summary>
        /// Площадь, в кв. метрах
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("area")]
        public decimal Area { get; set; }
        /// <summary>
        /// Вид(ы) разрешенного использования
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("permitted_use", IsNullable = false, ElementName = "permitted_uses")]
        [System.Xml.Serialization.XmlArray("permitted_uses")]
        [System.Xml.Serialization.XmlArrayItem("permitted_use", Type = typeof(PermittedUse), IsNullable = false)]
        public List<PermittedUse> Permitted_uses { get; set; }
        /// <summary>
        /// Наименование помещения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Назначение помещения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("purpose")]
        public Dict Purpose { get; set; }
        /// <summary>
        /// Вид жилого помещения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public Dict Type { get; set; }
        /// <summary>
        /// Общее имущество собственников помещений в многоквартирном доме
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("common_property")]
        public bool Common_property { get; set; }
        /// <summary>
        /// Имущество общего пользования
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("service")]
        public bool Service { get; set; }
        /// <summary>
        /// Вид жилого помещения специализированного жилищного фонда
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("special_type")]
        public Dict Special_type { get; set; }
    }
}
