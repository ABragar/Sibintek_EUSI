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
    /// Классификатор ОС, по которому установлены годовые нормы амортизационных отчислений
    /// </summary>
    [EnableFullTextSearch]
    public class ENAOF : DictObject
    {
        public ENAOF() : base()
        {

        }

    }
}
