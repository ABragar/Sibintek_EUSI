using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Base;
using Base.DAL;
using Base.Extensions;
using CorpProp.Entities.Request;
using CorpProp.Entities.Request.ResponseCells;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic;

namespace CorpProp.Services.Response.Fasade
{
    public class RequestUpdate : RequestQuery
    {
        private readonly Entities.Request.Response _response;
        private readonly ICellsData _cells;
        private readonly TableData _table;
        private readonly IUnitOfWork _unitOfWork;

        public RequestUpdate(Entities.Request.Response response, ICellsData cells, TableData table,
            IUnitOfWork unitOfWork)
        {
            _response = response;
            _cells = cells;
            _table = table;
            _unitOfWork = unitOfWork;
        }

        private static PropertyInfo GetProperty(object src, string field)
        {
            return src.GetType().GetProperty(field);
        }

        private void SetValue(object src, string field, object value)
        {
            var valueAsJObject = src as JObject;
            if (valueAsJObject != null)
                valueAsJObject[field] = new JValue(value);
            else
                GetProperty(src, field).SetValue(src, field);
        }

        private object GetValue(object src, string field)
        {
            var o = src as JObject;
            return o != null ? o[field] : GetProperty(src, field).GetValue(src);
        }

        private IEnumerable<Tuple<string, string>> GetFieldValuesTuples(object source)
        {
            var o = source as JObject;
            if (o == null)
                throw new ArgumentException("source должен быть приводим к JObject");
            var token = o.PropertyValues();
            foreach (var t in token)
            {
                yield return new Tuple<string, string>(t.Path, t.ToString());
            }
        }

        public class KeyValuePair<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
        }

        protected IEnumerable<KeyValuePair<RequestColumn, object>> ColumnValues(object src)
        {
            var querible =
                _table.ResponseColumns.ToList()
                    .Select(
                        column =>
                            new KeyValuePair<RequestColumn, object>()
                            {
                                Key = column,
                                Value = GetValue(src, ColumnPropertyAliasNameMaker(column))
                            });
            return querible;
        }


        private ResponseRow CreateRow()
        {
            var responseRow = new ResponseRow()
            {
                ResponseID = _response.ID
            };

            responseRow = _unitOfWork.GetRepository<ResponseRow>().Create(responseRow);
            return responseRow;
        }

        public static void SetIResponseCellPropertyValue<T, TProperty>(T target,
            Expression<Func<IResponseCell<object>, TProperty>> memberLamda, TProperty value)
        {
            var memberExpression = GetMemberExpression(memberLamda);

            if (memberExpression == null) return;
            var property = memberExpression.Member as PropertyInfo;
            if (property == null) return;
            var tproperty = typeof(T).GetProperty(property.Name);
            if (tproperty == null) return;
            tproperty.SetValue(target, value, null);
        }

        private static MemberExpression GetMemberExpression<TInput, TProperty>(
            Expression<Func<TInput, TProperty>> memberLamda)
        {
            MemberExpression memberExpression = null;

            switch (memberLamda.Body.NodeType)
            {
                case ExpressionType.Convert:
                    var body = (UnaryExpression) memberLamda.Body;
                    memberExpression = body.Operand as MemberExpression;
                    break;
                case ExpressionType.MemberAccess:
                    memberExpression = memberLamda.Body as MemberExpression;
                    break;
            }

            return memberExpression;
        }


        private T CreateValue<T>(RequestColumn column, ResponseRow row, object value)
            where T : class
        {
            var cell = typeof(T).GetConstructor(new Type[] {})?.Invoke(new object[] {}) as T;
            if (cell == null)
                //TODO Системные ошибки писать в лог (настроить логирование в проекте)
                throw new NotSupportedException($"Не удалось создать экземпляр класса {typeof(T).Name}");
            SetIResponseCellPropertyValue(cell, responseCell => responseCell.LinkedRowID, row.ID);
            SetIResponseCellPropertyValue(cell, responseCell => responseCell.LinkedResponseID, _response.ID);
            SetIResponseCellPropertyValue(cell, responseCell => responseCell.LinkedColumnID, column.ID);
            SetPopertyIResponseCellValue(value, cell);
            return cell;
        }

        private void SetPopertyIResponseCellValue<T>(object value, T cell) where T : class
        {
            // ReSharper disable once LocalNameCapturedOnly
            IResponseCell<object> sampleResponseCell;
            const string valuePropName = nameof(sampleResponseCell.Value);
            var memberInfo = typeof(T).GetProperty(valuePropName);
            if (memberInfo != null)
            {
                var valueType = memberInfo.PropertyType;
                var typingObject = FromStringToType(valueType, value);
                var propertyInfo = memberInfo;
                propertyInfo.SetValue(cell, typingObject);
            }
            else
            {
                throw new NotSupportedException(
                    $"Не удалось получить свойство {valuePropName} в объекте типа {typeof(T).Name}");
            }
        }


        private object FromStringToType(Type type, object value)
        {
            var strValue = value.ToString();
            string converterName;
            if (type.IsGenericType && type.IsNullable())
            {
                converterName = nameof(ResponseCellValueConverter.NulableConverter);
                var baseType = type.GetGenericArguments()[0];
                if (!baseType.IsClass)
                    type = baseType;
            }
            else
            {
                converterName = nameof(ResponseCellValueConverter.NonNullableConverter);
            }
            var responseCellValueConverterType = typeof(ResponseCellValueConverter);
            object typedValue;

            var methodInfo = responseCellValueConverterType.GetMethod(converterName);
            if (methodInfo != null)
            {
                var getMethod = methodInfo.GetGenericMethodDefinition();
                var makeGenericMethod = getMethod.MakeGenericMethod(type);
                typedValue = makeGenericMethod.Invoke(null, new object[] {strValue});
            }
            else
            {
                throw new NotSupportedException(
                    $"Не найден метод {converterName} в классе {responseCellValueConverterType.Name}");
            }

            return typedValue;
        }

        private static Delegate CreateStaticMethodDelegate(MethodInfo methodInfo)
        {
            Func<Type[], Type> getType;
            var isAction = methodInfo.ReturnType == (typeof(void));
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);
            if (isAction)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] {methodInfo.ReturnType});
            }
            var methodDelegate = methodInfo.CreateDelegate(getType(types.ToArray()));
            return methodDelegate;
        }

        private object CreateCallOnRepo(Type callType, object cell)
        {
            // ReSharper disable once LocalNameCapturedOnly
            IUnitOfWork sampleUow;
            var methodInfo = _unitOfWork.GetType()
                .GetMethod(nameof(sampleUow.GetRepository));
            if (methodInfo != null)
            {
                var repoMethod = methodInfo
                    .MakeGenericMethod(callType);
                var repox = repoMethod.Invoke(_unitOfWork, new object[] {});
                // ReSharper disable once LocalNameCapturedOnly
                IRepository<BaseObject> sampleRepository;
                var createOnRepoMethod = repox.GetType().GetMethod(nameof(sampleRepository.Create));
                if (createOnRepoMethod != null)
                {
                    var createdObject = createOnRepoMethod.Invoke(repox, new[] {cell});
                    return createdObject;
                }
                else
                {
                    throw new NotSupportedException(
                        $"Не удалось получить метод {nameof(sampleRepository.Create)} класса {repox.GetType().Name}");
                }
            }
            else
            {
                throw new NotSupportedException(
                    $"Не удалось получить метод {nameof(sampleUow.GetRepository)} класса {_unitOfWork.GetType().Name}");
            }
        }

        private void CreateCells(object toSave, ResponseRow row)
        {
            var columnNameByValues = GetFieldValuesTuples(toSave);
            var columnIdByValue = columnNameByValues.ToDictionary(
                tuple => ColumnIdFromColumnPropertyAlias(tuple.Item1), tuple => tuple.Item2);
            var columns = from column in _table.ResponseColumns
                join cid in columnIdByValue.Keys on column.ID equals cid
                select column;
            columns = columns.Distinct().OrderBy(keySelector: column => column.SortOrder);
            var columnsList = columns.ToList();
            foreach (var column in columnsList)
            {
                var currentColumn = column;
                var currentCallValue = columnIdByValue[column.ID];
                
                var cellType = ResponseTypeDataFacade.GetQueryByCode(_cells, currentColumn.TypeData).ElementType;
                var mi = GetType()
                    .GetRuntimeMethods().First(info => info.Name == nameof(CreateValue) &&
                                                       info.GetParameters()
                                                           .Select(parameterInfo => parameterInfo.ParameterType)
                                                           .Intersect(new[]
                                                               {currentColumn.GetType(), row.GetType(), typeof(object)})
                                                           .First() != null);
                var methodInfo = mi.MakeGenericMethod(cellType);


                var cell = methodInfo
                    .Invoke(this, new object[] {currentColumn, row, currentCallValue});
                CreateCallOnRepo(cellType, cell);
            }
        }

        public int SaveRowModel(object saveObject)
        {
            int? linkedRowID = ((dynamic) saveObject).LinkedRowID;
            if (linkedRowID == null)
            {
                var row = CreateRow();
                _unitOfWork.SaveChanges();
                CreateCells(saveObject, row);
                linkedRowID = row.ID;
            }
            else
            {
                UpdateValues<ResponseCellBoolean, bool?>(saveObject, linkedRowID);
                UpdateValues<ResponseCellDateTime, DateTime?>(saveObject, linkedRowID);
                UpdateValues<ResponseCellDecimal, decimal?>(saveObject, linkedRowID);
                UpdateValues<ResponseCellDict, int?>(saveObject, linkedRowID);
                UpdateValues<ResponseCellDouble, double?>(saveObject, linkedRowID);
                UpdateValues<ResponseCellFloat, float?>(saveObject, linkedRowID);
                UpdateValues<ResponseCellInt, int?>(saveObject, linkedRowID);
                UpdateValues<ResponseCellString, string>(saveObject, linkedRowID);
            }
            _unitOfWork.SaveChanges();
            return linkedRowID.Value;
        }

        private void UpdateValues<T, TItem>(object saveObject, int? linkedRowID)
            where T : BaseObject
        {
            var repo = _unitOfWork.GetRepository<T>();

            var values = repo.All().Where("it.LinkedRowID = @0", linkedRowID);
            UpdateOnlyValues<TItem>(values, saveObject);
            values.ForEach(s => repo.Update(s));
        }

        private void UpdateOnlyValues<TItem>(IEnumerable<dynamic> values, object saveObject)
        {
            values.ForEach(s => s.Value = (TItem) GetValues(s, saveObject));
        }

        private T GetValues<T>(IResponseCell<T> dest, object src)
        {
            dynamic val = GetValue(src, $"{ColumnPropertyAliasNameMaker(dest.LinkedColumnID)}");
            var u = Nullable.GetUnderlyingType(typeof(T));
            var value = val.Value;
            if (value == null)
                return default(T);
            TypeConverter conv;
            if (u != null)
            {
                if (value is string && string.IsNullOrWhiteSpace(value))
                    return default(T);
                conv = TypeDescriptor.GetConverter(u);

                if (value is string)
                {
                    if (u == typeof(DateTime))
                    {
                        DateTime dt;
                        if (DateTime.TryParseExact(value, "dd.MM.yyyy", null, DateTimeStyles.None, out dt)
                            || DateTime.TryParseExact(value, "dd/MM/yyyy", null, DateTimeStyles.None, out dt))
                            return (T)(dynamic)dt;
                    }
                    if (u == typeof(decimal))
                        value = (value as string).Replace(',', '.');
                    return conv.ConvertFromInvariantString(value);
                }
                else
                    return (T) conv.ConvertTo(value, u);
            }
            conv = TypeDescriptor.GetConverter(value);
            return conv.ConvertTo(value, typeof(T));
        }

        private void DeleteRow<T>(int? responseId, int? linkedRowID)
                where T : BaseObject
        {
            var repo = _unitOfWork.GetRepository<T>();
            var values = repo.All().Where("it.LinkedRowID = @0", linkedRowID);
            values.ForEach(s => repo.Delete(s));
        }

        public int DeleteRow(int linkedRowID)
        {
            var responseId = _response.ID;
            DeleteRow<ResponseCellBoolean>(responseId, linkedRowID);
            DeleteRow<ResponseCellDateTime>(responseId, linkedRowID);
            DeleteRow<ResponseCellDecimal>(responseId, linkedRowID);
            DeleteRow<ResponseCellDict>(responseId, linkedRowID);
            DeleteRow<ResponseCellDouble>(responseId, linkedRowID);
            DeleteRow<ResponseCellFloat>(responseId, linkedRowID);
            DeleteRow<ResponseCellInt>(responseId, linkedRowID);
            DeleteRow<ResponseCellString>(responseId, linkedRowID);
            _unitOfWork.SaveChanges();
            return linkedRowID;
        }


    }
}
