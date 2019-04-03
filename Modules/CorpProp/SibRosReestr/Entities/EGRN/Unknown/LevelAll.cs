using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Описание местоположения (Уровень (Этаж))
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LevelAll")]
    public class LevelAll
    {
        public LevelAll()
        {
            Plans = new List<Plan>();
        }
        /// <summary>
        /// Номер этажа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("floor")]
        public string Floor { get; set; }
        /// <summary>
        /// Тип этажа
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("floor_type")]
        public Dict Floor_type { get; set; }
        /// <summary>
        /// Номер на поэтажном плане
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("plan_number")]
        public string Plan_number { get; set; }
        /// <summary>
        /// Описание расположения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("description")]
        public string Description { get; set; }
        /// <summary>
        /// Планы
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("plan", IsNullable = false, ElementName = "plans")]
        [System.Xml.Serialization.XmlArray("plans")]
        [System.Xml.Serialization.XmlArrayItem("plan", Type = typeof(Plan), IsNullable = false)]
        public List<Plan> Plans { get; set; }
    }
}
