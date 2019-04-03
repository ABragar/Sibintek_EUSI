using Base.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.ComplexKeyObjects.Superb;
using Base.Entities.Complex;

namespace Base.Task.Entities
{
    public class BaseTaskCategory : BaseObject, ISuperObject<BaseTaskCategory>
    {
        [MaxLength(255)]
        [DetailView("Наименование", Order = 0), ListView]
        public string Title { get; set; }

        [ListView("Тип", Order = -100)]
        public string ExtraID { get; }
    }
}
