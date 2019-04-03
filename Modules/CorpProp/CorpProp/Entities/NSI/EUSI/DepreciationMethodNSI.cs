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
    /// Представляет справочник методов амортизации по РСБУ.
    /// </summary>
    [EnableFullTextSearch]
    public class DepreciationMethodRSBU : DictObject
    {

        public DepreciationMethodRSBU() : base()
        {

        }

    }


    /// <summary>
    /// Представляет справочник методов амортизации по МСФО.
    /// </summary>
    [EnableFullTextSearch]
    public class DepreciationMethodMSFO : DictObject
    {

        public DepreciationMethodMSFO() : base()
        {

        }

    }


    /// <summary>
    /// Представляет справочник методов амортизации по НУ.
    /// </summary>
    [EnableFullTextSearch]
    public class DepreciationMethodNU : DictObject
    {

        public DepreciationMethodNU() : base()
        {

        }

    }
}
