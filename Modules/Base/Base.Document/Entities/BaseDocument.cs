using System;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.BusinessProcesses.Entities;
using Base.ComplexKeyObjects.Superb;
using Base.Contact.Entities;
using Base.Security;
using Base.Translations;
using Base.Utils.Common.Attributes;

namespace Base.Document.Entities
{
    [EnableFullTextSearch]
    public class BaseDocument : BaseObject, ISuperObject<BaseDocument>, IBPObject, ICreateObject
    {

        private static readonly CompiledExpression<BaseDocument, string> _title = 
            DefaultTranslationOf<BaseDocument>.Property(x => x.Title).Is(x => "# " + x.Serial + " от " + x.Date);

        [SystemProperty]
        public string Title => _title.Evaluate(this);

        [MaxLength(255)]
        [FullTextSearchProperty]
        [DetailView("Номер", Required = true, Order = 0), ListView(Width = 100)]
        public string Serial { get; set; }

        [DetailView("Дата", Required = true, Order = 1), ListView(Width = 100)]
        public DateTime Date { get; set; }

        public int? ContaractorID { get; set; }
        [DetailView("Покупатель", Order = 5), ListView]
        public virtual BaseContact Contaractor { get; set; }

        public int? CompanyID { get; set; }
        [DetailView("Продавец", Order = 6), ListView(Visible = false)]
        public virtual Company Company { get; set; }        

        public int CreatorID { get; set; }
        [DetailView("Автор", Required = true, Order = 7), ListView]
        public virtual User Creator { get; set; }

        [DetailView("Подписывающее лицо", Visible = false , Order = 8)]
        public virtual BaseEmployee SignPerson { get; set; }

        [DetailView("Количество копий", Order = 9)]
        public int NumberOfCopies { get; set; }

        [DetailView(TabName = "[9]Примечание")]
        [PropertyDataType(PropertyDataType.SimpleHtml)]
        public string Description { get; set; }

        public int? WorkflowContextID { get; set; }
        public virtual WorkflowContext WorkflowContext { get; set; }

        [DetailView(Name = "Дата последнего изменения", Visible = false)]
        public DateTime LastChangeDate { get; set; }

        [ListView(Visible = false, Order = -100)]
        [PropertyDataType(PropertyDataType.ExtraId)]
        public string ExtraID { get; }
    }
}
