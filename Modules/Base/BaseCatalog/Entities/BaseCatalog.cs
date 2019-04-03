using System.ComponentModel.DataAnnotations;
using Base;
using Base.Attributes;

namespace BaseCatalog.Entities
{
    public abstract class BaseCatalog: BaseObject
    {

        [DetailView("Наименование", Required = true)]
        [ListView]
        [MaxLength(255)]
        public string Title { get; set; }

        [DetailView("Описание")]
        [ListView]
        public string Description { get; set; }
    }
}