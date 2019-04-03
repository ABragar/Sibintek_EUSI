using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;

namespace Data.Entities
{
    public class ImplementationNNA : BaseObject
    {

      //  public ImplementationMethod ImplementationMethod { get; set; }
        public DateTime ImplementationDate { get; set; }

        public long ICostsWithoutVAT { get; set; }

        public long ICostsWithtVAT { get; set; }

        public string Description { get; set; }

    }
}
