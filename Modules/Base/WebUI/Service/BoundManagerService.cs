using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Base;
using Base.UI;
using Base.UI.Presets;
using Kendo.Mvc.UI.Fluent;
using WebUI.BoundsRegister;
using WebUI.Models;

namespace WebUI.Service
{
    public class BoundManagerService : IBoundManagerService
    {
        private readonly IColumnBoundRegisterService _columnBoundRegisterService;

        public BoundManagerService(IColumnBoundRegisterService columnBoundRegisterService)
        {
            _columnBoundRegisterService = columnBoundRegisterService;
        }

        public void BoundColumn<T>(GridColumnFactory<T> factory, ColumnViewModel column, ColumnPreset preset,
            StandartGridView grid, Action<GridBoundColumnBuilder<dynamic>, ColumnPreset, ColumnViewModel, StandartGridView> columnBuilderDelegate = null)
            where T : class
        {
            ColumnBoundConfig config;
            GridBoundColumnBuilder<T> builder = null;
            Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel, StandartGridView> action;

            if (!_columnBoundRegisterService.ExistsConfig(column, column.ParentViewModelConfig))
            {
                if (column.PropertyType.IsBaseObject())
                {
                    config = _columnBoundRegisterService.GetBoundByType(typeof(BaseObject));

                    builder = factory.Bound(column.PropertyType, column.PropertyName);

                    builder = InitColumn(builder, preset, column);

                    action =
                        config.Delegate as
                            Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel, StandartGridView>;

                    action?.Invoke(builder, preset, column, grid);

                    return;
                }
                else
                {
                    if (column.PropertyType.IsBaseCollection())
                    {
                        config = _columnBoundRegisterService.GetBoundByType(typeof(ICollection<BaseObject>));

                        builder = factory.Bound(column.PropertyType, column.PropertyName);

                        builder = InitColumn(builder, preset, column);

                        action =
                            config.Delegate as
                                Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel, StandartGridView>;

                        action?.Invoke(builder, preset, column, grid);

                        return;
                    }
                    else
                    {
                        if (column.ViewModelConfig != null)
                        {
                            string lookupPropertyForUi = column.ViewModelConfig.LookupPropertyForUI;

                            factory.Bound(typeof(string), preset.Name + "." + lookupPropertyForUi)
                            
                                .Title(preset.Title)
                                .Width(preset.Width ?? 200)
                                .Sortable(column.Sortable)
                                .Filterable(column.Filterable);    
                                
                                return;
                        }
                        else
                        {
                            factory.Bound(column.PropertyType, preset.Name)
                            .Hidden()
                                .Title(preset.Title)
                                .Width(preset.Width ?? 200)
                                .Sortable(column.Sortable)
                                .Filterable(column.Filterable);

                            return;
                        }
                    }
                }
            }
            config = _columnBoundRegisterService.GetBoundByType(column.PropertyType);
            if (columnBuilderDelegate != null)
                config.Delegate += columnBuilderDelegate;

            if (config != null)
            {
                if (config.CustomBoundType != null)
                {
                    builder = factory.Bound(config.CustomBoundType, column.PropertyName);
                }
                else
                {
                    builder = factory.Bound(column.PropertyType, column.PropertyName);
                }

                if (config.IsInitColumn)
                {
                    builder = InitColumn(builder, preset, column);
                }

                action =
                    config.Delegate as Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel, StandartGridView>;

                action?.Invoke(builder, preset, column, grid);
            }

            if (column.PropertyDataType.HasValue &&
                _columnBoundRegisterService.GetBoundByPropertyDataTypeEnum(column.PropertyDataType.Value) != null)
            {
                var enumConfig =
                    _columnBoundRegisterService.GetBoundByPropertyDataTypeEnum(column.PropertyDataType.Value);

                if (builder == null)
                {
                    if (enumConfig.CustomBoundType != null)
                    {
                        builder = factory.Bound(enumConfig.CustomBoundType, column.PropertyName);
                    }
                    else
                    {
                        builder = factory.Bound(column.PropertyType, column.PropertyName);
                    }
                }

                var enumAction =
                    _columnBoundRegisterService.GetBoundByPropertyDataTypeEnum(column.PropertyDataType.Value).Delegate
                        as Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel, StandartGridView>;

                enumAction?.Invoke(builder, preset, column, grid);
            }

            if (!string.IsNullOrEmpty(column.PropertyDataTypeName) &&
                _columnBoundRegisterService.GetBoundByPropertyDataTypeString(column.PropertyDataTypeName) != null)
            {
                var stringConfig =
                    _columnBoundRegisterService.GetBoundByPropertyDataTypeString(column.PropertyDataTypeName);

                var stringAction =
                    _columnBoundRegisterService.GetBoundByPropertyDataTypeString(column.PropertyDataTypeName).Delegate
                        as Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel, StandartGridView>;

                if (builder == null)
                {
                    if (stringConfig.CustomBoundType != null)
                    {
                        builder = factory.Bound(stringConfig.CustomBoundType, column.PropertyName);
                    }
                    else
                    {
                        builder = factory.Bound(column.PropertyType, column.PropertyName);
                    }
                }

                stringAction?.Invoke(builder, preset, column, grid);
            }

            if (column.ParentViewModelConfig != null)
            {
                if (_columnBoundRegisterService.GetBoundByGridTypeAndProperty(column.ParentViewModelConfig.TypeEntity,
                        column.PropertyName) != null)
                {
                    var columnGridConfig =
                        _columnBoundRegisterService.GetBoundByGridTypeAndProperty(
                            column.ParentViewModelConfig.TypeEntity, column.PropertyName);

                    BountProperty(builder, columnGridConfig, factory, column, preset, grid, action = null);
                }

                if (_columnBoundRegisterService.GetBoundByGridTypeMnemonicAndProperty(
                        column.ParentViewModelConfig.TypeEntity, column.ParentViewModelConfig.Mnemonic,
                        column.PropertyName) != null)
                {
                    var columnGridConfig = _columnBoundRegisterService.GetBoundByGridTypeMnemonicAndProperty(
                        column.ParentViewModelConfig.TypeEntity, column.ParentViewModelConfig.Mnemonic,
                        column.PropertyName);

                    BountProperty(builder, columnGridConfig, factory, column, preset, grid, action = null);
                }
            }

            if (builder == null)
                throw new InvalidOperationException(
                    $"Не найдена ни одна конфигурация баунда для колонки {column.PropertyName}");
        }


        private GridBoundColumnBuilder<T> InitColumn<T>(GridBoundColumnBuilder<T> columnBuilder,
            ColumnPreset columnPreset, ColumnViewModel column)
            where T : class
        {
            if (String.IsNullOrEmpty(column.Format) 
                && (DateTime.Equals(column.PropertyType, typeof(DateTime))
                || DateTime.Equals(column.PropertyType, typeof(DateTime?))
                )
                && column.PropertyDataType != Base.Attributes.PropertyDataType.DateTime)
                column.Format = "{0:dd.MM.yyyy}";

            return columnBuilder
                .Title(columnPreset.Title)
                .Sortable(column.Sortable)
                .Filterable(column.Filterable)
                .Locked(column.Locked)
                .Lockable(column.Lockable)
                .Groupable(column.Groupable)
                .Width(columnPreset.Width ?? 200)
                .Hidden(!columnPreset.Visible)
                .ClientGroupHeaderTemplate(column.ClientGroupHeaderTemplate)
                .ClientFooterTemplate(column.ClientFooterTemplate)
                .Format(column.Format);
        }

        private void BountProperty<T>(GridBoundColumnBuilder<T> builder,
            ColumnBoundConfig columnGridConfig,
            GridColumnFactory<T> factory,
            ColumnViewModel column,
            ColumnPreset preset,
            StandartGridView grid,
            Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel, StandartGridView> action)
            where T : class
        {
            if (builder == null)
            {
                if (columnGridConfig.CustomBoundType != null)
                {
                    builder = factory.Bound(columnGridConfig.CustomBoundType, column.PropertyName);
                }
                else
                {
                    builder = factory.Bound(column.PropertyType, column.PropertyName);
                }
            }

            if (columnGridConfig.IsInitColumn)
            {
                builder = InitColumn(builder, preset, column);
            }

            if (columnGridConfig.IsInitBaseObjectBound)
            {
                var baseObjconfig = _columnBoundRegisterService.GetBoundByType(typeof(BaseObject));
                var baseObjAction =
                    baseObjconfig.Delegate as Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel,
                        StandartGridView>;
                baseObjAction?.Invoke(builder, preset, column, grid);
            }

            if (columnGridConfig.IsInitBaseObjectCollectionBound)
            {
                var baseObjCollectionconfig =
                    _columnBoundRegisterService.GetBoundByType(typeof(ICollection<BaseObject>));
                var baseObjcollectionAction =
                    baseObjCollectionconfig.Delegate as Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel,
                        StandartGridView>;
                baseObjcollectionAction?.Invoke(builder, preset, column, grid);
            }
            action =
                columnGridConfig.Delegate as Action<GridBoundColumnBuilder<T>, ColumnPreset, ColumnViewModel,
                    StandartGridView>;
            action?.Invoke(builder, preset, column, grid);
        }
    }
}