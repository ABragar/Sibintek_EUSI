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
    public abstract class AccMovMSFOCreditBaseValidator : IExcelImportFileEntityValidator
    {
        private const string AccountGKCreditErrorText =
            "Поле \"Счет ГК (учетной системы) (Кредит)\" не соответствует виду движения.";

        private const string SubPositionErrorText =
            "Отсутствует значение при заполненном поле \"Подпозиция консолидации\".";

        private const string AccountGKDebitErrorText =
            "Отсутствует значение при заполненном поле \"Счет ГК (учетной системы) (Дебет)\".";

        private const string SubPositionValueForBusinessAreaCredit = "340";

        private const string SubPositionValueForPartnerCredit = "310";

        /// <summary>
        /// Список полей, всегда обязательных к заполнению.
        /// </summary>
        protected virtual Dictionary<string, string> _propNamesDictionary { get; set; }

        /// <summary>
        /// Строковое начение, с которого должен начинаться Счет ГК (учетной системы) (Кредит) в проверяемом шаблоне.
        /// </summary>
        protected virtual string _accountGKCreditValidValue { get; set; }

        /// <summary>
        /// Список полей, обязательных к заполнению при наличии определенного значения подпозиции 
        /// консолидации или значения Счет ГК (учетной системы) (Дебет).
        /// </summary>
        private readonly Dictionary<string, string> _conditionallyRequiredPropNamesDictionary = new Dictionary<string, string>
            {
                { "propNameContragent", nameof(AccountingMovingMSFO.Contragent) },
                { "propNameExternalIDCredit", nameof(AccountingMovingMSFO.ExternalIDCredit) },
                { "propNameBusinessAreaCredit", nameof(AccountingMovingMSFO.BusinessAreaCredit) },
                { "propNamePartnerCredit", nameof(AccountingMovingMSFO.PartnerCredit) },
                { "propNameExternalIDDebit", nameof(AccountingMovingMSFO.ExternalIDDebit) }//Счет ГК (учетной системы) (Дебет)
            };

        private readonly string[] _subPositionValuesForContragent = { "300", "310" };

        private readonly string[] _subPositionValuesForExternalIDCredit = { "320", "340", "390" };

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
                    else if (fieldName == nameof(AccountingMovingMSFO.AccountGKCredit) && !value.StartsWith(_accountGKCreditValidValue))
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

                if (_subPositionValuesForExternalIDCredit.Contains(subPositionValue))
                {
                    ImportHelper.ValidateRequiredFieldForNullValue(shortDataTable, dataRow,
                        conditionСolumnNumbersDictionary["propNameExternalIDCredit"],
                        _conditionallyRequiredPropNamesDictionary["propNameExternalIDCredit"], ref history, SubPositionErrorText);
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
            }
        }
    }
}
