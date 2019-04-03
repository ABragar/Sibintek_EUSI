using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Base;
using Base;
using Base.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Utils.Common.Attributes;
using Newtonsoft.Json;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник ОКВЭД.
    /// </summary>
    [Table("SibOkveds")]
    [EnableFullTextSearch]
    public class SibOKVED : HDictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SibOKVED.
        /// </summary>
        public SibOKVED()
        {

        }

       

        /// <summary>
        /// Получает или задает родительский элемент.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual SibOKVED Parent_ { get; set; }

        [JsonIgnore]
        public virtual ICollection<SibOKVED> Children_ { get; set; }

        [NotMapped]
        public override HCategory Parent => this.Parent_;

        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<SibOKVED>();
    }
}
