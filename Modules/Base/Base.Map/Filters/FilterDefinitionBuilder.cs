using Base.Map.Filters.Definitions;
using Base.Map.Helpers;
using Base.Map.Services;
using Base.Service.Crud;
using Base.UI;
using Base.UI.Extensions;
using Base.UI.ViewModal;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Base.Map.Filters
{
    internal class FilterDefinitionBuilder
    {
        private readonly IMapServiceFacade _serviceFacade;

        public FilterDefinitionBuilder(IMapServiceFacade serviceFacade)
        {
            if (serviceFacade == null)
            {
                throw new ArgumentNullException(nameof(serviceFacade));
            }

            _serviceFacade = serviceFacade;
        }

        public TextFilterDefinition CreateText(ColumnViewModel column)
        {
            return new TextFilterDefinition()
            {
                UIType = FilterUIType.Text,
                Title = column.Title,
                Field = column.PropertyName
            };
        }

        public BoolFilterDefinition CreateBoolean(ColumnViewModel column)
        {
            return new BoolFilterDefinition
            {
                UIType = FilterUIType.Checkbox,
                Title = column.Title,
                Field = column.PropertyName
            };
        }

        public FilterDefinition CreateNumeric(ColumnViewModel column, ViewModelConfig config, bool loadConstraints = true)
        {
            var service = config.GetService<IBaseObjectCrudService>();

            object minValue = null;
            object maxValue = null;

            if (loadConstraints)
            {
                using (var unitOfWork = _serviceFacade.UnitOfWorkFactory.Create())
                {
                    var minMax = service.GetAll(unitOfWork)
                        .GroupBy("1")
                        .Select($"new (Min({column.PropertyName}) as Min, Max({column.PropertyName}) as Max)")
                        .FirstOrDefault();

                    minValue = minMax?.Min;
                    maxValue = minMax?.Max;
                }
            }

            var result = FilterDefinitionFactory.createNumeric(column.ColumnType, minValue, maxValue);
            result.UIType = FilterUIType.Range;
            result.Title = column.Title;
            result.Field = column.PropertyName;

            return result;
        }

        public EnumFilterDefinition CreateRelationObjectEnum(ColumnViewModel column, ViewModelConfig config, bool loadConstraints = true)
        {
            var service = config.GetService<IBaseObjectCrudService>();
            var relatedObjectConfig = ViewModelConfigHelper.GetViewModelConfig(_serviceFacade.ViewModelConfigService, column.ColumnType);

            var options = Enumerable.Empty<OptionDefinition>();

            if (loadConstraints)
            {
                using (var unitOfWork = _serviceFacade.UnitOfWorkFactory.Create())
                {
                    options = service.GetAll(unitOfWork)
                        .Where($"{column.PropertyName} != null") //TODO and BoType.Mnemonic = \"{config.Mnemonic}\"
                        .Select(
                            $"new ({column.PropertyName}.ID as Value, {column.PropertyName}.{relatedObjectConfig.LookupProperty} as Text)")
                        .Distinct()
                        .OrderBy("Text")
                        .AsEnumerable()
                        .Select(x => new OptionDefinition
                        {
                            Value = x.Value.ToString(),
                            Text = x.Text.ToString()
                        }).ToList();
                }
            }

            var result = new IntegerEnumFilterDefinition
            {
                UIType = FilterUIType.MultiSelect,
                Title = column.Title,
                Field = $"{column.PropertyName}.ID",
                Options = options
            };

            return result;
        }

        public EnumFilterDefinition CreateEnum(ColumnViewModel column, ViewModelConfig config, bool loadConstraints = true)
        {
            var options = Enumerable.Empty<OptionDefinition>();

            if (loadConstraints)
            {
                var enumType = CheckType.IsNullableEnum(column.ColumnType) ? Nullable.GetUnderlyingType(column.ColumnType) : column.ColumnType;

                options = _serviceFacade.UiEnumService.GetEnum(enumType).Values.Select(x => new OptionDefinition
                {
                    Value = x.Value,
                    Text = x.Title
                });
            }

            var result = new EnumEnumFilterDefinition(column.ColumnType)
            {
                UIType = FilterUIType.MultiSelect,
                Title = column.Title,
                Field = column.PropertyName,
                Options = options
            };

            return result;
        }
    }
}