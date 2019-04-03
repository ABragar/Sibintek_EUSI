﻿using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Справочник типов аренды МСФО.
    /// </summary>
    [EnableFullTextSearch]
    public class RentTypeMSFO : DictObject
    {
        public RentTypeMSFO() : base()
        {

        }

    }
}
