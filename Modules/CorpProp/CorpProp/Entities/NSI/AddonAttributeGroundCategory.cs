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
    /// Представляет справочник дополнительных признаков категорий земель.
    /// </summary>
    [EnableFullTextSearch]
    public class AddonAttributeGroundCategory : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса AddonAttributeGroundCategory.
        /// </summary>
        public AddonAttributeGroundCategory()
        {

        }
    }
}
