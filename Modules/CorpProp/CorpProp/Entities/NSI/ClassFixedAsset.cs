using Base;
using CorpProp.Entities.Base;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Base.Utils.Common.Attributes;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет справочник классов основных средств.
    /// </summary>
    /// <remarks>
    /// Внешний справочник (КИС САП РН).
    /// </remarks>
    [EnableFullTextSearch]
    public class ClassFixedAsset : DictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ClassFixedAsset.
        /// </summary>
        public ClassFixedAsset()
        {

        }

        ///// <summary>
        ///// Получает или задает родительский элемент.
        ///// </summary>
        //[JsonIgnore]
        //[ForeignKey("ParentID")]
        //public virtual ClassFixedAsset Parent_ { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<ClassFixedAsset> Children_ { get; set; }

        //[NotMapped]
        //public override HCategory Parent => this.Parent_;

        //[NotMapped]
        //public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<ClassFixedAsset>();

    }
}
