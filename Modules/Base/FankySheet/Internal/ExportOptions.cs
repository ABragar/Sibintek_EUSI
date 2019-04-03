using System;
using System.Collections.Generic;
using System.Threading;

namespace FankySheet.Internal
{
    internal class ExportOptions<TData>
    {
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        public bool WriteHeader { get; set; }
        public int RowsCount { get; set; }

        public Func<int, string> SheetNameFunc { get; set; }

        public int Begin { get; set; }

        public List<ExportColumnOptions<TData>> Columns { get; } = new List<ExportColumnOptions<TData>>();
    }
}