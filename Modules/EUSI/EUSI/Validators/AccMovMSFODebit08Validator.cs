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
    public class AccMovMSFODebit08Validator : IExcelImportFileEntityValidator
    {
        private const string AccountGKDebitErrorText =
            "Поле \"Счет ГК (учетной системы) (Дебет)\" не соответствует виду движения.";

        private const string PositionDebitErrorText =
            "Поле \"Позиция консолидации (Дебет)\" не соответствует виду движения.";

        private const string SubPositionErrorText =
            "Отсутствует значение при заполненном поле \"Подпозиция консолидации\".";

        private const string SubPositionValueForBusinessAreaCredit = "240";

        private const string AccountGKAndPositionValidValue = "08";

        /// <summary>
        /// Список полей, всегда обязательных к заполнению.
        /// </summary>
        private readonly Dictionary<string, string> _propNamesDictionary = new Dictionary<string, string>
            {
                { "propNameConsolidation", nameof(AccountingMovingMSFO.Consolidation) },
                { "propNameSubPosition", nameof(AccountingMovingMSFO.SubPosition) },
                { "propNameDocDate", nameof(AccountingMovingMSFO.DocDate) },
                { "propNameDate", nameof(AccountingMovingMSFO.Date) },
                { "propNameCost", nameof(AccountingMovingMSFO.Cost) },
                { "propNamePositionStorno", nameof(AccountingMovingMSFO.PositionStorno) },
                { "propNameAccountGKDebit", nameof(AccountingMovingMSFO.AccountGKDebit) },
                { "propNameNameDebit", nameof(AccountingMovingMSFO.NameDebit) },
                { "propNamePositionDebit", nameof(AccountingMovingMSFO.PositionDebit) },
                { "propNameExternalIDDebit", nameof(AccountingMovingMSFO.ExternalIDDebit) },
                { "propNameDepositDebit", nameof(AccountingMovingMSFO.DepositDebit) },
                { "propNameIXOInitialDebit", nameof(AccountingMovingMSFO.IXOInitialDebit) },
                { "propNameAnalyticOneDebit", nameof(AccountingMovingMSFO.AnalyticOneDebit) },
                { "propNameAnalyticTwoDebit", nameof(AccountingMovingMSFO.AnalyticTwoDebit) },
                { "propNameBusinessUnitDebit", nameof(AccountingMovingMSFO.BusinessUnitDebit) },
                { "propNameGroupObjDebit", nameof(AccountingMovingMSFO.GroupObjDebit) },
                { "propNameAccountGKCredit", nameof(AccountingMovingMSFO.AccountGKCredit) },
                { "propNameBusinessUnitCredit", nameof(AccountingMovingMSFO.BusinessUnitCredit) }
            };

        /// <summary>
        /// Список полей, обязательных к заполнению при наличии определенного значения подпозиции консолидации.
        /// </summary>
        private readonly Dictionary<string, string> _subPositionRequiredPropNamesDictionary = new Dictionary<string, string>
            {
                { "propNameContragent", nameof(AccountingMovingMSFO.Contragent) },
                { "propNameExternalIDCredit", nameof(AccountingMovingMSFO.ExternalIDCredit) },
                { "propNameBusinessAreaCredit", nameof(AccountingMovingMSFO.BusinessAreaCredit) },
                { "propNamePartnerCredit", nameof(AccountingMovingMSFO.PartnerCredit) }
            };

        private readonly string[] _subPositionValuesForContragent = { "200", "202", "210", "212" };

        private readonly string[] _subPositionValuesForExternalIDCredit = { "220", "240", "290" };

        private readonly string[] _subPositionValuesForPartnerCredit = { "210", "212" };

        public void Validate(DataTable dataTable, ref ImportHistory history)
        {
            Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(dataTable);
            DataTable cleanDataTable = ImportHelper.GetDataTableWithoutHeader(dataTable);
            string errorType = ImportExtention.GetErrorTypeName(ErrorType.System);
            int startRowIndex = ImportHelper.GetRowStartIndex(dataTable);
            DataTable shortDataTable = dataTable.AsEnumerable().Skip(startRowIndex).CopyToDataTable();

            Dictionary<string, string> concatedDictionary = _propNamesDictionary
                .Concat(_subPositionRequiredPropNamesDictionary)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            ImportHelper.CheckColumnsExisting(concatedDictionary, colsNameMapping, errorType, ref history);

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
                        history.ImportErrorLogs.AddError(errorRowNumber, columnNumberPair.Value, fieldName, ErrorType.Required);
                    }
                    else if (fieldName == nameof(AccountingMovingMSFO.AccountGKDebit) && !value.StartsWith(AccountGKAndPositionValidValue))
                    {
                        int errorRowNumber = shortDataTable.Rows.IndexOf(dataRow) + 1;
                        history.ImportErrorLogs.AddError(errorRowNumber, columnNumberPair.Value,
                            fieldName, AccountGKDebitErrorText, ErrorType.Required);
                    }
                    else if (fieldName == nameof(AccountingMovingMSFO.PositionDebit) && !value.StartsWith(AccountGKAndPositionValidValue))
                    {
                        int errorRowNumber = shortDataTable.Rows.IndexOf(dataRow) + 1;
                        history.ImportErrorLogs.AddError(errorRowNumber, columnNumberPair.Value,
                            fieldName, PositionDebitErrorText, ErrorType.Required);
                    }
                }
            }

            Dictionary<string, int> subPositionСolumnNumbersDictionary =
                ImportHelper.GetDictionaryOfColumnNumbers(_subPositionRequiredPropNamesDictionary, colsNameMapping, cleanDataTable, true);

            foreach (DataRow dataRow in shortDataTable.Rows)
            {
                string subPositionValue = dataRow[columnNumbersDictionary["propNameSubPosition"]].ToString();

                if (_subPositionValuesForContragent.Contains(subPositionValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow, subPositionСolumnNumbersDictionary["propNameContragent"],
                        _subPositionRequiredPropNamesDictionary["propNameContragent"], ref history, SubPositionErrorText);
                }

                if (_subPositionValuesForExternalIDCredit.Contains(subPositionValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow, subPositionСolumnNumbersDictionary["propNameExternalIDCredit"],
                        _subPositionRequiredPropNamesDictionary["propNameExternalIDCredit"], ref history, SubPositionErrorText);
                }

                if (subPositionValue == SubPositionValueForBusinessAreaCredit)
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow, subPositionСolumnNumbersDictionary["propNameBusinessAreaCredit"],
                        _subPositionRequiredPropNamesDictionary["propNameBusinessAreaCredit"], ref history, SubPositionErrorText);
                }

                if (_subPositionValuesForPartnerCredit.Contains(subPositionValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow, subPositionСolumnNumbersDictionary["propNamePartnerCredit"],
                        _subPositionRequiredPropNamesDictionary["propNamePartnerCredit"], ref history, SubPositionErrorText);
                }
            }
        }
    }
}
