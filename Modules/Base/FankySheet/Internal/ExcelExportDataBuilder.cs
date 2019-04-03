using System;
using System.Threading;
using FankySheet.Internal.Writers;

namespace FankySheet.Internal
{
    internal class ExcelExportDataBuilder<TData> : IExcelExportDataBuilder<TData>
    {
        public ExcelExportDataBuilder(ExportOptions<TData> options)
        {
            Options = options;
        }

        private ExportOptions<TData> Options { get; set; }

        public IExcelExportDataBuilder<TData> Sheet(int begin, Func<int, string> name_func)
        {
            Options.Begin = begin;

            Options.SheetNameFunc = name_func;

            return this;
        }

        public IExcelExportDataBuilder<TData> Cancel(CancellationToken token)
        {

            Options.CancellationToken = token;
            return this;
        }


        public IExcelExportDataBuilder<TData> WriteHeader(bool header)
        {
            Options.WriteHeader = header;
            return this;
        }

        public IExcelExportDataBuilder<TData> RowsCount(int count)
        {
            Options.RowsCount = count;
            return this;

        }

        public IExcelExportDataBuilder<TData> Add(Func<TData, int?> func, Action<IColumnSettingsBuilder> column)
        {

            return AddColumn(new IntWriter<TData>(func), column);
        }

        private IExcelExportDataBuilder<TData> AddColumn(ICellWriterProvider<TData> cell,
            Action<IColumnSettingsBuilder> column_builder)
        {

            var column = new ExportColumnOptions<TData>
            {
                Cell = cell
            };


            var builder = new ColumnSettingsBuilder<TData>(column);
            
            column_builder?.Invoke(builder);


            Options.Columns.Add(column);


            return this;
        }

        public IExcelExportDataBuilder<TData> Add(Func<TData, double?> func, Action<IColumnSettingsBuilder> column)
        {
            return AddColumn(new DoubleWriter<TData>(func), column);
        }

        public IExcelExportDataBuilder<TData> Add(Func<TData, decimal?> func, Action<IColumnSettingsBuilder> column)
        {

            return AddColumn(new DecimalWriter<TData>(func), column);
        }

        public IExcelExportDataBuilder<TData> Add(Func<TData, string> func, Action<IColumnSettingsBuilder> column)
        {
            return AddColumn(new StringWriter<TData>(func), column);
        }

        public IExcelExportDataBuilder<TData> AddSharedString(Func<TData, string> func, Action<IColumnSettingsBuilder> column)
        {
            return AddColumn(new SharedStringWriter<TData>(func), column);
        }

        public IExcelExportDataBuilder<TData> Add(Func<TData, bool?> func, Action<IColumnSettingsBuilder> column)
        {
            return AddColumn(new BoolWriter<TData>(func), column);
        }

        public IExcelExportDataBuilder<TData> Add(Func<TData, DateTime?> func, Action<IColumnSettingsBuilder> column)
        {
            return AddColumn(new DateTimeWriter<TData>(func), column);
        }
    }
}