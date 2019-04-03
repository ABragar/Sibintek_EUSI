using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using Base;
using Base.DAL;
using Base.Extensions;
using Base.Service;
using CorpProp.Entities.Accounting;
using CorpProp.Entities.Base;
using CorpProp.Entities.Document;
using CorpProp.Entities.Settings;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Export;
using EUSI.Entities.Accounting;
using EUSI.Entities.ManyToMany;
using ExcelDataReader;
using Ionic.Zip;
using OfficeOpenXml;

namespace EUSI.Helpers
{
    /// <summary>
    /// Экспорт ОС в Excel.
    /// </summary>
    public class EUSIExportHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileSystemService _fileSystemService;
        private readonly bool _isAccountigObject;

        /// <summary>
        /// Инициализация.
        /// </summary>
        /// <param name="unitOfWork">Сессия.</param>
        /// <param name="fileSystemService">Сервис ФС.</param>
        /// <param name="isAccountigObject">Признак определяющий, будут ли передаваемые ID являться ID ОСов.</param>
        public EUSIExportHelper(IUnitOfWork unitOfWork, IFileSystemService fileSystemService, bool isAccountigObject = false)
        {
            _unitOfWork = unitOfWork;
            _fileSystemService = fileSystemService;
            _isAccountigObject = isAccountigObject;
        }

        /// <summary>
        /// Запуск экспорта.
        /// </summary>
        /// <param name="elementsIds">ID элементов.</param>
        /// <returns>Base64-строка архива.</returns>
        public string Export(int[] elementsIds)
        {
            if (elementsIds.Length == 0)
                return null;

            List<AccountingObject> baseData = ReadData(elementsIds);

            if (baseData.Count == 0)
                throw new Exception("ОС не найдены.");

            int[] societyIds = baseData.Where(w => w.OwnerID != null).Select(s => (int)s.OwnerID).ToArray();

            if (societyIds.Length == 0)
                throw new Exception("ОГ не найдены.");

            Dictionary<int, Dictionary<string, string>> filesPathList = GetTemplatesBySociety(societyIds, typeof(AccountingObject));
            if (filesPathList.Count == 0)
                throw new Exception("Шаблоны не найдены.");

            return ReadTemplate(filesPathList, baseData);
        }

        /// <summary>
        /// Чтение и заполнение шаблона.
        /// </summary>
        /// <param name="filesPathList">Коллекция (ID ОГ, (Файл для заполнения, файл шаблона)).</param>
        /// <param name="baseData">ОСы.</param>
        /// <returns>Base64 архива.</returns>
        private static string ReadTemplate(Dictionary<int, Dictionary<string, string>> filesPathList, List<AccountingObject> baseData)
        {
            List<string> result = new List<string>();
            foreach (var filePath in filesPathList)
            {
                foreach (var q in filePath.Value)
                {
                    result.Add(q.Key);
                    using (var pack = new ExcelPackage(new FileInfo(q.Key)))
                    {
                        using (var stream = File.OpenRead(q.Value))
                        {
                            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            Type _type = typeof(AccountingObject);
                            DataSet ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = x => new ExcelDataTableConfiguration() {UseHeaderRow = false}
                            });
                            var tables = ds.Tables;
                            DataTable dt = tables[0];

                            var colsMap = ImportHelper.ColumnsNameMapping(dt);


                            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(_type);
                            var aoList = baseData.Where(w => w.OwnerID == filePath.Key);
                            foreach (var ao in aoList)
                            {
                                var row = dt.NewRow();
                                foreach (var col in colsMap)
                                {
                                    var prop = props[col.Value];

                                    string value = prop?.GetValue(ao) != null ? 
                                        CorpProp.Helpers.Export.ExcelExportHelper.GetValue(prop.GetValue(ao)?.GetType(),
                                        prop.GetValue(ao)) : "";
                                    row[col.Key] = value;
                                }

                                dt.Rows.Add(row);
                            }

                            ExcelWorksheet worksheet = pack.Workbook.Worksheets.Add("AO");
                            ApplyStyles(worksheet);
                            worksheet.Cells["A1"].LoadFromDataTable(dt, false);
                            pack.Save();
                        }
                    }
                }
            }

            return ExcelExportHelper.PrepareArchive(result);
        }

        /// <summary>
        /// Получение значений свойства.
        /// </summary>
        /// <param name="_type">Тип.</param>
        /// <param name="value">Полученное значение.</param>
        /// <returns>Значение в виде строки.</returns>
       

        /// <summary>
        /// Собирает ОСы из заявок.
        /// </summary>
        /// <param name="elementsIds">ID элементов.</param>
        /// <returns>ОСы.</returns>
        private List<AccountingObject> ReadData(int[] elementsIds)
        {
            if (!_isAccountigObject)
            {
                int[] links = _unitOfWork.GetRepository<AccountingObjectAndEstateRegistrationObject>()
                    .Filter(f => elementsIds.Contains(f.ObjRigthId)).Select(s => s.ObjLeftId).ToArray();

                return _unitOfWork.GetRepository<AccountingObject>().Filter(w => links.Contains(w.ID)).ToList();
            }
            else
            {
                return _unitOfWork.GetRepository<AccountingObject>().Filter(w => elementsIds.Contains(w.ID)).ToList();
            }
        }

        /// <summary>
        /// Создает временную папку для хранения шаблонов, формирует пути к файлам.
        /// </summary>
        /// <param name="societyIds">ID Балансодержателей.</param>
        /// <returns>Коллекция (ID ОГ, (Файл для заполнения, файл шаблона)).</returns>
        private Dictionary<int, Dictionary<string, string>> GetTemplatesBySociety(int[] societyIds, Type tt)
        {
            Dictionary<int, Dictionary<string, string>> result = new Dictionary<int, Dictionary<string, string>>();
            
            var societyList = _unitOfWork.GetRepository<Society>().Filter(f => societyIds.Contains(f.ID)).ToList();
            if (societyList.Count == 0)
                throw new Exception("ОГ не найдено.");

            foreach (var soc in societyList)
            {
                Dictionary<string, string> files = new Dictionary<string, string>();
                string[] templateParam = GetTemplateFilePath(soc.ID, tt);
                if (templateParam.Length < 2)
                    throw new Exception($"Ошибка при получении шаблона выгрузки.{Environment.NewLine}Не указан путь до шаблона или название шаблона");
                string ext = templateParam[1].Split('.').Length > 1 ? templateParam[1] : $"{templateParam[1]}.xlsx";
                string fileName = $"{soc.IDEUP}_{DateTime.Today.Date:ddMMyyyy}_{ext}";
                string newFilePath = $@"{Directory.GetParent(Path.GetDirectoryName(templateParam[0]))}\_TMP_EXPORT\src\";
                if (!Directory.Exists(newFilePath))
                    Directory.CreateDirectory(newFilePath);

                if (!files.ContainsKey($@"{newFilePath}{fileName}"))
                    files.Add($@"{newFilePath}{fileName}", templateParam[0]);

                if (!result.ContainsKey(soc.ID))
                    result.Add(soc.ID, files);
            }
            
            return result;
        }

        /// <summary>
        /// Применение стилей к шаблону.
        /// </summary>
        /// <param name="worksheet"></param>
        private static void ApplyStyles(ExcelWorksheet worksheet)
        {
            var columnHeaderStyle = worksheet.Workbook.Styles.CreateNamedStyle("columnHeaderStyle");

            columnHeaderStyle.Style.Font.Bold = true;
            
            //TODO: Добавить стили.
            worksheet.Cells["A1:B6"].StyleName = "columnHeaderStyle";
        }

        /// <summary>
        /// Получение пути файла шаблона ОГ.
        /// </summary>
        /// <param name="societyId">ИД ОГ.</param>
        /// <param name="operationType">Тип операции.</param>
        /// <returns>Путь до файла.</returns>
        private string[] GetTemplateFilePath(int societyId
            , Type tt
            , ExportImportSettingType operationType = ExportImportSettingType.Export
            )
        {
            var fileCardRepo = _unitOfWork.GetRepository<FileCardOne>();

            var settings = _unitOfWork.GetRepository<ExportImportSettings>()
                .Filter(f =>
                f.OperationType == operationType 
                && f.SocietyID == societyId 
                && f.Mnemonic == tt.Name
                && !f.Hidden
                && !f.IsHistory)
                .ToList();

            if (settings == null || settings.Count == 0)
                throw new Exception($"Не найдено настроек для {operationType.GetTitle()}.");

            var id = settings.SingleOrDefault().FileCardID.GetValueOrDefault(0);
            FileCardOne fileCard = fileCardRepo.Find(f => f.ID == id);

            if (fileCard == null)
                throw new Exception("Не удалось получить данные о шаблоне.");

            FileData fileData = fileCard.FileData;
            return new string[] { _fileSystemService.GetFilePath(fileData.FileID), fileCard.Name };
    }

       
    }
}
