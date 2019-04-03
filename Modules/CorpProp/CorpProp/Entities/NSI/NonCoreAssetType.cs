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
    /// Представляет справочник типов ННА.
    /// </summary>
    [EnableFullTextSearch]
    public class NonCoreAssetType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса NonCoreAssetType.
        /// </summary>
        public NonCoreAssetType()
        {

        }
    }
}
