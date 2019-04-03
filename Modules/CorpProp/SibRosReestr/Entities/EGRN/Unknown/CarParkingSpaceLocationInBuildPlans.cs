using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Местоположение машино-места, расположенного в объекте недвижимости (планы расположения машино-места)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CarParkingSpaceLocationInBuildPlans")]
    public class CarParkingSpaceLocationInBuildPlans
    {

        /// <summary>
        /// Общие сведения (кадастровый номер машино-места)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("object")]
        public ObjectCadNumber Object { get; set; }
        /// <summary>
        /// Расположение в пределах объекта недвижимости (планы)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("location_in_build")]
        public LevelAll Location_in_build { get; set; }
    }
}
