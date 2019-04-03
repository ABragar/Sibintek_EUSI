using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.Entities.Complex.KLADR;
using Base.Translations;
using Base.UI.ViewModal;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace Base.Contact.Entities
{
    [EnableFullTextSearch]
    public class BaseEmployee : BaseContact, ICategorizedItem

    {
        private static CompiledExpression<BaseEmployee, Company> _company =
            DefaultTranslationOf<BaseEmployee>.Property(x => x.Company).Is(x => x.Department != null ? x.Department.Company : null);


        [MaxLength(100)]
        [DetailView("Фамилия", Required = true, Order = 2)]
        public string LastName { get; set; }

        [MaxLength(100)]
        [DetailView("Имя", Order = 3)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [DetailView("Отчество", Order = 4)]
        public string MiddleName { get; set; }

        [DetailView("Источник", Order = 120)]
        public SourceContact Source { get; set; }

        [DetailView("Адрес", TabName = "[2]Контакты")]
        public Address Address { get; set; } = new Address();
        
        [DetailView(TabName = "[5]Семья")]
        public virtual ICollection<FamilyMember> Family { get; set; } = new List<FamilyMember>();

        [DetailView(TabName = "[6]Представители")]
        [InverseProperty("BaseEmployee")]
        public virtual ICollection<BasePersonAgent> Agents { get; set; } = new List<BasePersonAgent>();

        [DetailView("Компания", TabName = "[7]Место работы")]
        public virtual Company Company => _company.Evaluate(this);    

        #region ICategorizedItem                
        [DetailView("Отдел", TabName = "[7]Место работы", Required = true)]
        public virtual Department Department { get; set; }

        [ForeignKey("Department")]
        public int CategoryID { get; set; }
        HCategory ICategorizedItem.Category => this.Department;
        #endregion

        public int? PostID { get; set; }
        [ListView(Order = 7)]
        [DetailView("Должность", TabName = "[7]Место работы")]
        public virtual EmployeePost Post { get; set; }
    }

    [UiEnum]
    public enum SourceContact
    {
        [UiEnumValue("Свой контакт")]
        Self = 0,
        [UiEnumValue("Существующий клиент")]
        Partner = 10,
        [UiEnumValue("Звонок")]
        Call = 20,
        [UiEnumValue("Вэб-сайт")]
        Web = 30,
        [UiEnumValue("Электронная почта")]
        Email = 40,
        [UiEnumValue("Конференция")]
        Conference = 50,
        [UiEnumValue("Выставка")]
        TradeShow = 60,
        [UiEnumValue("HR - департамент")]
        Hr = 70,
        [UiEnumValue("Сотрудник")]
        Emplpyee = 80,
        [UiEnumValue("Другое")]
        Other = 200
    }

   
}