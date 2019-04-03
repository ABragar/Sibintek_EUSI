using Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Base.Utils.Common.Attributes;

namespace Base
{
    [Serializable]
    [EnableFullTextSearch]
    public abstract class HCategory : BaseObject, ITreeNode
    {
        public const char Seperator = ';';
        public const char WrapID = '@';

        public string sys_all_parents { get; protected set; }

        [ListView]
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Order = 1, Required = true)]
        [MaxLength(500)]
        public string Name { get; set; }

        public int? ParentID { get; set; }

        public abstract HCategory Parent { get; }
        public abstract IEnumerable<HCategory> Children { get; }
        public int Level => this.sys_all_parents?.Count(x => x == Seperator) + 1 ?? 0;
        public bool IsRoot => sys_all_parents == null;
        public int GetParentID(int level)
        {
            string parent = this.sys_all_parents?.Split(HCategory.Seperator).Skip(level).FirstOrDefault();

            if (parent == null)
                return -1;
            ;

            return HCategory.IdToInt(parent);
        }

        #region Helpers
        public static string IdToString(int id)
        {
            return $"{WrapID}{id}{WrapID}";
        }

        public static int IdToInt(string id)
        {
            return int.Parse(id.Trim(WrapID));
        }

        public void SetParent(HCategory parent)
        {
            if (parent == null)
            {
                this.ParentID = null;
                this.sys_all_parents = null;
            }
            else
            {
                if (this.ID == parent.ID)
                {
                    throw new Exception("Циклическая зависимость");
                }

                if (parent.sys_all_parents != null && parent.sys_all_parents.Contains(HCategory.IdToString(this.ID)))
                {
                    throw new Exception("Циклическая зависимость");
                }

                this.ParentID = parent.ID;
                this.sys_all_parents = (parent.sys_all_parents != null ? parent.sys_all_parents + Seperator : "") + IdToString(parent.ID);
            }
        }
        #endregion

        #region ITreeNode
        string ITreeNode.Name => this.Name;
        ITreeNode ITreeNode.Parent => this.Parent;
        IEnumerable<ITreeNode> ITreeNode.Children => this.Children;
        #endregion
    }
}
