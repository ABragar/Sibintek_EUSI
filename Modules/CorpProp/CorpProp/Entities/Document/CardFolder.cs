using Base.Attributes;
using Base.Utils.Common.Attributes;
using CorpProp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Base.Security.ObjectAccess;
using Base.Security.ObjectAccess.Policies;
using CorpProp.Entities.Base;
using Newtonsoft.Json;
using Base.DAL;
using System.ComponentModel;

namespace CorpProp.Entities.Document
{
    /// <summary>
    /// Представляет папку.
    /// </summary>
    [AccessPolicy(typeof(CreatorOnlyPolicy))]
    [EnableFullTextSearch]
    public class CardFolder : HCategory, ITypeObject, IAccessibleObject
    {
       

        #region Constructor
        /// <summary>
        /// Инициализирует новый экземпляр класса FileCard.
        /// </summary>
        public CardFolder():base()
        {
            Oid = System.Guid.NewGuid();
            CreateDate = DateTime.Now;
            ActualDate = DateTime.Now.Date;
            IsHistory = false;
        }
        #endregion

        #region History       

        public virtual void InitHistory(IUnitOfWork uow) { }

        [SystemProperty]
        [DefaultValue(false)]
        public bool IsHistory { get; set; }

        ///// <summary>
        ///// Получает или задает дату начала действия историчности записи.
        ///// </summary>
        [SystemProperty]
        public DateTime? ActualDate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания действия историчности записи.
        /// </summary>
        [SystemProperty]
        public DateTime? NonActualDate { get; set; }
                

        #endregion

        /// <summary>
        /// Получает или задает уникальный ИД объекта.
        /// </summary>
        [SystemProperty]
        public System.Guid Oid { get; set; }

        ///// <summary>
        ///// Получает или задает дату создания объекта.
        ///// </summary>
        [SystemProperty]
        public DateTime? CreateDate { get; set; }
        

        [JsonIgnore]
        [ForeignKey("ParentID")]
        public virtual CardFolder Parent_ { get; set; }

        [JsonIgnore]
        public virtual ICollection<CardFolder> Children_ { get; set; }

        #region HCategory
        [NotMapped]
        public override HCategory Parent
        {
            get { return this.Parent_; }
        }
        [NotMapped]
        public override IEnumerable<HCategory> Children => Children_ ?? new List<CardFolder>();

        #endregion

        public new string sys_all_parents
        {
            get { return base.sys_all_parents; }
            set { base.sys_all_parents = value; }
        }

        /// <summary>
        /// Получает или задает наименование.
        /// </summary>       
        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Order = 0, TabName = CaptionHelper.DefaultTabName, Required = true)]
        public new string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

       
    }
}
