#pragma warning disable
namespace SibRosReestr.EGRP.V06.ExtractObj
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Xml;
    using System.Collections.Generic;
    using SibRosReestr.EGRP.V06.ExtractSubj;


    /// <summary>
    /// Пакет информации - ответ на запрос сведений ЕГРП
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Extract
    {
        /// <summary>
        /// Электронный документ
        /// </summary>
        [XmlElement(Order = 0, ElementName = "eDocument")]
        public TServisInf EDocument { get; set; }
        /// <summary>
        /// Выписка из ЕГРП на ОНИ
        /// </summary>
        [XmlElement(Order = 1)]
        public ExtractReestrExtract ReestrExtract { get; set; }
    }


    /// <summary>
    /// раздел ЕГРП
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tObjectRight")]
    public class TObjectRight
    {
        /// <summary>
        /// Описание объекта
        /// </summary>
        [XmlElement(Order = 0)]
        public TObject ObjectDesc { get; set; }
        /// <summary>
        /// Права
        /// </summary>
        [XmlElement("Right", Order = 1)]
        public List<TObjectRightRight> Right { get; set; }
        [XmlElement("NoShareHolding", typeof(object), Order = 2)]
        [XmlElement("ShareHolding", typeof(TShareHolding), Order = 2)]
        public List<object> Items { get; set; }

        /// <summary>
        /// TObjectRight class constructor
        /// </summary>
        public TObjectRight()
        {
            Items = new List<object>();
            Right = new List<TObjectRightRight>();
        }
    }

    /// <summary>
    /// Права
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true, TypeName = "tObjectRightRight")]
    [XmlRootAttribute("tObjectRightRight")]
    public class TObjectRightRight : TRightObj
    {
        [XmlAttribute(AttributeName = "RightNumber")]
        public string RightNumber { get; set; }
    }

   
    /// <summary>
    /// Выписка из ЕГРП на ОНИ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute("ExtractReestrExtract")]
    public class ExtractReestrExtract
    {
        /// <summary>
        /// Информация о запросе
        /// </summary>
        [XmlElement(Order = 0)]
        public TExtrAttribut DeclarAttribute { get; set; }
        /// <summary>
        /// выписка о правах на ОНИ
        /// </summary>
        [XmlElement(Order = 1)]
        public ExtractReestrExtractExtractObjectRight ExtractObjectRight { get; set; }
        /// <summary>
        /// уведомление об отсутствии сведений на ОНИ
        /// </summary>
        [XmlElement(Order = 2)]
        public TNoticeObj NoticelObj { get; set; }
        /// <summary>
        /// отказ в выдаче выписки о правах на бъект
        /// </summary>
        [XmlElement(Order = 3)]
        public TRefusalObj RefusalObj { get; set; }
    }

    /// <summary>
    /// выписка о правах на ОНИ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute("ExtractReestrExtractExtractObjectRight")]
    public class ExtractReestrExtractExtractObjectRight
    {
        /// <summary>
        /// Объединенное текстовое описание запроса
        /// </summary>
        [XmlElement(Order = 0)]
        public THeadContent HeadContent { get; set; }
        /// <summary>
        /// Информация из ЕГРП
        /// </summary>
        [XmlElement("ExtractObject", Order = 1)]
        public List<ExtractReestrExtractExtractObjectRightExtractObject> ExtractObject { get; set; }
        /// <summary>
        /// Объединенное  описание завершающего текста (ссылка на закон и т.п.)
        /// </summary>
        [XmlElement(Order = 2)]
        public TFootContent FootContent { get; set; }

        /// <summary>
        /// ExtractReestrExtractExtractObjectRight class constructor
        /// </summary>
        public ExtractReestrExtractExtractObjectRight()
        {
            ExtractObject = new List<ExtractReestrExtractExtractObjectRightExtractObject>();
        }
    }

    /// <summary>
    /// Информация из ЕГРП
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute("ExtractReestrExtractExtractObjectRightExtractObject")]
    public class ExtractReestrExtractExtractObjectRightExtractObject
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        [XmlElement(Order = 0)]
        public TObjectRight ObjectRight { get; set; }
        /// <summary>
        /// правопритязания
        /// </summary>
        [XmlElement(Order = 1)]
        public string RightAssert { get; set; }
        /// <summary>
        /// право требования
        /// </summary>
        [XmlElement(Order = 2)]
        public string RightClaim { get; set; }
        /// <summary>
        /// отметка о возражении
        /// </summary>
        [XmlElement(Order = 3)]
        public string RightAgainst { get; set; }
        /// <summary>
        /// отметка об изъятии
        /// </summary>
        [XmlElement(Order = 4)]
        public string RightSteal { get; set; }
    }
}
#pragma warning restore
