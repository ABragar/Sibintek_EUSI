using CorpProp.Entities.Subject;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CorpProp.Entities.Import;

namespace CorpProp.Services.Import.BulkMerge
{
    public class ScheduleStateRegistrationQueryBuilder : QueryBuilder
    {
        public ScheduleStateRegistrationQueryBuilder(Dictionary<string, string> colsNameMapping, Type type) : base(colsNameMapping, type)
        {
        }

        public override string BuildMergeQuery(ImportHistory history)
        {
            var mergeSqlScript = new StringBuilder();

            mergeSqlScript.AppendLine(
                $"declare @inserted table({GetColumnSpecification()})");
            //сопоставление по SocietyShareholderID и SocietyRecipientID
            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY Society, Year ) as cnt " +
                                      $"\r\nFROM {GetTempTableName()}) AS subquery " +
                                      $"\r\nWHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO dbo.{MainType.Name}SocietyView AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON target.Year = source.Year " +
                                      $"\r\nAND target.IDEUP=source.Society " +
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

        public override int Merge(SqlCommand command, ref ImportHistory history)
        {
            CreateView(command);
            var result = base.Merge(command, ref history);
            DropView(command);
            return result;
        }

        private void DropView(SqlCommand command)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                $"IF OBJECT_ID('dbo.{MainType.Name}SocietyView','V') IS NOT NULL DROP VIEW dbo.{MainType.Name}SocietyView");
            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        private void CreateView(SqlCommand command)
        {
            DropView(command);

            var sb = new StringBuilder();
            var societytype = typeof(Society);
            sb.AppendLine($"CREATE VIEW dbo.{MainType.Name}SocietyView AS(SELECT l.*, r.IDEUP from {GetTableName(MainType)} l inner join {GetTableName(societytype)} r on l.SocietyID = r.ID)");

            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }
    }
}