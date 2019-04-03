using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace Data.Entities
{
    public class TransactionSubject : BaseObject
    {
        public int? InventoryObjectID { get; set; }

        public virtual InventoryObject InventoryObject { get; set; }

        public long TransactionAmountR { get; set; }
    }
}
