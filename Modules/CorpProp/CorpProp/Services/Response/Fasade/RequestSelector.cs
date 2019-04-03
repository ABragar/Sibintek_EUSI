using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using Base.DAL;
using Base.Utils.Common.Maybe;
using CorpProp.Entities.Request;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;

namespace CorpProp.Services.Response.Fasade
{
    public class RequestSelector : RequestQuery
    {
        private readonly ICellsData _cellsData;
        private readonly TableData _table;
        private readonly IUnitOfWork _unitOfWork;

        public RequestSelector(ICellsData cellsData, TableData table, IUnitOfWork unitOfWork)
        {
            _cellsData = cellsData;
            _table = table;
            _unitOfWork = unitOfWork;
        }

        public IQueryable FilterByRequestId(IQueryable query, int requestId)
        {
            return query.Where($"{RequestCellRequestIdName} = @0", requestId);
        }

        public IQueryable FilterByResponseId(IQueryable query, int responseId)
        {
            return query.Where($"{RequestCellLinkedResponseIdName} = @0", responseId);
        }

        public IQueryable GetResponseData()
        {
            var result = PivotProjection(_cellsData, _table);
            return result;
        }

        public class SourceTablesForRequest
        {
            public IQueryable<ResponseRow> ResponseRow { get; set; }
            public IQueryable<Entities.Request.Request> Request { get; set; }
            public IQueryable<Entities.Request.Response> Response { get; set; }
            public IQueryable<SibUser> SibUser { get; set; }
            public IQueryable<Society> Society { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="societyField">ID or etc.</param>
        /// <param name="resultQueryColumnName"></param>
        /// <returns></returns>
        public IQueryable AppendSociety(IQueryable data, string societyField, string resultQueryColumnName)
        {
            if (data == null)
                return null;
            var rows = from responseRow in _table.ResponseRow
                                 join response in _table.Response on responseRow.ResponseID equals response.ID
                                 join user in _table.SibUser on response.ExecutorID equals user.ID
                                 join society in _table.Society on user.SocietyID equals society.ID into userSocietyes
                                 from userSociety in userSocietyes.DefaultIfEmpty()
                                 select new { ResponseRow = responseRow, userSociety };

            var rowsAndSociety = rows.Select($"new(ResponseRow, userSociety.{societyField} as SocietyID)");
            var dataProps = data.ElementType.GetProperties().Select(pinfo => pinfo.Name);
            var selector = string.Join(", ", dataProps.Select(field => $"outer.{field}").Concat(new[] { $"inner.SocietyID as {resultQueryColumnName}" }));
            var newData = data.Join(rowsAndSociety, RequestCellLinkedRowName, $"ResponseRow.{BaseObjectIdFieldName}", $"new ( {selector} )");
            return newData;
        }

        public IQueryable HideFields(IQueryable data, IEnumerable<string> columnNames)
        {
            if (data == null)
                return null;
            var elementType = data.ElementType;
            var columns = elementType.GetProperties().Select(info => info.Name).Where(name => columnNames.Contains(name));
            var querySelectorFields = string.Join(", ", columns);
            return data.Select($"new ({querySelectorFields})");
        }

        private IEnumerable<Tuple<RequestColumn, RequestColumn>> GetTuple(IList<RequestColumn> columns)
        {
            bool first = true;
            RequestColumn prev = null;
            foreach (var current in columns)
            {
                if (first)
                {
                    prev = current;
                    first = false;
                    continue;
                }
                yield return new Tuple<RequestColumn, RequestColumn>(prev, current);
                prev = current;
            }
        }

        private IQueryable PivotProjection(ICellsData srcQueryes, TableData table)
        {
            var orderedColumnsList = table.ResponseColumns.OrderBy(column => column.SortOrder).ToList();

            var columnsCount = orderedColumnsList.Count;

            if (columnsCount <= 1)
                return null;

            IQueryable currentQuery = null;
            if (columnsCount > 1)
            {
                var tuples = GetTuple(orderedColumnsList);
                var first = true;
                foreach (var tuple in tuples)
                {
                    if (first)
                    {
                        var fsrcQuery = ResponseTypeDataFacade.GetQueryByCode(srcQueryes, tuple.Item1.TypeData);
                        currentQuery = fsrcQuery;
                        fsrcQuery = ResponseTypeDataFacade.GetQueryByCode(srcQueryes, tuple.Item2.TypeData);
                        currentQuery = JoinSelfFirst(currentQuery, fsrcQuery, tuple.Item1, tuple.Item2);
                        first = false;
                        continue;
                    }
                    var srcQuery = ResponseTypeDataFacade.GetQueryByCode(srcQueryes, tuple.Item2.TypeData);
                    currentQuery = JoinSelf(currentQuery, srcQuery, tuple.Item1, tuple.Item2);
                }
            }
            else if (columnsCount == 1)
            {
                currentQuery = ResponseTypeDataFacade.GetQueryByCode(srcQueryes, orderedColumnsList[0].TypeData);
            }

            currentQuery = currentQuery.Where(MakeWhereExpressionStr(orderedColumnsList));

            var proj = GetData(orderedColumnsList);
            try
            {
                currentQuery = currentQuery.Select(proj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return currentQuery;
        }

        private string GetData(IList<RequestColumn> orderedColumns)
        {
            var columnWherePathes = MakeColumnWherePathes(orderedColumns).ToArray();
            var fieldSelectors = columnWherePathes.Select(FieldSelector());
            var rowSelectorStr = RowSelector(columnWherePathes);
            var sortOrderSelectorStr = SortOrderSelector(columnWherePathes);
            var selectors = fieldSelectors.Concat(new[] {rowSelectorStr}).Concat(new[] {sortOrderSelectorStr});
            var projectionSelector = $"new ({string.Join(", ", selectors)})";
            return projectionSelector;
        }

        public string RowSelector(KeyValuePair<RequestColumn, string>[] columnWherePathes)
        {

            return
                (string.IsNullOrEmpty(columnWherePathes.First().Value) ? columnWherePathes.First().Value : columnWherePathes.First().Value + ".") + 
                $"{ColumnPropertyAliasNameMaker(columnWherePathes.First().Key)}.{RequestCellLinkedRowName} as {RequestCellLinkedRowName}";
        }

        public string SortOrderSelector(KeyValuePair<RequestColumn, string>[] columnWherePathes)
        {
            return
                (string.IsNullOrEmpty(columnWherePathes.First().Value) ? columnWherePathes.First().Value : columnWherePathes.First().Value + ".") +
                $"{ColumnPropertyAliasNameMaker(columnWherePathes.First().Key)}.{RequestSortOrderName} as {SelectorSortOrderFieldName}";
        }

        private Func<KeyValuePair<RequestColumn, string>, string> FieldSelector()
        {
            return
                s =>
                    (string.IsNullOrEmpty(s.Value) ? s.Value : s.Value + ".") +
                    $"{ColumnPropertyAliasNameMaker(s.Key)}.{RequestValueTheValueName} as {ColumnPropertyAliasNameMaker(s.Key)}";
        }

        private string MakeWhereExpressionStr(IList<RequestColumn> orderedColumns)
        {
            var columnWherePathes = MakeColumnWherePathes(orderedColumns);
            var columnWhereExpressions = columnWherePathes
                .Select(
                    s =>
                        (string.IsNullOrEmpty(s.Value) ? s.Value : s.Value + ".") +
                        $"{ColumnPropertyAliasNameMaker(s.Key)}.{RequestCellLinkedColumnName} == {s.Key.ID}")
                .Select(s => $"({s})");
            var whereExpressionStr = $"{string.Join(" && ", columnWhereExpressions)}";
            return whereExpressionStr;
        }

        private IEnumerable<KeyValuePair<RequestColumn, string>> MakeColumnWherePathes(
            IList<RequestColumn> orderedColumns)
        {
            int order = 0;
            foreach (var column in orderedColumns)
            {
                yield return
                    new KeyValuePair<RequestColumn, string>(column, MakeWherepathStr(order, orderedColumns.Count));
                order++;
            }
        }

        private string MakeWherepathStr(int i, int count)
        {
            if (count <= 0)
                throw new IndexOutOfRangeException();
            if (i >= count)
                throw new IndexOutOfRangeException();
            var repeats = i <= 1 ? count - 2 : count - i - 1;
            repeats = repeats >= 0 ? repeats : 0;
            return string.Join(".", Enumerable.Repeat(JoinOuterFieldName, repeats));
        }

        private IQueryable JoinSelfFirst(IQueryable innerQuery, IQueryable outerQuery, RequestColumn prevColumn,
            RequestColumn nextColumn)
        {
            var outer = OuterkeySelectorFirst();
            var inner = InnerKeySelector();
            var selector = ResultSelectorFirst(prevColumn, nextColumn);
            Debugger.Log(9, "",
                $"JoinSelfFirst outer=${outer}; inner=${inner}; selector=${selector}{Environment.NewLine}");
            return innerQuery.Join(outerQuery, outer, inner, selector);
        }

        private IQueryable JoinSelf(IQueryable innerQuery, IQueryable outerQuery, RequestColumn prevColumn,
            RequestColumn nextColumn)
        {
            var outer = OuterkeySelector(prevColumn);
            var inner = InnerKeySelector();
            var selector = ResultSelector(nextColumn);
            Debugger.Log(9, "", $"JoinSelf outer=${outer}; inner=${inner}; selector=${selector}{Environment.NewLine}");
            IQueryable result = null;
            try
            {
                result = innerQuery.Join(outerQuery, outer, inner, selector);
            }
            catch (Exception e)
            {
                Debugger.Log(9, "", e?.Message);
                throw;
            }
            return result;
        }

        private string OuterkeySelectorFirst()
        {
            return RequestCellLinkedRowName;
        }

        private string OuterkeySelector(RequestColumn prevColumn)
        {
            var result = $" {ColumnPropertyAliasNameMaker(prevColumn)}.{RequestCellLinkedRowName}";
            return result;
        }

        private string InnerKeySelector() => OuterkeySelectorFirst();

        private string ResultSelectorFirst(RequestColumn prevColumn, RequestColumn nextColumn)
        {
            var result =
                $"new (outer as {ColumnPropertyAliasNameMaker(prevColumn)}, inner as {ColumnPropertyAliasNameMaker(nextColumn)})";
            return result;
        }

        private string ResultSelector(RequestColumn nextColumn)
        {
            var result = $"new (outer as {JoinOuterFieldName}, inner as {ColumnPropertyAliasNameMaker(nextColumn)})";
            return result;
        }
    }


}
