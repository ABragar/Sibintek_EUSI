using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CorpProp.Services.ScheduleStateRegistrationExport
{
    internal class CellWriter
    {
        protected string _colName;
        protected uint _rowIndex;
        protected CellFormatter _formater;
        private string _value;

        protected virtual string GetCellValue()
        {
            return _value;
        }

        protected CellWriter(string colName, uint rowIndex)
        {
            this._colName = colName;
            this._rowIndex = rowIndex;
            this._formater = new CellFormatter();
        }

        public CellWriter(string colName, uint rowIndex, string value)
        {
            this._colName = colName;
            this._rowIndex = rowIndex;
            this._value = value;
            this._formater = new CellFormatter();
        }

        protected Cell GetCell(Worksheet worksheet)
        {
            Row row = GetRow(worksheet, _rowIndex);

            if (row == null) return null;

            var cell = row.Elements<Cell>()
                .LastOrDefault(c => String.Compare(c.CellReference.Value, $"{_colName}{_rowIndex}", StringComparison.OrdinalIgnoreCase) <= 0);


            return cell;
        }
        protected Row GetRow(Worksheet worksheet, uint rowIndex)
        {
            Row row = worksheet.GetFirstChild<SheetData>().Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            if (row == null)
            {
                Row lastRow = sheetData.Elements<Row>().Last();
                row = CopyToLine(lastRow, rowIndex, sheetData);
            }

            return row;
        }

        internal Row CopyToLine(Row refRow, uint rowIndex, SheetData sheetData)
        {
            uint newRowIndex;
            var newRow = (Row)refRow.CloneNode(true);
            // Loop through all the rows in the worksheet with higher row 
            // index values than the one you just added. For each one,
            // increment the existing row index.

            foreach (Cell cell in newRow.Elements<Cell>())
            {
                // Update the references for reserved cells.
                string cellReference = cell.CellReference.Value;
                cell.CellReference = new StringValue(cellReference.Replace(newRow.RowIndex.Value.ToString(), rowIndex.ToString()));
            }

            newRow.RowIndex = rowIndex;
            sheetData.InsertAfter(newRow, refRow);
            return newRow;
        }

        public CellWriter SetFormatter(CellFormatter formater)
        {
            this._formater = formater;
            return this;
        }

        public void Write(Worksheet wsheet)
        {
            var cell = GetCell(wsheet);
            _formater.Format(cell);
            cell.CellValue = new CellValue(GetCellValue());
        }
    }
}
