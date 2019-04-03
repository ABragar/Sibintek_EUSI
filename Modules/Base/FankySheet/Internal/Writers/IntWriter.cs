using System;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public class IntWriter<TData> : SimpleWriter<TData,int?>
    {
        public IntWriter(Func<TData, int?> func) : base(func, CellValues.Number)
        {
        }

        protected override string GetValue(ExcelExporter exporter, int? value)
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo);
            
        }
    }
}