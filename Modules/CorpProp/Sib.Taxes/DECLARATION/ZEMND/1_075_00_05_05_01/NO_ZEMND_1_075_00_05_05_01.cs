// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 4.2.0.72
//    <NameSpace>Sib.Taxes</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><DataMemberNameArg>Never</DataMemberNameArg><DataMemberOnXmlIgnore>False</DataMemberOnXmlIgnore><CodeBaseTag>Net45</CodeBaseTag><InitializeFields>Collections</InitializeFields><GenerateUnusedComplexTypes>False</GenerateUnusedComplexTypes><GenerateUnusedSimpleTypes>False</GenerateUnusedSimpleTypes><GenerateXMLAttributes>True</GenerateXMLAttributes><OrderXMLAttrib>True</OrderXMLAttrib><EnableLazyLoading>False</EnableLazyLoading><VirtualProp>False</VirtualProp><PascalCase>False</PascalCase><AutomaticProperties>True</AutomaticProperties><PropNameSpecified>Default</PropNameSpecified><PrivateFieldName>StartWithUnderscore</PrivateFieldName><PrivateFieldNamePrefix></PrivateFieldNamePrefix><EnableRestriction>True</EnableRestriction><RestrictionMaxLenght>True</RestrictionMaxLenght><RestrictionRegEx>True</RestrictionRegEx><RestrictionRange>True</RestrictionRange><ValidateProperty>False</ValidateProperty><ClassNamePrefix></ClassNamePrefix><ClassLevel>Public</ClassLevel><PartialClass>False</PartialClass><ClassesInSeparateFiles>False</ClassesInSeparateFiles><ClassesInSeparateFilesDir></ClassesInSeparateFilesDir><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><EnableAppInfoSettings>True</EnableAppInfoSettings><EnableExternalSchemasCache>False</EnableExternalSchemasCache><EnableDebug>False</EnableDebug><EnableWarn>False</EnableWarn><ExcludeImportedTypes>False</ExcludeImportedTypes><ExpandNesteadAttributeGroup>False</ExpandNesteadAttributeGroup><CleanupCode>True</CleanupCode><EnableXmlSerialization>False</EnableXmlSerialization><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><EnableEncoding>False</EnableEncoding><EnableXMLIndent>False</EnableXMLIndent><IndentChar>Indent2Space</IndentChar><NewLineAttr>False</NewLineAttr><OmitXML>False</OmitXML><Encoder>UTF8</Encoder><Serializer>XmlSerializer</Serializer><sspNullable>False</sspNullable><sspString>False</sspString><sspCollection>False</sspCollection><sspComplexType>False</sspComplexType><sspSimpleType>False</sspSimpleType><sspEnumType>False</sspEnumType><XmlSerializerEvent>False</XmlSerializerEvent><BaseClassName>EntityBase</BaseClassName><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><CustomUsings></CustomUsings><AttributesToExlude></AttributesToExlude>
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace Sib.Taxes.Declaration.ZEMND.V1_075_00_05_05_01
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
    [ShemaPathAttribute(@"Sib.Taxes.DECLARATION.ZEMND._1_075_00_05_05_01.NO_ZEMND_1_075_00_05_05_01.xsd")]    
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
        /// Налоговая декларация по земельному налогу
        /// </summary>
        [XmlElement(Order = 2)]
        public ФайлДокументЗемНалНД ЗемНалНД { get; set; }
        /// <summary>
        /// Код формы отчетности по КНД
        /// </summary>
        [XmlAttribute]
        public ФайлДокументКНД КНД { get; set; }
        /// <summary>
        /// Дата формирования документа
        /// </summary>
        /// <summary>
        /// Дата в формате ДД.ММ.ГГГГ
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("(((0[1-9]{1}|[1-2]{1}[0-9]{1})\\.(0[1-9]{1}|1[0-2]{1}))|((30)\\.(01|0[3-9]{1}|1[0-2" +
    "]{1}))|((31)\\.(01|03|05|07|08|10|12)))\\.(19[0-9]{2}|20[0-9]{2})")]
        public string ДатаДок { get; set; }
        /// <summary>
        /// Налоговый период (код)
        /// </summary>
        [XmlAttribute]
        public ФайлДокументПериод Период { get; set; }
        /// <summary>
        /// Отчетный год
        /// </summary>
        /// <summary>
        /// Год в формате ГГГГ
        /// </summary>
        [XmlAttribute(DataType = "gYear")]
        public string ОтчетГод { get; set; }
        /// <summary>
        /// Код налогового органа, в который представляется документ
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("[0-9]{4}")]
        public string КодНО { get; set; }
        /// <summary>
        /// Номер корректировки
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string НомКорр { get; set; }
        /// <summary>
        /// Код места нахождения (учета), по которому представляется документ
        /// </summary>
        [XmlAttribute]
        public ФайлДокументПоМесту ПоМесту { get; set; }
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
        /// Налогоплательщик - юридическое лицо
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
    /// Налогоплательщик - юридическое лицо
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСвНПНПЮЛ
    {
        /// <summary>
        /// Сведения о реорганизованной (ликвидированной) организации
        /// </summary>
        [XmlElement(Order = 0)]
        public ФайлДокументСвНПНПЮЛСвРеоргЮЛ СвРеоргЮЛ { get; set; }
        /// <summary>
        /// Полное наименование организации
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
        [RegularExpressionAttribute("([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})([0-9]{2})(0[0-7A-Z]{1}|1[0-9A-Q]{1}|2[0-9A-I]" +
    "{1}|3[0-9A-Y]{1}|4[0-58-9A-Z]{1}|5[1-9A-Z]{1}|[B-Z]{1}[0-9A-Z]{1}|[6-9]{1}[0-9A-" +
    "Z]{1})([0-9]{3})")]
        public string КПП { get; set; }
    }

    /// <summary>
    /// Сведения о реорганизованной (ликвидированной) организации
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументСвНПНПЮЛСвРеоргЮЛ
    {
        /// <summary>
        /// Код формы реорганизации (ликвидация)
        /// </summary>
        /// <summary>
        /// Принимает значение:
        /// 0 – ликвидация   |
        /// 1 – преобразование   |
        /// 2 – слияние   |
        /// 3 – разделение   |
        /// 5 – присоединение   |
        /// 6 – разделение с одновременным присоединением
        /// </summary>
        [XmlAttribute]
        public ФайлДокументСвНПНПЮЛСвРеоргЮЛФормРеорг ФормРеорг { get; set; }
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
        [RegularExpressionAttribute("([0-9]{1}[1-9]{1}|[1-9]{1}[0-9]{1})([0-9]{2})(0[0-7A-Z]{1}|1[0-9A-Q]{1}|2[0-9A-I]" +
    "{1}|3[0-9A-Y]{1}|4[0-58-9A-Z]{1}|5[1-9A-Z]{1}|[B-Z]{1}[0-9A-Z]{1}|[6-9]{1}[0-9A-" +
    "Z]{1})([0-9]{3})")]
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
        [XmlEnumAttribute("0")]
        Item0,
    }

    /// <summary>
    /// Сведения по сумме налоговой льготы
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public class СумЛьгот
    {
        /// <summary>
        /// Код налоговой льготы
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("[0-9]{7}")]
        public string КодНалЛьгот { get; set; }
        /// <summary>
        /// Сумма налоговой льготы
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СумЛьг { get; set; }
    }

    /// <summary>
    /// Сведения по сумме налоговой льготы (с указанием основания льготы)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public class СумЛьгот2
    {
        /// <summary>
        /// Код налоговой льготы
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("([0-9]{7})/(............)")]
        public string КодНалЛьгот { get; set; }
        /// <summary>
        /// Сумма налоговой льготы
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СумЛьг { get; set; }
    }

    /// <summary>
    /// Сведения по не облагаемой налогом сумме (с указанием основания налоговой льготы)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public class СумНеОбл
    {
        /// <summary>
        /// Код налоговой льготы
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("([0-9]{7})/(............)")]
        public string КодНалЛьгот { get; set; }
        [XmlAttribute("СумНеОбл", DataType = "integer")]
        public string СумНеОбл1 { get; set; }
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
        /// Обязательно для «ПрПодп»=2
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
    /// Обязательно для «ПрПодп»=2
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
    /// Налоговая декларация по земельному налогу
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументЗемНалНД
    {
        /// <summary>
        /// Сумма земельного налога, подлежащая уплате в бюджет
        /// </summary>
        [XmlElement("СумПУ", Order = 0)]
        public List<ФайлДокументЗемНалНДСумПУ> СумПУ { get; set; }
        /// <summary>
        /// Наименование соглашения о разделе продукции (для участков недр, предоставленных в пользование на условиях СРП)
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(160, MinimumLength = 1)]
        public string НаимСРП { get; set; }

        /// <summary>
        /// ФайлДокументЗемНалНД class constructor
        /// </summary>
        public ФайлДокументЗемНалНД()
        {
            СумПУ = new List<ФайлДокументЗемНалНДСумПУ>();
        }
    }

    /// <summary>
    /// Сумма земельного налога, подлежащая уплате в бюджет
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументЗемНалНДСумПУ
    {
        /// <summary>
        /// Расчет налоговой базы и суммы земельного налога
        /// </summary>
        [XmlElement("РасчПлатЗН", Order = 0)]
        public List<ФайлДокументЗемНалНДСумПУРасчПлатЗН> РасчПлатЗН { get; set; }
        /// <summary>
        /// Код бюджетной классификации
        /// </summary>
        [XmlAttribute]
        public string КБК { get; set; }
        /// <summary>
        /// Код по ОКТМО
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(11, MinimumLength = 8)]
        [MultipleRegularExpressionAttribute("[0-9]{8}")]
        [MultipleRegularExpressionAttribute("[0-9]{11}")]
        public string ОКТМО { get; set; }
        /// <summary>
        /// Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string НалИсчисл { get; set; }
        /// <summary>
        /// В том числе сумма авансовых платежей, подлежащая уплате в бюджет за первый квартал
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string АвПУКв1 { get; set; }
        /// <summary>
        /// В том числе сумма авансовых платежей, подлежащая уплате в бюджет за второй квартал
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string АвПУКв2 { get; set; }
        /// <summary>
        /// В том числе сумма авансовых платежей, подлежащая уплате в бюджет за третий квартал
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string АвПУКв3 { get; set; }
        /// <summary>
        /// Сумма налога, подлежащая уплате в бюджет (Сумма налога, исчисленная к уменьшению)
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string НалПУ { get; set; }

        /// <summary>
        /// ФайлДокументЗемНалНДСумПУ class constructor
        /// </summary>
        public ФайлДокументЗемНалНДСумПУ()
        {
            РасчПлатЗН = new List<ФайлДокументЗемНалНДСумПУРасчПлатЗН>();
        }
    }

    /// <summary>
    /// Расчет налоговой базы и суммы земельного налога
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументЗемНалНДСумПУРасчПлатЗН
    {
        private System.Nullable<ФайлДокументЗемНалНДСумПУРасчПлатЗНПерСтр> _перСтр;
        /// <summary>
        /// Определение налоговой базы
        /// </summary>
        [XmlElement(Order = 0)]
        public ФайлДокументЗемНалНДСумПУРасчПлатЗНОпрНалБаза ОпрНалБаза { get; set; }
        /// <summary>
        /// Исчисление суммы земельного налога
        /// </summary>
        [XmlElement(Order = 1)]
        public ФайлДокументЗемНалНДСумПУРасчПлатЗНСумНалИсчисл СумНалИсчисл { get; set; }
        /// <summary>
        /// Кадастровый номер земельного участка
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(100, MinimumLength = 1)]
        public string НомКадастрЗУ { get; set; }
        /// <summary>
        /// Категория земель (код)
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("[0-9]{12}")]
        public string КатегорЗем { get; set; }
        /// <summary>
        /// Кадастровая стоимость (доля кадастровой стоимости) земельного участка / Нормативная цена земли
        /// </summary>
        [XmlAttribute]
        public decimal СтКадастрЗУ { get; set; }
        /// <summary>
        /// Доля налогоплательщика в праве на земельный
        /// участок
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(21, MinimumLength = 3)]
        [RegularExpressionAttribute(@"([1-9]{1}|[1-9]{1}[0-9]{1}|[1-9]{1}[0-9]{0,2}|[1-9]{1}[0-9]{0,3}|[1-9]{1}[0-9]{0,4}|[1-9]{1}[0-9]{0,5}|[1-9]{1}[0-9]{0,6}|[1-9]{1}[0-9]{0,7}|[1-9]{1}[0-9]{0,8}|[1-9]{1}[0-9]{0,9})/([1-9]{1}|[1-9]{1}[0-9]{1}|[1-9]{1}[0-9]{0,2}|[1-9]{1}[0-9]{0,3}|[1-9]{1}[0-9]{0,4}|[1-9]{1}[0-9]{0,5}|[1-9]{1}[0-9]{0,6}|[1-9]{1}[0-9]{0,7}|[1-9]{1}[0-9]{0,8}|[1-9]{1}[0-9]{0,9})")]
        public string ДоляЗУ { get; set; }
        /// <summary>
        /// Налоговая ставка (%)
        /// </summary>
        [XmlAttribute]
        public decimal НалСтав { get; set; }

        /// <summary>
        /// Период строительства
        /// </summary>
        [XmlAttribute]
        public ФайлДокументЗемНалНДСумПУРасчПлатЗНПерСтр ПерСтр
        {
            get
            {
                if (_перСтр.HasValue)
                {
                    return _перСтр.Value;
                }
                else
                {
                    return default(ФайлДокументЗемНалНДСумПУРасчПлатЗНПерСтр);
                }
            }
            set
            {
                _перСтр = value;
            }
        }

        [XmlIgnore]
        public bool ПерСтрSpecified
        {
            get
            {
                return _перСтр.HasValue;
            }
            set
            {
                if (value == false)
                {
                    _перСтр = null;
                }
            }
        }
    }

    /// <summary>
    /// Определение налоговой базы
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументЗемНалНДСумПУРасчПлатЗНОпрНалБаза
    {
        /// <summary>
        /// Налоговая льгота в виде не облагаемой налогом суммы (пункт 2 статьи 387 Налогового кодекса Российской Федерации)
        /// </summary>
        [XmlElement(Order = 0)]
        public СумНеОбл Льгот387_2Сум { get; set; }
        /// <summary>
        /// Налоговая льгота в виде доли необлагаемой площади земельного участка (пункт 2 статьи 387 Налогового кодекса Российской Федерации)
        /// </summary>
        [XmlElement(Order = 1)]
        public ФайлДокументЗемНалНДСумПУРасчПлатЗНОпрНалБазаЛьгот387_2Пл Льгот387_2Пл { get; set; }
        /// <summary>
        /// Налоговая база
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string НалБаза { get; set; }
    }

    /// <summary>
    /// Налоговая льгота в виде доли необлагаемой площади земельного участка (пункт 2 статьи 387 Налогового кодекса Российской Федерации)
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументЗемНалНДСумПУРасчПлатЗНОпрНалБазаЛьгот387_2Пл
    {
        /// <summary>
        /// Код налоговой льготы
        /// </summary>
        [XmlAttribute]
        [RegularExpressionAttribute("([0-9]{7})/(............)")]
        public string КодНалЛьгот { get; set; }
        /// <summary>
        /// Доля необлагаемой площади земельного участка
        /// </summary>
        [XmlAttribute]
        [StringLengthAttribute(21, MinimumLength = 3)]
        [RegularExpressionAttribute(@"([1-9]{1}|[1-9]{1}[0-9]{1}|[1-9]{1}[0-9]{0,2}|[1-9]{1}[0-9]{0,3}|[1-9]{1}[0-9]{0,4}|[1-9]{1}[0-9]{0,5}|[1-9]{1}[0-9]{0,6}|[1-9]{1}[0-9]{0,7}|[1-9]{1}[0-9]{0,8}|[1-9]{1}[0-9]{0,9})/([1-9]{1}|[1-9]{1}[0-9]{1}|[1-9]{1}[0-9]{0,2}|[1-9]{1}[0-9]{0,3}|[1-9]{1}[0-9]{0,4}|[1-9]{1}[0-9]{0,5}|[1-9]{1}[0-9]{0,6}|[1-9]{1}[0-9]{0,7}|[1-9]{1}[0-9]{0,8}|[1-9]{1}[0-9]{0,9})")]
        public string ДоляПлЗУ { get; set; }
    }

    /// <summary>
    /// Исчисление суммы земельного налога
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public class ФайлДокументЗемНалНДСумПУРасчПлатЗНСумНалИсчисл
    {
        /// <summary>
        /// Налоговая льгота в виде освобождения от налогообложения (пункт 2 статьи 387 Налогового кодекса Российской Федерации)
        /// </summary>
        [XmlElement(Order = 0)]
        public СумЛьгот2 Льгот387_2Осв { get; set; }
        /// <summary>
        /// Налоговая льгота в виде освобождения от налогообложения (статья 395, статья 7 Налогового кодекса Российской Федерации)
        /// </summary>
        [XmlElement(Order = 1)]
        public СумЛьгот Льгот395 { get; set; }
        /// <summary>
        /// Налоговая льгота в виде уменьшения суммы налога (пункт 2 статьи 387 Налогового кодекса Российской Федерации)
        /// </summary>
        [XmlElement(Order = 2)]
        public СумЛьгот2 Льгот387_2УмСум { get; set; }
        /// <summary>
        /// Код налоговой льготы в виде снижения налоговой ставки
        /// </summary>
        [XmlElement(Order = 3)]
        [RegularExpressionAttribute("([0-9]{7})/(............)")]
        public string ЛьготСнСтав { get; set; }
        /// <summary>
        /// Количество полных месяцев владения земельным участком в течение налогового периода
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string КолМесВлЗУ { get; set; }
        /// <summary>
        /// Коэффициент Кв
        /// </summary>
        [XmlAttribute]
        public decimal Кв { get; set; }
        /// <summary>
        /// Коэффициент Ки
        /// </summary>
        [XmlAttribute]
        public decimal Ки { get; set; }
        [XmlIgnore]
        public bool КиSpecified { get; set; }
        /// <summary>
        /// Сумма исчисленного налога
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СумНалИсчисл { get; set; }
        /// <summary>
        /// Количество полных месяцев использования  льготы
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string КолМесЛьгот { get; set; }
        /// <summary>
        /// Коэффициент Кл
        /// </summary>
        [XmlAttribute]
        public decimal Кл { get; set; }
        /// <summary>
        /// Исчисленная сумма налога, подлежащая уплате в бюджет за налоговый период
        /// </summary>
        [XmlAttribute(DataType = "integer")]
        public string СумНалУплат { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументЗемНалНДСумПУРасчПлатЗНПерСтр
    {
        [XmlEnumAttribute("1")]
        Item1,
        [XmlEnumAttribute("2")]
        Item2,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументКНД
    {
        [XmlEnumAttribute("1153005")]
        Item1153005,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументПериод
    {
        [XmlEnumAttribute("34")]
        Item34,
        [XmlEnumAttribute("50")]
        Item50,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлДокументПоМесту
    {
        [XmlEnumAttribute("213")]
        Item213,
        [XmlEnumAttribute("216")]
        Item216,
        [XmlEnumAttribute("250")]
        Item250,
        [XmlEnumAttribute("251")]
        Item251,
        [XmlEnumAttribute("270")]
        Item270,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ФайлВерсФорм
    {
        [XmlEnumAttribute("5.05")]
        Item505,
    }
}
#pragma warning restore
