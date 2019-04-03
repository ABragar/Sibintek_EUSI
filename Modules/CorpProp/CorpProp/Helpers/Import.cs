using Base.DAL;
using Base.Service;
using Base.Service.Log;
using Base.UI.Service;
using Base.Utils.Common;
using CorpProp.Common;
using CorpProp.Entities.Asset;
using CorpProp.Entities.Common;
using CorpProp.Entities.Import;
using CorpProp.Entities.Law;
using CorpProp.Extentions;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CorpProp.Helpers
{
    public class ImportStarter : IImportStarter
    {
        private readonly IAccessService _accessService;
        private readonly IExcelImportChecker _checker;
        private readonly ILogService _logger;

        public ImportStarter(IAccessService accessService, IExcelImportChecker checker, ILogService logger)
        {
            _accessService = accessService;
            _checker = checker;
            _logger = logger;
        }

        public void Import(
              IUiFasade uiFacade
            , IUnitOfWork unitOfWork
            , IUnitOfWork histUnitOfWork
            , IExcelDataReader reader
            , ref string error
            , ref int count
            , ref ImportHistory history
            , string fileName = ""
            )
        {
            try
            {
                var tables = reader.GetVisbleTables();
                DataTable entryTable = tables[0];
                DeleteEmptyRows(ref entryTable);

                FillMainDataHistory(entryTable, ref history);

                if (String.IsNullOrWhiteSpace(history.Mnemonic))
                    throw new Exception("Не удалось определить тип. Проверьте формат файла.");



                Type entityType = TypesHelper.GetTypeByName(history.Mnemonic);
                if (entityType == null)
                    throw new Exception("Не удалось определить тип. Проверьте формат файла.");

                Dictionary<string, string> colsMapping = ImportHelper.ColumnsNameMapping(entryTable);

                var config = uiFacade.GetViewModelConfig(history.Mnemonic);

                if (config == null)
                {
                    throw new Exception("Не удалось определить описание представления объекта импорта. Проверьте системное имя объекта в файле импорта.");
                }

                //TODO: перенести в настройку шаблона имопрта
                //Идентификация справочников по коду
                bool identyDictByCode = false;
                if (config.ServiceType.GetInterfaces().Contains(typeof(ISystemImportEntity)))
                {
                    var service = config.GetService<ISystemImportEntity>();
                    identyDictByCode = service.IdentyDictByCode();
                }

                IExcelImportChecker checker = config.ServiceType.GetInterfaces().Contains(typeof(IExcelImportChecker)) ?
                    config.GetService<IExcelImportChecker>() :
                    _checker;

                checker.StartDataCheck(unitOfWork, histUnitOfWork, entryTable, entityType, ref history, identyDictByCode);

                if (config.ServiceType.GetInterfaces().Contains(typeof(IAdditionalExcelImportChecker)))
                {
                    config.GetService<IAdditionalExcelImportChecker>()?
                        .AdditionalChecks(unitOfWork, histUnitOfWork, entryTable, entityType, ref history, identyDictByCode);
                }


                if (history.ImportErrorLogs.Count > 0)
                {
                    throw new ImportException("Ошибки при проверке.");
                }

                if (config.ServiceType.GetInterfaces().Contains(typeof(IExcelImportEntity)))
                {
                    var service = config.GetService<IExcelImportEntity>();
                    service.Import(unitOfWork, histUnitOfWork, entryTable, colsMapping, ref count, ref history);
                }
                else if (entityType.IsSubclassOf(typeof(Entities.Base.DictObject)))
                {
                    ImportLoader.ImportDictObject(unitOfWork, _accessService, entryTable, colsMapping, entityType, ref count, ref history, _logger);
                }
                else if (entityType.Equals(typeof(ScheduleStateRegistration)))
                {
                    var service = config.GetService<Services.Law.ScheduleStateRegistrationService>();
                    service.Import(unitOfWork, histUnitOfWork, reader, entityType, ref count, ref history);
                }
                else if (entityType.Equals(typeof(ScheduleStateTerminate)))
                {
                    var service = config.GetService<Services.Law.ScheduleStateTerminateService>();
                    service.Import(unitOfWork, histUnitOfWork, reader, entityType, ref count, ref history);
                }
                else if (entityType.Equals(typeof(NonCoreAssetList)))
                {
                    var service = config.GetService<Services.Asset.NonCoreAssetListService>();
                    service.Import(unitOfWork, histUnitOfWork, reader, entityType, ref count, ref history);
                }
                else if (entityType.Equals(typeof(Entities.CorporateGovernance.Appraisal)))
                {
                    var service = config.GetService<Services.CorporateGovernance.AppraisalService>();
                    service.Import(unitOfWork, histUnitOfWork, reader, entityType, ref count, ref history);
                }
            }
            catch (ImportException ex)
            {
                _logger.Log(ex);
                //TODO: Обработать исключение.
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
                history.ImportErrorLogs.Add(new ImportErrorLog()
                {
                    MessageDate = DateTime.Now,
                    ErrorText = ex.ToStringWithInner()
                });
            }
        }

        /// <summary>
        /// Наполняет историю импорта основными данными из файла Excel.
        /// </summary>
        /// <param name="history"></param>
        public virtual void FillMainDataHistory(DataTable table, ref ImportHistory history)
        {
            history.Mnemonic = ImportHelper.FindSystemName(table);
            history.CurrentFileUser = ImportHelper.FindCurrentFileUser(table);
            history.ActualityDate = ImportHelper.GetActualityDate(table);
        }

        public virtual CheckImportResult CheckImport(
           IUiFasade uiFacade,
           IExcelDataReader reader,
           ITransactionUnitOfWork uofw,
           IUnitOfWork histUnitOfWork,
           StreamReader stream,
           string fileName,
           ImportHistory importHistory
           )
        {
            var checkResult = new CheckImportResult();
            var tables = reader.GetVisbleTables();
            DataTable entryTable = tables[0];
            string mnemonic = ImportHelper.FindSystemName(entryTable);
            string errText = "";
            
            if (ImportHelper.CheckRepeatImport(uofw, stream.BaseStream))
            {
                checkResult.IsError = true;
                checkResult.ErrorMessage = "Идентичный файл импортировался ранее.";
                return checkResult;
            }
            

            if (mnemonic?.ToLower() != "estateregistration" && mnemonic?.ToLower() != "bcsdata" && ImportHelper.CheckDataVersion(uofw, fileName, reader))
            {
                checkResult.IsError = true;
                checkResult.ErrorMessage = "Файл с такой же или меньшей версией данных импортировался ранее.";
                return checkResult;
            }

            //Есть в системе более актуальные данные или шаблон импорта некорректный
            if (mnemonic?.ToLower() == "bcsdata" && ImportHelper.CheckActualityDate(uofw, fileName, reader, ref errText))
            {
                checkResult.IsError = true;
                checkResult.ErrorMessage = (!string.IsNullOrWhiteSpace(errText)) ? errText : "Файл с более актуальными данными импортировался ранее.";
                return checkResult;
            }

            return checkResult;
        }

        /// <summary>
        /// Удаление пустых строк.
        /// </summary>
        /// <param name="dt"></param>
        public static void DeleteEmptyRows(ref DataTable dt)
        {
            if (dt == null) return;
            //TODO: удаление пустых строк
            int start = ImportHelper.GetRowStartIndex(dt);

            var emtyRows = dt.Rows.Cast<DataRow>()
                .Where(f => !f.ItemArray
                    .Where(w => (w != System.DBNull.Value
                        && w != null
                        && !String.IsNullOrEmpty(w.ToString())
                        && !String.IsNullOrEmpty(w.ToString().Trim())
                        )).Any()
                );

            Parallel.ForEach(emtyRows,
            rr =>
            {
                rr.Delete();
            });

            dt.AcceptChanges();
        }
    }
}