using System;

namespace DAL.Entities
{
    public class GraphBySociety
    {
        public Nullable<decimal> ShareMarket { get; set; }
        public Nullable<int> SocietyShareholderID { get; set; }
        public Nullable<int> SocietyRecipientID { get; set; }
        public string Recipient { get; set; }
        public string Shareholder { get; set; }
        public string R { get; set; }
    }
}