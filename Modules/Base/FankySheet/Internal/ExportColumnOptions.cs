namespace FankySheet.Internal
{
    internal class ExportColumnOptions<TData>
    {

        public ExportOptions<TData> Options { get; set; }

        public ISheetWriter Header { get; set; }

        public ICellWriterProvider<TData> Cell { get; set; }

        public ISheetWriter Column { get; set; }
        
    }
}