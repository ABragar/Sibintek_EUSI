using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Base;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Services.Import.BulkMerge;
using EUSI.Entities.Accounting;

namespace EUSI.Services.Accounting
{
    /// <summary>
    /// Построитель запроса для функции экспорта данных в ИР-Аренда.
    /// </summary>
    public class RentalOSExportQueryBuilder : QueryBuilder
    {
        private string connectionString => ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
        private DateTime _date;
        private System.Guid _historyOid;

        //при экспорте кастомный селект только по следующим колонкам
        private Dictionary<string, string> updateColumns = new Dictionary<string, string> {
            {nameof(RentalOS.NameByDoc), nameof(CorpProp.Entities.Estate.Estate)}
            , {nameof(RentalOS.Deposit), nameof(CorpProp.Entities.Base.DictObject)}
            , {nameof(RentalOS.LandPurpose), nameof(CorpProp.Entities.Base.DictObject)}
            , {nameof(RentalOS.DepreciationGroup), nameof(CorpProp.Entities.Base.DictObject)}
            , {nameof(RentalOS.CadastralNumber), nameof(CorpProp.Entities.Estate.Cadastral)}
            , {nameof(RentalOS.CadastralValue), nameof(CorpProp.Entities.Estate.Cadastral)}
            , {nameof(RentalOS.Comments), "Rent"}
            , {nameof(RentalOS.EUSINumber), ""}
        };

        public RentalOSExportQueryBuilder(Dictionary<string, string> colsNameMapping, Type type, DateTime? period, System.Guid historyOid) : base(colsNameMapping, type)
        {
            if (period.HasValue)
                _date = period.Value;
            else
                period = DateTime.Now;

            _date = new DateTime(_date.Year, _date.Month, 1);
            _historyOid = historyOid;
        }

        public string BuildSelectQuery(DataTable table)
        {
            //кастомный селект
            var colsSelect = new StringBuilder();
            foreach (DataColumn column in table.Columns)
            {
                if (colsSelect.Length > 0)
                    colsSelect.Append(",");

                if (updateColumns.Keys.Contains(column.ColumnName))
                {
                    if (column.ColumnName == nameof(RentalOS.EUSINumber))
                    {
                        colsSelect.AppendLine($"{column.ColumnName} =  Estate.[Number]");
                    }
                    else if (updateColumns[column.ColumnName] == nameof(CorpProp.Entities.Base.DictObject))
                    {
                        colsSelect.AppendLine($"{column.ColumnName} = {column.ColumnName}.Name");
                    }
                    else
                        colsSelect.AppendLine($"{column.ColumnName} = {updateColumns[column.ColumnName]}.{column.ColumnName}");
                }
                else
                    colsSelect.AppendLine($"{column.ColumnName} = importData.{column.ColumnName}");
            }

            return $@"SELECT {colsSelect}
                      FROM {GetTempTableName()} as importData
                      LEFT JOIN
					  (   SELECT rentBE.*, BE = be.[Code]
						  FROM {GetTableName(typeof(EUSI.Entities.Accounting.RentalOS))} AS rentBE
						  LEFT JOIN {GetTableName(typeof(CorpProp.Entities.Base.DictObject))}  AS be on rentBE.{nameof(RentalOS.ConsolidationID)} = be.ID
						  WHERE rentBE.[Hidden] = 0
                            AND rentBE.ID IN (SELECT [Entity_ID]
                                              FROM [CorpProp.Import].[ImportObject]
                                              WHERE [ImportHistoryOid] = '{HistoryOid()}' AND [Entity_TypeName] = N'{TypeName()}')
					  ) AS Rent ON importData.[InventoryNumber] = Rent.[InventoryNumber]  AND ISNULL(importData.[EUSINumber], -1) = ISNULL(Rent.[EUSINumber], -1) AND importData.[Consolidation] = Rent.[BE]
                      LEFT JOIN [CorpProp.Accounting].[AccountingObjectTbl] AS os ON Rent.AccountingObjectID = os.ID
                      LEFT JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS Estate ON os.EstateID = Estate.ID
                      LEFT JOIN {GetTableName(typeof(CorpProp.Entities.Estate.InventoryObject))} AS InventoryObject ON os.EstateID = InventoryObject.ID
                      LEFT JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Cadastral))} AS Cadastral ON os.EstateID = Cadastral.ID
                      LEFT JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Land))} AS Land ON os.EstateID = Land.ID
                      LEFT JOIN {GetTableName(typeof(CorpProp.Entities.Base.DictObject))} AS Deposit ON InventoryObject.DepositID = Deposit.ID
                      LEFT JOIN {GetTableName(typeof(CorpProp.Entities.Base.DictObject))} AS DepreciationGroup ON InventoryObject.DepreciationGroupID = DepreciationGroup.ID
                      LEFT JOIN {GetTableName(typeof(CorpProp.Entities.Base.DictObject))} AS LandPurpose ON Land.LandPurposeID = LandPurpose.ID
                    ";
        }

        private string PeriodDate()
        {
            return _date.ToString("yyy-MM-dd");
        }

        private string HistoryOid()
        {
            return _historyOid.ToString();
        }

        private string TypeName()
        {
            return typeof(RentalOS).GetTypeName();
        }

        public override string BuildMergeQuery(ImportHistory history)
        {
            throw new NotImplementedException();
        }

        private SqlDataReader Select(SqlCommand command, DataTable table)
        {
            command.CommandText = BuildSelectQuery(table);
            return command.ExecuteReader();
        }

        public DataTable SelectResult(DataTable table)
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn))
                {
                    command.CommandTimeout = 600000;

                    var queryBuilder = this;
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
                            string columnName = table.Rows[fieldRow][k].ToString();
                            if (typeof(RentalOS).GetProperty(columnName) == null)
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

                    var rd = queryBuilder.Select(command, table);
                    dt.Load(rd);
                    return dt;
                }
            }
        }
    }
}