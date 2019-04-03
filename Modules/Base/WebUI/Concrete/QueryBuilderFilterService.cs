using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Base;
using Base.Attributes;
using Base.Extensions;
using Base.UI;
using WebUI.Models.QueryBuilderFilter;

namespace WebUI.Concrete
{
    public class QueryBuilderFilterService
    {
        private readonly IViewModelConfigService _viewModelConfigService;

        public QueryBuilderFilterService(IViewModelConfigService viewModelConfigService)
        {
            _viewModelConfigService = viewModelConfigService;
        }

        //получить фильтр для querybuilder
        private QueryFilterVm GetQueryFilterVm(string id, string label, QueryBuilderType type, string system_type, List<QueryBuilderOperator> operators,
            string additional_info = null)
        {
            return new QueryFilterVm()
            {
                id = id,
                label = label,
                type = type.ToString().ToLower(),
                data = new QueryFilterDataVm()
                {
                    system_type = system_type,
                    additional_info = additional_info
                },
                operators = operators.Select(x => x.ToString()).ToList()
            };
        }

        //возвращает коллекцию фильтров для querybuilder
        public IEnumerable<QueryFilterVm> GetFilters(string mnemonic)
        {
            var editors = _viewModelConfigService.Get(mnemonic).ListView.Columns;

            List<QueryFilterVm> result = new List<QueryFilterVm>();

            foreach (var editor in editors)
            {
                if (editor.PropertyType == typeof(string) && (editor.PropertyDataTypeName == "String" || editor.PropertyDataTypeName == "MultilineText" || editor.PropertyDataTypeName == "Text"))
                {
                    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.STRING, editor.PropertyDataTypeName,
                        new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.not_equal, QueryBuilderOperator.contains, QueryBuilderOperator.not_contains, QueryBuilderOperator.is_null, QueryBuilderOperator.is_not_null }));
                }
                else if (editor.PropertyType == typeof(int) && editor.PropertyDataTypeName == "Integer" || editor.PropertyDataTypeName == "Number")
                {
                    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.INTEGER, editor.PropertyDataTypeName,
                        new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.not_equal, QueryBuilderOperator.less, QueryBuilderOperator.less_or_equal, QueryBuilderOperator.greater, QueryBuilderOperator.greater_or_equal }));
                }
                else if (editor.PropertyType == typeof(int?) && editor.PropertyDataTypeName == "Integer" || editor.PropertyDataTypeName == "Number")
                {
                    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.INTEGER, editor.PropertyDataTypeName,
                        new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.not_equal, QueryBuilderOperator.less, QueryBuilderOperator.less_or_equal, QueryBuilderOperator.greater, QueryBuilderOperator.greater_or_equal, QueryBuilderOperator.is_null, QueryBuilderOperator.is_not_null }));
                }
                else if (editor.PropertyType == typeof(double) || editor.PropertyType == typeof(decimal) || editor.PropertyType == typeof(float))
                {
                    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.DOUBLE, editor.PropertyDataTypeName,
                        new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.not_equal, QueryBuilderOperator.less, QueryBuilderOperator.less_or_equal, QueryBuilderOperator.greater, QueryBuilderOperator.greater_or_equal }));
                }
                else if ( editor.PropertyType == typeof(double?) || editor.PropertyType == typeof(decimal?) || editor.PropertyType == typeof(float?))
                {
                    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.DOUBLE, editor.PropertyDataTypeName,
                        new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.not_equal, QueryBuilderOperator.less, QueryBuilderOperator.less_or_equal, QueryBuilderOperator.greater, QueryBuilderOperator.greater_or_equal, QueryBuilderOperator.is_null, QueryBuilderOperator.is_not_null }));
                }
                else if (editor.PropertyType == typeof(bool) && editor.PropertyDataTypeName == "Boolean")
                {
                    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.BOOLEAN, editor.PropertyDataTypeName,
                        new List<QueryBuilderOperator>() { QueryBuilderOperator.equal }));
                }
                else if (editor.PropertyType == typeof(bool?) && editor.PropertyDataTypeName == "Boolean")
                {
                    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.BOOLEAN, editor.PropertyDataTypeName,
                        new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.is_null, QueryBuilderOperator.is_not_null }));
                }
                else if (editor.PropertyType == typeof(DateTime))
                {

                    if (editor.PropertyDataTypeName == Enum.GetName(typeof(PropertyDataType), PropertyDataType.Date))
                    {
                        result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.DATE, editor.PropertyDataTypeName,
                            new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.not_equal, QueryBuilderOperator.less, QueryBuilderOperator.less_or_equal, QueryBuilderOperator.greater, QueryBuilderOperator.greater_or_equal }));
                    }
                }
                else if (editor.PropertyType == typeof(DateTime?))
                {
                    if (editor.PropertyDataTypeName == Enum.GetName(typeof(PropertyDataType), PropertyDataType.Date))
                    {
                        result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.DATE, editor.PropertyDataTypeName,
                            new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.not_equal, QueryBuilderOperator.less, QueryBuilderOperator.less_or_equal, QueryBuilderOperator.greater, QueryBuilderOperator.greater_or_equal, QueryBuilderOperator.is_null, QueryBuilderOperator.is_not_null }));
                    }
                }
                else if (editor.PropertyType.IsSubclassOf(typeof(BaseObject)))
                {
                    if (editor.PropertyDataTypeName == "BaseObjectOne")
                    {
                        var additional_info = editor.ViewModelConfig.Mnemonic;
                        result.Add(GetQueryFilterVm($"{editor.PropertyName}", $"{editor.Title}", QueryBuilderType.INTEGER, editor.PropertyDataTypeName,
                            new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.is_null, QueryBuilderOperator.is_not_null }, additional_info));
                    }
                }
                else if (editor.PropertyType.IsEnum)
                {
                    var additional_info = editor.PropertyType.GetEnumType().GetTypeName();
                    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title,
                        QueryBuilderType.INTEGER, editor.PropertyDataTypeName,
                        new List<QueryBuilderOperator>() { QueryBuilderOperator.equal, QueryBuilderOperator.is_null, QueryBuilderOperator.is_not_null }, additional_info));
                }
                //TODO: 
                //else if (editor.EditorTemplate == "EasyCollection")
                //{
                //    var additional_info = editor.ViewModelConfig.Mnemonic;
                //    result.Add(GetQueryFilterVm(editor.PropertyName, editor.Title, QueryBuilderType.INTEGER, "EasyCollection", new List<QueryBuilderOperator>() { QueryBuilderOperator.any }, additional_info));
                //}
            }
            return result.OrderBy(ord=>ord.label);            
        }
    }
}