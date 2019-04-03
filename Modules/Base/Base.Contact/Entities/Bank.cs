using Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Contact.Entities
{
    public class Bank : BaseObject
    {
        [ListView]
        [DetailView("Наименование", Order = 5, Required = true)]
        [MaxLength(255)]
        public string Title { get; set; }

        [DetailView("Номер лицензии", Order = 10, Required = true)]
        [MaxLength(255)]
        public string LicenseNumber { get; set; }

        [DetailView("Дата выдачи лицензии", Order = 20, Required = true)]
        [PropertyDataType(PropertyDataType.Date)]
        public DateTime LicenseDate { get; set; }

        [DetailView("Регистрационный номер", Order = 30, Required = true)]
        [MaxLength(255)]
        public string RegistrationNumber { get; set; }

        [DetailView("БИК", Order = 40, Required = true)]
        [PropertyDataType(PropertyDataType.Bik)]
        public string Bik { get; set; }

        [DetailView("ИНН", Order = 50, Required = true)]
        [PropertyDataType(PropertyDataType.Inn)]
        public string Inn { get; set; }

        [DetailView("КПП", Order = 60, Required = true)]
        [PropertyDataType(PropertyDataType.Kpp)]
        public string Kpp { get; set; }

        [DetailView("ОГРН", Order = 70, Required = true)]
        [PropertyDataType(PropertyDataType.Ogrn)]
        public string Ogrn { get; set; }
    }
}
