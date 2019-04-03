using System;
using System.Collections.Generic;
using System.Linq;
using Base.Attributes;
using Base.Entities.Complex.KLADR;
using Base.Enums;
using Base.Security;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.Contact.Entities
{
    [EnableFullTextSearch]
    public class EmployeeUser : BaseEmployee
    {
        private static readonly CompiledExpression<EmployeeUser, Gender> _gender =
            DefaultTranslationOf<EmployeeUser>.Property(x => x.Gender).Is(x => x.User != null ? x.User.Profile != null ? x.User.Profile.Gender : Gender.Male : Gender.Male);

        private static readonly CompiledExpression<EmployeeUser, DateTime?> _birthDate =
            DefaultTranslationOf<EmployeeUser>.Property(x => x.BirthDate)
                .Is(x => x.User != null ? x.User.Profile != null ? x.User.Profile.BirthDate : null : null);

        private static readonly CompiledExpression<EmployeeUser, int?> _profileID =
          DefaultTranslationOf<EmployeeUser>.Property(x => x.ProfileId).Is(x => x.User != null ? x.User.BaseProfileID : null);

        public int UserID { get; set; }        

        [DetailView(Visible = false)]
        public virtual User User { get; set; }

        [DetailView("Пол", Order = 10), ListView(Visible = false)]
        public Gender Gender => _gender.Evaluate(this);

        [DetailView("Дата рождения", Order = 11), ListView(Visible = false)]
        public DateTime? BirthDate => _birthDate.Evaluate(this);

         [DetailView(Visible = false)]
        public int? ProfileId => _profileID.Evaluate(this);
    }

}