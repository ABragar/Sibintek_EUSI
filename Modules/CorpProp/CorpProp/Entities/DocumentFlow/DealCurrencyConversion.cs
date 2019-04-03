using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Attributes;

namespace CorpProp.Entities.DocumentFlow
{
    [NotMapped]
    public class DealCurrencyConversion : SibDeal
    {
        public DealCurrencyConversion()
        { }

        public DealCurrencyConversion(SibDeal deal)
        {
            DealTypeID = deal.DealTypeID;
            NumberContragent = deal.NumberContragent;
            DateFrom = deal.DateFrom;
            DateTo = deal.DateTo;
            CurrencyID = deal.CurrencyID;
            SumDeal = deal.SumDeal;
            IsRequiresStateRegistration = deal.IsRequiresStateRegistration;
            DateStateRegistration = deal.DateStateRegistration;

            SourseInformationID = deal.SourseInformationID;
            ExternalSystemIdentifier = deal.ExternalSystemIdentifier;
            Name = deal.Name;
            Number = deal.Number;
            FullNumber = deal.FullNumber;
            DateDoc = deal.DateDoc;
            DateRegistration = deal.DateRegistration;
            Description = deal.Description;
            DocKindID = deal.DocKindID;
            DocTypeID = deal.DocTypeID;
            DocParentID = deal.DocParentID;

            ID = deal.ID;
            Hidden = deal.Hidden;
            SortOrder = deal.SortOrder;
            RowVersion = deal.RowVersion;
           
        }
        
        [ListView(Name = "Сумма в валюте", Order = 42)]
        public decimal SumDealInTargetedCurrency { get; set; }
    }
}
