using System;
using System.Collections.Generic;
using Base.Attributes;
using Base.Entities.Complex.KLADR;
using Base.Enums;
using Base.Utils.Common.Attributes;

namespace Base.Contact.Entities
{
    [EnableFullTextSearch]
    public class Employee : BaseEmployee
    {
        [DetailView("Пол", Order = 10), ListView(Visible = false)]
        public Gender Gender { get; set; }

        [DetailView("Дата рождения", Order = 11), ListView]
        public DateTime? BirthDate { get; set; }


    }
}