using Base;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class CadastralValue : BaseObject
    {
        public long Cost { get; set; }

        public DateTime RelevanceDate { get; set; }

        public DateTime ActionStartDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        //public InformationSource InformationSource { get; set; }
    }
}
