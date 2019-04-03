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
    /// Помещение.
    /// </summary>
    [EnableFullTextSearch]
    public class Room : BuildingStructure
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Room.
        /// </summary>
        public Room():base()
        {

        }

        /// <summary>
        /// Инициализирует новый экземпляр класса Room из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public Room(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {

        }
    }
}
