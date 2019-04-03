using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Import;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Validators;

namespace EUSI.Validators
{
    public class AccountingObjectDuplicatesValidator : IExcelImportFileEntityValidator
    {
        public void Validate(DataTable dataTable, ref ImportHistory history)
        {
            int columnsSystemNameStringRow = ImportHelper.GetRowSystemNameRow(dataTable);
            Dictionary<string, string> colsNameMapping = ImportHelper.ColumnsNameMapping(dataTable);
            var cleanDataTable = ImportHelper.GetDataTableWithoutHeader(dataTable);

            string propNameEUSI = nameof(AccountingObject.EUSINumber);
            string propNameConsolidation = nameof(AccountingObject.Consolidation);
            string propNameInventoryNumber = nameof(AccountingObject.InventoryNumber);
            string propNameSubNumber = nameof(AccountingObject.SubNumber);

            if (!colsNameMapping.ContainsValue(propNameEUSI) ||
                !colsNameMapping.ContainsValue(propNameConsolidation) ||
                !colsNameMapping.ContainsValue(propNameInventoryNumber) ||
                !colsNameMapping.ContainsValue(propNameSubNumber))
            {
                history.ImportErrorLogs.Add(new ImportErrorLog
                {
                    MessageDate = DateTime.Now,
                    ErrorText = "Ошибка проверки записей на дублирование, проверьте правильность шаблона на наличие колонок: Номер ЕУСИ, БЕ, Инвентарный номер, Субномер.",
                    ErrorType = ImportExtention.GetErrorTypeName(ErrorType.System)
                });
                return;
            }

            //Определяем номера колонок необходимых для валидации полей
            string columnNameEUSI = colsNameMapping.FirstOrDefault(x => x.Value == propNameEUSI).Key;
            int columnNumberEUSI = ImportHelper.GetColumnNumber(cleanDataTable, columnNameEUSI);
            string columnNameConsolidation = colsNameMapping.FirstOrDefault(x => x.Value == propNameConsolidation).Key;
            int columnNumberConsolidation = ImportHelper.GetColumnNumber(cleanDataTable, columnNameConsolidation);
            string columnNameInventoryNumber = colsNameMapping.FirstOrDefault(x => x.Value == propNameInventoryNumber).Key;
            int columnNumberInventoryNumber = ImportHelper.GetColumnNumber(cleanDataTable, columnNameInventoryNumber);
            string columnNameSubNumber = colsNameMapping.FirstOrDefault(x => x.Value == propNameSubNumber).Key;
            int columnNumberSubNumber = ImportHelper.GetColumnNumber(cleanDataTable, columnNameSubNumber);

            int startRowIndex = ImportHelper.GetRowStartIndex(dataTable);
            EnumerableRowCollection<DataRow> shortDataTable =
                dataTable.AsEnumerable().Skip(startRowIndex).CopyToDataTable().AsEnumerable();

            //Собираем список анонимных объектов по требуемым полям 
            var listAccountingObjectsFromExcel = shortDataTable
                .Select(a =>
                    new
                    {
                        EUSINumber = Int32.TryParse(a[columnNumberEUSI].ToString(),
                            out var eUSINumber) ? eUSINumber : (int?)null,
                        ConsolidationID = Int32.TryParse(a[columnNumberConsolidation].ToString(),
                            out var consolidationID) ? consolidationID : (int?)null,
                        InventoryNumber = a[columnNumberInventoryNumber].ToString(),
                        SubNumber = a[columnNumberSubNumber].ToString()
                    }
                )
                .ToList();

            //Когда мы превращаем List анонимных объектов в Hashset, дубликаты удаляются.
            var hashsetAccountingObjectsFromExcel = listAccountingObjectsFromExcel.ToHashSet();

            if (hashsetAccountingObjectsFromExcel.Count < listAccountingObjectsFromExcel.Count)
            {
                var dictOfDuplicatesCount = new Dictionary<string, int>();
                var duplicatesHashset = hashsetAccountingObjectsFromExcel.Where(el =>
                {
                    int count = listAccountingObjectsFromExcel
                        .Where(s => s.InventoryNumber == el.InventoryNumber &&
                            s.SubNumber == el.SubNumber &&
                            s.ConsolidationID == el.ConsolidationID &&
                            s.EUSINumber == el.EUSINumber)
                        .Count();

                    //Если количество записей больше одной, то присутствуют дубликаты этой записи в таблице
                    if (count > 1)
                    {
                        if (!dictOfDuplicatesCount.ContainsKey(el.InventoryNumber))
                            dictOfDuplicatesCount.Add(el.InventoryNumber, count - 1);
                        return true;
                    }
                    return false;
                }).ToHashSet();

                string errorType = ImportExtention.GetErrorTypeName(ErrorType.System);

                foreach (var duplicate in duplicatesHashset)
                {
                    history.ImportErrorLogs.Add(new ImportErrorLog
                    {
                        MessageDate = DateTime.Now,
                        ErrorText = $"Запись с инвентарным номером {duplicate.InventoryNumber} имеет {dictOfDuplicatesCount[duplicate.InventoryNumber]} " +
                            $"дубликатов в файле импорта по полям: Инвентарный номер(Колонка {columnNumberInventoryNumber})," +
                            $"БЕ(Колонка {columnNumberConsolidation}), Номер ЕУСИ(Колонка {columnNumberEUSI}) и Субномер(Колонка {columnNumberSubNumber}).",
                        ErrorType = errorType
                    });
                }
            }
        }
    }
}
