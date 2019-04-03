using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.NU
{
    /// <summary>
    /// Налоговый расчет по авансовому платежу по налогу на имущество организаций.
    /// </summary>
    [EnableFullTextSearch]
    public class DeclarationCalcEstate : Declaration
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса DeclarationCalcEstate.
        /// </summary>
        public DeclarationCalcEstate() : base()
        {

        }



    }
}
