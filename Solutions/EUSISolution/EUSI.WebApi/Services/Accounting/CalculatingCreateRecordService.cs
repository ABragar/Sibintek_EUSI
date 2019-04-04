using Base.DAL;
using CorpProp.Entities.Accounting;
using EUSI.Entities.Accounting;
using EUSI.Entities.NSI;
using EUSI.Entities.Report;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace EUSI.WebApi.Services.Accounting
{
    class CalculatingCreateRecordService
    {
        public const string ImportErrorText = "Завершено с ошибками.";

        public const string CalcSuccessText = "Расчет завершен.";

        private const string ConnectionStringName = "DataContext";

        private const string StoredProcedureName_Estate = "pReport_Create_AccountingCalculated_Estate";
        private const string StoredProcedureName_Land = "pReport_Create_AccountingCalculated_Land";
        private const string StoredProcedureName_Vehicle = "pReport_Create_AccountingCalculated_Vehicle";


        private const string pConsolidationID = "@vintConsolidationUnitID";
        private const string pYear = "@year";
        private const string pTaxRateType = "@TaxRateType";
        private const string pvintReportPeriod = "@vintReportPeriod";
        private const string pInitiator = "@initiator";

        public void CreateCalculateRecord(int year, int consolidationId, string taxRateTypeCode, int periodCalculatedNU, int securityUserID, IUnitOfWork uofw)
        {
            SqlParameter sConsolidation = new SqlParameter
            {
                ParameterName = pConsolidationID,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = consolidationId
            };

            SqlParameter sYear = new SqlParameter
            {
                ParameterName = pYear,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = year
            };

            SqlParameter sTaxRateType = new SqlParameter
            {
                ParameterName = pTaxRateType,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = taxRateTypeCode
            };

            SqlParameter sReportPeriod = new SqlParameter
            {
                ParameterName = pvintReportPeriod,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = periodCalculatedNU
            };

            SqlParameter sInitiator = new SqlParameter
            {
                ParameterName = pInitiator,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input,
                Value = securityUserID
            };

            SqlParameter result = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.Int
            };
            result.Direction = ParameterDirection.Output;

            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

            using (var conn = new SqlConnection(connectionString))
            {
                string StoredProcedureName = "";

                switch (taxRateTypeCode)
                {
                    case ("102"):
                        StoredProcedureName = StoredProcedureName_Land;
                        break;
                    case ("101"):
                        StoredProcedureName = StoredProcedureName_Estate;
                        break;
                    case ("103"):
                        StoredProcedureName = StoredProcedureName_Vehicle;
                        break;
                }
                if (!string.IsNullOrEmpty(StoredProcedureName))
                {
                    var command = new SqlCommand(StoredProcedureName, conn);
                    command.CommandTimeout = 60000;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(sConsolidation);
                    command.Parameters.Add(sYear);
                    command.Parameters.Add(sTaxRateType);
                    command.Parameters.Add(sReportPeriod);
                    command.Parameters.Add(sInitiator);
                    command.Parameters.Add(result);
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                    int spResult = int.Parse(command.Parameters["@result"].Value.ToString());
                    if (spResult != 0)
                    {
                        throw new Exception("Ошибка обработки данных");
                    }
                }
            }
        }
    }
}