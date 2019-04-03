using System;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal.Writers
{
    public abstract class SimpleWriter<TData, TValue> : ValueWriter, ICellWriterProvider<TData>
    {
        private readonly Func<TData, TValue> _func;


        protected SimpleWriter(Func<TData, TValue> func, CellValues? value_type) : base(value_type)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            _func = func;
        }

        protected abstract string GetValue(ExcelExporter exporter, TValue value);


        public ISheetWriter GetWriter(ExcelExporter exporter, TData data)
        {
            var value = _func(data);

            Value = GetValue(exporter, value);

            return Value == null ? null : this;
        }
    }
}