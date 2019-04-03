using System;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public class DoubleWriter<TData> : SimpleWriter<TData, double?>
    {
        public DoubleWriter(Func<TData, double?> func) : base(func, CellValues.Number)
        {
        }

        protected override string GetValue(ExcelExporter exporter, double? value)
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}