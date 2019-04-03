using System;
using Base.UI;
using Base.UI.Presets;
using Kendo.Mvc.UI.Fluent;
using WebUI.Models;

namespace WebUI.BoundsRegister
{
    public class ColumnBoundConfig
    {
        public Action<GridBoundColumnBuilder<dynamic>, ColumnPreset, ColumnViewModel, StandartGridView> Delegate { get; set; }
        public bool IsInitColumn { get; set; }
        public bool IsInitBaseObjectBound { get; set; }
        public bool IsInitBaseObjectCollectionBound { get; set; }
        public Type CustomBoundType { get; set; }
    }
}
