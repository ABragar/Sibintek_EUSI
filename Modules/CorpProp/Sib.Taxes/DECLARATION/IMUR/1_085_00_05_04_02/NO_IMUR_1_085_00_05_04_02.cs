// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 4.2.0.72
//    <NameSpace>Sib.Taxes</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><DataMemberNameArg>OnlyIfDifferent</DataMemberNameArg><DataMemberOnXmlIgnore>False</DataMemberOnXmlIgnore><CodeBaseTag>Net45</CodeBaseTag><InitializeFields>Collections</InitializeFields><GenerateUnusedComplexTypes>False</GenerateUnusedComplexTypes><GenerateUnusedSimpleTypes>False</GenerateUnusedSimpleTypes><GenerateXMLAttributes>True</GenerateXMLAttributes><OrderXMLAttrib>True</OrderXMLAttrib><EnableLazyLoading>False</EnableLazyLoading><VirtualProp>False</VirtualProp><PascalCase>False</PascalCase><AutomaticProperties>True</AutomaticProperties><PropNameSpecified>Default</PropNameSpecified><PrivateFieldName>StartWithUnderscore</PrivateFieldName><PrivateFieldNamePrefix></PrivateFieldNamePrefix><EnableRestriction>True</EnableRestriction><RestrictionMaxLenght>True</RestrictionMaxLenght><RestrictionRegEx>True</RestrictionRegEx><RestrictionRange>True</RestrictionRange><ValidateProperty>False</ValidateProperty><ClassNamePrefix></ClassNamePrefix><ClassLevel>Public</ClassLevel><PartialClass>False</PartialClass><ClassesInSeparateFiles>False</ClassesInSeparateFiles><ClassesInSeparateFilesDir></ClassesInSeparateFilesDir><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><EnableAppInfoSettings>True</EnableAppInfoSettings><EnableExternalSchemasCache>False</EnableExternalSchemasCache><EnableDebug>False</EnableDebug><EnableWarn>False</EnableWarn><ExcludeImportedTypes>False</ExcludeImportedTypes><ExpandNesteadAttributeGroup>False</ExpandNesteadAttributeGroup><CleanupCode>True</CleanupCode><EnableXmlSerialization>False</EnableXmlSerialization><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><EnableEncoding>True</EnableEncoding><EnableXMLIndent>False</EnableXMLIndent><IndentChar>Indent2Space</IndentChar><NewLineAttr>False</NewLineAttr><OmitXML>False</OmitXML><Encoder>UTF8</Encoder><Serializer>XmlSerializer</Serializer><sspNullable>False</sspNullable><sspString>False</sspString><sspCollection>False</sspCollection><sspComplexType>False</sspComplexType><sspSimpleType>False</sspSimpleType><sspEnumType>False</sspEnumType><XmlSerializerEvent>False</XmlSerializerEvent><BaseClassName>EntityBase</BaseClassName><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><CustomUsings></CustomUsings><AttributesToExlude></AttributesToExlude>
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace Sib.Taxes.Declaration.IMUR.V1_085_00_05_04_02
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


    /// <summary>
    /// Файл обмена
    /// </summary>
    [ShemaPathAttribute(@"Sib.Taxes.DECLARATION.IMUR._1_085_00_05_04_02.NO_IMUR_1_085_00_05_04_02.xsd")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class Файл
    {
        /// <summary>
        /// Состав и структура документа
        /// </summary>
        [XmlElement(Order = 0)]
        public ФайлДокумент Документ { get; set; }
        /// <summary>
        /// Идентификатор файла
        /// </summary>
        /// <summary>
        /// Содержит (повторяет) имя сформированного файла (без расширения)
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(255, MinimumLength = 1)]
        public string ИдФайл { get; set; }
        /// <summary>
        /// Версия программы, с помощью которой сформирован файл
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(40, MinimumLength = 1)]
        public string ВерсПрог { get; set; }
        /// <summary>
        /// Версия формата
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(5, MinimumLength = 1)]
        public ФайлВерсФорм ВерсФорм { get; set; }
    }

    /// <summary>
    /// Состав и структура документа
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокумент
    {
        /// <summary>
        /// Сведения о налогоплательщике
        /// </summary>
        [XmlElement(Order = 0)]
        public ФайлДокументСвНП СвНП { get; set; }
        /// <summary>
        /// Сведения о лице, подписавшем документ
        /// </summary>
        [XmlElement(Order = 1)]
        public ФайлДокументПодписант Подписант { get; set; }
        /// <summary>
        /// Налоговый расчет по авансовому платежу по налогу на имущество организаций
        /// </summary>
        [XmlArrayAttribute(Order = 2)]
        [XmlArrayItemAttribute("СумНалПУ", IsNullable = false)]
        public List<ФайлДокументСумНалПУ> ИмущАв { get; set; }
        /// <summary>
        /// Код формы отчетности по КНД
        /// </summary>
        [XmlAttribute]
        public ФайлДокументКНД КНД { get; set; }
        /// <summary>
        /// Дата формирования документа
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("((((0[1-9]{1}|1[0-9]{1}|2[0-8]{1})\\.(0[1-9]{1}|1[0-2]{1}))|((29|30)\\.(01|0[3-9]{1" +
    "}|1[0-2]{1}))|(31\\.(01|03|05|07|08|10|12)))\\.((19|20)[0-9]{2}))|(29\\.02\\.((19|20" +
    ")(((0|2|4|6|8)(0|4|8))|((1|3|5|7|9)(2|6)))))")]
        public string ДатаДок { get; set; }
        /// <summary>
        /// Период (код)
        /// </summary>
        [XmlAttribute]
        public ФайлДокументПериод Период { get; set; }
        /// <summary>
        /// Отчетный год
        /// </summary>
        [XmlAttribute(DataType = "gYear")]
        public string ОтчетГод { get; set; }
        /// <summary>
        /// Код налогового органа
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("[0-9]{4}")]
        public string КодНО { get; set; }
        /// <summary>
        /// Номер корректировки
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(3, MinimumLength = 1)]
        public string НомКорр { get; set; }
        /// <summary>
        /// Код места, по которому представляется документ
        /// </summary>
        [XmlAttribute]
        public ФайлДокументПоМесту ПоМесту { get; set; }

        /// <summary>
        /// ФайлДокумент class constructor
        /// </summary>
        public ФайлДокумент()
        {
            ИмущАв = new List<ФайлДокументСумНалПУ>();
        }
    }

    /// <summary>
    /// Сведения о налогоплательщике
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСвНП
    {
        /// <summary>
        /// Налогоплательщик - организация
        /// </summary>
        [XmlElement(Order = 0)]
        public ФайлДокументСвНПНПЮЛ НПЮЛ { get; set; }
        /// <summary>
        /// Номер контактного телефона
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(20, MinimumLength = 1)]
        public string Тлф { get; set; }
    }

    /// <summary>
    /// Налогоплательщик - организация
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСвНПНПЮЛ
    {
        /// <summary>
        /// Сведения о реорганизованной  организации
        /// </summary>
        [XmlElement(Order = 0)]
        public ФайлДокументСвНПНПЮЛСвРеоргЮЛ СвРеоргЮЛ { get; set; }
        /// <summary>
        /// Наименование организации
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(1000, MinimumLength = 1)]
        public string НаимОрг { get; set; }
        /// <summary>
        /// ИНН организации
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{8}")]
        public string ИННЮЛ { get; set; }
        /// <summary>
        /// КПП
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})([0-9]{2})([0-4A-Z]{1}[0-9A-Z]{1}|5[1-9A-Z]{1}" +
    "|[6-9A-Z]{1}[0-9A-Z]{1})([0-9]{3})")]
        public string КПП { get; set; }
    }

    /// <summary>
    /// Сведения о реорганизованной  организации
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСвНПНПЮЛСвРеоргЮЛ
    {
        /// <summary>
        /// Код формы реорганизации
        /// </summary>
        /// <summary>
        /// Принимает значение:
        /// 1 – преобразование   |
        /// 2 – слияние   |
        /// 3 – разделение   |
        /// 5 – присоединение   |
        /// 6 – разделение с одновременным присоединением
        /// </summary>
        [XmlAttribute]
        public ФайлДокументСвНПНПЮЛСвРеоргЮЛФормРеорг ФормРеорг { get; set; }
        /// <summary>
        /// ИНН реорганизованной организации
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{8}")]
        public string ИННЮЛ { get; set; }
        /// <summary>
        /// КПП
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})([0-9]{2})([0-4A-Z]{1}[0-9A-Z]{1}|5[1-9A-Z]{1}" +
    "|[6-9A-Z]{1}[0-9A-Z]{1})([0-9]{3})")]
        public string КПП { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументСвНПНПЮЛСвРеоргЮЛФормРеорг
    {
        [XmlEnumAttribute("1")]
        Item1,
        [XmlEnumAttribute("2")]
        Item2,
        [XmlEnumAttribute("3")]
        Item3,
        [XmlEnumAttribute("5")]
        Item5,
        [XmlEnumAttribute("6")]
        Item6,
    }

    /// <summary>
    /// Остаточная стоимость основных средств помесячно
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public class ОстСтоимМес
    {
        /// <summary>
        /// Остаточная стоимость основных средств, признаваемых объектом налогообложения
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СтОстОН { get; set; }
        /// <summary>
        /// Стоимость льготируемого имущества
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СтЛьгИмущ { get; set; }
    }

    /// <summary>
    /// Фамилия, имя, отчество
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public class ФИОТип
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(60, MinimumLength = 1)]
        public string Фамилия { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(60, MinimumLength = 1)]
        public string Имя { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(60, MinimumLength = 1)]
        public string Отчество { get; set; }
    }

    /// <summary>
    /// Сведения о лице, подписавшем документ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументПодписант
    {
        /// <summary>
        /// Фамилия, имя, отчество
        /// </summary>
        [XmlElement(Order = 0)]
        public ФИОТип ФИО { get; set; }
        /// <summary>
        /// Сведения о представителе налогоплательщика
        /// </summary>
        /// <summary>
        /// Элемент обязателен при <ПрПодп>=2
        /// </summary>
        [XmlElement(Order = 1)]
        public ФайлДокументПодписантСвПред СвПред { get; set; }
        /// <summary>
        /// Признак лица, подписавшего документ
        /// </summary>
        /// <summary>
        /// Принимает значение:
        /// 1 – налогоплательщик   |
        /// 2 – представитель налогоплательщика
        /// </summary>
        [XmlAttribute]
        public ФайлДокументПодписантПрПодп ПрПодп { get; set; }
    }

    /// <summary>
    /// Сведения о представителе налогоплательщика
    /// </summary>
    /// <summary>
    /// Элемент обязателен при <ПрПодп>=2
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументПодписантСвПред
    {
        /// <summary>
        /// Наименование и реквизиты документа, подтверждающего полномочия представителя
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(120, MinimumLength = 1)]
        public string НаимДок { get; set; }
        /// <summary>
        /// Наименование организации - представителя налогоплательщика
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(1000, MinimumLength = 1)]
        public string НаимОрг { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументПодписантПрПодп
    {
        [XmlEnumAttribute("1")]
        Item1,
        [XmlEnumAttribute("2")]
        Item2,
    }

    /// <summary>
    /// Сумма авансового платежа по налогу, подлежащая уплате в бюджет, по данным налогоплательщика
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСумНалПУ
    {
        /// <summary>
        /// Исчисление суммы авансового платежа по налогу в отношении подлежащего налогообложению имущества российских организаций и иностранных организаций, осуществляющих деятельность в Российской Федерации через постоянные представительства
        /// </summary>
        [XmlArrayAttribute(Order = 0)]
        [XmlArrayItemAttribute("РасОб", IsNullable = false)]
        public List<ФайлДокументСумНалПУРасОб> РасОбДеятРФ { get; set; }
        /// <summary>
        /// Информация об объектах недвижимого имущества, облагаемых налогом по среднегодовой стоимости
        /// </summary>
        [XmlArrayAttribute(Order = 1)]
        [XmlArrayItemAttribute("ИнфОбъект", IsNullable = false)]
        public List<ФайлДокументСумНалПУИнфОбъект> ОбъектОблНал { get; set; }
        /// <summary>
        /// Исчисление суммы авансового платежа по налогу за отчетный период по объекту недвижимого имущества, налоговая база в отношении которого определяется как кадастровая стоимость
        /// </summary>
        [XmlArrayAttribute(Order = 2)]
        [XmlArrayItemAttribute("РасОб", IsNullable = false)]
        public List<ФайлДокументСумНалПУРасОб1> РасОБНедИО { get; set; }
        /// <summary>
        /// Код по ОКТМО
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(11, MinimumLength = 8)]
        [MultipleRegularExpressionAttribute("[0-9]{8}")]
        [MultipleRegularExpressionAttribute("[0-9]{11}")]
        public string ОКТМО { get; set; }
        /// <summary>
        /// Код бюджетной классификации
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("[0-9]{20}")]
        public string КБК { get; set; }
        /// <summary>
        /// Сумма авансового платежа по налогу, подлежащая уплате в бюджет
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string НалПУ { get; set; }

        /// <summary>
        /// ФайлДокументСумНалПУ class constructor
        /// </summary>
        public ФайлДокументСумНалПУ()
        {
            РасОБНедИО = new List<ФайлДокументСумНалПУРасОб1>();
            ОбъектОблНал = new List<ФайлДокументСумНалПУИнфОбъект>();
            РасОбДеятРФ = new List<ФайлДокументСумНалПУРасОб>();
        }
    }

    /// <summary>
    /// Расчет по объекту имущества
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСумНалПУРасОб
    {
        /// <summary>
        /// Данные для расчета средней стоимости имущества за отчетный период
        /// </summary>
        [XmlElement(Order = 0)]
        public ФайлДокументСумНалПУРасОбДанРасСтПер ДанРасСтПер { get; set; }
        /// <summary>
        /// Расчет суммы авансового платежа по налогу
        /// </summary>
        [XmlElement(Order = 1)]
        public ФайлДокументСумНалПУРасОбРасчАванПл РасчАванПл { get; set; }
        /// <summary>
        /// Код вида имущества
        /// </summary>
        [XmlAttribute]
        public ФайлДокументСумНалПУРасОбВидИмущ ВидИмущ { get; set; }
    }

    /// <summary>
    /// Данные для расчета средней стоимости имущества за отчетный период
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСумНалПУРасОбДанРасСтПер
    {
        /// <summary>
        /// Остаточная стоимость основных средств на 01.01
        /// </summary>
        [XmlElement(Order = 0)]
        public ОстСтоимМес ОстСтом0101 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.02
        /// </summary>
        [XmlElement(Order = 1)]
        public ОстСтоимМес ОстСтом0102 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.03
        /// </summary>
        [XmlElement(Order = 2)]
        public ОстСтоимМес ОстСтом0103 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.04
        /// </summary>
        [XmlElement(Order = 3)]
        public ОстСтоимМес ОстСтом0104 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.05
        /// </summary>
        [XmlElement(Order = 4)]
        public ОстСтоимМес ОстСтом0105 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.06
        /// </summary>
        [XmlElement(Order = 5)]
        public ОстСтоимМес ОстСтом0106 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.07
        /// </summary>
        [XmlElement(Order = 6)]
        public ОстСтоимМес ОстСтом0107 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.08
        /// </summary>
        [XmlElement(Order = 7)]
        public ОстСтоимМес ОстСтом0108 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.09
        /// </summary>
        [XmlElement(Order = 8)]
        public ОстСтоимМес ОстСтом0109 { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств на 01.10
        /// </summary>
        [XmlElement(Order = 9)]
        public ОстСтоимМес ОстСтом0110 { get; set; }
    }

    /// <summary>
    /// Расчет суммы авансового платежа по налогу
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСумНалПУРасОбРасчАванПл
    {
        /// <summary>
        /// Средняя стоимость имущества за отчетный период
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СтИмущ { get; set; }
        /// <summary>
        /// Код налоговой льготы
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(20, MinimumLength = 7)]
        [RegularExpressionAttribute("([0-9]{7})|(2012000/(............))")]
        public string КодНалЛьг { get; set; }
        /// <summary>
        /// Средняя стоимость необлагаемого налогом имущества за  отчетный период
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СтИмущНеобл { get; set; }
        /// <summary>
        /// Доля балансовой стоимости объекта недвижимого имущества на территории соответствующего субъекта Российской Федерации
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(21, MinimumLength = 3)]
        [RegularExpressionAttribute(@"([1-9]{1}|[1-9]{1}[0-9]{1}|[1-9]{1}[0-9]{0,2}|[1-9]{1}[0-9]{0,3}|[1-9]{1}[0-9]{0,4}|[1-9]{1}[0-9]{0,5}|[1-9]{1}[0-9]{0,6}|[1-9]{1}[0-9]{0,7}|[1-9]{1}[0-9]{0,8}|[1-9]{1}[0-9]{0,9})/([1-9]{1}|[1-9]{1}[0-9]{1}|[1-9]{1}[0-9]{0,2}|[1-9]{1}[0-9]{0,3}|[1-9]{1}[0-9]{0,4}|[1-9]{1}[0-9]{0,5}|[1-9]{1}[0-9]{0,6}|[1-9]{1}[0-9]{0,7}|[1-9]{1}[0-9]{0,8}|[1-9]{1}[0-9]{0,9})")]
        public string ДолСт { get; set; }
        /// <summary>
        /// Код налоговой льготы (установленной  в виде понижения налоговой ставки)
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("2012400/(............)")]
        public string КодЛгПНС { get; set; }
        /// <summary>
        /// Налоговая ставка (%)
        /// </summary>
        [XmlAttribute]
        public decimal НалСтав { get; set; }
        /// <summary>
        /// Кжд
        /// </summary>
        [XmlAttribute]
        public decimal Кжд { get; set; }
        [XmlIgnore]
        public bool КждSpecified { get; set; }
        /// <summary>
        /// Сумма авансового платежа
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СумАвИсчисл { get; set; }
        /// <summary>
        /// Код налоговой льготы (в виде уменьшения суммы налога, подлежащей уплате в бюджет)
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("2012500/(............)")]
        public string КодЛгУмен { get; set; }
        /// <summary>
        /// Сумма налоговой льготы  по авансовому платежу, уменьшающей сумму авансового платежа по налогу, подлежащую уплате в бюджет
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СумЛгУмен { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств по состоянию на 01.04, 01.07, 01.10
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СтОстВс { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументСумНалПУРасОбВидИмущ
    {
        [XmlEnumAttribute("01")]
        Item01,
        [XmlEnumAttribute("02")]
        Item02,
        [XmlEnumAttribute("03")]
        Item03,
        [XmlEnumAttribute("04")]
        Item04,
        [XmlEnumAttribute("05")]
        Item05,
        [XmlEnumAttribute("08")]
        Item08,
        [XmlEnumAttribute("09")]
        Item09,
        [XmlEnumAttribute("10")]
        Item10,
    }

    /// <summary>
    /// Информация об объекте недвижимого имущества
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСумНалПУИнфОбъект
    {
        /// <summary>
        /// Кадастровый номер
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(100, MinimumLength = 1)]
        public string КадастНом { get; set; }
        /// <summary>
        /// Условный номер
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(100, MinimumLength = 1)]
        public string УсловНом { get; set; }
        /// <summary>
        /// Инвентарный номер
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(100, MinimumLength = 1)]
        public string ИнвентНом { get; set; }
        /// <summary>
        /// Код ОКОФ
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(16, MinimumLength = 5)]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{1}")]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{2}")]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{2}\\.[0-9]{1}")]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{2}\\.[0-9]{2}")]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{2}\\.[0-9]{2}\\.[0-9]{1}")]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}")]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}\\.[0-9]{1}")]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}")]
        [MultipleRegularExpressionAttribute("[0-9]{3}\\.[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}\\.[0-9]{3}")]
        public string ОКОФ { get; set; }
        /// <summary>
        /// Остаточная стоимость основных средств по состоянию на 01.04, 01.07, 01.10
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СтОстВс { get; set; }
    }

    /// <summary>
    /// Сумма авансового платежа по налогу, подлежащая уплате в бюджет, по данным налогоплательщика
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСумНалПУРасОб1
    {
        [XmlAttribute]
        public ФайлДокументСумНалПУРасОбВидИмущ1 ВидИмущ { get; set; }
        [XmlAttribute]
        public string НомКадЗдан { get; set; }
        [XmlAttribute]
        public string НомКадПом { get; set; }
        [XmlAttribute(DataType = "integer")]
        public string СтКадастр { get; set; }
        [XmlAttribute(DataType = "integer")]
        public string СтКадастрНеобл { get; set; }
        [XmlAttribute]
        public string ДоляПравСоб { get; set; }
        [XmlAttribute("Доля_6.378.2")]
        public string Доля_63782 { get; set; }
        [XmlAttribute]
        public string КодНалЛьг { get; set; }
        [XmlAttribute]
        public string ДолСт { get; set; }
        [XmlAttribute]
        public string КодЛгПНС { get; set; }
        [XmlAttribute]
        public decimal НалСтав { get; set; }
        [XmlAttribute]
        public string КоэфК { get; set; }
        [XmlAttribute(DataType = "integer")]
        public string СумАвИсчисл { get; set; }
        [XmlAttribute]
        public string КодЛгУмен { get; set; }
        [XmlAttribute(DataType = "integer")]
        public string СумЛгУмен { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументСумНалПУРасОбВидИмущ1
    {
        [XmlEnumAttribute("11")]
        Item11,
        [XmlEnumAttribute("12")]
        Item12,
        [XmlEnumAttribute("13")]
        Item13,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументКНД
    {
        [XmlEnumAttribute("1152028")]
        Item1152028,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументПериод
    {
        [XmlEnumAttribute("21")]
        Item21,
        [XmlEnumAttribute("17")]
        Item17,
        [XmlEnumAttribute("18")]
        Item18,
        [XmlEnumAttribute("51")]
        Item51,
        [XmlEnumAttribute("47")]
        Item47,
        [XmlEnumAttribute("48")]
        Item48,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументПоМесту
    {
        [XmlEnumAttribute("213")]
        Item213,
        [XmlEnumAttribute("214")]
        Item214,
        [XmlEnumAttribute("215")]
        Item215,
        [XmlEnumAttribute("216")]
        Item216,
        [XmlEnumAttribute("221")]
        Item221,
        [XmlEnumAttribute("245")]
        Item245,
        [XmlEnumAttribute("281")]
        Item281,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлВерсФорм
    {
        [XmlEnumAttribute("5.04")]
        Item504,
    }
}
#pragma warning restore
