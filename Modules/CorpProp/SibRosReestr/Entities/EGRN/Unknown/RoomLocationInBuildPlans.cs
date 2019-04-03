using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Местоположение помещения, расположенного в объекте недвижимости (планы расположения помещения)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RoomLocationInBuildPlans")]
    public class RoomLocationInBuildPlans
    {
        public RoomLocationInBuildPlans()
        {
            Location_in_build = new List<LevelAll>();
        }
        /// <summary>
        /// Общие сведения (кадастровый номер помещения)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("object")]
        public ObjectCadNumber Object { get; set; }
        /// <summary>
        /// Расположение в пределах объекта недвижимости (планы)
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("level", IsNullable = false, ElementName = "location_in_build")]
        [System.Xml.Serialization.XmlArray("location_in_build")]
        [System.Xml.Serialization.XmlArrayItem("level", Type = typeof(LevelAll), IsNullable = false)]
        public List<LevelAll> Location_in_build { get; set; }
    }
}
