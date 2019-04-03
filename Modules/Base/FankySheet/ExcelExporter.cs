using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FankySheet.Internal;

namespace FankySheet
{
    public class ExcelExporter : BaseExcelExporter
    {
        public ExcelExporter(Stream output_stream) : base(output_stream)
        {

        }


        protected int Export(IEnumerable<IGrouping<string, IEnumerable<ISheetWriter>>> sheets, IReadOnlyCollection<ISheetWriter> columns)
        {
            int count = 0;

            using (var context = new SheetContext(this))
            {
                foreach (var sheet in sheets)
                {
                    count++;

                    var part = CreateWorkSheetPart(sheet.Key);
                    using (var stream = CreateOutputEntryStream(part.Uri.ToString()))
                    {
                        context.Export(stream, sheet, columns);
                    }
                }
            }

            return count;
        }

        public int Export<TData>(IEnumerable<TData> data, Action<IExcelExportDataBuilder<TData>> func)
        {

            var options = new ExportOptions<TData>();
            var builder = new ExcelExportDataBuilder<TData>(options);

            func(builder);


            var cells = data.Select(x => options.Columns.Select(r => r.Cell.GetWriter(this,x))).PartByCount(options.RowsCount);

            var columns = options.Columns.Select(x => x.Column).ToList();
            if (columns.All(x => x == null))
                columns = null;


            if (options.WriteHeader)
            {
                var headers = options.Columns.Select(x => x.Header).ToList();

                cells = cells.Select(x => x.AppendBefore(headers));
            }

            return Export(cells.Select(options.Begin,
                (x, i) => new Part<string, IEnumerable<ISheetWriter>>(options.SheetNameFunc(i), x)), columns);


        }
    }
}