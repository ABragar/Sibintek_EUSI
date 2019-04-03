using Base.Attributes;
using Base.Enums;
using Base.UI.Wizard;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Base.Security.Entities.Concrete
{
    public class AccessUserWizard : DecoratedWizardObject<User>
    {
        public override User GetObject()
        {
            return new User()
            {
                Profile = new SimpleProfile()
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
                    }
                },
                CategoryID = CategoryID,
            };
        }

        [SystemProperty]
        public int CategoryID { get; set; }

        [DetailView("Фотография"), ListView]
        [Image(Circle = true, DefaultImage = DefaultImage.NoPhoto)]
        public virtual FileData Image { get; set; }

        [MaxLength(100)]
        [DetailView("Фамилия", Required = true)]
        public string LastName { get; set; }

        [MaxLength(100)]
        [DetailView("Имя", Required = true)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [DetailView("Отчество")]
        public string MiddleName { get; set; }

        [DetailView("Пол"), ListView]
        public Gender Gender { get; set; }

        [DetailView("Email", Required = true)]
        [PropertyDataType(PropertyDataType.EmailAddress)]
        public string Email { get; set; }

        [DetailView("Пароль", Required = true)]
        [PropertyDataType(PropertyDataType.Password)]
        public string Password { get; set; }
    }
}
