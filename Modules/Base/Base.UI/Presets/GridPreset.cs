using Base.Attributes;
using Base.UI.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Base.UI.ViewModal;
using Base.Utils.Common.Maybe;


namespace Base.UI.Presets
{
    [Serializable]
    [JsonObject]
    public class GridPreset : Preset
    {
        [DetailView("Кол-во записей", Order = 1)]
        [PropertyDataType("ValidationPageSize")]
        public int PageSize { get; set; }

        [DetailView("Возможность группировки столбцов", Order = 10)]
        public bool Groupable { get; set; }

        [DetailView("Список столбцов", Order = 30)]
        [PropertyDataType("GridPreset_Columns")]
        public List<ColumnPreset> Columns { get; set; } = new List<ColumnPreset>();

        [DetailView("Расширенные настройки фильтрации", Order = 20)]
        public bool ExtendedFilterSettings { get; set; }

        [SystemProperty]
        public int? MnemonicFilterID { get; set; }

        public string Filter { get; set; }
        public string Sorts { get; set; }

        public bool ShowFooter { get; set; }

        //sib

        public string Aggregates { get; set; }

        /// <summary>
        /// Множественный выбор.
        /// </summary>
        [DetailView("Множественный выбор", Order = 21)]
        public bool IsMultiselect { get; set; }

        /// <summary>
        /// Множественный выбор.
        /// </summary>
        [DetailView("Показать все столбцы", Order = 22)]
        public bool ShowAllColumns { get; set; }

        //end sib
    }

    [Serializable]
    [JsonObject]
    public class ColumnPreset : BaseObject
    {
        public string Name { get; set; }

        [ListView(Sortable = false)]
        [DetailView("Наименование")]
        [MaxLength(255)]
        public string Title { get; set; }

        [ListView(Sortable = false, Width = 200)]
        [DetailView("Ширина")]
        public int? Width { get; set; }

        [ListView(Sortable = false)]
        [DetailView("Видимость")]
        public bool Visible { get; set; }

        [ListView(Sortable = false)]
        [DetailView("В одну строку")]
        public bool OneLine { get; set; }
    }
}
