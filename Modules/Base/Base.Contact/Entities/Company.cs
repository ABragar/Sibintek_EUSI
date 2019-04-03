using Base.Attributes;
using Base.Entities;
using Base.Entities.Complex.KLADR;
using Base.EntityFrameworkTypes.Complex;
using Base.Translations;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Map;

namespace Base.Contact.Entities
{
    [EnableFullTextSearch]
    public class Company : BaseContact, IGeoObject
    {
        private static readonly CompiledExpression<Company, string> _name =
             DefaultTranslationOf<Company>.Property(x => x.Name).Is(x => x.ShortTitle != null && x.ShortTitle.Trim() != "" ? x.ShortTitle : x.Title);

        public Company()
        {
            Location = new Location(); ;
        }

        [SystemProperty]
        public string Name => _name.Evaluate(this);

        [MaxLength(255)]
        [DetailView("Сокращенное название", Order = 30)]
        public string ShortTitle { get; set; }

        [DetailView(TabName = "[3]Адреса")]
        public virtual ICollection<CompanyAddress> Addresses { get; set; }

        public int? MainContactID { get; set; }
        [DetailView(TabName = "[4]Контакты", Visible = false, Order = 10), ListView("Основ. контакт")]
        public virtual BaseEmployee MainContact { get; set; }

        [DetailView("Форма осуществления деятельности", TabName = "[5]Реквизиты")]
        public IncorporationForm IncorporationForm { get; set; }

        [DetailView("Система налогообложения", TabName = "[5]Реквизиты")]
        public TaxForm TaxForm { get; set; }

        [DetailView("ИНН", TabName = "[5]Реквизиты", Required = true)]
        [PropertyDataType(PropertyDataType.Inn)]
        public string Inn { get; set; }

        [DetailView("КПП", TabName = "[5]Реквизиты")]
        [PropertyDataType(PropertyDataType.Kpp)]
        [MaxLength(9)]
        public string Kpp { get; set; }

        public int? OkvedID { get; set; }
        [DetailView("ОКВЭД", TabName = "[5]Реквизиты")]
        public virtual OkvedType Okved { get; set; }

        [DetailView("ОКПО", TabName = "[5]Реквизиты")]
        [PropertyDataType(PropertyDataType.Okpo)]
        public string Okpo { get; set; }

        [DetailView("ОКТМО", TabName = "[5]Реквизиты")]
        [PropertyDataType(PropertyDataType.Oktmo)]
        public string Oktmo { get; set; }

        [DetailView("ОГРН", TabName = "[5]Реквизиты")]
        [PropertyDataType(PropertyDataType.Ogrn)]
        public string Ogrn { get; set; }

        [DetailView("Дата выдачи свидетельство о регистрации в ЕГРЮЛ", TabName = "[5]Реквизиты")]
        public DateTime? DateEgrul { get; set; }

        [DetailView("Серия и номер свидетельства о регистрации в ЕГРЮЛ", TabName = "[5]Реквизиты")]
        [MaxLength(255)]
        public string Egrul { get; set; }

        [DetailView(TabName = "[6]Платежные реквизиты")]
        public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();


        [ListView("Местоположение")]
        [DetailView(TabName = "[7]Местоположение")]
        [PropertyDataType(PropertyDataType.LocationPoint)]
        public Location Location { get; set; }


        [DetailView(TabName = "[8]Роли", Order = 100)]
        public virtual ICollection<RoleCompany> CompanyRoles { get; set; }
    }

    public class RoleCompany : EasyCollectionEntry<CompanyRole>
    {

    }

    public class PaymentDetail : BaseObject
    {
        public int? BankId { get; set; }
        [ListView]
        [DetailView("Банк", Required = true)]
        public virtual Bank Bank { get; set; }

        [DetailView("Расчетный счёт", Required = true)]
        [ListView]
        [MaxLength(255)]
        public string BankAccount { get; set; }

        [DetailView("Расчетный счёт", Required = true)]
        [ListView]
        public BankAccountType BankAccountType { get; set; }

        [DetailView("Корр. счёт", Required = true)]
        [ListView]
        [MaxLength(255)]
        public string KorBankAccount { get; set; }       
        
    }

    

    public class CompanyAddress : BaseObject
    {
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }

        [DetailView("Тип"), ListView]
        public AddressType Type { get; set; }

        [DetailView("Адрес"), ListView]
        public Address Address { get; set; } = new Address();
    }

    [UiEnum]
    public enum AddressType
    {
        [UiEnumValue("Фактический")]
        Factual,
        [UiEnumValue("Юридический")]
        Legal,
        [UiEnumValue("Адрес филиала")]
        FilialAddress
    }

    [UiEnum]
    public enum IncorporationForm
    {
        [UiEnumValue("ООО")]
        OOO,
        [UiEnumValue("ПАО")]
        PAO,
        [UiEnumValue("АО")]
        AO,
        [UiEnumValue("ИП")]
        IP
    }

    [UiEnum]
    public enum TaxForm
    {
        [UiEnumValue("ОСНО")]
        OSNO,
        [UiEnumValue("УСН")]
        USN,
        [UiEnumValue("ЕНВД")]
        ENVD
    }

    [UiEnum]
    public enum BankAccountType
    {
        [UiEnumValue("Основной")]
        Main,
        [UiEnumValue("Кредитный")]
        Credit
    }

}