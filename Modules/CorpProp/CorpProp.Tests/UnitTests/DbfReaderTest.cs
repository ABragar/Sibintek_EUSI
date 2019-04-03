using System;
using System.IO;
using CorpProp.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CorpProp.Tests.UnitTests
{
    [TestClass]
    public class DbfReaderTest
    {
        [TestMethod]
        public void InvokeTest()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "UnitTests\\Resources\\Euro.dbf");

            var dataSet = DbfReader.Invoke(filePath);

            Assert.IsTrue(dataSet.IsInitialized, "Инициализцаия DataSet не была завершена.");

            Assert.IsFalse(dataSet.HasErrors, "Файл .dbf был загружен с ошибками.");

            const string schemaError = "Схема загруженной таблицы не совпадает с файлом.";
            Assert.AreEqual(1, dataSet.Tables.Count, schemaError);
            Assert.AreEqual(5,dataSet.Tables[0].Columns.Count, schemaError);
            Assert.AreEqual("DATA",dataSet.Tables[0].Columns[0].Caption, schemaError);
            Assert.AreEqual("NOMINAL", dataSet.Tables[0].Columns[1].Caption, schemaError);
            Assert.AreEqual("CURS", dataSet.Tables[0].Columns[2].Caption, schemaError);
            Assert.AreEqual("NUM_CODE", dataSet.Tables[0].Columns[3].Caption, schemaError);
            Assert.AreEqual("CHAR_CODE", dataSet.Tables[0].Columns[4].Caption, schemaError);

            const string dataError = "Данные загруженной таблицы не совпадают с файлом.";
            Assert.AreEqual(4618, dataSet.Tables[0].Rows.Count, dataError);
            Assert.AreEqual(new DateTime(2002,07,19), dataSet.Tables[0].Rows[884].ItemArray[0], dataError);
            Assert.AreEqual(1.0, dataSet.Tables[0].Rows[1886].ItemArray[1], dataError);
            Assert.AreEqual(39.7028, dataSet.Tables[0].Rows[2799].ItemArray[2], dataError);
            Assert.AreEqual(978.0, dataSet.Tables[0].Rows[3505].ItemArray[3], dataError);
            Assert.AreEqual("EUR", dataSet.Tables[0].Rows[4144].ItemArray[4], dataError);
        }
    }
}
