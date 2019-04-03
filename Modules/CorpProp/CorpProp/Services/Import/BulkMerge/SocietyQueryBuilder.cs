using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using CorpProp.Entities.Import;

namespace CorpProp.Services.Import.BulkMerge
{
    public class SocietyQueryBuilder : QueryBuilder
    {
        #region Public Constructors

        public SocietyQueryBuilder(Dictionary<string, string> colsNameMapping, Type type) : base(colsNameMapping, type)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override string BuildMergeQuery(ImportHistory history)
        {
            var mergeSqlScript = new StringBuilder();

            mergeSqlScript.AppendLine(
                $"declare @inserted table({GetColumnSpecification()})");
            //1ая часть мержа для сопоставления по коду и наименованию
            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY IDEUP) as cnt " +
                                      $"\r\nFROM {GetTempTableName()}) AS subquery " +
                                      $"\r\nWHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO [CorpProp.Subject].Society AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON target.IDEUP = source.IDEUP AND target.Hidden=0 AND target.IsHistory=0");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()}");
            mergeSqlScript.AppendLine($"WHEN NOT MATCHED BY target THEN ");
            mergeSqlScript.AppendLine($"INSERT({GetInsertColumnSpecification()})");
            mergeSqlScript.AppendLine($"VALUES({GetValuesSpecification()})");
            mergeSqlScript.AppendLine($"OUTPUT $Action, {GetOutputColumnSpecification()} INTO  @inserted;");
            mergeSqlScript.AppendLine($"SELECT COUNT(*) FROM @inserted");
            return mergeSqlScript.ToString();
        }

        #endregion Public Methods
    }
}