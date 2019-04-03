using System;
using System.Threading;

namespace FankySheet
{
    public interface IExcelExportDataBuilder<TData>
    {

        IExcelExportDataBuilder<TData> Sheet(int begin,Func<int,string> name_func);

        IExcelExportDataBuilder<TData> Cancel(CancellationToken token);

        IExcelExportDataBuilder<TData> WriteHeader(bool header);

        IExcelExportDataBuilder<TData> RowsCount(int count);

        IExcelExportDataBuilder<TData> Add(Func<TData, int?> func, Action<IColumnSettingsBuilder> column = null);

        IExcelExportDataBuilder<TData> Add(Func<TData, double?> func, Action<IColumnSettingsBuilder> column = null);

        IExcelExportDataBuilder<TData> Add(Func<TData, decimal?> func, Action<IColumnSettingsBuilder> column = null);

        IExcelExportDataBuilder<TData> Add(Func<TData, string> func, Action<IColumnSettingsBuilder> column = null);

        IExcelExportDataBuilder<TData> AddSharedString(Func<TData, string> func, Action<IColumnSettingsBuilder> column = null);

        IExcelExportDataBuilder<TData> Add(Func<TData, bool?> func, Action<IColumnSettingsBuilder> column = null);

        IExcelExportDataBuilder<TData> Add(Func<TData, DateTime?> func, Action<IColumnSettingsBuilder> column = null);
    }
}