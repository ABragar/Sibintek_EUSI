using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.Contact.Entities
{
    public class EmployeePost : BaseObject
    {
        [MaxLength(255)]
        [DetailView("Наименование"), ListView]
        public string Title { get; set; }

        [DetailView("Ранг")]
        public EmployeePostRank Rank { get; set; } = EmployeePostRank.Simple;
    }

    [UiEnum]
    public enum EmployeePostRank
    {
        [UiEnumValue("Начальник")]
        Chief = 0,

        [UiEnumValue("Заместитель начальника")]
        Deputy = 1,

        [UiEnumValue("Сотрудник")]
        Simple = 2,
    }
}
