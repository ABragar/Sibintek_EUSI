using Base.DAL;
using Base.Extensions;
using Base.Service;
using CorpProp.Entities.Base;
using CorpProp.Entities.Export;
using CorpProp.Entities.NSI;
using CorpProp.Entities.Subject;
using CorpProp.Helpers;
using CorpProp.Helpers.Export;
using CorpProp.Services.Document;
using EUSI.Entities.Accounting;
using ExcelDataReader;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUSI.Export
{
    /// <summary>
    /// Представляет класс для экспорта данных по движению в файлы кастомизированных шаблонов.
    /// </summary>
    public class AccMovingsExport
    {
        private IFileDBService _fileService;
        private IFileSystemService _fileSystemService;
        private ICollection<ExportMovingConsolidation> _consolidations;
        private IUnitOfWork _uow;
        private Guid _oid;

        /// <summary>
        /// Инициализирует новый экезмпляр класса AccMovingsExport.
        /// </summary>
        public AccMovingsExport(
             IFileDBService fileService
            , IFileSystemService fileSystemService
            , IUnitOfWork uow
            , ICollection<ExportMovingConsolidation> be
            , DateTime? startDate
            , DateTime? endDate)
        {
            _uow = uow;
            _consolidations = be;
            _fileSystemService = fileSystemService;
            _oid = System.Guid.NewGuid();
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Получает файловый сервис (БД).
        /// </summary>
        public IFileDBService FileService { get { return _fileService; } }

        /// <summary>
        /// Получает файловый сервис ПХД.
        /// </summary>
        public IFileSystemService FileSystemService { get { return _fileSystemService; } }

        /// <summary>
        /// Получает сессию выгрузки.
        /// </summary>
        public IUnitOfWork Uow { get { return _uow; } }

        /// <summary>
        /// Получает БЕ.
        /// </summary>
        public ICollection<ExportMovingConsolidation> Consolidations { get { return _consolidations; } }

        /// <summary>
        /// Получает или задает дату начала периода выгрузки.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Получает или задает дату окончания периода выгрузки.
        /// </summary>
        public DateTime? EndDate { get; set; }




        public string Export()
        {
            List<string> result = new List<string>();
            foreach (var item in Consolidations)
            {
                var codeBE = item.Object?.Code;
                var groups = Uow.GetRepository<AccountingMovingMSFO>()
                    .Filter(f =>
                    !f.Hidden
                    && !f.IsHistory
                    && f.Consolidation != null
                    && f.Consolidation.Code == codeBE
                    && f.Date >= StartDate
                    && f.Date <= EndDate
                    && f.TypeMovingMSFO != null
                    )
                     .Include(inc => inc.AccountingObject)
                      .Include(inc => inc.AccountingObject.Estate)
                      .Include(inc => inc.Angle)
                      .Include(inc => inc.Consolidation)
                      .Include(inc => inc.LoadType)
                      .Include(inc => inc.MovingType)

                      .Include(inc => inc.DepGroupDebit)
                      .Include(inc => inc.DepGroupCredit)
                      .Include(inc => inc.BusinessAreaDebit)
                      .Include(inc => inc.BusinessAreaCredit)
                      .Include(inc => inc.OKOFDebit)
                      .Include(inc => inc.OKOFCredit)
                    .GroupBy(gr => gr.TypeMovingMSFO)
                    .ToList()
                    ;

                foreach (IGrouping<TypeMovingMSFO, AccountingMovingMSFO> g in groups)
                {
                    var template = GetTemplate(g.Key);
                    //TODO: обработать отсутствие шаблона.
                    if (template == null)
                        throw new Exception($"Не найден шаблон выгрузки. Добавьте шаблон <{g.Key.ToString()}>.");

                    var filePath = GenerateNewFilePath(codeBE, template);
                    Dictionary<int, string> colsMap = template.GetColumnsMap();

                    using (var pack = new ExcelPackage(new FileInfo(filePath)))
                    {
                        DataTable dt = ToDataTable(g.ToList<AccountingMovingMSFO>(), colsMap);                        
                        ExcelWorksheet ws = pack.Workbook.Worksheets[1];
                        ws.Cells[template.StartRow.Value, template.StartColumn.Value].LoadFromDataTable(dt, false);
                        pack.Save();
                        result.Add(filePath);
                    }
                }

                if (result.Count == 0)
                    return "";

            }

            return ExcelExportHelper.PrepareArchive(result);
        }


        /// <summary>
        /// Наполняет DataTable.
        /// </summary>      
        /// <param name="list"></param>
        /// <param name="colsMap"></param>
        /// <returns></returns>
        /// <remarks>
        /// Временное решения для получения значения номера ЕУСИ и ОИ и ОБУ.
        /// По умолчанию использовать универсальное предобразования списка в DataTable
        /// <see cref="CorpProp.Helpers.Export.ExcelExportHelper"/>
        /// </remarks>
        private DataTable ToDataTable(IList<AccountingMovingMSFO> list, Dictionary<int, string> colsMap)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(AccountingMovingMSFO));

            DataTable table = new DataTable();
            foreach (var col in colsMap)
            {
                PropertyDescriptor prop = props.Find(col.Value, false);
                if (prop != null || col.Value == "RowNumber")
                    table.Columns.Add(((prop == null) ? col.Value : prop.Name), typeof(string));

            }
            object[] values = new object[colsMap.Count];
            for (int j = 0; j < list.Count; j++)               
            {
                var item = list[j];
                //TODO: навигационные св-ва приходят пустыми 
                // EUSINumber не вычислено
                for (int i = 0; i < values.Length; i++)
                {
                    if (colsMap.ContainsKey(i))
                    {
                        PropertyDescriptor prop = props.Find(colsMap[i], false);
                        if (prop == null && colsMap[i] == "RowNumber")
                        {
                            values[i] = j + 1;
                            continue;
                        }
                        if (prop != null)
                        {                            
                            values[i] = (prop.GetValue(item) != null) ?
                            CorpProp.Helpers.Export.ExcelExportHelper.GetValue(prop.GetValue(item)?.GetType(),
                                       prop.GetValue(item)) : "";

                            if (prop.Name == "EUSINumber")
                            {
                                var id = item.AccountingObject?.EstateID;
                                if (id != null)
                                {
                                    var est = Uow.GetRepository<CorpProp.Entities.Estate.Estate>()
                                    .Find(f => f.ID == id);
                                    if (est != null)
                                        values[i] = est.EUSINumber;
                                }
                                else
                                {
                                    values[i] = item.EUSI;
                                }
                            }
                        }
                       
                    }
                    else
                        values[i] = null;
                }
                table.Rows.Add(values);
            }
            return table;
        }


        public ExportTemplate GetTemplate(TypeMovingMSFO tt)
        {
            var templateCode = tt.ToString();

            return
                Uow.GetRepository<ExportTemplate>()
                .Filter(f => 
                !f.Hidden 
                && !f.IsHistory 
                && f.Mnemonic == nameof(AccountingMovingMSFO)
                && f.Code == templateCode
                )
                .Include(s => s.File)
                .FirstOrDefault();
        }


        public string GenerateNewFilePath(string codeBE, ExportTemplate tmpl)
        {
            var root = FileSystemService.FilesDirectory;
            var newPath = root + "\\Temp\\"+this._oid;

            if (!System.IO.Directory.Exists(newPath))
                System.IO.Directory.CreateDirectory(newPath);

            var newFileName = $"{newPath}\\{codeBE}_{tmpl.FileName}"; 

            if (System.IO.File.Exists(newFileName))
                System.IO.File.Delete(newFileName);

            var file = tmpl.File;
            if (file == null && tmpl.FileID != null)            
                file = Uow.GetRepository<CorpProp.Entities.Document.FileDB>()
                    .Find(f => f.ID == tmpl.FileID);


            File.WriteAllBytes(newFileName, file.Content);
            return newFileName;
        }

    }
}
