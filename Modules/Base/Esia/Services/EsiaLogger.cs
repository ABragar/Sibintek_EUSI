using System;
using System.IO;

namespace Esia.Services
{
    public class EsiaLogger
    {
        public EsiaLogger(string filePath)
        {
            this.LogFilePath = filePath;
        }

        public enum ByteFormat { Hexadecimal, Base64 }

        public string LogFilePath { get; private set; }
        private static string _singleLine = new String('-', 80);
        private static string _doubleLine = new String('=', 80);

        public void Log(object value, string title = "")
        {
            //Ничего не выводим, если некуда
            if (String.IsNullOrWhiteSpace(LogFilePath))
                return;

            using (var writer = File.AppendText(LogFilePath))
            {
                //Выводим шапку сообщения
                writer.WriteLine("{0:s}", DateTime.Now);
                if (String.IsNullOrEmpty(title) == false)
                    writer.WriteLine(title);
                writer.WriteLine(_singleLine);

                //Выводим само сообщение
                writer.WriteLine(value);
                writer.WriteLine(_doubleLine);
                writer.WriteLine();
            }
        }

        public void LogBytes(byte[] data, string title = "", ByteFormat format = ByteFormat.Base64)
        {
            string text = "<пусто>";

            if (data != null)
            {
                title += String.Format("\nData length: {0}", data.Length);
                switch (format)
                {
                    case ByteFormat.Base64:
                        text = Convert.ToBase64String(data);
                        break;

                    case ByteFormat.Hexadecimal:
                        text = BitConverter.ToString(data).Replace('-', ' ');
                        break;
                }
            }

            Log(text, title.Trim());
        }
    }
}
