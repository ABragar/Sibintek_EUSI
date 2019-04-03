using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CorpProp.Entities.Import;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Validators;
using EUSI.Entities.Accounting;

namespace EUSI.Validators
{
    public class AccountingMovingMSFODataValidator : IExcelImportFileEntityValidator
    {
        private readonly List<string> _validateValues = new List<string> { "01", "08" };

        private DataTable _dataTable;
        private EnumerableRowCollection<DataRow> _shortDataTable;

        private readonly string _propNamePositionDebit = nameof(AccountingMovingMSFO.PositionDebit);
        private readonly string _propNameAccountingGkDebit = nameof(AccountingMovingMSFO.AccountGKDebit);

        private int _positionDebit;
        private int _accountingGkDebit;
        private int _startRowIndex;
        private int _columnsUserNameStringRow;

        public void Validate(DataTable dataTable, ref ImportHistory history)
        {
            Initialize(dataTable, ref history);
            ValidateAccountingGk(ref history);
            ValidateForConcreteTemplate(ref history);
        }

        private void ValidateForConcreteTemplate(ref ImportHistory history)
        {
            var templateNameArray = history.TemplateName.Split('_');
            var movingName = templateNameArray[templateNameArray.Length - 1];
            if (Enum.TryParse(movingName, out TypeMovingMSFO movingType))
            {
                switch (movingType)
                {
                    case TypeMovingMSFO.Depreciation01:
                        new AccMovMSFODepreciation01Validator().Validate(_dataTable, ref history);
                        break;
                    case TypeMovingMSFO.Debit01:
                        new AccMovMSFODebit01Validator().Validate(_dataTable, ref history);
                        break;
                    case TypeMovingMSFO.Debit07:
                        new AccMovMSFODebit07Validator().Validate(_dataTable, ref history);
                        break;
                    case TypeMovingMSFO.Debit08:
                        new AccMovMSFODebit08Validator().Validate(_dataTable, ref history);
                        break;
                    case TypeMovingMSFO.Credit01:
                        new AccMovMSFOCredit01Validator().Validate(_dataTable, ref history);
                        break;
                    case TypeMovingMSFO.Credit07:
                        new AccMovMSFOCredit07Validator().Validate(_dataTable, ref history);
                        break;
                    case TypeMovingMSFO.Credit08:
                        new AccMovMSFOCredit08Validator().Validate(_dataTable, ref history);
                        break;
                }
            }
        }

        private void Initialize(DataTable dataTable, ref ImportHistory history)
        {
            _dataTable = dataTable;

            var colsNameMapping = ImportHelper.ColumnsNameMapping(dataTable);
            var cleanDataTable = ImportHelper.GetDataTableWithoutHeader(dataTable);
            _columnsUserNameStringRow = ImportHelper.GetRowUserNameRow(dataTable);

            _startRowIndex = ImportHelper.GetRowStartIndex(dataTable);
            _shortDataTable = dataTable.AsEnumerable().Skip(_startRowIndex).CopyToDataTable().AsEnumerable();

            if (!colsNameMapping.ContainsValue(_propNamePositionDebit) ||
                !colsNameMapping.ContainsValue(_propNameAccountingGkDebit)
                )
            {
                history.ImportErrorLogs.Add(new ImportErrorLog
                {
                    MessageDate = DateTime.Now,
                    ErrorText = "Файл не соответствует шаблону.",
                    ErrorType = ImportExtention.GetErrorTypeName(ErrorType.System)
                });
                return;
            }

            _positionDebit = ImportHelper.GetColumnNumber(
                cleanDataTable,
                colsNameMapping.FirstOrDefault(x => x.Value == _propNamePositionDebit).Key);
            _accountingGkDebit = ImportHelper.GetColumnNumber(
                cleanDataTable,
                colsNameMapping.FirstOrDefault(x => x.Value == _propNameAccountingGkDebit).Key);
        }

        private void ValidateAccountingGk(ref ImportHistory history)
        {            

            var index = 1;
            var invalidItems = _shortDataTable
                .AsEnumerable()
                .Select(dr =>
                    new
                    {
                        RowIndex = index++,
                        Consolidation = (dr.ItemArray.ElementAtOrDefault(_positionDebit)?.ToString()?.Length > 2) ? dr.ItemArray.ElementAtOrDefault(_positionDebit)?.ToString()?.Substring(0, 2) : "",
                        AccountingGK = (dr.ItemArray.ElementAtOrDefault(_accountingGkDebit)?.ToString()?.Length > 2) ? dr.ItemArray.ElementAtOrDefault(_accountingGkDebit)?.ToString()?.Substring(0, 2): ""
                    })
                .Where(i =>
                    i.Consolidation != i.AccountingGK ||
                    !_validateValues.Contains(i.Consolidation) ||
                    !_validateValues.Contains(i.AccountingGK)
                    );

            foreach (var item in invalidItems)
            {
                history.ImportErrorLogs.AddError(
                    item.RowIndex + _startRowIndex,
                    _positionDebit,
                    _dataTable.Rows[_columnsUserNameStringRow][_positionDebit].ToString(),
                    "Позиция консолидации не соответствует указанному счету ГК.",
                    ErrorType.System);
            }
        }
    }
}
