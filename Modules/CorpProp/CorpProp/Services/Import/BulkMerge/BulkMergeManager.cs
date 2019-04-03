using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Base;
using Base.DAL.EF;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.CorporateGovernance;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;

namespace CorpProp.Services.Import.BulkMerge
{
    public class BulkMergeManager
    {
        #region Private Fields

        private Dictionary<string, string> _colsNameMapping;
        private PropertyInfo[] _properties;
        private Type _type;
        private QueryBuilder _queryBuilder;

        #endregion Private Fields

        #region Public Constructors

        public BulkMergeManager(Dictionary<string, string> colsNameMapping, Type type, ref ImportHistory history)
        {
            _type = type;
            _colsNameMapping = colsNameMapping;
            _history = history;
        }

        public BulkMergeManager(Dictionary<string, string> colsNameMapping, Type type, ref ImportHistory history, QueryBuilder queryBuilder)
        {
            _type = type;
            _colsNameMapping = colsNameMapping;
            _history = history;
            _queryBuilder = queryBuilder;
        }

        #endregion Public Constructors

        #region Private Properties

        private string connectionString => ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

        #endregion Private Properties

        protected ImportHistory _history;

        #region Public Methods

        /// <summary>
        /// Импортирует строки из DataTable во временную таблицу на сервере
        /// </summary>
        /// <param name="table">Имя импортируемой таблицы</param>
        /// <param name="type">Тип импортируемой таблицы</param>
        /// <param name="count">Счетчик строк</param>
        public void BulkInsertAll(DataTable table, Type type, Dictionary<string, string> colsNameMapping, ref int count)
        {
            if (table.Rows.Count < 9)
            {
                throw new Exception("Справочник неверного формата");
            }

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn))
                {
                    command.CommandTimeout = 900000;

                    var queryBuilder = _queryBuilder ?? GetQueryBuilder();
                    queryBuilder.Init(command);

                    //создание временной таблицы
                    queryBuilder.CreateTempTable(command);

                    //Bulk insert into temp table
                    using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                    {
                        //именование колонок таблицы для корректного маппинга для SqlBulkCopy
                        int fieldRow = ImportHelper.FindFieldSystemNameRow(table);
                        int k = 0;
                        var unfindedColums = new List<string>();
                        foreach (DataColumn column in table.Columns)
                        {
                            string columnName = table.Rows[fieldRow][k].ToString().Trim();

                            if (!type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Any(x => x.Name.ToLower() == columnName.ToLower()))
                            {
                                unfindedColums.Add(column.ColumnName);
                                k++;
                                continue;
                            }
                            column.ColumnName = columnName;
                            bulkcopy.ColumnMappings.Add(columnName, columnName);
                            k++;
                        }
                        //удаление из DataTable хедера таблицы
                        var startRow = ImportHelper.GetRowStartIndex(table);
                        for (int i = 0; i < startRow; i++)
                        {
                            table.Rows[i].Delete();
                        }
                        unfindedColums.ForEach(x => table.Columns.Remove(x));

                        table.AcceptChanges();

                        bulkcopy.BulkCopyTimeout = 600000;
                        bulkcopy.DestinationTableName = queryBuilder.GetTempTableName();

                        bulkcopy.WriteToServer(table);
                        bulkcopy.Close();
                    }

                    count = table.Rows.Count;
                    queryBuilder.Merge(command, ref _history);
                }
            }
        }

        public void UpdateOlderDictObject(string tableName)
        {
            UpdateStatusDictObject("DelRequest", "Temporary", tableName);
        }

        public void UpdateStatusDictObject(string statusCode, string stateCode, string tableName)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sqlExpression = "sp_UpdateNsiRecordStatus";
                SqlCommand command = new SqlCommand(sqlExpression, conn);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter result = new SqlParameter
                {
                    ParameterName = "@result",
                    SqlDbType = SqlDbType.Int
                };
                result.Direction = ParameterDirection.Output;
                command.Parameters.Add(result);

                command.Parameters.Add("@State", SqlDbType.NVarChar, -1).Value = stateCode;
                command.Parameters.Add("@Status", SqlDbType.NVarChar, -1).Value = statusCode;
                command.Parameters.Add("@TableName", SqlDbType.NVarChar, -1).Value = tableName;

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

        public void UpdateAccountingObjectRaw(string tableName, SqlConnection sqlConnection)
        {
            UpdateEstateByEUSINumber(tableName, sqlConnection);
            UpdateEstatePropertyComplex(tableName, sqlConnection);
        }

        /// <summary>
        /// Связывание ОИ с ОБУ.
        /// </summary>
        /// <param name="tableName">Имя таблицы с сырыми данными ОБУ</param>
        /// <param name="sqlConnection">Сессия.</param>
        public void UpdateEstateByEUSINumber(string tableName, SqlConnection sqlConnection)
        {
            const string sqlExpression = "sp_UpdateEstateByEUSINumber";
            List<int> errorRows = new List<int>();

            SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
            SqlCommand createRowError = new SqlCommand("IF OBJECT_ID(N'tempdb..#TempErrors') IS NOT NULL DROP TABLE #TempErrors CREATE TABLE #TempErrors (rowNumber  INT)", sqlConnection);

            command.CommandType = CommandType.StoredProcedure;

            SqlParameter result = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.Int
            };
            result.Direction = ParameterDirection.Output;
            command.Parameters.Add(result);
            command.Parameters.Add("@TmpTableName", SqlDbType.NVarChar, -1).Value = tableName;

            createRowError.ExecuteNonQuery();
            command.ExecuteNonQuery();
            int spResult = int.Parse(command.Parameters["@result"].Value.ToString());

            if (spResult != 0)
            {
                throw new Exception("Ошибка обработки данных");
            }

            command = new SqlCommand("SELECT RowNumber FROM #TempErrors", sqlConnection);
            command.CommandType = CommandType.Text;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!errorRows.Contains(reader.GetInt32(0)))
                        errorRows.Add(reader.GetInt32(0));
                }
            }


            if (errorRows.Count > 0)
            {
                foreach (int rowNum in errorRows)
                {
                    _history.ImportErrorLogs.AddError(rowNum, 0, "", "Неверный номер ЕУСИ", ErrorType.System);
                }
            }
        }

        /// <summary>
        /// Создание ИК и обновление связанных с ОБУ ОИ.
        /// </summary>
        /// <param name="tableName">Имя таблицы с сырыми данными ОБУ</param>
        /// <param name="sqlConnection">Сессия.</param>
        public void UpdateEstatePropertyComplex(string tableName, SqlConnection sqlConnection)
        {
            const string sqlExpression = "sp_UpdateEstatePropertyComplex";

            SqlCommand command = new SqlCommand(sqlExpression, sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            //SqlCommand createRowError = new SqlCommand("IF OBJECT_ID(N'tempdb..#TempErrors') IS NOT NULL DROP TABLE #TempErrors CREATE TABLE #TempErrors (rowNumber  INT)", sqlConnection);

            SqlParameter result = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = SqlDbType.Int
            };
            result.Direction = ParameterDirection.Output;
            command.Parameters.Add(result);
            command.Parameters.Add("@TmpTableName", SqlDbType.NVarChar, -1).Value = tableName;

            command.ExecuteNonQuery();
            int spResult = int.Parse(command.Parameters["@result"].Value.ToString());

            if (spResult != 0)
            {
                throw new Exception("Ошибка обработки данных");
            }
        }


        #endregion Public Methods

        #region Private Methods

        private QueryBuilder GetQueryBuilder()
        {
            if (_type.IsSubclassOf(typeof(DictObject)))
            {
                return new NsiQueryBuilder(_colsNameMapping, _type);
            }
            else if (_type == typeof(Society))
            {
                return new SocietyQueryBuilder(_colsNameMapping, _type);
            }
            else if (_type == typeof(Shareholder))
            {
                return new ShareHolderQueryBuilder(_colsNameMapping, _type);
            }
            else if (_type == typeof(Entities.Subject.Subject))
            {
                return new SubjectQueryBuilder(_colsNameMapping, _type);
            }
            else if (_type == typeof(AccountingObject))
            {
                return new AccountingObjectQueryBuilder(_colsNameMapping, _type);
            }
            else if (_type == typeof(BankingDetail))
            {
                return new BankingDetailObjectQueryBuilder(_colsNameMapping, _type);
            }
            else if (_type == typeof(ScheduleStateRegistration))
            {
                return new ScheduleStateRegistrationQueryBuilder(_colsNameMapping, _type);
            }

            throw new NotImplementedException($"bulkInsert не реализован для типа {_type.Name}");
        }

        #endregion Private Methods
    }
}