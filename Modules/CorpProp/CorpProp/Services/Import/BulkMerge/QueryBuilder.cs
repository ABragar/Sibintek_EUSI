using Base.Extensions;
using Base.Utils.Common.Maybe;
using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using CorpProp.Entities.Import;
using Base.Security;

namespace CorpProp.Services.Import.BulkMerge
{
    public abstract class QueryBuilder
    {
        #region Protected Fields

        protected readonly Dictionary<string, string> ColsNameMapping;
        protected readonly Type DictObjectType = typeof(DictObject);
        protected Type MainType;

        #endregion Protected Fields

        #region Private Fields

        private List<SqlTableProperties> _propertiesByTable;

        private string _tmpTableName;

        #endregion Private Fields

        #region Public Constructors

        public QueryBuilder(Dictionary<string, string> colsNameMapping, Type type)
        {
            MainType = type;
            ColsNameMapping = colsNameMapping;
#if DEBUG
            _tmpTableName = $"##Tmp_{this.MainType.Name}_{System.Guid.NewGuid().ToString().ToLower().Replace("-", "_")}";
#else
            _tmpTableName =  $"#Tmp_{this.MainType.Name}";
#endif
        }

        #endregion Public Constructors

        #region Public Properties

        public List<SqlTableProperties> PropertiesByTable => _propertiesByTable;

        /// <summary>
        /// Получает или задает кастомизированные колонки для временной таблицы вставки.
        /// </summary>
        public Dictionary<string, Type> TempTableCustomColumns { get; set; }


        #endregion Public Properties

        #region Protected Properties

        protected SqlCommand Command { get; set; }

        protected virtual IEnumerable<string> ObjectProperties => MainType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.CanWrite).Select(x => x.Name);

        protected List<SqlColumnDefinition> SqlColumnDefinitions { get; set; }

        #endregion Protected Properties

        #region Public Methods

        public abstract string BuildMergeQuery(ImportHistory history);

        /// <summary>
        /// Создание временной таблицы для BulkInsert
        /// </summary>
        /// <param name="command"></param>
        public virtual void CreateTempTable(SqlCommand command)
        {
            var createTableScript = GetCreateTableScript();
            command.CommandText = createTableScript;
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Разделяем св-ва для базового класса(включая абстракные) и потомков
        /// </summary>
        public void Init(SqlCommand command)
        {
            this.Command = command;
            this._propertiesByTable = new List<SqlTableProperties>();
            var currentType = MainType;
            var flag = true;
            Type nonAbstractType = null;
            SqlTableProperties currentSqlFieldCollection = null;
            while (flag)
            {
                if (currentType.BaseType == null)
                {
                    flag = false;
                }

                if (!currentType.IsAbstract)
                {
                    nonAbstractType = currentType;
                    currentSqlFieldCollection = new SqlTableProperties(nonAbstractType);
                    _propertiesByTable.Add(currentSqlFieldCollection);
                }

                if (currentType.BaseType.IsAbstract)
                {
                    flag = false;
                    currentType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Where(x => x.CanWrite).ForEach(x => currentSqlFieldCollection.Add(x));
                }
                else
                {
                    currentType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                        .Where(x => x.CanWrite).ForEach(x => currentSqlFieldCollection.Add(x));
                }

                currentType = currentType.BaseType;
            }
            SqlColumnDefinitions = GetSqlColumnDefinition(MainType);
        }

        public virtual int Merge(SqlCommand command, ref ImportHistory history)
        {
            command.CommandText = BuildMergeQuery(history);
            return (int)command.ExecuteScalar();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Описывает декларацию табличной переменной для хранения результатов MERGE
        /// </summary>
        /// <returns></returns>
        protected virtual string GetColumnSpecification()
        {
            var sb = new StringBuilder();
            sb.AppendLine("action nvarchar(10)");

            foreach (var columnDefinition in SqlColumnDefinitions)
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine(",");
                }

                var dataType = columnDefinition.DataType.ToLower() == "nvarchar"
                    ? "nvarchar(max)"
                    : columnDefinition.DataType;

                var defaultValue = columnDefinition.IsNullable ? string.Empty : $"default {GetSqlDefaultValue(columnDefinition)}";

                sb.Append($"[{columnDefinition.ColumnName}] {dataType} {defaultValue}");
            }

            return sb.ToString();
        }

        protected string GetComplexPropertyId(SqlTableProperties childTable, string argName)
        {
            var propertyIdName = childTable.Properties.FirstOrDefault(x => x.Name == $"{argName}ID").With(x => x.Name);
            if (propertyIdName == null)
            {
                propertyIdName = $"{argName}_ID";
            }

            return propertyIdName;
        }

        /// <summary>
        /// Возвращает строку запроса в SQL-формате, для создания таблицы, сформированную на основе
        /// массива свойств типа объекта
        /// </summary>
        /// <param name="tableName">Имя создаваемой таблицы</param>
        /// <param name="properties">Массив свойств типа объекта</param>
        /// <returns></returns>
        protected virtual string GetCreateTableScript()
        {
            var tableName = GetTempTableName();
            var properties = MainType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite).ToArray();
            var createScript = new StringBuilder();
            createScript.AppendLine($"IF OBJECT_ID(N'tempdb..{tableName}') IS NOT NULL DROP TABLE {tableName} \r\n CREATE TABLE {tableName} ({GetTempTableColumnsSpecification(properties)})");
            return createScript.ToString();
        }

        protected string GetDBType(Type propertyType)
        {
            SqlParameter param;
            System.ComponentModel.TypeConverter tc;
            param = new SqlParameter();
            tc = System.ComponentModel.TypeDescriptor.GetConverter(param.DbType);
            var underlyingPropertyType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            if (propertyType.Name == "Currency")
            {
                param.SqlDbType = SqlDbType.NVarChar;
            }
            else if (tc.CanConvertFrom(underlyingPropertyType))
            {
                param.DbType = (DbType)tc.ConvertFrom(underlyingPropertyType.Name);

                if (param.DbType.ToString() == typeof(decimal).Name)
                {
                    param.Precision = 31;
                    param.Scale = 8;
                }
            }
            else
            {
                // try to forcefully convert
                try
                {
                    param.DbType = (DbType)tc.ConvertFrom(underlyingPropertyType.Name);

                    if (param.DbType.ToString() == typeof(decimal).Name)
                    {
                        param.Precision = 31;
                        param.Scale = 8;
                    }
                }
                catch (Exception e)
                {
                    // ignore the exception
                }
            }

            var result = param.SqlDbType == SqlDbType.NVarChar ? "nvarchar(max)" : param.SqlDbType.ToString();
            if ((param.DbType.ToString() == typeof(decimal).Name) && param.Precision != 0 && param.Scale != 0)
            {
                result = string.Format("{0}({1},{2})", param.SqlDbType.ToString(), param.Precision, param.Scale);
            }
            if (propertyType.IsPrimitive && propertyType.IsValueType)
            {
                result = $"{result} DEFAULT 0";
            }
            return result;
        }

        /// <summary>
        /// Возвращает строку с разделителем столбцов для вставки, включая обязательные колонки
        /// </summary>
        /// <returns></returns>
        protected virtual string GetInsertColumnSpecification()
        {
            var insertColumns = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);

            return String.Join(",\r\n", insertColumns.Select(x => $"[{x}]").ToArray());
        }

        /// <summary>
        /// Возвращает строку с разделителем столбцов для вставки, включая обязательные колонки
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSelectColumnSpecification(string prefix)
        {
            var insertColumns = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);

            return String.Join(",\r\n", insertColumns.Select(x => string.IsNullOrEmpty(prefix) ? $"[{x}]" : $"{prefix}.[{x}]").ToArray());
        }

        /// <summary>
        /// Возвращает перечень колонок для OUTPUT
        /// </summary>
        /// <returns></returns>
        protected virtual string GetOutputColumnSpecification()
        {
            var result = string.Join(",\r\n", GetSqlTableColumns().Select(x => $"inserted.[{x}]"));

            return result;
        }

        protected List<string> GetRequiredFields(Type type)
        {
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite);

            var res = new List<string>();
            foreach (var propertyInfo in props)
            {
                if (propertyInfo.PropertyType.IsPrimitive)
                {
                    res.Add($"{type.Name}_{propertyInfo.Name}");
                }
                else if (!IsPrimitiveType(propertyInfo.PropertyType))
                {
                    //complex
                    var isComplex = Attribute.GetCustomAttribute((propertyInfo.PropertyType), typeof(ComplexTypeAttribute));
                    if (isComplex != null)
                    {
                        var ComplexProps = GetRequiredFields(propertyInfo.PropertyType);
                        res.AddRange(ComplexProps);
                    }
                }
            }

            return res.Distinct().ToList();
        }

        /// <summary>
        /// Подзапрос для получения ID
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        protected string GetSelectSubQuery(PropertyInfo prop, string colName, string prefix = null)
        {            
            if (prop == null)
            {
                throw new Exception("Не удалось получить информацию по определению атрибута <" + (string.IsNullOrEmpty(colName) ? "" : colName) + ">.");
            }

            if (String.IsNullOrEmpty(prefix))
                prefix = "source";
            prefix = $"{prefix}.";

            if (Type.Equals(prop.PropertyType, typeof(DictObject)) ||
                prop.PropertyType.IsSubclassOf(typeof(DictObject)))
            {
                return GetDictValueSelect(prop.PropertyType, $"{prefix}[{colName}]");               
            }

            if (Type.Equals(prop.PropertyType, typeof(Society)))
            {
                return $"(SELECT TOP 1 t1.ID FROM {GetTableName(prop.PropertyType)} AS t1 WHERE t1.Hidden = 0 AND t1.IsHistory = 0 AND t1.IDEUP = {prefix}[{colName}])";
            }
            if (Type.Equals(prop.PropertyType, typeof(CorpProp.Entities.Subject.Subject)))
            {
                return $"(SELECT TOP 1 t.ID " +
                    $"FROM {GetTableName(prop.PropertyType)} AS t " +
                    $"left outer join (SELECT top 1 split.a.value('.', 'VARCHAR(100)') AS Val FROM (SELECT Cast ('<M>' + Replace(Replace(isnull({prefix}[{colName}],''),' ',''), '/', '</M><M>')+ '</M>' AS XML) AS Data) AS A CROSS apply data.nodes ('/M') AS Split(a)) as tValue on t.sdp=tValue.Val or t.inn=tValue.Val or t.KSK=tValue.Val " +
                    $"left outer join (select id as 'subjID', INN,KPP from [CorpProp.Subject].Subject) as tValue2 on ((isnull(replace(tValue2.inn,' ',''), '') + '/' + isnull(replace(tValue2.kpp,' ',''),''))=Replace(isnull({prefix}[{colName}],''),' ','')) and tValue2.subjID=t.ID " +                  
                    $"WHERE (tValue2.subjID is not null or tValue.Val is not null)  and tValue.Val != '' and  t.Hidden = 0 AND t.IsHistory = 0)";
            }
            if (Type.Equals(prop.PropertyType, typeof(CorpProp.Entities.Law.Right)))
            {
                return $"(SELECT TOP 1 t1.ID FROM {GetTableName(prop.PropertyType)} AS t1 WHERE t1.Hidden = 0 AND t1.IsHistory = 0 AND t1.RegNumber = {prefix}[{colName}])";
            }

            if (Type.Equals(prop.PropertyType, typeof(CorpProp.Entities.Document.FileCard)) || prop.PropertyType.IsSubclassOf(typeof(CorpProp.Entities.Document.FileCard)))
                return $"(SELECT TOP 1 t1.ID FROM {GetTableName(typeof(CorpProp.Entities.Document.FileCard))} AS t1 WHERE t1.Name = {prefix}[{colName}] AND t1.Hidden = 0 AND t1.IsHistory = 0)";

            if (Type.Equals(prop.PropertyType, typeof(CorpProp.Entities.DocumentFlow.SibDeal)) || prop.PropertyType.IsSubclassOf(typeof(CorpProp.Entities.DocumentFlow.SibDeal)))
                return $"(SELECT TOP 1 t1.ID " +
                    $"FROM {GetTableName(prop.PropertyType)} AS t1 " +
                    $"INNER JOIN  {GetTableName(typeof(CorpProp.Entities.DocumentFlow.Doc))} AS t2 ON t2.ID = t1.ID " +
                    $"WHERE t2.Number = {prefix}[{colName}] AND t2.Hidden = 0 AND t2.IsHistory = 0) ";

            if (Type.Equals(prop.PropertyType, typeof(CorpProp.Entities.Security.SibUser)) || prop.PropertyType.IsSubclassOf(typeof(CorpProp.Entities.Security.SibUser)))
                return $"(SELECT TOP 1 t1.ID " +
                    $"FROM {GetTableName(prop.PropertyType)} AS t1 " +                  
                    $"INNER JOIN  {GetTableName(typeof(BaseProfile))} AS t2 ON t1.ID = t2.ID " +
                    $"WHERE {prefix}[{colName}] IS NOT NULL AND {prefix}[{colName}] = (t2.LastName + N' '+t2.FirstName + N' '+ t2.MiddleName) AND t2.Hidden = 0 AND t2.IsHistory = 0)";
            

            if (prop.PropertyType.GetProperty("ExternalID") != null)
            {
                return $"(SELECT TOP 1 t1.ID FROM {GetTableName(prop.PropertyType)} AS t1 WHERE t1.Hidden = 0 AND t1.ExternalID = {prefix}[{colName}])";
            }

            return
                $"(SELECT TOP 1 t1.ID FROM {GetTableName(prop.PropertyType)} AS t1 WHERE t1.ID = {prefix}[{colName}])";
        }

        /// <summary>
        /// Перечень выражений для UPDATE запроса (SET)
        /// </summary>
        /// <returns></returns>
        protected virtual string GetSetSpecification()
        {
            var setSpecification = new StringBuilder();

            foreach (var keyValue in ColsNameMapping)
            {
                var colName = keyValue.Value;
                if (!ObjectProperties.Contains(colName))
                {
                    continue;
                }

                var prop = MainType.GetProperty(colName);

                if (setSpecification.Length > 0)
                {
                    setSpecification.Append(",");
                }
                if (IsPrimitiveType(prop.PropertyType))
                {
                    /*if (colName == "ExternalID")
                    setSpecification.AppendLine($" target.[{colName}] = case when target.[{colName}] is not null and target.[{colName}] <> source.[{colName}] then source.[{colName}] else NULL end");
                    else*/
                    setSpecification.AppendLine($" target.[{colName}] = source.[{colName}]");
                }
                else
                {
                    var idColName = $"{colName}ID";

                    setSpecification.AppendLine($" target.[{idColName}] = {GetSelectSubQuery(prop, colName)}");
                }
            }
            setSpecification.AppendLine($",target.ImportUpdateDate = getDate()");
            return setSpecification.ToString();
        }

        protected virtual string GetShortTableName(Type value)
        {
            return $"{value.Name}";
        }

        protected List<SqlColumnDefinition> GetSqlColumnDefinition(Type type)
        {
            var getColumnsQuery =
                $"SELECT COLUMN_NAME, COLUMN_DEFAULT, IS_NULLABLE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH " +
                $"FROM INFORMATION_SCHEMA.COLUMNS " +
                $"WHERE TABLE_SCHEMA = '{GetTableSchemaName(type)}' " +
                $"AND TABLE_NAME = '{GetShortTableName(type)}'" +
                $"AND COLUMN_NAME NOT IN ('RowVersion', 'Discriminator')";

            Command.CommandText = getColumnsQuery;
            var sqlReader = Command.ExecuteReader();
            var result = new List<SqlColumnDefinition>();
            while (sqlReader.Read())
            {
                result.Add(new SqlColumnDefinition()
                {
                    ColumnName = GetDbReaderValue(sqlReader.GetValue(0))?.ToString(),
                    ColumnDefault = GetDbReaderValue(sqlReader.GetValue(1))?.ToString(),
                    IsNullable = GetDbReaderValue(sqlReader.GetValue(2))?.ToString() == "YES",
                    DataType = GetDbReaderValue(sqlReader.GetValue(3))?.ToString(),
                    CharacterMaximumLenght = Convert.ToInt32(GetDbReaderValue(sqlReader.GetValue(4)))
                });
            }

            sqlReader.Close();
            return result;
        }

        protected string GetSqlDefaultValue(SqlColumnDefinition columnDefinition)
        {
            switch (columnDefinition.DataType.ToLower())
            {
                case "uniqueidentifier":
                    {
                        return "NEWID()";
                    }
                case "datetime":
                    {
                        return "GETDATE()";
                    }

                default: return "0";
            }
        }

        /// <summary>
        /// Возвращает список столбцов для вставки, включая обязательные колонки
        /// </summary>
        /// <returns></returns>
        protected virtual List<string> GetSqlTableColumns()
        {
            return SqlColumnDefinitions.Select(x => x.ColumnName).ToList();
        }

        protected virtual string GetTableName(Type value)
        {
            return $"[{GetTableSchemaName(value)}].{value.Name}";
        }

        protected virtual string GetTableSchemaName(Type value)
        {
            return $"{value.Namespace.Replace(".Entities", String.Empty)}";
        }

        protected virtual string GetTempTableColumnsSpecification(PropertyInfo[] properties, string prefix = "")
        {
            var sb = new StringBuilder();
            if (String.IsNullOrWhiteSpace(prefix))
            {
                sb.AppendLine($"RowNumb INT NOT NULL IDENTITY(1,1),");    //номер строки
                sb.AppendLine($"RowID uniqueidentifier DEFAULT NEWID()"); //УИД строки     
            }                     
            foreach (var propertyInfo in properties)
            {
                var isComplex = Attribute.GetCustomAttribute((propertyInfo.PropertyType), typeof(ComplexTypeAttribute));
                if (isComplex != null)
                {
                    if (sb.Length > 0)
                    {
                        sb.AppendLine(",");
                    }
                    sb.AppendLine(GetTempTableColumnsSpecification(propertyInfo.PropertyType.GetProperties(), $"{propertyInfo.Name}_"));
                }

                if (sb.Length > 0)
                {
                    sb.AppendLine(",");
                }
                sb.Append($"\r\n[{prefix}{propertyInfo.Name}] {GetDBType(propertyInfo.PropertyType)}");
                if (!IsPrimitiveType(propertyInfo.PropertyType) && properties.All(x => x.Name != $"{propertyInfo.Name}ID")) //если не определено нав. св-во
                {
                    sb.Append($"\r\n, [{prefix}{propertyInfo.Name}_ID] INT");
                }
            }

            //TODO: проверять и формировать уникальное имя для кастомных колонок временной таблицы вставки
            // в случае, если они совпадают со свойствами основного типа.
            if (TempTableCustomColumns != null)
                foreach (var customColumn in TempTableCustomColumns)
                {
                    sb.Append($"\r\n,[{prefix}{customColumn.Key}] {GetDBType(customColumn.Value)}");
                }


            return sb.ToString();
        }

        public string GetTempTableName()
        {
            return _tmpTableName;
        }

        /// <summary>
        /// Описание Значений для Insert
        /// </summary>
        /// <returns></returns>
        protected virtual string GetValuesSpecification()
        {
            var val = new StringBuilder();
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate" || x.ColumnName.ToLower() == "actualdate")
                {
                    return $"GetDate()";
                }
                return x.IsNullable ? $"source.[{x.ColumnName}]" : $"ISNULL(source.[{x.ColumnName}],{GetSqlDefaultValue(x)})";
            });

            var setSpecification = new StringBuilder();

            foreach (var keyValue in ColsNameMapping)
            {
                var colName = keyValue.Value;
                if (!ObjectProperties.Contains(colName))
                {
                    continue;
                }
                var prop = MainType.GetProperty(colName);

                if (IsPrimitiveType(prop.PropertyType))
                {
                    if (values.ContainsKey(colName))
                    {
                        values[colName] = $"source.[{colName}]";
                    }
                }
                else
                {
                    var idColName = $"{colName}ID";
                    if (values.ContainsKey(idColName))
                    {
                        values[idColName] = $"{GetSelectSubQuery(prop, colName)}";
                    }
                }
            }
            foreach (var sqlExpression in values)
            {
                if (string.IsNullOrEmpty(sqlExpression.Value))
                {
                    continue;
                }
                if (setSpecification.Length > 0)
                {
                    setSpecification.AppendLine(",");
                }
                setSpecification.Append(sqlExpression.Value);
            }

            return setSpecification.ToString();
        }

        protected bool IsPrimitiveType(Type propertyType)
        {
            return propertyType.IsValueType || propertyType.IsPrimitive || propertyType == typeof(string) ||
                   (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ||
                    propertyType == typeof(Guid));
        }

        /// <summary>
        /// Возвращает текст выборки ИД справочника НСИ по пердаваемому значению.
        /// </summary>
        /// <param name="entityType">Тип сущности НСИ (потомок DictObject).</param>
        /// <param name="value">Значение, поиск по которому будет производится в выборке.</param>
        /// <returns></returns>
        protected string GetDictValueSelect(Type entityType, string value)
        {
            return
                   $"(SELECT TOP 1 t1.ID FROM {GetTableName(entityType)} AS t1 " +
                   $"INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID " +
                   $"WHERE Hidden = 0 AND (" +
                   $"(ISNULL(Code, N'') <> N'' AND Code = CAST({value} AS NVARCHAR(MAX)) )" +
                   $"OR (ISNULL(Name, N'') <> N'' AND Name = CAST({value} as NVARCHAR(MAX)) )" +
                   $"OR (ISNULL(ExternalID, N'') <> N'' AND ExternalID = CAST({value} as NVARCHAR(MAX)))))";
        }
        /// <summary>
        /// Возвращает словарь обязатульных колонок и знаечния по умолчанию для этой колонки заданного типа сущности.
        /// </summary>
        /// <param name="entityType">Тип сущности, таблица sql.</param>
        /// <returns></returns>
        protected Dictionary<string, string> GetRequiredColumnsAndValues(Type entityType)
        {
            return GetSqlColumnDefinition(entityType)
                .Where(f => f.ColumnName != "ID" && !f.IsNullable)
                .OrderBy(sort => sort.ColumnName)
                .ToDictionary(x => $"[{x.ColumnName}]", x =>
                {
                    return GetSqlDefaultValue(x);
                });
        }

        #endregion Protected Methods

        #region Private Methods

        private object GetDbReaderValue(object value)
        {
            if (value is DBNull)
            {
                return null;
            }

            return value;
        }

        #endregion Private Methods
    }
}