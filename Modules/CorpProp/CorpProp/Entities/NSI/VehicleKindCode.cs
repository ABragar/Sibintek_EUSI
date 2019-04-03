﻿using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник коды вида транспортного средства.
    /// </summary>
    [EnableFullTextSearch]
    public class VehicleKindCode : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса VehicleKindCode.
        /// </summary>
        public VehicleKindCode()
        {

        }
    }
}