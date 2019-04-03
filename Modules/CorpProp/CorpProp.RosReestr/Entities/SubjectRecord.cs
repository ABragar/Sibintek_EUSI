using Base;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Entities
{
    /// <summary>
    /// Сведения о субъекте
    /// </summary>
    public class SubjectRecord : BaseObject, ISuperObject<SubjectRecord>
    {
        public SubjectRecord()
        {
            //Partners = new List<SubjectRecord>();
        }

        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; }

        /// <summary>
        /// ИД субъекта в Росреестре
        /// </summary>
        public string IDSubject { get; set; }

        #region  Иной субъект права AnotherRight

        /// <summary>
        /// Собственники помещений в многоквартирном доме
        /// </summary>      
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Собственники помещений в многоквартирном доме")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Aparthouse_owners_name { get; set; }

        /// <summary>
        /// Государственный регистрационный номер выпуска облигаций
        /// </summary>      
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Владельцы облигаций - Государственный регистрационный номер выпуска облигаций")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Bonds_number { get; set; }

        /// <summary>
        /// Дата государственной регистрации номера выпуска облигаций
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Владельцы облигаций - Дата государственной регистрации номера выпуска облигаций")]     
        public System.DateTime? Issue_date { get; set; }

        /// <summary>
        /// Владельцы ипотечных сертификатов участия
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Владельцы ипотечных сертификатов участия",
            Description = "Индивидуальное обозначение, идентифицирующее ипотечные сертификаты участия, в интересах владельцев которых осуществляется доверительное управление таким ипотечным покрытием")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Certificate_name { get; set; }

        /// <summary>
        /// Владельцы инвестиционных паев
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Владельцы инвестиционных паев", Description = "Название (индивидуальное обозначение), идентифицирующее паевой инвестиционный фонд")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Investment_unit_name { get; set; }


        /// <summary>
        /// Участники долевого строительства
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Участники долевого строительства")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Equity_participants { get; set; }

        /// <summary>
        /// Участники долевого строительства по договорам участия в долевом строительстве, которым не переданы объекты долевого строительства
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Участники долевого строительства по договорам участия в долевом строительстве, которым не переданы объекты долевого строительства")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Not_equity_participants { get; set; }

        /// <summary>
        /// Публичный сервитут
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Публичный сервитут")]
        [PropertyDataType(PropertyDataType.Text)]
        public string PublicServitude { get; set; }

        /// <summary>
        /// Не определено
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Не определено")]
        [PropertyDataType(PropertyDataType.Text)]
        public string undefined { get; set; }

        #region OtherSubject
        /// <summary>
        /// Наименование
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true,Name = "Наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }
        /// <summary>
        /// Краткое наименование
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true,Name = "Краткое наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Short_name { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Комментарий")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Comment { get; set; }
        /// <summary>
        /// Наименование для печати
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Наименование для печати")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Print_text { get; set; }
        /// <summary>
        /// Регистрирующий орган
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Регистрирующий орган")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Registration_organ { get; set; }


        #endregion //OtherSubject

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "ИД партнера")]
        
        public int? PartnerID { get; set; }
        public virtual SubjectRecord Partner { get; set; }

        ///// <summary>
        ///// Участники договора инвестиционного товарищества
        ///// </summary>     
        //[InverseProperty("Partner")]
        //public virtual ICollection<SubjectRecord> Partners { get; set; }

        #endregion //AnotherRight

        #region IndividualOut

        /// <summary>
        /// Тип физического лица
        /// </summary>        
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Код типа физического лица")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Individual_typeCode { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Тип физического лица")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Individual_typeName { get; set; }

        ///// <summary>
        ///// Фамилия
        ///// </summary>        
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "Фамилия")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string Surname { get; set; }

        ///// <summary>
        ///// Имя
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "Имя")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string FirstName { get; set; }

        ///// <summary>
        ///// Отчество
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "Отчество")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string Patronymic { get; set; }

        ///// <summary>
        ///// Дата рождения
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "Дата рождения")]      
        //public System.DateTime? Birth_date { get; set; }

        ///// <summary>
        ///// Место рождения
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "Место рождения")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string Birth_place { get; set; }

        ///// <summary>
        ///// Без гражданства
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "Без гражданства")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string No_citizenship { get; set; }

        ///// <summary>
        ///// Страна гражданства
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "Код страны гражданства")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string Citizenship_countryCode { get; set; }

        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "Страна гражданства")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string Citizenship_countryName { get; set; }


        ///// <summary>
        ///// СНИЛС
        ///// </summary>
        //[ListView(Hidden = true)]
        //[DetailView(ReadOnly = true,Name = "СНИЛС")]
        //[PropertyDataType(PropertyDataType.Text)]
        //public string Snils { get; set; }

       


        #endregion //IndividualOut

        #region LegalEntityOut
        /// <summary>
        /// Тип юридического лица
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Код типа юридического лица")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeCode { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Тип юридического лица")]
        [PropertyDataType(PropertyDataType.Text)]
        public string TypeName { get; set; }


        #region Юридическое лицо, орган власти GovementEntity

        /// <summary>
        /// Полное наименование
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Полное наименование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Full_name { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true,Name = "ИНН")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Inn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true,Name = "ОГРН")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Ogrn { get; set; }
        #endregion //Юридическое лицо, орган власти GovementEntity


        #region Иностранное юридическое лицо NotResidentOut
        /// <summary>
        /// Организационно-правовая форма
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Организационно-правовая форма (Код)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Incorporation_formCode { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Организационно-правовая форма")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Incorporation_formName { get; set; }


        /// <summary>
        /// Страна регистрации (инкорпорации)
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Код страны регистрации (инкорпорации)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Incorporate_countryCode { get; set; }
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Страна регистрации (инкорпорации)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Incorporate_countryName { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true,Name = "Регистрационный номер")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Registration_number { get; set; }

        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        [ListView]
        [DetailView(ReadOnly = true,Name = "Дата государственной регистрации")]
        [PropertyDataType(PropertyDataType.Text)]
        public System.DateTime? Date_state_reg { get; set; }

        /// <summary>
        /// Адрес (местонахождение) в стране регистрации (инкорпорации)
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Адрес (местонахождение) в стране регистрации (инкорпорации)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Reg_address_subject { get; set; }

        #endregion //Иностранное юридическое лицо NotResidentOut
        #endregion

        #region PublicFormations     


        /// <summary>
        /// Полное наименование иностранного государства
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Код иностранного государства")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ForeignPublicCode { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Полное наименование иностранного государства")]
        [PropertyDataType(PropertyDataType.Text)]
        public string ForeignPublicName { get; set; }


        /// <summary>
        /// Муниципальное образование
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Муниципальное образование")]
        [PropertyDataType(PropertyDataType.Text)]
        public string MunicipalityName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Код РФ")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RussiaCode { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Российская Федерация")]
        [PropertyDataType(PropertyDataType.Text)]
        public string RussiaName { get; set; }

        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Субъект Российской Федерации (Код)")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubjectOfRFCode { get; set; }

        /// <summary>
        /// Наименование субъекта Российской Федерации
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Субъект Российской Федерации")]
        [PropertyDataType(PropertyDataType.Text)]
        public string SubjectOfRFName { get; set; }

        /// <summary>
        /// Союзное государство
        /// </summary>
        [ListView(Hidden = true)]
        [DetailView(ReadOnly = true,Name = "Союзное государство")]
        [PropertyDataType(PropertyDataType.Text)]
        public string UnionStateName { get; set; }

        #endregion //PublicFormations

        #region Contact

        [ListView]
        [DetailView(ReadOnly = true,Name = "Email")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Email { get; set; }

        [ListView]
        [DetailView(ReadOnly = true,Name = "Почтовый адрес")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Mailing_addess { get; set; }
        #endregion



        /// <summary>
        /// Субъект правоотношений (код)
        /// </summary>        
        public string Code_SP { get; set; }

        /// <summary>
        /// Субъект правоотношений (наименование)
        /// </summary>
        public string Code_SPName { get; set; }

        /// <summary>
        /// Суммароное описание субъекта
        /// </summary>      
        public string Content { get; set; }
       


      

     
        

    }
}
