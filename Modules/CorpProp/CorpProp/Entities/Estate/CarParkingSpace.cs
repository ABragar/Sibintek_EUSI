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
    /// Представляет Машино-место
    /// </summary>
    [EnableFullTextSearch]
    public class CarParkingSpace : BuildingStructure
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса CarParkingSpace.
        /// </summary>
        public CarParkingSpace() : base()
        {

        }

        /// <summary>
        /// Инициализирует новый экземпляр класса CarParkingSpace из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public CarParkingSpace(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {

        }
    }
}
