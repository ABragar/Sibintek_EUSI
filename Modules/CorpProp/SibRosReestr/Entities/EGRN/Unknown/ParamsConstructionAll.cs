﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Характеристики сооружения
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ParamsConstructionAll")]
    public class ParamsConstructionAll
    {
        public ParamsConstructionAll()
        {
            Base_parameters = new List<BaseParametersBase_parameter>();
            Permitted_uses = new List<PermittedUse>();

        }

        /// <summary>
        /// Основные характеристики
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("base_parameter", IsNullable = false, ElementName = "base_parameters")]
        [System.Xml.Serialization.XmlArray("base_parameters")]
        [System.Xml.Serialization.XmlArrayItem("base_parameter", Type = typeof(BaseParametersBase_parameter), IsNullable = false)]
        public List<BaseParametersBase_parameter> Base_parameters { get; set; }
        /// <summary>
        /// Количество этажей (в том числе подземных)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("floors")]
        public string Floors { get; set; }
        /// <summary>
        /// Количество подземных этажей
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("underground_floors")]
        public string Underground_floors { get; set; }
        /// <summary>
        /// Назначение сооружения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("purpose")]
        public string Purpose { get; set; }
        /// <summary>
        /// Наименование сооружения
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Год ввода в эксплуатацию по завершении строительства
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("year_commisioning")]
        public string Year_commisioning { get; set; }
        /// <summary>
        /// Год завершения строительства
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("year_built")]
        public string Year_built { get; set; }
        /// <summary>
        /// Вид(ы) разрешенного использования
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("permitted_use", IsNullable = false, ElementName = "permitted_uses")]
        [System.Xml.Serialization.XmlArray("permitted_uses")]
        [System.Xml.Serialization.XmlArrayItem("permitted_use", Type = typeof(PermittedUse), IsNullable = false)]
        public List<PermittedUse> Permitted_uses { get; set; }
    }
}