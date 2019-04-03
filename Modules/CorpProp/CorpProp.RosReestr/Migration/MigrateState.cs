using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.RosReestr.Migration
{
    /// <summary>
    /// Статус миграции.
    /// </summary>
    [EnableFullTextSearch]
    public class MigrateState : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса MigrateState.
        /// </summary>
        public MigrateState()
        {

        }

        /// <summary>
        /// Получает или задает наименование.
        /// </summary>
        [ListView(Order = 2, Width = 100)]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Order = 1, Required = true)]
        [PropertyDataType(PropertyDataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Получает или задает системный код.
        /// </summary>
        [ListView(Order = 1, Width = 100)]
        [DetailView(Name = "Код", Order = 2)]
        [FullTextSearchProperty]
        [PropertyDataType(PropertyDataType.Text)]
        [SystemProperty]
        public string Code { get; set; }
    }
}
