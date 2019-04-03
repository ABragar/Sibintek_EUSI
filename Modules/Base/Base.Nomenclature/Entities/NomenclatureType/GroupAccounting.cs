using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace Base.Nomenclature.Entities.NomenclatureType
{
    public class GroupAccounting : BaseObject
    {
        /// <summary>
        /// Номер
        /// </summary>
        [MaxLength(255)]
        [DetailView("Номер"), ListView]
        public string Number { get; set; }

        [MaxLength(255)]
        [DetailView("Наименование"), ListView]
        public string Title { get; set; }
    }
}
