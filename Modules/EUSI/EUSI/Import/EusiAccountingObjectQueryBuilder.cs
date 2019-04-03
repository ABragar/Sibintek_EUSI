using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using CorpProp.Entities.Import;
using CorpProp.Services.Accounting;
using CorpProp.Services.Import.BulkMerge;

namespace EUSI.Import.BulkMerge
{
    public class EusiAccountingObjectQueryBuilder : QueryBuilder
    {
        private OBUVersionControl _version;

        public EusiAccountingObjectQueryBuilder(Dictionary<string, string> colsNameMapping, Type type, OBUVersionControl version) : base(colsNameMapping, type)
        {
            _version = version;
        }

        public override string BuildMergeQuery()
        {
            var mergeSqlScript = new StringBuilder();

            mergeSqlScript.AppendLine(
                $"IF OBJECT_ID(N'tempdb..#AccountingObjectInsertedID') IS NOT NULL DROP TABLE #AccountingObjectInsertedID CREATE TABLE #AccountingObjectInsertedID(ID int)");
            mergeSqlScript.AppendLine(
                $"IF OBJECT_ID(N'tempdb..#AccountingObjectUpdatedID') IS NOT NULL DROP TABLE #AccountingObjectUpdatedID CREATE TABLE #AccountingObjectUpdatedID(ID int)");
            //ActualDate = @StartPeriod - update record
            mergeSqlScript.AppendLine(
                $"declare @inserted table({GetColumnSpecification()})");
            mergeSqlScript.AppendLine($"declare @StartPeriod dateTime = {GetStartPeriod()}");
            mergeSqlScript.AppendLine($"declare @EndPeriod dateTime = {GetEndPeriod()}");

            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY InventoryNumber) as cnt FROM {GetTempTableName()}) AS subquery WHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO dbo.AccountingObjectExtView AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON target.Hidden=0 " +
                                      $"AND target.IsHistory=0 " +
                                      $"AND target.ConsolidationCode = source.Consolidation " +
                                      $"AND target.Number = source.EUSINumber " +
                                      $"AND ISNULL(target.InventoryNumber,'') = COALESCE(source.InventoryNumber, ISNULL(target.InventoryNumber, '')) " +
                                      $"AND target.ActualDate = @StartPeriod ");

            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()};");

            //ActualDate < @StartPeriod - CreateNewVersion
            mergeSqlScript.AppendLine($"{GetInsertColumnSpecification()}");
            mergeSqlScript.AppendLine($"SELECT ID FROM  dbo.AccountingObjectExtView target" +
                                      $" INNER JOIN {GetTempTableName()} " +
                                      $" ON target.Hidden=0" +
                                      $" AND target.IsHistory=0 " +
                                      $" AND target.ConsolidationCode = source.Consolidation " +
                                      $" AND target.Number = source.EUSINumber " +
                                      $" AND ISNULL(target.InventoryNumber,'') = COALESCE(source.InventoryNumber, ISNULL(target.InventoryNumber, '')) " +
                                      $" AND target.ActualDate < @StartPeriod ");

            //ActualDate > @StartPeriod - CreateOLDVersion
            mergeSqlScript.AppendLine($"SELECT ID FROM  dbo.AccountingObjectExtView target" +
                                      $" INNER JOIN {GetTempTableName()} " +
                                      $" ON target.Hidden=0" +
                                      $" AND target.IsHistory=0 " +
                                      $" AND target.ConsolidationCode = source.Consolidation " +
                                      $" AND target.Number = source.EUSINumber " +
                                      $" AND ISNULL(target.InventoryNumber,'') = COALESCE(source.InventoryNumber, ISNULL(target.InventoryNumber, '')) " +
                                      $" AND target.ActualDate > @StartPeriod ");



            mergeSqlScript.AppendLine($";IF OBJECT_ID(N'tempdb..#ModifiedRecords') IS NOT NULL " +
                                        $"\r\nDROP TABLE #ModifiedRecords CREATE TABLE #ModifiedRecords(ID int)");

            mergeSqlScript.AppendLine($"INSERT INTO #ModifiedRecords(ID) " +
                                        "\r\nSELECT ID FROM " +
                                        "\r\n(Select ID FROM #AccountingObjectInsertedID " +
                                        "\r\nunion all " +
                                        "\r\nSelect ID FROM #AccountingObjectUpdatedID)t");

            //Историчность

            mergeSqlScript.AppendLine($"SELECT count(*) as 'RelationValidationResult'" +
                                      $"\r\nFROM[CorpProp.Accounting].AccountingObject a " +
                                      $"\r\nINNER JOIN #ModifiedRecords t" +
                                      $"\r\nON a.ID = t.ID " +
                                      $"\r\nINNER JOIN [CorpProp.Law].[Right] r " +
                                      $"\r\nON r.EstateID = a.EstateID " +
                                      $"\r\nAND r.RegNumber = a.RegNumber ");

            mergeSqlScript.AppendLine($"SELECT count(*) as 'TotalRecords' FROM #ModifiedRecords");

            mergeSqlScript.AppendLine($"DROP table #AccountingObjectInsertedID");
            mergeSqlScript.AppendLine($"DROP table #AccountingObjectUpdatedID");
            mergeSqlScript.AppendLine($"DROP table #ModifiedRecords");

            return mergeSqlScript.ToString();
        }

        private string GetStartPeriod()
        {
            return $"'{_version.StartPeriod.ToString("yyyy-MM-dd")}'";
        }

        private string GetEndPeriod()
        {
            return $"'{_version.StartPeriod.ToString("yyyy-MM-dd")}'";
        }

        private string GetTargetValuesSpecification()
        {
            var val = new StringBuilder();
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate" || x.ColumnName.ToLower() == "actualdate")
                {
                    return $"GetDate()";
                }
                return x.IsNullable ? $"target.[{x.ColumnName}]" : $"ISNULL(target.[{x.ColumnName}],{GetSqlDefaultValue(x)})";
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
                        values[colName] = $"target.[{colName}]";
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

        protected override string GetCreateTableScript()
        {
            var tableName = GetTempTableName();
            var properties = MainType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite).ToArray();
            var createScript = new StringBuilder();
            createScript.AppendLine($"IF OBJECT_ID(N'tempdb..{tableName}') IS NOT NULL DROP TABLE {tableName} \r\n CREATE TABLE {tableName} ({GetTempTableColumnsSpecification(properties)} ,EUSINumber int )");
            return createScript.ToString();
        }

        /// <summary>
        /// Перечень выражений для UPDATE запроса (SET)
        /// </summary>
        /// <returns></returns>
        protected override string GetSetSpecification()
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

                var idColName = "";
                if (IsPrimitiveType(prop.PropertyType))
                {
                    setSpecification.AppendLine($" target.[{colName}] = source.[{colName}]");
                }
                else
                {
                    if (colName == "Status")
                    {
                        idColName = "AccountingStatusID";
                    }
                    else if (colName == "Model")
                    {
                        idColName = "VehicleModelID";
                    }
                    else
                    {
                        idColName = $"{colName}ID";
                    }

                    setSpecification.AppendLine($" target.[{idColName}] = {GetSelectSubQuery(prop, colName)}");
                }
            }
            setSpecification.AppendLine($",target.ImportUpdateDate = getDate()");
            setSpecification.AppendLine($",target.ActualDate = {GetStartPeriod()}");
            setSpecification.AppendLine($",target.NonActualDate = {GetEndPeriod()}");
            return setSpecification.ToString();
        }

        public override int Merge(SqlCommand command, ref ImportHistory history)
        {
            CreateView(command);
            command.CommandText = BuildMergeQuery();
            var reader = command.ExecuteReader();

            int relationCheckOk = 0;
            int totalRecords = 0;
            int id = 0;

            do
            {
                reader.Read();
                var resultName = reader.GetName(0);
                switch (resultName)
                {
                    case "RelationValidationResult":
                        relationCheckOk = reader.HasRows ? Convert.ToInt32(reader[0]) : 0;
                        break;

                    case "TotalRecords":
                        totalRecords = reader.HasRows ? Convert.ToInt32(reader[0]) : 0;
                        break;

                    case "ID":
                        id = reader.HasRows ? Convert.ToInt32(reader[0]) : 0;
                        break;
                }
            } while (reader.NextResult());

            DropView(command);
            //Валидация
            ProcessValidationResult(ref history, totalRecords, relationCheckOk);
            return totalRecords;
        }

        private void CreateView(SqlCommand command)
        {
            var sb = new StringBuilder();
            DropView(command);

            sb.AppendLine($"CREATE VIEW dbo.AccountingObjectExtView AS(" +
                          $"SELECT a.*,e.Number, d.Code AS ConsolidationCode " +
                          "FROM[CorpProp.Accounting].AccountingObject a " +
                          "INNER JOIN[CorpProp.Estate].Estate e on a.EstateID = e.ID " +
                          "INNER JOIN[CorpProp.NSI].Consolidation c on a.ConsolidationID = c.ID " +
                          $"INNER JOIN[CorpProp.Base].DictObject d  on  d.ID = c.ID)");

            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        private void DropView(SqlCommand command)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                $"IF OBJECT_ID('dbo.AccountingObjectExtView','V') IS NOT NULL DROP VIEW dbo.AccountingObjectExtView");
            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        private void ProcessValidationResult(ref ImportHistory history, int processedRecords, int relationCheckOk)
        {
            var relationCheckFail = processedRecords - relationCheckOk;
            var relationCheckText = $"Проверка по составу связей ОБУ и ОП (по полю \"Номер записи гос регистрации\"): для {relationCheckOk} ОБУ связь с объектом права найдена, для {relationCheckFail} ОБУ связь с ОП не найдена. ";
            history.ResultText += relationCheckText;
        }
    }
}