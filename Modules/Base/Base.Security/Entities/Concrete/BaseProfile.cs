using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Entities.Complex;
using Base.Entities.Complex.KLADR;
using Base.Enums;
using Base.Translations;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Base.Security
{

    [EnableFullTextSearch]
    public abstract class BaseProfile : BaseObject, ISuperObject<BaseProfile>, ICloneable
    {
        protected BaseProfile()
        {
            Address = new Address();
        }

        private static readonly CompiledExpression<BaseProfile, string> _fullname =
            DefaultTranslationOf<BaseProfile>.Property(x => x.FullName).Is(x => x.LastName + " " + (x.FirstName ?? "").Trim() + " " + (x.MiddleName ?? "").Trim());

        [ListView("Ф.И.О.")]
        [DetailView(Visible = false)]
        [FullTextSearchProperty]
        public string FullName => _fullname.Evaluate(this);

        public int? ImageID { get; set; }
        [SystemProperty]
        [DetailView("Фотография"), ListView]
        [Image(Circle = true)]
        public virtual FileData Image { get; set; }

        [MaxLength(100)]
        [DetailView("Фамилия", Required = true)]
        public string LastName { get; set; }
        [MaxLength(100)]
        [DetailView("Имя")]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [DetailView("Отчество")]
        public string MiddleName { get; set; }
        [DetailView("Пол"), ListView]
        public Gender Gender { get; set; }
        [DetailView("Дата рождения"), ListView]
        public virtual DateTime? BirthDate { get; set; }
        [DetailView("Адрес")]
        public Address Address { get; set; }

        [DetailView(TabName = "[1]Электронная почта")]
        public virtual ICollection<ProfileEmail> Emails { get; set; } = new List<ProfileEmail>();

        [DetailView(TabName = "[1]Телефоны")]
        public virtual ICollection<ProfilePhone> Phones { get; set; } = new List<ProfilePhone>();

        public string GetPrimaryEmail() => Emails?.FirstOrDefault(x => x.IsPrimary)?.Email;

        public bool IsEmpty { get; set; }

        public string ExtraID { get; } = null;
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class ProfilePhone : BaseObject
    {
        private static readonly CompiledExpression<ProfilePhone, string> _title =
            DefaultTranslationOf<ProfilePhone>.Property(x => x.Title).Is(x => "(" + x.Phone.Code + ")" + x.Phone.Number);

        [ListView]
        [DetailView("Номер телефона", Visible = false)]
        public string Title => _title.Evaluate(this);

        public int BaseProfileID { get; set; }
        public BaseProfile BaseProfile { get; set; }

        [DetailView("Телефон")]
        public Phone Phone { get; set; } = new Phone();
    }


    public class ProfileEmail : BaseEmail
    {
        public int BaseProfileID { get; set; }
        public BaseProfile BaseProfile { get; set; }

        [DetailView("Основной"), ListView]
        public bool IsPrimary { get; set; }
    }
}
