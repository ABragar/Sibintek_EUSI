using Base;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    /// <summary>
    /// Представляет территориальный признак пользователя к ОГ.
    /// </summary>
    /// <remarks>
    /// Cвязь между профилем пользователя и ОГ.
    /// </remarks>
    public class SibUserTerritory : ManyToManyAssociation<SibUser, Society>
    {
        
        public SibUserTerritory(): base()
        {

        }
    }
}
