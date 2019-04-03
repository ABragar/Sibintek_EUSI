using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Tuple = System.Tuple;

namespace FankySheet.Internal
{
    public class BaseExcelExporter : IDisposable
    {

        public BaseExcelExporter(Stream output_stream)
        {
            

            _result_archive = new Lazy<ZipArchive>(() => new ZipArchive(new WrappedStream(output_stream), ZipArchiveMode.Create));

            _temp_stream = new MemoryStream();



            Document = SpreadsheetDocument.Create(_temp_stream, SpreadsheetDocumentType.Workbook, true);

            Workbookpart = Document.AddWorkbookPart();

            Styles = new StylesContext(_stylesheet);



            InitWorkBook();
        }

        protected readonly SpreadsheetDocument Document;
        protected readonly WorkbookPart Workbookpart;

        private readonly MemoryStream _temp_stream;

        private readonly Lazy<ZipArchive> _result_archive;

        private readonly HashSet<string> _entries = new HashSet<string>();

        private void InitWorkBook()
        {

            var workbook = new Workbook();
            Workbookpart.Workbook = workbook;

        }

        private void CreateSheets()
        {
            Workbookpart.Workbook.AppendChild(
            new Sheets(_sheet_parts.Select((x, i) => new Sheet()
            {
                SheetId = (uint)i + 1,
                Id = Workbookpart.GetIdOfPart(x.Item2),
                Name = x.Item1
            })));
        }

        protected WorksheetPart CreateWorkSheetPart(string name)
        {

            var part = Workbookpart.AddNewPart<WorksheetPart>();
            _sheet_parts.Add(Tuple.Create(name, part));


            return part;
        }




        private readonly List<Tuple<string, WorksheetPart>> _sheet_parts = new List<Tuple<string, WorksheetPart>>(1);

        protected void CloseDocument()
        {
            CreateSheets();
            CreateSharedStringsPart();
            CreateStylesPart();
            //???_document.Save();

            Document.Close();
        }


        private void CreateStylesPart()
        {
            if (Styles.Count == 0)
                return;

            var part = Workbookpart.AddNewPart<WorkbookStylesPart>();

            part.Stylesheet = _stylesheet;
        }

        private Stylesheet _stylesheet = new Stylesheet();

        public StylesContext Styles { get; } 




        private readonly List<string> _shared_strings_list = new List<string>();

        private readonly Dictionary<string, int> _shared_strings = new Dictionary<string, int>();

        protected internal int CreateSharedString(string str)
        {
            int index;

            if (_shared_strings.TryGetValue(str, out index))
            {
                return index;
            }
            index = _shared_strings_list.Count;
            _shared_strings_list.Add(str);
            _shared_strings.Add(str, index);

            return index;
        }

        private void CreateSharedStringsPart()
        {
            if (_shared_strings.Count == 0)
                return;


            var part = Workbookpart.AddNewPart<SharedStringTablePart>();

            using (var stream = CreateOutputEntryStream(part.Uri.ToString()))
            {
                using (var writer = OpenXmlWriter.Create(stream))
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement(new SharedStringTable());
                    foreach (var str in _shared_strings_list)
                    {
                        writer.WriteStartElement(new SharedStringItem());
                        writer.WriteElement(new Text(str));
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }


            }

        }

        private void CopyDocumentToOutput()
        {

            using (var temp_zip_arhive = new ZipArchive(_temp_stream, ZipArchiveMode.Read, true))
            {

                foreach (var temp_entry in temp_zip_arhive.Entries)
                {
                    var path = temp_entry.FullName;

                    if (!_entries.Contains(path))
                    {
                        using (var entry_stream = temp_entry.Open())
                        {

                            using (var output = CreateOutputEntryStream(path))
                            {
                                entry_stream.CopyTo(output);
                            }

                        }

                    }
                }
            }
        }

        protected Stream CreateOutputEntryStream(string path)
        {
            CheckState();
            path = path.StartsWith("/") ? path.Substring(1) : path;

            var new_entry = _result_archive.Value.CreateEntry(path, CompressionLevel.Fastest);

            _entries.Add(path);

            return new_entry.Open();
        }

        protected void CheckState()
        {
            if (_closed)
                throw new InvalidOperationException();
        }

        private bool _closed;

        public void Close()
        {
            if (!_closed)
            {
                CloseDocument();
                CopyDocumentToOutput();
                _result_archive.Value.Dispose();
                _closed = true;
            }
        }

        public void Dispose()
        {
            if (_result_archive.IsValueCreated)
            {

                Close();

            }
            Document.Dispose();
        }


     
    }
}