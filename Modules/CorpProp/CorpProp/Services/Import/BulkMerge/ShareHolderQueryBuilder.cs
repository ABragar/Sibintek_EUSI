using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using CorpProp.Entities.Import;

namespace CorpProp.Services.Import.BulkMerge
{
    public class ShareHolderQueryBuilder : QueryBuilder
    {
        #region Public Constructors

        public ShareHolderQueryBuilder(Dictionary<string, string> colsNameMapping, Type type) : base(colsNameMapping, type)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override string BuildMergeQuery(ImportHistory history)
        {
            var mergeSqlScript = new StringBuilder();

            mergeSqlScript.AppendLine(
                $"declare @inserted table({GetColumnSpecification()})");
            //сопоставление по SocietyShareholderID и SocietyRecipientID
            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY SocietyShareholderID,SocietyRecipientID ) as cnt " +
                                      $"\r\nFROM {GetTempTableName()}) AS subquery " +
                                      $"\r\nWHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO {GetTableName(MainType)} AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON target.SocietyShareholderID = source.SocietyShareholder " +
                                      $"\r\nAND target.SocietyRecipientID=source.SocietyRecipient " +
                                      $"\r\nAND target.Hidden=0 AND target.IsHistory=0");
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