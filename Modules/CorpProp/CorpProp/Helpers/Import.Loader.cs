using Base;
using Base.DAL;
using Base.Security;
using Base.Service;
using Base.UI.Service;
using Base.Utils.Common;
using CorpProp.Common;
using CorpProp.Entities.Base;
using CorpProp.Entities.Import;
using CorpProp.Entities.Security;
using CorpProp.Entities.Subject;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Base.Enums;
using CorpProp.Helpers.Import.Extentions;
using CorpProp.Services.Import.BulkMerge;
using Base.Service.Log;

namespace CorpProp.Helpers
{
    public static class ImportLoader
    {

        private static string[] dictForOldAlg = new[]
              {
                    "",
                    //"SibFederalDistrict",
                    //"SibRegion",
                    //"SibCityNSI",
                };

        public static void ImportDictObject(IUnitOfWork uofw, IAccessService accessService, DataTable table, Dictionary<string, string> colsNameMapping, Type type, ref int count, ref ImportHistory history, ILogService logger)
        {
            try
            {
                if (dictForOldAlg.All(d => d != type.Name))
                {
                    accessService.ThrowIfAccessDenied(uofw, type, TypePermission.Create | TypePermission.Write);

                    var bulkMergeManager = new BulkMergeManager(colsNameMapping, type, ref history);

                    bulkMergeManager.BulkInsertAll(table, type, colsNameMapping, ref count);
                    return;
                }

                MethodInfo methodUow = typeof(ImportHelper).GetMethod("SimpleInstance");
                MethodInfo genericUow = methodUow.MakeGenericMethod(type);

                for (int r = 9; r < table.Rows.Count; r++)
                {
                    var row = table.Rows[r];
                    var impl = genericUow.Invoke(null, new object[] { uofw, row, colsNameMapping, history });
                    count++;
                }

                methodUow = typeof(ImportHelper).GetMethod("UpdateOlderDictObject");

                if (methodUow == null)
                    throw new Exception("MethodInfo is null");

                genericUow = methodUow.MakeGenericMethod(type);
                genericUow.Invoke(null, new object[] { uofw, type });
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                history.ImportErrorLogs.AddError(ex);
            }
        }

        public static void ImportSociety(IUnitOfWork uofw, IAccessService accessService, DataTable table, Dictionary<string, string> colsNameMapping, Type type, ref int count, ref ImportHistory history, ILogService logger)
        {
            try
            {
                accessService.ThrowIfAccessDenied(uofw, type, TypePermission.Create | TypePermission.Write);

                var bulkMergeManager = new BulkMergeManager(colsNameMapping, type, ref history);

                bulkMergeManager.BulkInsertAll(table, type, colsNameMapping, ref count);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                history.ImportErrorLogs.AddError(ex);
            }
        }

        public static void Import(IUnitOfWork uofw, IAccessService accessService, DataTable table, Dictionary<string, string> colsNameMapping, Type type, ref int count, ref ImportHistory history, ILogService logger)
        {
            try
            {
                accessService.ThrowIfAccessDenied(uofw, type, TypePermission.Create | TypePermission.Write);

                var bulkMergeManager = new BulkMergeManager(colsNameMapping, type, ref history);

                bulkMergeManager.BulkInsertAll(table, type, colsNameMapping, ref count);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                throw new Exception($"В процессе импорта данных были выявлены критические ошибки в файле шаблона импорта. Дальнейший импорт невозможен. {System.Environment.NewLine}Системный текст ошибки: {ex.ToStringWithInner()}");
            }
        }

        private static Society GetSocietyByIDEUP(IUnitOfWork uofw, string ideup, ref string error, ref int count, ref ImportHistory history)
        {
            try
            {
                var elements = uofw.GetRepository<Society>().Filter(f => f.IDEUP == ideup).ToList();

                if (elements.Count == 0)
                    throw new Exception();
                else if (elements.Count > 1)
                    throw new Exception();

                return elements[0];
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static void ImportAnyObject(
            IUnitOfWork uofw
            , DataTable table
            , Dictionary<string, string> colsNameMapping
            , Type type
            , ref int count
            , ref ImportHistory history
            , bool dictCode = false)
        {
            try
            {
                MethodInfo methodUow = typeof(ImportHelper).GetMethod("SimpleInstance");
                MethodInfo genericUow = methodUow.MakeGenericMethod(type);

                for (int r = 9; r < table.Rows.Count; r++)
                {
                    var row = table.Rows[r];
                    var impl = genericUow.Invoke(null, new object[] { uofw, row, colsNameMapping, history, dictCode });
                    count++;
                }
            }
            catch (Exception ex)
            {
                history.ImportErrorLogs.AddError(ex);
            }
        }
    }
}
