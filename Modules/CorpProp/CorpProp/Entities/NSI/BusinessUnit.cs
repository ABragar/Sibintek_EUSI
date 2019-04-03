using Base.Attributes;
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
    /// Представляет справочник Бизнес-единицы (Аналитика 97 ИКСО).
    /// </summary>
    [EnableFullTextSearch]
    [ViewModelConfig(Title = "Бизнес-единица (Аналитика 97 ИКСО)")]
    public class BusinessUnit : DictObject
    {
    }
}
