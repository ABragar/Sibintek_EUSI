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
    /// Группа учета (вид) ОС/группа консолидации.
    /// </summary>
    /// <remarks>
    /// Указывается группа продуктов для последующей консолидации данных в ФСД (группа основных средств в формуляре).
    /// </remarks>
    [EnableFullTextSearch]
    public class PositionConsolidation : DictObject
    {
        public PositionConsolidation() : base()
        {

        }
        
        /// <summary>
        /// Получает или задает код группы консолидации.
        /// </summary>
        [ListView(Order = 1, Width = 100, Visible = false)]
        [DetailView(Name = "Код Группы", Visible = false)]
        [PropertyDataType(PropertyDataType.Text)]
        [FullTextSearchProperty]
        [SystemProperty]
        public string GroupCode { get; set; }

    }
}
