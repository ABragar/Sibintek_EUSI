using Base.DAL;
using Base.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using Base.Extensions;
using Base.Utils.Common;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.Export;
using CorpProp.Entities.Subject;
using CorpProp.Helpers.Export;
using EUSI.Entities.ManyToMany;
using OfficeOpenXml;
using Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.NSI;
using EUSI.Entities.Estate;
using EUSI.Entities.Accounting;

namespace EUSI.Export
{
    /// <summary>
    /// Представляет класс для экспорта ОС в Excel шаблоны.
    /// </summary>
    public class AccObjectExport
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileSystemService _fileSystemService;
        private readonly bool _isAccountigObject;
        private readonly Guid _tempDirName;

        /// <summary>
        /// Инициализирует экземпляр класса AccObjectExport.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="fileSystemService">Служба ФС.</param>
        /// <param name="isAccountigObject">Признак определяющий, будут ли передаваемые ID являться ID ОСов.</param>
        public AccObjectExport(IUnitOfWork unitOfWork, IFileSystemService fileSystemService,
            bool isAccountigObject = false)
        {
            _unitOfWork = unitOfWork;
            _fileSystemService = fileSystemService;
            _isAccountigObject = isAccountigObject;
            _tempDirName = Guid.NewGuid();
        }

        /// <summary>
        /// Запуск экспорта.
        /// </summary>
        /// <param name="elementsIds">ID заявок.</param>
        /// <returns>Base64-строка архива.</returns>
        /// <param name="emptyeusinumbers">к-во записей без значения в поле Номер ЕУСИ </param>

        public string Export(int[] elementsIds, ref int emptyeusinumbers)
        {
            List<string> filesList = new List<string>();

            try
            {
                List<IGrouping<Consolidation, AccountingObject>> groups = GetAccountingObjects(elementsIds);

                if (groups.Count == 0)
                    return null;


                var stateID = _unitOfWork.GetRepository<CorpProp.Entities.NSI.StateObjectRSBU>()
                               .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Code == "OUTBUS")
                               .FirstOrDefault()?.ID;

                var states = _unitOfWork.GetRepository<CorpProp.Entities.NSI.StateObjectRSBU>()
                               .FilterAsNoTracking(f => !f.Hidden && !f.IsHistory && f.Code == "DRAFT")
                               .Select(s => s.ID);

                if (!_isAccountigObject)
                {
                    var ers = _unitOfWork.GetRepository<EstateRegistration>()
                    .Filter(f => elementsIds.Contains(f.ID))
                    .DefaultIfEmpty()
                    .ToList();

                    ers.ForEach(f => {
                        if (!f.TransferBUS)
                            f.TransferBUS = true;
                        if (f.TransferBUSDate == null)
                            f.TransferBUSDate = DateTime.Now;
                    });
                }

                foreach (IGrouping<Consolidation, AccountingObject> group in groups)
                {
                    //TODO: Получение кода (БУС) по ОГ.
                    ExportTemplate exportTemplate =
                        ExcelExportHelper.GetExportTemplate(_unitOfWork, nameof(AccountingObject), "BUH_4");

                    if (exportTemplate == null)
                        throw new Exception($"Не найден шаблон для выгрузки для БЕ {group.Key.Code + " " + group.Key.Name}.");

                    if (!_isAccountigObject)
                    {
                        var oss = group.ToList<AccountingObject>();

                        emptyeusinumbers += GetEmptyEusiNumbers(oss.Select(x => x.EstateID).ToList());
                        string filePath = GenerateNewFilePath(exportTemplate, group.Key.Code, null);
                        ExportOS(oss, exportTemplate, filePath, stateID, states);
                        filesList.Add(filePath);
                    }
                    else
                    {
                        var list = group.ToList<AccountingObject>();
                        emptyeusinumbers += GetEmptyEusiNumbers(list.Select(x => x.EstateID).ToList()); ;

                        string filePath = GenerateNewFilePath(exportTemplate, group.Key.Code);
                        ExportOS(list, exportTemplate, filePath, stateID, states);
                        filesList.Add(filePath);
                    }

                }
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToStringWithInner());
            }

            return ExcelExportHelper.PrepareArchive(filesList);
        }
        
        private int GetEmptyEusiNumbers(List<int?> estateIDs)
        {
            int result = estateIDs.Count(x => !x.HasValue);
            var notNullIDs = estateIDs.Where(x => x.HasValue).ToList();
            result += _unitOfWork.GetRepository<Estate>().All().Where(x => notNullIDs.Contains(x.ID))
                .Count(x => !x.Number.HasValue && !x.PCNumber.HasValue);
            return result;
        }

        private void ExportOS(
             List<AccountingObject> list
            , ExportTemplate exportTemplate
            , string filePath
            , int? stateID
            , IQueryable<int> states)
        {

            Dictionary<int, string> colsMap = exportTemplate.GetColumnsMap();

            using (ExcelPackage pack = new ExcelPackage(new FileInfo(filePath)))
            {
                DataTable dataTable =
                    ExcelExportHelper.ToDataTable(_unitOfWork, list, colsMap);
                ExcelWorksheet worksheet = pack.Workbook.Worksheets[1];

                if (exportTemplate.StartRow == null || exportTemplate.StartColumn == null)
                    throw new Exception("Не указана стартовая строка или колонка.");

                worksheet.Cells[exportTemplate.StartRow.Value, exportTemplate.StartColumn.Value]
                    .LoadFromDataTable(dataTable, false);

                pack.Save();

                ChangeStatus(stateID, states, list);
            }
        }

        public string GetContentForExportOs(ExportZip model)
        {
            var filesList = new List<string>();
            try
            {
                var consolidationCodes = model.Consolidations.Select(c => c.Object.Code).Distinct().ToList();
                var stateObjectRsbuCodes = new List<string> { "DRAFT", "OUTBUS" };

                if (!_unitOfWork
                    .GetRepository<AccountingObject>()
                    .FilterAsNoTracking(ao => !ao.Hidden && !ao.IsHistory
                            && (!ao.IsArchived.HasValue || (ao.IsArchived.HasValue && !ao.IsArchived.Value))
                            && ao.Consolidation != null && consolidationCodes.Contains(ao.Consolidation.Code)
                            && (ao.StateObjectRSBU == null || (ao.StateObjectRSBU != null && !stateObjectRsbuCodes.Contains(ao.StateObjectRSBU.Code.ToUpper())))
                          )
                    .Any())
                {
                    return null;
                }

                var exportTemplate = ExcelExportHelper.GetExportTemplate(_unitOfWork, nameof(AccountingObject), "ExportOS");

                if (exportTemplate == null)
                {
                    throw new Exception("Не найден шаблон для экспорта данных об ОС/НМА");
                }

                foreach (string code in consolidationCodes)
                {
                    var oss =
                    _unitOfWork
                    .GetRepository<AccountingObject>()
                    .FilterAsNoTracking(ao => !ao.Hidden && !ao.IsHistory
                            && (!ao.IsArchived.HasValue || (ao.IsArchived.HasValue && !ao.IsArchived.Value))
                            && ao.Consolidation != null && ao.Consolidation.Code == code
                            && (ao.StateObjectRSBU == null || (ao.StateObjectRSBU != null && !stateObjectRsbuCodes.Contains(ao.StateObjectRSBU.Code.ToUpper())))
                          )
                    .ToList();
                    string filePath = GenerateNewFilePath(exportTemplate, code);
                    ExportOS(oss, exportTemplate, filePath, null, null);
                    filesList.Add(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToStringWithInner());
            }

            return ExcelExportHelper.PrepareArchive(filesList);
        }

        private void ChangeStatus(int? newStateID, IEnumerable<int> states, IEnumerable<AccountingObject> list)
        {
            if (newStateID == null) return;
            var arr = list.Where(f => f.StateObjectRSBUID != null && states.Contains(f.StateObjectRSBUID.Value));
            foreach (AccountingObject item in arr)
            {
                item.StateObjectRSBUID = newStateID;
                if (item.TransferBUSDate == null)
                    item.TransferBUSDate = DateTime.Now;
            }
        }


        /// <summary>
        /// Собирает ОСы из заявок.
        /// </summary>
        /// <param name="elementsIds">ID элементов.</param>
        /// <returns>ОСы.</returns>
        private List<IGrouping<Consolidation, AccountingObject>> GetAccountingObjects(int[] elementsIds)
        {
            if (!_isAccountigObject) //это заявки
            {
                ///TODO: Необходимо выполнить анализ требования и реализации. Предположительно некорректно опираться на признак "Прототип". При наличии двух связанных ОС (после ВГП аренда и ВГП реализация), при выгрузке в шаблон Excel берется не то ОС.
                int[] links = _unitOfWork.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                    .Filter(f =>
                        elementsIds.Contains(f.ObjRigthId)
                        && !f.Hidden
                        && f.IsPrototype)
                    .Select(s => s.ObjLeftId).ToArray();

                return
                    _unitOfWork.GetRepository<AccountingObject>()
                        .Filter(f =>
                            links.Contains(f.ID) &&
                            f.OwnerID != null &&
                            (!f.IsArchived.HasValue || !f.IsArchived.Value) &&
                            !f.Hidden &&
                            !f.IsHistory
                        )
                        .Include(i => i.Owner)
                        //.Include(i => i.EUSINumber)
                        .Include(i => i.Consolidation)
                        .Include(i => i.Contragent)
                        .Include(i => i.StateObjectRSBU)
                        .Include(i => i.SibCountry)
                        .Include(i => i.SibFederalDistrict)
                        .Include(i => i.Region)
                        .GroupBy(g => g.Consolidation)
                        .ToList();
            }
            else
            {
                return
                    _unitOfWork.GetRepository<AccountingObject>()
                        .Filter(f =>
                            elementsIds.Contains(f.ID) &&
                            f.OwnerID != null &&
                            (!f.IsArchived.HasValue || !f.IsArchived.Value) &&
                            !f.Hidden &&
                            !f.IsHistory
                        )
                        .Include(i => i.Consolidation)
                        .GroupBy(g => g.Consolidation)
                        .ToList();
            }
        }

        /// <summary>
        /// Генерация пути временного файла.
        /// </summary>
        /// <param name="exportTemplate">Шаблон экспорта.</param>
        /// <param name="ideup">ИДЕУП ОГ.</param>
        /// <returns></returns>
        private string GenerateNewFilePath(ExportTemplate exportTemplate, string be, string er = null)
        {
            string tempDir = $@"{_fileSystemService.FilesDirectory}\Temp\{_tempDirName}";

            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            string filePath = $@"{tempDir}\{GenerateNewFileName(be, exportTemplate.FileName, er)}";

            if (File.Exists(filePath))
                File.Delete(filePath);

            FileDB file = exportTemplate.File;

            if (file == null && exportTemplate.FileID != null)
                file = _unitOfWork.GetRepository<FileDB>().Find(f => f.ID == exportTemplate.FileID);

            if (file == null)
                throw new Exception("К шаблону не прикреплен файл.");

            File.WriteAllBytes(filePath, file.Content);

            return filePath;
        }

        /// <summary>
        /// Генерация имени файла.
        /// </summary>
        /// <param name="ideup">ИДЕУП ОГ.</param>
        /// <param name="templateFileName">Имя файла шаблона.</param>
        /// <returns></returns>
        private static string GenerateNewFileName(string be, string templateFileName, string er = null)
        {
            return $"{be}_{((String.IsNullOrEmpty(er)) ? "" : (er + "_"))}{DateTime.Now.Date:ddMMyyyy}_{templateFileName}";
        }


        public static void GetObjects(IServiceLocator locator)
        {
            var confService = locator.GetService<Base.UI.IViewModelConfigService>();
            string filePath = @"E:\zzz\123.xlsx";
            var types = AppDomain.CurrentDomain.GetAssemblies()
                   .SelectMany(s => s.GetTypes())
                   .Where(n =>
                      !n.IsAbstract
                   && !n.IsInterface
                   && !n.IsPrimitive
                   //&& !n.IsGenericType
                   && n.IsSubclassOf(typeof(Base.BaseObject)))
                   .ToList();

            using (ExcelPackage pack = new ExcelPackage(new FileInfo(filePath)))
            {

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Assembly", typeof(string));
                dataTable.Columns.Add("Type", typeof(string));
                dataTable.Columns.Add("FullName", typeof(string));
                dataTable.Columns.Add("Mnemonic", typeof(string));
                dataTable.Columns.Add("Title", typeof(string));

                foreach (var type in types)
                {
                    Base.UI.ViewModal.ViewModelConfig conf = null;
                    try
                    {
                        conf = confService.Get(type);

                    }
                    catch (Exception ex)
                    {

                    }
                    dataTable.Rows.Add(new object[] {
                        type.Assembly.FullName
                        , type.FullName
                        , type.GetTypeName()
                        , conf?.Mnemonic
                        , conf?.Title });
                }
                //pack.Workbook.Worksheets.Add("list1");
                ExcelWorksheet worksheet = pack.Workbook.Worksheets[1];
                worksheet.Cells[1, 1]
                    .LoadFromDataTable(dataTable, false);
                pack.Save();
            }
        }
    }
}
