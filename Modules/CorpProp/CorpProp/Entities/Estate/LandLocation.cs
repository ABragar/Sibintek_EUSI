using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.Estate
{
    /// <summary>
    /// Описание местоположений границ ЗУ.
    /// </summary>
    public class LandLocation : TypeObject
    {
        public LandLocation() : base() { }

        /// <summary>
        /// Получает или задает кадастровый номер объекта.
        /// </summary>
        public string CadastralNumber { get; set; }

    }
}
