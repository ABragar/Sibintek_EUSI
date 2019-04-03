using Base;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.Import;
using CorpProp.Entities.NSI;
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

namespace EUSI.Import.BulkMerge
{
    public class OSQueryBuilder : QueryBuilder
    {
        private OBUVersionControl _version;

        public OSQueryBuilder(Dictionary<string, string> colsNameMapping, Type type, OBUVersionControl version) : base(colsNameMapping, type)
        {
            _version = version;
        }

        private string GetJoinCondition(string target = null, string source = null)
        {
            if (String.IsNullOrWhiteSpace(target))
                target = "target";
            if (String.IsNullOrWhiteSpace(source))
                source = "source";

            return             $"\r\n {target}.Hidden = 0 AND" +
                               $"\r\n {target}.IsHistory = 0 AND" +
                               $"\r\n (({target}.ConsolidationCode = {source}.Consolidation AND" +
                               $"\r\n {target}.Number = {source}.EUSINumber AND" +
                               $"\r\n ISNULL({target}.InventoryNumber, '') = COALESCE({source}.InventoryNumber, ISNULL({target}.InventoryNumber, '')))" +
                               $"\r\n OR {target}.ConsolidationCode = {source}.Consolidation AND {target}.Number = {source}.EUSINumber)";
             
        }

        public override string BuildMergeQuery(ImportHistory history)
        {
            var mergeSqlScript = new StringBuilder();

            mergeSqlScript.AppendLine(
                $"IF OBJECT_ID(N'tempdb..#OSActionsIds') IS NOT NULL DROP TABLE #OSActionsIds CREATE TABLE #OSActionsIds(Change VARCHAR(20), ID int, Oid uniqueidentifier NULL)");
           

           
            mergeSqlScript.AppendLine($"declare @StartPeriod dateTime = {GetStartPeriod()}");
            mergeSqlScript.AppendLine($"declare @EndPeriod dateTime = {GetEndPeriod()}");

            mergeSqlScript.AppendLine($"declare @modified table(ID int)");
            mergeSqlScript.AppendLine($"declare @errorTable Table(errorRowNum int, description nvarchar(max))");

            #region Проверка значений в атрибутах: Номер ЕУСИ, БЕ, Инвентарный номер

            // Проверка значений в атрибутах: Номер ЕУСИ, БЕ, Инвентарный номер
            mergeSqlScript.AppendLine($"declare @MappingTable Table(rowid uniqueidentifier, " +
                                      $"rowNum int, " +
                                      $"ID int, " +
                                      $"Consolidation nvarchar(max), " +
                                      $"EUSINumber int, " +
                                      $"InventoryNumber nvarchar(max))");

           
            
            mergeSqlScript.AppendLine($"PRINT 'Consolidation, EUSINumber, InventoryNumber mapping Table'");
            mergeSqlScript.AppendLine(
                "INSERT INTO @MappingTable(rowNum,rowID, ID,Consolidation, EUSINumber, InventoryNumber) " +
                "\r\nSELECT ROW_NUMBER() OVER(ORDER BY target.id) AS rowNum" +
                "\r\n, source.rowid" +
                "\r\n, target.ID" +
                "\r\n, source.Consolidation" +
                "\r\n, source.EUSINumber" +
                "\r\n, source.InventoryNumber" +
                $"\r\nFROM {GetTempTableName()} source" +
                $"\r\nLEFT JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target on {GetJoinCondition()}");
            mergeSqlScript.AppendLine($"PRINT 'INSERT ErrorTable'");
            mergeSqlScript.AppendLine($"INSERT INTO @errorTable " +
                                      $"\r\nSELECT rowNum as errorRowNum, " +
                                      $"\r\nN'В Системе не найден ОС/НМА для обновления. Проверьте значения в атрибутах: Номер ЕУСИ, БЕ, Инвентарный номер' " +
                                      $"\r\nFROM @MappingTable WHERE Id IS NULL");
            mergeSqlScript.AppendLine($"PRINT 'DELETE source'");
            mergeSqlScript.AppendLine($"DELETE t " +
                                      $"\r\nFROM {GetTempTableName()} t" +
                                      $"\r\nINNER JOIN @MappingTable m on t.rowId = m.rowid" +
                                      $"\r\nWHERE m.ID IS NULL");

            //обновление временной таблицы для проставления Oid-ов найденных ОС
            mergeSqlScript.AppendLine($"UPDATE t " + 
                                      $"\r\n SET t.Oid = os.Oid" +
                                      $"\r\n FROM {GetTempTableName()} t" +
                                      $"\r\n LEFT JOIN {GetTableName(typeof(AccountingObjectExtView))} AS os ON {GetJoinCondition("os","t")}" +
                                      $"\r\n ");

            #endregion Проверка значений в атрибутах: Номер ЕУСИ, БЕ, Инвентарный номер

            //проверка номера заявки и номера позиции
            if (ColsNameMapping.ContainsValue(nameof(AccountingObject.CreatingFromER))
            || ColsNameMapping.ContainsValue(nameof(AccountingObject.CreatingFromERPosition)))
            {
                CheckCreatingFromERAndPosition(mergeSqlScript);
                DeleteMappingForCreatingFromERAndPosition();
            }

            #region ОКТМО-Publishcode

            //ОКТМО -publishCode
            mergeSqlScript.AppendLine($"declare @oktmoMappingTable Table(rowid uniqueidentifier, rowNum int, oktmoid int)");

            mergeSqlScript.AppendLine($"UPDATE t SET OktmoID = x.ID" +
                                      $"\r\nFROM {GetTempTableName()} t" +
                                      $"\r\nINNER JOIN (SELECT d.ID, PublishCode, d.Hidden " +
                                      $"\r\nFROM {GetTableName(typeof(OKTMO))} i " +
                                      $"\r\nINNER JOIN [CorpProp.Base].DictObject d  on  d.ID = i.ID)x " +
                                      $"\r\nON x.PublishCode = t.OKTMO AND x.Hidden = 0");

            mergeSqlScript.AppendLine($"SELECT * FROM {GetTempTableName()}");

            mergeSqlScript.AppendLine(
                "INSERT INTO @oktmoMappingTable(rowNum,rowID, oktmoid) " +
                "\r\nSELECT ROW_NUMBER() OVER(ORDER BY source.id) AS rowNum" +
                "\r\n, source.rowid" +
                "\r\n, source.oktmoid" +
                $"\r\nFROM {GetTempTableName()} source");

            mergeSqlScript.AppendLine($"INSERT INTO @errorTable " +
                                      $"\r\nSELECT rowNum as errorRowNum, " +
                                      $"\r\nN'Неверный код ОКТМО' " +
                                      $"\r\nFROM @oktmoMappingTable WHERE oktmoid IS NULL");

            mergeSqlScript.AppendLine($"DELETE t " +
                                      $"\r\nFROM {GetTempTableName()} t" +
                                      $"\r\nWHERE oktmoid IS NULL");



            #endregion ОКТМО-Publishcode
            

            //ActualDate = @StartPeriod - update record
            mergeSqlScript.AppendLine($"PRINT 'MERGE ActualDate = @StartPeriod - update record'");
            mergeSqlScript.AppendLine($";With src as (SELECT * FROM {GetTempTableName()})");
            mergeSqlScript.AppendLine($"MERGE INTO {GetTableName(typeof(AccountingObjectExtView))} AS target");
            mergeSqlScript.AppendLine($"USING src AS source ON {GetJoinCondition()}");
            mergeSqlScript.AppendLine($"AND target.ActualDate = @StartPeriod ");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()};");
            //mergeSqlScript.AppendLine($"OUTPUT $action, Inserted.ID, Inserted.Oid INTO #OSActionsIds ;");

          

            mergeSqlScript.AppendLine($"PRINT 'DELETE source if ActualDate = @StartPeriod'");
            mergeSqlScript.AppendLine($"DELETE source FROM {GetTempTableName()} as source " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target ON" +
                                      $"\r\n {GetJoinCondition()}" +
                                      $"\r\n AND target.ActualDate = @StartPeriod");
            
            //ActualDate < @StartPeriod - CreateNewVersion (history)           
            mergeSqlScript.AppendLine($"PRINT 'INSERT INTO {GetTableName(MainType)}'");
            mergeSqlScript.AppendLine($"INSERT INTO {GetTableName(MainType)}({GetInsertColumnSpecification()})");
            //mergeSqlScript.AppendLine($"OUTPUT N'INSERTED', Inserted.ID, Inserted.Oid INTO #OSActionsIds");
            mergeSqlScript.AppendLine($"SELECT {GetSelectNewHistoryItemColumnSpecification("target")} " +
                                      $"\r\nFROM  {GetTableName(typeof(AccountingObjectExtView))} target" +
                                      $"\r\nINNER JOIN {GetTempTableName()} source" +
                                      $"\r\nON {GetJoinCondition()}" +
                                      $"\r\nAND target.ActualDate < @StartPeriod ");

            //update current
            mergeSqlScript.AppendLine($"PRINT 'MERGE update current'");
            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY InventoryNumber) as cnt FROM {GetTempTableName()}) AS subquery WHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO {GetTableName(typeof(AccountingObjectExtView))} AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON {GetJoinCondition()} " +
                                      $"\r\nAND target.ActualDate < @StartPeriod ");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()};");
            //mergeSqlScript.AppendLine($"OUTPUT $action, Inserted.ID, Inserted.Oid INTO #OSActionsIds ;");
                       
            
            mergeSqlScript.AppendLine($"PRINT 'DELETE source if ActualDate < @StartPeriod'");
            mergeSqlScript.AppendLine($"DELETE source FROM {GetTempTableName()} as source " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target ON" +
                                      $"\r\n {GetJoinCondition()}" +
                                      $"\r\n AND target.ActualDate = @StartPeriod");

            //ActualDate > @StartPeriod
            //если существует актуальная версия в статусе draft или outbus с датой актуальности позже импортируемой - то обновление записи 
            mergeSqlScript.AppendLine($"PRINT 'MERGE draft | outbus -> updateVersion'");

            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY InventoryNumber) as cnt FROM {GetTempTableName()}) AS subquery WHERE cnt = 1)");
            mergeSqlScript.AppendLine($"\r\nMERGE INTO {GetTableName(typeof(AccountingObjectExtView))} AS target");
            mergeSqlScript.AppendLine($"\r\nUSING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"\r\nON {GetJoinCondition()}" +
                                      $"\r\nAND target.ActualDate > @StartPeriod " +
                                      $"\r\nAND target.StateObjectRSBUID IS NOT null" +
                                      $"\r\nAND (target.StateObjectRSBUCode = 'draft' or target.StateObjectRSBUCode = 'outbus') ");
            mergeSqlScript.AppendLine($"\r\nWHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"\r\nUPDATE SET ");
            mergeSqlScript.AppendLine($"\r\n{GetSetSpecification()};");
            //mergeSqlScript.AppendLine($"OUTPUT  $action, Inserted.ID, Inserted.Oid INTO #OSActionsIds ;");

            mergeSqlScript.AppendLine($"DELETE source FROM {GetTempTableName()} as source " +                                    
                                      $"\r\n WHERE source.[Oid] IN (SELECT Oid FROM #OSActionsIds)");

            //создание историчной записи "задним" числом
            mergeSqlScript.AppendLine($"PRINT 'MERGE CreateOLDVersion'");        
            
            //обновление существующей истории
            mergeSqlScript.AppendLine($";With src as (SELECT * FROM {GetTempTableName()})");
            mergeSqlScript.AppendLine($"MERGE INTO {GetTableName(typeof(AccountingObjectExtView))} AS target");
            mergeSqlScript.AppendLine($"USING src AS source");
            mergeSqlScript.AppendLine($"ON  " +
                                      $"\r\n target.Hidden = 0 " +
                                      $"\r\n AND target.ConsolidationCode = source.Consolidation " +
                                      $"\r\n AND target.Number = source.EUSINumber " +
                                      $"\r\n AND ISNULL(target.InventoryNumber,'') = COALESCE(source.InventoryNumber, ISNULL(target.InventoryNumber, '')) " +
                                      $"\r\n AND target.ActualDate = @StartPeriod ");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()}");           
            mergeSqlScript.AppendLine($"WHEN NOT MATCHED BY target THEN ");
            mergeSqlScript.AppendLine($"INSERT({GetInsertColumnSpecification()})");
            mergeSqlScript.AppendLine($"VALUES({GetOldVersionValuesSpecification()});");
            //mergeSqlScript.AppendLine($"OUTPUT  $action, Inserted.ID, Inserted.Oid INTO #OSActionsIds ;");



            //SetIsDisput(mergeSqlScript);

            //UpdateEstate(mergeSqlScript);

            //SetIsEnergy(mergeSqlScript);

            //LinkHistoryObject(mergeSqlScript, history);

            mergeSqlScript.AppendLine($";IF OBJECT_ID(N'tempdb..#ModifiedRecords') IS NOT NULL " +
                                        $"\r\nDROP TABLE #ModifiedRecords CREATE TABLE #ModifiedRecords(ID int)");

            mergeSqlScript.AppendLine($"SELECT count(*) as 'RelationValidationResult'" +
                                      $"\r\nFROM[CorpProp.Accounting].AccountingObject a " +
                                      $"\r\nINNER JOIN #ModifiedRecords t" +
                                      $"\r\nON a.ID = t.ID " +
                                      $"\r\nINNER JOIN [CorpProp.Law].[Right] r " +
                                      $"\r\nON r.EstateID = a.EstateID " +
                                      $"\r\nAND r.RegNumber = a.RegNumber ");

            mergeSqlScript.AppendLine($"SELECT * FROM #OSActionsIds");

            mergeSqlScript.AppendLine($"SELECT count(*) as 'TotalRecords' FROM #ModifiedRecords");

            mergeSqlScript.AppendLine($"SELECT ID as Modified FROM #ModifiedRecords");

            mergeSqlScript.AppendLine($"SELECT errorRowNum, description from @errorTable");

            mergeSqlScript.AppendLine($"DROP table #OSActionsIds");          
            mergeSqlScript.AppendLine($"DROP table #ModifiedRecords");

            return mergeSqlScript.ToString();
        }

        /// <summary>
        /// Удаление из мэппинга колонок, полученных из файла имопрта колонки CreatingFromER, CreatingFromERPosition,
        /// чтобы дальнейшие инструкции sql не обновили значения в существующих ОС/НМа Системы
        /// </summary>
        private void DeleteMappingForCreatingFromERAndPosition()
        {
            List<string> itemsToRemove = new List<string>();

            itemsToRemove = ColsNameMapping
                .Where(v => v.Value == (nameof(AccountingObject.CreatingFromER))
                || v.Value == (nameof(AccountingObject.CreatingFromERPosition)))
                .Select(s => s.Key)
                .ToList() ;
            foreach (string item in itemsToRemove)
            {
                ColsNameMapping.Remove(item);
            }
        }

        /// <summary>
        /// Сравнение номера заявки и номера позиции заявки в системе и загружаемом шаблоне.
        /// </summary>
        /// <param name="mergeSqlScript"></param>
        private void CheckCreatingFromERAndPosition(StringBuilder mergeSqlScript)
        {
            //14492
            //если заполнен Номер заявки и Номер позиции, то у найденного ОС значения в этих полях должны совпадать
            //если не совпадают - пишем ошибку
            mergeSqlScript.AppendLine($"declare @AdditionalTransferTable Table(rowid uniqueidentifier, " +
                                      $"rowNum int, " +
                                      $"ID int, " +
                                      $"Consolidation nvarchar(max), " +
                                      $"EUSINumber int, " +
                                      $"InventoryNumber nvarchar(max))");
            mergeSqlScript.AppendLine($"PRINT 'INSERT ErrorTable (CreatingFromER and CreatingFromERPosition)'");
            mergeSqlScript.AppendLine(
                "INSERT INTO @AdditionalTransferTable(rowNum,rowID, ID,Consolidation, EUSINumber, InventoryNumber) " +
                "\r\nSELECT source.RowNumb" +
                "\r\n, source.rowid" +
                "\r\n, target.ID" +
                "\r\n, source.Consolidation" +
                "\r\n, source.EUSINumber" +
                "\r\n, source.InventoryNumber" +
                $"\r\nFROM {GetTempTableName()} source" +
                $"\r\nLEFT JOIN {GetTableName(typeof(AccountingObjectExtView))} AS target on {GetJoinCondition()}");
            mergeSqlScript.AppendLine(
                $"WHERE (source.CreatingFromER IS NOT NULL AND (target.CreatingFromER IS NULL OR source.CreatingFromER <> target.CreatingFromER )) " +
                $"OR (source.CreatingFromERPosition IS NOT NULL AND (target.CreatingFromERPosition IS NULL OR source.CreatingFromERPosition <> target.CreatingFromERPosition))");

            mergeSqlScript.AppendLine($"INSERT INTO @errorTable " +
                                      $"\r\nSELECT rowNum as errorRowNum, " +
                                      $"\r\nN'Номер заявки и номер позиции заявки в системе и загружаемом шаблоне не совпадают'" +
                                      $"\r\nFROM @AdditionalTransferTable");            
            
        }

        private void LinkHistoryObject(StringBuilder mergeSqlScript, ImportHistory history)
        {
            mergeSqlScript.AppendLine(
                $"\r\nINSERT INTO {GetTableName(typeof(ImportObject))} (Entity_ID, Entity_TypeName, ImportHistoryOid, Type, CreateDate)" +
                $"\r\n SELECT ao.ID, '{typeof(AccountingObject).GetTypeName()}', '{history.Oid}', 1, GetDate() FROM" +
                $"\r\n {GetTableName(typeof(AccountingObject))} ao" +
                $"\r\n INNER JOIN @modified m ON ao.ID = m.ID");

            //todo ф-л привязки созданных ОИ реализовать при создании ОИ
        }

        /// <summary>
        /// Устанавливает признак "Энергоэффективное оборудование" у объекта ОИ.
        /// </summary>
        /// <param name="mergeSqlScript"></param>
        private void SetIsEnergy(StringBuilder mergeSqlScript)
        {
            mergeSqlScript.AppendLine($"PRINT 'SetIsEnergy'");
            mergeSqlScript.AppendLine($" UPDATE i" +
                " SET IsEnergy = 1 " +
                $" FROM {GetTableName(MainType)} target " +
                " INNER JOIN @modified m ON target.ID = m.ID " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(Estate))} as estate ON estate.ID = target.estateID" +
                                      $"\r\nINNER JOIN {GetTableName(typeof(InventoryObject))} i on i.ID = estate.ID " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(EnergyLabel))} el on i.EnergyLabelID = i.ID " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(DictObject))} el_d on el_d.ID = el.ID " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(OKOF2014))} as okof2014 on okof2014.ID = estate.OKOF2014ID " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(DictObject))} as okof2014_d ON okof2014_d.ID = okof2014.ID " +
                                      $"\r\nWHERE okof2014_d.Code  = ANY " +
                                      $"\r\n(SELECT hf.CodeOKOF2 " +
                                      $"\r\nFROM {GetTableName(typeof(HighEnergyEfficientFacility))} hf " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(DictObject))} d ON d.ID = hf.ID" +
                                      $"\r\nWHERE d.Hidden = 0 AND hf.CodeOKOF2 = okof2014_d.Code) " +
                                      $"\r\nOR okof2014_d.Code  = ANY " +
                                      $"\r\n(SELECT hf2.CodeOKOF2 " +
                                      $"\r\nFROM {GetTableName(typeof(HighEnergyEfficientFacilityKP))} hf2 " +
                                      $"\r\nINNER JOIN {GetTableName(typeof(DictObject))} d ON d.ID = hf2.ID" +
                                      $"\r\nWHERE d.Hidden = 0 AND hf2.CodeOKOF2 = okof2014_d.Code) " +
                                      $"\r\nAND i.EnergyDocsExist IS NOT NULL " +
                                      $"\r\nAND el_d.Code IN (SELECT label FROM(VALUES('A'), ('A+'), ('A++'))x(label))");
        }

        /// <summary>
        /// Обновляет данные в объекте имущества на основании данных ОС/НМА.
        /// </summary>
        /// <param name="mergeSqlScript"></param>
        private void UpdateEstate(StringBuilder mergeSqlScript)
        {
            mergeSqlScript.AppendLine($"UPDATE target" +
                                      $"\r\nSET {nameof(UnfinishedConstruction.StartDateUse)} = a.{nameof(AccountingObject.StartDateUse)}" +
                                      $"\r\nFROM {GetTableName(typeof(UnfinishedConstruction))} target" +
                                      $"\r\nINNER JOIN {GetTableName(typeof(AccountingObject))} a" +
                                      $"\r\nON a.EstateID = target.ID" +
                                      $"\r\nINNER JOIN @modified m ON a.ID = m.ID"
                                      );

            mergeSqlScript.AppendLine($"UPDATE target" +
                                      $"\r\nSET {nameof(Cadastral.CadastralNumber)} = a.{nameof(AccountingObject.CadastralNumber)}" +
                                      $"\r\nFROM {GetTableName(typeof(Cadastral))} target" +
                                      $"\r\nINNER JOIN {GetTableName(typeof(AccountingObject))} a" +
                                      $"\r\nON a.EstateID = target.ID" +
                                      $"\r\nINNER JOIN @modified m ON a.ID = m.ID"
            );

            mergeSqlScript.AppendLine($"UPDATE target" +
                                      $"\r\nSET {nameof(Vehicle.YearOfIssue)} = a.{nameof(AccountingObject.YearOfIssue)}" +
                                      $"\r\n,{nameof(Vehicle.VehicleCategoryID)} = a.{nameof(AccountingObject.VehicleCategoryID)}" +
                                      $"\r\n,{nameof(Vehicle.DieselEngine)} = a.{nameof(AccountingObject.DieselEngine)}" +
                                      $"\r\n,{nameof(Vehicle.SibMeasureID)} = a.{nameof(AccountingObject.SibMeasureID)}" +
                                      $"\r\n,{nameof(Vehicle.Power)} = a.{nameof(AccountingObject.Power)}" +
                                      $"\r\n,{nameof(Vehicle.SerialNumber)} = a.{nameof(AccountingObject.SerialNumber)}" +
                                      $"\r\n,{nameof(Vehicle.EngineSize)} = a.{nameof(AccountingObject.EngineSize)}" +
                                      $"\r\n,{nameof(Vehicle.VehicleModelID)} = a.{nameof(AccountingObject.VehicleModelID)}" +
                                      $"\r\n,{nameof(Vehicle.RegNumber)} = a.{nameof(AccountingObject.VehicleRegNumber)}" +
                                      $"\r\n,{nameof(Vehicle.RegDate)} = a.{nameof(AccountingObject.VehicleRegDate)}" +
                                      $"\r\nFROM {GetTableName(typeof(Vehicle))} target" +
                                      $"\r\nINNER JOIN {GetTableName(typeof(AccountingObject))} a" +
                                      $"\r\nON a.EstateID = target.ID" +
                                      $"\r\nINNER JOIN @modified m ON a.ID = m.ID"
            );

            mergeSqlScript.AppendLine($"UPDATE target" +
                                      $"\r\nSET {nameof(InventoryObject.SibCountryID)} = a.{nameof(AccountingObject.SibCountryID)}" +
                                      $"\r\n, {nameof(InventoryObject.SibFederalDistrictID)} = a.{nameof(AccountingObject.SibFederalDistrictID)}" +
                                      $"\r\n, {nameof(InventoryObject.SibRegionID)} = a.{nameof(AccountingObject.RegionID)}" +
                                      $"\r\n, {nameof(InventoryObject.SibCityNSIID)} = a.{nameof(AccountingObject.SibCityNSIID)}" +
                                      $"\r\n, {nameof(InventoryObject.Address)} = a.{nameof(AccountingObject.Address)}" +
                                      $"\r\nFROM {GetTableName(typeof(InventoryObject))} target" +
                                      $"\r\nINNER JOIN {GetTableName(typeof(AccountingObject))} a" +
                                      $"\r\nON a.EstateID = target.ID" +
                                      $"\r\nINNER JOIN @modified m ON a.ID = m.ID"
            );
            mergeSqlScript.AppendLine($"UPDATE e" +
                                      $"\r\nSET {nameof(Estate.InventoryNumber)} = a.{nameof(AccountingObject.InventoryNumber)}" +
                                      $"\r\nFROM {GetTableName(typeof(Estate))} e" +
                                      $"\r\nINNER JOIN {GetTableName(typeof(AccountingObject))} a" +
                                      $"\r\nON a.EstateID = e.ID" +
                                      $"\r\nINNER JOIN @modified m ON a.ID = m.ID"
                                      );
        }

        /// <summary>
        /// Set IsDispit=true if is MovableEstate && !String.IsNullOrEmpty(obj.RegNumber)
        /// </summary>
        /// <param name="mergeSqlScript"></param>
        private void SetIsDisput(StringBuilder mergeSqlScript)
        {
            mergeSqlScript.AppendLine($"print 'SetIsDisput'");
            mergeSqlScript.AppendLine($"UPDATE target " +
                                      $"\r\nSET IsDispute = 1 " +
                                      $"\r\nFROM {GetTableName(MainType)} target " +
                                      $"\r\nINNER JOIN @modified m ON target.ID = m.ID" +
                                      $"\r\nINNER JOIN {GetTableName(typeof(MovableEstate))}  as movable " +
                                      $"\r\nON target.EstateID = movable.ID" +
                                      $"\r\nWHERE ISNULL(target.RegNumber,'')<>'' ");
        }

        private string GetOldVersionValuesSpecification()
        {
            var val = new StringBuilder();
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "actualdate")
                {
                    return $"{GetStartPeriod()}";
                }
                if (x.ColumnName.ToLower() == "nonactualdate")
                {
                    return $"{GetEndPeriod()}";
                }
                if (x.ColumnName.ToLower() == "ishistory")
                {
                    return $"1";
                }
                if (x.ColumnName.ToLower() == "estateid")
                {
                   return $" (SELECT TOP 1 " +
                          $" ID FROM {GetTableName(typeof(Estate))} as estate " +
                          $" WHERE estate.Hidden = 0 AND estate.IsHistory = 0 AND estate.NUMBER = source.EUSINumber)";
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

        private string GetStartPeriod()
        {
            return $"'{_version.StartPeriod.ToString("yyyy-MM-dd")}'";
        }

        private string GetEndPeriod()
        {
            return $"'{_version.EndPeriod.ToString("yyyy-MM-dd")}'";
        }

        /// <summary>
        /// Возвращает строку с разделителем столбцов для вставки, включая обязательные колонки
        /// </summary>
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
                    return $"ISNULL({p}{x}, DATEADD(day , -1 , {GetStartPeriod()}))";
                }

                return string.IsNullOrEmpty(prefix) ? $"[{x}]" : $"{prefix}.[{x}]";
            }).ToArray());
        }

        private string GetSelectUpdateHistoryItemColumnSpecification(string prefix)
        {
            var insertColumns = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x.ColumnName);
            var p = string.IsNullOrEmpty(prefix) ? string.Empty : $"{prefix}.";

            return String.Join(",\r\n", insertColumns.Select(x =>
            {
                if (x == "IsHistory")
                {
                    return "0";
                }

                if (x == "NonActualDate")
                {
                    return $"{GetEndPeriod()}";
                }

                if (x == "ActualDate")
                {
                    return $"{GetStartPeriod()}";
                }

                return string.IsNullOrEmpty(prefix) ? $"[{x}]" : $"{prefix}.[{x}]";
            }).ToArray());
        }

        private string GetTargetValuesSpecification()
        {
            var val = new StringBuilder();
            var values = SqlColumnDefinitions.Where(x => x.ColumnName != "ID").Select(x => x).ToDictionary(x => x.ColumnName, x =>
            {
                if (x.ColumnName.ToLower() == "importdate" || x.ColumnName.ToLower() == "createdate" || x.ColumnName.ToLower() == "actualdate")
                {
                    return $"GetDate()";
                }
                return x.IsNullable ? $"target.[{x.ColumnName}]" : $"ISNULL(target.[{x.ColumnName}],{GetSqlDefaultValue(x)})";
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
                        values[colName] = $"target.[{colName}]";
                    }
                }
                else
                {
                    var idColName = $"{colName}ID";
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

        protected override string GetCreateTableScript()
        {
            var tableName = GetTempTableName();
            var properties = MainType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite).ToArray();
            var createScript = new StringBuilder();
            createScript.AppendLine($"IF OBJECT_ID(N'tempdb..{tableName}') IS NOT NULL DROP TABLE {tableName} \r\n CREATE TABLE {tableName} ({GetTempTableColumnsSpecification(properties)} )");
            return createScript.ToString();
        }

        /// <summary>
        /// Перечень выражений для UPDATE запроса (SET)
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
                    setSpecification.AppendLine($" target.[{colName}] = (SELECT TOP 1 " +
                                                $" ID FROM {GetTableName(typeof(Estate))} as estate " +
                                                $" WHERE estate.Hidden = 0 AND estate.IsHistory = 0 AND  estate.NUMBER = source.EUSINumber)");
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
            setSpecification.AppendLine($",target.ActualDate = {GetStartPeriod()}");
            setSpecification.AppendLine($",target.NonActualDate = {GetEndPeriod()}");
            return setSpecification.ToString();
        }

        public override int Merge(SqlCommand command, ref ImportHistory history)
        {
           
            command.CommandText = BuildMergeQuery(history);
            var reader = command.ExecuteReader();

            int relationCheckOk = 0;
            int totalRecords = 0;
            int id = 0;

            do
            {
                reader.Read();
                var resultName = reader.GetName(0);
                switch (resultName)
                {
                    case "RelationValidationResult":
                        relationCheckOk = reader.HasRows ? Convert.ToInt32(reader[0]) : 0;
                        break;

                    case "TotalRecords":
                        totalRecords = reader.HasRows ? Convert.ToInt32(reader[0]) : 0;
                        break;

                    case "ID":
                        id = reader.HasRows ? Convert.ToInt32(reader[0]) : 0;
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
                                    
                                    var error = new ImportErrorLog()
                                    {
                                        ImportHistory = history,
                                        RowNumber = rowNum,
                                        ErrorText = errorText,                                      
                                        ErrorType = ImportExtention.GetErrorTypeName(ErrorType.System),                                        
                                    };
                                    history.ImportErrorLogs.Add(error);
                                }

                            } while (reader.Read());
                            break;
                        }
                }
            } while (reader.NextResult());
            reader.Close();

            //Валидация
            ProcessValidationResult(ref history, totalRecords, relationCheckOk);
            return totalRecords;
        }
                   

        private void ProcessValidationResult(ref ImportHistory history, int processedRecords, int relationCheckOk)
        {
            var relationCheckFail = processedRecords - relationCheckOk;
            var relationCheckText = $"Проверка по составу связей ОБУ и ОП (по полю \"Номер записи гос регистрации\"): для {relationCheckOk} ОБУ связь с объектом права найдена, для {relationCheckFail} ОБУ связь с ОП не найдена. ";
            history.ResultText += relationCheckText;
        }
    }
}