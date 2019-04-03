#pragma warning disable
namespace SibRosReestr.EGRP.V04.ExtractSubj
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
    /// РџР°РєРµС‚ РёРЅС„РѕСЂРјР°С†РёРё - РѕС‚РІРµС‚ РЅР° Р·Р°РїСЂРѕСЃ СЃРІРµРґРµРЅРёР№ Р•Р“Р Рџ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true)]
    [XmlRootAttribute(Namespace="", IsNullable=false)]
    public class Extract
    {
        /// <summary>
        /// Р­Р»РµРєС‚СЂРѕРЅРЅС‹Р№ РґРѕРєСѓРјРµРЅС‚
        /// </summary>
        [XmlElement(Order=0, ElementName="eDocument")]
        public TServisInf EDocument { get; set; }
        /// <summary>
        /// Р’С‹РїРёСЃРєР° Рѕ РїСЂР°РІР°С… РѕС‚РґРµР»СЊРЅРѕРіРѕ Р»РёС†Р° РЅР° РћРќР
        /// </summary>
        [XmlElement(Order=1)]
        public ExtractReestrExtract ReestrExtract { get; set; }
    }
    
    /// <summary>
    /// РЎРµСЂРІРёСЃРЅР°СЏ РёРЅС„РѕСЂРјР°С†РёСЏ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tServisInf")]
    public class TServisInf
    {
        /// <summary>
        /// РћС‚РїСЂР°РІРёС‚РµР»СЊ
        /// </summary>
        [XmlElement(Order=0)]
        public TServisInfSender Sender { get; set; }
        /// <summary>
        /// РџРѕР»СѓС‡Р°С‚РµР»СЊ
        /// </summary>
        [XmlElement(Order=1)]
        public TServisInfRecipient Recipient { get; set; }
        /// <summary>
        /// РўРёРї РїРµСЂРµРґР°РІР°РµРјРѕР№ РёРЅС„РѕСЂРјР°С†РёРё
        /// </summary>
        [XmlAttribute(AttributeName="CodeType")]
        public string CodeType { get; set; }
        /// <summary>
        /// Р’РµСЂСЃРёСЏ СЃС…РµРјС‹
        /// </summary>
        [XmlAttribute(AttributeName="Version")]
        public string Version { get; set; }
        /// <summary>
        /// РўРёРї СѓС‡РµС‚РЅРѕР№ СЃРёСЃС‚РµРјС‹
        /// </summary>
        [XmlAttribute(AttributeName="Scope")]
        [StringLengthAttribute(10)]
        public TServisInfScope Scope { get; set; }
        
        /// <summary>
        /// TServisInf class constructor
        /// </summary>
        public TServisInf()
        {
            Version = "04";
        }
    }
    
    /// <summary>
    /// РћС‚РїСЂР°РІРёС‚РµР»СЊ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tServisInfSender")]
    [XmlRootAttribute("tServisInfSender")]
    public class TServisInfSender
    {
        /// <summary>
        /// РљРѕРґ
        /// </summary>
        [XmlAttribute(AttributeName="Kod")]
        [StringLengthAttribute(12)]
        public string Kod { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// Р РµРіРёРѕРЅ
        /// </summary>
        [XmlAttribute(AttributeName="Region")]
        public DRegionsRF Region { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РІС‹РіСЂСѓР·РєРё
        /// </summary>
        [XmlAttribute(DataType="date", AttributeName="Date_Upload")]
        public System.DateTime Date_Upload { get; set; }
        /// <summary>
        /// Р”РѕР»Р¶РЅРѕСЃС‚СЊ
        /// </summary>
        [XmlAttribute(AttributeName="Appointment")]
        [StringLengthAttribute(250)]
        public string Appointment { get; set; }
        /// <summary>
        /// Р¤РРћ
        /// </summary>
        [XmlAttribute(AttributeName="FIO")]
        [StringLengthAttribute(100)]
        public string FIO { get; set; }
        /// <summary>
        /// РђРґСЂРµСЃ СЌР»РµРєС‚СЂРѕРЅРЅРѕР№ РїРѕС‡С‚С‹ РѕС‚РїСЂР°РІРёС‚РµР»СЏ
        /// </summary>
        [XmlAttribute(AttributeName="E_Mail")]
        [StringLengthAttribute(60)]
        public string E_Mail { get; set; }
        /// <summary>
        /// РўРµР»РµС„РѕРЅ
        /// </summary>
        [XmlAttribute(AttributeName="Telephone")]
        [StringLengthAttribute(100)]
        public string Telephone { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dRegionsRF")]
    public enum DRegionsRF
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
        [XmlEnumAttribute("06")]
        Item06,
        [XmlEnumAttribute("07")]
        Item07,
        [XmlEnumAttribute("08")]
        Item08,
        [XmlEnumAttribute("09")]
        Item09,
        [XmlEnumAttribute("10")]
        Item10,
        [XmlEnumAttribute("11")]
        Item11,
        [XmlEnumAttribute("12")]
        Item12,
        [XmlEnumAttribute("13")]
        Item13,
        [XmlEnumAttribute("14")]
        Item14,
        [XmlEnumAttribute("15")]
        Item15,
        [XmlEnumAttribute("16")]
        Item16,
        [XmlEnumAttribute("17")]
        Item17,
        [XmlEnumAttribute("18")]
        Item18,
        [XmlEnumAttribute("19")]
        Item19,
        [XmlEnumAttribute("20")]
        Item20,
        [XmlEnumAttribute("21")]
        Item21,
        [XmlEnumAttribute("22")]
        Item22,
        [XmlEnumAttribute("23")]
        Item23,
        [XmlEnumAttribute("24")]
        Item24,
        [XmlEnumAttribute("25")]
        Item25,
        [XmlEnumAttribute("26")]
        Item26,
        [XmlEnumAttribute("27")]
        Item27,
        [XmlEnumAttribute("28")]
        Item28,
        [XmlEnumAttribute("29")]
        Item29,
        [XmlEnumAttribute("30")]
        Item30,
        [XmlEnumAttribute("31")]
        Item31,
        [XmlEnumAttribute("32")]
        Item32,
        [XmlEnumAttribute("33")]
        Item33,
        [XmlEnumAttribute("34")]
        Item34,
        [XmlEnumAttribute("35")]
        Item35,
        [XmlEnumAttribute("36")]
        Item36,
        [XmlEnumAttribute("37")]
        Item37,
        [XmlEnumAttribute("38")]
        Item38,
        [XmlEnumAttribute("39")]
        Item39,
        [XmlEnumAttribute("40")]
        Item40,
        [XmlEnumAttribute("41")]
        Item41,
        [XmlEnumAttribute("42")]
        Item42,
        [XmlEnumAttribute("43")]
        Item43,
        [XmlEnumAttribute("44")]
        Item44,
        [XmlEnumAttribute("45")]
        Item45,
        [XmlEnumAttribute("46")]
        Item46,
        [XmlEnumAttribute("47")]
        Item47,
        [XmlEnumAttribute("48")]
        Item48,
        [XmlEnumAttribute("49")]
        Item49,
        [XmlEnumAttribute("50")]
        Item50,
        [XmlEnumAttribute("51")]
        Item51,
        [XmlEnumAttribute("52")]
        Item52,
        [XmlEnumAttribute("53")]
        Item53,
        [XmlEnumAttribute("54")]
        Item54,
        [XmlEnumAttribute("55")]
        Item55,
        [XmlEnumAttribute("56")]
        Item56,
        [XmlEnumAttribute("57")]
        Item57,
        [XmlEnumAttribute("58")]
        Item58,
        [XmlEnumAttribute("59")]
        Item59,
        [XmlEnumAttribute("60")]
        Item60,
        [XmlEnumAttribute("61")]
        Item61,
        [XmlEnumAttribute("62")]
        Item62,
        [XmlEnumAttribute("63")]
        Item63,
        [XmlEnumAttribute("64")]
        Item64,
        [XmlEnumAttribute("65")]
        Item65,
        [XmlEnumAttribute("66")]
        Item66,
        [XmlEnumAttribute("67")]
        Item67,
        [XmlEnumAttribute("68")]
        Item68,
        [XmlEnumAttribute("69")]
        Item69,
        [XmlEnumAttribute("70")]
        Item70,
        [XmlEnumAttribute("71")]
        Item71,
        [XmlEnumAttribute("72")]
        Item72,
        [XmlEnumAttribute("73")]
        Item73,
        [XmlEnumAttribute("74")]
        Item74,
        [XmlEnumAttribute("75")]
        Item75,
        [XmlEnumAttribute("76")]
        Item76,
        [XmlEnumAttribute("77")]
        Item77,
        [XmlEnumAttribute("78")]
        Item78,
        [XmlEnumAttribute("79")]
        Item79,
        [XmlEnumAttribute("80")]
        Item80,
        [XmlEnumAttribute("81")]
        Item81,
        [XmlEnumAttribute("82")]
        Item82,
        [XmlEnumAttribute("83")]
        Item83,
        [XmlEnumAttribute("84")]
        Item84,
        [XmlEnumAttribute("85")]
        Item85,
        [XmlEnumAttribute("86")]
        Item86,
        [XmlEnumAttribute("87")]
        Item87,
        [XmlEnumAttribute("88")]
        Item88,
        [XmlEnumAttribute("89")]
        Item89,
        [XmlEnumAttribute("98")]
        Item98,
        [XmlEnumAttribute("99")]
        Item99,
    }
    
    /// <summary>
    /// РЈРІРµРґРѕРјР»РµРЅРёРµ РѕР± РѕС‚СЃСѓС‚СЃС‚РІРёРё СЃРІРµРґРµРЅРёР№ РѕР± РѕР±СЉРµРєС‚РµСЉ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tNoticeObj")]
    public class TNoticeObj
    {
        /// <summary>
        /// РўРµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlElement(Order=0)]
        public THeadContent HeadContent { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚ СѓРІРµРґРѕРјР»РµРЅРёСЏ
        /// </summary>
        [XmlElement("NoticeObj", Order=1)]
        public List<TNoticeObjNoticeObj> NoticeObj { get; set; }
        /// <summary>
        /// Р—Р°РІРµСЂС€Р°СЋС‰РёР№ С‚РµРєСЃС‚ (СЃСЃС‹Р»РєР° РЅР° Р·Р°РєРѕРЅ Рё С‚.Рї.)
        /// </summary>
        [XmlElement(Order=2)]
        public TFootContent FootContent { get; set; }
        
        /// <summary>
        /// TNoticeObj class constructor
        /// </summary>
        public TNoticeObj()
        {
            NoticeObj = new List<TNoticeObjNoticeObj>();
        }
    }
    
    /// <summary>
    /// РїСЂРѕР»РѕРі РІС‹РїРёСЃРєРё
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tHeadContent")]
    public class THeadContent
    {
        /// <summary>
        /// РЈРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ РґРѕРєСѓРјРµРЅС‚Р° РІ Р·Р°РїРёСЃРё РљРЈР’Р
        /// </summary>
        [XmlElement(Order=0)]
        public object ID_REC_KUVI { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ СЃР»СѓР¶Р±С‹
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(100)]
        public string Title { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ С‚РµСЂСЂРёС‚РѕСЂРёР°Р»СЊРЅРѕРіРѕ СѓРїСЂР°РІР»РµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(500)]
        public string DeptName { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(500)]
        public string ExtractTitle { get; set; }
        /// <summary>
        /// РЎСѓРјРјР°СЂРЅРѕРµ  РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(2000)]
        public string Content { get; set; }
    }
    
    /// <summary>
    /// С‚РµРєСЃС‚ СѓРІРµРґРѕРјР»РµРЅРёСЏ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tNoticeObjNoticeObj")]
    [XmlRootAttribute("tNoticeObjNoticeObj")]
    public class TNoticeObjNoticeObj
    {
        /// <summary>
        /// Р’РёРґ Р·Р°РїСЂРѕС€РµРЅРЅРѕР№ РёРЅС„РѕСЂРјР°С†РёРё
        /// </summary>
        [XmlElement(Order=0)]
        public object TypeInfoText { get; set; }
        [XmlElement("ObjectDetail", typeof(TNoticeObjNoticeObjObjectDetail), Order=1)]
        [XmlElement("ObjectInfo", typeof(object), Order=1)]
        public object Item { get; set; }
        /// <summary>
        /// РїСЂР°РІРѕРїСЂРёС‚СЏР·Р°РЅРёСЏ
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(4000)]
        public string RightAssert { get; set; }
        /// <summary>
        /// РїСЂР°РІРѕ С‚СЂРµР±РѕРІР°РЅРёСЏ
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(4000)]
        public string ClaimArrests { get; set; }
    }
    
    /// <summary>
    /// СЃС‚СЂСѓРєС‚СѓСЂРёСЂРѕРІР°РЅРЅРѕРµ РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕС€РµРЅРЅРѕРіРѕ РѕР±СЉРµРєС‚Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tNoticeObjNoticeObjObjectDetail")]
    [XmlRootAttribute("tNoticeObjNoticeObjObjectDetail")]
    public class TNoticeObjNoticeObjObjectDetail
    {
        [XmlElement("CadastralNumber", Order=0)]
        [XmlElement("ConditionalNumber", Order=0)]
        [XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item { get; set; }
        [XmlElement(Order=1)]
        [XmlIgnore]
        public ItemChoiceType3 ItemElementName { get; set; }
        /// <summary>
        /// РљРѕРґ С‚РёРїР° РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement(Order=2)]
        public TObjectObjectType ObjectType { get; set; }
        /// <summary>
        /// РўРµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ С‚РёРїР° РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(1000)]
        public string ObjectTypeText { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ  РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё.
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(4000)]
        public string Name { get; set; }
        /// <summary>
        /// Р°РґСЂРµСЃ РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement(Order=5)]
        public TAddress ObjectAddress { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlTypeAttribute(IncludeInSchema=false)]
    [XmlRootAttribute("ItemChoiceType3")]
    public enum ItemChoiceType3
    {
        CadastralNumber,
        ConditionalNumber,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("tObjectObjectType")]
    public enum TObjectObjectType
    {
        [XmlEnumAttribute("002001000000")]
        Item002001000000,
        [XmlEnumAttribute("002001001000")]
        Item002001001000,
        [XmlEnumAttribute("002001002000")]
        Item002001002000,
        [XmlEnumAttribute("002002000000")]
        Item002002000000,
        [XmlEnumAttribute("002002001000")]
        Item002002001000,
        [XmlEnumAttribute("002002002000")]
        Item002002002000,
        [XmlEnumAttribute("002002004000")]
        Item002002004000,
        [XmlEnumAttribute("002002005000")]
        Item002002005000,
        [XmlEnumAttribute("002003000000")]
        Item002003000000,
        [XmlEnumAttribute("002003001000")]
        Item002003001000,
        [XmlEnumAttribute("002003002000")]
        Item002003002000,
        [XmlEnumAttribute("002003003000")]
        Item002003003000,
        [XmlEnumAttribute("002003004000")]
        Item002003004000,
        [XmlEnumAttribute("002004000000")]
        Item002004000000,
        [XmlEnumAttribute("002004001000")]
        Item002004001000,
        [XmlEnumAttribute("002004003000")]
        Item002004003000,
    }
    
    /// <summary>
    /// РџРѕС‡С‚РѕРІС‹Р№ Р°РґСЂРµСЃ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tAddress")]
    public class TAddress
    {
        /// <summary>
        /// РЈРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ Р°РґСЂРµСЃР°
        /// </summary>
        [XmlElement(DataType="integer", Order=0)]
        public string ID_Address { get; set; }
        /// <summary>
        /// РЎСѓРјРјР°СЂРЅРѕРµ РЅРµС„РѕСЂРјР°Р»РёР·РѕРІР°РЅРЅРѕРµ РѕРїРёСЃР°РЅРёРµ
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(4000)]
        public string Content { get; set; }

        [XmlElement("Country", typeof(TAddressCountry), Order=2)]
        [XmlElement("Region", typeof(TAddressRegion), Order=2)]
        public object Item { get; set; }
        /// <summary>
        /// РћРљРђРўРћ
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(11)]
        public string Code_OKATO { get; set; }
        /// <summary>
        /// РљР›РђР”Р 
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(20)]
        public string Code_KLADR { get; set; }
        /// <summary>
        /// РџРѕС‡С‚РѕРІС‹Р№ РёРЅРґРµРєСЃ
        /// </summary>
        [XmlElement(Order=5)]
        public string Postal_Code { get; set; }
        /// <summary>
        /// Р Р°Р№РѕРЅ
        /// </summary>
        [XmlElement(Order=6)]
        public TAddressDistrict District { get; set; }
        /// <summary>
        /// РњСѓРЅРёС†РёРїР°Р»СЊРЅРѕРµ РѕР±СЂР°Р·РѕРІР°РЅРёРµ
        /// </summary>
        [XmlElement(Order=7)]
        public TAddressCity City { get; set; }
        /// <summary>
        /// Р“РѕСЂРѕРґСЃРєРѕР№ СЂР°Р№РѕРЅ
        /// </summary>
        [XmlElement(Order=8)]
        public TAddressUrban_District Urban_District { get; set; }
        /// <summary>
        /// РЎРµР»СЊСЃРѕРІРµС‚
        /// </summary>
        [XmlElement(Order=9)]
        public TAddressSoviet_Village Soviet_Village { get; set; }
        /// <summary>
        /// РќР°СЃРµР»РµРЅРЅС‹Р№ РїСѓРЅРєС‚
        /// </summary>
        [XmlElement(Order=10)]
        public TAddressLocality Locality { get; set; }
        /// <summary>
        /// РЈР»РёС†Р°
        /// </summary>
        [XmlElement(Order=11)]
        public TAddressStreet Street { get; set; }
        /// <summary>
        /// Р”РѕРј
        /// </summary>
        [XmlElement(Order=12)]
        public TAddressLevel1 Level1 { get; set; }
        /// <summary>
        /// РљРѕСЂРїСѓСЃ
        /// </summary>
        [XmlElement(Order=13)]
        public TAddressLevel2 Level2 { get; set; }
        /// <summary>
        /// РЎС‚СЂРѕРµРЅРёРµ
        /// </summary>
        [XmlElement(Order=14)]
        public TAddressLevel3 Level3 { get; set; }
        /// <summary>
        /// РљРІР°СЂС‚РёСЂР°
        /// </summary>
        [XmlElement(Order=15)]
        public TAddressApartment Apartment { get; set; }
        /// <summary>
        /// РРЅРѕРµ
        /// </summary>
        [XmlElement(Order=16)]
        [StringLengthAttribute(2500)]
        public string Other { get; set; }
    }
    
    /// <summary>
    /// РЎС‚СЂР°РЅР° СЂРµРіРёСЃС‚СЂР°С†РёРё
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressCountry")]
    [XmlRootAttribute("tAddressCountry")]
    public class TAddressCountry
    {
        /// <summary>
        /// РџРѕР»РЅРѕРµ РЅР°РёРјРµРЅРѕРІР°РЅРёРµ СЃС‚СЂР°РЅС‹ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РљРѕРґ СЃС‚СЂР°РЅС‹ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlAttribute(AttributeName="Code")]
        [StringLengthAttribute(255)]
        public string Code { get; set; }
    }
    
    /// <summary>
    /// Р РµРіРёРѕРЅ Р Р¤
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressRegion")]
    [XmlRootAttribute("tAddressRegion")]
    public class TAddressRegion
    {
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ СЂРµРіРёРѕРЅР° Р Р¤
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РљРѕРґ СЂРµРіРёРѕРЅР° Р Р¤
        /// </summary>
        [XmlAttribute(AttributeName="Code")]
        public DRegionsRF Code { get; set; }
    }
    
    /// <summary>
    /// Р Р°Р№РѕРЅ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressDistrict")]
    [XmlRootAttribute("tAddressDistrict")]
    public class TAddressDistrict
    {
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        [StringLengthAttribute(255)]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// РњСѓРЅРёС†РёРїР°Р»СЊРЅРѕРµ РѕР±СЂР°Р·РѕРІР°РЅРёРµ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressCity")]
    [XmlRootAttribute("tAddressCity")]
    public class TAddressCity
    {
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        [StringLengthAttribute(255)]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// Р“РѕСЂРѕРґСЃРєРѕР№ СЂР°Р№РѕРЅ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressUrban_District")]
    [XmlRootAttribute("tAddressUrban_District")]
    public class TAddressUrban_District
    {
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// РЎРµР»СЊСЃРѕРІРµС‚
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressSoviet_Village")]
    [XmlRootAttribute("tAddressSoviet_Village")]
    public class TAddressSoviet_Village
    {
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// РќР°СЃРµР»РµРЅРЅС‹Р№ РїСѓРЅРєС‚
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressLocality")]
    [XmlRootAttribute("tAddressLocality")]
    public class TAddressLocality
    {
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        [StringLengthAttribute(255)]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// РЈР»РёС†Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressStreet")]
    [XmlRootAttribute("tAddressStreet")]
    public class TAddressStreet
    {
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        [StringLengthAttribute(255)]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// Р”РѕРј
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressLevel1")]
    [XmlRootAttribute("tAddressLevel1")]
    public class TAddressLevel1
    {
        /// <summary>
        /// Р—РЅР°С‡РµРЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// РљРѕСЂРїСѓСЃ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressLevel2")]
    [XmlRootAttribute("tAddressLevel2")]
    public class TAddressLevel2
    {
        /// <summary>
        /// Р—РЅР°С‡РµРЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// РЎС‚СЂРѕРµРЅРёРµ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressLevel3")]
    [XmlRootAttribute("tAddressLevel3")]
    public class TAddressLevel3
    {
        /// <summary>
        /// Р—РЅР°С‡РµРЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// РљРІР°СЂС‚РёСЂР°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tAddressApartment")]
    [XmlRootAttribute("tAddressApartment")]
    public class TAddressApartment
    {
        /// <summary>
        /// Р—РЅР°С‡РµРЅРёРµ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
        /// <summary>
        /// РўРёРї
        /// </summary>
        [XmlAttribute(AttributeName="Type")]
        public string Type { get; set; }
    }
    
    /// <summary>
    /// СЌРїРёР»РѕРі РІС‹РїРёСЃРєРё
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tFootContent")]
    public class TFootContent
    {
        /// <summary>
        /// РѕРїРёСЃР°РЅРёРµ РїРѕР»СѓС‡Р°С‚РµР»СЏ РёРЅС„РѕСЂРјР°С†РёРё ( "РІС‹РїРёСЃРєР° РІС‹РґР°РЅР° ...")
        /// </summary>
        [XmlElement(Order=0)]
        [StringLengthAttribute(4000)]
        public string Recipient { get; set; }
        /// <summary>
        /// РІС‹РїРёСЃРєР° Рѕ РїСЂР°РІР°С… РЅР° РћРќР
        /// </summary>
        [XmlElement(Order=1)]
        public TFootContentExtractRegion ExtractRegion { get; set; }
        /// <summary>
        /// РґР°С‚Р° РІС‹РїРёСЃРєРё/СЃРїСЂР°РІРєРё/СЃРѕРѕР±С‰РµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=2)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string ExtractDate { get; set; }
        /// <summary>
        /// РЎСѓРјРјР°СЂРЅРѕРµ  РѕРїРёСЃР°РЅРёРµ Р·Р°РІРµСЂС€Р°СЋС‰РµРіРѕ С‚РµРєСЃС‚Р° (СЃСЃС‹Р»РєР° РЅР° Р·Р°РєРѕРЅ Рё С‚.Рї.)
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(4000)]
        public string Content { get; set; }
    }
    
    /// <summary>
    /// РІС‹РїРёСЃРєР° Рѕ РїСЂР°РІР°С… РЅР° РћРќР
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tFootContentExtractRegion")]
    [XmlRootAttribute("tFootContentExtractRegion")]
    public class TFootContentExtractRegion
    {
        [XmlElement("CodRegion", typeof(DDepartments), Order=0)]
        [XmlElement("Region", typeof(string), Order=0)]
        public List<object> Items { get; set; }
        
        /// <summary>
        /// TFootContentExtractRegion class constructor
        /// </summary>
        public TFootContentExtractRegion()
        {
            Items = new List<object>();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dDepartments")]
    public enum DDepartments
    {
        [XmlEnumAttribute("00")]
        Item00,
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
        [XmlEnumAttribute("06")]
        Item06,
        [XmlEnumAttribute("07")]
        Item07,
        [XmlEnumAttribute("08")]
        Item08,
        [XmlEnumAttribute("09")]
        Item09,
        [XmlEnumAttribute("10")]
        Item10,
        [XmlEnumAttribute("11")]
        Item11,
        [XmlEnumAttribute("12")]
        Item12,
        [XmlEnumAttribute("13")]
        Item13,
        [XmlEnumAttribute("14")]
        Item14,
        [XmlEnumAttribute("15")]
        Item15,
        [XmlEnumAttribute("16")]
        Item16,
        [XmlEnumAttribute("17")]
        Item17,
        [XmlEnumAttribute("18")]
        Item18,
        [XmlEnumAttribute("19")]
        Item19,
        [XmlEnumAttribute("20")]
        Item20,
        [XmlEnumAttribute("21")]
        Item21,
        [XmlEnumAttribute("22")]
        Item22,
        [XmlEnumAttribute("23")]
        Item23,
        [XmlEnumAttribute("24")]
        Item24,
        [XmlEnumAttribute("25")]
        Item25,
        [XmlEnumAttribute("26")]
        Item26,
        [XmlEnumAttribute("27")]
        Item27,
        [XmlEnumAttribute("28")]
        Item28,
        [XmlEnumAttribute("29")]
        Item29,
        [XmlEnumAttribute("30")]
        Item30,
        [XmlEnumAttribute("31")]
        Item31,
        [XmlEnumAttribute("32")]
        Item32,
        [XmlEnumAttribute("33")]
        Item33,
        [XmlEnumAttribute("34")]
        Item34,
        [XmlEnumAttribute("35")]
        Item35,
        [XmlEnumAttribute("36")]
        Item36,
        [XmlEnumAttribute("37")]
        Item37,
        [XmlEnumAttribute("38")]
        Item38,
        [XmlEnumAttribute("39")]
        Item39,
        [XmlEnumAttribute("40")]
        Item40,
        [XmlEnumAttribute("41")]
        Item41,
        [XmlEnumAttribute("42")]
        Item42,
        [XmlEnumAttribute("43")]
        Item43,
        [XmlEnumAttribute("44")]
        Item44,
        [XmlEnumAttribute("45")]
        Item45,
        [XmlEnumAttribute("46")]
        Item46,
        [XmlEnumAttribute("47")]
        Item47,
        [XmlEnumAttribute("48")]
        Item48,
        [XmlEnumAttribute("49")]
        Item49,
        [XmlEnumAttribute("50")]
        Item50,
        [XmlEnumAttribute("51")]
        Item51,
        [XmlEnumAttribute("52")]
        Item52,
        [XmlEnumAttribute("53")]
        Item53,
        [XmlEnumAttribute("54")]
        Item54,
        [XmlEnumAttribute("55")]
        Item55,
        [XmlEnumAttribute("56")]
        Item56,
        [XmlEnumAttribute("57")]
        Item57,
        [XmlEnumAttribute("58")]
        Item58,
        [XmlEnumAttribute("59")]
        Item59,
        [XmlEnumAttribute("60")]
        Item60,
        [XmlEnumAttribute("61")]
        Item61,
        [XmlEnumAttribute("62")]
        Item62,
        [XmlEnumAttribute("63")]
        Item63,
        [XmlEnumAttribute("64")]
        Item64,
        [XmlEnumAttribute("65")]
        Item65,
        [XmlEnumAttribute("66")]
        Item66,
        [XmlEnumAttribute("67")]
        Item67,
        [XmlEnumAttribute("68")]
        Item68,
        [XmlEnumAttribute("69")]
        Item69,
        [XmlEnumAttribute("70")]
        Item70,
        [XmlEnumAttribute("71")]
        Item71,
        [XmlEnumAttribute("72")]
        Item72,
        [XmlEnumAttribute("73")]
        Item73,
        [XmlEnumAttribute("74")]
        Item74,
        [XmlEnumAttribute("75")]
        Item75,
        [XmlEnumAttribute("76")]
        Item76,
        [XmlEnumAttribute("77")]
        Item77,
        [XmlEnumAttribute("78")]
        Item78,
        [XmlEnumAttribute("79")]
        Item79,
        [XmlEnumAttribute("86")]
        Item86,
        [XmlEnumAttribute("89")]
        Item89,
        [XmlEnumAttribute("98")]
        Item98,
        [XmlEnumAttribute("99")]
        Item99,
    }
    
    /// <summary>
    /// СѓРІРµРґРѕРјР»РµРЅРёРµ РѕР± РѕС‚СЃСѓС‚СЃС‚РІРёРё СЃРІРµРґРµРЅРёР№ Рѕ СЃСѓР±СЉРµРєС‚Рµ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tNoticeSubj")]
    public class TNoticeSubj
    {
        /// <summary>
        /// РўРµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlElement(Order=0)]
        public THeadContent HeadContent { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚ СѓРІРµРґРѕРјР»РµРЅРёСЏ
        /// </summary>
        [XmlElement("NoticeSubj", Order=1)]
        public List<TNoticeSubjNoticeSubj> NoticeSubj { get; set; }
        /// <summary>
        /// Р—Р°РІРµСЂС€Р°СЋС‰РёР№С‚РµРєСЃС‚ (СЃСЃС‹Р»РєР° РЅР° Р·Р°РєРѕРЅ Рё С‚.Рї.)
        /// </summary>
        [XmlElement(Order=2)]
        public TFootContent FootContent { get; set; }
        
        /// <summary>
        /// TNoticeSubj class constructor
        /// </summary>
        public TNoticeSubj()
        {
            NoticeSubj = new List<TNoticeSubjNoticeSubj>();
        }
    }
    
    /// <summary>
    /// С‚РµРєСЃС‚ СѓРІРµРґРѕРјР»РµРЅРёСЏ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tNoticeSubjNoticeSubj")]
    [XmlRootAttribute("tNoticeSubjNoticeSubj")]
    public class TNoticeSubjNoticeSubj
    {
        /// <summary>
        /// Р’РёРґ Р·Р°РїСЂРѕС€РµРЅРЅРѕР№ РёРЅС„РѕСЂРјР°С†РёРё
        /// </summary>
        [XmlElement(Order=0)]
        public object TypeInfoText { get; set; }
        [XmlElement("Subject", typeof(TSubject), Order=1)]
        [XmlElement("SubjectInfo", typeof(object), Order=1)]
        public object Item { get; set; }
        /// <summary>
        /// РёРЅС„РѕСЂРјР°С†РёСЏ Рѕ РЅРµРґРµРµСЃРїРѕСЃРѕР±РЅРѕСЃС‚Рё РёР· РљРЈРђ
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(4000)]
        public string ArrestInfo { get; set; }
    }
    
    /// <summary>
    /// РЎСѓР±СЉРµРєС‚
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tSubject")]
    public class TSubject
    {
        /// <summary>
        /// РЈРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(DataType="integer", Order=0)]
        public string ID_Subject { get; set; }
        [XmlElement("Governance", typeof(TGovernance), Order=1)]
        [XmlElement("Organization", typeof(TOrganization), Order=1)]
        [XmlElement("Person", typeof(TPerson), Order=1)]
        public object Item { get; set; }
    }
    
    /// <summary>
    /// РЎСѓР±СЉРµРєС‚ РїСѓР±Р»РёС‡РЅРѕРіРѕ РїСЂР°РІР°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tGovernance")]
    public class TGovernance
    {
        /// <summary>
        /// РЎСѓР±СЉРµРєС‚ РїСЂР°РІРѕРѕС‚РЅРѕС€РµРЅРёР№
        /// </summary>
        [XmlElement(Order=0)]
        public DGovernance Code_SP { get; set; }
        /// <summary>
        /// РЎСѓРјРјР°СЂРѕРЅРѕРµ РѕРїРёСЃР°РЅРёРµ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(2000)]
        public string Content { get; set; }
        /// <summary>
        /// РќР°Р·РІР°РЅРёРµ СЃСѓР±СЉРµРєС‚Р° РїСѓР±Р»РёС‡РЅРѕРіРѕ РїСЂР°РІР°
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(500)]
        public string Name { get; set; }
        /// <summary>
        /// РљРѕРґ РћРљРђРўРћ
        /// </summary>
        [XmlElement(IsNullable=true, Order=3)]
        [StringLengthAttribute(11)]
        public string OKATO_Code { get; set; }
        /// <summary>
        /// РќРѕРјРµСЂ СЂРµРіРёСЃС‚СЂР°С†РёРѕРЅРЅРѕР№ Р·Р°РїРёСЃРё
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(45)]
        public string RegNumber { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РіРѕСЃСѓРґР°СЂСЃС‚РІРµРЅРЅРѕР№ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlElement(Order=5)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string RegDate { get; set; }
        /// <summary>
        /// РџРѕР»РЅРѕРµ РЅР°РёРјРµРЅРѕРІР°РЅРёРµ СЃС‚СЂР°РЅС‹ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlElement(Order=6)]
        [StringLengthAttribute(255)]
        public string Country { get; set; }
        /// <summary>
        /// РђРґСЂРµСЃ РІ СЃС‚СЂР°РЅРµ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlElement(Order=7)]
        [StringLengthAttribute(255)]
        public string Address { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dGovernance")]
    public enum DGovernance
    {
        [XmlEnumAttribute("007001000000")]
        Item007001000000,
        [XmlEnumAttribute("007001001000")]
        Item007001001000,
        [XmlEnumAttribute("007001001001")]
        Item007001001001,
        [XmlEnumAttribute("007001001002")]
        Item007001001002,
        [XmlEnumAttribute("007001001003")]
        Item007001001003,
        [XmlEnumAttribute("007001002000")]
        Item007001002000,
        [XmlEnumAttribute("007001003000")]
        Item007001003000,
        [XmlEnumAttribute("007001004000")]
        Item007001004000,
        [XmlEnumAttribute("007002000000")]
        Item007002000000,
        [XmlEnumAttribute("007002001000")]
        Item007002001000,
        [XmlEnumAttribute("007002002000")]
        Item007002002000,
        [XmlEnumAttribute("007002003000")]
        Item007002003000,
        [XmlEnumAttribute("007003000000")]
        Item007003000000,
        [XmlEnumAttribute("007003001000")]
        Item007003001000,
        [XmlEnumAttribute("007003002000")]
        Item007003002000,
        [XmlEnumAttribute("007003003000")]
        Item007003003000,
    }
    
    /// <summary>
    /// Р®СЂРёРґРёС‡РµСЃРєРѕРµ Р»РёС†Рѕ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tOrganization")]
    public class TOrganization
    {
        /// <summary>
        /// РЎСѓР±СЉРµРєС‚ РїСЂР°РІРѕРѕС‚РЅРѕС€РµРЅРёР№
        /// </summary>
        [XmlElement(Order=0)]
        public DGovernance Code_SP { get; set; }
        /// <summary>
        /// РЎСѓРјРјР°СЂРЅРѕРµ РѕРїРёСЃР°РЅРёРµ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(2000)]
        public string Content { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РјРѕРґРёС„РёРєР°С†РёРё
        /// </summary>
        [XmlElement(Order=2)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string MdfDate { get; set; }
        /// <summary>
        /// РљРѕРґ РћРџР¤
        /// </summary>
        [XmlElement(Order=3)]
        public DOPF Code_OPF { get; set; }
        /// <summary>
        /// РќР°Р·РІР°РЅРёРµ РѕСЂРіР°РЅРёР·Р°С†РёРё Р®СЂ. Р»РёС†Р°
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(500)]
        public string Name { get; set; }
        [XmlElement("Country", Order=5)]
        [XmlElement("INN", Order=5)]
        [XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item { get; set; }
        [XmlElement(Order=6)]
        [XmlIgnore]
        public ItemChoiceType ItemElementName { get; set; }
        /// <summary>
        /// РљРѕРґ РћР“Р Рќ (Р РµРіРёСЃС‚СЂР°С†РёРѕРЅРЅС‹Р№ РЅРѕРјРµСЂ РІ СЃС‚СЂР°РЅРµ СЂРµРіРёСЃС‚СЂР°С†РёРё (РёРЅРєРѕСЂРїРѕСЂР°С†РёРё))
        /// </summary>
        [XmlElement(Order=7)]
        [StringLengthAttribute(20)]
        public string Code_OGRN { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РіРѕСЃСѓРґР°СЂСЃС‚РІРµРЅРЅРѕР№ СЂРµРіРёСЃС‚СЂР°С†РёРё (Р”Р°С‚Р° СЂРµРіРёСЃС‚СЂР°С†РёРё РІ СЃС‚СЂР°РЅРµ СЂРµРіРёСЃС‚СЂР°С†РёРё (РёРЅРєРѕСЂРїРѕСЂР°С†РёРё))
        /// </summary>
        [XmlElement(Order=8)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string RegDate { get; set; }
        /// <summary>
        /// РѕСЂРіР°РЅ СЂРµРіРёСЃС‚СЂР°С†РёРё (РЅР°РёРјРµРЅРѕРІР°РЅРёРµ СЂРµРіРёСЃС‚СЂРёСЂСѓСЋС‰РµРіРѕ РѕСЂРіР°РЅР°)
        /// </summary>
        [XmlElement(Order=9)]
        [StringLengthAttribute(1000)]
        public string AgencyRegistration { get; set; }
        /// <summary>
        /// РљРѕРґ РљРџРџ
        /// </summary>
        [XmlElement(Order=10)]
        [StringLengthAttribute(9)]
        public string Code_CPP { get; set; }
        /// <summary>
        /// Р—Р°СЂРµРіРёСЃС‚СЂРёСЂРѕРІР°РЅРЅС‹Р№ Р°РґСЂРµСЃ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=11)]
        public TAddress Location { get; set; }
        /// <summary>
        /// Р¤Р°РєС‚РёС‡РµСЃРєРёР№ Р°РґСЂРµСЃ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=12)]
        public TAddress FactLocation { get; set; }
        [XmlElement("E-mail", Order=13)]
        public string Email { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dOPF")]
    public enum DOPF
    {
        [XmlEnumAttribute("009001000000")]
        Item009001000000,
        [XmlEnumAttribute("009001001000")]
        Item009001001000,
        [XmlEnumAttribute("009001002000")]
        Item009001002000,
        [XmlEnumAttribute("009001003000")]
        Item009001003000,
        [XmlEnumAttribute("009001004000")]
        Item009001004000,
        [XmlEnumAttribute("009001005000")]
        Item009001005000,
        [XmlEnumAttribute("009001006000")]
        Item009001006000,
        [XmlEnumAttribute("009001007000")]
        Item009001007000,
        [XmlEnumAttribute("009001008000")]
        Item009001008000,
        [XmlEnumAttribute("009001009000")]
        Item009001009000,
        [XmlEnumAttribute("009001010000")]
        Item009001010000,
        [XmlEnumAttribute("009002000000")]
        Item009002000000,
        [XmlEnumAttribute("009002001000")]
        Item009002001000,
        [XmlEnumAttribute("009002002000")]
        Item009002002000,
        [XmlEnumAttribute("009002003000")]
        Item009002003000,
        [XmlEnumAttribute("009002004000")]
        Item009002004000,
        [XmlEnumAttribute("009002005000")]
        Item009002005000,
        [XmlEnumAttribute("009002006000")]
        Item009002006000,
        [XmlEnumAttribute("009002007000")]
        Item009002007000,
        [XmlEnumAttribute("009002008000")]
        Item009002008000,
        [XmlEnumAttribute("009002009000")]
        Item009002009000,
        [XmlEnumAttribute("009002010000")]
        Item009002010000,
        [XmlEnumAttribute("009002011000")]
        Item009002011000,
        [XmlEnumAttribute("009002012000")]
        Item009002012000,
        [XmlEnumAttribute("009002013000")]
        Item009002013000,
        [XmlEnumAttribute("009003000000")]
        Item009003000000,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlTypeAttribute(IncludeInSchema=false)]
    [XmlRootAttribute("ItemChoiceType")]
    public enum ItemChoiceType
    {
        Country,
        INN,
    }
    
    /// <summary>
    /// Р¤РёР·РёС‡РµСЃРєРѕРµ Р»РёС†Рѕ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tPerson")]
    public class TPerson
    {
        /// <summary>
        /// РЎСѓР±СЉРµРєС‚ РїСЂР°РІРѕРѕС‚РЅРѕС€РµРЅРёР№
        /// </summary>
        [XmlElement(Order=0)]
        public DGovernance Code_SP { get; set; }
        /// <summary>
        /// РЎСѓРјРјР°СЂРЅРѕРµ РѕРїРёСЃР°РЅРёРµ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(2000)]
        public string Content { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РјРѕРґРёС„РёРєР°С†РёРё
        /// </summary>
        [XmlElement(Order=2)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string MdfDate { get; set; }
        /// <summary>
        /// Р¤Р°РјРёР»РёСЏ, РёРјСЏ, РѕС‚С‡РµСЃС‚РІРѕ РґР»СЏ Р¤РёР·. Р»РёС†
        /// </summary>
        [XmlElement(Order=3)]
        public TFIO FIO { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° СЂРѕР¶РґРµРЅРёСЏ
        /// </summary>
        [XmlElement(IsNullable=true, Order=4)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string DateBirth { get; set; }
        /// <summary>
        /// РњРµСЃС‚Рѕ СЂРѕР¶РґРµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=5)]
        [StringLengthAttribute(255)]
        public string Place_Birth { get; set; }
        /// <summary>
        /// Р“СЂР°Р¶РґР°РЅСЃС‚РІРѕ
        /// </summary>
        [XmlElement(Order=6)]
        [StringLengthAttribute(255)]
        public string Citizen { get; set; }
        /// <summary>
        /// РџРѕР»
        /// </summary>
        [XmlElement(Order=7)]
        [StringLengthAttribute(255)]
        public string Sex { get; set; }
        /// <summary>
        /// Р”РѕРєСѓРјРµРЅС‚, СѓРґРѕСЃС‚РѕРІРµСЂСЏСЋС‰РёР№ Р»РёС‡РЅРѕСЃС‚СЊ
        /// </summary>
        [XmlElement(Order=8)]
        public TDocPerson Document { get; set; }
        [XmlElement(IsNullable=true, Order=9)]
        [StringLengthAttribute(12)]
        public string INN { get; set; }
        /// <summary>
        /// Р—Р°СЂРµРіРёСЃС‚СЂРёСЂРѕРІР°РЅРЅС‹Р№ Р°РґСЂРµСЃ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=10)]
        public TAddress Location { get; set; }
        /// <summary>
        /// Р¤Р°РєС‚РёС‡РµСЃРєРёР№ Р°РґСЂРµСЃ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=11)]
        public TAddress FactLocation { get; set; }
        /// <summary>
        /// РЎРќРР›РЎ
        /// </summary>
        [XmlElement(Order=12)]
        [RegularExpressionAttribute("\\d{3}-\\d{3}-\\d{3}( |-)\\d{2}")]
        public string SNILS { get; set; }
    }
    
    /// <summary>
    /// Р¤Р°РјРёР»РёСЏ, РРјСЏ, РћС‚С‡РµСЃС‚РІРѕ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tFIO")]
    public class TFIO
    {
        /// <summary>
        /// Р¤Р°РјРёР»РёСЏ
        /// </summary>
        [XmlElement(Order=0)]
        [StringLengthAttribute(45)]
        public string Surname { get; set; }
        /// <summary>
        /// РРјСЏ
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(255)]
        public string First { get; set; }
        /// <summary>
        /// РћС‚С‡РµСЃС‚РІРѕ
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(45)]
        public string Patronymic { get; set; }
    }
    
    /// <summary>
    /// Р”РѕРєСѓРјРµРЅС‚ - РѕРїРёСЃР°РЅРёРµ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tDocPerson")]
    public class TDocPerson
    {
        /// <summary>
        /// РЎСѓРјРјР°СЂРЅРѕРµ РѕРїРёСЃР°РЅРёРµ
        /// </summary>
        [XmlElement(Order=0)]
        [StringLengthAttribute(4000)]
        public string Content { get; set; }
        /// <summary>
        /// РўРёРї РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(Order=1)]
        public DCertificates Type_Document { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(2000)]
        public string Name { get; set; }
        /// <summary>
        /// РЎРµСЂРёСЏ РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(45)]
        public string Series { get; set; }
        /// <summary>
        /// РќРѕРјРµСЂ РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(IsNullable=true, Order=4)]
        [StringLengthAttribute(45)]
        public string Number { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РІС‹РґР°С‡Рё РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(IsNullable=true, Order=5)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string Date { get; set; }
        /// <summary>
        /// РћСЂРіР°РЅРёР·Р°С†РёСЏ, РІС‹РґР°РІС€Р°СЏ РґРѕРєСѓРјРµРЅС‚
        /// </summary>
        [XmlElement(Order=6)]
        [StringLengthAttribute(255)]
        public string IssueOrgan { get; set; }
        /// <summary>
        /// РљРѕРґ РїРѕРґСЂР°Р·РґРµР»РµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=7)]
        [StringLengthAttribute(45)]
        public string DeptCode { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dCertificates")]
    public enum DCertificates
    {
        [XmlEnumAttribute("008001000000")]
        Item008001000000,
        [XmlEnumAttribute("008001001000")]
        Item008001001000,
        [XmlEnumAttribute("008001002000")]
        Item008001002000,
        [XmlEnumAttribute("008001003000")]
        Item008001003000,
        [XmlEnumAttribute("008001004000")]
        Item008001004000,
        [XmlEnumAttribute("008001005000")]
        Item008001005000,
        [XmlEnumAttribute("008001006000")]
        Item008001006000,
        [XmlEnumAttribute("008001007000")]
        Item008001007000,
        [XmlEnumAttribute("008001008000")]
        Item008001008000,
        [XmlEnumAttribute("008001009000")]
        Item008001009000,
        [XmlEnumAttribute("008001010000")]
        Item008001010000,
        [XmlEnumAttribute("008001011000")]
        Item008001011000,
        [XmlEnumAttribute("008001012000")]
        Item008001012000,
        [XmlEnumAttribute("008001013000")]
        Item008001013000,
        [XmlEnumAttribute("008001014000")]
        Item008001014000,
        [XmlEnumAttribute("008001015000")]
        Item008001015000,
        [XmlEnumAttribute("008001016000")]
        Item008001016000,
        [XmlEnumAttribute("008001017000")]
        Item008001017000,
        [XmlEnumAttribute("008001018000")]
        Item008001018000,
        [XmlEnumAttribute("008001019000")]
        Item008001019000,
        [XmlEnumAttribute("008001020000")]
        Item008001020000,
        [XmlEnumAttribute("008001021000")]
        Item008001021000,
        [XmlEnumAttribute("008002000000")]
        Item008002000000,
        [XmlEnumAttribute("008002001000")]
        Item008002001000,
        [XmlEnumAttribute("008002002000")]
        Item008002002000,
    }
    
    /// <summary>
    /// РѕС‚РєР°Р· РІ РІС‹РґР°С‡Рµ РёРЅС„РѕСЂРјР°С†РёРё РѕР± РѕР±СЉРµРєС‚Рµ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tRefusalObj")]
    public class TRefusalObj
    {
        /// <summary>
        /// РўРµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlElement(Order=0)]
        public THeadContent HeadContent { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚ РѕС‚РєР°Р·Р°
        /// </summary>
        [XmlElement("RefusalObj", Order=1)]
        public List<TRefusalObjRefusalObj> RefusalObj { get; set; }
        /// <summary>
        /// Р—Р°РІРµСЂС€Р°СЋС‰РµРіРѕ С‚РµРєСЃС‚Р° (СЃСЃС‹Р»РєР° РЅР° Р·Р°РєРѕРЅ Рё С‚.Рї.)
        /// </summary>
        [XmlElement(Order=2)]
        public TFootContent FootContent { get; set; }
        
        /// <summary>
        /// TRefusalObj class constructor
        /// </summary>
        public TRefusalObj()
        {
            RefusalObj = new List<TRefusalObjRefusalObj>();
        }
    }
    
    /// <summary>
    /// С‚РµРєСЃС‚ РѕС‚РєР°Р·Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tRefusalObjRefusalObj")]
    [XmlRootAttribute("tRefusalObjRefusalObj")]
    public class TRefusalObjRefusalObj
    {
        /// <summary>
        /// Р’РёРґ Р·Р°РїСЂРѕС€РµРЅРЅРѕР№ РёРЅС„РѕСЂРјР°С†РёРё
        /// </summary>
        [XmlElement(Order=0)]
        public object TypeInfoText { get; set; }
        [XmlElement("ObjectDetail", typeof(TRefusalObjRefusalObjObjectDetail), Order=1)]
        [XmlElement("ObjectInfo", typeof(object), Order=1)]
        public object Item { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚ РѕС‚РєР°Р·Р°
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(4000)]
        public string TextRefusal { get; set; }
        /// <summary>
        /// С‚РёРї РѕС‚РєР°Р·Р°
        /// </summary>
        [XmlAttribute(AttributeName="RefusalType")]
        public DRefusal RefusalType { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚ С‚РёРїР° РѕС‚РєР°Р·Р°
        /// </summary>
        [XmlAttribute(AttributeName="RefusalTypeText")]
        [StringLengthAttribute(255)]
        public string RefusalTypeText { get; set; }
    }
    
    /// <summary>
    /// СЃС‚СЂСѓРєС‚СѓСЂРёСЂРѕРІР°РЅРЅРѕРµ РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕС€РµРЅРЅРѕРіРѕ РѕР±СЉРµРєС‚Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tRefusalObjRefusalObjObjectDetail")]
    [XmlRootAttribute("tRefusalObjRefusalObjObjectDetail")]
    public class TRefusalObjRefusalObjObjectDetail
    {
        [XmlElement("CadastralNumber", Order=0)]
        [XmlElement("ConditionalNumber", Order=0)]
        [XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item { get; set; }
        [XmlElement(Order=1)]
        [XmlIgnore]
        public ItemChoiceType2 ItemElementName { get; set; }
        /// <summary>
        /// РљРѕРґ С‚РёРїР° РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement(Order=2)]
        public TObjectObjectType ObjectType { get; set; }
        /// <summary>
        /// РўРµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ С‚РёРїР° РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(1000)]
        public string ObjectTypeText { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ  РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё.
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(4000)]
        public string Name { get; set; }
        /// <summary>
        /// Р°РґСЂРµСЃ РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement(Order=5)]
        public TAddress ObjectAddress { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlTypeAttribute(IncludeInSchema=false)]
    [XmlRootAttribute("ItemChoiceType2")]
    public enum ItemChoiceType2
    {
        CadastralNumber,
        ConditionalNumber,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dRefusal")]
    public enum DRefusal
    {
        [XmlEnumAttribute("001")]
        Item001,
        [XmlEnumAttribute("002")]
        Item002,
        [XmlEnumAttribute("003")]
        Item003,
        [XmlEnumAttribute("004")]
        Item004,
        [XmlEnumAttribute("005")]
        Item005,
        [XmlEnumAttribute("006")]
        Item006,
        [XmlEnumAttribute("007")]
        Item007,
        [XmlEnumAttribute("008")]
        Item008,
        [XmlEnumAttribute("009")]
        Item009,
        [XmlEnumAttribute("010")]
        Item010,
        [XmlEnumAttribute("100")]
        Item100,
        [XmlEnumAttribute("999")]
        Item999,
    }
    
    /// <summary>
    /// РѕС‚РєР°Р· РІ РІС‹РґР°С‡Рµ СЃРІРµРґРµРЅРёР№ Рѕ СЃСѓР±СЉРµРєС‚Рµ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tRefusalSubj")]
    public class TRefusalSubj
    {
        /// <summary>
        /// РўРµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlElement(Order=0)]
        public THeadContent HeadContent { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚ РѕС‚РєР°Р·Р°
        /// </summary>
        [XmlElement("RefusalSubj", Order=1)]
        public List<TRefusalSubjRefusalSubj> RefusalSubj { get; set; }
        /// <summary>
        /// Р—Р°РІРµСЂС€Р°СЋС‰РёР№С‚РµРєСЃС‚ (СЃСЃС‹Р»РєР° РЅР° Р·Р°РєРѕРЅ Рё С‚.Рї.)
        /// </summary>
        [XmlElement(Order=2)]
        public TFootContent FootContent { get; set; }
        
        /// <summary>
        /// TRefusalSubj class constructor
        /// </summary>
        public TRefusalSubj()
        {
            RefusalSubj = new List<TRefusalSubjRefusalSubj>();
        }
    }
    
    /// <summary>
    /// С‚РµРєСЃС‚ РѕС‚РєР°Р·Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tRefusalSubjRefusalSubj")]
    [XmlRootAttribute("tRefusalSubjRefusalSubj")]
    public class TRefusalSubjRefusalSubj
    {
        /// <summary>
        /// Р’РёРґ Р·Р°РїСЂРѕС€РµРЅРЅРѕР№ РёРЅС„РѕСЂРјР°С†РёРё
        /// </summary>
        [XmlElement(Order=0)]
        public object TypeInfoText { get; set; }
        [XmlElement("Subject", typeof(TSubject), Order=1)]
        [XmlElement("SubjectInfo", typeof(object), Order=1)]
        public object Item { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚ РѕС‚РєР°Р·Р°
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(4000)]
        public string TextRefusal { get; set; }
        /// <summary>
        /// С‚РёРї РѕС‚РєР°Р·Р°
        /// </summary>
        [XmlAttribute(AttributeName="RefusalType")]
        public DRefusal RefusalType { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚ С‚РёРїР° РѕС‚РєР°Р·Р°
        /// </summary>
        [XmlAttribute(AttributeName="RefusalTypeText")]
        [StringLengthAttribute(255)]
        public string RefusalTypeText { get; set; }
    }
    
    /// <summary>
    /// Р”РѕР»Р¶РЅРѕСЃС‚РЅРѕРµ Р»РёС†Рѕ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tPerson_Org")]
    public class TPerson_Org
    {
        /// <summary>
        /// Р”РѕР»Р¶РЅРѕСЃС‚СЊ
        /// </summary>
        [XmlElement(Order=0)]
        [StringLengthAttribute(255)]
        public string Appointment { get; set; }
        /// <summary>
        /// Р¤Р°РјРёР»РёСЏ, РёРјСЏ, РѕС‚С‡РµСЃС‚РІРѕ
        /// </summary>
        [XmlElement(Order=1)]
        public TFIO FIO { get; set; }
        /// <summary>
        /// РўРµР»РµС„РѕРЅ
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(50)]
        public string Tel { get; set; }
        /// <summary>
        /// Р°РґСЂРµСЃ СЌР»РµРєС‚СЂРѕРЅРЅРѕР№ РїРѕС‡С‚С‹
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(50)]
        public string Email { get; set; }
    }
    
    /// <summary>
    /// РѕР±СЉРµРєС‚С‹ РґРѕР»РµРІРѕРіРѕ СЃС‚СЂРѕРёС‚РµР»СЊСЃС‚РІР°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tShareHolding")]
    public class TShareHolding
    {
        /// <summary>
        /// РЈРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ РґРѕРіРѕРІРѕСЂР° РґРѕР»РµРІРѕРіРѕ СѓС‡Р°СЃС‚РёСЏ
        /// </summary>
        [XmlElement(DataType="integer", Order=0)]
        public string ID_DDU { get; set; }
        /// <summary>
        /// РћРїРёСЃР°РЅРёРµ РѕР±СЉРµРєС‚Р° РґРѕР»РµРІРѕРіРѕ СЃС‚СЂРѕРёС‚РµР»СЊСЃС‚РІР°
        /// </summary>
        [XmlArrayAttribute(Order=1)]
        [XmlArrayItemAttribute("ShareObjects", IsNullable=false)]
        public List<string> ShareHolding { get; set; }
        /// <summary>
        /// РЈС‡Р°СЃС‚РЅРёРєРё РґРѕР»РµРІРѕРіРѕ СЃС‚СЂРѕРёС‚РµР»СЊСЃС‚РІР°
        /// </summary>
        [XmlElement("Owner", Order=2)]
        public List<TSubject> Owner { get; set; }
        /// <summary>
        /// РћРїРёСЃР°РЅРёРµ РёРїРѕС‚РµРєРё
        /// </summary>
        [XmlElement("Encumbrance", Order=3)]
        public List<TEmcumbrance> Encumbrance { get; set; }
        
        /// <summary>
        /// TShareHolding class constructor
        /// </summary>
        public TShareHolding()
        {
            Encumbrance = new List<TEmcumbrance>();
            Owner = new List<TSubject>();
        }
    }
    
    /// <summary>
    /// Р—Р°РїРёСЃСЊ РѕР± РѕРіСЂР°РЅРёС‡РµРЅРёРё
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tEmcumbrance")]
    public class TEmcumbrance
    {
        /// <summary>
        /// РЈРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ Р·Р°РїРёСЃРё РѕР± РѕРіСЂР°РЅРёС‡РµРЅРёРё
        /// </summary>
        [XmlElement(DataType="integer", Order=0)]
        public string ID_Record { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РјРѕРґРёС„РёРєР°С†РёРё
        /// </summary>
        [XmlElement(Order=1)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string MdfDate { get; set; }
        /// <summary>
        /// РќРѕРјРµСЂ РіРѕСЃСѓРґР°СЂСЃС‚РІРµРµРЅРЅРѕР№ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(45)]
        public string RegNumber { get; set; }
        /// <summary>
        /// РљРѕРґ  РѕРіСЂР°РЅРёС‡РµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=3)]
        public DEncumbrances Type { get; set; }
        /// <summary>
        /// Р’РёРґ РѕРіСЂР°РЅРёС‡РµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(4000)]
        public string Name { get; set; }
        /// <summary>
        /// РџСЂРµРґРјРµС‚ РѕРіСЂР°РЅРёС‡РµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=5)]
        public string ShareText { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РіРѕСЃСѓРґР°СЂСЃС‚РІРµРЅРЅРѕР№ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlElement(Order=6)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string RegDate { get; set; }
        /// <summary>
        /// РЎСЂРѕРє РґРµР№СЃС‚РІРёСЏ.
        /// </summary>
        [XmlElement(Order=7)]
        public TEmcumbranceDuration Duration { get; set; }



        [XmlElement("AllShareOwner", typeof(string), Order=8)]
        [XmlElement("Owner", typeof(TSubject), Order=8)]
        public List<object> Items { get; set; }
        /// <summary>
        /// Р”РѕРєСѓРјРµРЅС‚С‹ - РѕСЃРЅРѕРІР°РЅРёСЏ СЂРµРіРёСЃС‚СЂР°С†РёРё РѕРіСЂР°РЅРёС‡РµРЅРёСЏ
        /// </summary>
        [XmlElement("DocFound", Order=9)]
        public List<TDocRight> DocFound { get; set; }
        
        /// <summary>
        /// TEmcumbrance class constructor
        /// </summary>
        public TEmcumbrance()
        {
            DocFound = new List<TDocRight>();
            Items = new List<object>();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dEncumbrances")]
    public enum DEncumbrances
    {
        [XmlEnumAttribute("022001000000")]
        Item022001000000,
        [XmlEnumAttribute("022001001000")]
        Item022001001000,
        [XmlEnumAttribute("022001002000")]
        Item022001002000,
        [XmlEnumAttribute("022002000000")]
        Item022002000000,
        [XmlEnumAttribute("022003000000")]
        Item022003000000,
        [XmlEnumAttribute("022004000000")]
        Item022004000000,
        [XmlEnumAttribute("022005000000")]
        Item022005000000,
        [XmlEnumAttribute("022006000000")]
        Item022006000000,
        [XmlEnumAttribute("022007000000")]
        Item022007000000,
        [XmlEnumAttribute("022008000000")]
        Item022008000000,
        [XmlEnumAttribute("022099000000")]
        Item022099000000,
    }
    
    /// <summary>
    /// РЎСЂРѕРє РґРµР№СЃС‚РІРёСЏ.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tEmcumbranceDuration")]
    [XmlRootAttribute("tEmcumbranceDuration")]
    public class TEmcumbranceDuration
    {
        /// <summary>
        /// Р”Р°С‚Р° РЅР°С‡Р°Р»Р° РґРµР№СЃС‚РІРёСЏ
        /// </summary>
        [XmlElement(Order=0)]
        public string Started { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РїСЂРµРєСЂР°С‰РµРЅРёСЏ РґРµР№СЃС‚РІРёСЏ
        /// </summary>
        [XmlElement(Order=1)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string Stopped { get; set; }
        /// <summary>
        /// РџСЂРѕРґРѕР»Р¶РёС‚РµР»СЊРЅРѕСЃС‚СЊ
        /// </summary>
        [XmlElement(Order=2)]
        public string Term { get; set; }
    }
    
    /// <summary>
    /// Р”РѕРєСѓРјРµРЅС‚ - РѕРїРёСЃР°РЅРёРµ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tDocRight")]
    public class TDocRight
    {
        /// <summary>
        /// СѓРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(DataType="integer", Order=0)]
        public string ID_Document { get; set; }
        /// <summary>
        /// РЎСѓРјРјР°СЂРЅРѕРµ РѕРїРёСЃР°РЅРёРµ
        /// </summary>
        [XmlElement(Order=1)]
        public string Content { get; set; }
        /// <summary>
        /// РўРёРї РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(Order=2)]
        public DDocuments Type_Document { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(Order=3)]
        [StringLengthAttribute(2000)]
        public string Name { get; set; }
        /// <summary>
        /// РЎРµСЂРёСЏ РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(45)]
        public string Series { get; set; }
        /// <summary>
        /// РќРѕРјРµСЂ РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(IsNullable=true, Order=5)]
        [StringLengthAttribute(45)]
        public string Number { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РІС‹РґР°С‡Рё РґРѕРєСѓРјРµРЅС‚Р°
        /// </summary>
        [XmlElement(IsNullable=true, Order=6)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string Date { get; set; }
        /// <summary>
        /// РћСЂРіР°РЅРёР·Р°С†РёСЏ, РІС‹РґР°РІС€Р°СЏ РґРѕРєСѓРјРµРЅС‚
        /// </summary>
        [XmlElement(Order=7)]
        [StringLengthAttribute(255)]
        public string IssueOrgan { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dDocuments")]
    public enum DDocuments
    {
        [XmlEnumAttribute("010001000000")]
        Item010001000000,
        [XmlEnumAttribute("010001001000")]
        Item010001001000,
        [XmlEnumAttribute("010001002000")]
        Item010001002000,
        [XmlEnumAttribute("010002000000")]
        Item010002000000,
        [XmlEnumAttribute("010002001000")]
        Item010002001000,
        [XmlEnumAttribute("010002001001")]
        Item010002001001,
        [XmlEnumAttribute("010002001002")]
        Item010002001002,
        [XmlEnumAttribute("010002002000")]
        Item010002002000,
        [XmlEnumAttribute("010002003000")]
        Item010002003000,
        [XmlEnumAttribute("010002004000")]
        Item010002004000,
        [XmlEnumAttribute("010003000000")]
        Item010003000000,
        [XmlEnumAttribute("010003001000")]
        Item010003001000,
        [XmlEnumAttribute("010003002000")]
        Item010003002000,
        [XmlEnumAttribute("010003003000")]
        Item010003003000,
        [XmlEnumAttribute("010003004000")]
        Item010003004000,
        [XmlEnumAttribute("010003005000")]
        Item010003005000,
        [XmlEnumAttribute("010003006000")]
        Item010003006000,
        [XmlEnumAttribute("010003007000")]
        Item010003007000,
        [XmlEnumAttribute("010003008000")]
        Item010003008000,
        [XmlEnumAttribute("010003009000")]
        Item010003009000,
        [XmlEnumAttribute("010003009001")]
        Item010003009001,
        [XmlEnumAttribute("010003010000")]
        Item010003010000,
        [XmlEnumAttribute("010003011000")]
        Item010003011000,
        [XmlEnumAttribute("010003012000")]
        Item010003012000,
        [XmlEnumAttribute("010003013000")]
        Item010003013000,
        [XmlEnumAttribute("010003014000")]
        Item010003014000,
        [XmlEnumAttribute("010003015000")]
        Item010003015000,
        [XmlEnumAttribute("010003016000")]
        Item010003016000,
        [XmlEnumAttribute("010004000000")]
        Item010004000000,
        [XmlEnumAttribute("010005000000")]
        Item010005000000,
        [XmlEnumAttribute("010005001000")]
        Item010005001000,
        [XmlEnumAttribute("010005002000")]
        Item010005002000,
        [XmlEnumAttribute("010005003000")]
        Item010005003000,
        [XmlEnumAttribute("010006000000")]
        Item010006000000,
        [XmlEnumAttribute("010006001000")]
        Item010006001000,
        [XmlEnumAttribute("010006002000")]
        Item010006002000,
        [XmlEnumAttribute("010006003000")]
        Item010006003000,
        [XmlEnumAttribute("010006004000")]
        Item010006004000,
        [XmlEnumAttribute("010006005000")]
        Item010006005000,
        [XmlEnumAttribute("010006006000")]
        Item010006006000,
        [XmlEnumAttribute("010006007000")]
        Item010006007000,
        [XmlEnumAttribute("010006008000")]
        Item010006008000,
        [XmlEnumAttribute("010006009000")]
        Item010006009000,
        [XmlEnumAttribute("010006010000")]
        Item010006010000,
        [XmlEnumAttribute("010006011000")]
        Item010006011000,
        [XmlEnumAttribute("010006012000")]
        Item010006012000,
        [XmlEnumAttribute("010007000000")]
        Item010007000000,
        [XmlEnumAttribute("010007001000")]
        Item010007001000,
        [XmlEnumAttribute("010007002000")]
        Item010007002000,
        [XmlEnumAttribute("010007003000")]
        Item010007003000,
        [XmlEnumAttribute("010007004000")]
        Item010007004000,
    }
    
    /// <summary>
    /// РЎРІРµРґРµРЅРёСЏ Рѕ РїСЂР°РІР°С… СЃСѓР±СЉРµРєС‚Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tRightSubj")]
    public class TRightSubj
    {
        /// <summary>
        /// РћРїРёСЃР°РЅРёРµ РѕР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=0)]
        public TObject Object { get; set; }
        /// <summary>
        /// Р—Р°СЂРµРіРёСЃС‚СЂРёСЂРѕРІР°РЅРЅРѕРµ РїСЂР°РІРѕ
        /// </summary>
        [XmlElement(Order=1)]
        public TOpenRegistration Registration { get; set; }
        /// <summary>
        /// РћРіСЂР°РЅРёС‡РµРЅРёРµ РїСЂР°РІР°
        /// </summary>
        [XmlElement("Encumbrance", Order=2)]
        public List<TRightSubjEncumbrance> Encumbrance { get; set; }
        
        /// <summary>
        /// TRightSubj class constructor
        /// </summary>
        public TRightSubj()
        {
            Encumbrance = new List<TRightSubjEncumbrance>();
        }
    }
    
    /// <summary>
    /// РћР±СЉРµРєС‚
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tObject")]
    public class TObject
    {
        /// <summary>
        /// РЈРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ РѕР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(DataType="integer", Order=0)]
        public string ID_Object { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РјРѕРґРёС„РёРєР°С†РёРё
        /// </summary>
        [XmlElement(Order=1)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string MdfDate { get; set; }

        [XmlElement("CadastralNumber", Order=2)]
        [XmlElement("ConditionalNumber", Order=2)]
        [XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item { get; set; }

        [XmlElement(Order=3)]
        [XmlIgnore]
        public ItemChoiceType1 ItemElementName { get; set; }
        /// <summary>
        /// РљРѕРґ С‚РёРїР° РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(255)]
        public TObjectObjectType ObjectType { get; set; }
        /// <summary>
        /// РўРµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ С‚РёРїР° РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement(Order=5)]
        [StringLengthAttribute(1000)]
        public string ObjectTypeText { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ  РѕР±СЉРµРєС‚Р° РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё.
        /// </summary>
        [XmlElement(Order=6)]
        [StringLengthAttribute(4000)]
        public string Name { get; set; }
        /// <summary>
        /// РљРѕРґ РЅР°Р·РЅР°С‡РµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=7)]
        public DAssignation Assignation_Code { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ РЅР°Р·РЅР°С‡РµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=8)]
        [StringLengthAttribute(1000)]
        public string Assignation_Code_Text { get; set; }
        /// <summary>
        /// Р¦РµР»РµРІРѕРµ РЅР°Р·РЅР°С‡РµРЅРёРµ (РєР°С‚РµРіРѕСЂРёСЏ) Р·РµРјРµР»СЊ
        /// </summary>
        [XmlElement(Order=9)]
        public DCategories GroundCategory { get; set; }
        /// <summary>
        /// С‚РµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ С†РµР»РµРІРѕРµРіРѕ РЅР°Р·РЅР°С‡РµРЅРёСЏ(РєР°С‚РµРіРѕСЂРёРё) Р·РµРјРµР»СЊ
        /// </summary>
        [XmlElement(Order=10)]
        [StringLengthAttribute(2000)]
        public string GroundCategoryText { get; set; }
        /// <summary>
        /// РћСЃРЅРѕРІРЅР°СЏ РїР»РѕС‰Р°РґСЊ Рё РґСЂСѓРіРёРµ РїР»РѕС‰Р°РґРё
        /// </summary>
        [XmlElement(Order=11)]
        public TArea Area { get; set; }
        /// <summary>
        /// РРЅРІРµРЅС‚Р°СЂРЅС‹Р№ РЅРѕРјРµСЂ, Р»РёС‚РµСЂ
        /// </summary>
        [XmlElement(Order=12)]
        [StringLengthAttribute(4000)]
        public string Inv_No { get; set; }
        /// <summary>
        /// Р­С‚Р°Р¶РЅРѕСЃС‚СЊ (СЌС‚Р°Р¶)
        /// </summary>
        [XmlElement(Order=13)]
        [StringLengthAttribute(1000)]
        public string Floor { get; set; }
        /// <summary>
        /// РќРѕРјРµСЂР° РЅР° РїРѕСЌС‚Р°Р¶РЅРѕРј РїР»Р°РЅРµ
        /// </summary>
        [XmlArrayAttribute(Order=14)]
        [XmlArrayItemAttribute("Explication", IsNullable=false)]
        public List<string> FloorPlan_No { get; set; }
        /// <summary>
        /// РђРґСЂРµСЃ
        /// </summary>
        [XmlElement(Order=15)]
        public TAddress Address { get; set; }
        /// <summary>
        /// РЎРѕСЃС‚Р°РІ СЃР»РѕР¶РЅРѕР№ РІРµС‰Рё
        /// </summary>
        [XmlArrayAttribute(Order=16)]
        [XmlArrayItemAttribute("Explication", IsNullable=false)]
        public List<string> Complex { get; set; }

        /// <summary>
        /// Р”Р°С‚Р° Р»РёРєРІРёРґР°С†РёРё РѕР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=17)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string ReEndDate { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlTypeAttribute(IncludeInSchema=false)]
    [XmlRootAttribute("ItemChoiceType1")]
    public enum ItemChoiceType1
    {
        CadastralNumber,
        ConditionalNumber,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dAssignation")]
    public enum DAssignation
    {
        [XmlEnumAttribute("005001000000")]
        Item005001000000,
        [XmlEnumAttribute("005001001000")]
        Item005001001000,
        [XmlEnumAttribute("005001001001")]
        Item005001001001,
        [XmlEnumAttribute("005001002000")]
        Item005001002000,
        [XmlEnumAttribute("005002000000")]
        Item005002000000,
        [XmlEnumAttribute("005002001000")]
        Item005002001000,
        [XmlEnumAttribute("005002001001")]
        Item005002001001,
        [XmlEnumAttribute("005002001002")]
        Item005002001002,
        [XmlEnumAttribute("005002001003")]
        Item005002001003,
        [XmlEnumAttribute("005002001004")]
        Item005002001004,
        [XmlEnumAttribute("005002001005")]
        Item005002001005,
        [XmlEnumAttribute("005002001006")]
        Item005002001006,
        [XmlEnumAttribute("005002001007")]
        Item005002001007,
        [XmlEnumAttribute("005002001008")]
        Item005002001008,
        [XmlEnumAttribute("005002001009")]
        Item005002001009,
        [XmlEnumAttribute("005002001010")]
        Item005002001010,
        [XmlEnumAttribute("005002001011")]
        Item005002001011,
        [XmlEnumAttribute("005002001012")]
        Item005002001012,
        [XmlEnumAttribute("005002001013")]
        Item005002001013,
        [XmlEnumAttribute("005002001014")]
        Item005002001014,
        [XmlEnumAttribute("005002001015")]
        Item005002001015,
        [XmlEnumAttribute("005002002000")]
        Item005002002000,
        [XmlEnumAttribute("005002002001")]
        Item005002002001,
        [XmlEnumAttribute("005002002002")]
        Item005002002002,
        [XmlEnumAttribute("005002002003")]
        Item005002002003,
        [XmlEnumAttribute("005002002004")]
        Item005002002004,
        [XmlEnumAttribute("005002002005")]
        Item005002002005,
        [XmlEnumAttribute("005002002006")]
        Item005002002006,
        [XmlEnumAttribute("005002002007")]
        Item005002002007,
        [XmlEnumAttribute("005002002008")]
        Item005002002008,
        [XmlEnumAttribute("005002002009")]
        Item005002002009,
        [XmlEnumAttribute("005002002010")]
        Item005002002010,
        [XmlEnumAttribute("005002002011")]
        Item005002002011,
        [XmlEnumAttribute("005002002012")]
        Item005002002012,
        [XmlEnumAttribute("005002002013")]
        Item005002002013,
        [XmlEnumAttribute("005002002014")]
        Item005002002014,
        [XmlEnumAttribute("005002002015")]
        Item005002002015,
        [XmlEnumAttribute("005002002016")]
        Item005002002016,
        [XmlEnumAttribute("005002002017")]
        Item005002002017,
        [XmlEnumAttribute("005002002018")]
        Item005002002018,
        [XmlEnumAttribute("005002002019")]
        Item005002002019,
        [XmlEnumAttribute("005002002020")]
        Item005002002020,
        [XmlEnumAttribute("005002002021")]
        Item005002002021,
        [XmlEnumAttribute("005002002022")]
        Item005002002022,
        [XmlEnumAttribute("005002002023")]
        Item005002002023,
        [XmlEnumAttribute("005002002024")]
        Item005002002024,
        [XmlEnumAttribute("005002002025")]
        Item005002002025,
        [XmlEnumAttribute("005002002026")]
        Item005002002026,
        [XmlEnumAttribute("005002002027")]
        Item005002002027,
        [XmlEnumAttribute("005002002028")]
        Item005002002028,
        [XmlEnumAttribute("005002003000")]
        Item005002003000,
        [XmlEnumAttribute("005002003001")]
        Item005002003001,
        [XmlEnumAttribute("005002003002")]
        Item005002003002,
        [XmlEnumAttribute("005002003003")]
        Item005002003003,
        [XmlEnumAttribute("005002003004")]
        Item005002003004,
        [XmlEnumAttribute("005002003005")]
        Item005002003005,
        [XmlEnumAttribute("005002003006")]
        Item005002003006,
        [XmlEnumAttribute("005002003007")]
        Item005002003007,
        [XmlEnumAttribute("005002004000")]
        Item005002004000,
        [XmlEnumAttribute("005002005000")]
        Item005002005000,
        [XmlEnumAttribute("005002006000")]
        Item005002006000,
        [XmlEnumAttribute("005002007000")]
        Item005002007000,
        [XmlEnumAttribute("005002008000")]
        Item005002008000,
        [XmlEnumAttribute("005002009000")]
        Item005002009000,
        [XmlEnumAttribute("005002010000")]
        Item005002010000,
        [XmlEnumAttribute("005002011000")]
        Item005002011000,
        [XmlEnumAttribute("005002012000")]
        Item005002012000,
        [XmlEnumAttribute("005002013000")]
        Item005002013000,
        [XmlEnumAttribute("005002013001")]
        Item005002013001,
        [XmlEnumAttribute("005002013002")]
        Item005002013002,
        [XmlEnumAttribute("005002013003")]
        Item005002013003,
        [XmlEnumAttribute("005002014000")]
        Item005002014000,
        [XmlEnumAttribute("005002014001")]
        Item005002014001,
        [XmlEnumAttribute("005002014002")]
        Item005002014002,
        [XmlEnumAttribute("005002014003")]
        Item005002014003,
        [XmlEnumAttribute("005002014004")]
        Item005002014004,
        [XmlEnumAttribute("005002014005")]
        Item005002014005,
        [XmlEnumAttribute("005002015000")]
        Item005002015000,
        [XmlEnumAttribute("005002016000")]
        Item005002016000,
        [XmlEnumAttribute("005002017000")]
        Item005002017000,
        [XmlEnumAttribute("005003000000")]
        Item005003000000,
        [XmlEnumAttribute("005004000000")]
        Item005004000000,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dCategories")]
    public enum DCategories
    {
        [XmlEnumAttribute("003001000000")]
        Item003001000000,
        [XmlEnumAttribute("003002000000")]
        Item003002000000,
        [XmlEnumAttribute("003003000000")]
        Item003003000000,
        [XmlEnumAttribute("003004000000")]
        Item003004000000,
        [XmlEnumAttribute("003005000000")]
        Item003005000000,
        [XmlEnumAttribute("003006000000")]
        Item003006000000,
        [XmlEnumAttribute("003007000000")]
        Item003007000000,
        [XmlEnumAttribute("003008000000")]
        Item003008000000,
    }
    
    /// <summary>
    /// РџР»РѕС‰Р°РґРё
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tArea")]
    public class TArea
    {
        /// <summary>
        /// Р—РЅР°С‡РµРЅРёРµ РїР»РѕС‰Р°РґРё
        /// </summary>
        [XmlElement(Order=0)]
        public decimal Area { get; set; }
        /// <summary>
        /// Р—РЅР°С‡РµРЅРёРµ РїР»РѕС‰Р°РґРё С‚РµРєСЃС‚РѕРј
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(4000)]
        public string AreaText { get; set; }
        /// <summary>
        /// Р•РґРёРЅРёС†Р° РёР·РјРµСЂРµРЅРёР№
        /// </summary>
        [XmlElement(Order=2)]
        public DUnit Unit { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dUnit")]
    public enum DUnit
    {
        [XmlEnumAttribute("012001000000")]
        Item012001000000,
        [XmlEnumAttribute("012001001000")]
        Item012001001000,
        [XmlEnumAttribute("012001002000")]
        Item012001002000,
        [XmlEnumAttribute("012002000000")]
        Item012002000000,
        [XmlEnumAttribute("012002001000")]
        Item012002001000,
        [XmlEnumAttribute("012002002000")]
        Item012002002000,
        [XmlEnumAttribute("012002003000")]
        Item012002003000,
        [XmlEnumAttribute("012003000000")]
        Item012003000000,
        [XmlEnumAttribute("012003001000")]
        Item012003001000,
        [XmlEnumAttribute("012003002000")]
        Item012003002000,
        [XmlEnumAttribute("012003003000")]
        Item012003003000,
        [XmlEnumAttribute("012003004000")]
        Item012003004000,
        [XmlEnumAttribute("012003005000")]
        Item012003005000,
        [XmlEnumAttribute("012004000000")]
        Item012004000000,
        [XmlEnumAttribute("012004001000")]
        Item012004001000,
        [XmlEnumAttribute("012004002000")]
        Item012004002000,
        [XmlEnumAttribute("012004003000")]
        Item012004003000,
        [XmlEnumAttribute("012004004000")]
        Item012004004000,
        [XmlEnumAttribute("012005000000")]
        Item012005000000,
        [XmlEnumAttribute("012005001000")]
        Item012005001000,
        [XmlEnumAttribute("012005002000")]
        Item012005002000,
        [XmlEnumAttribute("012005003000")]
        Item012005003000,
    }
    
    /// <summary>
    /// Р—Р°РїРёСЃСЊ Рѕ РїСЂР°РІРµ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tOpenRegistration")]
    public class TOpenRegistration
    {
        /// <summary>
        /// РЈРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ Р·Р°РїРёСЃРё Рѕ РїСЂР°РІРµ (РѕРіСЂР°РЅРёС‡РµРЅРёРё)
        /// </summary>
        [XmlElement(DataType="integer", Order=0)]
        public string ID_Record { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РјРѕРґРёС„РёРєР°С†РёРё
        /// </summary>
        [XmlElement(Order=1)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string MdfDate { get; set; }
        /// <summary>
        /// РќРѕРјРµСЂ РіРѕСЃСѓРґР°СЂСЃС‚РІРµРµРЅРЅРѕР№ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(45)]
        public string RegNumber { get; set; }
        /// <summary>
        /// РљРѕРґ  РїСЂР°РІР°
        /// </summary>
        [XmlElement(Order=3)]
        public DRights Type { get; set; }
        /// <summary>
        /// Р’РёРґ РїСЂР°РІР°
        /// </summary>
        [XmlElement(Order=4)]
        [StringLengthAttribute(4000)]
        public string Name { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РіРѕСЃСѓРґР°СЂСЃС‚РІРµРЅРЅРѕР№ СЂРµРіРёСЃС‚СЂР°С†РёРё
        /// </summary>
        [XmlElement(Order=5)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string RegDate { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РїСЂРµРєСЂР°С‰РµРЅРёСЏ РїСЂР°РІР°
        /// </summary>
        [XmlElement(Order=6)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string EndDate { get; set; }


        [XmlElement("Share", typeof(TOpenRegistrationShare), Order=7)]
        [XmlElement("ShareText", typeof(string), Order=7)]
        public object Item { get; set; }
        /// <summary>
        /// Р”РѕРєСѓРјРµРЅС‚С‹ - РѕСЃРЅРѕРІР°РЅРёСЏ СЂРµРіРёСЃС‚СЂР°С†РёРё РїСЂР°РІР°
        /// </summary>
        [XmlElement("DocFound", Order=8)]
        public List<TDocRight> DocFound { get; set; }
        
        /// <summary>
        /// TOpenRegistration class constructor
        /// </summary>
        public TOpenRegistration()
        {
            DocFound = new List<TDocRight>();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dRights")]
    public enum DRights
    {
        [XmlEnumAttribute("001001000000")]
        Item001001000000,
        [XmlEnumAttribute("001002000000")]
        Item001002000000,
        [XmlEnumAttribute("001003000000")]
        Item001003000000,
        [XmlEnumAttribute("001004000000")]
        Item001004000000,
        [XmlEnumAttribute("001005000000")]
        Item001005000000,
        [XmlEnumAttribute("001006000000")]
        Item001006000000,
        [XmlEnumAttribute("001007000000")]
        Item001007000000,
        [XmlEnumAttribute("001008000000")]
        Item001008000000,
        [XmlEnumAttribute("001099000000")]
        Item001099000000,
    }
    
    /// <summary>
    /// Р”РѕР»СЏ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tOpenRegistrationShare")]
    [XmlRootAttribute("tOpenRegistrationShare")]
    public class TOpenRegistrationShare
    {
        /// <summary>
        /// Р§РёСЃР»РёС‚РµР»СЊ РґРѕР»Рё
        /// </summary>
        [XmlAttribute(DataType="integer", AttributeName="Numerator")]
        public string Numerator { get; set; }
        /// <summary>
        /// Р—РЅР°РјРµРЅР°С‚РµР»СЊ РґРѕР»Рё
        /// </summary>
        [XmlAttribute(DataType="integer", AttributeName="Denominator")]
        public string Denominator { get; set; }
    }
    
    /// <summary>
    /// РћРіСЂР°РЅРёС‡РµРЅРёРµ РїСЂР°РІР°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tRightSubjEncumbrance")]
    [XmlRootAttribute("tRightSubjEncumbrance")]
    public class TRightSubjEncumbrance : TEmcumbrance
    {
        [XmlAttribute(AttributeName="EncumbNumber")]
        public string EncumbNumber { get; set; }
    }
    
    /// <summary>
    /// РЎРІРµРґРµРЅРёСЏ Рѕ РїСЂР°РІР°С… РїРѕ РћРќ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tRightObj")]
    public class TRightObj
    {
        [XmlElement("NoOwner", typeof(object), Order=0)]
        [XmlElement("Owner", typeof(TRightObjOwner), Order=0)]
        public List<object> Items { get; set; }
        [XmlElement("NoRegistration", typeof(object), Order=1)]
        [XmlElement("Registration", typeof(TOpenRegistration), Order=1)]
        public object Item { get; set; }
        [XmlElement("Encumbrance", typeof(TRightObjEncumbrance), Order=2)]
        [XmlElement("NoEncumbrance", typeof(object), Order=2)]
        public List<object> Items1 { get; set; }
        
        /// <summary>
        /// TRightObj class constructor
        /// </summary>
        public TRightObj()
        {
            Items1 = new List<object>();
            Items = new List<object>();
        }
    }
    
    /// <summary>
    /// РџСЂР°РІРѕРѕР±Р»Р°РґР°С‚РµР»СЊ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tRightObjOwner")]
    [XmlRootAttribute("tRightObjOwner")]
    public class TRightObjOwner : TSubject
    {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tRightObjEncumbrance")]
    [XmlRootAttribute("tRightObjEncumbrance")]
    public class TRightObjEncumbrance : TEmcumbrance
    {
        [XmlAttribute(AttributeName="EncumbNumber")]
        public string EncumbNumber { get; set; }
    }
    
    /// <summary>
    /// Р°С‚СЂРёР±СѓС‚С‹ Р·Р°РїСЂРѕСЃР° Рё РёС‚РѕРіРѕРІРѕРіРѕ РґРѕРєСѓРјРµРЅС‚Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlRootAttribute("tExtrAttribut")]
    public class TExtrAttribut
    {
        /// <summary>
        /// РђРґСЂРµСЃР°С‚
        /// </summary>
        [XmlElement(Order=0)]
        [StringLengthAttribute(512)]
        public string ReceivName { get; set; }
        /// <summary>
        /// РЎСѓР±СЉРµРєС‚, РѕС‚ РёРјРµРЅРё РєРѕС‚РѕСЂРѕРіРѕ РґРµР№СЃС‚РІСѓРµС‚ Р°РґСЂРµСЃР°С‚
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(512)]
        public string Representativ { get; set; }
        /// <summary>
        /// РђРґСЂРµСЃ
        /// </summary>
        [XmlElement(Order=2)]
        [StringLengthAttribute(2000)]
        public string ReceivAdress { get; set; }
        /// <summary>
        /// РЈРЅРёРєР°Р»СЊРЅС‹Р№ РёРґРµРЅС‚РёС„РёРєР°С‚РѕСЂ Р·Р°РїРёСЃРё  РљРЈР’Р
        /// </summary>
        [XmlAttribute(AttributeName="ID_KUVI")]
        public decimal ID_KUVI { get; set; }
        /// <summary>
        /// РєРѕРґ Р·Р°РїСЂРѕС€РµРЅРЅРѕР№ РёРЅС„РѕСЂРјР°С†РёРё
        /// </summary>
        [XmlAttribute(AttributeName="ExtractTypeCode")]
        public DTypeInfo ExtractTypeCode { get; set; }
        /// <summary>
        /// РІРёРґ Р·Р°РїСЂРѕС€РµРЅРЅРѕР№ РёРЅС„РѕСЂРјР°С†РёРё С‚РµРєСЃС‚
        /// </summary>
        [XmlAttribute(AttributeName="ExtractTypeText")]
        [StringLengthAttribute(255)]
        public string ExtractTypeText { get; set; }
        /// <summary>
        /// РЅРѕРјРµСЂ РІС‹РїРёСЃРєРё/СЃРїСЂР°РІРєРё/СЃРѕРѕР±С‰РµРЅРёСЏ
        /// </summary>
        [XmlAttribute(AttributeName="ExtractNumber")]
        [StringLengthAttribute(50)]
        public string ExtractNumber { get; set; }
        /// <summary>
        /// РґР°С‚Р° РІС‹РїРёСЃРєРё/СЃРїСЂР°РІРєРё/СЃРѕРѕР±С‰РµРЅРёСЏ
        /// </summary>
        [XmlAttribute(AttributeName="ExtractDate")]
        public string ExtractDate { get; set; }
        /// <summary>
        /// РЅРѕРјРµСЂ РґРѕРєСѓРјРµРЅС‚Р° Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlAttribute(AttributeName="RequeryNumber")]
        [StringLengthAttribute(50)]
        public string RequeryNumber { get; set; }
        /// <summary>
        /// РґР°С‚Р° РґРѕРєСѓРјРµРЅС‚Р° Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlAttribute(AttributeName="RequeryDate")]
        public string RequeryDate { get; set; }
        /// <summary>
        /// РёСЃС…РѕРґСЏС‰РёР№ РЅРѕРјРµСЂ СѓС‡СЂРµР¶РґРµРЅРёСЏ
        /// </summary>
        [XmlAttribute(AttributeName="OfficeNumber")]
        [StringLengthAttribute(50)]
        public string OfficeNumber { get; set; }
        /// <summary>
        /// РёСЃС…РѕРґСЏС‰Р°СЏ РґР°С‚Р° СѓС‡СЂРµР¶РґРµРЅРёСЏ
        /// </summary>
        [XmlAttribute(AttributeName="OfficeDate")]
        public string OfficeDate { get; set; }
        /// <summary>
        /// РєРѕР»РёС‡РµСЃС‚РІРѕ СЃС„РѕСЂРјРёСЂРѕРІР°РЅРЅС‹С… РІС‹РїРёСЃРѕРє
        /// </summary>
        [XmlAttribute(AttributeName="ExtractCount")]
        public int ExtractCount { get; set; }
        /// <summary>
        /// РєРѕР»РёС‡РµСЃС‚РІРѕ СЃС„РѕСЂРјРёСЂРѕРІР°РЅРЅС‹С… СѓРІРµРґРѕРјР»РµРЅРёР№
        /// </summary>
        [XmlAttribute(AttributeName="NoticeCount")]
        public int NoticeCount { get; set; }
        /// <summary>
        /// РєРѕР»РёС‡РµСЃС‚РІРѕ СЃС„РѕСЂРјРёСЂРѕРІР°РЅРЅС‹С… РѕС‚РєР°Р·РѕРІ
        /// </summary>
        [XmlAttribute(AttributeName="RefuseCount")]
        public int RefuseCount { get; set; }
        /// <summary>
        /// СЂРµРіРёСЃС‚СЂР°С‚РѕСЂ, РїРѕРґРїРёСЃР°РІС€РёР№ РІС‹РїРёСЃРєСѓ(СЃРїСЂР°РІРєСѓ)
        /// </summary>
        [XmlAttribute(AttributeName="Registrator")]
        [StringLengthAttribute(100)]
        public string Registrator { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlRootAttribute("dTypeInfo")]
    public enum DTypeInfo
    {
        [XmlEnumAttribute("010000")]
        Item010000,
        [XmlEnumAttribute("020000")]
        Item020000,
        [XmlEnumAttribute("030000")]
        Item030000,
        [XmlEnumAttribute("040000")]
        Item040000,
        [XmlEnumAttribute("050100")]
        Item050100,
        [XmlEnumAttribute("050200")]
        Item050200,
        [XmlEnumAttribute("060000")]
        Item060000,
        [XmlEnumAttribute("070000")]
        Item070000,
    }
    
    /// <summary>
    /// РџРѕР»СѓС‡Р°С‚РµР»СЊ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tServisInfRecipient")]
    [XmlRootAttribute("tServisInfRecipient")]
    public class TServisInfRecipient
    {
        /// <summary>
        /// РљРѕРґ РѕСЂРіР°РЅРёР·Р°С†РёРё РїРѕР»СѓС‡Р°С‚РµР»СЏ
        /// </summary>
        [XmlAttribute(AttributeName="Kod")]
        [StringLengthAttribute(12)]
        public string Kod { get; set; }
        /// <summary>
        /// РќР°РёРјРµРЅРѕРІР°РЅРёРµ РѕСЂРіР°РЅРёР·Р°С†РёРё РїРѕР»СѓС‡Р°С‚РµР»СЏ
        /// </summary>
        [XmlAttribute(AttributeName="Name")]
        [StringLengthAttribute(255)]
        public string Name { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [XmlTypeAttribute(AnonymousType=true, TypeName="tServisInfScope")]
    [XmlRootAttribute("tServisInfScope")]
    public enum TServisInfScope
    {
        EGRP,
        GKN,
    }
    
    /// <summary>
    /// Р’С‹РїРёСЃРєР° Рѕ РїСЂР°РІР°С… РѕС‚РґРµР»СЊРЅРѕРіРѕ Р»РёС†Р° РЅР° РћРќР
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true)]
    [XmlRootAttribute("ExtractReestrExtract")]
    public class ExtractReestrExtract
    {
        /// <summary>
        /// РРЅС„РѕСЂРјР°С†РёСЏ Рѕ Р·Р°РїСЂРѕСЃРµ
        /// </summary>
        [XmlElement(Order=0)]
        public TExtrAttribut DeclarAttribute { get; set; }
        /// <summary>
        /// РџРµСЂРёРѕРґ, Р·Р° РєРѕС‚РѕСЂС‹Р№ Р·Р°РїСЂРѕС€РµРЅС‹ СЃРІРµРґРµРЅРёСЏ
        /// </summary>
        [XmlElement(Order=1)]
        public ExtractReestrExtractExtractPeriod ExtractPeriod { get; set; }
        /// <summary>
        /// РІС‹РїРёСЃРєР° РѕР± РѕР±РѕР±С‰РµРЅРЅС‹С… РїСЂР°РІР°С… СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=2)]
        public ExtractReestrExtractExtractSubjectRights ExtractSubjectRights { get; set; }
        /// <summary>
        /// СѓРІРµРґРѕРјР»РµРЅРёРµ РѕР± РѕС‚СЃСѓС‚СЃС‚РІРёРё СЃРІРµРґРµРЅРёР№  РѕР± РѕР±РѕР±С‰РµРЅРЅС‹С… РїСЂР°РІР°С… СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=3)]
        public TNoticeSubj NoticeSubj { get; set; }
        /// <summary>
        /// РѕС‚РєР°Р· РІ РІС‹РґР°С‡Рµ РІС‹РїРёСЃРєРё РѕР± РѕР±РѕР±С‰РµРЅРЅС‹С… РїСЂР°РІР°С… СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=4)]
        public TRefusalSubj RefusalSubj { get; set; }
    }
    
    /// <summary>
    /// РџРµСЂРёРѕРґ, Р·Р° РєРѕС‚РѕСЂС‹Р№ Р·Р°РїСЂРѕС€РµРЅС‹ СЃРІРµРґРµРЅРёСЏ
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true)]
    [XmlRootAttribute("ExtractReestrExtractExtractPeriod")]
    public class ExtractReestrExtractExtractPeriod
    {
        /// <summary>
        /// Р”Р°С‚Р° РЅР°С‡Р°Р»Р° РїРµСЂРёРѕРґР°
        /// </summary>
        [XmlElement(Order=0)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string ExtractPeriodSDate { get; set; }
        /// <summary>
        /// Р”Р°С‚Р° РѕРєРѕРЅС‡Р°РЅРёСЏ РїРµСЂРёРѕРґР°
        /// </summary>
        [XmlElement(Order=1)]
        [RegularExpressionAttribute("([0-3][0-9].[0-1][0-9].\\d{4})?")]
        public string ExtractPeriodEDate { get; set; }
    }
    
    /// <summary>
    /// РІС‹РїРёСЃРєР° РѕР± РѕР±РѕР±С‰РµРЅРЅС‹С… РїСЂР°РІР°С… СЃСѓР±СЉРµРєС‚Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true)]
    [XmlRootAttribute("ExtractReestrExtractExtractSubjectRights")]
    public class ExtractReestrExtractExtractSubjectRights
    {
        /// <summary>
        /// РўРµРєСЃС‚РѕРІРѕРµ РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlElement(Order=0)]
        public THeadContent HeadContent { get; set; }
        /// <summary>
        /// РІС‹РїРёСЃРєР° РѕР± РѕР±РѕР±С‰РµРЅРЅС‹С… РїСЂР°РІР°С… СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement("ExtractSubj", Order=1)]
        public List<ExtractReestrExtractExtractSubjectRightsExtractSubj> ExtractSubj { get; set; }
        /// <summary>
        /// Р—Р°РІРµСЂС€Р°СЋС‰РёР№С‚РµРєСЃС‚ (СЃСЃС‹Р»РєР° РЅР° Р·Р°РєРѕРЅ Рё С‚.Рї.)
        /// </summary>
        [XmlElement(Order=2)]
        public TFootContent FootContent { get; set; }
        
        /// <summary>
        /// ExtractReestrExtractExtractSubjectRights class constructor
        /// </summary>
        public ExtractReestrExtractExtractSubjectRights()
        {
            ExtractSubj = new List<ExtractReestrExtractExtractSubjectRightsExtractSubj>();
        }
    }
    
    /// <summary>
    /// РІС‹РїРёСЃРєР° РѕР± РѕР±РѕР±С‰РµРЅРЅС‹С… РїСЂР°РІР°С… СЃСѓР±СЉРµРєС‚Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true)]
    [XmlRootAttribute("ExtractReestrExtractExtractSubjectRightsExtractSubj")]
    public class ExtractReestrExtractExtractSubjectRightsExtractSubj
    {
        /// <summary>
        /// РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕС€РµРЅРЅРѕРіРѕ СЃСѓР±СЉРµРєС‚Р°
        /// </summary>
        [XmlElement(Order=0)]
        public ExtractReestrExtractExtractSubjectRightsExtractSubjSubject Subject { get; set; }
        /// <summary>
        /// РЎСѓРјРјР°СЂРЅРѕРµ  РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕСЃР°
        /// </summary>
        [XmlElement(Order=1)]
        [StringLengthAttribute(2000)]
        public string HeadLast { get; set; }
        /// <summary>
        /// РћР±СЉРµРєС‚ РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
        /// </summary>
        [XmlElement("ObjectRight", Order=2)]
        public List<ExtractReestrExtractExtractSubjectRightsExtractSubjObjectRight> ObjectRight { get; set; }
        
        /// <summary>
        /// ExtractReestrExtractExtractSubjectRightsExtractSubj class constructor
        /// </summary>
        public ExtractReestrExtractExtractSubjectRightsExtractSubj()
        {
            ObjectRight = new List<ExtractReestrExtractExtractSubjectRightsExtractSubjObjectRight>();
        }
    }
    
    /// <summary>
    /// РѕРїРёСЃР°РЅРёРµ Р·Р°РїСЂРѕС€РµРЅРЅРѕРіРѕ СЃСѓР±СЉРµРєС‚Р°
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true)]
    [XmlRootAttribute("ExtractReestrExtractExtractSubjectRightsExtractSubjSubject")]
    public class ExtractReestrExtractExtractSubjectRightsExtractSubjSubject : TSubject
    {
    }
    
    /// <summary>
    /// РћР±СЉРµРєС‚ РЅРµРґРІРёР¶РёРјРѕСЃС‚Рё
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [Serializable]
    [DebuggerStepThrough]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType=true)]
    [XmlRootAttribute("ExtractReestrExtractExtractSubjectRightsExtractSubjObjectRight")]
    public class ExtractReestrExtractExtractSubjectRightsExtractSubjObjectRight : TRightSubj
    {
        [XmlAttribute(AttributeName="ObjectNumber")]
        public string ObjectNumber { get; set; }
    }
}
#pragma warning restore
