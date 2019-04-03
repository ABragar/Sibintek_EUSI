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
    /// Справочник типов объектов оценки.
    /// </summary>
    [EnableFullTextSearch]
    public class EstateAppraisalType : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса EstateAppraisalType.
        /// </summary>
        public EstateAppraisalType()
        {

        }
    }
}
