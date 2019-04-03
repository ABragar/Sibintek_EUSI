using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;
using Base.ComplexKeyObjects.Superb;

namespace Base.UI.Filter
{
    public class MnemonicFilter : BaseObject, ISuperObject<MnemonicFilter>
    {
        [ListView("Тип", Order = -100)]
        public string ExtraID { get; }

        [SystemProperty]
        public System.Guid Oid { get; set; }

        [MaxLength(255)]
        [ListView]
        [DetailView("Наименование", Required = true)]
        public string Title { get; set; }

        [DetailView("Описание", ReadOnly = true)]
        public string Description { get; set; }

        [DetailView("Фильтр", Required = true), ListView]
        [PropertyDataType("QueryBuilderFilter")]
        public string Filter { get; set; }

        [MaxLength(255)]
        [DetailView("Мнемоника", Visible = false)]
        [ListView(Visible = false)]
        public string Mnemonic { get; set; }

        public int? UserID { get; set; }

        public MnemonicFilter()
        {
            Oid = GetGuid();
        }

        protected virtual Guid GetGuid()
        {
            return System.Guid.NewGuid();
        }
    }
}
