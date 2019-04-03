using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public class ValueWriter : ISheetWriter
    {
        protected readonly CellValues? ValueType;

        protected string Value;
        protected uint? Style;
        public ValueWriter(CellValues? value_type)
        {
            ValueType = value_type;
        }

        public virtual void Write(SheetContext context)
        {
            context.Writer.WriteElement(new Cell()
            {
                CellValue = Value == null ? null : new CellValue(Value),
                DataType = ValueType == null ? null : new EnumValue<CellValues>(ValueType),
                StyleIndex = Style == null ? null : new UInt32Value(Style),
                CellReference = context.CellReference
            });

        }
    }
}