using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Attributes;
using Base.UI.Filter;
using Newtonsoft.Json;

namespace Base.UI.Presets
{
    public class GridExtendedFilterPreset: BaseObject
    {
        [SystemProperty]
        [DetailView("Кол-во записей", Order = 1)]
        [PropertyDataType("ValidationPageSize")]
        public int PageSize { get; set; }

        [SystemProperty]
        [DetailView("Возможность группировки", Order = 10)]
        public bool Groupable { get; set; }

        [ListView]
        [DetailView("Список столбцов", Order = 30)]
        //[PropertyDataType("GridPreset_Columns")]
        public virtual ICollection<ColumnExtendedFilterPreset> Columns { get; set; }

        [SystemProperty]
        [ListView]
        [DetailView("Расширенные настройки фильтрации", Order = 20)]
        public bool ExtendedFilterSettings { get; set; }

        [SystemProperty]
        [ListView]
        [DetailView]
        public int? MnemonicFilterID { get; set; }

        [SystemProperty]
        [ListView]
        [DetailView]
        public string Filter { get; set; }

        [SystemProperty]
        [DetailView]
        public string Sorts { get; set; }
        [DetailView]

        [SystemProperty]
        public bool ShowFooter { get; set; }

        [SystemProperty]
        public string For { get; set; }
    }

    public class ColumnExtendedFilterPreset: BaseObject
    {
        [SystemProperty]
        public string Name { get; set; }

        [SystemProperty]
        [ListView(Sortable = false)]
        [DetailView("Наименование")]
        [MaxLength(255)]
        public string Title { get; set; }

        [SystemProperty]
        [ListView(Sortable = false, Width = 200)]
        [DetailView("Ширина")]
        public int? Width { get; set; }

        [SystemProperty]
        [ListView(Sortable = false)]
        [DetailView("Видимость")]
        public bool Visible { get; set; }

        [SystemProperty]
        [ListView(Sortable = false)]
        [DetailView("В одну строку")]
        public bool OneLine { get; set; }
    }
}