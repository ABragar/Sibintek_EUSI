using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Группа учета (вид) ОС/группа консолидации по РСБУ.
    /// </summary>
    /// <remarks>
    /// Указывается группа продуктов для последующей консолидации данных в ФСД (группа основных средств в формуляре).
    /// </remarks>
    [EnableFullTextSearch]
    public class GroupConsolidationRSBU : DictObject
    {
        public GroupConsolidationRSBU() : base()
        {

        }

    }
}
