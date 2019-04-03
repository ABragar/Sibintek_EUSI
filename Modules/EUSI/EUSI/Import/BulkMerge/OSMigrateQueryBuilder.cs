using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Accounting;
using CorpProp.Services.Import.BulkMerge;
using EUSI.Entities.Accounting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Import.BulkMerge
{
    public class OSMigrateQueryBuilder : QueryBuilder
    {
        OBUVersionControl _version;
        string errTable = $"@errorTable";
        string rowProcessed = $"rowProcessed";
        string oidEstate = "oidEstate";
        string isNewOS = "isNewOS";
        string isNewEstate = "isNewEstate";
        string typeEstate = "typeEstate";

        string startPeriod = null;
        string endPeriod = null;

        public OSMigrateQueryBuilder(Dictionary<string, string> colsNameMapping, Type type, OBUVersionControl version) : base(colsNameMapping, type)
        {
            _version = version;

            TempTableCustomColumns = new Dictionary<string, Type>()
            {                
                 //строка обработана = ощибка или обновлена
                 { rowProcessed, typeof(bool) },                 
                 //Оid объекта имущества
                 { oidEstate, typeof(Guid) },                 
                 //новый ОС на создание
                 { isNewOS, typeof(bool) },  
                 //новый ОИ на создание
                 { isNewEstate, typeof(bool) },
                 //Указывает тип ОИ
                 { typeEstate, typeof(string) },
            };

            startPeriod = $"'{_version.StartPeriod.ToString("yyyy-MM-dd")}'";
            endPeriod = $"'{_version.EndPeriod.ToString("yyyy-MM-dd")}'";
        }


        protected override string GetCreateTableScript()
        {
            var tableName = GetTempTableName();
            var properties = MainType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite).ToArray();
            var createScript = new StringBuilder();
            createScript.AppendLine($"IF OBJECT_ID(N'tempdb..{tableName}') IS NOT NULL DROP TABLE {tableName} \r\n " +
                $"CREATE TABLE {tableName} ({GetTempTableColumnsSpecification(properties)} )");
            return createScript.ToString();
        }

        public override int Merge(SqlCommand command, ref ImportHistory history)
        {

            command.CommandText = BuildMergeQuery(history);
            var reader = command.ExecuteReader();
            int totalRecords = 0;

            do
            {
                reader.Read();
                var resultName = reader.GetName(0);
                switch (resultName)
                {
                    case "TotalRecords":
                        totalRecords = reader.HasRows ? reader.GetInt32(0) : 0;
                        break;
                    case "errorRowNum":
                        {
                            do
                            {
                                if (reader.HasRows)
                                {
                                    int? rowNum = null;
                                    if (!reader.IsDBNull(0))
                                        rowNum = reader.GetInt32(0);
                                    var errorText = (!reader.IsDBNull(1)) ? reader.GetString(1) : "";
                                    var errorCode = (!reader.IsDBNull(2)) ? reader.GetString(2) : "";
                                    var invNumber = (!reader.IsDBNull(3)) ? reader.GetString(3) : "";
                                    var eusiNumber = (!reader.IsDBNull(4)) ? reader.GetString(4) : "";
                                    var error = new ImportErrorLog()
                                    {
                                        ImportHistory = history,
                                        RowNumber = rowNum,
                                        ErrorText = errorText,
                                        ErrorCode = errorCode,
                                        ErrorType = ImportExtention.GetErrorTypeName(ErrorType.System),
                                        InventoryNumber = invNumber,
                                        EusiNumber = eusiNumber
                                    };
                                    history.ImportErrorLogs.Add(error);
                                }

                            } while (reader.Read());

                            break;
                        }
                }
            } while (reader.NextResult());
            reader.Close();


            return totalRecords;
        }

        public override string BuildMergeQuery(ImportHistory history)
        {
            bool errExistsToImportHistory = (history.ImportErrorLogs.Count > 0);

            int errExists = (!errExistsToImportHistory) ? 0 : 1;

            var script = new StringBuilder();
            script.AppendLine($";DISABLE TRIGGER [{GetTableSchemaName(typeof(AccountingObjectTbl))}].[TR_AccountingCalculatedFild_CostEstate]  ON {GetTableName(typeof(AccountingObjectTbl))}");
            //Отключение триггера для Estate
            script.AppendLine($" ;DISABLE TRIGGER [{GetTableSchemaName(typeof(Estate))}].[TR_EstateCalculatedField_Ins]  ON {GetTableName(typeof(Estate))}");
            script.AppendLine($" ;DISABLE TRIGGER [{GetTableSchemaName(typeof(InventoryObject))}].[TR_EstateCalculatedField_Count]  ON {GetTableName(typeof(InventoryObject))}");
            script.AppendLine($" ;DISABLE TRIGGER [{GetTableSchemaName(typeof(InventoryObject))}].[TR_InventoryObject_UpdateCost]  ON {GetTableName(typeof(InventoryObject))}");
            //Переменные
            script.AppendLine($" declare @errExistsToImportHistory int = {errExists};");
            script.AppendLine($" ");
            script.AppendLine($"DECLARE @StartPeriod DATETIME = {startPeriod}");
            script.AppendLine($"DECLARE @EndPeriod DATETIME = {endPeriod}");
            script.AppendLine($"DECLARE {errTable} TABLE(" +
                $"rowNumb INT, " +
                $"text NVARCHAR(max), " +
                $"errResultCode NVARCHAR(max), " +
                $"errCode NVARCHAR(max)," +
                $"invNumber NVARCHAR(max)," +
                $"eusiNumber NVARCHAR(max)" +
                $")");

            script.AppendLine(
               $"DECLARE @OSActions TABLE(" +
               $"Change VARCHAR(20), " +
               $"ID int, " +
               $"Oid uniqueidentifier NULL)");

            script.AppendLine($"if(@errExistsToImportHistory=1)");
            script.AppendLine($"BEGIN ");
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"  N'' as 'rowNumb'");
            script.AppendLine($", N'При анализе файли шаблона импорта были зафиксированы ошибки' as 'Text'");
            script.AppendLine($", N'ERR_MigrateOS' as 'errResultCode'");
            script.AppendLine($", N'errExistsToImportHistory' AS 'errCode'");
            script.AppendLine($", N'' as 'invNumber'");
            script.AppendLine($", N'' as 'eusiNumber';");
            script.AppendLine($"END ");

            //выполнение проверки на дубликаты данных в файле импорта по составному ключу "Инвентарный номер" : "Номер записи в учётной системе" : "БЕ"
            script.AppendLine($"INSERT INTO @errorTable");
            script.AppendLine($"SELECT err_target.rowNumb, N'Обнаружены дублирующие данные по составному ключу \"Инвентарный номер\" : \"Номер записи в учётной системе\" : \"БЕ\", а именно (\"'+err_target.[InventoryNumber]+'\" : \"'+err_target.[ExternalID]+'\" : \"'+err_target.[Consolidation]+'\")', N'ERR_OSID', N'IM00', err_target.[InventoryNumber], err_target.[EUSINumber]");
            script.AppendLine($"FROM {GetTempTableName()} as err_target with (nolock) ");
            script.AppendLine($"inner join(");
            script.AppendLine($"select target.rowid as 'rowid', count(target.rowid) as 'cntERR' from {GetTempTableName()} AS target  with (nolock) ");
            script.AppendLine($"inner join  {GetTableName(typeof(AccountingObjectExtView))} AS source  with(nolock)");
            script.AppendLine($"ON source.Hidden = 0 AND source.IsHistory = 0 AND target.Oid IS NULL  AND target.EUSINumber IS NULL  AND target.ExternalID IS NOT NULL  AND target.rowProcessed <> 1 AND source.ConsolidationCode = target.Consolidation  AND source.InventoryNumber = target.InventoryNumber  AND source.ExternalID = target.ExternalID");
            script.AppendLine($"group by target.rowid");
            script.AppendLine($"having count(target.rowid) > 1) as errt on err_target.rowid = errt.rowid");
            script.AppendLine($" ");
            script.AppendLine($"INSERT INTO @errorTable ");
            script.AppendLine($"SELECT err_target.rowNumb, N'Обнаружены дублирующие данные по составному ключу \"Инвентарный номер\" : \"Номер записи в учётной системе\" : \"БЕ\", а именно (\"' + err_target.[InventoryNumber] + '\" : \"' + err_target.[ExternalID] + '\" : \"' + err_target.[Consolidation] + '\")', N'ERR_OSID', N'IM00', err_target.[InventoryNumber], err_target.[EUSINumber] ");
            script.AppendLine($"FROM {GetTempTableName()} as err_target with (nolock) ");
            script.AppendLine($"left outer join @errorTable as tERR on err_target.rowNumb = tERR.rowNumb ");
            script.AppendLine($"inner join( ");
            script.AppendLine($"select target.rowid as 'rowid', count(target.rowid) as 'cntERR' from {GetTempTableName()} AS target  with (nolock)  ");
            script.AppendLine($"inner join  {GetTempTableName()} AS source  with(nolock) ");
            script.AppendLine($"ON source.Hidden = 0 AND source.IsHistory = 0 AND target.Oid IS NULL AND target.EUSINumber IS NULL AND target.ExternalID IS NOT NULL  AND target.rowProcessed <> 1 AND source.Consolidation = target.Consolidation  AND source.InventoryNumber = target.InventoryNumber  AND source.ExternalID = target.ExternalID ");
            script.AppendLine($"group by target.rowid ");
            script.AppendLine($"having count(target.rowid) > 1) as errt on err_target.rowid = errt.rowid ");
            script.AppendLine($"where tERR.rowNumb is null ");
            script.AppendLine($" ");
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET rowProcessed = 1");
            script.AppendLine($"WHERE rowNumb IN(SELECT rowNumb FROM @errorTable)");

            //обновление признака "за баланс"
            script.AppendLine(SetOutOfBus());

            //Первичная идентификация ОС
            script.AppendLine(FirstIdentityOS());



            //Обновление идентифицировнных ОС с учетом истории
            script.AppendLine(UpdateOS());

            //Проверка на наличие ошибок перед началом транзакции
            script.AppendLine($"if(@errExistsToImportHistory=0)");
            script.AppendLine($"BEGIN ");

            //Начало транзакции
            script.AppendLine($"BEGIN TRANSACTION BEGIN TRY");
            //Создание новых ОС
            script.AppendLine(CreateNewOS());


            //Создание движений
            script.AppendLine(CreateMovings());

            //1
            //Создание ОИ
            script.AppendLine(CreateEstate());
            
            //2
            //Создание связи ОС и ОИ
            script.AppendLine(LinkAccountingObjectAndEstate());
            //Создание расширенной модели EstateTaxes для InventoryObject
            script.AppendLine($"INSERT INTO [CorpProp.Estate].[EstateTaxes] (TaxesOfID,Hidden,SortOrder)");
            script.AppendLine($"select inv.id, 0, 1  from [CorpProp.Estate].[EstateTaxes] as et");
            script.AppendLine($"right outer join [CorpProp.Estate].InventoryObject as inv on et.TaxesOfID = inv.ID");
            script.AppendLine($"where et.ID is null");
            //Наполнение данными EstateTaxes
            script.AppendLine($"update et");
            script.AppendLine($"set et.TaxBaseID = ao.TaxBaseID");
            script.AppendLine($"from [CorpProp.Estate].[EstateTaxes] as et");
            script.AppendLine($"inner join [CorpProp.Estate].InventoryObject as inv on et.TaxesOfID = inv.ID");
            script.AppendLine($"left join  { GetTempTableName()} as ao on inv.ID = ao.EstateID");
            script.AppendLine($"where ao.TaxBaseID is not null");
            
            //Обновление существующих (не новых) ОИ по логике ЕУСИ (импорт состояний)
            script.AppendLine(UpdateExistsEstate());

            //Обновление атрибутов созданных ОИ по данным ОС
            script.AppendLine(UpdateCreatedEstate());


            //Вызываем ошибку при наличии блокирующих ошибок.
            script.AppendLine($"if exists(SELECT rowNumb AS errorRowNum, text, errCode, invNumber, eusiNumber FROM @errorTable WHERE errCode IS NOT NULL AND errCode <> N'') ");
            script.AppendLine($"begin ");
            script.AppendLine($"ROLLBACK ");
            script.AppendLine($"end ");
            script.AppendLine($"else ");
            script.AppendLine($"begin ");
            script.AppendLine($"COMMIT ");
            script.AppendLine($"end ");


            script.AppendLine($" END TRY");
            script.AppendLine($"BEGIN CATCH");
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT");
            script.AppendLine($"N'' as 'rowNumb'");
            script.AppendLine($", cast(ERROR_MESSAGE() as nvarchar(max)) as 'Text'");
            script.AppendLine($", N'ERR_MigrateOS' as 'errResultCode'");
            script.AppendLine($", N'SQL_' + cast(ERROR_NUMBER() as nvarchar(max)) AS 'errCode'");
            script.AppendLine($", N'' as 'invNumber'");
            script.AppendLine($", N'' as 'eusiNumber';");
            script.AppendLine($"ROLLBACK");
            script.AppendLine($"END CATCH");


            script.AppendLine($"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
            script.AppendLine($"begin tran");

            script.AppendLine($"declare @current_value int = cast(isnull((sELECT current_value   FROM sys.sequences  WHERE name = 'EstateSequence'), '0') as int)");
            script.AppendLine($"DECLARE @StartEstateSequence INT = 0");
            script.AppendLine($"DECLARE @MaxInventoryObject INT = ISNULL((SELECT MAX(e.Number)");
            script.AppendLine($"FROM[CorpProp.Estate].Estate e");
            script.AppendLine($"INNER JOIN[CorpProp.Estate].InventoryObject io ON io.ID = e.ID");
            script.AppendLine($"WHERE io.IsPropertyComplex = 0), 0) +1");
            script.AppendLine($"DECLARE @MaxIntangibleAsset INT = ISNULL((SELECT MAX(e.Number)");
            script.AppendLine($"FROM[CorpProp.Estate].Estate e");
            script.AppendLine($"INNER JOIN[CorpProp.Estate].IntangibleAsset io ON io.ID = e.ID), 0) +1");
            script.AppendLine($"set @StartEstateSequence = case when @MaxInventoryObject> @MaxIntangibleAsset then @MaxInventoryObject else @MaxIntangibleAsset end;");
            script.AppendLine($"if (@current_value > @StartEstateSequence)");
            script.AppendLine($"BEGIN");
            script.AppendLine($"DECLARE @sql NVARCHAR(MAX)");
            script.AppendLine($"SET @sql = 'alter SEQUENCE EstateSequence RESTART WITH ' + CONVERT(NVARCHAR(255), @StartEstateSequence);");
            script.AppendLine($"EXEC(@sql);");
            script.AppendLine($"END");

            script.AppendLine($"COMMIT TRAN;");

            //Окончание проверки на наличие ошибок перед началом транзакции
            script.AppendLine($"END ");

            script.AppendLine($" ;ENABLE TRIGGER [{GetTableSchemaName(typeof(AccountingObjectTbl))}].[TR_AccountingCalculatedFild_CostEstate]  ON {GetTableName(typeof(AccountingObjectTbl))}");
            //Включение триггера для Estate
            script.AppendLine($" ;ENABLE TRIGGER [{GetTableSchemaName(typeof(Estate))}].[TR_EstateCalculatedField_Ins]  ON {GetTableName(typeof(Estate))}");
            script.AppendLine($" ;ENABLE TRIGGER [{GetTableSchemaName(typeof(InventoryObject))}].[TR_EstateCalculatedField_Count]  ON {GetTableName(typeof(InventoryObject))}");
            script.AppendLine($" ;ENABLE TRIGGER [{GetTableSchemaName(typeof(InventoryObject))}].[TR_InventoryObject_UpdateCost]  ON {GetTableName(typeof(InventoryObject))}");

            //Получение информации о количестве созданных ОИ по типам
            script.AppendLine(GetCountCreateEstate());
            //или
            //всего обработано объектов
            script.AppendLine($"SELECT count(*) as 'TotalRecords' ");
            script.AppendLine($"FROM {GetTempTableName()}");

            //ошибки, только те, которые стопят импорт: код ошибки не пусто
            script.AppendLine($"SELECT rowNumb AS errorRowNum, text, errCode, invNumber, eusiNumber");
            script.AppendLine($"FROM {errTable}");
            script.AppendLine($"WHERE errCode IS NOT NULL AND errCode <> N''");

            //завершаем удалением таблицы импорта
            script.AppendLine($"DROP TABLE {GetTempTableName()}");



            return script.ToString();
        }


        /// <summary>
        /// Скрипт первичной идентификации существующих в Системе ОС.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Формирует sql-скрипт, который обновляет в импортируемой таблице Oid-ы найденных ОС.
        /// </remarks>
        private string FirstIdentityOS()
        {
            var script = new StringBuilder();

            //БЕ и Инвентарный не задан = ошибка            
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT rowNumb" +
                $", N'ОС не идентифицирован, не задан БЕ и/или Инвентарный №'" +
                $", N'ERR_OSID'" +
                $", N'IM03'" +
                $",[{nameof(AccountingObject.InventoryNumber)}]" +
                $",[{nameof(AccountingObject.EUSINumber)}]");
            script.AppendLine($"FROM {GetTempTableName()} with (nolock) ");
            script.AppendLine($"WHERE {nameof(AccountingObject.Consolidation)} IS NULL OR {nameof(AccountingObject.InventoryNumber)} IS NULL");

            //Указан несуществующий номер ЕУСИ = ошибка            
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT rowNumb" +
                $", N'ОИ с указанным номером ЕУСИ не найден'" +
                $", N'ERR_ROW_PR'" +
                $", N'IM02'" +
                $",[{nameof(AccountingObject.InventoryNumber)}]" +
                $",[{nameof(AccountingObject.EUSINumber)}]");
            script.AppendLine($"FROM {GetTempTableName()} with (nolock) ");
            script.AppendLine($"WHERE {nameof(AccountingObject.EUSINumber)} IS NOT NULL " +
                $"AND {nameof(AccountingObject.EUSINumber)} NOT IN (" +
                $"SELECT DISTINCT ISNULL({nameof(Estate.Number)},0) " +
                $"FROM {GetTableName(typeof(Estate))}  with (nolock) " +
                $"WHERE Hidden = 0 AND IsHistory = 0" +
                $")");


            //ошибочные ОС-ки обработаны
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET {rowProcessed} = 1");
            script.AppendLine($"WHERE rowNumb IN (SELECT rowNumb FROM {errTable})");


            //Номер ЕУСИ задан, ключ1 = НомерЕУСИ+Инв№+БЕ           
            script.AppendLine($"MERGE {GetTempTableName()} AS target ");
            script.AppendLine($"USING {GetTableName(typeof(AccountingObjectExtView))} AS source ");
            script.AppendLine($"ON source.Hidden = 0 AND source.IsHistory = 0" +
                              $" AND source.{nameof(AccountingObjectExtView.ConsolidationCode)} = target.{nameof(AccountingObject.Consolidation)} " +
                              $" AND source.{nameof(AccountingObjectExtView.InventoryNumber)} = target.{nameof(AccountingObject.InventoryNumber)} " +
                              $" AND source.{nameof(AccountingObjectExtView.Number)} = target.{nameof(AccountingObject.EUSINumber)} ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($"target.{nameof(AccountingObject.Oid)} = source.{nameof(AccountingObject.Oid)}");
            script.AppendLine($";");


            //Номер ЕУСИ задан, ключ2 = НомерЕУСИ+БЕ 
            script.AppendLine($"MERGE {GetTempTableName()} AS target ");
            script.AppendLine($"USING {GetTableName(typeof(AccountingObjectExtView))} AS source ");
            script.AppendLine($"ON source.Hidden = 0 AND source.IsHistory = 0 " +
                              $" AND target.{nameof(AccountingObject.Oid)} IS NULL " +
                              $" AND target.{nameof(AccountingObject.EUSINumber)} IS NOT NULL " +
                              $" AND target.{rowProcessed} <> 1 " +
                              $" AND source.{nameof(AccountingObjectExtView.ConsolidationCode)} = target.{nameof(AccountingObject.Consolidation)} " +
                              $" AND source.{nameof(AccountingObjectExtView.Number)} = target.{nameof(AccountingObject.EUSINumber)} ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($"target.{nameof(AccountingObject.Oid)} = source.{nameof(AccountingObject.Oid)}");
            script.AppendLine($";");


            //Не идентифицировали по ключу 1 или ключу 2 = ошибка идентификации, 
            //но не помечаем обработку строки, т.к. дальше ОС м.б. идентифицирован по иным ключам.
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT rowNumb" +
                $", N'Результат первичной идентификации ОС \"ОС не идентифицирован с номером ЕУСИ\"'" +
                $", N'EUSI_2'" +
                $", N''" +
                $",[{nameof(AccountingObject.InventoryNumber)}]" +
                $",[{nameof(AccountingObject.EUSINumber)}]");
            script.AppendLine($"FROM {GetTempTableName()} with (nolock) ");
            script.AppendLine($"WHERE { nameof(AccountingObject.Oid)} IS NULL " +
               $"AND {nameof(AccountingObject.EUSINumber)} IS NOT NULL " +
               $"AND {rowProcessed} <> 1 " +
               $"AND rowNumb NOT IN (SELECT rowNumb FROM {errTable})");


            //Номер ЕУСИ не задан, Системный № задан ключ3= Инв№+Сист№+БЕ 
            script.AppendLine($"MERGE {GetTempTableName()} AS target ");
            script.AppendLine($"USING {GetTableName(typeof(AccountingObjectExtView))} AS source ");
            script.AppendLine($"ON source.Hidden = 0 AND source.IsHistory = 0" +
                              $" AND target.{nameof(AccountingObject.Oid)} IS NULL " +
                              $" AND target.{nameof(AccountingObject.EUSINumber)} IS NULL " +
                              $" AND target.{nameof(AccountingObject.ExternalID)} IS NOT NULL " +
                              $" AND target.{rowProcessed} <> 1" +
                              $" AND source.{nameof(AccountingObjectExtView.ConsolidationCode)} = target.{nameof(AccountingObject.Consolidation)} " +
                              $" AND source.{nameof(AccountingObjectExtView.InventoryNumber)} = target.{nameof(AccountingObject.InventoryNumber)} " +
                              $" AND source.{nameof(AccountingObjectExtView.ExternalID)} = target.{nameof(AccountingObject.ExternalID)} ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($"target.{nameof(AccountingObject.Oid)} = source.{nameof(AccountingObject.Oid)}");
            script.AppendLine($";");


            //Номер ЕУСИ не задан и системный номер не задан, ключ4= Инв№+БЕ
            script.AppendLine($"MERGE {GetTempTableName()} AS target ");
            script.AppendLine($"USING {GetTableName(typeof(AccountingObjectExtView))} AS source ");
            script.AppendLine($"ON source.Hidden = 0  AND source.IsHistory = 0" +
                              $" AND target.{nameof(AccountingObject.Oid)} IS NULL " +
                              $" AND target.{nameof(AccountingObject.EUSINumber)} IS NULL " +
                              $" AND target.{nameof(AccountingObject.ExternalID)} IS NULL " +
                              $" AND target.{rowProcessed} <> 1" +
                              $" AND source.{nameof(AccountingObjectExtView.ConsolidationCode)} = target.{nameof(AccountingObject.Consolidation)} " +
                              $" AND source.{nameof(AccountingObjectExtView.InventoryNumber)} = target.{nameof(AccountingObject.InventoryNumber)} ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($"target.{nameof(AccountingObject.Oid)} = source.{nameof(AccountingObject.Oid)}");
            script.AppendLine($";");


            //Не идентифицировали по ключу 3 или ключу 4 = ошибка идентификации, 
            //но не помечаем обработку строки, т.к. дальше ОС м.б. идентифицирован по иным ключам.
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT rowNumb" +
                $", N'Результат первичной идентификации ОС \"ОС не идентифицирован с номером ЕУСИ\"'" +
                $", N'NOTEUSI_2'" +
                $", N''" +
                $",[{nameof(AccountingObject.InventoryNumber)}]" +
                $",[{nameof(AccountingObject.EUSINumber)}]");
            script.AppendLine($"FROM {GetTempTableName()} with (nolock) ");
            script.AppendLine($"WHERE { nameof(AccountingObject.Oid)} IS NULL " +
               $"AND {nameof(AccountingObject.EUSINumber)} IS NULL " +
               $"AND {rowProcessed} <> 1 " +
               $"AND rowNumb NOT IN (SELECT rowNumb FROM {errTable})");


            //исключим неидентифицированный забалансовый ОС с неидентифицированным КА           
            script.AppendLine($"INSERT INTO {errTable}");
            script.AppendLine($"SELECT " +
                $"source.rowNumb" +
                $", CASE WHEN source.{nameof(AccountingObject.SubjectCode)} IS NULL THEN N'КА не может быть идентифицирован, поле \"Контрагент по договору (Код СДП)\" не заполнено'" +
                $"       WHEN (source.{nameof(AccountingObject.SubjectCode)} IS NOT NULL AND subj.ID IS NULL) THEN  N'КА не идентифицирован, деловой партнер с заданным кодом (Код СДП) не существует'" +
                $"       WHEN (source.{nameof(AccountingObject.SubjectCode)} IS NOT NULL AND subj.ID IS NOT NULL AND og.ID IS NOT NULL AND be.ID IS NULL ) THEN  N'Невозможно определить БЕ для указанного КА'" +
                $"  END" +
                $", CASE WHEN (source.{nameof(AccountingObject.SubjectCode)} IS NULL OR subj.ID IS NULL) THEN N'ERR_subjID'" +
                $"       WHEN (source.{nameof(AccountingObject.SubjectCode)} IS NOT NULL AND subj.ID IS NOT NULL AND og.ID IS NOT NULL AND be.ID IS NULL ) THEN  N'ERR_EstateID'" +
                $"  END" +
                $", CASE WHEN source.{nameof(AccountingObject.SubjectCode)} IS NULL THEN N'IM04'" +
                $"       WHEN (source.{nameof(AccountingObject.SubjectCode)} IS NOT NULL AND subj.ID IS NULL) THEN  N'IM05'" +
                $"       WHEN (source.{nameof(AccountingObject.SubjectCode)} IS NOT NULL AND subj.ID IS NOT NULL AND og.ID IS NOT NULL AND be.ID IS NULL ) THEN  N'IM06'" +
                $"  END " +
                $", source.{nameof(AccountingObject.InventoryNumber)} " +
                $", source.{nameof(AccountingObject.EUSINumber)} ");
            script.AppendLine($"FROM {GetTempTableName()} source ");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(Subject))} subj  with (nolock) ON source.[{nameof(AccountingObject.SubjectCode)}] = subj.{nameof(Subject.SDP)}");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(Society))} og  with (nolock) ON subj.[{nameof(Subject.SocietyID)}] = og.ID");
            script.AppendLine($"LEFT JOIN {GetTableName(typeof(DictObject))} be  with (nolock) ON og.[{nameof(Society.ConsolidationUnitID)}] = be.ID");
            script.AppendLine($"WHERE source.{nameof(AccountingObject.Oid)} IS NULL AND source.{nameof(AccountingObject.OutOfBalance)} = 1 ");
            script.AppendLine($"AND source.{rowProcessed} <> 1");
            script.AppendLine($"AND (source.{nameof(AccountingObject.SubjectCode)} IS NULL OR subj.ID IS NULL OR og.ID IS NULL OR be.ID IS NULL)");

            //ошибочные ОС-ки обработаны
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET {rowProcessed} = 1");
            script.AppendLine($"WHERE rowNumb IN (SELECT rowNumb FROM {errTable} WHERE errResultCode IN ( N'ERR_subjID',N'ERR_EstateID'))");




            return script.ToString();
        }



        /// <summary>
        /// Скрипт обновления признака "За баланс" в импортируемой таблице. 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Признак учавствует в алгоритме миграции, влияет на идентификацию ОС.
        /// </remarks>
        private string SetOutOfBus()
        {
            var script = new StringBuilder();

            //Номер счета начинается с двух нулей 00 * или начинается с
            //Z или буквы З(забалансовые счета Износ основных средств 010
            //и Основные средства, сданные в аренду 011 –
            //неприменимы для ЕУСИ)

            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET {nameof(AccountingObject.OutOfBalance)} = 1");
            script.AppendLine($"WHERE {nameof(AccountingObject.AccountNumber)} LIKE N'00%' " +
                $"OR {nameof(AccountingObject.AccountNumber)} LIKE N'Z%' " +
                $"OR {nameof(AccountingObject.AccountNumber)} LIKE N'З%' ");

            return script.ToString();
        }

        /// <summary>
        /// Обновление существующих в Системе ОС с учетом истории.
        /// </summary>
        /// <returns></returns>
        private string UpdateOS()
        {
            var script = new StringBuilder();

            script.AppendLine($"----UPDATE-OS------------------------------------------");

            // 1. obj.ActualDate == StartPeriod  обновление текущей версии ОС
            script.AppendLine(UpdateVersionOS());

            // 2. obj.ActualDate < StartPeriod 
            script.AppendLine(CreateNewVersionOS());

            // 3. obj.ActualDate > StartPeriod 
            script.AppendLine(CreateOldVersionOS());

            script.AppendLine($"----END-UPDATE-OS-------------------------------------");

            return script.ToString();
        }

        /// <summary>
        /// Формирует текст скрипта создания новых ОС.
        /// </summary>
        /// <returns></returns>
        private string CreateNewOS()
        {
            var script = new StringBuilder();

            //пометим что нужно создать
            script.AppendLine(SetMarkOnCreateOS());

            //Связывание ОС и ОИ
            script.AppendLine(LinkOSAndEstate());


            //создаём новые ОС
            script.AppendLine($"INSERT INTO {GetTableName(MainType)}({GetInsertColumnSpecification()})");
            script.AppendLine($"SELECT {GetSelectSpecification("source")} ");
            script.AppendLine($"FROM {GetTempTableName()} source");
            script.AppendLine($"WHERE source.[{rowProcessed}] <> 1 ");
            script.AppendLine($"AND source.[{isNewOS}] = 1 ");


            return script.ToString();

        }


        /// <summary>
        /// Формирует скрипт обновления атрибутов для созданных в результате миграции ОИ.
        /// </summary>
        /// <returns></returns>
        private string UpdateCreatedEstate()
        {
            //TODO: перенести заполнение атрибутов в скрипт создания ОИ

            var script = new StringBuilder();

            script.AppendLine($"");
            script.AppendLine($"-------ОБНОВЛЕНИЕ-АТРИБУТОВ-НОВЫХ-ОИ-----------------------------------------------------------------------------------");
            script.AppendLine($"");

            //обновление Estate  
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.Estate.AddonOKOFID)} = os.{nameof(AccountingObjectExtView.AddonOKOFID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.BusinessAreaID)} = os.{nameof(AccountingObjectExtView.BusinessAreaID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.DepreciationMethodMSFOID)} = os.{nameof(AccountingObjectExtView.DepreciationMethodMSFOID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.DepreciationMethodNUID)} = os.{nameof(AccountingObjectExtView.DepreciationMethodNUID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.DepreciationMethodRSBUID)} = os.{nameof(AccountingObjectExtView.DepreciationMethodRSBUID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.DepreciationMultiplierForNU)} = os.{nameof(AccountingObjectExtView.DepreciationMultiplierForNU)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.DivisibleTypeID)} = os.{nameof(AccountingObjectExtView.DivisibleTypeID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.NameEUSI)} = os.{nameof(AccountingObjectExtView.Name)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.OKOF2014ID)} = os.{nameof(AccountingObjectExtView.OKOF2014ID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.OKTMOID)} = os.{nameof(AccountingObjectExtView.OKTMOID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.UsefulForNU)} = os.{nameof(AccountingObjectExtView.UsefulForNU)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.UsefulForRSBU)} = os.{nameof(AccountingObjectExtView.Useful)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Estate.OutOfBalance)} = CASE WHEN ISNULL(src.{nameof(AccountingObjectExtView.OutOfBalance)},3) = 1 THEN 1 ELSE est.{nameof(CorpProp.Entities.Estate.Estate.OutOfBalance)} END");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS est " +
                              $"ON src.{oidEstate} = est.{nameof(TypeObject.Oid)} AND est.Hidden = 0 AND est.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");


            //обновление InventoryObject
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.InventoryObject.Address)} = os.{nameof(AccountingObjectExtView.Address)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.BuildingLength)} = os.{nameof(AccountingObjectExtView.BuildingLength)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.DepositID)} = os.{nameof(AccountingObjectExtView.DepositID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.DepreciationGroupID)} = os.{nameof(AccountingObjectExtView.DepreciationGroupID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.EstateMovableNSIID)} = os.{nameof(AccountingObjectExtView.EstateMovableNSIID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.LicenseAreaID)} = os.{nameof(AccountingObjectExtView.LicenseAreaID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.PositionConsolidationID)} = os.{nameof(AccountingObjectExtView.PositionConsolidationID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.RentContractDate)} = os.{nameof(AccountingObjectExtView.RentContractDate)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.RentContractNumberSZVD)} = os.{nameof(AccountingObjectExtView.RentContractNumberSZVD)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibCityNSIID)} = os.{nameof(AccountingObjectExtView.SibCityNSIID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibCountryID)} = os.{nameof(AccountingObjectExtView.SibCountryID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibFederalDistrictID)} = os.{nameof(AccountingObjectExtView.SibFederalDistrictID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibRegionID)} = os.{nameof(AccountingObjectExtView.RegionID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.SPPCode)} = src.{nameof(AccountingObjectExtView.SPPCode)}");
            // поле в др. таблице налогов script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.InventoryObject.TaxBaseID)} = os.{nameof(AccountingObjectExtView.TaxBaseID)}");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.InventoryObject))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");


            //обновление РМД налоги
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.EstateTaxes.TaxBaseID)} = os.{nameof(AccountingObjectExtView.TaxBaseID)}");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.EstateTaxes))} AS est " +
                              $"ON tgr.ID = est.{nameof(CorpProp.Entities.Estate.EstateTaxes.TaxesOfID)} ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");


            //обновление RealEstate
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.RealEstate.YearCommissionings)} = ");
            script.AppendLine($" ( CASE WHEN os.{nameof(AccountingObjectExtView.BuildYear)} IS NOT NULL THEN os.{nameof(AccountingObjectExtView.BuildYear)} " +
                                     $" WHEN os.{nameof(AccountingObjectExtView.Year)} IS NOT NULL THEN os.{nameof(AccountingObjectExtView.Year)} " +
                                     $"ELSE NULL " +
                                     $"END)");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.RealEstate))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");


            //обновление Cadastral
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.Cadastral.BuildingArea)} = os.{nameof(AccountingObjectExtView.BuildingArea)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Cadastral.Bush)} = src.{nameof(AccountingObjectExtView.Bush)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Cadastral.CadastralNumber)} = os.{nameof(AccountingObjectExtView.CadastralNumber)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Cadastral.Depth)} = os.{nameof(AccountingObjectExtView.DepthWell)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Cadastral.RightRegDate)} = os.{nameof(AccountingObjectExtView.RightRegDate)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Cadastral.UsesKind)} = os.{nameof(AccountingObjectExtView.UsesKind)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Cadastral.Well)} = src.{nameof(AccountingObjectExtView.Well)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Cadastral.CadastralNumberLand)} = src.{nameof(AccountingObjectExtView.GroundNumber)}");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Cadastral))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");


            //обновление Land
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.Land.GroundCategoryID)} = ( " +
                $"SELECT TOP 1 gr.ID " +
                $"FROM {GetTableName(typeof(GroundCategory))} gr " +
                $"INNER JOIN {GetTableName(typeof(DictObject))} dd ON gr.ID = dd.ID " +
                $"WHERE Hidden = 0 AND src.{nameof(AccountingObjectExtView.GroundCategory)} IS NOT NULL AND ( " +
                $"Code = CAST(src.[{nameof(AccountingObjectExtView.GroundCategory)}] as NVARCHAR(MAX)) " +
                $"OR Name=CAST(src.[{nameof(AccountingObjectExtView.GroundCategory)}] as NVARCHAR(MAX)) " +
                $"OR ExternalID=CAST(src.[{nameof(AccountingObjectExtView.GroundCategory)}] as NVARCHAR(MAX))))");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Land))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");


            //обновление Ship
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.Ship.RegSeaNumber)} = os.{nameof(AccountingObjectExtView.ShipRegNumber)}"); //??? вычисляемое поле          
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Ship))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");

            //обновление UnfinishedConstruction
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.UnfinishedConstruction.StartDateUse)} = os.{nameof(AccountingObjectExtView.StartDateUse)}");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.UnfinishedConstruction))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");


            //обновление Vehicle
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.Vehicle.DieselEngine)} = os.{nameof(AccountingObjectExtView.DieselEngine)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.EcoKlassID)} = os.{nameof(AccountingObjectExtView.EcoKlassID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.EngineSize)} = os.{nameof(AccountingObjectExtView.EngineSize)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.InOtherSystem)} = os.{nameof(AccountingObjectExtView.InOtherSystem)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleModelID)} = os.{nameof(AccountingObjectExtView.VehicleModelID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.Power)} = os.{nameof(AccountingObjectExtView.Power)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.RegDate)} = os.{nameof(AccountingObjectExtView.VehicleRegDate)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.RegNumber)} = os.{nameof(AccountingObjectExtView.VehicleRegNumber)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.SerialNumber)} = os.{nameof(AccountingObjectExtView.SerialNumber)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.SibMeasureID)} = os.{nameof(AccountingObjectExtView.SibMeasureID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.SignNumber)} = os.{nameof(AccountingObjectExtView.SignNumber)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleCategoryID)} = os.{nameof(AccountingObjectExtView.VehicleCategoryID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleTaxFactor)} = os.{nameof(AccountingObjectExtView.VehicleTaxFactor)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleTypeID)} = os.{nameof(AccountingObjectExtView.VehicleTypeID)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.YearOfIssue)} = os.{nameof(AccountingObjectExtView.YearOfIssue)}");
            script.AppendLine($", est.{nameof(CorpProp.Entities.Estate.Vehicle.Model2)} = os.{nameof(AccountingObjectExtView.Model2)}");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{isNewEstate} = 1 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Vehicle))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");


            script.AppendLine($"----------------------------------------------------------------------------------------------------------------");
            script.AppendLine($"");

            //Обновление EstateDefinitionTypeID в Estate
            script.AppendLine($"update est set est.EstateDefinitionTypeID = DictEDT.id");
            script.AppendLine($"FROM  {GetTempTableName()} AS ao ");
            script.AppendLine($"left join [CorpProp.Estate].Estate AS est  on ao.oidEstate = est.oid ");
            script.AppendLine($"left join [CorpProp.Base].DictObject as DictEDT on ao.typeEstate = DictEDT.Code ");
            script.AppendLine($"inner join [CorpProp.NSI].EstateDefinitionType as edt on DictEDT.ID = edt.ID ");

            //Создание EstateCalculatedField для Estate
            script.AppendLine($"insert into [CorpProp.Estate].EstateCalculatedField (EstateID, Hidden, SortOrder)");
            script.AppendLine($"select est.id,0,1 from [CorpProp.Estate].Estate as est ");
            script.AppendLine($"left outer join [CorpProp.Estate].EstateCalculatedField as calc on est.ID = calc.EstateID");
            script.AppendLine($"where calc.EstateID is null");
            script.AppendLine($"");
            script.AppendLine($"update est set est.CalculateID = calc.id");
            script.AppendLine($"from [CorpProp.Estate].Estate as est");
            script.AppendLine($"left join [CorpProp.Estate].EstateCalculatedField as calc on est.ID = calc.EstateID");
            script.AppendLine($"where est.CalculateID is null");
            //Обновление стоимостных показателей ОИ (EstateCalculatedField для Estate)
            script.AppendLine($"update calc");
            script.AppendLine($"set");
            script.AppendLine($"calc.OwnerID = aobj.OwnerID");
            script.AppendLine($", calc.WhoUseID = aobj.WhoUseID");
            script.AppendLine($", calc.DealProps = aobj.DealProps");
            script.AppendLine($", calc.MainOwnerID = aobj.MainOwnerID");
            script.AppendLine($", calc.InitialCostSumOBU = isnull(aobj.InitialCost, 0.00)");
            script.AppendLine($", calc.ResidualCostSumOBU = isnull(aobj.ResidualCost, 0.00)");
            script.AppendLine($", calc.InitialCostSumNU = isnull(aobj.InitialCostNU, 0.00)");
            script.AppendLine($", calc.ResidualCostSumNU = isnull(aobj.ResidualCostNU, 0.00)");
            script.AppendLine($"from[CorpProp.Estate].[EstateCalculatedField] as calc with(nolock)");
            script.AppendLine($"inner join [CorpProp.Estate].InventoryObject as inv with(nolock) on calc.EstateID = inv.ID");
            script.AppendLine($"left join (select");
            script.AppendLine($"                        aobjV.ID");
            script.AppendLine($"                        , aobjV.EstateID");
            script.AppendLine($"                        , aobjV.OwnerID");
            script.AppendLine($"                        , aobjV.WhoUseID");
            script.AppendLine($"                        , aobjV.DealProps");
            script.AppendLine($"                        , aobjV.MainOwnerID");
            script.AppendLine($"                        , isnull(aobjV.InitialCost, 0.00) as InitialCost");
            script.AppendLine($"                        , isnull(aobjV.ResidualCost, 0.00) as ResidualCost");
            script.AppendLine($"                        , isnull(aobjV.InitialCostNU, 0.00) as InitialCostNU");
            script.AppendLine($"                        , isnull(aobjV.ResidualCostNU, 0.00) as ResidualCostNU");
            script.AppendLine($"                         from[CorpProp.Accounting].[AccountingObjectTbl] as aobjV with(nolock)");
            script.AppendLine($"                         where(aobjV.AccountNumber like '1%' or aobjV.AccountNumber like '01%') and aobjV.IsHistory = 0 and aobjV.Hidden = 0) as aobj on inv.ID is not null and inv.ID = aobj.EstateID");
            script.AppendLine($"where calc.ID is not null and isnull(inv.IsPropertyComplex, 0) = 0 and");
            script.AppendLine($"(");
            script.AppendLine($"((calc.OwnerID is null and aobj.OwnerID is not null) or calc.OwnerID <> aobj.OwnerID) or");
            script.AppendLine($"((calc.WhoUseID is null and aobj.WhoUseID is not null) or calc.WhoUseID<> aobj.WhoUseID) or");
            script.AppendLine($"((calc.DealProps is null and aobj.DealProps is not null) or calc.DealProps<> aobj.DealProps) or");
            script.AppendLine($"((calc.MainOwnerID is null and aobj.MainOwnerID is not null) or calc.MainOwnerID<> aobj.MainOwnerID) or");
            script.AppendLine($"(isnull(calc.InitialCostSumOBU, 0.00) <> isnull(aobj.InitialCost, 0.00)) or");
            script.AppendLine($"(isnull(calc.ResidualCostSumOBU, 0.00) <> isnull(aobj.ResidualCost, 0.00)) or");
            script.AppendLine($"(isnull(calc.InitialCostSumNU, 0.00) <> isnull(aobj.InitialCostNU, 0.00)) or");
            script.AppendLine($"(isnull(calc.ResidualCostSumNU, 0.00) <> isnull(aobj.ResidualCostNU, 0.00))");
            script.AppendLine($")");
            script.AppendLine($"----------------------------------------------------------------------------------------------------------------");
            script.AppendLine($"");

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт обновления атрибутов для существующих ОИ по логике импорта состояний ЕУСИ.
        /// <seealso cref="EUSI.Services.Accounting.AccountingObjectExtService.UpdateEstateData(Base.DAL.IUnitOfWork, AccountingObject, ref ImportHistory)"/>
        /// </summary>       
        /// <returns></returns>
        private string UpdateExistsEstate()
        {            
            var script = new StringBuilder();

            //по логике импорта состояний ОС ЕУСИ в случае, если ОС исчторичный, ОИ не обновляется.

            script.AppendLine($"");
            script.AppendLine($"-------ОБНОВЛЕНИЕ-АТРИБУТОВ-СУЩЕСТВУЮЩИХ-ОИ-----------------------------------------------------------------------------------");
            script.AppendLine($"");

            //обновление Estate  
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"est.{nameof(CorpProp.Entities.Estate.Estate.InventoryNumber)} = os.{nameof(AccountingObjectExtView.InventoryNumber)}");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS est " +
                              $"ON src.{oidEstate} = est.{nameof(TypeObject.Oid)} AND est.Hidden = 0 AND src.{rowProcessed} <> 1 AND ISNULL(est.{nameof(CorpProp.Entities.Estate.Estate.InventoryNumber)},N'') = N''");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.IsHistory = 0 AND os.ActualDate = @StartPeriod");


            //обновление InventoryObject
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibCountryID)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibCountryID)},-1) = -1 THEN os.{nameof(AccountingObjectExtView.SibCountryID)} ELSE est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibCountryID)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibFederalDistrictID)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibFederalDistrictID)},-1) = -1 THEN os.{nameof(AccountingObjectExtView.SibFederalDistrictID)} ELSE est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibFederalDistrictID)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibRegionID)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibRegionID)},-1) = -1 THEN os.{nameof(AccountingObjectExtView.RegionID)} ELSE est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibRegionID)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibCityNSIID)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibCityNSIID)},-1) = -1 THEN os.{nameof(AccountingObjectExtView.SibCityNSIID)} ELSE est.{nameof(CorpProp.Entities.Estate.InventoryObject.SibCityNSIID)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.InventoryObject.Address)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.InventoryObject.Address)},N'') = N'' THEN os.{nameof(AccountingObjectExtView.Address)} ELSE est.{nameof(CorpProp.Entities.Estate.InventoryObject.Address)} END");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.InventoryObject))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.IsHistory = 0 AND os.ActualDate = @StartPeriod");
            

            //обновление Cadastral
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");         
            script.AppendLine($"est.{nameof(CorpProp.Entities.Estate.Cadastral.CadastralNumber)} = os.{nameof(AccountingObjectExtView.CadastralNumber)}");          
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Cadastral))} AS est " +
                              $"ON tgr.ID = est.ID AND ISNULL(est.{nameof(CorpProp.Entities.Estate.Cadastral.CadastralNumber)},N'') = N''");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.IsHistory = 0 AND os.ActualDate = @StartPeriod");

                      

            //обновление UnfinishedConstruction
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.UnfinishedConstruction.StartDateUse)} = os.{nameof(AccountingObjectExtView.StartDateUse)}");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0  AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.UnfinishedConstruction))} AS est " +
                              $"ON tgr.ID = est.ID AND est.{nameof(CorpProp.Entities.Estate.UnfinishedConstruction.StartDateUse)} IS NULL");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.IsHistory = 0 AND os.ActualDate = @StartPeriod");


            //обновление Vehicle
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"  est.{nameof(CorpProp.Entities.Estate.Vehicle.DieselEngine)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.Vehicle.DieselEngine)},-1) = -1 THEN os.{nameof(AccountingObjectExtView.DieselEngine)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.DieselEngine)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.RegDate)} = CASE WHEN est.{nameof(CorpProp.Entities.Estate.Vehicle.RegDate)} IS NULL THEN os.{nameof(AccountingObjectExtView.VehicleRegDate)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.RegDate)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.YearOfIssue)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.Vehicle.YearOfIssue)},-1) = -1 THEN os.{nameof(AccountingObjectExtView.YearOfIssue)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.YearOfIssue)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleCategoryID)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleCategoryID)},N'') = N'' THEN os.{nameof(AccountingObjectExtView.VehicleCategoryID)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleCategoryID)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.SibMeasureID)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.Vehicle.SibMeasureID)},N'') = N'' THEN os.{nameof(AccountingObjectExtView.SibMeasureID)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.SibMeasureID)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.Power)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.Vehicle.Power)},-1) = -1 THEN os.{nameof(AccountingObjectExtView.Power)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.Power)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.SerialNumber)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.Vehicle.SerialNumber)},N'') = N'' THEN os.{nameof(AccountingObjectExtView.SerialNumber)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.SerialNumber)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.EngineSize)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.Vehicle.EngineSize)},-1) = -1 THEN os.{nameof(AccountingObjectExtView.EngineSize)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.EngineSize)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleModelID)} = CASE WHEN est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleModelID)} IS NULL THEN os.{nameof(AccountingObjectExtView.VehicleModelID)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.VehicleModelID)} END");
            script.AppendLine($"  ,est.{nameof(CorpProp.Entities.Estate.Vehicle.RegNumber)} = CASE WHEN ISNULL(est.{nameof(CorpProp.Entities.Estate.Vehicle.RegNumber)},N'') = N'' THEN os.{nameof(AccountingObjectExtView.VehicleRegNumber)} ELSE est.{nameof(CorpProp.Entities.Estate.Vehicle.RegNumber)} END");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS tgr " +
                             $"ON src.{oidEstate} = tgr.{nameof(TypeObject.Oid)} AND tgr.Hidden = 0 AND tgr.IsHistory = 0 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Vehicle))} AS est " +
                              $"ON tgr.ID = est.ID ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.IsHistory = 0 AND os.ActualDate = @StartPeriod");


            //отдельное обновление признака "ЗА БАЛАНС"             
            script.AppendLine($"UPDATE est");
            script.AppendLine($"SET ");
            script.AppendLine($"est.{nameof(CorpProp.Entities.Estate.Estate.OutOfBalance)} = CASE WHEN ISNULL(src.{nameof(AccountingObjectExtView.OutOfBalance)}, 3) = 0 THEN 0 ELSE est.{nameof(CorpProp.Entities.Estate.Estate.OutOfBalance)} END");
            script.AppendLine($"FROM {GetTempTableName()} AS src");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(CorpProp.Entities.Estate.Estate))} AS est " +
                              $"ON src.{oidEstate} = est.{nameof(TypeObject.Oid)} AND est.Hidden = 0 AND src.{rowProcessed} <> 1 ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os " +
                              $"ON est.ID = os.{nameof(AccountingObjectExtView.EstateID)} AND os.Hidden = 0 AND os.ActualDate = @StartPeriod");

            script.AppendLine($"----------------------------------------------------------------------------------------------------------------");
            script.AppendLine($"");
                        

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт обновления импортируемой таблицы для пометки на создание ОС.
        /// </summary>
        /// <returns></returns>
        private string SetMarkOnCreateOS()
        {
            var script = new StringBuilder();

            script.AppendLine($"");

            //Номер ЕУСИ задан, но сам ОС не идентифицирован и код ошибки EUSI_2 
            //тогда создаем новые ОС
            script.AppendLine($";WITH src AS (SELECT * " +
               $"FROM {GetTempTableName()} " +
               $"WHERE {nameof(AccountingObject.Oid)} IS NULL " +
               $"AND {nameof(AccountingObject.EUSINumber)} IS NOT NULL " +
               $"AND {rowProcessed} <> 1 " +
               $"AND rowNumb IN (SELECT rowNumb FROM {errTable} WHERE errResultCode = N'EUSI_2') " +
               $") ");
            script.AppendLine($"MERGE src AS target");
            script.AppendLine($"USING (SELECT ID, Oid, Number " +
                                        $"FROM {GetTableName(typeof(Estate))} " +
                                        $"WHERE Hidden = 0 AND IsHistory = 0" +
                                        $") AS source");
            script.AppendLine($"ON target.{nameof(AccountingObject.EUSINumber)} = source.{nameof(Estate.Number)} ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($" target.{nameof(AccountingObject.Oid)} = NEWID()");
            script.AppendLine($", target.{nameof(AccountingObject.EstateID)} = source.ID");
            script.AppendLine($", target.{oidEstate} = source.Oid");
            script.AppendLine($", target.{isNewOS} = 1");
            script.AppendLine($";");



            //ОС не идентифицирован и забаланс или (не забаланс и в теории и + код ошибки = NOTEUSI_2, но в алгоритме это условно) 
            //тогда создаем новые ОС
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET {nameof(AccountingObject.Oid)} = NEWID()");
            script.AppendLine($", {isNewOS} = 1");
            script.AppendLine($"WHERE {nameof(AccountingObject.Oid)} IS NULL ");
            script.AppendLine($"AND {rowProcessed} <> 1 ");


            return script.ToString();
        }


        /// <summary>
        /// Возвращает строку с разделителем столбцов для вставки нового ОС, включая обязательные колонки
        /// </summary>
        /// <returns></returns>
        private string GetSelectNewOSColumns(string prefix)
        {
            var insertColumns = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);
            var p = string.IsNullOrEmpty(prefix) ? string.Empty : $"{prefix}.";

            return String.Join(",\r\n", insertColumns.Select(x =>
            {
                return string.IsNullOrEmpty(prefix) ? $"[{x}]" : $"{prefix}.[{x}]";
            }).ToArray());
        }

        /// <summary>
        /// Формирует скрипт создания движений для новых созданных ОС.
        /// </summary>
        /// <returns></returns>
        private string CreateMovings()
        {
            var script = new StringBuilder();

            //параметры
            script.AppendLine($"DECLARE @angleRSBU INT = (" +
                    $"SELECT TOP 1 t1.ID FROM {GetTableName(typeof(EUSI.Entities.NSI.Angle))} AS t1 " +
                    $"INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID " +
                    $"WHERE Hidden = 0 AND " +
                    $"UPPER(Code) = N'RSBU')");
            script.AppendLine($"DECLARE @angleMSFO INT = (" +
                    $"SELECT TOP 1 t1.ID FROM { GetTableName(typeof(EUSI.Entities.NSI.Angle))} AS t1 " +
                    $"INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID " +
                    $"WHERE Hidden = 0 AND " +
                    $"UPPER(Code) = N'MSFO')");
            script.AppendLine($"DECLARE @movINTRO INT = (" +
                    $"SELECT TOP 1 t1.ID FROM { GetTableName(typeof(EUSI.Entities.NSI.MovingType))} AS t1 " +
                    $"INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID " +
                    $"WHERE Hidden = 0 AND " +
                    $"UPPER(Code) = N'INTRODUCTION')");
            script.AppendLine($"DECLARE @movDEPR INT = (" +
                    $"SELECT TOP 1 t1.ID FROM { GetTableName(typeof(EUSI.Entities.NSI.MovingType))} AS t1 " +
                    $"INNER JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID " +
                    $"WHERE Hidden = 0 AND " +
                    $"UPPER(Code) = N'DEPRECIATION')");

            //создание движения РСБУ ввод в эксплуатацию
            script.AppendLine(GetInsertToMovingScript(1));

            //создание движения РСБУ амортизация
            script.AppendLine(GetInsertToMovingScript(2));

            //создание движения МСФО ввод в эксплуатацию
            script.AppendLine(GetInsertToMovingScript(3));

            //создание движения МСФО амортизация
            script.AppendLine(GetInsertToMovingScript(4));

            return script.ToString();
        }

        private string GetInsertToMovingScript(int movType)
        {
            var script = new StringBuilder();
            //TODO: оптимизировтаь

            script.AppendLine($"INSERT INTO {GetTableName(typeof(AccountingMoving))} ({GetMovingColumns()})");
            script.AppendLine($"SELECT {GetMovingInsert(movType)}");
            script.AppendLine($"FROM {GetTempTableName()} source");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} target");
            script.AppendLine($" ON source.Oid = target.Oid AND target.Hidden = 0 AND target.IsHistory = 0");
            script.AppendLine($"WHERE source.{rowProcessed} <> 1 AND source.{isNewOS} = 1");
            script.AppendLine($"");

            return script.ToString();
        }


        /// <summary>
        /// Формирует текст скрипта обновления текущей версии ОС.
        /// </summary>
        /// <returns></returns>
        private string UpdateVersionOS()
        {
            var script = new StringBuilder();

            script.AppendLine($";With src as (SELECT * FROM {GetTempTableName()} " +
                $"WHERE {rowProcessed} <> 1 " +
                $"AND {nameof(AccountingObject.Oid)} IS NOT NULL " +
                $")");
            script.AppendLine($"MERGE INTO {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"USING src AS source ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                              $"AND target.IsHistory = 0 AND target.Hidden = 0 ");
            script.AppendLine($"AND target.ActualDate = @StartPeriod ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($"{GetSetSpecification()}");
            script.AppendLine($";");

            script.AppendLine($"UPDATE source");
            script.AppendLine($"SET source.[{rowProcessed}] = 1");
            script.AppendLine($"FROM (SELECT * FROM {GetTempTableName()} " +
                $"WHERE {rowProcessed} <> 1 " +
                $"AND {nameof(AccountingObject.Oid)} IS NOT NULL " +
                $") AS source");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                              $"AND target.IsHistory = 0 AND target.Hidden = 0 ");
            script.AppendLine($"AND target.ActualDate = @StartPeriod ");

            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт создания новой актуальной версии ОС.
        /// </summary>
        /// <returns></returns>
        private string CreateNewVersionOS()
        {
            var script = new StringBuilder();
            script.AppendLine($"INSERT INTO {GetTableName(MainType)}({GetInsertColumnSpecification()})");
            script.AppendLine($"SELECT {GetSelectNewHistoryItemColumnSpecification("target")} " +
                                      $"FROM  {GetTableName(typeof(AccountingObjectExtView))} target " +
                                      $"INNER JOIN {GetTempTableName()} source " +
                                      $"ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                                      $"AND target.IsHistory = 0 AND target.Hidden = 0 " +
                                      $"AND target.ActualDate < @StartPeriod ");
            //обновление
            script.AppendLine($";With src as (SELECT * FROM {GetTempTableName()} " +
               $"WHERE {rowProcessed} <> 1 " +
               $"AND {nameof(AccountingObject.Oid)} IS NOT NULL " +
               $")");
            script.AppendLine($"MERGE INTO {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"USING src AS source ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                              $"AND target.IsHistory = 0 AND target.Hidden = 0 ");
            script.AppendLine($"AND target.ActualDate < @StartPeriod ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($"{GetSetSpecification()}");
            script.AppendLine($";");

            script.AppendLine($"UPDATE source");
            script.AppendLine($"SET source.[{rowProcessed}] = 1");
            script.AppendLine($"FROM (SELECT * FROM {GetTempTableName()} " +
               $"WHERE {rowProcessed} <> 1 " +
               $"AND {nameof(AccountingObject.Oid)} IS NOT NULL " +
               $") AS source");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                              $"AND target.IsHistory = 0 AND target.Hidden = 0 ");
            script.AppendLine($"AND target.ActualDate = @StartPeriod ");


            return script.ToString();
        }


        /// <summary>
        /// Формирует текст скрипта создания историчной записи ОС "задним" числом.
        /// </summary>
        /// <returns></returns>
        private string CreateOldVersionOS()
        {
            var script = new StringBuilder();
            //ActualDate > @StartPeriod

            //если существует актуальная версия в статусе draft или outbus с датой актуальности позже импортируемой
            //то обновление записи 
            script.AppendLine($";With src as (SELECT * FROM {GetTempTableName()} " +
              $"WHERE {rowProcessed} <> 1 " +
              $"AND {nameof(AccountingObject.Oid)} IS NOT NULL " +
              $")");
            script.AppendLine($"MERGE INTO {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"USING src AS source ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                              $"AND target.IsHistory = 0 AND target.Hidden = 0 " +
                              $"AND UPPER(target.{nameof(AccountingObjectExtView.StateObjectRSBUCode)}) IN (N'DRAFT', N'OUTBUS')");
            script.AppendLine($"AND target.ActualDate > @StartPeriod ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($"{GetSetSpecification()}");
            script.AppendLine($";");

            script.AppendLine($"UPDATE source");
            script.AppendLine($"SET source.[{rowProcessed}] = 1");
            script.AppendLine($"FROM (SELECT * FROM {GetTempTableName()} " +
               $"WHERE {rowProcessed} <> 1 " +
               $"AND {nameof(AccountingObject.Oid)} IS NOT NULL " +
               $") AS source");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                              $"AND target.IsHistory = 0 AND target.Hidden = 0 ");
            script.AppendLine($"AND target.ActualDate = @StartPeriod ");


            //обновление существующей истории и вставка если её нет
            script.AppendLine($";With src as (SELECT * FROM {GetTempTableName()} " +
             $"WHERE {rowProcessed} <> 1 " +
             $"AND {nameof(AccountingObject.Oid)} IS NOT NULL " +
             $")");
            script.AppendLine($"MERGE INTO {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"USING src AS source ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                              $"AND target.Hidden = 0 ");
            script.AppendLine($"AND target.ActualDate = @StartPeriod ");
            script.AppendLine($"WHEN MATCHED THEN");
            script.AppendLine($"UPDATE SET ");
            script.AppendLine($"{GetSetSpecification()}");
            script.AppendLine($"WHEN NOT MATCHED BY target THEN ");
            script.AppendLine($"INSERT({GetInsertColumnSpecification()})");
            script.AppendLine($"VALUES({GetOldVersionValuesSpecification()})");
            script.AppendLine($";");

            script.AppendLine($"UPDATE source");
            script.AppendLine($"SET source.[{rowProcessed}] = 1");
            script.AppendLine($"FROM (SELECT * FROM {GetTempTableName()} " +
               $"WHERE {rowProcessed} <> 1 " +
               $"AND {nameof(AccountingObject.Oid)} IS NOT NULL " +
               $") AS source");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"ON source.{nameof(AccountingObject.Oid)} = target.{nameof(AccountingObjectExtView.Oid)} " +
                              $"AND target.Hidden = 0 ");
            script.AppendLine($"AND target.ActualDate = @StartPeriod ");


            return script.ToString();
        }

        /// <summary>
        /// Перечень выражений для обновления текущей версии ОС.
        /// </summary>
        /// <returns></returns>
        protected override string GetSetSpecification()
        {
            var setSpecification = new StringBuilder();

            foreach (var keyValue in ColsNameMapping)
            {
                var colName = keyValue.Value;
                if (!ObjectProperties.Contains(colName))
                {
                    continue;
                }

                var prop = MainType.GetProperty(colName);

                if (setSpecification.Length > 0)
                {
                    setSpecification.Append(",");
                }

                var idColName = "";
                if (colName.ToLower() == "oid")
                {
                    setSpecification.AppendLine($" target.[{colName}] = target.[{colName}]");
                }
                else if (colName.ToLower() == "estateid")
                {
                    setSpecification.AppendLine($" target.[{colName}] = target.[{colName}]");
                }
                else if (IsPrimitiveType(prop.PropertyType))
                {
                    setSpecification.AppendLine($" target.[{colName}] = source.[{colName}]");
                }
                else
                {
                    if (colName == "Status")
                    {
                        idColName = "AccountingStatusID";
                    }
                    else if (colName == "Model")
                    {
                        idColName = "VehicleModelID";
                    }
                    else
                    {
                        idColName = $"{colName}ID";
                    }

                    setSpecification.AppendLine($" target.[{idColName}] = {GetSelectSubQuery(prop, colName)}");
                }
            }
            setSpecification.AppendLine($",target.ImportUpdateDate = getDate()");
            setSpecification.AppendLine($",target.ActualDate = @StartPeriod");
            setSpecification.AppendLine($",target.NonActualDate = @EndPeriod");
            return setSpecification.ToString();
        }

        private string GetSelectSpecification(string prefix = null)
        {
            var val = new StringBuilder();
            var p = string.IsNullOrEmpty(prefix) ? string.Empty : $"{prefix}.";
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "actualdate")
                {
                    return $"{startPeriod}";
                }
                if (x.ColumnName.ToLower() == "nonactualdate")
                {
                    return $"{endPeriod}";
                }
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate")
                {
                    return $"GetDate()";
                }
                return x.IsNullable ? $"{p}[{x.ColumnName}]" : $"ISNULL({p}[{x.ColumnName}],{GetSqlDefaultValue(x)})";
            });

            var setSpecification = new StringBuilder();

            foreach (var keyValue in ColsNameMapping)
            {
                var colName = keyValue.Value;
                if (!ObjectProperties.Contains(colName))
                {
                    continue;
                }
                var prop = MainType.GetProperty(colName);

                if (IsPrimitiveType(prop.PropertyType))
                {
                    if (values.ContainsKey(colName))
                    {
                        values[colName] = $"{p}[{colName}]";
                    }
                }
                else
                {
                    var idColName = $"{colName}ID";
                    if (colName == nameof(AccountingObject.Model))
                    {
                        idColName = "VehicleModelID";
                    }
                    if (values.ContainsKey(idColName))
                    {
                        values[idColName] = $"{GetSelectSubQuery(prop, colName)}";
                    }
                }
            }
            foreach (var sqlExpression in values)
            {
                if (string.IsNullOrEmpty(sqlExpression.Value))
                {
                    continue;
                }
                if (setSpecification.Length > 0)
                {
                    setSpecification.AppendLine(",");
                }
                setSpecification.Append(sqlExpression.Value);
            }

            return setSpecification.ToString();
        }


        /// <summary>
        /// Формирует текст выборки для отложения текущей версии в историю.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string GetSelectNewHistoryItemColumnSpecification(string prefix)
        {
            var insertColumns = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);
            var p = string.IsNullOrEmpty(prefix) ? string.Empty : $"{prefix}.";

            return String.Join(",\r\n", insertColumns.Select(x =>
            {
                if (x == "IsHistory")
                {
                    return "1";
                }

                if (x == "NonActualDate")
                {
                    return $"ISNULL({p}{x}, DATEADD(day , -1 , {startPeriod}))";
                }

                return string.IsNullOrEmpty(prefix) ? $"[{x}]" : $"{prefix}.[{x}]";
            }).ToArray());
        }

        /// <summary>
        /// Формирует текст выборки для вставки новой историчной записи ОС.
        /// </summary>
        /// <returns></returns>
        private string GetOldVersionValuesSpecification()
        {
            var val = new StringBuilder();
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "actualdate")
                {
                    return $"{startPeriod}";
                }
                if (x.ColumnName.ToLower() == "nonactualdate")
                {
                    return $"{endPeriod}";
                }
                if (x.ColumnName.ToLower() == "ishistory")
                {
                    return $"1";
                }
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate")
                {
                    return $"GetDate()";
                }
                return x.IsNullable ? $"source.[{x.ColumnName}]" : $"ISNULL(source.[{x.ColumnName}],{GetSqlDefaultValue(x)})";
            });

            var setSpecification = new StringBuilder();

            foreach (var keyValue in ColsNameMapping)
            {
                var colName = keyValue.Value;
                if (!ObjectProperties.Contains(colName))
                {
                    continue;
                }
                var prop = MainType.GetProperty(colName);

                if (IsPrimitiveType(prop.PropertyType))
                {
                    if (values.ContainsKey(colName))
                    {
                        values[colName] = $"source.[{colName}]";
                    }
                }
                else
                {
                    var idColName = $"{colName}ID";
                    if (colName == nameof(AccountingObject.Model))
                    {
                        idColName = "VehicleModelID";
                    }
                    if (values.ContainsKey(idColName))
                    {
                        values[idColName] = $"{GetSelectSubQuery(prop, colName)}";
                    }
                }
            }
            foreach (var sqlExpression in values)
            {
                if (string.IsNullOrEmpty(sqlExpression.Value))
                {
                    continue;
                }
                if (setSpecification.Length > 0)
                {
                    setSpecification.AppendLine(",");
                }
                setSpecification.Append(sqlExpression.Value);
            }

            return setSpecification.ToString();
        }



        private string GetMovingInsert(int movType)
        {
            var script = new StringBuilder();
            var angle = (movType == 3 || movType == 4) ? "@angleMSFO" : "@angleRSBU";
            var move = (movType == 1 || movType == 3) ? "@movINTRO" : "@movDEPR";
            var amount = "0";
            var inRSBU = (movType == 1 || movType == 2) ? "0" :
                    $"ISNULL((SELECT TOP 1 CASE WHEN t4.Code = N'RSBU' THEN 1 ELSE 0 END FROM {GetTableName(typeof(Consolidation))} AS t1 " +
                    $"LEFT JOIN {GetTableName(typeof(DictObject))} AS t2 on t1.ID=t2.ID " +
                    $"LEFT JOIN {GetTableName(typeof(TypeAccounting))} AS t3 on t1.TypeAccountingID=t3.ID " +
                    $"LEFT JOIN {GetTableName(typeof(DictObject))} AS t4 on t3.ID=t4.ID " +
                    $"WHERE  t1.ID = target.[ConsolidationID]),0)";

            switch (movType)
            {
                case 1:
                    amount = $"target.[{nameof(AccountingObject.InitialCost)}]";
                    break;
                case 2:
                    amount = $"(ISNULL(target.[{nameof(AccountingObject.ResidualCost)}], 0) - ISNULL(target.[{nameof(AccountingObject.InitialCost)}],0))";
                    break;
                case 3:
                    amount = $"target.[{nameof(AccountingObject.InitialCostMSFO)}]";
                    break;
                case 4:
                    amount = $"(ISNULL(target.[{nameof(AccountingObject.ResidualCostMSFO)}], 0) - ISNULL(target.[{nameof(AccountingObject.InitialCostMSFO)}],0))";
                    break;
                default:
                    break;
            }

            script.AppendLine($"target.[ID]"); // [AccountingObjectID]
            script.AppendLine($",NULL"); // [ExternalID]
            script.AppendLine("," + angle); // [AngleID]
            script.AppendLine($",@EndPeriod"); // [Date]
            script.AppendLine($",target.[InventoryNumber]"); // [InventoryNumber]
            script.AppendLine($",target.[ConsolidationID]"); // [ConsolidationID]
            script.AppendLine("," + amount); // [Amount]
            script.AppendLine($",@StartPeriod"); // [Period_Start]
            script.AppendLine($",@EndPeriod"); // [Period_End]           
            script.AppendLine("," + move); // [MovingTypeID]
            script.AppendLine("," + inRSBU); // [InRSBU]
            script.AppendLine($",NEWID()"); // [Oid]
            script.AppendLine($",0"); // [IsHistory]
            script.AppendLine($",GETDATE()"); // [CreateDate]
            script.AppendLine($",@StartPeriod"); // [ActualDate]
            script.AppendLine($",@EndPeriod"); // [NonActualDate]
            script.AppendLine($",GETDATE()"); // [ImportUpdateDate]
            script.AppendLine($",GETDATE()"); // [ImportDate]
            script.AppendLine($",0"); // [Hidden]
            script.AppendLine($",-1"); // [SortOrder]

            return script.ToString();
        }


        private string GetMovingColumns()
        {
            return @"
                   [AccountingObjectID]                  
                  ,[ExternalID]
                  ,[AngleID]
                  ,[Date]
                  ,[InventoryNumber]
                  ,[ConsolidationID]
                  ,[Amount]
                  ,[Period_Start]
                  ,[Period_End]              
                  ,[MovingTypeID]
                  ,[InRSBU]
                  ,[Oid]
                  ,[IsHistory]
                  ,[CreateDate]
                  ,[ActualDate]
                  ,[NonActualDate]
                  ,[ImportUpdateDate]
                  ,[ImportDate]
                  ,[Hidden]
                  ,[SortOrder]
            ";
        }


        /// <summary>
        /// Формирование скрипта связывания существующих ОИ с новыми созданными ОС-ми.
        /// </summary>
        /// <returns></returns>
        private string LinkOSAndEstate()
        {
            var script = new StringBuilder();

            //поиск ОИ для ОС забаланс
            //проверка КА на вхождение в периметр
            //если входит = связываем, нет - помечаем на создание

            script.AppendLine($"UPDATE ss");
            script.AppendLine($"SET ss.EstateID = target.EstateID");
            script.AppendLine($", ss.{oidEstate} = est.Oid");
            script.AppendLine($"FROM {GetTempTableName()} ss");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Subject))} subj ON ss.[{nameof(AccountingObject.SubjectCode)}] = subj.{nameof(Subject.SDP)}");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Society))} og ON subj.[{nameof(Subject.SocietyID)}] = og.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} be ON og.[{nameof(Society.ConsolidationUnitID)}] = be.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} target ON target.{nameof(AccountingObjectExtView.InventoryNumber)} = ss.{nameof(AccountingObject.InventoryNumber)} " +
                $"AND target.{nameof(AccountingObjectExtView.ConsolidationCode)} = be.Code ");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Estate))} est ON target.[{nameof(AccountingObjectExtView.EstateID)}] = est.ID");
            script.AppendLine($"WHERE ss.{nameof(AccountingObject.EstateID)} IS NULL AND ss.{nameof(AccountingObject.OutOfBalance)} = 1 ");
            script.AppendLine($"AND ss.{nameof(AccountingObject.SubjectCode)} IS NOT NULL AND ss.{rowProcessed} <> 1");
            script.AppendLine($"AND target.Hidden = 0 AND target.IsHistory = 0 ");
            script.AppendLine($"AND og.ID IS NOT NULL");


            //поиск ОИ для ОС забаланс
            //проверка КА на вхождение в периметр
            //если входит и не найден ОС по ключу - помечаем на создание      


            script.AppendLine($"UPDATE source");
            script.AppendLine($"SET source.{isNewEstate} = 1");
            script.AppendLine($"FROM (SELECT impl.*, be.Code AS BECode ");
            script.AppendLine($"FROM {GetTempTableName()} impl");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Subject))} subj ON impl.[{nameof(AccountingObject.SubjectCode)}] = subj.{nameof(Subject.SDP)}");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Society))} og ON subj.[{nameof(Subject.SocietyID)}] = og.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} be ON og.[{nameof(Society.ConsolidationUnitID)}] = be.ID");
            script.AppendLine($"WHERE impl.{nameof(AccountingObject.EstateID)} IS NULL AND impl.{nameof(AccountingObject.OutOfBalance)} = 1 ");
            script.AppendLine($"AND impl.{nameof(AccountingObject.SubjectCode)} IS NOT NULL AND impl.{rowProcessed} <> 1");
            script.AppendLine($"AND be.ID IS NOT NULL) AS source");
            script.AppendLine($"LEFT JOIN ( SELECT * FROM {GetTableName(typeof(AccountingObjectExtView))} WHERE Hidden = 0 AND IsHistory = 0) AS target");
            script.AppendLine($"ON source.BECode = target.{nameof(AccountingObjectExtView.ConsolidationCode)}");
            script.AppendLine($"AND source.{nameof(AccountingObject.InventoryNumber)} = target.{nameof(AccountingObjectExtView.InventoryNumber)} ");
            script.AppendLine($"WHERE target.ID IS NULL");



            //поиск ОИ по ОС забаланса стороннего БЕ
            //для НЕ забаланса ОС

            //а) сопоставление по БЕ+инв 
            script.AppendLine($"UPDATE source");
            script.AppendLine($"SET source.{nameof(AccountingObject.EstateID)} = target.{nameof(AccountingObjectExtView.EstateID)}");
            script.AppendLine($"FROM (SELECT impl.* ");
            script.AppendLine($"FROM {GetTempTableName()} impl");
            script.AppendLine($"WHERE impl.{nameof(AccountingObject.EstateID)} IS NULL AND impl.{nameof(AccountingObject.OutOfBalance)} <> 1 ");
            script.AppendLine($"AND impl.{rowProcessed} <> 1");
            script.AppendLine($") AS source");
            script.AppendLine($"INNER JOIN (SELECT os.*, be.Code AS BECode ");
            script.AppendLine($"FROM {GetTableName(typeof(AccountingObjectExtView))} os");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Subject))} subj ON os.[{nameof(AccountingObject.SubjectCode)}] = subj.{nameof(Subject.SDP)}");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Society))} og ON subj.[{nameof(Subject.SocietyID)}] = og.ID");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(DictObject))} be ON og.[{nameof(Society.ConsolidationUnitID)}] = be.ID");
            script.AppendLine($"WHERE os.Hidden = 0 AND os.IsHistory = 0");
            script.AppendLine($") AS target");
            script.AppendLine($"ON target.BECode = source.{nameof(AccountingObject.Consolidation)}");
            script.AppendLine($"AND source.{nameof(AccountingObject.InventoryNumber)} IS NOT NULL AND source.{nameof(AccountingObject.InventoryNumber)} = target.{nameof(AccountingObjectExtView.InventoryNumber)} ");


            //б) сопоставление по кадастровому № +инв 
            script.AppendLine($"UPDATE source");
            script.AppendLine($"SET source.{nameof(AccountingObject.EstateID)} = target.{nameof(AccountingObjectExtView.EstateID)}");
            script.AppendLine($"FROM (SELECT impl.* ");
            script.AppendLine($"FROM {GetTempTableName()} impl");
            script.AppendLine($"WHERE impl.{nameof(AccountingObject.EstateID)} IS NULL AND impl.{nameof(AccountingObject.OutOfBalance)} <> 1 ");
            script.AppendLine($"AND impl.{rowProcessed} <> 1");
            script.AppendLine($") AS source");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"ON source.{nameof(AccountingObject.CadastralNumber)} = target.{nameof(AccountingObjectExtView.CadastralNumber)}");
            script.AppendLine($"AND source.{nameof(AccountingObject.InventoryNumber)} = target.{nameof(AccountingObjectExtView.InventoryNumber)} ");
            script.AppendLine($"AND source.{nameof(AccountingObject.InventoryNumber)} IS NOT NULL");
            script.AppendLine($"AND target.Hidden = 0 AND target.IsHistory = 0");
            script.AppendLine($"");

            //в) сопоставление только по кадастровому № и не равном инв.№
            script.AppendLine($"UPDATE source");
            script.AppendLine($"SET source.{nameof(AccountingObject.EstateID)} = target.{nameof(AccountingObjectExtView.EstateID)}");
            script.AppendLine($"FROM (SELECT impl.* ");
            script.AppendLine($"FROM {GetTempTableName()} impl");
            script.AppendLine($"WHERE impl.{nameof(AccountingObject.EstateID)} IS NULL AND impl.{nameof(AccountingObject.OutOfBalance)} <> 1 ");
            script.AppendLine($"AND impl.{rowProcessed} <> 1");
            script.AppendLine($") AS source");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target");
            script.AppendLine($"ON source.{nameof(AccountingObject.CadastralNumber)} = target.{nameof(AccountingObjectExtView.CadastralNumber)}");
            script.AppendLine($"AND source.{nameof(AccountingObject.InventoryNumber)} IS NOT NULL");
            script.AppendLine($"AND source.{nameof(AccountingObject.InventoryNumber)} <> target.{nameof(AccountingObjectExtView.InventoryNumber)} ");
            script.AppendLine($"AND target.Hidden = 0 AND target.IsHistory = 0");
            script.AppendLine($"");


            //обновляем Oid
            script.AppendLine($"UPDATE imp");
            script.AppendLine($"SET imp.{oidEstate} = est.Oid");
            script.AppendLine($"FROM {GetTempTableName()} imp");
            script.AppendLine($"INNER JOIN {GetTableName(typeof(Estate))} est ON imp.{nameof(AccountingObject.EstateID)} = est.ID");

            //обновляем признак создания ОИ
            script.AppendLine($"UPDATE {GetTempTableName()}");
            script.AppendLine($"SET {isNewEstate} = 1");
            script.AppendLine($"WHERE {nameof(AccountingObject.EstateID)} IS NULL AND {rowProcessed} <> 1");


            return script.ToString();
        }

        /// <summary>
        /// Формирует скрипт создания ОИ
        /// </summary>
        /// <returns></returns>
        private string CreateEstate()
        {
            var script = new StringBuilder();

            //Временные таблицы
            script.AppendLine($"CREATE TABLE #tblTypeEstate (TypeName nvarchar(255))");
            script.AppendLine($"create table #tblReferencedEstate (Position int, RefEstateTableName nvarchar(500), TableName nvarchar(500), COLUMN_Position nvarchar(500), COLUMN_NAME nvarchar(500), DATA_TYPE nvarchar(500), DefaulValue nvarchar(500))");
            script.AppendLine($"CREATE TABLE #tblCreateEstate (SortOrder int, RefEstateTableName nvarchar(500),Position int,  TableName nvarchar(500), StrScript nvarchar(MAX))");
            script.AppendLine($"CREATE TABLE #tblShemEstate (TableName nvarchar(500), ReferencedTableName nvarchar(500))");
            script.AppendLine($"CREATE TABLE #tblGetCreateEstateType ( AOID int, oidAO uniqueidentifier,  oidEstate uniqueidentifier, typeEstate nvarchar(MAX))");
            //Обновление Bulk таблицы ОС
            script.AppendLine($"insert into #tblGetCreateEstateType (AOID,oidAO, oidEstate, typeEstate)");
            script.AppendLine($"select addAO.addAOID, addAO.accOID, NEWID()");
            script.AppendLine($",case");
            script.AppendLine($" --create using data OKOF2014 (OKOFEstates)");
            script.AppendLine($"when addAO.EstateTypeCFA is null and addAO.EstateTypeOKOF2 is not null then addAO.EstateTypeOKOF2");
            script.AppendLine($" --create using data EstateType (AccountingEstates)");
            script.AppendLine($"when addAO.EstateTypeCFA is not null then addAO.EstateTypeCFA");
            script.AppendLine($" --create using EstateOfRulCriteria ");
            script.AppendLine($"when addAO.EstateOfRulCriteria is not null then addAO.EstateOfRulCriteria");
            script.AppendLine($" --create using indicated value CadastralNumber ");
            script.AppendLine($"when addAO.EstateOfRulCriteria is null and addAO.EstateTypeCFA is null and addAO.EstateTypeOKOF2 is null and isnull(addAO.isCadastralObject, 0) <> 0 then 'Cadastral'");
            script.AppendLine($" --If can not determine the type of Estate, then create InventoryObject ");
            //TODO: Временно! Если не удалось определить тип ОИ, то создаем InventoryObject
            script.AppendLine($"else 'InventoryObject' end");
            //-----------------------------------------------
            script.AppendLine($"FROM(select  acc.ID AS 'addAOID', acc.Name, acc.Oid AS 'accOID', NEWID() AS 'estOID', acc.CadastralNumber");
            script.AppendLine($",case");
            script.AppendLine($"when LIntangibleAsset.IDRul1 is not null then 'IntangibleAsset'");
            script.AppendLine($"when LLand.IDRul2 is not null then 'Land'");
            script.AppendLine($"when LBuildingStructure.IDRul3 is not null then 'BuildingStructure'");
            script.AppendLine($"when LVehicle.IDRul4 is not null then 'Vehicle'");
            script.AppendLine($"when LShip.IDRul5 is not null then 'Ship'");
            script.AppendLine($"when LAircraft.IDRul6 is not null then 'Aircraft'");
            script.AppendLine($"end AS EstateOfRulCriteria");
            script.AppendLine($", AccCFA.EstateType AS 'EstateTypeCFA'");
            script.AppendLine($", AccOKOF2.EstateType AS 'EstateTypeOKOF2'");
            script.AppendLine($", case when AccCFA.EstateType is null and(acc.CadastralNumber is not null and REPLACE(acc.CadastralNumber, ' ', '') <> '') then 1 else 0 end AS 'isCadastralObject'");
            script.AppendLine($", acc.isNewEstate");
            script.AppendLine($", acc.oidEstate");
            script.AppendLine($", acc.rowProcessed");
            script.AppendLine($"FROM { GetTempTableName()} AS acc with (nolock)");
            script.AppendLine($"INNER JOIN [EUSI.Accounting].AccountingObjectExtView target with(nolock) ON acc.Oid = target.Oid AND target.Hidden = 0 AND target.IsHistory = 0");
            script.AppendLine($"left join (select '1' AS 'Rul', accRul1.OID AS 'IDRul1' FROM { GetTempTableName()} AS accRul1 with (nolock) ");
            script.AppendLine($"where isnull(accRul1.isNewEstate, 0) = 1 and accRul1.oidEstate is null and accRul1.rowProcessed <> 1 and(accRul1.AccountNumber like N'4%' or accRul1.AccountNumber like N'04%' or accRul1.AccountNumber like N'004%' or RTRIM(accRul1.AccountNumber) like N'97%' or RTRIM(accRul1.AccountNumber) like N'*97%')) AS LIntangibleAsset on acc.OID = LIntangibleAsset.IDRul1");
            script.AppendLine($"left join (select '2' AS 'Rul', accRul2.OID AS 'IDRul2' FROM { GetTempTableName()} AS accRul2 with (nolock) ");
            script.AppendLine($"where isnull(accRul2.isNewEstate, 0) = 1 and accRul2.oidEstate is null and accRul2.rowProcessed <> 1 and accRul2.GroundCategoryID is not null OR isnull(accRul2.GroundCadastralNumber, '') <> '') AS LLand   on acc.OID = LLand.IDRul2");
            script.AppendLine($"left join (select '3' AS 'Rul', accRul3.OID AS 'IDRul3' FROM { GetTempTableName()} AS accRul3 with (nolock) ");
            script.AppendLine($"where isnull(accRul3.isNewEstate, 0) = 1 and accRul3.oidEstate is null and accRul3.rowProcessed <> 1 and(isnull(accRul3.BuildingFullArea, 0) <> 0) OR(isnull(accRul3.BuildingLength, 0) <> 0) OR(accRul3.LayingTypeID is not null) OR(isnull(accRul3.ContainmentVolume, 0) <> 0) OR(isnull(accRul3.DepthWell, 0) <> 0) OR(isnull(accRul3.Height, 0) <> 0)) AS LBuildingStructure   on acc.OID = LBuildingStructure.IDRul3");
            script.AppendLine($"left join (select '4' AS 'Rul', accRul4.OID AS 'IDRul4' FROM { GetTempTableName()} AS accRul4 with (nolock) ");
            script.AppendLine($"where isnull(accRul4.isNewEstate, 0) = 1 and accRul4.oidEstate is null and accRul4.rowProcessed <> 1 and(accRul4.VehicleTypeID is not null) OR(accRul4.SibMeasureID is not null) OR(isnull(accRul4.[Power], 0) <> 0) OR(isnull(accRul4.SerialNumber, '') <> '') OR(isnull(accRul4.EngineSize, 0) <> 0) OR(accRul4.VehicleModelID is not null) OR(isnull(accRul4.YearOfIssue, 0) <> 0) OR(isnull(accRul4.VehicleRegNumber, '') <> '') OR(accRul4.VehicleRegDate is not null) OR(accRul4.VehicleDeRegDate is not null) OR(isnull(accRul4.InOtherSystem, 0) = 1)) AS LVehicle   on acc.OID = LVehicle.IDRul4");
            script.AppendLine($"left join (select '5' AS 'Rul', accRul5.OID AS 'IDRul5' FROM { GetTempTableName()} AS accRul5 with (nolock) ");
            script.AppendLine($"where isnull(accRul5.isNewEstate, 0) = 1 and accRul5.oidEstate is null and accRul5.rowProcessed <> 1 and(isnull(accRul5.ShipName, '') <> '') OR(isnull(accRul5.ShipRegNumber, '') <> '') OR(isnull(accRul5.OldName, '') <> '') OR(accRul5.ShipTypeID is not null) OR(isnull(accRul5.ShipAppointment, '') <> '') OR(accRul5.ShipClassID is not null) OR(isnull(accRul5.BuildYear, 0) <> 0) OR(isnull(accRul5.BuildPlace, '') <> '') OR(isnull(accRul5.ShellMaterial, '') <> '') OR(isnull(accRul5.MainEngineType, '') <> '') OR(isnull(accRul5.MainEngineCount, 0) <> 0) OR(isnull(accRul5.MainEnginePower, 0) <> 0) OR(accRul5.PowerUnitID is not null) OR(isnull(accRul5.[Length], 0) <> 0) OR(accRul5.LengthUnitID is not null) OR(isnull(accRul5.Width, 0) <> 0) OR(accRul5.WidthUnitID is not null) OR(isnull(accRul5.DraughtHard, 0) <> 0) OR(accRul5.DraughtHardUnitID is not null) OR(isnull(accRul5.DraughtLight, 0) <> 0) OR(accRul5.DraughtLightUnitID is not null) OR(isnull(accRul5.MostHeight, 0) <> 0) OR(accRul5.MostHeightUnitID is not null) OR(isnull(accRul5.DeadWeight, 0) <> 0) OR(accRul5.DeadWeightUnitID is not null) OR(isnull(accRul5.SeatingCapacity, 0) <> 0) OR(isnull(accRul5.Harbor, '') <> '') OR(isnull(accRul5.OldHarbor, '') <> '')) AS LShip   on acc.OID = LShip.IDRul5");
            script.AppendLine($"left join (select '6' AS 'Rul', accRul6.OID AS 'IDRul6' FROM { GetTempTableName()} AS accRul6 with (nolock) ");
            script.AppendLine($"where isnull(accRul6.isNewEstate, 0) = 1 and accRul6.oidEstate is null and accRul6.rowProcessed <> 1 and(accRul6.AircraftKindID is not null) OR(accRul6.AircraftTypeID is not null) OR(isnull(accRul6.SerialName, '') <> '') OR(isnull(accRul6.AircraftAppointment, '') <> '') OR(isnull(accRul6.GliderNumber, '') <> '') OR(isnull(accRul6.EngineNumber, '') <> '') OR(isnull(accRul6.PropulsionNumber, '') <> '') OR(accRul6.ProductionDate is not null) OR(isnull(accRul6.MakerName, '') <> '') OR(isnull(accRul6.AirtcraftLocation, '') <> '')) AS LAircraft   on acc.OID = LAircraft.IDRul6");
            script.AppendLine($"left join (select dAccEst.ID AS 'dAccEstID', dAccEst.EstateType, dClassFixedAsset.ID FROM[CorpProp.Base].DictObject AS dClassFixedAsset with(nolock)");
            script.AppendLine($"inner join [CorpProp.NSI].ClassFixedAsset AS CFA with(nolock)  on dClassFixedAsset.ID = CFA.ID");
            script.AppendLine($"left join [CorpProp.Mapping].AccountingEstates AS dAccEst with(nolock)  on dClassFixedAsset.ID = dAccEst.ClassFixedAsset_ID) AS AccCFA on target.ClassFixedAssetID = AccCFA.ID");
            script.AppendLine($"left join (select dOKOFEst.ID AS 'dOKOFEstID', dOKOFEst.EstateType, dOKOF2.ID FROM[CorpProp.Base].DictObject AS dOKOF2 with(nolock)");
            script.AppendLine($"inner join [CorpProp.NSI].OKOF2014 AS OKOF2 with(nolock)  on dOKOF2.ID = OKOF2.ID");
            script.AppendLine($"left join [CorpProp.Mapping].OKOFEstates AS dOKOFEst with(nolock)  on dOKOF2.ID = dOKOFEst.OKOF2014_ID) AccOKOF2 on target.OKOF2014ID = AccOKOF2.ID");
            script.AppendLine($") AS addAO");
            script.AppendLine($"where isnull(addAO.isNewEstate, 0) = 1 and addAO.oidEstate is null and addAO.rowProcessed <> 1");

            script.AppendLine($"update bulkAO set bulkAO.oidEstate = getEstateType.oidEstate, bulkAO.typeEstate = getEstateType.typeEstate");
            script.AppendLine($"from { GetTempTableName()} AS bulkAO ");
            script.AppendLine($"inner join #tblGetCreateEstateType as getEstateType on bulkAO.oid=getEstateType.oidAO");

            script.AppendLine($"DROP TABLE #tblGetCreateEstateType");

            //собираем информацию о типах ОИ
            script.AppendLine($"insert into #tblTypeEstate (typeName) select typeEstate FROM {GetTempTableName()} group by typeEstate");
            //Сбор данных о наследниках Estate
            script.AppendLine($"INSERT INTO #tblShemEstate (TableName, ReferencedTableName)");
            script.AppendLine($"SELECT");
            script.AppendLine($"OBJECT_NAME(f.parent_object_id) AS TableName,");
            script.AppendLine($"OBJECT_NAME(f.referenced_object_id) AS ReferenceTableName");
            script.AppendLine($"FROM    sys.foreign_keys AS f");
            script.AppendLine($"INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id");
            script.AppendLine($"INNER JOIN sys.objects AS o ON o.OBJECT_ID = fc.referenced_object_id");
            script.AppendLine($"WHERE SCHEMA_NAME(f.SCHEMA_ID) = '{GetTableSchemaName(typeof(Estate))}' and COL_NAME(fc.parent_object_id, fc.parent_column_id) = 'ID' and SCHEMA_NAME(o.SCHEMA_ID) = '{GetTableSchemaName(typeof(Estate))}' ORDER BY ReferenceTableName");
            //
            script.AppendLine($"declare @RefTableName nvarchar(500) ,@RefEstateTableName nvarchar(500) , @endTableName  nvarchar(500)='Estate'");
            script.AppendLine($"declare @tPosition int = 1");
            //Курсор формирования скриптов создания ОИ от Estate
            script.AppendLine($"DECLARE @TypeEstate nvarchar(255)");
            script.AppendLine($"DECLARE typeEstate_cur CURSOR LOCAL FOR");
            script.AppendLine($"SELECT typeName FROM  #tblTypeEstate");
            script.AppendLine($"OPEN typeEstate_cur");
            script.AppendLine($"FETCH NEXT FROM typeEstate_cur INTO @TypeEstate");
            script.AppendLine($"WHILE @@FETCH_STATUS = 0");
            script.AppendLine($"BEGIN");
            script.AppendLine($"set @RefTableName = @TypeEstate");
            script.AppendLine($"set @RefEstateTableName = @RefTableName");
            script.AppendLine($"while (isnull(@RefTableName, '') <> '') ");
            script.AppendLine($"Begin");
            script.AppendLine($"INSERT INTO #tblReferencedEstate(Position, RefEstateTableName, TableName,COLUMN_Position,COLUMN_NAME, DATA_TYPE, DefaulValue)");
            script.AppendLine($"SELECT @tPosition, @RefEstateTableName, @RefTableName");
            script.AppendLine($",case ");
            script.AppendLine($"when COLUMN_NAME = 'ID' then '01'");
            script.AppendLine($"when COLUMN_NAME = 'OID' then '02'");
            script.AppendLine($"else '1' + cast(ROW_NUMBER() OVER(ORDER BY COLUMN_NAME ASC) AS nvarchar(255))");
            script.AppendLine($"end");
            script.AppendLine($"AS COLUMN_Position");
            script.AppendLine($", COLUMN_NAME, DATA_TYPE");
            script.AppendLine($",case ");
            script.AppendLine($"when COLUMN_NAME = 'ID' then 'est.id'");
            script.AppendLine($"when DATA_TYPE = 'int' then '0'");
            script.AppendLine($"when DATA_TYPE = 'float' then '0'");
            script.AppendLine($"when DATA_TYPE = 'bit' then '0'");
            script.AppendLine($"when DATA_TYPE = 'uniqueidentifier' then 'NEWID()'");
            script.AppendLine($"end AS defaulValue");
            script.AppendLine($"FROM INFORMATION_SCHEMA.columns");
            script.AppendLine($"WHERE TABLE_SCHEMA = '{GetTableSchemaName(typeof(Estate))}' and TABLE_NAME = @RefTableName and IS_NULLABLE = 'NO' and COLUMN_NAME not in ('RowVersion')");
            script.AppendLine($"and TABLE_NAME<>'Estate'");
            script.AppendLine($"order by  COLUMN_NAME");
            script.AppendLine($"if (isnull(@RefTableName, '') <> 'Estate') ");
            script.AppendLine($"SELECT @RefTableName = ReferencedTableName, @tPosition = @tPosition + 1 FROM #tblShemEstate WHERE TableName=@RefTableName");
            script.AppendLine($"else");
            script.AppendLine($"set @RefTableName = NULL");
            script.AppendLine($"End");
            script.AppendLine($"INSERT INTO #tblCreateEstate (SortOrder,RefEstateTableName,Position, TableName, StrScript)");
            script.AppendLine($"SELECT(ROW_NUMBER() OVER(ORDER BY tmp.RefEstateTableName, tmp.Position desc)), tmp.RefEstateTableName ,  tmp.Position, tmp.TableName, tmp3.str3Script");
            script.AppendLine($"FROM #tblReferencedEstate AS tmp with (nolock)");
            script.AppendLine($"left join (");
            script.AppendLine($"SELECT tmp2.Position, tmp2.RefEstateTableName, 'INSERT INTO [{GetTableSchemaName(typeof(Estate))}].' + tmp2.TableName");
            script.AppendLine($"+ ' (' + ");
            script.AppendLine($"STUFF((SELECT case when ccm.TableName = 'Estate' and ccm.COLUMN_NAME = 'ID' then '' else ', ' + ccm.COLUMN_NAME end + ''");
            script.AppendLine($"FROM #tblReferencedEstate AS ccm with (nolock)");
            script.AppendLine($"WHERE ccm.Position = tmp2.Position and ccm.RefEstateTableName = tmp2.RefEstateTableName");
            script.AppendLine($"order by ccm.COLUMN_Position FOR XML PATH('')),1,1,'')");
            script.AppendLine($"+ ') SELECT ' + ");
            script.AppendLine($"STUFF((SELECT case when ccm.TableName = 'Estate' and ccm.COLUMN_NAME = 'ID' then '' else ',' + ccm.DefaulValue end + ' '");
            script.AppendLine($"FROM #tblReferencedEstate AS ccm with (nolock)");
            script.AppendLine($"WHERE ccm.Position = tmp2.Position  and ccm.RefEstateTableName = tmp2.RefEstateTableName");
            script.AppendLine($"order by ccm.COLUMN_Position FOR XML PATH('')),1,1,'')");
            script.AppendLine($"+ ' FROM { GetTempTableName()} AS ao left outer join {GetTableName(typeof(Estate))} AS est on ao.oidEstate=est.oid left join [{GetTableSchemaName(typeof(Estate))}].'");
            script.AppendLine($"+ tmp2.TableName + ' AS obj on est.ID=obj.ID  WHERE ao.typeEstate = ''' + tmp2.RefEstateTableName + ''' and est.oid is not null and obj.id is null'");
            script.AppendLine($"AS str3Script");
            script.AppendLine($"FROM #tblReferencedEstate AS tmp2 with (nolock)) AS tmp3 on tmp.Position=tmp3.Position and tmp.RefEstateTableName=tmp3.RefEstateTableName");
            script.AppendLine($"left join { GetTempTableName()} AS aoBulk on tmp.RefEstateTableName=aoBulk.typeEstate");
            script.AppendLine($"group by tmp.Position,tmp.RefEstateTableName, tmp.TableName, tmp3.str3Script");
            script.AppendLine($"order by tmp.RefEstateTableName,tmp.Position desc");
            script.AppendLine($"delete FROM #tblReferencedEstate");
            script.AppendLine($"FETCH NEXT FROM typeEstate_cur INTO @TypeEstate");
            script.AppendLine($"END");
            script.AppendLine($"CLOSE typeEstate_cur");
            script.AppendLine($"DEALLOCATE typeEstate_cur");

            script.AppendLine($"DROP TABLE #tblTypeEstate");
            script.AppendLine($"DROP TABLE #tblReferencedEstate");
            script.AppendLine($"DROP TABLE #tblShemEstate");


            //Создание Estate для ОС
            script.AppendLine($"declare @ExecuteDate datetime = getdate()");
            script.AppendLine($"INSERT INTO {GetTableName(typeof(Estate))} (Oid, [Name],[NameEUSI],[NameByDoc], EstateStatus, Hidden, IsHistory, IsNonCoreAsset, OutOfBalance, SortOrder, [CreateDate],[ActualDate],[NonActualDate],[ImportUPDATEDate],[ImportDate])");
            script.AppendLine($"SELECT ao.oidEstate, ao.Name, ao.NameEUSI, ao.NameByDoc, 0, 0, 0, 0, 0, 0, @ExecuteDate, @StartPeriod, NULL, @ExecuteDate, @ExecuteDate");
            script.AppendLine($"FROM { GetTempTableName()} AS ao  with (nolock) ");
            script.AppendLine($"left outer join {GetTableName(typeof(Estate))} AS est with(nolock) on ao.oidEstate = est.oid");
            script.AppendLine($"WHERE ao.typeEstate is not null and est.oid is null and(isnull(ao.isNewEstate, 0) = 1 and ao.EstateID is null and ao.rowProcessed <> 1)");
            //Курсор для выполнения сформированных скриптов создания ОИ от Estate
            script.AppendLine($"DECLARE @StrScript nvarchar(max)");
            script.AppendLine($"DECLARE CreateEstate_cur CURSOR LOCAL FOR");
            script.AppendLine($"SELECT StrScript FROM  #tblCreateEstate");
            script.AppendLine($"order by RefEstateTableName, SortOrder");
            script.AppendLine($"OPEN CreateEstate_cur");
            script.AppendLine($"FETCH NEXT FROM CreateEstate_cur INTO @StrScript");
            script.AppendLine($"WHILE @@FETCH_STATUS = 0");
            script.AppendLine($"BEGIN");
            script.AppendLine($"execute(@StrScript)");
            script.AppendLine($"FETCH NEXT FROM CreateEstate_cur INTO @StrScript");
            script.AppendLine($"END");
            script.AppendLine($"CLOSE CreateEstate_cur");
            script.AppendLine($"DEALLOCATE CreateEstate_cur");


            script.AppendLine($"DROP TABLE #tblCreateEstate");

            return script.ToString();
        }

       

        /// <summary>
        /// Формирует скрипт связывания ОИ с ОС.
        /// </summary>
        /// <returns></returns>
        private string LinkAccountingObjectAndEstate()
        {
            var script = new StringBuilder();
            //Создание связи ОС и ОИ
            script.AppendLine($"UPDATE acc set acc.estateid= est.id");
            script.AppendLine($"FROM { GetTempTableName()} AS ao  with (nolock) ");
            script.AppendLine($"left outer join {GetTableName(typeof(Estate))} AS est  with(nolock) on ao.oidEstate = est.oid");
            script.AppendLine($"left join {GetTableName(typeof(AccountingObjectTbl))} AS acc  with(nolock) on ao.OID = acc.OID and isnull(acc.Hidden,0)=0");
            script.AppendLine($"WHERE ao.typeEstate is not null and est.oid is not null and (isnull(ao.isNewEstate, 0) = 1 and acc.EstateID is null and ao.rowProcessed <> 1)");

            return script.ToString();
        }

        /// <summary>
        /// Получение информации о количестве созданных ОИ по типам.
        /// </summary>
        /// <returns></returns>
        private string GetCountCreateEstate()
        {
            var script = new StringBuilder();
            script.AppendLine($"SELECT ao.typeEstate,count(ao.typeEstate) AS CountCreateObject");
            script.AppendLine($"FROM { GetTempTableName()} AS ao  with (nolock) ");
            script.AppendLine($"left outer join {GetTableName(typeof(Estate))} AS est  with(nolock) on ao.oidEstate = est.oid");
            script.AppendLine($"left join {GetTableName(typeof(AccountingObjectTbl))} AS acc  with(nolock) on ao.ID = acc.id");
            script.AppendLine($"WHERE ao.typeEstate is not null and est.oid is not null");
            script.AppendLine($"group by ao.typeEstate");

            return script.ToString();
        }
    }
}
