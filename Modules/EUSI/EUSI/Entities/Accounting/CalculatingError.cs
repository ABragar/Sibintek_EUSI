using Base;
using Base.Attributes;
using Base.Translations;
using CorpProp.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.Accounting
{
    public class CalculatingError : BaseObject
    {
        public CalculatingError() : base()
        {

        }

        //private static readonly CompiledExpression<CalculatingError, string> _accountingObjectName =
        //    DefaultTranslationOf<CalculatingError>.Property(x => x.AccountingObjectName).Is(x => (x.AccountingObject != null) ? x.AccountingObject.InventoryNumber + " " + x.AccountingObject.Name : "");

        [DetailView(Name = "ОС", ReadOnly = true)]
        [ListView(Name = "ОС")]
        public string AccountingObjectName {get;set; }

        [DetailView(Name = "Сообщение", ReadOnly = true)]
        [ListView(Name = "Сообщение")]
        public string Message {get; set;}
        //public virtual AccountingObject AccountingObject {get;set;}
        //public int? AccountingObjectID {get;set;}
        public int? CalculatingRecordID {get;set;}
    }
}
