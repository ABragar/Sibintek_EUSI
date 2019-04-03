using Base.Attributes;
using Base.Entities.Complex;
using Base.UI;
using Base.UI.ViewModal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Security
{
    public class UserCategory : HCategory, IUserCategory, ITreeNodeIcon, IAccessibleObject
    {
        public UserCategory()
        {
            Icon = new Icon();
        }

        public string SysName { get; set; }

        [ForeignKey("ParentID")]
        public virtual UserCategory Parent_ { get; set; }
        public virtual ICollection<UserCategory> Children_ { get; set; }

        #region HCategory
        public override HCategory Parent => this.Parent_;
        public override IEnumerable<HCategory> Children => Children_ ?? new List<UserCategory>();
        #endregion

        [DetailView("Иконка", Order = -10), ListView]
        public Icon Icon { get; set; }
        [DetailView("Пресеты", Order = 2), ListView]
        public virtual ICollection<UserCategoryPreset> Presets { get; set; } = new List<UserCategoryPreset>();

        [DetailView("Роли", Order = 3), ListView]
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

        [DetailView("Профиль", Order = 4, Required = true)]
        [PropertyDataType("AccessibleProfiles")]
        public string ProfileMnemonic { get; set; }

        [DetailView("Доступен после регистрации", Order = 5), ListView]
        public bool IsAccessible { get; set; }
    }

    public class UserCategoryPreset : EasyCollectionEntry<PresetRegistor>
    {
    }

}



