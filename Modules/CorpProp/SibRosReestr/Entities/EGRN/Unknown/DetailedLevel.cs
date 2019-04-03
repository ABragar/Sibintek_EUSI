using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Детализированный уровень
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("DetailedLevel")]
    public class DetailedLevel
    {

        /// <summary>
        /// Улица
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("street")]
        public Street Street { get; set; }
        /// <summary>
        /// Дом
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("level1")]
        public Level1 Level1 { get; set; }
        /// <summary>
        /// Корпус
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("level2")]
        public Level2 Level2 { get; set; }
        /// <summary>
        /// Строение
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("level3")]
        public Level3 Level3 { get; set; }
        /// <summary>
        /// Квартира
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("apartment")]
        public Apartment Apartment { get; set; }
        /// <summary>
        /// Иное описание местоположения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("other")]
        public string Other { get; set; }
    }
}
