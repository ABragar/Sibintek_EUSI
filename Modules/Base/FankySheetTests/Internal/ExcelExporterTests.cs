using Microsoft.VisualStudio.TestTools.UnitTesting;
using FankySheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FankySheet.Internal;

namespace FankySheet.Tests
{
    [TestClass()]
    public class ExcelExporterTests
    {
        [TestMethod()]
        public void ExportTest()
        {


            using (var stream = new TestStream(GetName()))
            {

                using (var exporter = new BaseExcelExporter(stream))
                {
                    // exporter.Export(Enumerable.Range(0,10).Select(x=> ));
                    exporter.Close();
                }



            }
        }

        [TestMethod()]
        public void ExportTest2()
        {

            using (var stream = new FileStream(GetName(), FileMode.Create))
            {
                using (var exporter = new ExcelExporter(stream))
                {

                }
            }
        }

        private static string GetName([CallerMemberName] string ss = null)
        {
            return ss + ".xlsx";
        }

        [TestMethod()]
        public void ExportTest1()
        {

            using (var stream = new FileStream(GetName(), FileMode.Create))
            {
                using (var exporter = new ExcelExporter(stream))
                {
                    exporter.Export(Enumerable.Range(1, 1000), b => b
                    .Sheet(1, x => $"Лист{x}")
                        .RowsCount(20)
                        .WriteHeader(true)
                        .Add(x => x, x => x.Caption("int"))
                        .Add(x => true)
                        .Add(x => (decimal)0.1 * x, x => x.Caption("Hello").Width(100))
                        .Add(x => (double)0.000000000000000000001 * x)
                        .Add(x => DateTime.Now.AddDays(x))
                        .Add(x => "test" + x + "!!!")
                        .AddSharedString(x => "test shared")
                        );
                }
            }

        }
    }

    public class TestStream : FileStream
    {
        public TestStream(string path) : base(path, FileMode.Create, FileAccess.Write)
        {

        }

        public override long Seek(long offset, SeekOrigin loc)
        {
            throw new NotSupportedException();

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();

        }


        public override bool CanSeek => false;

        public override bool CanRead { get; } = false;

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }
    }
}