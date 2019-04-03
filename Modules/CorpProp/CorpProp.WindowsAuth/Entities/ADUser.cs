using Base;
using Base.Attributes;
using Base.Translations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.WindowsAuth.Entities
{
    public class ADUser : BaseObject
    {
        public ADUser()
        {

        }

        private static readonly CompiledExpression<ADUser, string> _fullname =
            DefaultTranslationOf<ADUser>.Property(x => x.FullName).Is(x => x.LastName + " " + (x.FirstName ?? "").Trim() + " " + (x.MiddleName ?? "").Trim());

        #region Account
        [DetailView("Логин", Required = true, ReadOnly = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Login { get; set; }
        #endregion

        #region General

        [DetailView("Ф.И.О")]
        public string FullName => _fullname.Evaluate(this);

        [MaxLength(100)]
        [DetailView("Имя", Required = true)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [DetailView("Отчество")]
        public string MiddleName { get; set; }

        [MaxLength(100)]
        [DetailView("Фамилия", Required = true)]
        public string LastName { get; set; }

        [DetailView(Name = "Примечание")]
        public string Description { get; set; }
        public string Office { get; set; }

        [DetailView("Email")]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        public string Email { get; set; }

        [DetailView(Name = "Телефон раб.")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Phone { get; set; }

        [DetailView(Name = "Телефон моб.")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Mobile { get; internal set; }
        #endregion

        #region Organization
        [DetailView("Подразделение")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Department { get; set; }

        [DetailView("Должность")]
        [PropertyDataType(PropertyDataType.Text)]
        public string JobTitle { get; set; }

        [DetailView("Компания")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Company { get; set; }
        #endregion
    }
}
