using System;
using System.ComponentModel.DataAnnotations.Schema;
using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
namespace CorpProp.Entities.NSI
{
    [EnableFullTextSearch]
    public class ExchangeRate : TypeObject, IAccessibleObject
    {
        [ForeignKey("CurrencyID")]
        [ListView(Order = 1)]
        [DetailView(Name = "Валюта", Order = 1, Required = true)]
        public virtual Currency Currency { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.Index("IX_UniqueRateAtDateForCurrency", 2, IsUnique = true)]
        public int CurrencyID { get; set; }

        [ListView(Order = 2)]
        [DetailView(Name = "Единиц", Order = 2, Required = true)]
        public int Amount { get; set; }

        [ListView(Order = 3)]
        [DetailView(Name = "Курс", Order = 3, Required = true)]

        public decimal Rate { get; set; }

        [ListView(Order = 4)]
        [DetailView(Name = "Дата", Order = 4, Required = true)]
        [System.ComponentModel.DataAnnotations.Schema.Index("IX_UniqueRateAtDateForCurrency", 1, IsUnique = true)]
        public DateTime Date { get; set; }
    }

    public class ExchangeRateNotFoundException : Exception
    {
        public ExchangeRateNotFoundException(DateTime exchangeDate, int currencyID) : base()
        {
            ExchangeDate = exchangeDate;
            CurrencyID = currencyID;
        }

        public ExchangeRateNotFoundException(string message, DateTime exchangeDate, int currencyID) : base(message)
        {
            ExchangeDate = exchangeDate;
            CurrencyID = currencyID;
        }

        public DateTime ExchangeDate { get; set; }

        public int CurrencyID { get; set; }

    }
}
