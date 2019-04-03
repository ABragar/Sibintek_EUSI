using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    /// <summary>
    /// Представляет связь между ИК и земельными участками.
    /// </summary>   
    public class IKAndLand : ManyToManyAssociation<Estate.PropertyComplexIO, Estate.Land>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса IKAndLand.
        /// </summary>
        public IKAndLand() : base()
        {

        }
    }
}
