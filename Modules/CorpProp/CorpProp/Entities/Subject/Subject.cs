using Base;
using CorpProp.Entities.FIAS;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Law;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Document;
using CorpProp.Helpers;

using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Base;
using CorpProp.Entities.DocumentFlow;
using System.ComponentModel;
using CorpProp.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using Base.ComplexKeyObjects.Superb;

namespace CorpProp.Entities.Subject
{
    /// <summary>
    /// Представляет делового партнера
    /// </summary>
    [EnableFullTextSearch]
    public class Subject : TypeObject, ISuperObject<Subject>
    {
        #region Constructor
        /// <summary>
        /// Инициализирует новый экземпляр класса Subject.
        /// </summary>
        public Subject()
        {
        }
        #endregion

        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; } = null;

        /// <summary>
        /// Получает или задает идентификатор СДП.
        /// </summary>
        /// <remarks>
        /// Для установления связи между СДП и ЕУП через КСК.
        /// </remarks>
        [DetailView(Name = "Идентификатор СДП", Order = 1, TabName = CaptionHelper.DefaultTabName, Required = true, ReadOnly = true)]
        [ListView(Order = 1)]
        [PropertyDataType(PropertyDataType.Text)]
        public string SDP { get; set; }

        /// <summary>
        /// Получает или задает ИД вида делового партнёра.
        /// </summary>
        public int? SubjectKindID { get; set; }

        /// <summary>
        /// Получает или задает вид делового партнёра.
        /// </summary>
        [DetailView(Name = "Вид делового партнёра", Order = 2, TabName = CaptionHelper.DefaultTabName, Required = true, ReadOnly = true)]
        public SubjectKind SubjectKind { get; set; }
       
        /// <summary>
        /// Получает или задает признак является субъектом МСП.
        /// </summary>
        [DetailView(Name = "Является субъектом МСП", Order = 3, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [DefaultValue(false)]
        public bool IsSubjectMSP { get; set; }

        /// <summary>
        /// Получает или задает ИД ОПФ.
        /// </summary>
        public int? OPFID { get; set; }

        /// <summary>
        /// Получает или задает ОПФ >.
        /// </summary>
        [DetailView(Name = "ОПФ", Order = 4, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public OPF OPF { get; set; }
        
        /// <summary>
        /// Получает или задает полное наименование.
        /// </summary>
        
        [FullTextSearchProperty]
        [DetailView(Name = "Полное наименование", Order = 5, TabName = CaptionHelper.DefaultTabName, Required = true, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Hidden = true)]
        public string FullName { get; set; }

        /// <summary>
        /// Получает или задает краткое наименование.
        /// </summary>
        
        [FullTextSearchProperty]
        
        [DetailView(Name = "Краткое наименование", Order = 6, TabName = CaptionHelper.DefaultTabName, 
            ReadOnly = true, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Order = 2)]
        public string ShortName { get; set; }


        /// <summary>
        /// Получает или задает дату регистрации.
        /// </summary>
        [DetailView(Name = "Дата регистрации", Order = 7, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Hidden = true)]
        public DateTime? DateRegistration { get; set; }
   
   /// <summary>
        /// Получает или задает ИД страны.
        /// </summary>
        public int? CountryID { get; set; }

        /// <summary>
        /// Получает или задает страну.
        /// </summary>
        [DetailView(Name = "Страна", Order = 8, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order = 3)]
        public SibCountry Country { get; set; }

        /// <summary>
        /// Получает или задает ИД Федерального округа.
        /// </summary>
        public int? FederalDistrictID { get; set; }

        /// <summary>
        /// Получает или задает Федеральный округ.
        /// </summary>
        [DetailView(Name = "Федеральный округ", Order = 9, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order = 4)]
        public SibFederalDistrict FederalDistrict { get; set; }

        /// <summary>
        /// Получает или задает ИД Региона.
        /// </summary>
        public int? RegionID { get; set; }

        /// <summary>
        /// Получает или задает Регион.
        /// </summary>
        [DetailView(Name = "Субъект РФ", Order = 10, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order = 5)]
        public SibRegion Region { get; set; }

        /// <summary>
        /// Получает или задает город.
        /// </summary>
        [DetailView(Name = "Город", Order = 11, TabName = CaptionHelper.DefaultTabName, Required = true, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        [ListView(Hidden = true)]
        public string City { get; set; }


        /// <summary>
        /// Получает или задает юридический адрес.
        /// </summary>
    
        [DetailView(Name = "Адрес юридический", Order = 12, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Hidden = true)]
        public string  AddressLegal { get; set; }

        /// <summary>
        /// Получает или задает фактический адрес.
        /// </summary>
        [DetailView(Name = "Адрес фактический", Order = 13, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Hidden = true)]
        public string AddressActual { get; set; }

        
        /// <summary>
        /// Получает или задает телефон.
        /// </summary>
        [MaxLength(255)]
        [PropertyDataType(PropertyDataType.PhoneNumber)]
        [DetailView(Name = "Телефон", Order = 14, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [ListView(Order = 6)]
        public string Phone { get; set; }

        /// <summary>
        /// Получает или задает факс.
        /// </summary>
        [MaxLength(30)]
        [PropertyDataType(PropertyDataType.PhoneNumber)]
        [DetailView(Name = "Факс", Order = 15, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public string Fax { get; set; }

        /// <summary>
        /// Получает или задает адрес электронной почты.
        /// </summary>
        [MaxLength(255)]
        [DetailView(Name = "Адрес электронной почты", Order = 16, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        [ListView(Order = 7)]
        public string Email { get; set; } 
        
        /// <summary>
        /// Получает или задает ФИО руководителя.
        /// </summary>
        
        [DetailView(Name = "Ф.И.О. руководителя", Order = 17, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string HeadName { get; set; }

        /// <summary>
        /// Получает или задает должность руководителя.
        /// </summary>
        
        [DetailView(Name = "Должность руководителя", Order = 18, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string HeadPosition { get; set; }


        /// <summary>
        /// Получает или задает идентификатор КСК.
        /// </summary>
        [DetailView(Name = "Идентификатор КСК", Order = 19, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string KSK { get; set; }

        /// <summary>
        /// Получает или задает ИД типа делового партнёра.
        /// </summary>
        public int? SubjectTypeID { get; set; }

        /// <summary>
        /// Получает или задает тип делового партнёра.
        /// </summary>
        [DetailView(Name = "Тип делового партнёра", Order = 20, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        public SubjectType SubjectType { get; set; }

        
        ///// <summary>
        ///// Получает или задает ИД юридического адреса.
        ///// </summary>
        //public int? AddressLegalID { get; set; }

        ///// <summary>
        ///// Получает или задает юридический адрес.
        ///// </summary>
        ////[ListView]
        //[DetailView(Name = "Адрес юридический", Order = 8, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //public SibAddress AddressLegal { get; set; }

        ///// <summary>
        ///// Получает или задает ИД фактического адреса.
        ///// </summary>
        //public int? AddressActualID { get; set; }

        ///// <summary>
        ///// Получает или задает фактический адрес.
        ///// </summary>
        //[DetailView(Name = "Адрес фактический", Order = 9, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //public SibAddress AddressActual { get; set; }

        /// <summary>
        /// Получает или задает ИНН.
        /// </summary>
        [UIHint("Integer")]       
        [DetailView(Name = "ИНН", Order = 1, TabName = "[2]Коды", ReadOnly = true)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=INN")]
        [ListView(Hidden = false, Order = 0, Visible = true)]
        public string INN { get; set; }

        /// <summary>
        /// Получает или задает ОКПО.
        /// </summary>
        [DetailView(Name = "ОКПО", Order = 2, TabName = "[2]Коды", ReadOnly = true)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=OKPO")]
        public string OKPO { get; set; }

        /// <summary>
        /// Получает или задает КПП.
        /// </summary>
        [DetailView(Name = "КПП", Order = 3, TabName = "[2]Коды", ReadOnly = true)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=KPP")]
        public string KPP { get; set; }

        /// <summary>
        /// Получает или задает ИД ОКТМО.
        /// </summary>
        public int? OKTMOID { get; set; }

        /// <summary>
        /// Получает или задает ОКТМО >.
        /// </summary>
        [DetailView(Name = "ОКТМО", Order = 4, TabName = "[2]Коды", ReadOnly = true)]
        public OKTMO OKTMO { get; set; }

        /// <summary>
        /// Получает или задает ОГРН.
        /// </summary>
        [DetailView(Name = "ОГРН", Order = 5, TabName = "[2]Коды", ReadOnly = true)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=OGRN")]
        [ListView(Hidden = true)]
        public string OGRN { get; set; }

        /// <summary>
        /// Получает или задает ОГРНИП.
        /// </summary>
        [DetailView(Name = "ОГРНИП", Order = 6, TabName = "[2]Коды", ReadOnly = true)]
        [PropertyDataType("Sib_TextOrganisationInfo", Params = "Type=OGRNIP")]
        [ListView(Hidden = true)]
        public string OGRNIP { get; set; }
        
      
        /// <summary>
        /// Получает или задает ИД ОКАТО.
        /// </summary>
        public int? OKATOID { get; set; }

        /// <summary>
        /// Получает или задает ОКАТО.
        /// </summary>
        [DetailView(Name = "ОКАТО", Order = 19, TabName = "[2]Коды", ReadOnly = true)]      
        public OKATO OKATO { get; set; }


        /// <summary>
        /// Получает или задает ИД ОКВЭД.
        /// </summary>
        public int? OKVEDID { get; set; }

        /// <summary>
        /// Получает или задает ОКВЭД.
        /// </summary>
        [DetailView(Name = "ОКВЭД", Order = 15, TabName = "[2]Коды", ReadOnly = true, Visible = false)]      
        public SibOKVED OKVED { get; set; }

              
        /// <summary>
        /// Получает или задает признак является оценивающей организацией.
        /// </summary>
        [DetailView(Name = "Является оценивающей организацией", Order = 23, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        [DefaultValue(false)]
        public bool IsAppraiser { get; set; }

        
        ///// <summary>
        ///// Получает или задает примечание.
        ///// </summary>
        //[DetailView(Name = "Примечание", Order = 24, TabName = CaptionHelper.DefaultTabName, ReadOnly = true)]
        //public string Description { get; set; }

        /// <summary>
        /// Получает или задает идентификатор Общества группы.
        /// </summary> 
        public int? SocietyID { get; set; }

        /// <summary>
        /// Получает или задает Общество группы.
        /// </summary>     
        [ListView(Hidden = true)]
        [DetailView(Name = "Общество группы", Order = 26, TabName = CaptionHelper.DefaultTabName, ReadOnly = true, Visible = true)]
        public Society Society { get; set; }
    }
}
