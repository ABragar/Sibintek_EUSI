using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Характеристики объекта незавершенного строительства (основные характеристики, назначение, степень готовности)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ParamsUnderConstructionDegree")]
    public class ParamsUnderConstructionDegree
    {
        public ParamsUnderConstructionDegree()
        {
            Base_parameters = new List<BaseParametersBase_parameter>();
        }
        /// <summary>
        /// Основные характеристики и их проектируемые значения
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("base_parameter", IsNullable = false, ElementName = "base_parameters")]
        [System.Xml.Serialization.XmlArray("base_parameters")]
        [System.Xml.Serialization.XmlArrayItem("base_parameter", Type = typeof(BaseParametersBase_parameter), IsNullable = false)]
        public List<BaseParametersBase_parameter> Base_parameters { get; set; }
        /// <summary>
        /// Степень готовности объекта незавершенного строительства в процентах
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("degree_readiness")]
        public string Degree_readiness { get; set; }
        /// <summary>
        /// Проектируемое назначение
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("purpose")]
        public string Purpose { get; set; }
    }
}
