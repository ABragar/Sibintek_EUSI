using System.Data;
using System.Data.OleDb;
using System.IO;

namespace CorpProp.Helpers
{
    /// <summary>
    ///     Инструмент для чтения .dbf файлов в <see cref="DataSet"/>
    /// </summary>
    public static class DbfReader
    {
        /// <summary>
        ///     Чтение .dbf файла в <see cref="DataSet"/>
        /// </summary>
        /// <param name="filePath">Полный абсолютный путь до .dbf файла. Максимальная длина имени файла - 8 символов.</param>
        /// <returns><see cref="DataSet"/> с данными из файла.</returns>
        public static DataSet Invoke(string filePath)
        {
            var connectionString =
                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.GetDirectoryName(filePath) +
                ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
            using (var dbConnection = new OleDbConnection(connectionString))
            {
                var extractCommand = new OleDbCommand("select * from " + Path.GetFileNameWithoutExtension(filePath), dbConnection);
                dbConnection.Open();
                var dataAdapter = new OleDbDataAdapter(extractCommand);
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }
    }
}
