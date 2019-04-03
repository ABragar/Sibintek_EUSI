using System;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public class DateTimeWriter<TData> : SimpleWriter<TData, DateTime?>
    {
        public DateTimeWriter(Func<TData, DateTime?> func) : base(func, null)
        {

        }

        private static Guid _datetime = Guid.NewGuid();
        private static Guid _numbering = Guid.NewGuid();

        protected override string GetValue(ExcelExporter exporter, DateTime? value)
        {
            //todo use column style
            if (Style == null)
            {
                var numbering = exporter.Styles.GetOrCreateStyle(_numbering, id => new NumberingFormat
                {
                    NumberFormatId = id,
                    FormatCode = @"dd.MM.yyyy hh:mm:ss"
                });


                Style = exporter.Styles.GetOrCreateStyle(_datetime, id => new CellFormat()
                {
                    ApplyNumberFormat = true,
                    FormatId = 0,
                    NumberFormatId = numbering.StyleId
                }).StyleIndex;

            }


            return value?.ToOADate().ToString(NumberFormatInfo.InvariantInfo);
        }

    }

}
