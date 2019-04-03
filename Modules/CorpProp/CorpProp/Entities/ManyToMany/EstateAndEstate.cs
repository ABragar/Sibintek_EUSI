using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities;

namespace CorpProp.Entities.ManyToMany
{
    /// <summary>
    /// Представляет связь М:М между объектами имущества.
    /// </summary>
    /// <remarks>
    /// Объект слева - вышестоящий.
    /// Объект справа - нижестоящий.
    /// </remarks>
    public class EstateAndEstate : ManyToManyAssociation<Estate.Estate, Estate.Estate>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса EstateAndEstate.
        /// </summary>
        public EstateAndEstate():base()
        {

        }
    }
}
