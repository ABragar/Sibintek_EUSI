using Base.Extensions;
using Base.Map.Helpers;
using Base.Map.Services;
using Base.UI;
using Base.UI.ViewModal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Map.Filters
{
    public class FilterManager : IFilterManager
    {
        private readonly IMapServiceFacade _serviceFacade;
        private readonly FilterDefinitionBuilder _definitionBuilder;

        public FilterManager(IMapServiceFacade serviceFacade)
        {
            if (serviceFacade == null)
            {
                throw new ArgumentNullException(nameof(serviceFacade));
            }

            _serviceFacade = serviceFacade;
            _definitionBuilder = new FilterDefinitionBuilder(_serviceFacade);
        }

        public IReadOnlyCollection<FilterDefinition> GetFilterDefinitions(string mnemonic)
        {
            if (string.IsNullOrEmpty(mnemonic))
            {
                throw new ArgumentNullException(nameof(mnemonic));
            }

            var config = GetViewModelConfig(mnemonic);

            if (!CheckType.IsGeoObject(config.TypeEntity))
            {
                return new FilterDefinition[0];
            }

            return GetFilterDefinitions(config);
        }

        private IReadOnlyCollection<FilterDefinition> GetFilterDefinitions(ViewModelConfig config, bool loadConstraints = true)
        {
            var query = from column in config.ListView.Columns
                where column.Filterable && column.Visible
                let filterDefinition = CreateFilterDefinition(column, config, loadConstraints)
                where filterDefinition != null
                select filterDefinition;

            var filterDifinitions = query.ToList();

            return filterDifinitions;
        }

        private FilterDefinition CreateFilterDefinition(ColumnViewModel column, ViewModelConfig config, bool loadConstraints)
        {
            if (column.ColumnType == typeof(string))
            {
                return _definitionBuilder.CreateText(column);
            }

            if (CheckType.IsValueType<bool>(column.ColumnType))
            {
                return _definitionBuilder.CreateBoolean(column);
            }

            if (CheckType.IsNumeric(column.ColumnType))
            {
                return _definitionBuilder.CreateNumeric(column, config, loadConstraints);
            }

            if (column.ColumnType.IsBaseObject() && !CheckType.IsCollection(column.PropertyType) && !CheckType.IsEnumerable(column.PropertyType))
            {
                return _definitionBuilder.CreateRelationObjectEnum(column, config, loadConstraints);
            }

            if (CheckType.IsEnum(column.ColumnType))
            {
                return _definitionBuilder.CreateEnum(column, config, loadConstraints);
            }

            return null;
        }

        public IQueryable BuildFilterWhereClause(IQueryable query, string mnemonic, FilterValues filters)
        {
            if (string.IsNullOrEmpty(mnemonic))
                throw new ArgumentNullException(nameof(mnemonic));

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            if (!filters.Any())
            {
                return query;
            }

            var config = GetViewModelConfig(mnemonic);

            if (!CheckType.IsGeoObject(config.TypeEntity))
            {
                return query;
            }

            return BuildFilterWhereClause(query, config, filters);
        }

        private IQueryable BuildFilterWhereClause(IQueryable query, ViewModelConfig config, FilterValues filters)
        {
            var filterDefinitions = from definition in GetFilterDefinitions(config, false).ToDictionary(k => k.Field)
                                    where filters.ContainsKey(definition.Key)
                                    let def = definition.Value.SetValue(filters[definition.Key])
                                    select def;

            filterDefinitions.ForEach(x => { query = x.BuildWhereClause(query); });
            return query;
        }

        private ViewModelConfig GetViewModelConfig(string mnemonic)
        {
            return ViewModelConfigHelper.GetViewModelConfig(_serviceFacade.ViewModelConfigService, mnemonic);
        }
    }
}