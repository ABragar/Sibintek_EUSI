﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Описание местоположения контура объекта недвижимости
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ContoursLocationOKSOut")]
    public class ContoursLocationOKSOut
    {
        public ContoursLocationOKSOut()
        {
            Contours = new List<ContourOKSOut>();
        }
        /// <summary>
        /// Общие сведения (кадастровый номер)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("object")]
        public ObjectCadNumber Object { get; set; }
        /// <summary>
        /// Описание местоположения контура
        /// </summary>
        //[System.Xml.Serialization.XmlArrayItemAttribute("contour", IsNullable = false, ElementName = "contours")]
        [XmlArray("contours")]
        [XmlArrayItem("contour", Type = typeof(ContourOKSOut), IsNullable = false)]
        public List<ContourOKSOut> Contours { get; set; }
    }
}
