using System;
using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using FankySheet.Internal.Writers;

namespace FankySheet.Internal
{
    class ColumnSettingsBuilder<TData> : IColumnSettingsBuilder, IDisposable
    {
        private ExportColumnOptions<TData> _column;

        public ColumnSettingsBuilder(ExportColumnOptions<TData> column)
        {
            _column = column;
        }

        private ColumnWriter _writer = new ColumnWriter();

        public IColumnSettingsBuilder Width(double? width)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            _writer.Width = width;

            _column.Column = _writer.GetWriter();

            return this;
        }

        public IColumnSettingsBuilder Caption(string caption)
        {

            _column.Header = caption == null ? null : new HeaderWriter(caption);
            return this;
        }

        private class ColumnWriter : ISheetWriter
        {
            public double? Width { get; set; }

            public uint? Style { get; set; }

            public bool CanWrite => Width != null || Style != null;

            public ISheetWriter GetWriter() => CanWrite ? this : null;

            public void Write(SheetContext context)
            {


                var column = new Column()
                {
                    Min = new UInt32Value(context.Column + 1),
                    Max = new UInt32Value(context.Column + 1),
                    Style = Style
                };

                if (Width != null)
                {
                    column.CustomWidth = new BooleanValue(true);
                    column.Width = new DoubleValue(Width);
                }

                context.Writer.WriteElement(column);

            }
        }

        private class HeaderWriter : ValueWriter
        {
            private readonly string _caption;

            public HeaderWriter(string caption) : base(CellValues.SharedString)
            {
                _caption = caption;
            }

            public override void Write(SheetContext context)
            {
                Value = context.Exporter.CreateSharedString(_caption).ToString(NumberFormatInfo.InvariantInfo);
                Style = 0;

                base.Write(context);
            }
        }

        public void Dispose()
        {
            _writer = null;
            _column = null;
        }
    }
}