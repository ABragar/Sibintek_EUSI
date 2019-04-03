using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет вид транспортного средства.
    /// </summary>
    [EnableFullTextSearch]
    public class VehicleType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса VehicleType.
        /// </summary>
        public VehicleType()
        {

        }
    }
}
