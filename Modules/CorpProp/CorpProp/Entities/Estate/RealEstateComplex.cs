using Base.DAL;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Единый недвижимый комплекс.
    /// </summary>
    [EnableFullTextSearch]
    public class RealEstateComplex : Cadastral
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса RealEstateComplex.
        /// </summary>
        public RealEstateComplex() : base()
        {

        }

        /// <summary>
        /// Инициализирует новый экземпляр класса RealEstateComplex из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public RealEstateComplex(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {
            


        }

    }
}
