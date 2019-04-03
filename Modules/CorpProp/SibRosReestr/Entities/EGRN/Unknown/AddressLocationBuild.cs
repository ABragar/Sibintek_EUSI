﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Адрес (местоположение) здания
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("AddressLocationBuild")]
    public class AddressLocationBuild
    {

        /// <summary>
        /// Тип адреса
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address_type")]
        public Dict Address_type { get; set; }
        /// <summary>
        /// Адрес (местоположение)
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("address")]
        public AddressMain Address { get; set; }
        /// <summary>
        /// Местоположение
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("location")]
        public LocationOks Location { get; set; }
    }
}
