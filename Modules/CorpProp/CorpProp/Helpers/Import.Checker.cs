using Base;
using Base.DAL;
using Base.Service;
using Base.Service.Log;
using Base.UI.Service;
using CorpProp.Common;
using CorpProp.Entities.Base;
using CorpProp.Entities.Common;
using CorpProp.Entities.Document;
using CorpProp.Entities.DocumentFlow;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using CorpProp.Extentions;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CorpProp
{
    /// <summary>
    /// Предоставляет методы для различных проверок импортируемого файла Excel.
    /// </summary>
    public class ImportChecker : IExcelImportChecker
    {
        private static string connectionString => ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;


        /// <summary>
        /// Проверка данных в таблице.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="table">Таблица.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="history">Запись об импорте.</param>
        public virtual void StartDataCheck(IUnitOfWork uofw, IUnitOfWork histUow, DataTable table, Type type,
            ref ImportHistory history, bool dictCode = false)
        {
            CheckContacts(uofw, table, ref history); 
            CheckRequiredFields(table, type, ref history);
            CheckFieldData(table, type, ref history, uofw, dictCode);
        }

        public virtual string FormatConfirmImportMessage(List<string> fileDescriptions)
        {
            throw new NotImplementedException();
           
        }
        public virtual CheckImportResult CheckVersionImport(IUiFasade uiFacade, IExcelDataReader reader, ITransactionUnitOfWork uofw,
          StreamReader stream, string fileName)
        {
           var tables = reader.GetVisbleTables();
            DataTable entryTable = tables[0];
            CheckImportResult checkResult = null;
            string mnemonic = ImportHelper.FindSystemName(entryTable);
            string strErr = "";
            if (string.IsNullOrEmpty(mnemonic))
                strErr = "Не удалось определить тип импортируемого объекта. Код ошибки: Err1001.";
            try
            {
                var config = uiFacade.GetViewModelConfig(mnemonic);
                if (config.ServiceType.GetInterfaces().Contains(typeof(IConfirmImportChecker)))
                {
                    var checkService = config.GetService<IConfirmImportChecker>();
                    checkResult = checkService.CheckConfirmResult(uofw, fileName, reader, entryTable);
                    if (checkResult != null)
                        checkResult.Checker = checkService;
                }
            }
            catch (Exception ex)
            {
                string strAddErr = "";
                strAddErr = string.Format("Файл импорта не соответствует утвержденному формату. \r\n {0} \r\n Текст ошибки: {1}", strErr,ex.Message);
                //ex = new Exception(strAddErr);
                throw new Exception(strAddErr);
                //return checkResult;
            }


            return checkResult;
        }

        /// <summary>
        /// Проверка заполнения обязательных полей.
        /// </summary>
        /// <param name="table">Таблица.</param>
        /// <param name="history">Запись об импорте.</param>
        public static void CheckRequiredFields(DataTable table, Type type, ref ImportHistory history)
        {
            //Коллекция обязательных полей.
            Dictionary<string, string> requiredFieldNames = new Dictionary<string, string>();
            int startDataRow = ImportHelper.GetRowStartIndex(table);            
            int columnsSystemNameStringRow = ImportHelper.GetRowSystemNameRow(table);
            int columnsUserNameStringRow = ImportHelper.GetRowUserNameRow(table);
            int requiredRowNum = startDataRow - 3;
            try
            {
                //Заполняем коллекцию обязательными полями.
                for (var c = 1; c < table.Columns.Count; c++)
                {
                    bool isRequired = (!String.IsNullOrEmpty(table.Rows[requiredRowNum][c].ToString()) && table.Rows[requiredRowNum][c].ToString().ToLower() == "да") ? true : false;
                    if (isRequired)
                    {
                        requiredFieldNames.Add(table.Columns[c].ColumnName, table.Rows[columnsUserNameStringRow][c].ToString());
                    }
                }

                if (requiredFieldNames.Count > 0)
                {
                    //int columnIndex = 1;
                    int dataRowNum = ImportHelper.GetRowStartIndex(table); //type == typeof(Society) ? 10 : 9;
                    DataTable dt = table.AsEnumerable().Skip(dataRowNum).CopyToDataTable();
                    //перечитываем коллекцию и ищем не заполненные ячейки
                    foreach (KeyValuePair<string, string> column in requiredFieldNames)
                    {
                        var foundRows = dt.Select($"{column.Key} IS NULL").ToList();
                        if (foundRows != null && foundRows.Count() > 0)
                        {
                            for (var r = 0; r < foundRows.Count; r++)
                            {
                                history.ImportErrorLogs.AddError(
                                    (dt.Rows.IndexOf(foundRows[r])+ startDataRow+1)
                                    , table.Columns.IndexOf(column.Key)// columnIndex
                                    , table.Rows[columnsUserNameStringRow][column.Key].ToString()
                                    , ErrorType.Required);                                   
                            }
                        }
                        //columnIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        /// <summary>
        /// Проверка типа данных и наличия связанных объектов.
        /// </summary>
        /// <param name="table">Таблица.</param>
        /// <param name="history">Запись журнала импорта.</param>
        /// <param name="uofw">Сессия.</param>
        public static void CheckFieldData(
            DataTable table
            , Type type
            , ref ImportHistory history
            , IUnitOfWork uofw
             , bool dictCode = false)
        {
            try
            {
                var logs = new System.Collections.Concurrent.ConcurrentBag<ImportErrorLog>();
                Dictionary<string, int> dErrColum = new Dictionary<string, int>();
                var yesValues = new List<string>() { "да", "истина", "yes", "true", "1" };
                var noValues = new List<string>() { "нет", "ложь", "no", "false", "0" };
                var boolValues = new List<string>() { "1", "0" };

                int startDataRow = ImportHelper.GetRowStartIndex(table);
                int columnsSystemNameStringRow = ImportHelper.GetRowSystemNameRow(table);
                int columnsUserNameStringRow = ImportHelper.GetRowUserNameRow(table);
                int dataTypeRowNum = columnsSystemNameStringRow + 1;

                for (int c = 1; c < table.Columns.Count; c++)
                {
                    string tColumName = table.Rows[columnsSystemNameStringRow][c].ToString();
                    if (!string.IsNullOrWhiteSpace(tColumName))
                    {
                        if (!dErrColum.Keys.Contains(tColumName))
                            dErrColum.Add(tColumName, c);
                        else
                        {
                            history.ImportErrorLogs
                                     .AddError(null, c, table.Rows[columnsUserNameStringRow][c].ToString(), "Найдено дублирующее поле: " + tColumName, ErrorType.System);
                            continue;
                        }
                    }

                    //модификация данных
                    table.Rows.Cast<DataRow>()
                        .Where(row => table.Rows.IndexOf(row) >= startDataRow)
                        .Where(r => String.IsNullOrWhiteSpace(r[c]?.ToString()))
                        .ToList().ForEach(r => {
                            r[c] = System.DBNull.Value;
                        });
                    table.AcceptChanges();

                    string typeName = table.Rows[dataTypeRowNum][c].ToString().ToLower().Trim();
                    var dataRows = table.Rows.Cast<DataRow>()
                        .Where(row => table.Rows.IndexOf(row) >= startDataRow)
                        .Where(r => !String.IsNullOrWhiteSpace(r[c]?.ToString()))
                        .ToList();                   

                    switch (typeName)
                    {
                        case "bool":
                            //модификация данных
                            table.Rows.Cast<DataRow>()
                                .Where(row => table.Rows.IndexOf(row) >= startDataRow)
                                .Where(r => !String.IsNullOrWhiteSpace(r[c]?.ToString()))
                                .ToList().ForEach(r => {
                                    if (yesValues.Contains(r[c].ToString().Trim().ToLower()))
                                        r[c] = 1;
                                    else if (noValues.Contains(r[c].ToString().Trim().ToLower()))
                                        r[c] = 0;
                                });
                            table.AcceptChanges();

                            var errRows = table.Rows.Cast<DataRow>()
                                            .Where(row => table.Rows.IndexOf(row) >= startDataRow)
                                            .Where(r => !String.IsNullOrWhiteSpace(r[c]?.ToString()))
                                            .Where(r => !boolValues.Contains(r[c].ToString()));                                                       

                            errRows.ToList()
                            .ForEach(r => {                                
                                logs.Add(NewError(table.Rows.IndexOf(r)+1, c
                                , table.Rows[columnsUserNameStringRow][c].ToString(), ErrorType.Type));

                            });
                            break;
                        case "datetime":                           
                            var errRowsdt = dataRows
                                           .Where(r => r[c].ToString().GetDate() == null);
                            errRowsdt.ToList()
                            .ForEach(r => {
                                logs.Add(NewError(table.Rows.IndexOf(r) + 1, c
                                , table.Rows[columnsUserNameStringRow][c].ToString(), ErrorType.Type));
                            });
                            table.AcceptChanges();
                            break;
                        case "decimal":                                                   
                            dataRows
                                .ForEach(r => 
                                {
                                    decimal de = 0m;
                                    var value = r[c].ToString().Replace('.', ',').Trim();
                                    if (!Decimal.TryParse(
                                    value,
                                        System.Globalization.NumberStyles.AllowLeadingSign |
                                        System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowExponent,
                                        System.Globalization.CultureInfo.CurrentCulture,
                                        out de))
                                    {
                                        logs.Add(NewError(table.Rows.IndexOf(r) + 1, c
                                        , table.Rows[columnsUserNameStringRow][c].ToString(), ErrorType.Type));
                                    }
                                    else
                                    {
                                        table.Rows[table.Rows.IndexOf(r)][c] = de;
                                    }
                                });
                            table.AcceptChanges();
                            break;
                        case "int": 
                            dataRows
                                .ForEach(r =>
                                {
                                    int i = 0;                                    
                                    if (!int.TryParse(
                                        r[c].ToString().Trim(),
                                        out i))
                                    {
                                        logs.Add(NewError(table.Rows.IndexOf(r) + 1, c
                                        , table.Rows[columnsUserNameStringRow][c].ToString(), ErrorType.Type));
                                    }
                                    else
                                    {
                                        table.Rows[table.Rows.IndexOf(r)][c] = i;
                                    }
                                });
                            table.AcceptChanges();
                            break;
                        case "string":
                            continue;
                        default:
                            var propType = TypesHelper.GetTypeByName(typeName?.ToLower());
                            if (propType != null && propType.IsSubclassOf(typeof(BaseObject)))
                            {
                                //проверка навигационных св-в переведена на sql
                                CheckNavigationField(dataRows, table.Columns[c].ColumnName, table.Rows[columnsUserNameStringRow][c].ToString()
                                                    ,c , propType, ref history);
                            }
                            break;
                    }
                }

                if (logs.Any())
                    foreach (var item in logs)
                    {
                        history.ImportErrorLogs.Add(item);
                    }
                
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }

        private static void CheckNavigationField(
            IEnumerable<DataRow> dataRows
            , string sysColumnName
            , string userColumnName
            , int columnIndex
            , Type propType                      
            , ref ImportHistory history)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand("", conn))
                    {
                        command.CommandTimeout = 600000;
                        var sql = new StringBuilder();
                        sql.AppendLine();
                        sql.AppendLine($"IF OBJECT_ID(N'#Rows') IS NOT NULL DROP TABLE #Rows");
                        sql.AppendLine($"CREATE TABLE #Rows (" +
                               $"rowNumb INT NOT NULL IDENTITY(1,1), " +
                               $"text NVARCHAR(max) " +
                               $")");
                        command.CommandText = sql.ToString();
                        command.ExecuteNonQuery();

                        sql = new StringBuilder();
                        sql.AppendLine($"SELECT rowNumb");
                        sql.AppendLine($"FROM #Rows AS s");
                        sql.AppendLine($"WHERE NOT EXISTS");
                        sql.AppendLine($"({GetCheckFieldCondition(propType)})");                       
                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                        {
                            bulkcopy.ColumnMappings.Add(sysColumnName, "text");
                            bulkcopy.BulkCopyTimeout = 600000;
                            bulkcopy.DestinationTableName = $"#Rows";
                            bulkcopy.WriteToServer(dataRows.ToArray());
                            bulkcopy.Close();
                        }
                        command.CommandText = sql.ToString();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.HasRows && reader.GetName(0) == "rowNumb")
                                {
                                    var rowNum = (!reader.IsDBNull(0)) ? reader.GetInt32(0) : 0;
                                    history.ImportErrorLogs
                                     .AddError(rowNum, columnIndex, userColumnName, ErrorType.ObjectNotFound);
                                }
                            }
                        }                     
                    }
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }            
        }

        private static string GetTableName(Type value)
        {
            return $"[{value.Namespace.Replace(".Entities", String.Empty)}].[{value.Name}] ";
        }


        private static string GetCheckFieldCondition(Type type)
        {
            var sql = new StringBuilder();           
            sql.AppendLine($"SELECT *");

            if (Type.Equals(type, typeof(FileCard)) || type.IsSubclassOf(typeof(FileCard)))
                sql.AppendLine($"FROM {GetTableName(type)} AS t WHERE t.Name = s.text AND t.Hidden = 0 AND t.IsHistory = 0");

            else if (Type.Equals(type, typeof(Society)) || type.IsSubclassOf(typeof(Society)))
                sql.AppendLine($"FROM {GetTableName(type)} AS t WHERE t.IDEUP = s.text AND t.Hidden = 0 AND t.IsHistory = 0");

            //TODO: t.INN = ? OR t.KPP = ?
            else if (Type.Equals(type, typeof(Subject)) || type.IsSubclassOf(typeof(Subject)))
            {
                //sql.AppendLine($"FROM {GetTableName(type)} AS t WHERE (t.SDP = s.text OR t.INN = s.text OR t.KPP = s.text) AND t.Hidden = 0 AND t.IsHistory = 0");
                if (!Type.Equals(type, typeof(Subject)))
                {
                    sql.AppendLine($"FROM {GetTableName(type.BaseType)} AS t ");
                    sql.AppendLine($"inner join {GetTableName(type)} as tRef on t.ID=tRef.ID");
                }
                else
                    sql.AppendLine($"FROM {GetTableName(type)} AS t ");
                sql.AppendLine($"left outer join (SELECT top 1 split.a.value('.', 'VARCHAR(100)') AS Val FROM (SELECT Cast ('<M>' + Replace(Replace(isnull(s.text,''),' ',''), '/', '</M><M>')+ '</M>' AS XML) AS Data) AS A CROSS apply data.nodes ('/M') AS Split(a)) as tValue on t.sdp=tValue.Val or t.inn=tValue.Val or t.KSK=tValue.Val");
                sql.AppendLine($"left outer join (select id as 'subjID', INN,KPP from [CorpProp.Subject].Subject) as tValue2 on ((isnull(replace(tValue2.inn,' ',''), '') + '/' + isnull(replace(tValue2.kpp,' ',''),''))=Replace(isnull(s.text,''),' ','')) and tValue2.subjID=t.ID");
                sql.AppendLine($"WHERE (tValue2.subjID is not null or tValue.Val is not null)  and tValue.Val != '' and  t.Hidden = 0 AND t.IsHistory = 0");
            }

            else if (Type.Equals(type, typeof(SibUser)) || type.IsSubclassOf(typeof(SibUser)))
                sql.AppendLine($"FROM {GetTableName(type)} AS tt " +
                        $"INNER JOIN  [Security].BaseProfile AS t ON tt.ID = t.ID " +
                        $"WHERE s.text = (t.LastName + N' '+t.FirstName + N' '+ t.MiddleName) AND t.Hidden = 0");

            else if (Type.Equals(type, typeof(SibDeal)) || type.IsSubclassOf(typeof(SibDeal)))
                sql.AppendLine($"FROM {GetTableName(type)} AS tt " +
                       $"INNER JOIN  {GetTableName(typeof(CorpProp.Entities.DocumentFlow.Doc))} AS t ON tt.ID = t.ID " +
                       $"WHERE t.Number = s.text AND t.Hidden = 0 AND t.IsHistory = 0");

            else if (Type.Equals(type, typeof(Entities.Law.Right)))
                sql.AppendLine($"t.RegNumber = s.text AND t.Hidden = 0 AND t.IsHistory = 0");

            else if (type.IsSubclassOf(typeof(DictObject)))
                sql.AppendLine($"FROM {GetTableName(type)} AS tt " +
                       $"INNER JOIN  {GetTableName(typeof(CorpProp.Entities.Base.DictObject))} AS t ON tt.ID = t.ID " +
                       $"WHERE (t.Code = s.text OR t.Name = s.text OR t.ExternalID = s.text) AND t.Hidden = 0 AND t.IsHistory = 0");

            else
                sql.AppendLine($"FROM {GetTableName(type)} AS t " +
                       "WHERE t.ID = TRY_CAST(s.text AS int)");
            
            return sql.ToString();
        }

        private static ImportErrorLog NewError(
             int? rowNumber
           , int? columnNumber
           , string propetyName
           , ErrorType errType
           , string sheet = ""
           )
        {
           return new ImportErrorLog()
           {
                MessageDate = DateTime.Now,
                RowNumber = rowNumber,
                ColumnNumber = columnNumber,
                PropetyName = propetyName,
                ErrorText = ImportExtention.GetErrorTypeName(errType),
                ErrorType = ImportExtention.GetErrorTypeName(errType),
                Sheet = sheet
           };
        }

        /// <summary>
        /// Проверка наличия объекта в системе.
        /// </summary>
        /// <param name="uofw">Сессия.</param>
        /// <param name="value">Значение для поиска.</param>
        /// <param name="typeName">Имя типа объекта.</param>
        /// <param name="history">Запись журнала импорта.</param>
        /// <returns>True - если найден.</returns>
        public static bool CheckSystemObjectExist(
            IUnitOfWork uofw
            , string value
            , string typeName
            , ref ImportHistory history
            , bool dictCode = false
            )
        {
            bool resultValue = false;
            object obj = null;
            Type type = null;
            try
            {
                var err = "";
                type = TypesHelper.GetTypeByName(typeName.ToLower());

                if (type == null || String.IsNullOrEmpty(value))
                    return false;

                if (Type.Equals(type, typeof(FileCard)) || type.IsSubclassOf(typeof(FileCard)))
                {
                    string name = value.ToString();
                    var fileCardByName = uofw.GetRepository<FileCard>().FilterAsNoTracking(x => x.Name == name).FirstOrDefault();
                    if (fileCardByName == null)
                    {
                        int strID = 0;
                        int.TryParse(value, out strID);
                        if (strID != 0)
                        {
                            return uofw.GetRepository<FileCard>().Filter(x => x.ID == strID).FirstOrDefault() != null;
                        }
                        else
                            return false;
                    }
                    return fileCardByName != null;

                }

                if (Type.Equals(type, typeof(Society)) || type.IsSubclassOf(typeof(Society)))
                {
                    string idEup = ImportHelper.GetIDEUP(value);
                    if (!string.IsNullOrWhiteSpace(idEup)) idEup = idEup.Trim();
                    return uofw.GetRepository<Society>().FilterAsNoTracking(x => x.IDEUP == idEup).FirstOrDefault() != null;
                }

                if (Type.Equals(type, typeof(Subject)) || type.IsSubclassOf(typeof(Subject)))
                {
                    var val = ImportHelper.GetSubjectBySDP(uofw, value);
                    if (val == null)
                        val = ImportHelper.GetSubjectByINNAndKPP(uofw, value);
                    return val != null;
                }               

                if (Type.Equals(type, typeof(SibUser)) || type.IsSubclassOf(typeof(SibUser)))
                    return uofw.GetRepository<SibUser>().FilterAsNoTracking(x => (x.LastName + " " + x.FirstName + " " + x.MiddleName) == value && !x.Hidden).FirstOrDefault() != null;

                if (Type.Equals(type, typeof(SibDeal)) || type.IsSubclassOf(typeof(SibDeal)))
                    return uofw.GetRepository<SibDeal>().FilterAsNoTracking(x => x.Number == value).FirstOrDefault() != null;

        if (Type.Equals(type, typeof(Entities.Law.Right)))
                {
                    string numb = value.ToString();
                    var rightRegNumber = uofw.GetRepository<Entities.Law.Right>().FilterAsNoTracking(x => x.RegNumber == numb).FirstOrDefault();
                    if (rightRegNumber == null)
                    {
                        int strID = 0;
                        int.TryParse(value, out strID);
                        if (strID != 0)
                        {
                            return uofw.GetRepository<Entities.Law.Right>().Filter(x => x.ID == strID).FirstOrDefault() != null;
                        }
                        else
                            return false;
                    }
                    return rightRegNumber != null;

                }

                if (Type.Equals(type, typeof(DictObject)) || type.IsSubclassOf(typeof(DictObject)))
                {
                    if (dictCode || (Type.Equals(type, typeof(Consolidation)) || type.IsSubclassOf(typeof(Consolidation))))
                        return ImportHelper.GetDictByCode(uofw, type, value, ref err) != null;

                    if (dictCode || (Type.Equals(type, typeof(PositionConsolidation)) || type.IsSubclassOf(typeof(PositionConsolidation))))
                        return ImportHelper.GetDictByCode(uofw, type, value, ref err) != null;

                    
                    //Если не нашли по имени то пробуем по коду
                    resultValue = ImportHelper.GetDictByName(uofw, type, value, ref err) != null;
                    if (!resultValue)
                        resultValue = ImportHelper.GetDictByCode(uofw, type, value, ref err) != null;

                    //Если не нашли по имени то пробуем по ExternalID
                    if (!resultValue)
                        resultValue = ImportHelper.GetDictByExternalID(uofw, type, value, ref err) != null;
                        
                    return resultValue;
                }
                else
                    return resultValue;
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);                   
                return obj != null;
            }
        }


        private static bool CheckImportTemplate(IUnitOfWork uofw, DataTable table, ref ImportHistory history)
        {
            bool res = false;
            ImportTemplate temp = FindTemplate(uofw, table);
            if (temp == null)
            {
                history.ImportErrorLogs.AddError(null, null, null
                    , "В Системе не найден активный шаблон импорта для данного файла."
                    , ErrorType.System);
                return res;
            }
            //TODO: проверка файла на соответствие шаблону импорта

            return res;

        }

        /// <summary>
        /// Проверка заполнения контактных данных.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="table">Таблица.</param>
        /// <param name="history">Лог.</param>
        public static void CheckContacts(IUnitOfWork unitOfWork, DataTable table, ref ImportHistory history)
        {
            const string _emailTemplate = @"^[\w-]*@[\w-]*.[\w]*";
            var arContactRows = GetArrContacts(table);                
                
            try
            {
                if (arContactRows.Count > 0)
                {
                    history.ContactName = arContactRows.ContainsKey("fio") ? table.Rows[arContactRows["fio"]][1].ToString() : "";
                    history.ContactPhone = arContactRows.ContainsKey("phone") ? table.Rows[arContactRows["phone"]][1].ToString() : "";

                    var value = arContactRows.ContainsKey("email") ? table.Rows[arContactRows["email"]][1].ToString() : "";
                    if (!String.IsNullOrEmpty(value) && !Regex.IsMatch(value, _emailTemplate))
                        throw new ImportException("Неверный формат E-mail. Допускается указание только одного адреса.");
                    history.ContactEmail = value;
                }             
            }
            catch(ImportException ex)
            {
                history.ImportErrorLogs.AddError(null, null, ex.Message, ErrorType.Contact);
            }
            catch(Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }


        private static IDictionary<string, int> GetArrContacts(DataTable table)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();

            int count = (table.Rows.Count > 12) ? 12 : table.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                DataRow row = table.Rows[i];  
                if (row.ItemArray[0].ToString().ToLower().Contains("фио ответственного"))
                    list.Add("fio",i);
                if (row.ItemArray[0].ToString().ToLower().Contains("контактный телефон"))
                    list.Add("phone", i);
                if (row.ItemArray[0].ToString().ToLower().Contains("email"))
                    list.Add("email", i);
            }           
            return list;
        }

        private static ImportTemplate FindTemplate(IUnitOfWork uofw, DataTable table)
        {          

            string mnemonic = table.Rows[2][1].ToString();
            string name = table.Rows[0][1].ToString();
            string vers = table.Rows[1][1].ToString();

            var temp = uofw.GetRepository<ImportTemplate>()
                .Filter(x => x.Hidden == false
                && x.Mnemonic == mnemonic
                && x.Name == name
                && x.Version == vers
                && x.Active == true
                ).FirstOrDefault();
            
            return temp;
        }

        /// <summary>
        /// Парсинг наименования файла формата CODE_YYYY_MM_dd.
        /// </summary>
        /// <param name="serv">Сервис истории импорта.</param>
        /// <param name="histUow">Сессия истории импорта.</param>
        /// <param name="history">История имопрта</param>
        /// <param name="isRequired">Признак обязательности проверки соответсвия формата.</param>
        /// <returns>True если наименование файла соответствует формату, иначе  False.</returns>
        public virtual bool ParseFileNameDefult(
             Services.Import.IImportHistoryService serv
            ,IUnitOfWork histUow
            , ref ImportHistory history
            , bool isRequired = false)
        {
            var errText = ErrorTypeName.InvalidFileNameFormat;

            if (Regex.IsMatch(history.FileName, ImportHelper._FILE_NAME_TEMPLATE_CODE_YYYY_MM_dd))
            {
                var arFileName = history.FileName.Split('_');
                if (arFileName.Length > 2)
                {                    
                    var period = ImportHelper.GetPeriod_YYYY_MM_DD(arFileName[1], arFileName[2], arFileName[3]);
                    if (period == null && isRequired)
                        history.ImportErrorLogs.AddError(errText + "Некорректный формат даты.");
                    else
                        history.Period = period;

                    string code = ImportHelper.GetIDEUP(arFileName[0]);
                    serv.InitSociety(histUow, code, isRequired, ref history);
                    
                }
                else if (isRequired)
                    history.ImportErrorLogs.AddError(errText);
            }
            else if (isRequired)
                history.ImportErrorLogs.AddError(errText);

            return
                !history
                .ImportErrorLogs
                .Where(log => log.ErrorText.Contains(errText))
                .Any();
        }
    }
}
