using Base.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using Base.ComplexKeyObjects.Superb;
using Base.Utils.Common.Attributes;

namespace Base.Settings
{
    [EnableFullTextSearch]
    [Serializable]
    public class SettingItem : BaseObject, ISuperObject<SettingItem>
    {
        [FullTextSearchProperty]
        [DetailView(Name = "Наименование", Required = true, Order = -1), ListView]
        [MaxLength(255)]
        public string Title { get; set; }

        public string ExtraID { get; }
    }
}
