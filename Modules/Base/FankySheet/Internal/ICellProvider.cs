namespace FankySheet.Internal
{
    public interface ICellWriterProvider<in TData>
    {
        ISheetWriter GetWriter(ExcelExporter exporter, TData data);
    }
}