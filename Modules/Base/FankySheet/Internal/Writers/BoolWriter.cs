using System;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public class BoolWriter<TData> : SimpleWriter<TData, bool?>
    {
        public BoolWriter(Func<TData, bool?> func) : base(func, CellValues.Boolean)
        {
        }

        protected override string GetValue(ExcelExporter exporter,bool? value)
        {
            return value == null ? null : value.Value ? "1" : "0";
            
        }
    }
}