using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
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

using CorpProp.Helpers.Import.Extentions;

namespace EUSI.Import.BulkMerge
{
    public class EusiAccountingObjectQueryBuilder : QueryBuilder
    {
        private OBUVersionControl _version;

        public EusiAccountingObjectQueryBuilder(Dictionary<string, string> colsNameMapping, Type type, OBUVersionControl version) : base(colsNameMapping, type)
        {
            _version = version;
        }

        public override string BuildMergeQuery(ImportHistory history)
        {
            var mergeSqlScript = new StringBuilder();

            mergeSqlScript.AppendLine(
                $"IF OBJECT_ID(N'tempdb..#AccountingObjectInsertedID') IS NOT NULL DROP TABLE #AccountingObjectInsertedID CREATE TABLE #AccountingObjectInsertedID(ID int)");
            mergeSqlScript.AppendLine(
                $"IF OBJECT_ID(N'tempdb..#AccountingObjectUpdatedID') IS NOT NULL DROP TABLE #AccountingObjectUpdatedID CREATE TABLE #AccountingObjectUpdatedID(ID int)");

            mergeSqlScript.AppendLine(
                $"declare @inserted table({GetColumnSpecification()})");
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

            var joinCondition = $"\r\ntarget.Hidden = 0 AND" +
                                "\r\ntarget.IsHistory = 0 AND" +
                                "\r\n((target.ConsolidationCode = source.Consolidation AND" +
                                "\r\ntarget.Number = source.EUSINumber AND" +
                                "\r\nISNULL(target.InventoryNumber, '') = COALESCE(source.InventoryNumber, ISNULL(target.InventoryNumber, '')))" +
                                "\r\nOR target.ConsolidationCode = source.Consolidation AND target.Number = source.EUSINumber)";

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
                $"\r\nLEFT JOIN dbo.AccountingObjectExtView AS target on {joinCondition}");
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
            mergeSqlScript.AppendLine($"SELECT * FROM ##tmp_accountingObject");

            #endregion Проверка значений в атрибутах: Номер ЕУСИ, БЕ, Инвентарный номер

            #region ОКТМО-Publishcode

            //ОКТМО -publishCode
            mergeSqlScript.AppendLine($"declare @oktmoMappingTable Table(rowid uniqueidentifier, rowNum int, oktmoid int)");

            mergeSqlScript.AppendLine($"UPDATE t SET OktmoID = x.ID" +
                                      $"\r\nFROM {GetTempTableName()} t" +
                                      $"\r\nINNER JOIN (SELECT d.ID, PublishCode " +
                                      $"\r\nFROM {GetTableName(typeof(OKTMO))} i " +
                                      $"\r\nINNER JOIN [CorpProp.Base].DictObject d  on  d.ID = i.ID)x " +
                                      $"\r\nON x.PublishCode = t.OKTMO");

            mergeSqlScript.AppendLine($"SELECT * FROM ##tmp_accountingObject");

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

            mergeSqlScript.AppendLine($"SELECT * FROM ##tmp_accountingObject");

            #endregion ОКТМО-Publishcode

            //ActualDate = @StartPeriod - update record
            mergeSqlScript.AppendLine($"PRINT 'MERGE ActualDate = @StartPeriod - update record'");
            mergeSqlScript.AppendLine($";With src as (SELECT * FROM {GetTempTableName()})");
            mergeSqlScript.AppendLine($"MERGE INTO dbo.AccountingObjectExtView AS target");
            mergeSqlScript.AppendLine($"USING src AS source ON {joinCondition}");
            mergeSqlScript.AppendLine($"AND target.ActualDate = @StartPeriod ");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()};");

            mergeSqlScript.AppendLine($"PRINT 'INSERT INTO @modified'");
            mergeSqlScript.AppendLine($"INSERT INTO @modified(ID) SELECT ID FROM #AccountingObjectUpdatedID");

            mergeSqlScript.AppendLine($"PRINT 'DELETE source if ActualDate = @StartPeriod'");
            mergeSqlScript.AppendLine($"DELETE source FROM {GetTempTableName()} as source " +
                                      $"\r\nINNER JOIN dbo.AccountingObjectExtView AS target ON" +
                                      $"\r\n {joinCondition}" +
                                      $"\r\n AND target.ActualDate = @StartPeriod");

            //ActualDate < @StartPeriod - CreateNewVersion (history)
            mergeSqlScript.AppendLine($"PRINT 'INSERT INTO {GetTableName(MainType)}'");
            mergeSqlScript.AppendLine($"INSERT INTO {GetTableName(MainType)}({GetInsertColumnSpecification()})");
            mergeSqlScript.AppendLine($"SELECT {GetSelectNewHistoryItemColumnSpecification("target")} " +
                                      $"\r\nFROM  dbo.AccountingObjectExtView target" +
                                      $"\r\nINNER JOIN {GetTempTableName()} source" +
                                      $"\r\nON {joinCondition}" +
                                      $"\r\nAND target.ActualDate < @StartPeriod ");

            //update current
            mergeSqlScript.AppendLine($"PRINT 'MERGE update current'");
            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY InventoryNumber) as cnt FROM {GetTempTableName()}) AS subquery WHERE cnt = 1)");
            mergeSqlScript.AppendLine($"MERGE INTO dbo.AccountingObjectExtView AS target");
            mergeSqlScript.AppendLine($"USING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"ON {joinCondition} " +
                                      $"\r\nAND target.ActualDate < @StartPeriod ");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()};");

            mergeSqlScript.AppendLine($"PRINT 'INSERT INTO @modified'");
            mergeSqlScript.AppendLine($"INSERT INTO @modified(ID) SELECT ID FROM #AccountingObjectUpdatedID");
            //mergeSqlScript.AppendLine($"TRUNCATE TABLE #AccountingObjectUpdatedID");

            mergeSqlScript.AppendLine($"PRINT 'DELETE source if ActualDate < @StartPeriod'");
            mergeSqlScript.AppendLine($"DELETE source FROM {GetTempTableName()} as source " +
                                      $"\r\nINNER JOIN dbo.AccountingObjectExtView AS target ON" +
                                      $"\r\n {joinCondition}" +
                                      $"\r\n AND target.ActualDate < @StartPeriod");

            //ActualDate > @StartPeriod
            //draft | outbus -> updateVersion
            mergeSqlScript.AppendLine($"PRINT 'MERGE draft | outbus -> updateVersion'");
            mergeSqlScript.AppendLine($";With NoDuplicates as (SELECT * FROM (SELECT *, Count(*) OVER (PARTITION BY InventoryNumber) as cnt FROM {GetTempTableName()}) AS subquery WHERE cnt = 1)");
            mergeSqlScript.AppendLine($"\r\nMERGE INTO dbo.AccountingObjectExtView AS target");
            mergeSqlScript.AppendLine($"\r\nUSING NoDuplicates AS source");
            mergeSqlScript.AppendLine($"\r\nON {joinCondition}" +
                                      $"\r\nAND target.ActualDate > @StartPeriod " +
                                      $"\r\nAND target.StateObjectRSBUID IS NOT null" +
                                      $"\r\nAND (target.StateObjectRSBUCode = 'draft' or target.StateObjectRSBUCode = 'outbus') ");
            mergeSqlScript.AppendLine($"\r\nWHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"\r\nUPDATE SET ");
            mergeSqlScript.AppendLine($"\r\n{GetSetSpecification()};");

            mergeSqlScript.AppendLine($"PRINT 'INSERT INTO @modified'");
            mergeSqlScript.AppendLine($"\r\nINSERT INTO @modified(ID) SELECT ID FROM #AccountingObjectUpdatedID");
            //mergeSqlScript.AppendLine($"\r\nTRUNCATE TABLE #AccountingObjectUpdatedID");

            //CreateOLDVersion
            mergeSqlScript.AppendLine($"PRINT 'MERGE CreateOLDVersion'");
            mergeSqlScript.AppendLine($";With " +
                                      $"\r\nOidValues AS (" +
                                      $"\r\nSELECT t.oid " +
                                      $"\r\nFROM  dbo.AccountingObjectExtView t " +
                                      $"\r\nINNER JOIN {GetTempTableName()} s " +
                                      $"\r\nON t.ConsolidationCode = s.Consolidation " +
                                      $"\r\nAND t.Number = s.EUSINumber " +
                                      $"\r\nAND ISNULL(t.InventoryNumber,'') = COALESCE(s.InventoryNumber, ISNULL(t.InventoryNumber, '')) " +
                                      $"\r\nAND t.ActualDate > @StartPeriod " +
                                      $"\r\nAND t.StateObjectRSBUCode != 'draft' " +
                                      $"\r\nAND t.StateObjectRSBUCode != 'outbus'), " +
            $"\r\nsrc as (SELECT * FROM {GetTempTableName()})");
            mergeSqlScript.AppendLine($"MERGE INTO dbo.AccountingObjectExtView AS target");
            mergeSqlScript.AppendLine($"USING src AS source");
            mergeSqlScript.AppendLine($"ON target.Oid in (SELECT Oid from OidValues) " +
                                      $"\r\nAND target.ConsolidationCode = source.Consolidation " +
                                      $"\r\nAND target.Number = source.EUSINumber " +
                                      $"\r\nAND ISNULL(target.InventoryNumber,'') = COALESCE(source.InventoryNumber, ISNULL(target.InventoryNumber, '')) " +
                                      $"\r\nAND target.ActualDate > @StartPeriod " +
                                      $"\r\nAND target.StateObjectRSBUCode != 'draft' " +
                                      $"\r\nAND target.StateObjectRSBUCode != 'outbus' ");
            mergeSqlScript.AppendLine($"WHEN MATCHED THEN");
            mergeSqlScript.AppendLine($"UPDATE SET ");
            mergeSqlScript.AppendLine($"{GetSetSpecification()}");
            mergeSqlScript.AppendLine($"WHEN NOT MATCHED BY target THEN ");
            mergeSqlScript.AppendLine($"INSERT({GetInsertColumnSpecification()})");
            mergeSqlScript.AppendLine($"VALUES({GetOldVersionValuesSpecification()});");
            //            mergeSqlScript.AppendLine($"TRUNCATE TABLE #AccountingObjectUpdatedID");

            SetIsDisput(mergeSqlScript);

            UpdateEstate(mergeSqlScript);

            SetIsEnergy(mergeSqlScript);

            LinkHistoryObject(mergeSqlScript, history);

            mergeSqlScript.AppendLine($";IF OBJECT_ID(N'tempdb..#ModifiedRecords') IS NOT NULL " +
                                        $"\r\nDROP TABLE #ModifiedRecords CREATE TABLE #ModifiedRecords(ID int)");

            mergeSqlScript.AppendLine($"INSERT INTO #ModifiedRecords(ID) " +
                                        "\r\nSELECT ID FROM " +
                                        "\r\n(Select ID FROM #AccountingObjectInsertedID " +
                                        "\r\nunion all " +
                                        "\r\nSelect ID FROM #AccountingObjectUpdatedID)t");

            mergeSqlScript.AppendLine($"SELECT count(*) as 'RelationValidationResult'" +
                                      $"\r\nFROM[CorpProp.Accounting].AccountingObject a " +
                                      $"\r\nINNER JOIN #ModifiedRecords t" +
                                      $"\r\nON a.ID = t.ID " +
                                      $"\r\nINNER JOIN [CorpProp.Law].[Right] r " +
                                      $"\r\nON r.EstateID = a.EstateID " +
                                      $"\r\nAND r.RegNumber = a.RegNumber ");

            mergeSqlScript.AppendLine($"SELECT count(*) as 'TotalRecords' FROM #ModifiedRecords");

            mergeSqlScript.AppendLine($"SELECT ID as Modified FROM #ModifiedRecords");

            mergeSqlScript.AppendLine($"SELECT errorRowNum, description from @errorTable");

            mergeSqlScript.AppendLine($"DROP table #AccountingObjectInsertedID");
            mergeSqlScript.AppendLine($"DROP table #AccountingObjectUpdatedID");
            mergeSqlScript.AppendLine($"DROP table #ModifiedRecords");

            return mergeSqlScript.ToString();
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
                                      $"\r\nFROM {GetTableName(typeof(AccountingObjectTbl))} target " +
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
            createScript.AppendLine($"IF OBJECT_ID(N'tempdb..{tableName}') IS NOT NULL DROP TABLE {tableName} \r\n CREATE TABLE {tableName} ({GetTempTableColumnsSpecification(properties)} ,EUSINumber int )");
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
                                                $" WHERE estate.NUMBER = source.EUSINumber)");
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
            CreateView(command);
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
                            if (reader.HasRows)
                            {
                                var rowNum = Convert.ToInt32(reader[0]);
                                var errorText = Convert.ToString(reader[1]);
                                history.ImportErrorLogs.AddError(rowNum, 0, "", errorText, ErrorType.System);
                            }
                            break;
                        }
                }
            } while (reader.NextResult());
            reader.Close();

            DropView(command);
            //Валидация
            ProcessValidationResult(ref history, totalRecords, relationCheckOk);
            return totalRecords;
        }

        private void CreateView(SqlCommand command)
        {
            var sb = new StringBuilder();
            DropView(command);

            sb.AppendLine($"CREATE VIEW dbo.AccountingObjectExtView AS(" +
                          $"\r\nSELECT a.*,e.Number, Consolidation.Code AS ConsolidationCode, StateObjectRSBU.Code as StateObjectRSBUCode " +
                          $"\r\nFROM[CorpProp.Accounting].AccountingObject a " +
                          $"\r\nLEFT JOIN[CorpProp.Estate].Estate e on a.EstateID = e.ID " +
                          $"\r\nLEFT JOIN (SELECT d.ID,Code FROM [CorpProp.NSI].Consolidation c INNER JOIN[CorpProp.Base].DictObject d  on  d.ID = c.ID) Consolidation on a.ConsolidationID = Consolidation.ID " +
                          $"\r\nLEFT JOIN (SELECT d.ID,Code FROM [CorpProp.NSI].StateObjectRSBU s INNER JOIN[CorpProp.Base].DictObject d  on  d.ID = s.ID) StateObjectRSBU on a.StateObjectRSBUID = StateObjectRSBU.ID)");

            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        private void DropView(SqlCommand command)
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                $"IF OBJECT_ID('dbo.AccountingObjectExtView','V') IS NOT NULL DROP VIEW dbo.AccountingObjectExtView");
            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        private void ProcessValidationResult(ref ImportHistory history, int processedRecords, int relationCheckOk)
        {
            var relationCheckFail = processedRecords - relationCheckOk;
            var relationCheckText = $"Проверка по составу связей ОБУ и ОП (по полю \"Номер записи гос регистрации\"): для {relationCheckOk} ОБУ связь с объектом права найдена, для {relationCheckFail} ОБУ связь с ОП не найдена. ";
            history.ResultText += relationCheckText;
        }
    }
}