using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public class StringWriter<TData> : SimpleWriter<TData, string>
    {
        public StringWriter(Func<TData, string> func) : base(func, CellValues.InlineString)
        {

        }

        protected override string GetValue(ExcelExporter exporter, string value)
        {
            return value;
        }

        public override void Write(SheetContext context)
        {

            context.Writer.WriteElement(new Cell()
            {
                DataType = new EnumValue<CellValues>(ValueType),
                InlineString = new InlineString(new Text(Value)),
                CellReference = context.CellReference
                
            });

        }
    }
}