using System;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public class DecimalWriter<TData> : SimpleWriter<TData, decimal?>
    {
        public DecimalWriter(Func<TData, decimal?> func) : base(func, CellValues.Number)
        {
        }

        protected override string GetValue(ExcelExporter exporter, decimal? value)
        {
            return value?.ToString(NumberFormatInfo.InvariantInfo);
            
        }
    }


    
}