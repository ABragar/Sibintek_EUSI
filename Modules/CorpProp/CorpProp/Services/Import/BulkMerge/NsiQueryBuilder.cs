using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Base.Utils.Common.Maybe;
using CorpProp.Entities.Base;
using CorpProp.Entities.Import;

namespace CorpProp.Services.Import.BulkMerge
{
    public class NsiQueryBuilder : QueryBuilder
    {
        #region Public Constructors

        public NsiQueryBuilder(Dictionary<string, string> colsNameMapping, Type type) : base(colsNameMapping, type)
        {
        }

        #endregion Public Constructors

        #region Protected Properties

        protected override IEnumerable<string> ObjectProperties => typeof(DictObject).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.CanWrite).Select(x => x.Name);

        #endregion Protected Properties

        #region Public Methods

        /// <summary>
        /// Возвращает результирующий запрос для выполнения синхронизации
        /// </summary>
        /// <returns></returns>
        public override string BuildMergeQuery(ImportHistory history)
        {
            SqlColumnDefinitions = GetSqlColumnDefinition(typeof(DictObject));
            var mergeSqlScript = new StringBuilder();
            var linkedUpdatePrefix = "upd";
            var updateStateStatus = $"\r\nUPDATE SET " +
                                      $"\r\nDictObjectStateID = (" +
                                      $"\r\nSELECT do.ID " +
                                      $"\r\nFROM [CorpProp.Base].[DictObject] AS do " +
                                      $"\r\nINNER JOIN  [CorpProp.NSI].DictObjectState as state " +
                                      $"\r\non state.ID = do.ID" +
                                      $"\r\nWHERE do.Code = 'Temporary')" +
                                      $"\r\n, DictObjectStatusID = (" +
                                      $"\r\nSELECT do.ID " +
                                      $"\r\nFROM [CorpProp.Base].[DictObject] AS do " +
                                      $"\r\nINNER JOIN  [CorpProp.NSI].DictObjectStatus as status " +
                                      $"\r\non status.ID = do.ID" +
                                      $"\r\nWHERE do.code = 'DelRequest')";
            //Начало транзакции
            mergeSqlScript.AppendLine($"BEGIN TRANSACTION BEGIN TRY");

            mergeSqlScript.AppendLine($"declare @result int = 0");
            mergeSqlScript.AppendLine($"declare @firstImport int = 0");
            mergeSqlScript.AppendLine($"SELECT @firstImport = CASE WHEN (SELECT COUNT(*) FROM dbo.DataDictionary{MainType.Name}) > 0 THEN 0 ELSE 1 END");
            mergeSqlScript.AppendLine($"declare @isCentral int = 0");
            mergeSqlScript.AppendLine($"SELECT @isCentral = CASE WHEN (select count(nsi.ID) FROM [CorpProp.NSI].[NSI] as nsi");
            mergeSqlScript.AppendLine($"left join [CorpProp.Base].DictObject as d on nsi.NSITypeID = d.ID");
            mergeSqlScript.AppendLine($"inner join [CorpProp.NSI].[NSIType] as nt on d.ID = nt.ID");
            mergeSqlScript.AppendLine($"where d.Code = 'Central' and nsi.Mnemonic in ('{MainType.Name}', '{MainType.Name}Menu')) > 0 THEN 1 ELSE 0 END");

            mergeSqlScript.AppendLine($"");
            mergeSqlScript.AppendLine($" declare @dState int");
            mergeSqlScript.AppendLine($" " +
                                      $"SELECT @dState = do.ID " +
                                      $"\r\nFROM [CorpProp.Base].[DictObject] AS do " +
                                      $"\r\nINNER JOIN  [CorpProp.NSI].DictObjectState as state " +
                                      $"\r\nON state.ID = do.ID " +
                                      $"\r\nWHERE do.Code = " +
                                      $"\r\nCASE WHEN @isCentral = 1 or (@isCentral = 0 and @firstImport = 1) then 'NotOld' ELSE 'Temporary' END ");
            mergeSqlScript.AppendLine($"");
            mergeSqlScript.AppendLine($" declare @dStatus int");
            mergeSqlScript.AppendLine($" " +
                                      $"SELECT @dStatus = do.ID " +
                                      $"\r\nFROM [CorpProp.Base].[DictObject] AS do" +
                                      $"\r\nINNER JOIN  [CorpProp.NSI].DictObjectStatus as status " +
                                      $"\r\nON status.ID = do.ID " +
                                      $"\r\nWHERE do.Code = " +
                                      $"\r\nCASE WHEN @isCentral = 1 or (@isCentral = 0 and @firstImport = 1) then 'AddConfirm' ELSE 'AddRequest' END ");




            mergeSqlScript.AppendLine(
                $"declare @inserted table({GetColumnSpecification()})");
            // 1ая часть мержа для сопоставления по коду и наименованию
            mergeSqlScript.AppendLine($"if(@isCentral=0) BEGIN ;With NoDuplicates as " +
                                      $"\r\n(SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY Code, Name) as cnt " +
                                      $"\r\nFROM {GetTempTableName()}) AS subquery " +
                                      $"\r\nWHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO dbo.DataDictionary{MainType.Name} AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON {GetNameCodeJoinCondition("target", "source")}");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()}");
            mergeSqlScript.AppendLine($"WHEN NOT MATCHED BY target THEN ");
            mergeSqlScript.AppendLine($"INSERT({GetInsertColumnSpecification()})");
            mergeSqlScript.AppendLine($"VALUES({GetValuesSpecification()})");
            //8968
            mergeSqlScript.AppendLine($"WHEN NOT MATCHED BY source THEN ");
            mergeSqlScript.AppendLine(updateStateStatus);

            mergeSqlScript.AppendLine($"OUTPUT $Action, {GetOutputColumnSpecification()} INTO  @inserted;");

            foreach (var linkedTable in GetLinkedTables())
            {
                var linkedTableColumns = GetSqlColumnDefinition(linkedTable.BaseType);
                mergeSqlScript.AppendLine(
                    $"INSERT INTO {GetTableName(linkedTable.BaseType)}({GetImportedColumnsAsString(linkedTableColumns)}) " +
                    $"\r\nSELECT i.[ID] {GetChildTableColumnSpecification(linkedTableColumns, "i", "source")} " +
                    $"\r\nFROM @inserted AS i");
                if (linkedTable.Properties.Count() >= 1)
                {
                    mergeSqlScript.AppendLine(
                        $"INNER JOIN {GetTempTableName()} AS source ON {GetNameCodeJoinCondition("i", "source")}");
                }
                mergeSqlScript.AppendLine(" WHERE action = 'INSERT'");

                var updateStr = GetChildTableUpdateColumnSpecification(linkedTable, linkedUpdatePrefix, "i", "source");

                if (!string.IsNullOrEmpty(updateStr))
                {
                    mergeSqlScript.AppendLine($"UPDATE {linkedUpdatePrefix} SET {updateStr} " +
                                              $"\r\nFROM @inserted AS i " +
                                              $"\r\nINNER JOIN {GetTableName(linkedTable.BaseType)} as {linkedUpdatePrefix} ON i.ID={linkedUpdatePrefix}.ID" +
                                              $"\r\nINNER JOIN {GetTempTableName()} AS source ON {GetNameCodeJoinCondition("i", "source")}" +
                                              $"\r\nWHERE action = 'UPDATE'");
                }
            }
            mergeSqlScript.AppendLine($"SET @result = @result+ (SELECT count(*) from @inserted)");
            mergeSqlScript.AppendLine($"DELETE FROM @inserted END");

            // 2ая часть мержа для сопоставления по ExternalID
            mergeSqlScript.AppendLine($"else BEGIN ;With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY ExternalID) as cnt " +
                                      $"\r\nFROM {GetTempTableName()}) AS subquery WHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO dbo.DataDictionary{MainType.Name} AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON {GetExternalIdJoinCondition("target", "source")}");
            mergeSqlScript.AppendLine($"WHEN MATCHED /*AND (isnull(target.Code,'') != isnull(source.Code,'') or isnull(target.Name,'') != isnull(source.Name,''))*/ THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()}");
            mergeSqlScript.AppendLine($"WHEN NOT MATCHED BY target THEN ");
            mergeSqlScript.AppendLine($"INSERT({GetInsertColumnSpecification()})");
            mergeSqlScript.AppendLine($"VALUES({GetValuesSpecification()})");
            //8968
            mergeSqlScript.AppendLine($"WHEN NOT MATCHED BY source THEN ");
            mergeSqlScript.AppendLine(updateStateStatus);

            mergeSqlScript.AppendLine($"OUTPUT $Action, {GetOutputColumnSpecification()} INTO  @inserted;");
            foreach (var linkedTable in GetLinkedTables())
            {
                var linkedTableColumns = GetSqlColumnDefinition(linkedTable.BaseType);

                mergeSqlScript.AppendLine(
                    $"INSERT INTO {GetTableName(linkedTable.BaseType)}({GetImportedColumnsAsString(linkedTableColumns)}) " +
                    $"\r\nSELECT i.[ID] {GetChildTableColumnSpecification(linkedTableColumns, "i", "source")} " +
                    $"\r\nFROM @inserted AS i");
                if (linkedTable.Properties.Count() >= 1)
                {
                    mergeSqlScript.AppendLine(
                        $"INNER JOIN {GetTempTableName()} AS source on ((i.ExternalID is not null and source.ExternalID is not null and i.ExternalID = source.ExternalID) or {GetNameCodeJoinCondition("i", "source")} ) ");
                }
                mergeSqlScript.AppendLine(" WHERE action = 'INSERT'");

                var updateStr = GetChildTableUpdateColumnSpecification(linkedTable, linkedUpdatePrefix, "i", "source");

                if (!string.IsNullOrEmpty(updateStr))
                {
                    mergeSqlScript.AppendLine($"UPDATE {linkedUpdatePrefix} SET {updateStr} " +
                                              $"\r\nFROM @inserted AS i " +
                                              $"\r\nINNER JOIN {GetTableName(linkedTable.BaseType)} as {linkedUpdatePrefix} ON i.ID={linkedUpdatePrefix}.ID" +
                                              $"\r\nINNER JOIN {GetTempTableName()} AS source on ((i.ExternalID is not null and source.ExternalID is not null and i.ExternalID = source.ExternalID) or {GetNameCodeJoinCondition("i", "source")} ) " +
                                              $"\r\nWHERE action = 'UPDATE'");
                }
                mergeSqlScript.AppendLine($" END");
            }

            //Окончание транзакции
            mergeSqlScript.AppendLine($"COMMIT END TRY");
            mergeSqlScript.AppendLine($"BEGIN CATCH");
            mergeSqlScript.AppendLine("declare @strErr nvarchar(max)='' ");
            mergeSqlScript.AppendLine("set @strErr = cast(ERROR_MESSAGE() as nvarchar(max)) ");
            mergeSqlScript.AppendLine($"SELECT");
            mergeSqlScript.AppendLine($" cast(ERROR_MESSAGE() as nvarchar(max)) as 'errText'");
            mergeSqlScript.AppendLine($", N'ERR_NsiQueryBulk' as 'errResultCode'");
            mergeSqlScript.AppendLine($", N'SQL_' + cast(ERROR_NUMBER() as nvarchar(max)) AS 'errCode' ;");
            mergeSqlScript.AppendLine($"ROLLBACK");
            mergeSqlScript.AppendLine("RAISERROR(@strErr, 10, 1)");
            mergeSqlScript.AppendLine($"END CATCH");


            mergeSqlScript.AppendLine($" SELECT count(*) as 'TotalRecords' from @inserted");
            return mergeSqlScript.ToString();
        }

        public override int Merge(SqlCommand command, ref ImportHistory history)
        {
            CreateDictionaryView(command);

            command.CommandText = BuildMergeQuery(history);
            var reader = command.ExecuteReader();

            int totalRecords = 0;

            do
            {
                reader.Read();
                var resultName = reader.GetName(0);
                switch (resultName)
                {
                    case "TotalRecords":
                        totalRecords = reader.HasRows ? Convert.ToInt32(reader[0]) : 0;
                        break;

                    case "errText":
                        {
                            if (reader.HasRows)
                            {
                                var errText = Convert.ToString(reader[0]);
                                var errResultCode = Convert.ToString(reader[1]);
                                var errCode = Convert.ToString(reader[2]);
                                var error = new ImportErrorLog()
                                {
                                    ImportHistory = history,
                                    RowNumber = null,
                                    ErrorText = errText,
                                    ErrorCode = errCode,
                                    ErrorType = Helpers.ErrorTypeName.System,
                                    InventoryNumber = "",
                                    EusiNumber = ""
                                };
                                history.ImportErrorLogs.Add(error);
                            }
                            break;
                        }
                }
            } while (reader.NextResult());
            reader.Close();

            DropDictionaryView(command);
            return totalRecords;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Описание Значений для Insert
        /// </summary>
        /// <returns></returns>
        protected override string GetValuesSpecification()
        {
            var val = new StringBuilder();
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate" || x.ColumnName.ToLower() == "actualdate")
                {
                    return $"GetDate()";
                }
                if (x.ColumnName.ToLower() == "dictobjectstateid")
                {
                    return $" @dState ";
                    /*$"(" +
                                      $"SELECT do.ID " +
                                      $"\r\nFROM [CorpProp.Base].[DictObject] AS do " +
                                      $"\r\nINNER JOIN  [CorpProp.NSI].DictObjectState as state " +
                                      $"\r\nON state.ID = do.ID " +
                                      $"\r\nWHERE do.Code = " +
                                      $"\r\nCASE WHEN @isCentral = 1 or (@isCentral = 0 and @firstImport = 1) then 'NotOld' ELSE 'Temporary' END)";*/
                }

                if (x.ColumnName.ToLower() == "dictobjectstatusid")
                {
                    return $" @dStatus ";
                    /*$"(" +
                                      $"SELECT do.ID " +
                                      $"\r\nFROM [CorpProp.Base].[DictObject] AS do" +
                                      $"\r\nINNER JOIN  [CorpProp.NSI].DictObjectStatus as status " +
                                      $"\r\nON status.ID = do.ID " +
                                      $"\r\nWHERE do.Code = " +
                                      $"\r\nCASE WHEN @isCentral = 1 or (@isCentral = 0 and @firstImport = 1) then 'AddConfirm' ELSE 'AddRequest' END)";*/
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

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Создание SQL View для DataDictionary c фильтрацией по конкретному типу словаря.
        /// Используется в конструкции merge в качестве целевого объекта.
        /// </summary>
        /// <param name="command"></param>
        private void CreateDictionaryView(SqlCommand command)
        {
            var sb = new StringBuilder();
            DropDictionaryView(command);

            sb.AppendLine($"CREATE VIEW dbo.DataDictionary{MainType.Name} AS(SELECT d.* from {GetTableName(DictObjectType)} d inner join {GetTableName(MainType)} o on d.ID = o.ID where isnull(d.[Hidden],0)=0)");

            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Удаление SQL View
        /// </summary>
        /// <param name="command"></param>
        private void DropDictionaryView(SqlCommand command)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                $"IF OBJECT_ID('dbo.DataDictionary{MainType.Name}','V') IS NOT NULL DROP VIEW dbo.DataDictionary{MainType.Name}");
            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="childTable"></param>
        /// <param name="lPrefix"></param>
        /// <param name="rPrefix"></param>
        /// <returns></returns>
        private string GetChildTableColumnSpecification(List<SqlColumnDefinition> columns, string lPrefix, string rPrefix)
        {
            var result = new StringBuilder();
            foreach (var column in columns.Where(x => x.ColumnName != "ID"))
            {
                if (column.ColumnName.EndsWith("ID"))
                {
                    var colName = column.ColumnName.Substring(0, column.ColumnName.Length - 2);
                    if (colName.EndsWith("_"))
                    {
                        colName = column.ColumnName.Substring(0, column.ColumnName.Length - 1);
                    }

                    if (ColsNameMapping.ContainsValue(colName))
                    {
                        result.Append($",{GetSelectSubQuery(MainType.GetProperty(colName), colName)}");
                    }
                    else
                    {
                        result.Append($",{rPrefix}.[{column.ColumnName}]");
                    }
                }
                else
                {
                    result.Append($",{rPrefix}.[{column.ColumnName}]");
                }
            }

            return result.ToString();
        }

        private string GetUpdateFieldSpecification(SqlColumnDefinition column, string cPrefix, string rPrefix)
        {
            string strUpdateTablePrefix = (string.IsNullOrEmpty(cPrefix)) ? "" : (cPrefix + ".");
            if (column.ColumnName.EndsWith("ID"))
            {
                var colName = column.ColumnName.Substring(0, column.ColumnName.Length - 2);
                if (colName.EndsWith("_"))
                {
                    colName = column.ColumnName.Substring(0, column.ColumnName.Length - 1);
                }

                //                if (ColsNameMapping.ContainsValue(colName))
                {
                    return $"{strUpdateTablePrefix + column.ColumnName} = {GetSelectSubQuery(MainType.GetProperty(colName), colName)}";
                }
            }
            else
            {
                return ($"{strUpdateTablePrefix}[{column.ColumnName}] = {rPrefix}.[{column.ColumnName}]");
            }
        }

        /// <summary>
        /// Возвращает перечнь столбцов для обновления из таблицы-потомка
        /// </summary>
        /// <param name="childTableProperties"></param>
        /// <param name="lPrefix"></param>
        /// <param name="rPrefix"></param>
        /// <returns></returns>
        private string GetChildTableUpdateColumnSpecification(SqlTableProperties childTableProperties, string cPrefix, string lPrefix, string rPrefix)
        {
            var res = new StringBuilder();
            var colDefinitions = GetSqlColumnDefinition(childTableProperties.BaseType);
            foreach (var columnDefinition in colDefinitions)
            {
                if (columnDefinition.ColumnName == "ID")
                {
                    continue;
                }
                if (res.Length > 0)
                {
                    res.Append(",");
                }

                res.Append(GetUpdateFieldSpecification(columnDefinition, cPrefix, rPrefix));
            }
            return res.ToString();
        }

        /// <summary>
        /// Условие ExternalID
        /// </summary>
        /// <param name="lPrefix"></param>
        /// <param name="rPrefix"></param>
        /// <returns></returns>
        private string GetExternalIdJoinCondition(string lPrefix, string rPrefix)
        {
            return $" {lPrefix}.ExternalID IS NOT NULL AND  {rPrefix}.ExternalID IS NOT NULL AND {lPrefix}.ExternalID = {rPrefix}.ExternalID";
        }

        private string GetImportedColumnsAsString(List<SqlColumnDefinition> columns)
        {
            return string.Join(",\r\n", columns.Select(x => $"[{x.ColumnName}]"));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SqlTableProperties> GetLinkedTables()
        {
            return PropertiesByTable.Where(x => x.BaseType.Name != DictObjectType.Name).Select(x => x);
        }

        /// <summary>
        /// Условие Name+Code
        /// </summary>
        /// <param name="lPrefix"></param>
        /// <param name="rPrefix"></param>
        /// <returns></returns>
        private string GetNameCodeJoinCondition(string lPrefix, string rPrefix)
        {
            return $"  (isnull({lPrefix}.Code, '') = isnull({rPrefix}.Code, '') and isnull({lPrefix}.Name, '') = isnull({rPrefix}.Name, '')) ";
        }

        #endregion Private Methods
    }
}