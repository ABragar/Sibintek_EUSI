using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Entities;
using Base.Enums;
using Base.Security;
using Base.Utils.Common.Attributes;

namespace Base.Contact.Entities
{
    [EnableFullTextSearch]
    public class BaseContact : BaseObject, ISuperObject<BaseContact>
    {
        [Image(DefaultImage = DefaultImage.NoPhoto)]
        [DetailView("Изображение", Order = 0), ListView(Width = 100, Height = 100, Order = 1)]
        public virtual FileData Image { get; set; }

        [FullTextSearchProperty]
        [MaxLength(255)]
        [DetailView("Наименование", Required = true, Order = 2), ListView(Order = 2)]
        public string Title { get; set; }

        public int? ResponsibleID { get; set; }
        [DetailView("Ответственный", Required = true, Order = 100), ListView(Visible = false)]
        public virtual User Responsible { get; set; }

        [DetailView("Сайт", TabName = "[2]Контакты")]
        [PropertyDataType(PropertyDataType.Url)]
        public string Site { get; set; }

        [DetailView("Телефоны", TabName = "[2]Контакты")]
        public virtual ICollection<ContactPhone> Phones { get; set; }

        [DetailView("E-mails", TabName = "[2]Контакты")]
        public virtual ICollection<ContactEmail> Emails { get; set; }

        //[DetailView(TabName = "[8]Интересы")]
        //public virtual ICollection<ContactInterest> ContactInterests { get; set; } = new List<ContactInterest>();

        [DetailView(TabName = "[9]Примечание")]
        [PropertyDataType(PropertyDataType.SimpleHtml)]
        public string Description { get; set; }

        [ListView("Тип")]
        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; } = null;

        public int? CompanyTypeID { get; set; }
        [DetailView("Тип компании"), ListView]
        public virtual CompanyType CompanyType { get; set; }
    }
}
