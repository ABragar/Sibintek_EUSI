using Base.DAL;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Accounting;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Представляет движимое имущество.
    /// </summary>
    [EnableFullTextSearch]
    public class MovableEstate : InventoryObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса MovableEstate.
        /// </summary>
        public MovableEstate() : base() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса MovableEstate из объекта БУ.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="obj">Объект БУ.</param>
        public MovableEstate(IUnitOfWork uofw, AccountingObject obj) : base (uofw, obj)
        {

        }

    }
}
