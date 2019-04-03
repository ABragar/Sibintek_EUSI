using Base.DAL;
using CorpProp.Entities.Accounting;
using EUSI.Entities.Accounting;
using EUSI.Entities.Report;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EUSI.Services.Monitor
{
    public class MonitorReportingService
    {
        private const string ConnectionStringName = "DataContext";

        private const string StoredProcedureName = "pGetMaxIterationIndex";

        private const string DateStartParameterName = "@periodstart";

        private const string DateEndParameterName = "@periodend";
        
        private const string ConsolidationParameterName = "@consolidation";

        private const string EventCodeParameterName = "@eventcode";

        private const string IndexParameterName = "@index";

        private const int StringParametersSize = 40;

        private const int StartIterationIndexValue = 1;

        public void CreateMonitorReporting(ReportMonitoring reportMonitoring, IUnitOfWork uofw)
        {
            if (reportMonitoring?.Consolidation == null || !reportMonitoring.StartDate.HasValue || !reportMonitoring.EndDate.HasValue)
            {
                return;
            }

            DateTime periodStartDate = reportMonitoring.StartDate.Value;
            DateTime periodEndDate = reportMonitoring.EndDate.Value;   

            SqlParameter periodStart = new SqlParameter
            {
                ParameterName = DateStartParameterName,
                SqlDbType = SqlDbType.NVarChar,
                Size = StringParametersSize,
                Direction = ParameterDirection.Input,
                Value = $"{periodStartDate.Year}.{periodStartDate.Month}.{periodStartDate.Day}"
            };

            SqlParameter periodEnd = new SqlParameter
            {
                ParameterName = DateEndParameterName,
                SqlDbType = SqlDbType.NVarChar,
                Size = StringParametersSize,
                Direction = ParameterDirection.Input,
                Value = $"{periodEndDate.Year}.{periodEndDate.Month}.{periodEndDate.Day}"
            };

            SqlParameter consolidation = new SqlParameter
            {
                ParameterName = ConsolidationParameterName,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = reportMonitoring.Consolidation.ID
            };
            
            SqlParameter eventCode = new SqlParameter
            {
                ParameterName = EventCodeParameterName,
                SqlDbType = SqlDbType.NVarChar,
                Size = StringParametersSize,
                Direction = ParameterDirection.Input,
                Value = (reportMonitoring.ReportMonitoringEventType?.Code) ?? ""
            };
                       

            SqlParameter iterationIndex = new SqlParameter
            {
                ParameterName = IndexParameterName,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(StoredProcedureName, conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(periodStart);
                command.Parameters.Add(periodEnd);
                command.Parameters.Add(consolidation);
                command.Parameters.Add(eventCode);
                command.Parameters.Add(iterationIndex);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();

                if (int.TryParse(command.Parameters[IndexParameterName].Value.ToString(), out int index))
                {
                    reportMonitoring.IterationIndex = index + 1;
                }
                else
                {
                    reportMonitoring.IterationIndex = StartIterationIndexValue;
                }
            }

            uofw.GetRepository<ReportMonitoring>().Update(reportMonitoring);
        }
    }
}