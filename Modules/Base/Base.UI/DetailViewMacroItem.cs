using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Attributes;
using Base.Entities;
using Base.Macros;
using Base.Macros.Entities;

namespace Base.UI
{

    [Serializable]
    public class DetailViewMacroItem : BaseObject
    {
        private static readonly Random Rnd = new Random();

        public DetailViewMacroItem()
        {
            ID = Rnd.Next(0, 99999);
        }

        [ListView(Order = 0)]
        [DetailView("Название", Order = 0)]
        public string Title { get; set; }

        [ListView("Описание", Order = 1)]
        [DetailView("Описание", Order = 1)]
        public string Description { get; set; }

        [ListView(Order = 2)]
        [DetailView("Действие", Order = 2)]
        public MacrosDirection MacrosDirection { get; set; }

        [PropertyDataType(PropertyDataType.BPObjectEditButton)]
        [DetailView("Макросы", Order = 3)]
        public List<InitItem> Rules { get; set; } = new List<InitItem>();           

        [DetailView("Свойства объекта", Order = 4)]
        public virtual ICollection<MacrosField> MacrosFields { get; set; } = new List<MacrosField>();
    }


    [Serializable]
    public class MacrosField : BaseObject
    {
        [MaxLength(255)]
        [DetailView(Name = "Поле", Required = true)]
        [PropertyDataType("FieldSetting")]
        public string FieldName { get; set; }
    }


    [UiEnum]
    public enum MacrosDirection
    {
        [UiEnumValue("Доступно")]
        Enable = 0,
        [UiEnumValue("Только для чтения")]
        Disable = 1,
        [UiEnumValue("Скрыто")]
        Hide = 2,
    }
}