using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник структурных подразделений компании.
    /// </summary>
    [EnableFullTextSearch]
    public class UnitOfCompany : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса UnitOfCompany.
        /// </summary>
        public UnitOfCompany()
        {

        }
    }
}
