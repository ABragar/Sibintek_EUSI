using System;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public class SharedStringWriter<TData> : SimpleWriter<TData, string>
    {
        public SharedStringWriter(Func<TData, string> func) : base(func, CellValues.SharedString)
        {

        }


        protected override string GetValue(ExcelExporter exporter, string value)
        {
            return value == null ? null : exporter.CreateSharedString(value).ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}