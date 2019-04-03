using Base.Attributes;
using Base.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.Security
{
    /// <summary>
    /// Представляет роль.
    /// </summary>
    public class SibRole : Role
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibRole.
        /// </summary>
        public SibRole()
        {

        }

        /// <summary>
        /// Получает или задает код.
        /// </summary>
        [FullTextSearchProperty]
        [ListView]
        [DetailView(Name = "Код", Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// Получает или задает признак административная роль.
        /// </summary>
        [DetailView(Name = "Административная", Order = 2)]
        [DefaultValue(false)]
        public bool IsAdministrative { get; set; }

        /// <summary>
        /// Получает или задает примечание.
        /// </summary>
        [DetailView(Name = "Примечание", Order = 3)]
        public string Description { get; set; }
    }
}
