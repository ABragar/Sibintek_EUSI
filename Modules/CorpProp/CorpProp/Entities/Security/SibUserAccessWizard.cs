using Base.Attributes;
using Base.DAL;
using Base.Entities.Complex;
using Base.Enums;
using Base.Security;
using Base.Security.Entities.Concrete;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Security
{
    public class SibUserAccessWizard : AccessUserWizard
    {

        [DetailView("Логин", Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public virtual string Login { get; set; }

        [DetailView("Активный", Visible = false)]
        public bool IsActive { get; set; } = true;


        [DetailView("Общество группы")]
        public Society Society { get; set; }

        [DetailView("Подразделение")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Department { get; set; }

        [DetailView("Должность")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Post { get; set; }

        /// <summary>
        /// Получает или задает рабочий телефон.
        /// </summary>
        [DetailView(Name = "Телефон раб.")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Phone { get; set; }

        /// <summary>
        /// Получает или задает мобильный телефон.
        /// </summary>
        [DetailView(Name = "Телефон моб.")]
        [PropertyDataType(PropertyDataType.Text)]
        public string Mobile { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView(Name = "Примечание")]
        public string Description { get; set; }

        [DetailView("Email")]
        [PropertyDataType(PropertyDataType.Text)]
        public new string Email { get; set; }

        /// <summary>
        /// Переопределяет GetObject.
        /// </summary>
        /// <returns>Пользователь с профилем SibUser</returns>
        public override User GetObject()
        {
            User us = new User()
            {
                SysName = Login,
                CategoryID = CategoryID,
                IsActive = this.IsActive
            };
            SibUser profile = new SibUser()
            {
                Image = Image,
                LastName = LastName,
                FirstName = FirstName,
                MiddleName = MiddleName,
                Gender = Gender,
                Emails = new List<ProfileEmail>()
                    {
                        new ProfileEmail()
                        {
                            IsPrimary = true,
                            Email = Email,
                        }
                    },
                Phones = new List<ProfilePhone>()
                {
                        new ProfilePhone()
                        {
                           Phone = new Phone() { Type = PhoneType.Work, Number = this.Phone }
                        },
                        new ProfilePhone()
                        {
                           Phone = new Phone() { Type = PhoneType.Mobile, Number = this.Mobile }
                        }
                },

                DeptName = this.Department,
                PostName = this.Post,                
                Phone = this.Phone,
                Mobile = this.Mobile,
                Email = this.Email,
                Description = this.Description                
            };           
            
            profile.User = us;
            us.Profile = profile;

            return us;
        }
    }
}
