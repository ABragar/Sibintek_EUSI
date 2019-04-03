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
    public class AccMovMSFODebit01Validator : IExcelImportFileEntityValidator
    {
        /// <summary>
        /// Список полей, всегда обязательных к заполнению
        /// </summary>
        private readonly Dictionary<string, string> _propNamesDictionary = new Dictionary<string, string>
            {
                { "propNameEUSINumber", nameof(AccountingMovingMSFO.EUSINumber) },
                { "propNameConsolidation", nameof(AccountingMovingMSFO.Consolidation) },
                { "propNameSubPosition", nameof(AccountingMovingMSFO.SubPosition) },
                { "propNameDocDate", nameof(AccountingMovingMSFO.DocDate) },
                { "propNameDate", nameof(AccountingMovingMSFO.Date) },
                { "propNameCost", nameof(AccountingMovingMSFO.Cost) },
                { "propNameCostDepreciation", nameof(AccountingMovingMSFO.CostDepreciation) },
                { "propNamePositionStorno", nameof(AccountingMovingMSFO.PositionStorno) },
                { "propNameAccountGKDebit", nameof(AccountingMovingMSFO.AccountGKDebit) },
                { "propNameNameDebit", nameof(AccountingMovingMSFO.NameDebit) },
                { "propNamePositionDebit", nameof(AccountingMovingMSFO.PositionDebit) },
                { "propNameExternalIDDebit", nameof(AccountingMovingMSFO.ExternalIDDebit) },
                { "propNameInventoryDebit", nameof(AccountingMovingMSFO.InventoryDebit) },
                { "propNameBusinessAreaDebit", nameof(AccountingMovingMSFO.BusinessAreaDebit) },
                { "propNameOKOFDebit", nameof(AccountingMovingMSFO.OKOFDebit) },
                { "propNameDepositDebit", nameof(AccountingMovingMSFO.DepositDebit) },
                { "propNameDepGroupDebit", nameof(AccountingMovingMSFO.DepGroupDebit) },
                { "propNameIXOInitialDebit", nameof(AccountingMovingMSFO.IXOInitialDebit) },
                { "propNameAnalyticOneDebit", nameof(AccountingMovingMSFO.AnalyticOneDebit) },
                { "propNameAnalyticTwoDebit", nameof(AccountingMovingMSFO.AnalyticTwoDebit) },
                { "propNameIXODepreciationDebit", nameof(AccountingMovingMSFO.IXODepreciationDebit) },
                { "propNameGroupObjDebit", nameof(AccountingMovingMSFO.GroupObjDebit) },
                { "propNameUsefulDebit", nameof(AccountingMovingMSFO.UsefulDebit) },
                { "propNameUsefulEndDebit", nameof(AccountingMovingMSFO.UsefulEndDebit) },
                { "propNameAccountGKCredit", nameof(AccountingMovingMSFO.AccountGKCredit) }
            };

        /// <summary>
        /// Список полей, обязательных к заполнению при наличии определенного значения подпозиции консолидации
        /// </summary>
        private readonly Dictionary<string, string> _subPositionRequiredPropNamesDictionary = new Dictionary<string, string>
            {
                { "propNameContragent", nameof(AccountingMovingMSFO.Contragent) },
                { "propNameExternalIDCredit", nameof(AccountingMovingMSFO.ExternalIDCredit) },
                { "propNameInventoryCredit", nameof(AccountingMovingMSFO.InventoryCredit) },
                { "propNameBusinessAreaCredit", nameof(AccountingMovingMSFO.BusinessAreaCredit) },
                { "propNamePartnerCredit", nameof(AccountingMovingMSFO.PartnerCredit) }
            };

        private readonly string _accountGKAndPositionDebitValidValue = "01";

        private readonly string _accountGKDebitErrorText =
            "Поле \"Счет ГК (учетной системы) (Дебет)\" не соответствует виду движения.";

        private readonly string _positionDebitErrorText =
            "Поле \"Позиция консолидации (Дебет)\" не соответствует виду движения.";

        private readonly string _subPositionErrorText =
            "Отсутствует значение при заполненном поле \"Подпозиция консолидации\".";

        private readonly string[] _subPositionValuesForContragent = { "200", "210" };

        private readonly string[] _subPositionValuesForExternalIDCredit = { "200", "240", "290" };

        private readonly string[] _subPositionValuesForInventoryCredit = { "240", "290" };

        private readonly string _subPositionValueForBusinessAreaCredit = "240";

        private readonly string _subPositionValueForPartnerCredit = "210";

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
                    else if (fieldName == nameof(AccountingMovingMSFO.AccountGKDebit) && !value.StartsWith(_accountGKAndPositionDebitValidValue))
                    {
                        int errorRowNumber = shortDataTable.Rows.IndexOf(dataRow) + 1;
                        history.ImportErrorLogs.AddError(errorRowNumber, columnNumberPair.Value,
                            fieldName, _accountGKDebitErrorText, ErrorType.Required);
                    }
                    else if (fieldName == nameof(AccountingMovingMSFO.PositionDebit) && !value.StartsWith(_accountGKAndPositionDebitValidValue))
                    {
                        int errorRowNumber = shortDataTable.Rows.IndexOf(dataRow) + 1;
                        history.ImportErrorLogs.AddError(errorRowNumber, columnNumberPair.Value,
                            fieldName, _positionDebitErrorText, ErrorType.Required);
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
                        _subPositionRequiredPropNamesDictionary["propNameContragent"], ref history, _subPositionErrorText);
                }

                if (_subPositionValuesForExternalIDCredit.Contains(subPositionValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow, subPositionСolumnNumbersDictionary["propNameExternalIDCredit"],
                        _subPositionRequiredPropNamesDictionary["propNameExternalIDCredit"], ref history, _subPositionErrorText);
                }

                if (_subPositionValuesForInventoryCredit.Contains(subPositionValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow, subPositionСolumnNumbersDictionary["propNameInventoryCredit"],
                        _subPositionRequiredPropNamesDictionary["propNameInventoryCredit"], ref history, _subPositionErrorText);
                }

                if (subPositionValue == _subPositionValueForBusinessAreaCredit)
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow, subPositionСolumnNumbersDictionary["propNameBusinessAreaCredit"],
                        _subPositionRequiredPropNamesDictionary["propNameBusinessAreaCredit"], ref history, _subPositionErrorText);
                }

                if (subPositionValue == _subPositionValueForPartnerCredit)
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow, subPositionСolumnNumbersDictionary["propNamePartnerCredit"],
                        _subPositionRequiredPropNamesDictionary["propNamePartnerCredit"], ref history, _subPositionErrorText);
                }
            }
        }
    }
}
