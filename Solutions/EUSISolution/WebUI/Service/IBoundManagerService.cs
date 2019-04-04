using System;
using System.Linq.Expressions;
using Base;
using Base.UI;
using Base.UI.Presets;
using Kendo.Mvc.UI.Fluent;
using WebUI.BoundsRegister;
using WebUI.Models;

namespace WebUI.Service
{
    public interface IBoundManagerService
    {
        void BoundColumn<T>(GridColumnFactory<T> factory, ColumnViewModel column, ColumnPreset preset, StandartGridView grid, Action<GridBoundColumnBuilder<dynamic>, ColumnPreset, ColumnViewModel, StandartGridView> columnBuilderDelegate = null)
            where T: class;
    }
}