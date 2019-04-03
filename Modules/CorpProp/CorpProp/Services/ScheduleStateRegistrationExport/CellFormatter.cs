using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CorpProp.Services.ScheduleStateRegistrationExport
{
    internal class CellFormatter
    {
        private CellValues _cellValueType = CellValues.String;
        public CellFormatter SetValueType(CellValues value)
        {
            this._cellValueType = value;
            return this;
        }

        public void Format(Cell cell)
        {
            cell.DataType = new EnumValue<CellValues>(_cellValueType);
        }
    }
}
