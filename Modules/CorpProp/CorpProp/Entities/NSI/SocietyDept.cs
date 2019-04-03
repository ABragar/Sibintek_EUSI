using Base;
using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.NSI
{
    /// <summary>
    /// Представляет структурное подразделение.
    /// </summary>
    [Table("SocietyDepts")]
    [EnableFullTextSearch]
    public class SocietyDept : HDictObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса SocietyDept.
        /// </summary>
        public SocietyDept()
        {

        }

        /// <summary>
        /// Получает или задает родительский элемент.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual SocietyDept Parent_ { get; set; }

        [JsonIgnore]
        public virtual ICollection<SocietyDept> Children_ { get; set; }

        [NotMapped]
        public override HCategory Parent => this.Parent_;

        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? Enumerable.Empty<SocietyDept>();

        /// <summary>
        /// Получает или задает ИД ОГ
        /// </summary>
        
        public int? SocietyID { get; set; }

        [DetailView(Name = "ОГ", Order = -1)]
        public Society Society { get; set; }
    }
}
