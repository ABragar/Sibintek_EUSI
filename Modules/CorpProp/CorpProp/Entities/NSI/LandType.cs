using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Тип ЗУ
    /// </summary>
    [EnableFullTextSearch]
    public class LandType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр LandType.
        /// </summary>
        public LandType(): base()
        {

        }

        public LandType(string name) : base(name)
        {

        }
    }
}
