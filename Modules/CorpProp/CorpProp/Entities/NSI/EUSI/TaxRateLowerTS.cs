﻿using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{

    [EnableFullTextSearch]
    public class TaxRateLowerTS : DictObject
    {
        public TaxRateLowerTS() : base()
        {

        }
    }
}