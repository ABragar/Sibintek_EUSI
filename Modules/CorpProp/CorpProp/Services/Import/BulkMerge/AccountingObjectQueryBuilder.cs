using CorpProp.Entities.Base;
using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CorpProp.Entities.Import;

namespace CorpProp.Services.Import.BulkMerge
{
    public class AccountingObjectQueryBuilder : QueryBuilder
    {
        public AccountingObjectQueryBuilder(Dictionary<string, string> colsNameMapping, Type type) : base(colsNameMapping, type)
        {
        }

        public override string BuildMergeQuery(ImportHistory history)
        {
            var mergeSqlScript = new StringBuilder();

            mergeSqlScript.AppendLine(
                $"IF OBJECT_ID(N'tempdb..#AccountingObjectInsertedID') IS NOT NULL DROP TABLE #AccountingObjectInsertedID CREATE TABLE #AccountingObjectInsertedID(ID int)");
            mergeSqlScript.AppendLine(
                $"IF OBJECT_ID(N'tempdb..#AccountingObjectUpdatedID') IS NOT NULL DROP TABLE #AccountingObjectUpdatedID CREATE TABLE #AccountingObjectUpdatedID(ID int)");

            mergeSqlScript.AppendLine(
                $"declare @inserted table({GetColumnSpecification()})");
            //1ая часть мержа для сопоставления по коду и наименованию
            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY InventoryNumber) as cnt FROM {GetTempTableName()}) AS subquery WHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO [CorpProp.Accounting].AccountingObject AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON target.InventoryNumber = source.InventoryNumber AND target.Hidden=0 AND target.IsHistory=0");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()}");
            mergeSqlScript.AppendLine($"WHEN NOT MATCHED BY target THEN ");
            mergeSqlScript.AppendLine($"INSERT({GetInsertColumnSpecification()})");
            mergeSqlScript.AppendLine($"VALUES({GetValuesSpecification()})");

            mergeSqlScript.AppendLine($";IF OBJECT_ID(N'tempdb..#ModifiedRecords') IS NOT NULL " +
                                      $"\r\nDROP TABLE #ModifiedRecords CREATE TABLE #ModifiedRecords(ID int)");

            mergeSqlScript.AppendLine($"INSERT INTO #ModifiedRecords(ID) " +
                                      "\r\nSELECT ID FROM " +
                                      "\r\n(Select ID FROM #AccountingObjectInsertedID " +
                                      "\r\nunion all " +
                                      "\r\nSelect ID FROM #AccountingObjectUpdatedID)t");

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
            return setSpecification.ToString();
        }

        public override int Merge(SqlCommand command, ref ImportHistory history)
        {
            command.CommandText = BuildMergeQuery(history);
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

            //Валидация
            ProcessValidationResult(ref history, totalRecords, relationCheckOk);
            return totalRecords;
        }

        private void ProcessValidationResult(ref ImportHistory history, int processedRecords, int relationCheckOk)
        {
            var relationCheckFail = processedRecords - relationCheckOk;
            var relationCheckText = $"Проверка по составу связей ОБУ и ОП (по полю \"Номер записи гос регистрации\"): для {relationCheckOk} ОБУ связь с объектом права найдена, для {relationCheckFail} ОБУ связь с ОП не найдена. ";
            history.ResultText += relationCheckText;
        }
    }
}