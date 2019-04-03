using Base.Attributes;
using Base.ComplexKeyObjects.Superb;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Entities.NU
{
    /// <summary>
    /// Налоговая декларация по налогу на имущество организаций.
    /// </summary>
    [EnableFullTextSearch]
    public class DeclarationEstate : Declaration
    {

        /// <summary>
        /// Инициализирует новый экземпляр класса DeclarationEstate.
        /// </summary>
        public DeclarationEstate(): base()
        {

        }


        
    }
}
