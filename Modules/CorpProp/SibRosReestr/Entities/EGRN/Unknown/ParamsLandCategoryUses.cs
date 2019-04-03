using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibRosReestr.EGRN.Unknown
{
    /// <summary>
    /// Характеристики земельного участка (площадь, категория, разрешенное использование)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ParamsLandCategoryUses")]
    public class ParamsLandCategoryUses
    {

        /// <summary>
        /// Категория
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("category")]
        public ParamsLandCategoryUsesCategory Category { get; set; }
        /// <summary>
        /// Разрешенное использование
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("permitted_use")]
        public AllowedUse Permitted_use { get; set; }
        /// <summary>
        /// Разрешенное использование по градостроительному регламенту
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("permittes_uses_grad_reg")]
        public PermittesUsesGradReg Permittes_uses_grad_reg { get; set; }
        /// <summary>
        /// Площадь
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("area")]
        public LandArea Area { get; set; }
    }
}
