using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FankySheet.Internal
{



    public static class SheetUtils
    {
        private static char[] _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private const int _chars = 26;
        public static string GetColumnLetter(uint column, string suffix = null)
        {

            var builder = new StringBuilder();

            while (column >= _chars)
            {

                builder.Insert(0, _letters[column % _chars]);
                column = column / _chars - 1;
            }

            builder.Insert(0, _letters[column]);

            builder.Append(suffix);

            return builder.ToString();


        }
    }


    public class SheetContext : IDisposable
    {
        public SheetContext(ExcelExporter exporter)
        {
            Exporter = exporter;
        }

        public ExcelExporter Exporter { get; private set; }

        public OpenXmlWriter Writer { get; private set; }

        public uint Row { get; private set; }
        public uint Column { get; private set; }





        public string CellReference => SheetUtils.GetColumnLetter(Column, Row.ToString());

        public void Dispose()
        {
            Exporter = null;
        }



        public void Export(Stream stream, IEnumerable<IEnumerable<ISheetWriter>> sheet_writer, IEnumerable<ISheetWriter> columns)
        {



            using (var writer = new OpenXmlPartWriter(stream))
            {
                Writer = writer;
                Row = 0;

                writer.WriteStartDocument();
                writer.WriteStartElement(new Worksheet() { }, null);


                if (columns != null)
                {
                    writer.WriteStartElement(new Columns());
                    Column = 0;

                    foreach (var col in columns)
                    {
                        col?.Write(this);
                        Column++;
                    }

                    writer.WriteEndElement();
                    
                }

                writer.WriteStartElement(new SheetData());


                Row = 1;

                foreach (var row in sheet_writer)
                {

                    bool write_row = false;

                    Column = 0;
                    foreach (var cell in row)
                    {
                        if (cell != null)
                        {
                            if (!write_row)
                            {
                                writer.WriteStartElement(new Row() { RowIndex = Row });
                                write_row = true;
                            }
                            cell.Write(this);
                        }

                        Column++;
                    }

                    if (write_row)
                    {
                        writer.WriteEndElement();
                    }

                    Row++;
                }

                writer.WriteEndElement();
                writer.WriteEndElement();



            }
        }
    }


}