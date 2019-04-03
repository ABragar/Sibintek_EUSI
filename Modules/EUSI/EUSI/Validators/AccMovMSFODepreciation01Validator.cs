using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Validators;
using EUSI.Entities.Accounting;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EUSI.Validators
{
    public class AccMovMSFODepreciation01Validator : IExcelImportFileEntityValidator
    {
        private readonly Dictionary<string, string> _propNamesDictionary = new Dictionary<string, string>
            {
                { "propNameEUSINumber", nameof(AccountingMovingMSFO.EUSINumber) },               
                { "propNameSubPosition", nameof(AccountingMovingMSFO.SubPosition) },
                { "propNameDocDate", nameof(AccountingMovingMSFO.DocDate) },
                { "propNameDate", nameof(AccountingMovingMSFO.Date) },
                { "propNameCostDepreciation", nameof(AccountingMovingMSFO.CostDepreciation) },
                { "propNamePositionStorno", nameof(AccountingMovingMSFO.PositionStorno) },
                { "propNameAccountGKDebit", nameof(AccountingMovingMSFO.AccountGKDebit) },
                { "propNameAccountGKCredit", nameof(AccountingMovingMSFO.AccountGKCredit) },
                { "propNamePositionConsolidation", nameof(AccountingMovingMSFO.PositionCredit) },
                { "propNameExternalIDCredit", nameof(AccountingMovingMSFO.ExternalIDCredit) },
                { "propNameInventoryCredit", nameof(AccountingMovingMSFO.InventoryCredit) },
                { "propNameIXODepreciationCredit", nameof(AccountingMovingMSFO.IXODepreciationCredit) }
            };

        private readonly string _accountGKCreditValidValue = "02";

        public void Validate(DataTable dataTable, ref ImportHistory history)
        {
            Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(dataTable);
            DataTable cleanDataTable = ImportHelper.GetDataTableWithoutHeader(dataTable);
            string errorType = ImportExtention.GetErrorTypeName(ErrorType.System);
            int startRowIndex = ImportHelper.GetRowStartIndex(dataTable);
            DataTable shortDataTable = dataTable.AsEnumerable().Skip(startRowIndex).CopyToDataTable();

            ImportHelper.CheckColumnsExisting(_propNamesDictionary, colsNameMapping, errorType, ref history);

            if (history.ImportErrorLogs.Count > 0)
            {
                return;
            }

            Dictionary<string, int> columnNumbersDictionary =
                ImportHelper.GetDictionaryOfColumnNumbers(_propNamesDictionary, colsNameMapping, cleanDataTable, true);

            foreach (var columnNumberPair in columnNumbersDictionary)
            {
                string fieldName = _propNamesDictionary[columnNumberPair.Key];

                foreach (DataRow dataRow in shortDataTable.Rows)
                {
                    string value = dataRow[columnNumberPair.Value].ToString();
                    if (string.IsNullOrEmpty(value))
                    {
                        int errorRowNumber = shortDataTable.Rows.IndexOf(dataRow) + 1;                        
                        history.ImportErrorLogs.AddError(
                                     errorRowNumber
                                     , columnNumberPair.Value
                                     , fieldName
                                     , ErrorType.Required);
                    }
                    else if (fieldName == nameof(AccountingMovingMSFO.AccountGKCredit) && !value.StartsWith(_accountGKCreditValidValue))
                    {
                        int errorRowNumber = shortDataTable.Rows.IndexOf(dataRow) + 1;
                        history.ImportErrorLogs.AddError(
                                     errorRowNumber
                                     , columnNumberPair.Value
                                     , fieldName
                                     , "Счет ГК (учетной системы) (Кредит) не соответствует виду движения"
                                     , ErrorType.System);                       
                    }
                }
            }
        }
    }
}
