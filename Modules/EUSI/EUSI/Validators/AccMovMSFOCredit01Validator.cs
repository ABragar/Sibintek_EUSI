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
    public class AccMovMSFOCredit01Validator : IExcelImportFileEntityValidator
    {
        private const string AccountGKCreditErrorText =
            "Поле \"Счет ГК (учетной системы) (Кредит)\" не соответствует виду движения.";

        private const string SubPositionErrorText =
            "Отсутствует значение при заполненном поле \"Подпозиция консолидации\".";

        private const string AccountGKDebitErrorText =
            "Отсутствует значение при заполненном поле \"Счет ГК (учетной системы) (Дебет)\".";

        private const string SubPositionValueForBusinessAreaCredit = "340";

        private const string SubPositionValueForPartnerCredit = "310";

        private const string AccountGKDebitValueForInventoryDebit = "01";

        private const string AccountGKCreditValidValue = "01";

        /// <summary>
        /// Список полей, всегда обязательных к заполнению.
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
                { "propNamePositionDebit", nameof(AccountingMovingMSFO.PositionDebit) },
                { "propNameOKOFDebit", nameof(AccountingMovingMSFO.OKOFDebit) },
                { "propNameIXOInitialDebit", nameof(AccountingMovingMSFO.IXOInitialDebit) },
                { "propNameBusinessUnitDebit", nameof(AccountingMovingMSFO.BusinessUnitDebit) },
                { "propNameIXODepreciationDebit", nameof(AccountingMovingMSFO.IXODepreciationDebit) },
                { "propNameGroupObjDebit", nameof(AccountingMovingMSFO.GroupObjDebit) },
                { "propNameAccountGKCredit", nameof(AccountingMovingMSFO.AccountGKCredit) }
            };

        /// <summary>
        /// Список полей, обязательных к заполнению при наличии определенного значения подпозиции 
        /// консолидации или значения Счет ГК (учетной системы) (Дебет).
        /// </summary>
        private readonly Dictionary<string, string> _conditionallyRequiredPropNamesDictionary = new Dictionary<string, string>
            {
                { "propNameContragent", nameof(AccountingMovingMSFO.Contragent) },
                { "propNameExternalIDCredit", nameof(AccountingMovingMSFO.ExternalIDCredit) },
                { "propNameInventoryCredit", nameof(AccountingMovingMSFO.InventoryCredit) },
                { "propNameBusinessAreaCredit", nameof(AccountingMovingMSFO.BusinessAreaCredit) },
                { "propNamePartnerCredit", nameof(AccountingMovingMSFO.PartnerCredit) },
                { "propNameExternalIDDebit", nameof(AccountingMovingMSFO.ExternalIDDebit) },
                { "propNameInventoryDebit", nameof(AccountingMovingMSFO.InventoryDebit) },
            };

        private readonly string[] _subPositionValuesForContragent = { "300", "303", "304", "310" };

        private readonly string[] _subPositionValuesForExternalIDAndInventory = { "340", "390" };

        private readonly string[] _accountGKDebitValuesForExternalIDDebit = { "01", "07", "08" };

        public void Validate(DataTable dataTable, ref ImportHistory history)
        {
            Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(dataTable);
            DataTable cleanDataTable = ImportHelper.GetDataTableWithoutHeader(dataTable);
            string errorType = ImportExtention.GetErrorTypeName(ErrorType.System);
            int startRowIndex = ImportHelper.GetRowStartIndex(dataTable);
            DataTable shortDataTable = dataTable.AsEnumerable().Skip(startRowIndex).CopyToDataTable();

            Dictionary<string, string> concatedDictionary = _propNamesDictionary
                .Concat(_conditionallyRequiredPropNamesDictionary)
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
                    else if (fieldName == nameof(AccountingMovingMSFO.AccountGKCredit) && !value.StartsWith(AccountGKCreditValidValue))
                    {
                        int errorRowNumber = shortDataTable.Rows.IndexOf(dataRow) + 1;
                        history.ImportErrorLogs.AddError(errorRowNumber, columnNumberPair.Value,
                            fieldName, AccountGKCreditErrorText, ErrorType.Required);
                    }
                }
            }

            if (history.ImportErrorLogs.Count > 0)
            {
                return;
            }

            Dictionary<string, int> conditionСolumnNumbersDictionary =
                ImportHelper.GetDictionaryOfColumnNumbers(_conditionallyRequiredPropNamesDictionary, colsNameMapping, cleanDataTable, true);

            foreach (DataRow dataRow in shortDataTable.Rows)
            {
                string subPositionValue = dataRow[columnNumbersDictionary["propNameSubPosition"]].ToString();

                if (_subPositionValuesForContragent.Contains(subPositionValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow,
                        conditionСolumnNumbersDictionary["propNameContragent"],
                        _conditionallyRequiredPropNamesDictionary["propNameContragent"], ref history, SubPositionErrorText);
                }

                if (_subPositionValuesForExternalIDAndInventory.Contains(subPositionValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow,
                        conditionСolumnNumbersDictionary["propNameExternalIDCredit"],
                        _conditionallyRequiredPropNamesDictionary["propNameExternalIDCredit"], ref history, SubPositionErrorText);
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow,
                        conditionСolumnNumbersDictionary["propNameInventoryCredit"],
                        _conditionallyRequiredPropNamesDictionary["propNameInventoryCredit"], ref history, SubPositionErrorText);
                }

                if (subPositionValue == SubPositionValueForBusinessAreaCredit)
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow,
                        conditionСolumnNumbersDictionary["propNameBusinessAreaCredit"],
                        _conditionallyRequiredPropNamesDictionary["propNameBusinessAreaCredit"], ref history, SubPositionErrorText);
                }

                if (subPositionValue == SubPositionValueForPartnerCredit)
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow,
                        conditionСolumnNumbersDictionary["propNamePartnerCredit"],
                        _conditionallyRequiredPropNamesDictionary["propNamePartnerCredit"], ref history, SubPositionErrorText);
                }

                string accountGKDebitFullValue = dataRow[columnNumbersDictionary["propNameAccountGKDebit"]].ToString();

                if (accountGKDebitFullValue.Length < 2)
                {
                    continue;
                }

                string accountGKDebitValue = accountGKDebitFullValue.Substring(0, 2);

                if (_accountGKDebitValuesForExternalIDDebit.Contains(accountGKDebitValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow,
                        conditionСolumnNumbersDictionary["propNameExternalIDDebit"],
                        _conditionallyRequiredPropNamesDictionary["propNameExternalIDDebit"], ref history, AccountGKDebitErrorText);
                }

                if (accountGKDebitValue == AccountGKDebitValueForInventoryDebit)
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow,
                        conditionСolumnNumbersDictionary["propNameInventoryDebit"],
                        _conditionallyRequiredPropNamesDictionary["propNameInventoryDebit"], ref history, AccountGKDebitErrorText);
                }
            }
        }
    }
}
